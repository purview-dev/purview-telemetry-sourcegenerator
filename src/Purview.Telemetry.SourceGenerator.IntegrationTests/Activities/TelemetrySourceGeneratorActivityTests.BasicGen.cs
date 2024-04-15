namespace Purview.Telemetry.SourceGenerator.Activities;

partial class TelemetrySourceGeneratorActivityTests
{
	[Fact]
	public async Task Generate_GivenBasicGen_GeneratesActivity()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Activity]
	void Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Event]
	void Event([Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenWithNonStringBaggage_RaisesDiagnosticAndGenerates()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Activity]
	void Activity([Baggage]string stringNonNullParam, [Baggage]int intParam, [Baggage]bool boolParam);

	[Event]
	void Event([Baggage]string? stringNullableParam, [Baggage]int? intParam, [Baggage]bool? boolParam);

	[Context]
	void Context([Baggage]object? objectParam, [Baggage]string stringNonNullParam, [Baggage]float? floatParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult, s => s.ScrubInlineGuids(), validateNonEmptyDiagnostics: true);
	}

	[Fact]
	public async Task Generate_GivenBasicGenWithReturningActivity_GeneratesActivity()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Activity]
	Activity Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Event]
	Activity Event([Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenBasicGenWithReturningNullableActivity_GeneratesActivity()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Activity]
	Activity Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Activity]
	Activity? ActivityWithNullableReturnActivity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenBasicGenWithNullableParams_GeneratesActivity()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Activity]
	Activity? Activity([Baggage]string? stringParam, [Tag]int? intParam, bool? boolParam);

	[Activity]
	Activity? ActivityWithNullableParams([Baggage]string? stringParam, [Tag]int? intParam, bool? boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
