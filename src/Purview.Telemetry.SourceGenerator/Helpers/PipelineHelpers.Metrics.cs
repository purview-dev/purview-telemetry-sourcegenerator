using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Helpers;

partial class PipelineHelpers {
	static public bool HasMeterTargetAttribute(SyntaxNode _, CancellationToken __) => true;

	static public MeterGenerationTarget? BuildMeterTransform(GeneratorAttributeSyntaxContext context, IGenerationLogger? logger, CancellationToken token) {
		token.ThrowIfCancellationRequested();

		if (context.TargetNode is not InterfaceDeclarationSyntax interfaceDeclaration) {
			logger?.Error($"Could not find interface syntax from the target node '{context.TargetNode.Flatten()}'.");
			return null;
		}

		if (context.TargetSymbol is not INamedTypeSymbol interfaceSymbol) {
			logger?.Error($"Could not find interface symbol '{interfaceDeclaration.Flatten()}'.");
			return null;
		}

		var semanticModel = context.SemanticModel;
		var meterTarget = SharedHelpers.GetMeterTargetAttribute(context.Attributes[0], semanticModel, logger, token);
		if (meterTarget == null) {
			logger?.Error($"Could not find {Constants.Metrics.MeterTargetAttribute} when one was expected '{interfaceDeclaration.Flatten()}'.");
			return null;
		}

		var className = meterTarget.ClassName.IsSet
			? meterTarget.ClassName.Value!
			: GenerateClassName(interfaceSymbol.Name);

		var meterAssembly = SharedHelpers.GetMeterAssemblyAttribute(semanticModel, logger, token);
		var fullNamespace = Utilities.GetFullNamespace(interfaceDeclaration, true);
		var instrumentMethods = BuildInstrumentationMethods(
			meterTarget,
			meterAssembly,
			semanticModel,
			interfaceSymbol,
			logger,
			token);

		var telemetryGeneration = SharedHelpers.GetTelemetryGenerationAttribute(interfaceSymbol, semanticModel, logger, token);

		return new(
			TelemetryGeneration: telemetryGeneration,

			ClassNameToGenerate: className,
			ClassNamespace: Utilities.GetNamespace(interfaceDeclaration),

			ParentClasses: Utilities.GetParentClasses(interfaceDeclaration),
			FullNamespace: fullNamespace,
			FullyQualifiedName: fullNamespace + className,

			InterfaceName: interfaceSymbol.Name,
			FullyQualifiedInterfaceName: fullNamespace + interfaceSymbol.Name,

			MeterName: meterTarget.Name?.Value,

			MeterAssembly: meterAssembly,

			InstrumentationMethods: instrumentMethods
		);
	}

