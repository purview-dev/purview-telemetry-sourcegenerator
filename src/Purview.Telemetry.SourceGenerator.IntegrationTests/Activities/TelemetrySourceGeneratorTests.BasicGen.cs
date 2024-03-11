namespace Purview.Telemetry.SourceGenerator.Activities;

partial class TelemetrySourceGeneratorTests {
	[Fact]
	async public Task Generate_GivenBasicGen_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activity;

namespace Testing;

[ActivityTarget]
public interface ITestActivities {
	[Activity]
	void Activity(string stringParam, int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
