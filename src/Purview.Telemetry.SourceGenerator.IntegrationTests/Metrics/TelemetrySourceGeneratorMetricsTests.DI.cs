namespace Purview.Telemetry.SourceGenerator.Metrics;

partial class TelemetrySourceGeneratorMetricsTests {
	[Fact]
	async public Task Generate_GivenAssemblyEnableDI_GeneratesMetrics() {
		// Arrange
		const string basicMetric = @"
using Purview.Telemetry;
using Purview.Telemetry.Metrics;

[assembly: TelemetryGeneration(GenerateDependencyExtension = true)]

namespace Testing;

[MeterTarget(""testing-meter"")]
public interface ITestMetrics {
	[CounterTarget]
	void Counter(int counterValue, [Tag]int intParam, [Tag]bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicMetric, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenInterfaceEnableDI_GeneratesMetrics() {
		// Arrange
		const string basicMetric = @"
using Purview.Telemetry;
using Purview.Telemetry.Metrics;

namespace Testing;

[MeterTarget(""testing-meter"")]
[TelemetryGeneration(GenerateDependencyExtension = true)]
public interface ITestMetrics {
	[CounterTarget]
	void Counter(int counterValue, [Tag]int intParam, [Tag]bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicMetric, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenDIDisabledAtAssemblyAndInterfaceEnableDI_GeneratesMetrics() {
		// Arrange
		const string basicMetric = @"
using Purview.Telemetry;
using Purview.Telemetry.Metrics;

[assembly: TelemetryGeneration(GenerateDependencyExtension = false)]

namespace Testing;

[MeterTarget(""testing-meter"")]
[TelemetryGeneration(GenerateDependencyExtension = true)]
public interface ITestMetrics {
	[CounterTarget]
	void Counter(int counterValue, [Tag]int intParam, [Tag]bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicMetric, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenDIEnabledAtAssemblyAndInterfaceDisabledDI_GeneratesMetrics() {
		// Arrange
		const string basicMetric = @"
using Purview.Telemetry;
using Purview.Telemetry.Metrics;

[assembly: TelemetryGeneration(GenerateDependencyExtension = true)]

namespace Testing;

[MeterTarget(""testing-meter"")]
[TelemetryGeneration(GenerateDependencyExtension = false)]
public interface ITestMetrics {
	[CounterTarget]
	void Counter(int counterValue, [Tag]int intParam, [Tag]bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicMetric, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