	static ImmutableArray<InstrumentMethodGenerationTarget> BuildInstrumentationMethods(
		MeterTargetAttributeRecord meterTarget,
		MeterAssemblyAttributeRecord? meterAssembly,
		SemanticModel semanticModel,
		INamedTypeSymbol interfaceSymbol,
		IGenerationLogger? logger,
		CancellationToken token) {

		token.ThrowIfCancellationRequested();

		var prefix = GeneratePrefix(meterAssembly, meterTarget, token);
		var lowercaseTagKeys = meterTarget.LowercaseTagKeys!.Value!.Value;

		List<InstrumentMethodGenerationTarget> methodTargets = [];
		foreach (var method in interfaceSymbol.GetMembers().OfType<IMethodSymbol>()) {
			token.ThrowIfCancellationRequested();

			if (Utilities.ContainsAttribute(method, Constants.Metrics.MeterExcludeAttribute, token)) {
				logger?.Debug($"Skipping {interfaceSymbol.Name}.{method.Name}, explicitly excluded.");
				continue;
			}

			InstrumentAttributeRecord? instrumentAttribute = null;
			if (TryGetInstrumentAttribute(method, token, out var attributeData)) {
				instrumentAttribute = SharedHelpers.GetInstrumentAttribute(attributeData!, semanticModel, logger, token);
			}
			else {
				logger?.Error("Missing instrument attribute.");
			}

			logger?.Debug($"Found instrument method {interfaceSymbol.Name}.{method.Name}.");

			var parameters = GetInstrumentParameters(method, prefix, lowercaseTagKeys, semanticModel, logger, token);
			var measurementParameters = parameters.Where(m => m.ParamDestination == InstrumentParameterDestination.Measurement).ToImmutableArray();
			var tagParameters = parameters.Where(m => m.ParamDestination == InstrumentParameterDestination.Tag).ToImmutableArray();
			var measurementParameter = measurementParameters.FirstOrDefault();

			var returnType = method.ReturnsVoid
				? Constants.System.VoidKeyword
				: Utilities.GetFullyQualifiedName(method.ReturnType);
			var fieldName = $"_{Utilities.LowercaseFirstChar(method.Name)}Instrument";
			var metricName = instrumentAttribute?.Name?.Value;
			if (string.IsNullOrWhiteSpace(metricName)) {
				metricName = method.Name;
			}

			var validAutoCounter = instrumentAttribute?.InstrumentType is InstrumentTypes.Counter && instrumentAttribute.IsAutoIncrement;

			List<TelemetryDiagnosticDescriptor> errorDiagnostics = [];
			if (instrumentAttribute == null) {
				errorDiagnostics.Add(TelemetryDiagnostics.Metrics.NoInstrumentDefined);
			}
			else if (!validAutoCounter && measurementParameter == null) {
				errorDiagnostics.Add(TelemetryDiagnostics.Metrics.NoMeasurementValueDefined);
			}
			else {
				if (validAutoCounter) {
					if (measurementParameters.Length > 0) {
						errorDiagnostics.Add(TelemetryDiagnostics.Metrics.AutoIncrementCountAndMeasurementParam);
					}
				}
				else {
					// Validate the parameters and type.
					if (instrumentAttribute.IsObservable) {
						if (!measurementParameter!.IsFunc) {
							errorDiagnostics.Add(TelemetryDiagnostics.Metrics.ObservableRequiredFunc);
						}
					}
					else {
						if (measurementParameters.Length != 1) {
							errorDiagnostics.Add(TelemetryDiagnostics.Metrics.MoreThanOneMeasurementValueDefined);
						}
					}
				}
			}

			var instrumentMeasurementType = measurementParameter?.InstrumentType ?? Constants.System.Int32;

			if (measurementParameter != null && !measurementParameter.IsValidInstrumentType) {
				errorDiagnostics.Add(TelemetryDiagnostics.Metrics.InvalidMeasurementType);
			}

			var returnsBool = Utilities.IsBoolean(method.ReturnType);

			if (!method.ReturnsVoid) {
				if (!(instrumentAttribute?.IsObservable == true && returnsBool)) {
					errorDiagnostics.Add(TelemetryDiagnostics.Metrics.DoesNotReturnVoid);
				}
			}

			methodTargets.Add(new(
				MethodName: method.Name,
				ReturnType: returnType,
				ReturnsBool: returnsBool,
				IsNullableReturn: method.ReturnType.NullableAnnotation == NullableAnnotation.Annotated,
				FieldName: fieldName,
				IsObservable: instrumentAttribute?.IsObservable == true,

				MetricName: metricName!,
				InstrumentMeasurementType: instrumentMeasurementType,

				MethodLocation: method.Locations.FirstOrDefault(),

				InstrumentAttribute: instrumentAttribute,

				Parameters: parameters,
				Tags: tagParameters,
				MeasurementParameter: measurementParameter,

				ErrorDiagnostics: [.. errorDiagnostics]
			));
		}

		return [.. methodTargets];
	}

