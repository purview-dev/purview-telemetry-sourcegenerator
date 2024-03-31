using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class MeterTargetClassEmitter {

	static int EmitFields(MeterTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		indent++;

		builder
			.Append(indent, Constants.Metrics.SystemDiagnostics.Meter, withNewLine: false)
			.Append(' ')
			.Append(_meterFieldName)
			.AppendLine(" = default!;")
			.AppendLine()
		;

		foreach (var method in target.InstrumentationMethods) {
			if (method.ErrorDiagnostics.Length > 0) {
				var isError = false;
				foreach (var diagnostic in method.ErrorDiagnostics) {
					logger?.Diagnostic($"{diagnostic.Id}: {diagnostic.Description}");

					TelemetryDiagnostics.Report(context.ReportDiagnostic, diagnostic, method.MethodLocation);

					if (diagnostic.Severity == DiagnosticSeverity.Error) {
						isError = true;
					}
				}

				if (isError) {
					continue;
				}
			}

			if (method.InstrumentAttribute == null) {
				// We've already 'reported' this error, so we can skip it.
				continue;
			}

			var type = Constants.Metrics.InstrumentTypeMap[method.InstrumentAttribute.InstrumentType]
					.MakeGeneric(method.InstrumentMeasurementType);

			builder
				.Append(indent, type, withNewLine: false)
				.Append("? ")
				.Append(method.FieldName)
				.AppendLine(" = null;")
			;
		}

		return --indent;
	}
}
