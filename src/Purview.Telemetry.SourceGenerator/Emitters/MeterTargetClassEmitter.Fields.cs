using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class MeterTargetClassEmitter {

	static int EmitFields(MeterGenerationTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		indent++;

		builder
			.Append(indent, "readonly ", withNewLine: false)
			.Append(Constants.Metrics.SystemDiagnostics.Meter)
			.Append(' ')
			.Append(_meterFieldName)
			.AppendLine(';')
			.AppendLine()
		;

		foreach (var method in target.InstrumentationMethods) {
			if (method.ErrorDiagnostics.Length > 0) {
				foreach (var diagnostic in method.ErrorDiagnostics) {
					logger?.Diagnostic($"{diagnostic.Id}: {diagnostic.Description}");

					TelemetryDiagnostics.Report(context.ReportDiagnostic, diagnostic, method.MethodLocation);
				}

				continue;
			}

			if (method.InstrumentAttribute == null) {
				// We've already 'reported' this error, so we can skip it.
				continue;
			}

			var type = Constants.Metrics.InstrumentTypeMap[method.InstrumentAttribute.InstrumentType]
					.MakeGeneric(method.InstrumentMeasurementType);

			builder
				.Append(indent, method.IsObservable ? "" : "readonly ", withNewLine: false)
				.Append(type)
				.Append(method.IsObservable ? "?" : "")
				.Append(' ')
				.Append(method.FieldName)
			;

			if (method.IsObservable) {
				builder
					.Append(" = null")
				;
			}

			builder
				.AppendLine(';');
			;
		}

		return --indent;
	}
}
