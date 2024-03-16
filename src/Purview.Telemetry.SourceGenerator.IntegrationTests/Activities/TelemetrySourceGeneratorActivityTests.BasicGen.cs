namespace Purview.Telemetry.SourceGenerator.Activities;

partial class TelemetrySourceGeneratorActivityTests {
	[Fact]
	async public Task Generate_GivenBasicGen_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;

namespace Testing;

[ActivityTarget(""testing-activity-source"")]
public interface ITestActivities {
	[ActivityGen]
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

	[Fact]
	async public Task Generate_GivenBasicGenWithReturningActivity_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivityTarget(""testing-activity-source"")]
public interface ITestActivities {
	[ActivityGen]
	Activity Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[ActivityEvent]
	Activity Event([Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicGenWithReturningNullableActivity_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivityTarget(""testing-activity-source"")]
public interface ITestActivities {
	[ActivityGen]
	Activity? Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[ActivityGen]
	Activity? Event([Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
