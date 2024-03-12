using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator;

partial class TelemetryDiagnostics {
	// Start at 3000
	static public class Activities {

		readonly static public TelemetryDiagnosticDescriptor BaggageParameterShouldBeString = new(
			Id: "TSG3000",
			Title: "Baggage parameter types can only be strings.",
			Description: "A baggage parameter type must be of type string.",
			Severity: DiagnosticSeverity.Error
		);

		readonly static public TelemetryDiagnosticDescriptor NoActivitySourceSpecified = new(
			Id: "TSG3001",
			Title: "No activity source specified.",
			Description: $"An activity source helps to identify your application and it's telemetry. Defaulting to '{Constants.DefaultActivitySourceName}'.",
			Severity: DiagnosticSeverity.Info
		);
	}
}
