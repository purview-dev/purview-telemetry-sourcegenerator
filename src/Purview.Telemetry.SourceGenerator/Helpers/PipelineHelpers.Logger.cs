using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Purview.Telemetry.SourceGenerator.Targets;

namespace Purview.Telemetry.SourceGenerator.Helpers;
sealed partial class PipelineHelpers {
	static public bool HasLoggerAttribute(SyntaxNode _, CancellationToken __) => true;

	static public LoggerTarget? BuildLoggerTransform(GeneratorAttributeSyntaxContext context, IGenerationLogger? logger, CancellationToken token) {
		token.ThrowIfCancellationRequested();

		if (context.TargetNode is not InterfaceDeclarationSyntax interfaceDeclaration) {
			logger?.Error($"Could not find interface syntax from the target node '{context.TargetNode.Flatten()}'.");
			return null;
		}

		if (context.TargetSymbol is not INamedTypeSymbol classSymbol) {
			logger?.Error($"Could not find class symbol '{interfaceDeclaration.Flatten()}'.");
			return null;
		}

		var semanticModel = context.SemanticModel;
		var generateIAggregateAttribute = SharedHelpers.GetLoggerTargetAttribute(context.Attributes[0], semanticModel, logger, token);
		if (generateIAggregateAttribute == null) {
			logger?.Invoke($"Could not find {Constants.Core.GenerateIAggregateAttribute} when one was expected '{interfaceDeclaration.Flatten()}'.", OutputType.Error);

			return null;
		}

	}
}
