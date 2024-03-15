using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator;

partial class TelemetryDiagnostics {
	// Start at 2000
	static public class Logging {

		readonly static public TelemetryDiagnosticDescriptor MultipleExceptionsDefined = new(
			Id: "TSG2000",
			Title: "Too many exception parameters",
			Description: "Only a single exceptions parameter is permitted.",
			Category: Constants.Diagnostics.Logging.Usage,
			Severity: DiagnosticSeverity.Error
		);

		readonly static public TelemetryDiagnosticDescriptor MaximumLogEntryParametersExceeded = new(
			Id: "TSG2001",
			Title: "More than 6 parameters",
			Description: $"The maximum number of parameters (excluding optional Exception) is {Constants.Logging.MaxNonExceptionParameters}",
			Category: Constants.Diagnostics.Logging.Usage,
			Severity: DiagnosticSeverity.Error
		);

		readonly static public TelemetryDiagnosticDescriptor InferringErrorLogLevel = new(
			Id: "TSG2002",
			Title: "Inferring error log level",
			Description: "Because an exception parameter was defined and no log level was defined the level was inferred to be Error. Consider explicitly defining the required level.",
			Category: Constants.Diagnostics.Logging.Usage,
			Severity: DiagnosticSeverity.Info
		);
	}
}
