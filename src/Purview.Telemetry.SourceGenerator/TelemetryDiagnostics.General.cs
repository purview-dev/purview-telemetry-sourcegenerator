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

		readonly static public TelemetryDiagnosticDescriptor InferenceNotSupportedWithMultiTargeting = new(
			Id: "TSG1001",
			Title: "Inferring generation targets is not supported when using multi-target generation.",
			Description: $"When using multiple generation targets - Activities, Logs or Metrics, each method must be either excluded or have an explicit generation target: " +
				$"{Constants.Activities.ActivityAttribute.Name}, {Constants.Activities.EventAttribute.Name}, {Constants.Activities.ContextAttribute.Name}, {Constants.Logging.LogAttribute.Name}, " +
				$"{Constants.Metrics.CounterAttribute.Name}, {Constants.Metrics.HistogramAttribute.Name}, {Constants.Metrics.UpDownCounterAttribute.Name}, " +
				$"{Constants.Metrics.ObservableCounterAttribute.Name}, {Constants.Metrics.ObservableGaugeAttribute.Name} or {Constants.Metrics.ObservableUpDownCounterAttribute.Name}.",
			Category: Constants.Diagnostics.Usage,
			Severity: DiagnosticSeverity.Error
		);

		readonly static public TelemetryDiagnosticDescriptor MultiGenerationTargetsNotSupported = new(
			Id: "TSG1002",
			Title: "Only single generation types are supported.",
			Description: $"Only a single generation target types (Activities, Logs or Metrics) is supported on methods. Use one of the following: " +
				$"{Constants.Activities.ActivityAttribute.Name}, {Constants.Activities.EventAttribute.Name}, {Constants.Activities.ContextAttribute.Name}, {Constants.Logging.LogAttribute.Name}, " +
				$"{Constants.Metrics.CounterAttribute.Name}, {Constants.Metrics.HistogramAttribute.Name}, {Constants.Metrics.UpDownCounterAttribute.Name}, " +
				$"{Constants.Metrics.ObservableCounterAttribute.Name}, {Constants.Metrics.ObservableGaugeAttribute.Name} or {Constants.Metrics.ObservableUpDownCounterAttribute.Name}.",
			Category: Constants.Diagnostics.Usage,
			Severity: DiagnosticSeverity.Error
		);
	}
}
