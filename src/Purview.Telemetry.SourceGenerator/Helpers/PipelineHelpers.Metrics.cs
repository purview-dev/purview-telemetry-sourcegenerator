using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Helpers;

partial class PipelineHelpers
{
	public static bool HasMeterTargetAttribute(SyntaxNode _, CancellationToken __) => true;

	public static MeterTarget? BuildMeterTransform(GeneratorAttributeSyntaxContext context, IGenerationLogger? logger, CancellationToken token)
	{
		token.ThrowIfCancellationRequested();

		if (context.TargetNode is not InterfaceDeclarationSyntax interfaceDeclaration)
		{
			logger?.Error($"Could not find interface syntax from the target node '{context.TargetNode.Flatten()}'.");
			return null;
		}

		if (context.TargetSymbol is not INamedTypeSymbol interfaceSymbol)
		{
			logger?.Error($"Could not find interface symbol '{interfaceDeclaration.Flatten()}'.");
			return null;
		}

		var semanticModel = context.SemanticModel;
		var meterAttribute = SharedHelpers.GetMeterAttribute(context.Attributes[0], semanticModel, logger, token);
		if (meterAttribute == null)
		{
			logger?.Error($"Could not find {Constants.Metrics.MeterAttribute} when one was expected '{interfaceDeclaration.Flatten()}'.");
			return null;
		}

		var telemetryGeneration = SharedHelpers.GetTelemetryGenerationAttribute(interfaceSymbol, semanticModel, logger, token);
		var className = telemetryGeneration.ClassName.IsSet
			? telemetryGeneration.ClassName.Value!
			: GenerateClassName(interfaceSymbol.Name);

		var generationType = SharedHelpers.GetGenerationTypes(interfaceSymbol, token);
		var meterGenerationAttribute = SharedHelpers.GetMeterGenerationAttribute(semanticModel, logger, token);
		var fullNamespace = Utilities.GetFullNamespace(interfaceDeclaration, true);
		var instrumentMethods = BuildInstrumentationMethods(
			generationType,
			meterAttribute,
			meterGenerationAttribute,
			semanticModel,
			interfaceSymbol,
			logger,
			token
		);

		var meterName = meterAttribute.Name.Value;
		if (string.IsNullOrWhiteSpace(meterName))
		{
			meterName = interfaceSymbol.Name;
			if (meterName[0] == 'I')
				meterName = meterName.Substring(1);
		}

		return new(
			TelemetryGeneration: telemetryGeneration,
			GenerationType: generationType,

			ClassNameToGenerate: className,
			ClassNamespace: Utilities.GetNamespace(interfaceDeclaration),

			ParentClasses: Utilities.GetParentClasses(interfaceDeclaration),
			FullNamespace: fullNamespace,
			FullyQualifiedName: fullNamespace + className,

			InterfaceName: interfaceSymbol.Name,
			FullyQualifiedInterfaceName: fullNamespace + interfaceSymbol.Name,

			MeterName: meterName,

			MeterGeneration: meterGenerationAttribute,

			InstrumentationMethods: instrumentMethods,
			DuplicateMethods: BuildDuplicateMethods(interfaceSymbol)
		);
	}

