using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Targets;
using Purview.Telemetry.SourceGenerator.Templates;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class LoggerTargetClassEmitter {
	static void GenerateLoggerClass(LoggerTarget target, SourceProductionContext context, IGenerationLogger? logger) {
		StringBuilder builder = new();

		builder
			.AppendLine("#nullable enable")
			.AppendLine()
		;

		var indent = EmitNamespaceStart(target, builder, context, logger);

		indent = EmitClassStart(target, builder, indent, context, logger);
		//indent = EmitFields(target, builder, indent, context, logger);
		//indent = EmitProperties(target, builder, indent, context, logger);
		//indent = EmitMethods(target, builder, indent, context, logger);

		EmitClassEnd(target, builder, indent, context, logger);
		EmitNamespaceEnd(target, builder, indent, context, logger);

		var sourceText = EmbeddedResources.Instance.AddHeader(builder.ToString());
		var hintName = $"{target.FullyQualifiedName}.g.cs";

		context.AddSource(hintName, Microsoft.CodeAnalysis.Text.SourceText.From(sourceText, Encoding.UTF8));
	}

	static int EmitClassStart(LoggerTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		logger?.Debug($"Generating class for: {target.FullyQualifiedName}");

		builder
			.Append(indent, "sealed partial class ", withNewLine: false)
			.Append(target.ClassNameToGenerate)
			.Append(" : ")
			.Append(target.FullyQualifiedInterfaceName)
			.AppendLine()
			.Append(indent, '{')
		;

		return indent;
	}

	static void EmitClassEnd(LoggerTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		logger?.Debug($"Generating class end for: {target.FullyQualifiedName}");

		builder
			.Append(indent, '}')
		;
	}
}
