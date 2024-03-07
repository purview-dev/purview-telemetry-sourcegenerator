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

		throw new NotImplementedException();
	}
}