	static ImmutableArray<InstrumentTarget> BuildInstrumentationMethods(
		GenerationType generationType,
		MeterAttributeRecord meterAttribute,
		MeterGenerationAttributeRecord? meterGenerationAttribute,
		SemanticModel semanticModel,
		INamedTypeSymbol interfaceSymbol,
		IGenerationLogger? logger,
		CancellationToken token)
	{
		token.ThrowIfCancellationRequested();

		var lowercaseInstrumentName = meterAttribute.LowercaseInstrumentName.IsSet
			? meterAttribute.LowercaseInstrumentName.Value!.Value
			: meterGenerationAttribute?.LowercaseInstrumentName?.IsSet == true
				? meterGenerationAttribute.LowercaseInstrumentName.Value!.Value
				: true;

		var prefix = GeneratePrefix(meterGenerationAttribute, meterAttribute, token);
		var lowercaseTagKeys = meterAttribute.LowercaseTagKeys!.Value!.Value;

		List<InstrumentTarget> methodTargets = [];
		foreach (var method in interfaceSymbol.GetMembers().OfType<IMethodSymbol>())
		{
			token.ThrowIfCancellationRequested();

			if (Utilities.ContainsAttribute(method, Constants.Shared.ExcludeAttribute, token))
			{
				logger?.Debug($"Skipping {interfaceSymbol.Name}.{method.Name}, explicitly excluded.");
				continue;
			}

			InstrumentAttributeRecord? instrumentAttribute = null;
			var isInferred = !SharedHelpers.TryGetInstrumentAttribute(method, token, out var attributeData);
			if (!isInferred)
				instrumentAttribute = SharedHelpers.GetInstrumentAttribute(attributeData!, semanticModel, logger, token);

			logger?.Debug($"Found possible instrument method {interfaceSymbol.Name}.{method.Name}.");

			var validAutoCounter = instrumentAttribute?.InstrumentType is InstrumentTypes.Counter && instrumentAttribute.IsAutoIncrement;

			var parameters = GetInstrumentParameters(method, lowercaseTagKeys, validAutoCounter, semanticModel, logger, token);
			var measurementParameters = parameters.Where(m => m.ParamDestination == InstrumentParameterDestination.Measurement).ToImmutableArray();
			var tagParameters = parameters.Where(m => m.ParamDestination == InstrumentParameterDestination.Tag).ToImmutableArray();
			var measurementParameter = measurementParameters.FirstOrDefault();

			var returnType = method.ReturnsVoid
				? Constants.System.VoidKeyword
				: Utilities.GetFullyQualifiedOrSystemName(method.ReturnType);
			var fieldName = $"_{Utilities.LowercaseFirstChar(method.Name)}Instrument";
			var instrumentName = instrumentAttribute?.Name?.Value;
			if (string.IsNullOrWhiteSpace(instrumentName))
				instrumentName = method.Name;

			if (lowercaseInstrumentName)
			{
#pragma warning disable CA1308 // Normalize strings to uppercase
				instrumentName = instrumentName!.ToLowerInvariant();
				prefix = prefix?.ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
			}

			List<TelemetryDiagnosticDescriptor> errorDiagnostics = [];

			var returnsBool = Utilities.IsBoolean(method.ReturnType);
			var targetGenerationState = Utilities.IsValidGenerationTarget(method, generationType, GenerationType.Metrics);
			if (!targetGenerationState.IsValid)
			{
				if (targetGenerationState.RaiseMultiGenerationTargetsNotSupported)
				{
					logger?.Debug($"Identified {interfaceSymbol.Name}.{method.Name} as problematic as it has another target types.");
					errorDiagnostics.Add(TelemetryDiagnostics.General.MultiGenerationTargetsNotSupported);
				}
				else if (targetGenerationState.RaiseInferenceNotSupportedWithMultiTargeting)
				{
					logger?.Debug($"Identified {interfaceSymbol.Name}.{method.Name} as problematic as it is inferred.");
					errorDiagnostics.Add(TelemetryDiagnostics.General.InferenceNotSupportedWithMultiTargeting);
				}
			}
			else
			{
				if (instrumentAttribute == null)
				{
					logger?.Warning("Missing instrument attribute.");
					errorDiagnostics.Add(TelemetryDiagnostics.Metrics.NoInstrumentDefined);
				}
				else if (!validAutoCounter && measurementParameter == null)
					errorDiagnostics.Add(TelemetryDiagnostics.Metrics.NoMeasurementValueDefined);
				else
				{
					if (validAutoCounter)
					{
						if (measurementParameters.Length > 0)
							errorDiagnostics.Add(TelemetryDiagnostics.Metrics.AutoIncrementCountAndMeasurementParam);
					}
					else
					{
						// Validate the parameters and type.
						if (instrumentAttribute.IsObservable)
						{
							if (!measurementParameter!.IsFunc)
								errorDiagnostics.Add(TelemetryDiagnostics.Metrics.ObservableRequiredFunc);
						}
						else
						{
							if (measurementParameters.Length != 1)
								errorDiagnostics.Add(TelemetryDiagnostics.Metrics.MoreThanOneMeasurementValueDefined);
						}
					}
				}

				if (instrumentAttribute != null)
				{
					if (!method.ReturnsVoid && !returnsBool)
						errorDiagnostics.Add(TelemetryDiagnostics.Metrics.DoesNotReturnVoid);
				}
			}

			var instrumentMeasurementType = measurementParameter?.InstrumentType ?? Constants.System.IntKeyword;

			if (measurementParameter != null && !measurementParameter.IsValidInstrumentType)
				errorDiagnostics.Add(TelemetryDiagnostics.Metrics.InvalidMeasurementType);

			methodTargets.Add(new(
				MethodName: method.Name,
				ReturnType: returnType,
				ReturnsBool: returnsBool,
				IsNullableReturn: method.ReturnType.NullableAnnotation == NullableAnnotation.Annotated,
				FieldName: fieldName,
				IsObservable: instrumentAttribute?.IsObservable == true,

				MetricName: prefix + instrumentName!,
				InstrumentMeasurementType: instrumentMeasurementType,

				MethodLocation: method.Locations.FirstOrDefault(),

				InstrumentAttribute: instrumentAttribute,

				Parameters: parameters,
				Tags: tagParameters,
				MeasurementParameter: measurementParameter,

				ErrorDiagnostics: [.. errorDiagnostics],

				TargetGenerationState: targetGenerationState
			));
		}

		return [.. methodTargets];
	}

