namespace Purview.Telemetry.SourceGenerator;

partial class TelemetrySourceGeneratorTests {
	[Fact]
	async public Task Generate_GivenInterfaceWithSingleBasicExplicitLogEntry_GenerateLogger() {
		// Arrange
		const string basicAggregate = @"
using Purview.Telemetry.Logging;

namespace Testing;

[LoggerTarget]
public interface ITestLogger {
	[LogEntry]
	void Log(string stringParam, int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicAggregate);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
