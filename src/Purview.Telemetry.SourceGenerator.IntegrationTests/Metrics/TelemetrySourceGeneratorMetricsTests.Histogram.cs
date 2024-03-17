namespace Purview.Telemetry.SourceGenerator.Metrics;

partial class TelemetrySourceGeneratorMetricsTests {
	[Fact]
	async public Task Generate_GivenBasicHistogram_GeneratesMetrics() {
		// Arrange
		const string basicMetric = @"
using Purview.Telemetry.Metrics;

namespace Testing;

[MeterTarget(""testing-meter"")]
public interface ITestMetrics {
	[Histogram]
	void Histogram(int counterValue, [Tag]int intParam, [Tag]bool boolParam);

	[Histogram]
	void Histogram1([InstrumentMeasurement]int counterValue, [Tag]int intParam, [Tag]bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicMetric);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
