namespace Purview.Telemetry.SourceGenerator.Activities;

partial class TelemetrySourceGeneratorActivityTests
{
	[Fact]
	public async Task Generate_GivenBasicEventWithActivityParameter_GeneratesEvent()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities 
{
	[Activity]
	System.Diagnostics.Activity? Activity();

	[Event]
	void Event(Activity activity, [Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		var generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenBasicEventWithNullableActivityParameter_GeneratesEvent()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities 
{
	[Activity]
	System.Diagnostics.Activity? Activity();

	[Event]
	void Event(Activity? activity, [Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		var generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenBasicEventStatusCodeParameterSetToOk_GeneratesEvent()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities 
{
	[Activity]
	System.Diagnostics.Activity? Activity();

	[Event(ActivityStatusCode.Ok)]
	void Event(Activity? activity);
}
";

		// Act
		var generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenBasicEventStatusCodeParameterSetToError_GeneratesEvent()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities 
{
	[Activity]
	System.Diagnostics.Activity? Activity();

	[Event(ActivityStatusCode.Error)]
	void Event(Activity? activity);
}
";

		// Act
		var generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenBasicEventStatusCodeParameterSetToErrorWithException_GeneratesEvent()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities 
{
	[Activity]
	System.Diagnostics.Activity? Activity();

	[Event(ActivityStatusCode.Error)]
	void Event(Activity? activity, Exception exception);
}
";

		// Act
		var generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenBasicEventStatusCodeParameterSetToErrorWithStatusDescriptionOnEventAttribute_GeneratesEvent()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities 
{
	[Activity]
	System.Diagnostics.Activity? Activity();

	[Event(ActivityStatusCode.Error, StatusDescription = ""This is a Test"")]
	void Event(Activity? activity, Exception exception);
}
";

		// Act
		var generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenBasicEventStatusCodeParameterSetToErrorWithStatusDescriptionOnParameter_GeneratesEvent()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities 
{
	[Activity]
	System.Diagnostics.Activity? Activity();

	[Event(ActivityStatusCode.Error)]
	void Event(Activity? activity, [StatusDescription]string? statusDescription);

	[Event(ActivityStatusCode.Error)]
	void Event2(Activity? activity, [StatusDescription]string statusDescription_another);
}
";

		// Act
		var generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
