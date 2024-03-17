using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class ActivityTargetClassEmitter {
	static void EmitActivityMethodBody(StringBuilder builder, int indent, ActivityMethodGenerationTarget methodTarget, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		if (!GuardParameters(methodTarget, context, logger,
			out var activityParam,
			out var parentContextOrId,
			out var tagsParam,
			out var linksParam,
			out var startTimeParam,
			out var timestampParam)) {
			return;
		}

		if (activityParam != null) {
			logger?.Diagnostic("Activity parameter not allowed on Activity start/ create method, only event.");

			TelemetryDiagnostics.Report(context.ReportDiagnostic,
				TelemetryDiagnostics.Activities.ActivityParameterNotAllowed,
				activityParam.Location,
				activityParam.ParameterName
			);

			return;
		}

		if (timestampParam != null) {
			logger?.Diagnostic("Timestamp parameter not allowed on Activity start/ create method, only events.");

			TelemetryDiagnostics.Report(context.ReportDiagnostic,
				TelemetryDiagnostics.Activities.TimestampParameterNotAllowed,
				timestampParam.Location,
				timestampParam.ParameterName
			);

			return;
		}

		var activityVariableName = "activity" + methodTarget.MethodName;

		builder
			.Append(indent, Constants.Activities.SystemDiagnostics.Activity, withNewLine: false)
			.Append("? ")
			.Append(activityVariableName)
			.Append(" = ")
			.Append(Constants.Activities.ActivitySourceFieldName)
			.Append('.')
		;

		var createOnly = methodTarget.ActivityAttribute?.CreateOnly?.Value == true;
		var createActivityMethod = createOnly
			? "Create"
			: "Start";
		var parentContextParameterName = Constants.Activities.SystemDiagnostics.ActivityContext.Equals(parentContextOrId?.ParameterType)
			? "parentContext"
			: "parentId";

		if (createOnly && startTimeParam != null) {
			logger?.Diagnostic("StartTime parameter not allowed on Activity create method.");

			TelemetryDiagnostics.Report(context.ReportDiagnostic,
				TelemetryDiagnostics.Activities.StartTimeParameterNotAllowed,
				startTimeParam.Location,
				startTimeParam.ParameterName
			);

			return;
		}

		builder
			.Append(createActivityMethod)
			// name:
			.Append("Activity(name: ")
			.Append(methodTarget.ActivityOrEventName.Wrap())
			// kind:
			.Append(", kind: ")
			.Append(Constants.Activities.SystemDiagnostics.ActivityKind.FullName)
			.Append('.')
			.Append(methodTarget.ActivityAttribute?.Kind?.Value ?? Activities.ActivityGeneratedKind.Internal)
			// parentContext/ parentId:
			.Append(", ")
			.Append(parentContextParameterName)
			.Append(": ")
			.Append(parentContextOrId?.ParameterName ?? "default")
			// tags:
			.Append(", tags: ")
			.Append(tagsParam?.ParameterName ?? "default")
			// links:
			.Append(", links: ")
			.Append(linksParam?.ParameterName ?? "default")
		;

		if (!createOnly) {
			builder
				// startTime:
				.Append(", startTime: ")
				.Append(startTimeParam?.ParameterName ?? "default")
			;
		}

		builder
			.AppendLine(");")
		;

		context.CancellationToken.ThrowIfCancellationRequested();

		EmitTagsOrBaggageParameters(builder, indent, activityVariableName, true, methodTarget.Tags, true);
		EmitTagsOrBaggageParameters(builder, indent, activityVariableName, false, methodTarget.Baggage, true);

		context.CancellationToken.ThrowIfCancellationRequested();

		if (Constants.Activities.SystemDiagnostics.Activity.Equals(methodTarget.ReturnType)) {
			builder
				.AppendLine()
				.Append(indent, "return ", withNewLine: false)
				.Append(activityVariableName)
				.AppendLine(';')
			;
		}
	}
}
