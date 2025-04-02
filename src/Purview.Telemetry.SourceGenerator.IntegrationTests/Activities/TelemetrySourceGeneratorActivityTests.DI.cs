﻿namespace Purview.Telemetry.SourceGenerator.Activities;

partial class TelemetrySourceGeneratorActivityTests
{
	[Fact]
	public async Task Generate_GivenAssemblyEnableDI_GeneratesActivity()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

[assembly: TelemetryGeneration(GenerateDependencyExtension = true)]

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Activity]
	Activity? Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Event]
	void Event(Activity? activity, [Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		var generationResult = await GenerateAsync(basicActivity, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenInterfaceEnableDI_GeneratesActivity()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

namespace Testing;

[TelemetryGeneration(GenerateDependencyExtension = true)]
[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Activity]
	Activity? Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Event]
	void Event(Activity? activity, [Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		var generationResult = await GenerateAsync(basicActivity, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenDIDisabledAtAssemblyAndInterfaceEnableDI_GeneratesActivity()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

[assembly: TelemetryGeneration(GenerateDependencyExtension = false)]

namespace Testing;

[TelemetryGeneration(GenerateDependencyExtension = true)]
[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Activity]
	Activity? Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Event]
	void Event(Activity? activity, [Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		var generationResult = await GenerateAsync(basicActivity, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenDIEnabledAtAssemblyAndInterfaceDisableDI_GeneratesActivity()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

[assembly: TelemetryGeneration(GenerateDependencyExtension = true)]

namespace Testing;

[TelemetryGeneration(GenerateDependencyExtension = false)]
[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Activity]
	Activity? Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Event]
	void Event(Activity? activity, [Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		var generationResult = await GenerateAsync(basicActivity, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenAssemblyEnableDIAndClassIsPublic_GeneratesActivity()
	{
		// Arrange
		const string basicActivity = @"
using Purview.Telemetry.Activities;
using System.Diagnostics;

[assembly: TelemetryGeneration(GenerateDependencyExtension = true, DependencyInjectionClassIsPublic = true)]

namespace Testing;

[ActivitySource(""testing-activity-source"")]
public interface ITestActivities {
	[Activity]
	Activity? Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Event]
	void Event(Activity? activity, [Baggage]string stringParam, [Tag]int intParam, bool boolParam);
}
";

		// Act
		var generationResult = await GenerateAsync(basicActivity, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
