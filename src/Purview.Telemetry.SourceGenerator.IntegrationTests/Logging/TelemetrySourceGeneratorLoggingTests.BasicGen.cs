namespace Purview.Telemetry.SourceGenerator.Logging;

partial class TelemetrySourceGeneratorLoggingTests {
	[Fact]
	async public Task Generate_GivenInterfaceWithSingleBasicExplicitLogEntry_GenerateLogger() {
		// Arrange
		const string basicLogger = @"
using Purview.Telemetry.Logging;

namespace Testing;

[Logger]
public interface ITestLogger {
	[Log]
	void Log(string stringParam, int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenInterfaceWithSingleBasicImplicitLogEntry_GenerateLogger() {
		// Arrange
		const string basicLogger = @"
using Purview.Telemetry.Logging;

namespace Testing;

[Logger]
public interface ITestLogger {
	void Log(string stringParam, int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Theory]
	[InlineData("Level = Microsoft.Extensions.Logging.LogLevel.Trace")]
	[InlineData("level: Microsoft.Extensions.Logging.LogLevel.Trace")]
	[InlineData("Microsoft.Extensions.Logging.LogLevel.Trace")]
	async public Task Generate_GivenInterfaceWithExplicitLogLevelAndAnExceptionParameter_GenerateLogger(string level) {
		// Arrange
		string basicLogger = @$"
using Purview.Telemetry.Logging;

namespace Testing;

[Logger]
public interface ITestLogger {{
	[Log({level})]
	void Log(string stringParam, int intParam, bool boolParam, Exception exception);
}}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.ScrubInlineGuids().UseHashedParameters(level));
	}

	[Fact]
	async public Task Generate_GivenInterfaceWithoutExplicitLogLevelAndAnExceptionParameter_GenerateLogger() {
		// Arrange
		const string basicLogger = @"
using Purview.Telemetry.Logging;

namespace Testing;

[Logger]
public interface ITestLogger {
	void Log(string stringParam, int intParam, bool boolParam, Exception exception);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.ScrubInlineGuids(), validateNonEmptyDiagnostics: true);
	}

	[Fact]
	async public Task Generate_GivenInterfaceMoreThanSixParameters_RaisesDiagnostic() {
		// Arrange
		const string basicLogger = @"
using Purview.Telemetry.Logging;

namespace Testing;

[Logger]
public interface ITestLogger {
	void Log(string stringParam, int intParam, bool boolParam, string stringParam1, int intParam1, bool boolParam1, string stringParam2, int intParam2, bool boolParam2);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.ScrubInlineGuids(),
			validateNonEmptyDiagnostics: true,
			validationCompilation: false
		);
	}

	[Fact]
	async public Task Generate_GivenInterfaceMoreThanOneExceptionParameter_RaisesDiagnostic() {
		// Arrange
		const string basicLogger = @"
using Purview.Telemetry.Logging;

namespace Testing;

[Logger]
public interface ITestLogger {
	void Log(string stringParam, Exception exception1, Exception exception2);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.ScrubInlineGuids(),
			validateNonEmptyDiagnostics: true,
			validationCompilation: false
		);
	}

	[Fact]
	async public Task Generate_GivenMethodReturnsIDisposable_GeneratesScopedLogEntry() {
		// Arrange
		const string basicLogger = @"
using Purview.Telemetry.Logging;

namespace Testing;

[Logger]
public interface ITestLogger {
	IDisposable Log();
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.ScrubInlineGuids());
	}

	[Fact]
	async public Task Generate_GivenMethodWithParamsAndExceptionReturnsIDisposable_GeneratesScopedLogEntry() {
		// Arrange
		const string basicLogger = @"
using Purview.Telemetry.Logging;

namespace Testing;

[Logger]
public interface ITestLogger {
	IDisposable Log(string stringParam, int intParam, Exception exception);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.ScrubInlineGuids());
	}

	[Fact]
	async public Task Generate_GivenMethodWithParamsReturnsIDisposable_GeneratesScopedLogEntry() {
		// Arrange
		const string basicLogger = @"
using Purview.Telemetry.Logging;

namespace Testing;

[Logger]
public interface ITestLogger {
	IDisposable Log(string stringParam, int intParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.ScrubInlineGuids());
	}
}
