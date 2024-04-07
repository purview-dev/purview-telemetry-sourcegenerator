using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class ActivitySourceTargetClassEmitter {
	static void EmitTagsOrBaggageParameters(StringBuilder builder, int indent,
		string activityVariableName,
		bool populateTags,
		ActivityBasedGenerationTarget method,
		bool checkForNullableActivity,
		SourceProductionContext context, IGenerationLogger? logger) {

		var parameters = populateTags ? method.Tags : method.Baggage;
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

		var useRecordedExceptionRules = Constants.Activities.UseRecordExceptionRulesDefault;
		if (method.EventAttribute?.UseRecordExceptionRules.IsSet == true) {
			useRecordedExceptionRules = method.EventAttribute.UseRecordExceptionRules.Value!.Value;
		}

		foreach (var param in parameters) {
			if (populateTags && param.IsException && useRecordedExceptionRules) {
				continue;
			}

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
				.Append(populateTags ? "SetTag" : "SetBaggage")
				.Append('(')
				.Append(param.GeneratedName.Wrap())
				.Append(", ")
				.Append(param.ParameterName)
			;

			if (!populateTags && !Utilities.IsString(param.ParameterType)) {
				logger?.Diagnostic("Found a baggage parameter type that is not a string.");

				TelemetryDiagnostics.Report(context.ReportDiagnostic, TelemetryDiagnostics.Activities.BaggageParameterShouldBeString, param.Location);

				if (param.IsNullable) {
					builder
						.Append('?')
					;
				}

				builder
					.Append(".ToString()")
				;
			}

			builder
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

	static bool GuardParameters(ActivityBasedGenerationTarget methodTarget, SourceProductionContext context, IGenerationLogger? logger,
		out ActivityBasedParameterTarget? activityParam,
		out ActivityBasedParameterTarget? parentContextOrId,
		out ActivityBasedParameterTarget? tagsParam,
		out ActivityBasedParameterTarget? linksParam,
		out ActivityBasedParameterTarget? startTimeParam,
		out ActivityBasedParameterTarget? timestampParam,
		out ActivityBasedParameterTarget? escapeParam
		) {

		activityParam = null;
		parentContextOrId = null;
		tagsParam = null;
		linksParam = null;
		startTimeParam = null;
		timestampParam = null;
		escapeParam = null;

		var activityParams = methodTarget.Parameters.Where(m => m.ParamDestination == ActivityParameterDestination.Activity).ToImmutableArray();
		var parentContextOrIdParams = methodTarget.Parameters.Where(m => m.ParamDestination == ActivityParameterDestination.ParentContextOrId).ToImmutableArray();
		var tagsParams = methodTarget.Parameters.Where(m => m.ParamDestination == ActivityParameterDestination.TagsEnumerable).ToImmutableArray();
		var linksParams = methodTarget.Parameters.Where(m => m.ParamDestination == ActivityParameterDestination.LinksEnumerable).ToImmutableArray();
		var startTimeParams = methodTarget.Parameters.Where(m => m.ParamDestination == ActivityParameterDestination.StartTime).ToImmutableArray();
		var timestampParams = methodTarget.Parameters.Where(m => m.ParamDestination == ActivityParameterDestination.Timestamp).ToImmutableArray();
		var escapeParams = methodTarget.Parameters.Where(m => m.ParamDestination == ActivityParameterDestination.Escape).ToImmutableArray();

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
			logger?.Diagnostic("More than one ActivityLink/ IEnumerable of ActivityLink is defined.");

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

		if (escapeParams.Length > 1) {
			logger?.Diagnostic("More than one Escape parameter defined.");

			TelemetryDiagnostics.Report(context.ReportDiagnostic,
				TelemetryDiagnostics.Activities.DuplicateParameterTypes,
				escapeParams.First().Location,
				string.Join(", ", escapeParams.Select(m => m.ParameterName)),
				"escape parameters"
			);

			return false;
		}
		else {
			escapeParam = escapeParams.FirstOrDefault();
			if (escapeParam != null) {
				if (!Utilities.IsBoolean(escapeParam.ParameterType)) {
					TelemetryDiagnostics.Report(context.ReportDiagnostic,
						TelemetryDiagnostics.Activities.EscapedParameterInvalidType,
						escapeParams.First().Location,
						string.Join(", ", escapeParams.Select(m => m.ParameterName)),
						"escape parameters"
					);

					return false;
				}

				if (methodTarget.MethodType != ActivityMethodType.Event) {
					TelemetryDiagnostics.Report(context.ReportDiagnostic,
						TelemetryDiagnostics.Activities.EscapedParameterInvalidType,
						escapeParams.First().Location,
						escapeParam.ParameterName
					);

					return false;
				}
			}
		}

		// There can be only one as it's checked on the
		// combination of parameter name and type.
		startTimeParam = startTimeParams.FirstOrDefault();
		timestampParam = timestampParams.FirstOrDefault();

		return true;
	}
}
