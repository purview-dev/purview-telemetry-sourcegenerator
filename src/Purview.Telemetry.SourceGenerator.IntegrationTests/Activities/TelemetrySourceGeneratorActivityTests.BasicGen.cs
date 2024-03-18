namespace Purview.Telemetry.SourceGenerator.Activities;

partial class TelemetrySourceGeneratorActivityTests {
	[Fact]
	async public Task Generate_GivenBasicGen_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;

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

[ActivitySourceTarget(""testing-activity-source"")]
public interface ITestActivities {
	[ActivityTarget]
	Activity Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[EventTarget]
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

[ActivitySourceTarget(""testing-activity-source"")]
public interface ITestActivities {
	[ActivityTarget]
	Activity? Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[ActivityTarget]
	Activity? ActivityWithNullableReturnActivity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicGenWithNullableParams_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySourceTarget(""testing-activity-source"")]
public interface ITestActivities {
	[ActivityTarget]
	Activity? Activity([Baggage]string? stringParam, [Tag]int? intParam, bool? boolParam);

	[ActivityTarget]
	Activity? ActivityWithNullableParams([Baggage]string? stringParam, [Tag]int? intParam, bool? boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
