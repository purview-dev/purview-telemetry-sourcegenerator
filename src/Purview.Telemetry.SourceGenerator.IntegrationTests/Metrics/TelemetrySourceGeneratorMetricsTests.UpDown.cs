namespace Purview.Telemetry.SourceGenerator.Metrics;

partial class TelemetrySourceGeneratorMetricsTests {

	[Fact]
	async public Task Generate_GivenBasicUpDown_GeneratesMetrics() {
		// Arrange
		const string basicMetric = @"
using Purview.Telemetry.Metrics;

namespace Testing;

[MeterTarget(""testing-meter"")]
public interface ITestMetrics {
	[UpDownCounterTarget]
	void UpDown(int counterValue, [Tag]int intParam, [Tag]bool boolParam);

	[UpDownCounterTarget]
	void UpDown2([InstrumentMeasurement]int counterValue, [Tag]int intParam, [Tag]bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicMetric);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicObservableUpDown_GeneratesMetrics() {
		// Arrange
		const string basicMetric = @"
using Purview.Telemetry.Metrics;
using System.Diagnostics.Metrics;
using System.Collections.Generic;

namespace Testing;

[MeterTarget(""testing-observable-meter"")]
public interface ITestMetrics {
	[ObservableUpDownCounterTarget]
	void ObservableUpDown(Func<int> f, [Tag]int intParam, [Tag]bool boolParam);

	[ObservableUpDownCounterTarget(ThrowOnAlreadyInitialized = true)]
	void ObservableUpDown2(Func<Measurement<int>> f, [Tag]int intParam, [Tag]bool boolParam);

	[ObservableUpDownCounterTarget]
	void ObservableUpDown3(Func<IEnumerable<Measurement<int>>> f, [Tag]int intParam, [Tag]bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicMetric);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
