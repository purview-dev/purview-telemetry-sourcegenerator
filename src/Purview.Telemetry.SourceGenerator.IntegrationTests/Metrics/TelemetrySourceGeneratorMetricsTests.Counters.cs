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
	[CounterTarget]
	void Counter(int counterValue, [Tag]int intParam, [Tag]bool boolParam);

	[CounterTarget]
	void Counter2(byte counterValue, [Tag]int intParam, [Tag]bool boolParam);

	[CounterTarget]
	void Counter3(long counterValue, [Tag]int intParam, [Tag]bool boolParam);

	[CounterTarget]
	void Counter4([InstrumentMeasurement]short counterValue, [Tag]int intParam, [Tag]bool boolParam);

	[CounterTarget]
	void Counter5([InstrumentMeasurement]double counterValue, [Tag]int intParam, [Tag]bool boolParam);

	[CounterTarget]
	void Counter6([InstrumentMeasurement]float counterValue, [Tag]int intParam, [Tag]bool boolParam);

	[CounterTarget]
	void Counter7([InstrumentMeasurement]decimal counterValue, [Tag]int intParam, [Tag]bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicMetric);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicCountersWithAutoIncrement_GeneratesMetrics() {
		// Arrange
		const string basicMetric = @"
using Purview.Telemetry.Metrics;

namespace Testing;

[MeterTarget(""testing-meter"")]
public interface ITestMetrics {
	[CounterTarget(autoIncrement: true)]
	void Counter1([Tag]int intParam, [Tag]bool boolParam);

	[CounterTarget(AutoIncrement = true)]
	void Counter2([Tag]int intParam, [Tag]bool boolParam);
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
using System.Diagnostics.Metrics;
using System.Collections.Generic;

namespace Testing;

[MeterTarget(""testing-observable-meter"")]
public interface ITestMetrics {
	[ObservableCounterTarget]
	void ObservableCounter(Func<int> f, [Tag]int intParam, [Tag]bool boolParam);

	[ObservableCounterTarget(ThrowOnAlreadyInitialized = true)]
	void ObservableCounter2(Func<Measurement<int>> f, [Tag]int intParam, [Tag]bool boolParam);

	[ObservableCounterTarget]
	void ObservableCounter3(Func<IEnumerable<Measurement<int>>> f, [Tag]int intParam, [Tag]bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicMetric);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
