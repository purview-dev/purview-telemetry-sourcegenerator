using Xunit.Abstractions;

namespace Purview.Telemetry.SourceGenerator.Logging;

public partial class TelemetrySourceGeneratorLoggingTests(ITestOutputHelper testOutputHelper) : IncrementalSourceGeneratorTestBase<TelemetrySourceGenerator>(testOutputHelper)
{
	[Theory]
	[MemberData(nameof(TelemetrySourceGeneratorTests.BasicGenericParameters), MemberType = typeof(TelemetrySourceGeneratorTests))]
	public async Task Generate_GivenMethodWithBasicGenericParams_GeneratesEntryCorrectly(string parameterType)
	{
		// Arrange
		var basicLogger = @$"
using Purview.Telemetry.Logging;

namespace Testing;

[Logger]
public interface ITestLogger {{
	void LogEntryWithGenericTypeParam({parameterType} paramName);
}}
";

		// Act
		var generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult, c => c
			.ScrubInlineGuids()
			.UseParameters(parameterType)
		);
	}
}
