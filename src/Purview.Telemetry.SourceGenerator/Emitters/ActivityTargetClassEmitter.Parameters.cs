using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class ActivityTargetClassEmitter {
	static void EmitTagsOrBaggageParameters(StringBuilder builder, int indent, string activityVariableName, bool isTag, ImmutableArray<ActivityMethodParameterTarget> parameters, bool checkForNullableActivity) {
		if (parameters.Length == 0) {
			return;
		}

		if (checkForNullableActivity) {
			builder
				.AppendLine()
				.Append(indent, "if (", withNewLine: false)
				.Append(activityVariableName)
				.AppendLine(" != null)")
				.Append(indent, '{');

			indent++;
		}

		foreach (var param in parameters) {
			if (param.SkipOnNullOrEmpty) {
				builder
					.Append(indent, "if (", withNewLine: false)
					.Append(param.ParameterName)
					.AppendLine(" != default)")
					.Append(indent, "{")
				;

				indent++;
			}

			builder
				.Append(indent, activityVariableName, withNewLine: false)
				.Append('.')
				.Append(isTag ? "SetTag" : "SetBaggage")
				.Append('(')
				.Append(param.GeneratedName.Wrap())
				.Append(", ")
				.Append(param.ParameterName)
				.AppendLine(");")
			;

			if (param.SkipOnNullOrEmpty) {
				indent--;

				builder
					.Append(indent, "}")
				;
			}
		}

		if (checkForNullableActivity) {
			builder
				.Append(--indent, '}')
			;
		}
	}

	static bool GuardParameters(ActivityMethodGenerationTarget methodTarget, SourceProductionContext context, IGenerationLogger? logger,
		out ActivityMethodParameterTarget? activityParam,
		out ActivityMethodParameterTarget? parentContextOrId,
		out ActivityMethodParameterTarget? tagsParam,
		out ActivityMethodParameterTarget? linksParam,
		out ActivityMethodParameterTarget? startTimeParam,
		out ActivityMethodParameterTarget? timestampParam
		) {

		activityParam = null;
		parentContextOrId = null;
		tagsParam = null;
		linksParam = null;
		startTimeParam = null;
		timestampParam = null;

		var activityParams = methodTarget.Parameters.Where(m => m.ParamDestination == ActivityParameterDestination.Activity).ToImmutableArray();
		var parentContextOrIdParams = methodTarget.Parameters.Where(m => m.ParamDestination == ActivityParameterDestination.ParentContextOrId).ToImmutableArray();
		var tagsParams = methodTarget.Parameters.Where(m => m.ParamDestination == ActivityParameterDestination.TagsEnumerable).ToImmutableArray();
		var linksParams = methodTarget.Parameters.Where(m => m.ParamDestination == ActivityParameterDestination.LinksEnumerable).ToImmutableArray();
		var startTimeParams = methodTarget.Parameters.Where(m => m.ParamDestination == ActivityParameterDestination.StartTime).ToImmutableArray();
		var timestampParams = methodTarget.Parameters.Where(m => m.ParamDestination == ActivityParameterDestination.Timestamp).ToImmutableArray();

		if (activityParams.Length > 1) {
			logger?.Diagnostic("More than one activity parameter defined.");

			TelemetryDiagnostics.Report(context.ReportDiagnostic,
				TelemetryDiagnostics.Activities.DuplicateParameterTypes,
				activityParams.First().Location,
				string.Join(", ", activityParams.Select(m => m.ParameterName)),
				"activity"
			);

			return false;
		}
		else {
			activityParam = activityParams.FirstOrDefault();
		}

		if (parentContextOrIdParams.Length > 1) {
			logger?.Diagnostic("More than one parent context/ id defined.");

			TelemetryDiagnostics.Report(context.ReportDiagnostic,
				TelemetryDiagnostics.Activities.DuplicateParameterTypes,
				parentContextOrIdParams.First().Location,
				string.Join(", ", parentContextOrIdParams.Select(m => m.ParameterName)),
				"parent context/ parent context Id"
			);

			return false;
		}
		else {
			parentContextOrId = parentContextOrIdParams.FirstOrDefault();
		}

		if (tagsParams.Length > 1) {
			logger?.Diagnostic("More than one tag IEnumerable defined.");

			TelemetryDiagnostics.Report(context.ReportDiagnostic,
				TelemetryDiagnostics.Activities.DuplicateParameterTypes,
				tagsParams.First().Location,
				string.Join(", ", tagsParams.Select(m => m.ParameterName)),
				"IEnumerable of ActivityTags"
			);

			return false;
		}
		else {
			tagsParam = tagsParams.FirstOrDefault();
		}

		if (linksParams.Length > 1) {
			logger?.Diagnostic("More than one link IEnumerable defined.");

			TelemetryDiagnostics.Report(context.ReportDiagnostic,
				TelemetryDiagnostics.Activities.DuplicateParameterTypes,
				linksParams.First().Location,
				string.Join(", ", linksParams.Select(m => m.ParameterName)),
				"IEnumerable of ActivityLinks"
			);

			return false;
		}
		else {
			linksParam = linksParams.FirstOrDefault();
		}

		// There can be only one as it's checked on the
		// combination of parameter name and type.
		startTimeParam = startTimeParams.FirstOrDefault();
		timestampParam = timestampParams.FirstOrDefault();

		return true;
	}
}
