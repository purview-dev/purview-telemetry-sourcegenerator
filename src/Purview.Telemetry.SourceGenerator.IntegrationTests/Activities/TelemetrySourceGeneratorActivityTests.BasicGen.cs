namespace Purview.Telemetry.SourceGenerator.Activities;

partial class TelemetrySourceGeneratorActivityTests {
	[Fact]
	async public Task Generate_GivenBasicGen_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;

namespace Testing;

[ActivityTarget]
public interface ITestActivities {
	[Activity]
	void Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[ActivityEvent]
	void Event([Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
