﻿using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class ActivitySourceTargetClassEmitter {
	static int EmitMethods(ActivityGenerationTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		indent++;

		foreach (var methodTarget in target.ActivityMethods) {
			context.CancellationToken.ThrowIfCancellationRequested();

			EmitMethod(builder, indent, methodTarget, target, context, logger);
		}

		return --indent;
	}

	static void EmitMethod(StringBuilder builder, int indent, ActivityMethodGenerationTarget methodTarget, ActivityGenerationTarget target, SourceProductionContext context, IGenerationLogger? logger) {
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