	static ImmutableArray<InstrumentMethodParameterTarget> GetInstrumentParameters(
		IMethodSymbol method,
		string? prefix,
		bool lowercaseTagKeys,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {

		List<InstrumentMethodParameterTarget> parameterTargets = [];
		foreach (var parameter in method.Parameters) {
			token.ThrowIfCancellationRequested();

			TagOrBaggageAttributeRecord? tagAttribute = null;
			var destination = InstrumentParameterDestination.Unknown;
			if (Utilities.TryContainsAttribute(parameter, Constants.Shared.TagAttribute, token, out var attribute)) {
				logger?.Debug($"Found explicit tag: {parameter.Name}.");
				destination = InstrumentParameterDestination.Tag;

				tagAttribute = SharedHelpers.GetTagOrBaggageAttribute(attribute!, semanticModel, logger, token);
			}
			else if (Utilities.ContainsAttribute(parameter, Constants.Metrics.InstrumentMeasurementAttribute, token)) {
				logger?.Debug($"Found explicit instrument measurement: {parameter.Name}.");
				destination = InstrumentParameterDestination.Measurement;
			}

			var isFuncType = false;
			var isIEnumerableType = false;
			var isMeasurementType = false;
			var isValidInstrumentType = false;

			string? instrumentType = null;

			if (destination != InstrumentParameterDestination.Tag) {
				if (parameter.Type is INamedTypeSymbol parameterType) {
					isFuncType = parameterType.ConstructedFrom.ToString() == Constants.System.Func.MakeGeneric("TResult");
					if (isFuncType) {
						// For observable instruments.
						if (parameterType.TypeArguments[0] is INamedTypeSymbol typeArg) {
							isIEnumerableType = Constants.System.IEnumerable.Equals(typeArg.ConstructedFrom);
							if (isIEnumerableType) {
								if (parameterType.TypeArguments[0] is INamedTypeSymbol enumerableType) {
									if (Constants.Metrics.SystemDiagnostics.Measurement.Equals(enumerableType.TypeArguments[0])) {
										if (enumerableType.TypeArguments[0] is INamedTypeSymbol measurementType) {
											isMeasurementType = true;
											isValidInstrumentType = SharedHelpers.IsValidMeasurementValueType(measurementType.TypeArguments[0]);
											if (isValidInstrumentType) {
												instrumentType = Utilities.GetFullyQualifiedName(measurementType.TypeArguments[0]);
												destination = InstrumentParameterDestination.Measurement;

												logger?.Debug($"Found valid instrument type: Func -> IEnumerable -> Measurement -> {instrumentType}");
											}
										}
									}
								}
							}
							else if (Constants.Metrics.SystemDiagnostics.Measurement.Equals(typeArg.ConstructedFrom)) {
								isMeasurementType = true;
								isValidInstrumentType = SharedHelpers.IsValidMeasurementValueType(typeArg.TypeArguments[0]);
								if (isValidInstrumentType) {
									instrumentType = Utilities.GetFullyQualifiedName(typeArg.TypeArguments[0]);
									destination = InstrumentParameterDestination.Measurement;

									logger?.Debug($"Found valid instrument type: Func -> Measurement -> {instrumentType}");
								}
							}
							else if (SharedHelpers.IsValidMeasurementValueType(typeArg)) {
								isValidInstrumentType = true;

								instrumentType = Utilities.GetFullyQualifiedName(typeArg);
								destination = InstrumentParameterDestination.Measurement;

								logger?.Debug($"Found valid instrument type: Func -> {instrumentType}");
							}
						}
						else {
							isValidInstrumentType = SharedHelpers.IsValidMeasurementValueType(parameterType.TypeArguments[0]);
							if (isValidInstrumentType) {
								instrumentType = Utilities.GetFullyQualifiedName(parameterType.TypeArguments[0]);
								destination = InstrumentParameterDestination.Measurement;

								logger?.Debug($"Found valid instrument type: Func -> {instrumentType}");
							}
						}
					}
					else {
						// For non-observable instruments.
						isValidInstrumentType = SharedHelpers.IsValidMeasurementValueType(parameterType);
						if (isValidInstrumentType) {
							instrumentType = Utilities.GetFullyQualifiedName(parameterType);
							destination = InstrumentParameterDestination.Measurement;

							logger?.Debug($"Found valid instrument type: {instrumentType}");
						}
					}
				}
			}

			if (destination == InstrumentParameterDestination.Unknown) {
				logger?.Debug($"Unable to match parameter {parameter.Name}, inferring tag.");
				destination = InstrumentParameterDestination.Tag;
			}

			var parameterName = parameter.Name;
			var generatedName = GenerateParameterName(tagAttribute?.Name?.Value ?? parameterName, prefix, lowercaseTagKeys);

			parameterTargets.Add(new(
				ParameterName: parameterName,
				ParameterType: parameter.Type.ToDisplayString(),

				IsFunc: isFuncType,
				IsIEnumerable: isIEnumerableType,
				IsMeasurement: isMeasurementType,
				IsValidInstrumentType: isValidInstrumentType,

				InstrumentType: instrumentType,

				IsNullable: parameter.NullableAnnotation == NullableAnnotation.Annotated,
				GeneratedName: generatedName,
				ParamDestination: destination,
				SkipOnNullOrEmpty: GetSkipOnNullOrEmptyValue(tagAttribute),

				Location: parameter.Locations.FirstOrDefault()
			));
		}

		return [.. parameterTargets];
	}

	static string? GeneratePrefix(MeterAssemblyAttributeRecord? meterAssembly, MeterTargetAttributeRecord meterTarget, CancellationToken token) {
		token.ThrowIfCancellationRequested();

		string? prefix = null;
		var separator = meterAssembly?.InstrumentSeparator?.IsSet == true
			? meterAssembly.InstrumentSeparator.Value
			: ".";

		if (meterAssembly?.InstrumentPrefix?.IsSet == true) {
			prefix = meterAssembly.InstrumentPrefix.Value;
			if (meterAssembly.InstrumentSeparator != null) {
				prefix += separator;
			}
		}

		if (meterTarget.InstrumentPrefix.IsSet) {
			if (meterTarget.IncludeAssemblyInstrumentPrefix.Value == true) {
				prefix += meterTarget.InstrumentPrefix.Value + separator;
			}
			else {
				prefix = meterTarget.InstrumentPrefix.Value + separator;
			}
		}

		return prefix;
	}

	static bool TryGetInstrumentAttribute(IMethodSymbol method, CancellationToken token, out AttributeData? attributeData) {
		attributeData = null;
		foreach (var instrumentAttribute in Constants.Metrics.ValidInstrumentAttributes) {
			if (Utilities.TryContainsAttribute(method, instrumentAttribute, token, out attributeData)) {
				return true;
			}
		}

		return false;
	}
}
