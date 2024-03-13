﻿using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class ActivityTargetClassEmitter {
	static int EmitFields(ActivityGenerationTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		indent++;

		var activitySourceName = target.ActivitySourceName;
		if (string.IsNullOrWhiteSpace(activitySourceName)) {
			logger?.Diagnostic($"No activity source specified.");

			TelemetryDiagnostics.Report(context.ReportDiagnostic, TelemetryDiagnostics.Activities.NoActivitySourceSpecified, location: null);

			activitySourceName = Constants.DefaultActivitySourceName;
		}

		builder
			.Append(indent, "readonly static ", withNewLine: false)
			.Append(Constants.Activities.SystemDiagnostics.ActivitySource)
			.Append(' ')
			.Append(Constants.Activities.ActivitySourceFieldName)
			.Append(" = new ")
			.Append(Constants.Activities.SystemDiagnostics.ActivitySource)
			.Append('(')
			.Append(activitySourceName!.Wrap())
			.AppendLine(");")
			.AppendLine()
		;

		return --indent;
	}
}
