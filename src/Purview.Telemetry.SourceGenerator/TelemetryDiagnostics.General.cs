using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator;

partial class TelemetryDiagnostics {
	static public class General {
		readonly static public TelemetryDiagnosticDescriptor FatalExecutionDuringExecution = new(
			Id: "TSG1000",
			Title: "Fatal execution error occurred",
			Description: "Failed to execute the generation stage: {0}",
			Category: Constants.Diagnostics.Usage,
			Severity: DiagnosticSeverity.Error
		);
	}
}
