using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry;

static partial class TelemetryDiagnostics {
	public const string IdentityNameKey = "{IdentityName}";
	public const string KindKey = "{Kind}";

	static public void Report(this Action<Diagnostic> report, TelemetryDiagnosticDescriptor telemetryDiagnostic, Location? location, params object?[] args) {
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
