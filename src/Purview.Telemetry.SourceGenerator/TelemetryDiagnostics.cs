using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator;

static partial class TelemetryDiagnostics {
	static public void Report(Action<Diagnostic> report, TelemetryDiagnosticDescriptor telemetryDiagnostic, Location? location, params object?[] args) {
		var diagnostic = Diagnostic.Create(
			new(
				id: telemetryDiagnostic.Id,
				title: telemetryDiagnostic.Title,
				messageFormat: telemetryDiagnostic.Description,
				category: telemetryDiagnostic.Category,
				defaultSeverity: telemetryDiagnostic.Severity,
				isEnabledByDefault: telemetryDiagnostic.EnabledByDefault
			),
			location,
			args
		);

		report(diagnostic);
	}
}
