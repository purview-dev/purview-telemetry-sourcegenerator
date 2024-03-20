using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class ActivitySourceTargetClassEmitter {
	static int EmitMethods(ActivitySourceTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		indent++;

		EmitRecordExceptionEvent(builder, indent, target, context, logger);

		foreach (var methodTarget in target.ActivityMethods) {
			context.CancellationToken.ThrowIfCancellationRequested();

			EmitMethod(builder, indent, methodTarget, target, context, logger);
		}

		return --indent;
	}

	static void EmitRecordExceptionEvent(StringBuilder builder, int indent, ActivitySourceTarget methodTarget, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		builder
			.Append(indent, "static void ", withNewLine: false)
			.Append(Constants.Activities.RecordExceptionMethodName)
			.Append('(')
			.Append(Constants.Activities.SystemDiagnostics.Activity)
			.Append("? activity, ")
			.Append(Constants.System.Exception)
			.Append("? exception, ")
			.Append(Constants.System.BoolKeyword)
			.AppendLine(" escape)")
			.Append(indent, '{')
		;

		indent++;

		builder
			.Append(indent, "if (activity == null || exception == null)")
			.Append(indent, '{')
			.Append(indent + 1, "return;")
			.Append(indent, '}')
			.AppendLine()
		;

		const string tagsListVariableName = "tagsCollection";
		builder
			.Append(indent, Constants.Activities.SystemDiagnostics.ActivityTagsCollection, withNewLine: false)
			.Append(' ')
			.Append(tagsListVariableName)
			.Append(" = new ")
			.Append(Constants.Activities.SystemDiagnostics.ActivityTagsCollection)
			.AppendLine("();")
		;

		EmitExceptionParam(builder, indent, tagsListVariableName, "escape", "exception");

		const string eventVariableName = "recordExceptionEvent";

		builder
			.AppendLine()
			.Append(indent, Constants.Activities.SystemDiagnostics.ActivityEvent, withNewLine: false)
			.Append(' ')
			.Append(eventVariableName)
			.Append(" = new ")
			.Append(Constants.Activities.SystemDiagnostics.ActivityEvent)
			// name:
			.Append("(name: ")
			.Append(Constants.Activities.Tag_ExceptionEventName.Wrap())
			// timestamp:
			.Append(", timestamp: default")
			// tags:
			.Append(", tags: ")
			.Append(tagsListVariableName)
			.AppendLine(");")
		;

		builder
			.AppendLine()
			.Append(indent, "activity.AddEvent(", withNewLine: false)
			.Append(eventVariableName)
			.AppendLine(");")
		;

		builder
			.Append(--indent, '}')
			.AppendLine()
		;
	}

	static void EmitExceptionParam(StringBuilder builder, int indent,
		string tagsListVariableName,
		string escapeParam, string exceptionParam) {
		builder

			.Append(indent, tagsListVariableName, withNewLine: false)
			.Append(".Add(")
			.Append(Constants.Activities.Tag_ExceptionEscaped.Wrap())
			.Append(", ")
			.Append(escapeParam)
			.AppendLine(");")
		;

		builder
			.Append(indent, tagsListVariableName, withNewLine: false)
			.Append(".Add(")
			.Append(Constants.Activities.Tag_ExceptionMessage.Wrap())
			.Append(", ")
			.Append(exceptionParam)
			.AppendLine(".Message);")
		;

		builder
			.Append(indent, tagsListVariableName, withNewLine: false)
			.Append(".Add(")
			.Append(Constants.Activities.Tag_ExceptionType.Wrap())
			.Append(", ")
			.Append(exceptionParam)
			.AppendLine(".GetType().FullName);")
		;

		builder
			.Append(indent, tagsListVariableName, withNewLine: false)
			.Append(".Add(")
			.Append(Constants.Activities.Tag_ExceptionStackTrace.Wrap())
			.Append(", ")
			.Append(exceptionParam)
			.AppendLine(".StackTrace);")
		;
	}

	static void EmitMethod(StringBuilder builder, int indent, ActivityBasedGenerationTarget methodTarget, ActivitySourceTarget target, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		if (!methodTarget.TargetGenerationState.IsValid) {
			if (methodTarget.TargetGenerationState.RaiseMultiGenerationTargetsNotSupported) {
				logger?.Debug($"Identified {target.InterfaceName}.{methodTarget.MethodName} as problematic as it has another target types.");

				TelemetryDiagnostics.Report(context.ReportDiagnostic, TelemetryDiagnostics.General.MultiGenerationTargetsNotSupported, methodTarget.MethodLocation);
			}
			else if (methodTarget.TargetGenerationState.RaiseInferenceNotSupportedWithMultiTargeting) {
				logger?.Debug($"Identified {target.InterfaceName}.{methodTarget.MethodName} as problematic as it is inferred.");

				TelemetryDiagnostics.Report(context.ReportDiagnostic, TelemetryDiagnostics.General.InferenceNotSupportedWithMultiTargeting, methodTarget.MethodLocation);
			}

			return;
		}

		if (methodTarget.ReturnType != Constants.System.VoidKeyword
			&& !Constants.Activities.SystemDiagnostics.Activity.Equals(methodTarget.ReturnType)) {
			logger?.Diagnostic($"The return type {methodTarget.ReturnType} isn't valid for an activity or event.");

			TelemetryDiagnostics.Report(context.ReportDiagnostic, TelemetryDiagnostics.Activities.InvalidReturnType, methodTarget.MethodLocation);

			return;
		}

		builder
			.Append(indent, "public ", withNewLine: false)
			.Append(methodTarget.ReturnType)
		;

		if (methodTarget.IsNullableReturn) {
			builder.Append('?');
		}

		builder
			.Append(' ')
			.Append(methodTarget.MethodName)
			.Append('(')
		;

		var index = 0;
		foreach (var parameter in methodTarget.Parameters) {
			context.CancellationToken.ThrowIfCancellationRequested();

			builder
				.Append(parameter.ParameterType)
			;

			if (parameter.IsNullable) {
				builder.Append('?');
			}

			builder
				.Append(' ')
				.Append(parameter.ParameterName)
			;

			if (index < methodTarget.Parameters.Length - 1) {
				builder
					.Append(", ")
				;
			}

			index++;
		}

		builder
			.AppendLine(')')
			.Append(indent, '{')
		;

		indent++;

		if (methodTarget.MethodType == ActivityMethodType.Activity) {
			EmitActivityMethodBody(builder, indent, methodTarget, context, logger);
		}
		else if (methodTarget.MethodType == ActivityMethodType.Event) {
			EmitEventMethodBody(builder, indent, methodTarget, context, logger);
		}
		else if (methodTarget.MethodType == ActivityMethodType.Context) {
			EmitContextMethodBody(builder, indent, methodTarget, context, logger);
		}

		builder
			.Append(--indent, '}')
			.AppendLine()
		;
	}
}
