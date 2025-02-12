using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Helpers;

partial class PipelineHelpers
{
	static readonly string[] SuffixesToRemove = [
		"Logs",
		"Logger",
		"Telemetry"
	];

	public static bool HasLoggerTargetAttribute(SyntaxNode _, CancellationToken __) => true;

	public static LoggerTarget? BuildLoggerTransform(GeneratorAttributeSyntaxContext context, IGenerationLogger? logger, CancellationToken token)
	{
		token.ThrowIfCancellationRequested();

		var iLoggerTypeSymbol = context.SemanticModel.Compilation.GetTypeByMetadataName(Constants.Logging.MicrosoftExtensions.ILogger.FullName);
		if (iLoggerTypeSymbol is null)
		{
			logger?.Diagnostic($"Requested a Logger target to be generated, but could not find the ILogger symbol referenced '{context.TargetNode.Flatten()}'.");
			return LoggerTarget.Failed(TelemetryDiagnostics.Logging.MSLoggingNotReferenced);
		}

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

		if (interfaceSymbol.Arity > 0)
		{
			logger?.Diagnostic($"Cannot generate a Logger target for a generic interface '{interfaceDeclaration.Flatten()}'.");
			return LoggerTarget.Failed(TelemetryDiagnostics.General.GenericInterfacesNotSupported);
		}

		var semanticModel = context.SemanticModel;
		var loggerAttribute = SharedHelpers.GetLoggerAttribute(context.TargetSymbol, semanticModel, logger, token);
		if (loggerAttribute == null)
		{
			logger?.Error($"Could not find {Constants.Logging.LoggerAttribute} when one was expected '{interfaceDeclaration.Flatten()}'.");
			return null;
		}

		var telemetryGeneration = SharedHelpers.GetTelemetryGenerationAttribute(interfaceSymbol, semanticModel, logger, token);
		var className = telemetryGeneration.ClassName.IsSet
			? telemetryGeneration.ClassName.Value!
			: GenerateClassName(interfaceSymbol.Name);

		var loggerGenerationAttribute = SharedHelpers.GetLoggerGenerationAttribute(semanticModel, logger, token);
		var defaultLogLevel = loggerGenerationAttribute?.DefaultLevel.IsSet == true
			? loggerGenerationAttribute.DefaultLevel.Value!.Value
			: Constants.Logging.DefaultLevel;
		var disableMSLoggingTelemetryGeneration = loggerAttribute.DisableMSLoggingTelemetryGeneration.Value
			?? loggerGenerationAttribute?.DisableMSLoggingTelemetryGeneration.Value
			?? false;
		var defaultPrefixType = loggerGenerationAttribute?.DefaultPrefixType.IsSet == true
			? loggerGenerationAttribute.DefaultPrefixType.Value!.Value
			: 0;

		if (!disableMSLoggingTelemetryGeneration)
		{
			// We got to here which means we're trying to use the new generation type,
			// so let's check if the LogPropertiesAttribute is referenced. If it's not,
			// we'll disable the new telemetry generation.
			disableMSLoggingTelemetryGeneration
				= context.SemanticModel.Compilation.GetTypeByMetadataName(
					Constants.Logging.MicrosoftExtensions.LogPropertiesAttribute.FullName) == null;
		}

		var generationType = SharedHelpers.GetGenerationTypes(interfaceSymbol, token);
		var fullNamespace = Utilities.GetFullNamespace(interfaceDeclaration, true);
		var logMethods = BuildLogMethods(
			generationType,
			className,
			defaultLogLevel,
			defaultPrefixType,
			loggerAttribute,
			context,
			semanticModel,
			interfaceSymbol,
			logger,
			token,
			out var methodDiagnostic
		);

		if (methodDiagnostic != null)
			return LoggerTarget.Failed(methodDiagnostic);

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

			LoggerAttribute: loggerAttribute,
			DefaultLevel: defaultLogLevel,

			LogMethods: logMethods,
			DuplicateMethods: BuildDuplicateMethods(interfaceSymbol),

			UseMSLoggingTelemetryBasedGeneration: !disableMSLoggingTelemetryGeneration
		);
	}

	static ImmutableArray<LogTarget> BuildLogMethods(
		GenerationType generationType,
		string className,
		int defaultLogLevel,
		int defaultPrefixType,
		LoggerAttributeRecord loggerTarget,
		GeneratorAttributeSyntaxContext _,
		SemanticModel semanticModel,
		INamedTypeSymbol interfaceSymbol,
		IGenerationLogger? logger,
		CancellationToken token,
		out TelemetryDiagnosticDescriptor? telemetryDiagnostic)
	{
		token.ThrowIfCancellationRequested();

		telemetryDiagnostic = null;

		List<LogTarget> methodTargets = [];
		foreach (var method in interfaceSymbol.GetMembers().OfType<IMethodSymbol>())
		{
			if (Utilities.ContainsAttribute(method, Constants.Shared.ExcludeAttribute, token))
			{
				logger?.Debug($"Skipping {interfaceSymbol.Name}.{method.Name}, explicitly excluded.");
				continue;
			}

			if (method.Arity > 0)
			{
				telemetryDiagnostic = TelemetryDiagnostics.General.GenericMethodsNotSupported;
				break;
			}

			logger?.Debug($"Found method {interfaceSymbol.Name}.{method.Name}.");

			var isScoped = !method.ReturnsVoid;
			var methodParameters = GetLogMethodParameters(method, semanticModel, logger, token);
			var logAttribute = SharedHelpers.GetLogAttribute(method, semanticModel, logger, token);
			var isKnownReturnType = method.ReturnsVoid || Constants.System.IDisposable.Equals(method.ReturnType);
			var loggerActionFieldName = $"_{Utilities.LowercaseFirstChar(method.Name)}Action";

			var logName = GetLogName(interfaceSymbol.Name, className, loggerTarget, logAttribute, method.Name, defaultPrefixType);
			var messageTemplate = logAttribute?.MessageTemplate.Value ?? GenerateTemplateMessage(logName, isScoped, methodParameters);
			var hasMultipleExceptions = !isScoped && methodParameters.Count(m => m.IsException) > 1;
			LogParameterTarget? exceptionParam = hasMultipleExceptions
				? null
				: isScoped
					? null
					: methodParameters.FirstOrDefault(m => m.IsException);

			var inferredErrorLevel = exceptionParam != null;
			if ((logAttribute?.Level.IsSet ?? false) == true)
				inferredErrorLevel = false;

			var level = (logAttribute?.Level.IsSet == true
				? logAttribute.Level.Value!.Value
				: exceptionParam == null
					? defaultLogLevel
					: 4 // Error
				)!;

			methodTargets.Add(new(
				MethodName: method.Name,
				IsScoped: isScoped,
				LoggerActionFieldName: loggerActionFieldName,

				UnknownReturnType: !isKnownReturnType,

				LogName: logName, // This includes any prefix information
				EventId: logAttribute?.EventId.Value,
				MessageTemplate: messageTemplate,
				MSLevel: Constants.Logging.LogLevelTypeMap[level],

				Parameters: methodParameters,
				ParametersSansException: isScoped
					? methodParameters
					: [.. methodParameters.Where(m => !m.IsException)],
				ExceptionParameter: exceptionParam,

				HasMultipleExceptions: hasMultipleExceptions,
				InferredErrorLevel: inferredErrorLevel,

				MethodLocation: method.Locations.FirstOrDefault(),

				TargetGenerationState: Utilities.IsValidGenerationTarget(method, generationType, GenerationType.Logging)
			));
		}

		return [.. methodTargets];
	}

	static string GetLogName(string interfaceName, string className, LoggerAttributeRecord loggerAttribute, LogAttributeRecord? logAttribute, string methodName, int defaultPrefixType)
	{
		if (logAttribute?.Name.IsSet == true)
			methodName = logAttribute!.Name.Value!;

		var prefixType = loggerAttribute.PrefixType.IsSet
			? loggerAttribute.PrefixType.Value
			: defaultPrefixType; // Default as LoggerGeneration level, or Default (0)

		if (prefixType == 1)
			// Interface
			return $"{interfaceName}.{methodName}";
		else if (prefixType == 2)
			// Class
			return $"{className}.{methodName}";
		else if (prefixType == 3)
		{
			// Custom
			if (!string.IsNullOrWhiteSpace(loggerAttribute.CustomPrefix.Value))
				return $"{loggerAttribute.CustomPrefix.Value}.{methodName}";
		}
		else if (prefixType == 4)
		{
			// TrimmedClassName
			if (interfaceName[0] == 'I')
				interfaceName = interfaceName.Substring(1);

			foreach (var suffix in SuffixesToRemove)
			{
				if (interfaceName.EndsWith(suffix, StringComparison.Ordinal) && interfaceName.Length > suffix.Length)
				{
					interfaceName = interfaceName.Substring(0, interfaceName.Length - suffix.Length);
					break;
				}
			}

			return $"{interfaceName}.{methodName}";
		}

		// This is the Default case or if it's Custom
		// and the CustomPrefix is null, empty or whitespace.
		return methodName;
	}

	static string GenerateTemplateMessage(string logEntryName, bool isScoped, ImmutableArray<LogParameterTarget> methodParameters)
	{
		StringBuilder builder = new();

		builder.Append(logEntryName);

		var count = methodParameters.Count(m => !m.IsException);
		if (count > 0)
			builder.Append(": ");

		var index = 0;
		foreach (var parameter in methodParameters)
		{
			if (!isScoped && parameter.IsException)
				continue;

			builder
				.Append(parameter.UpperCasedName)
				.Append(" = ")
				.Append('{')
				.Append(parameter.UpperCasedName)
				.Append("}, ")
			;

			index++;
		}

		if (index > 0)
			// Trim the last ", "
			builder.Remove(builder.Length - 2, 2);

		return builder.ToString();
	}
	static ImmutableArray<LogParameterTarget> GetLogMethodParameters(IMethodSymbol method, SemanticModel semanticModel, IGenerationLogger? logger, CancellationToken token)
	{
		List<LogParameterTarget> parameters = [];
		foreach (var parameter in method.Parameters)
		{
			token.ThrowIfCancellationRequested();

			var logPropertiesAttribute = SharedHelpers.GetLogPropertiesAttribute(
				parameter, semanticModel, logger, token);
			var expandEnumerableAttribute = SharedHelpers.GetExpandEnumerableAttribute(
				parameter, semanticModel, logger, token);

			List<LogPropertiesParameterDetails>? logProperties = null;
			if (logPropertiesAttribute != null)
			{
				// At this point, we know the caller wants to expand the properties for the given type.
				// So we can find the names of all the properties and their types.

				var type = parameter.Type;
				foreach (var property in type.GetMembers().OfType<IPropertySymbol>())
				{
					var propertyName = property.Name;
					if (Utilities.ContainsAttribute(property, Constants.Logging.MicrosoftExtensions.LogPropertyIgnoreAttribute, token))
					{
						logger?.Debug($"Skipping property {propertyName} on {parameter.Name} as it is marked with {Constants.Logging.MicrosoftExtensions.LogPropertyIgnoreAttribute}.");
						continue;
					}

					var isNullable = property.Type.IsReferenceType || property.Type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T;

					logProperties ??= [];

					logProperties.Add(new(
						PropertyName: propertyName,
						IsNullable: isNullable
					));
				}
			}

			parameters.Add(new(
				Name: parameter.Name,
				UpperCasedName: Utilities.UppercaseFirstChar(parameter.Name),
				FullyQualifiedType: Utilities.GetFullyQualifiedOrSystemName(parameter.Type),

				IsNullable: parameter.NullableAnnotation == NullableAnnotation.Annotated,
				IsException: Utilities.IsExceptionType(parameter.Type),

				IsIEnumerable: Utilities.IsIEnumerable(parameter.Type, semanticModel.Compilation),
				IsArray: Utilities.IsArray(parameter.Type),

				IsComplexType: Utilities.IsComplexType(parameter.Type),

				LogPropertiesAttribute: logPropertiesAttribute,
				LogProperties: logProperties?.ToImmutableArray(),

				ExpandEnumerableAttribute: expandEnumerableAttribute
			));
		}

		logger?.Debug($"Found {parameters.Count} parameter(s) for {method.Name}.");

		return [.. parameters];
	}
}
