using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Targets;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class LoggerTargetClassEmitter {
	static int EmitNamespaceStart(LoggerTarget target, StringBuilder builder, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		logger?.Debug($"Generating namespace for: {target.FullyQualifiedName}");

		var indent = 0;
		if (target.ClassNamespace != null) {
			builder
				.Append("namespace ")
				.AppendLine(target.ClassNamespace)
			;

			builder
				.Append('{')
				.AppendLine();

			indent++;
		}

		if (target.ParentClasses.Length > 0) {
			logger?.Debug($"Generating parent partial classes for: {target.FullyQualifiedName}");

			foreach (var parentClass in target.ParentClasses.Reverse()) {
				builder
					.Append(indent, "partial class ", withNewLine: false)
					.Append(parentClass)
					.AppendLine()
					.Append(indent, "{");

				indent++;
			}
		}

		return indent++;
	}

	static void EmitNamespaceEnd(LoggerTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		logger?.Debug($"Generating namespace end for: {target.FullyQualifiedName}");

		if (target.ParentClasses.Length > 0) {
			foreach (var parentClass in target.ParentClasses) {
				builder
					.Append(--indent, '}')
				;
			}
		}

		if (target.ClassNamespace != null) {
			builder
				.Append('}')
			;
		}
	}
}
