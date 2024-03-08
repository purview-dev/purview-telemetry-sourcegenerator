using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Targets;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class LoggerTargetClassEmitter {
	static int EmitFields(LoggerTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		var defaultLevel =
			target.LoggerTargetAttribute.DefaultLevel.IsSet
			? target.LoggerTargetAttribute.DefaultLevel.Value!
			: target.LoggerDefaultsAttribute?.DefaultLevel.IsSet == true
				? target.LoggerDefaultsAttribute.DefaultLevel.Value
				: Logging.LogGeneratedLevel.Information;

		builder
			.Append(indent + 1, "const ", withNewLine: false)
			.Append(Constants.Logging.MicrosoftExtensions.LogLevel)
			.Append(' ')
			.Append(Constants.Logging.DefaultLogLevelConstantName)
			.Append(" = ")
			.Append(Constants.Logging.MicrosoftExtensions.ConvertToMSLogLevel(defaultLevel!))
			.AppendLine(';');
		;

		// Generate the event types.
		builder
			.Append(indent + 1, "readonly ", withNewLine: false)
			.Append(Constants.Shared.List)
			.Append('<')
			.Append(Constants.Shared.Type)
			.Append("> _eventTypes = new ")
			.Append(Constants.Shared.List)
			.Append('<')
			.Append(Constants.Shared.Type)
			.AppendLine(">() {")
			.Append(3, "// System events")
			.Append(3, "typeof(Purview.EventSourcing.Aggregates.Events.DeleteEvent),")
			.Append(3, "typeof(Purview.EventSourcing.Aggregates.Events.RestoreEvent),")
			.Append(3, "typeof(Purview.EventSourcing.Aggregates.Events.ForceSaveEvent),")
		;

		if (target.GeneratedApplyMethods.Length > 0) {

			builder.Append(indent + 2, "// Generated events");
			foreach (var generatedEvent in target.GeneratedApplyMethods) {
				builder
					.Append(indent + 2, "typeof(", withNewLine: false)
					.Append(generatedEvent.EventType)
					.AppendLine("),")
				;
			}
		}

		if (target.PredefinedApplyMethods.Length > 0) {

			builder.Append(indent + 2, "// Found pre-defined events");
			foreach (var foundEvent in target.PredefinedApplyMethods) {
				builder
					.Append(indent + 2, "typeof(", withNewLine: false)
					.Append(foundEvent.EventType)
					.AppendLine("),")
				;
			}
		}

		builder
			.Append(indent + 1, "};")
			.AppendLine()
		;

		return indent;
	}
}
