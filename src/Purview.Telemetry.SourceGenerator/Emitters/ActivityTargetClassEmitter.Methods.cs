using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class ActivityTargetClassEmitter {
	static int EmitMethods(ActivityGenerationTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		indent++;

		foreach (var methodTarget in target.ActivityMethods) {
			context.CancellationToken.ThrowIfCancellationRequested();

			EmitActivityOrEventMethod(builder, indent, methodTarget, context, logger);
		}

		return --indent;
	}

	static void EmitActivityOrEventMethod(StringBuilder builder, int indent, ActivityMethodGenerationTarget methodTarget, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		if (methodTarget.ReturnType != Constants.System.VoidKeyword && !Constants.Activities.SystemDiagnostics.Activity.Equals(methodTarget.ReturnType)) {
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

		if (methodTarget.IsActivity) {
			EmitActivityMethodBody(builder, indent, methodTarget, context, logger);
		}
		else {
			EmitEventMethodBody(builder, indent, methodTarget, context, logger);
		}

		builder
			.Append(--indent, '}')
			.AppendLine()
		;
	}
}
