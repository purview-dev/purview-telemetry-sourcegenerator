﻿namespace Purview.Telemetry.SourceGenerator.Metrics;

partial class TelemetrySourceGeneratorMetricsTests {
	[Fact]
	async public Task Generate_GivenObservablesReturnBool_GeneratesMetrics() {
		// Arrange
		const string basicMetric = @"
using Purview.Telemetry.Metrics;

namespace Testing;

[MeterTarget(""testing-meter"")]
public interface ITestMetrics {
	[ObservableCounter]
	bool Counter(Func<int> counterValue, [Tag]int intParam, [Tag]bool boolParam);

	[ObservableGauge]
	bool Gauge(Func<int> counterValue, [Tag]int intParam, [Tag]bool boolParam);

	[ObservableUpDownCounter]
	bool UpDown(Func<int> counterValue, [Tag]int intParam, [Tag]bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicMetric);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenObservablesReturnBoolAndThrowsOnAlreadyInitialized_GeneratesMetrics() {
		// Arrange
		const string basicMetric = @"
using Purview.Telemetry.Metrics;

namespace Testing;

[MeterTarget(""testing-meter"")]
public interface ITestMetrics {
	[ObservableCounter(ThrowOnAlreadyInitialized = true)]
	bool Counter(Func<int> counterValue, [Tag]int intParam, [Tag]bool boolParam);

	[ObservableGauge(ThrowOnAlreadyInitialized = true)]
	bool Gauge(Func<int> counterValue, [Tag]int intParam, [Tag]bool boolParam);

	[ObservableUpDownCounter(ThrowOnAlreadyInitialized = true)]
	bool UpDown(Func<int> counterValue, [Tag]int intParam, [Tag]bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicMetric);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
