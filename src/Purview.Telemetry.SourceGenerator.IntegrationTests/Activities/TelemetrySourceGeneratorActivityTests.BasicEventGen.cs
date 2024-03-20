namespace Purview.Telemetry.SourceGenerator.Activities;

partial class TelemetrySourceGeneratorActivityTests {
	[Fact]
	async public Task Generate_GivenBasicEventWithActivityParameter_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Event]
	void Event(Activity activity, [Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicEventWithNullableActivityParameter_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Event]
	void Event(Activity? activity, [Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
