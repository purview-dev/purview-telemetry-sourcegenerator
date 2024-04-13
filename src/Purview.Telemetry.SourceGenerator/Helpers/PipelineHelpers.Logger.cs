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
		"Logger"
	];

	public static bool HasLoggerTargetAttribute(SyntaxNode _, CancellationToken __) => true;

	public static LoggerTarget? BuildLoggerTransform(GeneratorAttributeSyntaxContext context, IGenerationLogger? logger, CancellationToken token)
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
		var loggerAttribute = SharedHelpers.GetLoggerAttribute(context.Attributes[0], semanticModel, logger, token);
		if (loggerAttribute == null)
		{
			logger?.Error($"Could not find {Constants.Logging.LoggerAttribute} when one was expected '{interfaceDeclaration.Flatten()}'.");

			return null;
		}

		var telemetryGeneration = SharedHelpers.GetTelemetryGenerationAttribute(interfaceSymbol, semanticModel, logger, token);
		var className = telemetryGeneration.ClassName.IsSet
			? telemetryGeneration.ClassName.Value!
			: GenerateClassName(interfaceSymbol.Name);

		var loggerGenerationAttribute = SharedHelpers.GetLoggerDefaultsAttribute(semanticModel, logger, token);
		var defaultLogLevel = loggerGenerationAttribute?.DefaultLevel.IsSet == true
			? loggerGenerationAttribute.DefaultLevel.Value!
			: Constants.Logging.DefaultLevel;

		var generationType = SharedHelpers.GetGenerationTypes(interfaceSymbol, token);
		var fullNamespace = Utilities.GetFullNamespace(interfaceDeclaration, true);
		var logMethods = BuildLogMethods(
			generationType,
			className,
			defaultLogLevel.Value,
			loggerAttribute,
			context,
			semanticModel,
			interfaceSymbol,
			logger,
			token
		);

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
			DefaultLevel: defaultLogLevel.Value,

			LogMethods: logMethods
		);
	}

	static ImmutableArray<LogTarget> BuildLogMethods(
		GenerationType generationType,
		string className,
		int defaultLogLevel,
		LoggerAttributeRecord loggerTarget,
		GeneratorAttributeSyntaxContext _,
		SemanticModel semanticModel,
		INamedTypeSymbol interfaceSymbol,
		IGenerationLogger? logger,
		CancellationToken token)
	{

		token.ThrowIfCancellationRequested();

		List<LogTarget> methodTargets = [];
		foreach (var method in interfaceSymbol.GetMembers().OfType<IMethodSymbol>())
		{
			if (Utilities.ContainsAttribute(method, Constants.Shared.ExcludeAttribute, token))
			{
				logger?.Debug($"Skipping {interfaceSymbol.Name}.{method.Name}, explicitly excluded.");
				continue;
			}

			logger?.Debug($"Found method {interfaceSymbol.Name}.{method.Name}.");

			var isScoped = !method.ReturnsVoid;
			var methodParameters = GetLogMethodParameters(method, logger, token);
			var logAttribute = SharedHelpers.GetLogAttribute(method, semanticModel, logger, token);
			var isKnownReturnType = method.ReturnsVoid || Constants.System.IDisposable.Equals(method.ReturnType);
			var loggerActionFieldName = $"_{Utilities.LowercaseFirstChar(method.Name)}Action";

			var logName = GetLogName(interfaceSymbol.Name, className, loggerTarget, logAttribute, method.Name);
			var messageTemplate = logAttribute?.MessageTemplate.Value ?? GenerateTemplateMessage(logName, isScoped, methodParameters);
			var hasMultipleExceptions = !isScoped && methodParameters.Count(m => m.IsException) > 1;
			LogParameterTarget? exceptionParam = hasMultipleExceptions
				? null
				: isScoped
					? null
					: methodParameters.FirstOrDefault(m => m.IsException);

			var inferredErrorLevel = exceptionParam != null;
			if ((logAttribute?.Level.IsSet ?? false) == true)
			{
				inferredErrorLevel = false;
			}

			var level = (logAttribute?.Level.IsSet == true
				? logAttribute.Level.Value!
				: exceptionParam == null
					? defaultLogLevel
					: 4 // Error
				)!;

			methodTargets.Add(new(
				MethodName: method.Name,
				IsScoped: isScoped,
				LoggerActionFieldName: loggerActionFieldName,

				UnknownReturnType: !isKnownReturnType,

				LogName: logName,
				EventId: logAttribute?.EventId.Value,
				MessageTemplate: messageTemplate,
				MSLevel: Constants.Logging.LogLevelTypeMap[level.Value],

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

	static string GetLogName(string interfaceName, string className, LoggerAttributeRecord loggerAttribute, LogAttributeRecord? logAttribute, string methodName)
	{
		if (logAttribute?.Name.IsSet == true)
		{
			methodName = logAttribute!.Name.Value!;
		};

		var prefixType = loggerAttribute.PrefixType.IsSet
			? loggerAttribute.PrefixType.Value
			: 0; // Default

		if (prefixType == 0)
		{
			// Default
			if (interfaceName[0] == 'I')
			{
				interfaceName = interfaceName.Substring(1);
			}

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
		else if (prefixType == 1)
		{
			// Interface
			return $"{interfaceName}.{methodName}";
		}
		else if (prefixType == 2)
		{
			// Class
			return $"{className}.{methodName}";
		}
		else if (prefixType == 3)
		{
			// Custom
			if (!string.IsNullOrWhiteSpace(loggerAttribute.CustomPrefix.Value))
			{
				return $"{loggerAttribute.CustomPrefix.Value}.{methodName}";
			}
		}

		// This is the NoSuffix case or if it's Custom and the CustomPrefix is null, empty or whitespace.
		return methodName;
	}

	static string GenerateTemplateMessage(string logEntryName, bool isScoped, ImmutableArray<LogParameterTarget> methodParameters)
	{
		StringBuilder builder = new();

		builder.Append(logEntryName);

		var count = methodParameters.Count(m => !m.IsException);
		if (count > 0)
		{
			builder.Append(": ");
		}

		var index = 0;
		foreach (var parameter in methodParameters)
		{
			if (!isScoped && parameter.IsException)
			{
				continue;
			}

			builder
				.Append(parameter.Name)
				.Append(": ")
				.Append('{')
				.Append(parameter.UpperCasedName)
				.Append("}, ")
			;

			index++;
		}

		if (index > 0)
		{
			// Trim the last ", "
			builder.Remove(builder.Length - 2, 2);
		}

		return builder.ToString();
	}

	static ImmutableArray<LogParameterTarget> GetLogMethodParameters(IMethodSymbol method, IGenerationLogger? logger, CancellationToken token)
	{
		List<LogParameterTarget> parameters = [];
		foreach (var parameter in method.Parameters)
		{
			token.ThrowIfCancellationRequested();

			parameters.Add(new(
				Name: parameter.Name,
				UpperCasedName: Utilities.UppercaseFirstChar(parameter.Name),
				FullyQualifiedType: Utilities.GetFullyQualifiedName(parameter.Type),
				IsNullable: parameter.NullableAnnotation == NullableAnnotation.Annotated,
				IsException: Utilities.IsExceptionType(parameter.Type)
			));
		}

		logger?.Debug($"Found {parameters.Count} parameter(s) for {method.Name}.");

		return [.. parameters];
	}
}
