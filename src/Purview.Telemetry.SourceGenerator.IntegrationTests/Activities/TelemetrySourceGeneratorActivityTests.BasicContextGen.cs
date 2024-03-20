namespace Purview.Telemetry.SourceGenerator.Activities;

partial class TelemetrySourceGeneratorActivityTests {
	[Fact]
	async public Task Generate_GivenBasicContextGen_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Context]
	void Context([Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicContextGenWithReturningActivity_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Context]
	Activity Context([Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicContextGenWithReturningNullableActivity_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Context]
	Activity Context([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Context]
	Activity? ContextWithNullableReturnActivity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicContextGenWithNullableParams_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Context]
	Activity Context([Baggage]string? stringParam, [Tag]int? intParam, bool? boolParam);

	[Context]
	Activity? ContextWithNullableParams([Baggage]string? stringParam, [Tag]int? intParam, bool? boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicContextGenWithActivity_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Context]
	Activity Context(Activity activityParameter, [Baggage]string? stringParam, [Tag]int? intParam, bool? boolParam);

	[Context]
	Activity? ContextWithNullableParams(Activity? activityParameter, [Baggage]string? stringParam, [Tag]int? intParam, bool? boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicContextGenWithActivityAndNoReturn_GeneratesActivity() {
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Context]
	void Context(Activity activityParameter, [Baggage]string? stringParam, [Tag]int? intParam, bool? boolParam);

	[Context]
	void ContextWithNullableParams(Activity? activityParameter, [Baggage]string? stringParam, [Tag]int? intParam, bool? boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
