using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class ActivityTargetClassEmitter {
	static int EmitMethods(ActivityGenerationTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		indent++;

		context.CancellationToken.ThrowIfCancellationRequested();

		foreach (var methodTarget in target.ActivityMethods) {
			context.CancellationToken.ThrowIfCancellationRequested();

			EmitActivityMethod(builder, indent, methodTarget, context, logger);
		}

		return --indent;
	}

	static void EmitActivityMethod(StringBuilder builder, int indent, ActivityMethodGenerationTarget methodTarget, SourceProductionContext context, IGenerationLogger? logger) {

	}
}
