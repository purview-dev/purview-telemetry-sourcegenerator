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

	[Fact]
	async public Task Generate_GivenInterfaceWithSingleBasicImplicitLogEntry_GenerateLogger() {
		// Arrange
		const string basicAggregate = @"
using Purview.Telemetry.Logging;

namespace Testing;

[LoggerTarget]
public interface ITestLogger {
	void Log(string stringParam, int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicAggregate);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenInterfaceWithExplicitLogLevelAndAnExceptionParameter_GenerateLogger() {
		// Arrange
		const string basicAggregate = @"
using Purview.Telemetry.Logging;

namespace Testing;

[LoggerTarget]
public interface ITestLogger {
	[LogEntry(Level = LogGeneratedLevel.Trace)]
	void Log(string stringParam, int intParam, bool boolParam, Exception exception);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicAggregate);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenInterfaceWithoutExplicitLogLevelAndAnExceptionParameter_GenerateLogger() {
		// Arrange
		const string basicAggregate = @"
using Purview.Telemetry.Logging;

namespace Testing;

[LoggerTarget]
public interface ITestLogger {
	void Log(string stringParam, int intParam, bool boolParam, Exception exception);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicAggregate);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.ScrubInlineGuids());
	}

	[Fact]
	async public Task Generate_GivenInterfaceMoreThanSixParameters_GenerateLogger() {
		// Arrange
		const string basicAggregate = @"
using Purview.Telemetry.Logging;

namespace Testing;

[LoggerTarget]
public interface ITestLogger {
	void Log(string stringParam, int intParam, bool boolParam, string stringParam1, int intParam1, bool boolParam1, string stringParam2, int intParam2, bool boolParam2);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicAggregate);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.ScrubInlineGuids());
	}

	[Fact]
	async public Task Generate_GivenInterfaceMoreThanOneExceptionParameter_GenerateLogger() {
		// Arrange
		const string basicAggregate = @"
using Purview.Telemetry.Logging;

namespace Testing;

[LoggerTarget]
public interface ITestLogger {
	void Log(string stringParam, Exception exception1, Exception exception2);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicAggregate);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.ScrubInlineGuids());
	}
}
