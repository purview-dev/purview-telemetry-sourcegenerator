namespace Purview.Telemetry.SourceGenerator.Metrics;

partial class TelemetrySourceGeneratorMetricsTests {
	[Fact]
	async public Task Generate_GivenBasicCounters_GeneratesMetrics() {
		// Arrange
		const string basicMetric = @"
using Purview.Telemetry.Metrics;

namespace Testing;

[MeterTarget(""testing-meter"")]
public interface ITestMetrics {
	[Counter]
	void Counter(int counterValue, [Tag]int intParam, [Tag]bool boolParam);

	[Counter(autoIncrement: true)]
	void Counter2([Tag]int intParam, [Tag]bool boolParam);

	[Counter(AutoIncrement = true)]
	void Counter3([Tag]int intParam, [Tag]bool boolParam);

	[Counter]
	void Counter4([InstrumentMeasurement]int counterValue, [Tag]int intParam, [Tag]bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicMetric);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicObservableCounters_GeneratesMetrics() {
		// Arrange
		const string basicMetric = @"
using Purview.Telemetry.Metrics;

namespace Testing;

[MeterTarget(""testing-observable-meter"")]
public interface ITestMetrics {
	[ObservableCounter]
	void ObservableCounter(Func<int> f, [Tag]int intParam, [Tag]bool boolParam);

	[ObservableCounter(ThrowOnAlreadyInitialized = true)]
	void ObservableCounter2(Func<Measurement<int>> f, [Tag]int intParam, [Tag]bool boolParam);

	[ObservableCounter]
	void ObservableCounter3(Func<IEnumerable<Measurement<int>>> f, [Tag]int intParam, [Tag]bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicMetric);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
