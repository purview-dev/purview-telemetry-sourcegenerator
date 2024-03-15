using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator;

partial class TelemetryDiagnostics {
	// Starts at 4000
	static public class Metrics {
		readonly static public TelemetryDiagnosticDescriptor NoInstrumentDefined = new(
			Id: "TSG4000",
			Title: "No instrument defined.",
			Description: "Either exclude this method, or define an instrument.",
			Category: Constants.Diagnostics.Metrics.Usage,
			Severity: DiagnosticSeverity.Error
		);

		readonly static public TelemetryDiagnosticDescriptor DoesNotReturnVoid = new(
			Id: "TSG4001",
			Title: "Must return void.",
			Description: "Instrument methods can only return void.",
			Category: Constants.Diagnostics.Metrics.Usage,
			Severity: DiagnosticSeverity.Error
		);

		readonly static public TelemetryDiagnosticDescriptor AutoIncrementCountAndMeasurementParam = new(
			Id: "TSG4002",
			Title: "Auto increment counter and measurement defined.",
			Description: "Auto increment counter and a measurement parameter are defined, either remove the parameter or disable auto increment.",
			Category: Constants.Diagnostics.Metrics.Usage,
			Severity: DiagnosticSeverity.Error
		);
	}
}
