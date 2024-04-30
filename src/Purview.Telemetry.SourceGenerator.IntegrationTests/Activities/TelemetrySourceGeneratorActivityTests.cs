using Xunit.Abstractions;

namespace Purview.Telemetry.SourceGenerator.Activities;

public partial class TelemetrySourceGeneratorActivityTests(ITestOutputHelper testOutputHelper) : IncrementalSourceGeneratorTestBase<TelemetrySourceGenerator>(testOutputHelper)
{
	[Theory]
	[MemberData(nameof(TelemetrySourceGeneratorTests.BasicGenericParameters), MemberType = typeof(TelemetrySourceGeneratorTests))]
	public async Task Generate_GivenMethodWithBasicGenericParams_GeneratesEntryCorrectly(string parameterType)
	{
		// Arrange
		var basicActivity = @$"
using Purview.Telemetry.Activities;

namespace Testing;

[ActivitySource]
public interface ITestActivities 
{{
	[Activity]
	void Activity({parameterType} paramName);

	[Event]
	void Event({parameterType} paramName);

	[Context]
	void Context({parameterType} paramName);
}}
";

		// Act
		var generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult, c => c
			.ScrubInlineGuids()
			.UseHashedParameters(parameterType)
		);
	}
}
