namespace Purview.Telemetry.SourceGenerator.Activities;

partial class TelemetrySourceGeneratorActivityTests {
	[Fact]
	async public Task Generate_GivenAssemblyEnableDI_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;

[assembly: TelemetryGeneration(GenerateDependencyExtension = true)]

namespace Testing;

[ActivitySourceTarget(""testing-activity-source"")]
public interface ITestActivities {
	[ActivityTarget]
	void Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[EventTarget]
	void Event([Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenInterfaceEnableDI_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;

namespace Testing;

[TelemetryGeneration(GenerateDependencyExtension = true)]
[ActivitySourceTarget(""testing-activity-source"")]
public interface ITestActivities {
	[ActivityTarget]
	void Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[EventTarget]
	void Event([Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenDIDisabledAtAssemblyAndInterfaceEnableDI_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;

[assembly: TelemetryGeneration(GenerateDependencyExtension = false)]

namespace Testing;

[TelemetryGeneration(GenerateDependencyExtension = true)]
[ActivitySourceTarget(""testing-activity-source"")]
public interface ITestActivities {
	[ActivityTarget]
	void Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[EventTarget]
	void Event([Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenDIEnabledAtAssemblyAndInterfaceDisableDI_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;

[assembly: TelemetryGeneration(GenerateDependencyExtension = true)]

namespace Testing;

[TelemetryGeneration(GenerateDependencyExtension = false)]
[ActivitySourceTarget(""testing-activity-source"")]
public interface ITestActivities {
	[ActivityTarget]
	void Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[EventTarget]
	void Event([Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
