using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Purview.Telemetry.SourceGenerator.Targets;

namespace Purview.Telemetry.SourceGenerator.Helpers;
partial class PipelineHelpers {
	static public bool HasLoggerAttribute(SyntaxNode _, CancellationToken __) => true;

	static public LoggerTarget? BuildLoggerTransform(GeneratorAttributeSyntaxContext context, IGenerationLogger? logger, CancellationToken token) {
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
		var loggerTargetAttribute = SharedHelpers.GetGenerateLoggerTargetAttribute(context.Attributes[0], semanticModel, logger, token);
		if (loggerTargetAttribute == null) {
			logger?.Error($"Could not find {Constants.Logging.LoggerTargetAttribute} when one was expected '{interfaceDeclaration.Flatten()}'.");

			return null;
		}

		var className = loggerTargetAttribute.ClassName.IsSet
			? loggerTargetAttribute.ClassName.Value!
			: GenerateClassName(interfaceSymbol.Name);

		var fullNamespace = Utilities.GetFullNamespace(interfaceDeclaration, true);
		return new(
			ClassNameToGenerate: className,
			ClassNamespace: Utilities.GetNamespace(interfaceDeclaration),
			ParentClasses: Utilities.GetParentClasses(interfaceDeclaration),
			FullNamespace: fullNamespace,
			FullyQualifiedName: fullNamespace + className,

			InterfaceName: interfaceSymbol.Name,
			FullyQualifiedInterfaceName: fullNamespace + interfaceSymbol.Name,

			LoggerTargetAttribute: loggerTargetAttribute,
			LoggerDefaultsAttribute: GetLoggerTargetAttributeRecord(semanticModel, logger, token)
		);
	}

	static string GenerateClassName(string name) {
		if (name[0] == 'I') {
			name = name.Substring(1);
		}

		return name + "Core";
	}

	static public LoggerDefaultsAttributeRecord? GetLoggerTargetAttributeRecord(SemanticModel semanticModel, IGenerationLogger? logger, CancellationToken token) {
		token.ThrowIfCancellationRequested();

		var loggerDefaultAttributeData = SharedHelpers.GetAttributeData(semanticModel.Compilation.Assembly, Constants.Logging.LogGeneratedLevel);
		return loggerDefaultAttributeData == null
			? null
			: SharedHelpers.GetGenerateLoggerDefaultsAttribute(loggerDefaultAttributeData, semanticModel, logger, token);
	}
}
