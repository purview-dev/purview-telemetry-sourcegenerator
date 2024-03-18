namespace Purview.Telemetry.SourceGenerator.Metrics;

partial class TelemetrySourceGeneratorMetricsTests {
	[Fact]
	async public Task Generate_GivenBasicObservableGauge_GeneratesMetrics() {
		// Arrange
		const string basicMetric = @"
using Purview.Telemetry.Metrics;
using System.Diagnostics.Metrics;
using System.Collections.Generic;

namespace Testing;

[MeterTarget(""testing-meter"")]
public interface ITestMetrics {
	[ObservableGaugeTarget]
	void ObservableGauge(Func<int> f, [Tag]int intParam, [Tag]bool boolParam);

	[ObservableGaugeTarget(ThrowOnAlreadyInitialized = true)]
	void ObservableGauge2(Func<Measurement<int>> f, [Tag]int intParam, [Tag]bool boolParam);

	[ObservableGaugeTarget]
	void ObservableGauge3(Func<IEnumerable<Measurement<int>>> f, [Tag]int intParam, [Tag]bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicMetric);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