	static ImmutableArray<InstrumentParameterTarget> GetInstrumentParameters(
		IMethodSymbol method,
		bool lowercaseTagKeys,
		bool isAutoCounter,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token)
	{
		List<InstrumentParameterTarget> parameterTargets = [];
		foreach (var parameter in method.Parameters)
		{
			token.ThrowIfCancellationRequested();

			TagOrBaggageAttributeRecord? tagAttribute = null;
			var destination = InstrumentParameterDestination.Unknown;
			if (Utilities.TryContainsAttribute(parameter, Constants.Shared.TagAttribute, token, out var attribute))
			{
				logger?.Debug($"Found explicit tag: {parameter.Name}.");
				destination = InstrumentParameterDestination.Tag;

				tagAttribute = SharedHelpers.GetTagOrBaggageAttribute(attribute!, semanticModel, logger, token);
			}
			else if (Utilities.ContainsAttribute(parameter, Constants.Metrics.InstrumentMeasurementAttribute, token))
			{
				logger?.Debug($"Found explicit instrument measurement: {parameter.Name}.");
				destination = InstrumentParameterDestination.Measurement;
			}

			var isFuncType = false;
			var isIEnumerableType = false;
			var isMeasurementType = false;
			var isValidInstrumentType = false;

			string? instrumentType = null;

			if (destination != InstrumentParameterDestination.Tag)
			{
				if (parameter.Type is INamedTypeSymbol parameterType)
				{
					isFuncType = parameterType.ConstructedFrom.ToString() == Constants.System.Func.MakeGeneric("TResult");
					if (isFuncType)
					{
						// For observable instruments.
						if (parameterType.TypeArguments[0] is INamedTypeSymbol typeArg)
						{
							isIEnumerableType = Constants.System.IEnumerable.Equals(typeArg.ConstructedFrom);
							if (isIEnumerableType)
							{
								if (parameterType.TypeArguments[0] is INamedTypeSymbol enumerableType)
								{
									if (Constants.Metrics.SystemDiagnostics.Measurement.Equals(enumerableType.TypeArguments[0]))
									{
										if (enumerableType.TypeArguments[0] is INamedTypeSymbol measurementType)
										{
											isMeasurementType = true;
											isValidInstrumentType = SharedHelpers.IsValidMeasurementValueType(measurementType.TypeArguments[0]);
											if (isValidInstrumentType)
											{
												instrumentType = Utilities.GetFullyQualifiedOrSystemName(measurementType.TypeArguments[0]);
												destination = InstrumentParameterDestination.Measurement;

												logger?.Debug($"Found valid instrument type: Func -> IEnumerable -> Measurement -> {instrumentType}");
											}
										}
									}
								}
							}
							else if (Constants.Metrics.SystemDiagnostics.Measurement.Equals(typeArg.ConstructedFrom))
							{
								isMeasurementType = true;
								isValidInstrumentType = SharedHelpers.IsValidMeasurementValueType(typeArg.TypeArguments[0]);
								if (isValidInstrumentType)
								{
									instrumentType = Utilities.GetFullyQualifiedOrSystemName(typeArg.TypeArguments[0]);
									destination = InstrumentParameterDestination.Measurement;

									logger?.Debug($"Found valid instrument type: Func -> Measurement -> {instrumentType}");
								}
							}
							else if (SharedHelpers.IsValidMeasurementValueType(typeArg))
							{
								isValidInstrumentType = true;

								instrumentType = Utilities.GetFullyQualifiedOrSystemName(typeArg);
								destination = InstrumentParameterDestination.Measurement;

								logger?.Debug($"Found valid instrument type: Func -> {instrumentType}");
							}
						}
						else
						{
							isValidInstrumentType = SharedHelpers.IsValidMeasurementValueType(parameterType.TypeArguments[0]);
							if (isValidInstrumentType)
							{
								instrumentType = Utilities.GetFullyQualifiedOrSystemName(parameterType.TypeArguments[0]);
								destination = InstrumentParameterDestination.Measurement;

								logger?.Debug($"Found valid instrument type: Func -> {instrumentType}");
							}
						}
					}
					else
					{
						// For non-observable instruments.
						isValidInstrumentType = SharedHelpers.IsValidMeasurementValueType(parameterType);
						if (isValidInstrumentType && !isAutoCounter)
						{
							instrumentType = Utilities.GetFullyQualifiedOrSystemName(parameterType);
							destination = InstrumentParameterDestination.Measurement;

							logger?.Debug($"Found valid instrument type: {instrumentType}");
						}
					}
				}
			}

			if (destination == InstrumentParameterDestination.Unknown)
			{
				logger?.Debug($"Unable to match parameter {parameter.Name}, inferring tag.");
				destination = InstrumentParameterDestination.Tag;
			}

			var parameterName = parameter.Name;
			var generatedName = GenerateParameterName(tagAttribute?.Name.Value ?? parameterName, null, lowercaseTagKeys);

			parameterTargets.Add(new(
				ParameterName: parameterName,
				ParameterType: Utilities.GetFullyQualifiedOrSystemName(parameter.Type),

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

	static string? GeneratePrefix(
		MeterGenerationAttributeRecord? meterGenerationAttribute,
		MeterAttributeRecord meterAttribute,
		CancellationToken token)
	{
		token.ThrowIfCancellationRequested();

		string? prefix = null;
		var separator = meterGenerationAttribute?
			.InstrumentSeparator.Or(Constants.Metrics.InstrumentSeparatorDefault);

		if (meterAttribute.IncludeAssemblyInstrumentPrefix.Value == true)
		{
			if (meterGenerationAttribute?.InstrumentPrefix.IsSet == true
				&& !string.IsNullOrWhiteSpace(meterGenerationAttribute?.InstrumentPrefix.Value))
				prefix = meterGenerationAttribute!.InstrumentPrefix.Value! + separator;
		}

		if (!string.IsNullOrWhiteSpace(meterAttribute.InstrumentPrefix.Value))
			prefix += meterAttribute.InstrumentPrefix.Value! + separator;

		return prefix;
	}
}
