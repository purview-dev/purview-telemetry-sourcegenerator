using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Targets;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class LoggerTargetClassEmitter {
	static int EmitCtor(LoggerGenerationTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		indent++;

		builder
			.AppendLine()
			.Append(indent, "public ", withNewLine: false)
			.Append(target.ClassNameToGenerate)
			.Append('(')
			.Append(Constants.Logging.MicrosoftExtensions.ILogger)
			.Append('<')
			.Append(target.FullyQualifiedInterfaceName)
			.Append('>')
			.Append(" logger)")
			.AppendLine()
			.Append(indent, '{')
			.Append(indent + 1, "_logger = logger;")
			.Append(indent, '}')
		;

		return --indent;
	}
}
