namespace Purview.Telemetry.SourceGenerator;

partial class TelemetrySourceGeneratorTests {
	[Fact]
	async public Task Generate_GivenNoNamespace_GeneratesTelemetry() {
		// Arrange
		const string basicTelemetry = @"
using Purview.Telemetry.Activities;
using Purview.Telemetry.Logging;
using Purview.Telemetry.Metrics;

[ActivitySource(""activity-source"")]
[Logger]
[Meter]
public interface ITestTelemetry
{
	[Activity]
	void Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Event]
	void Event([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Context]
	void Context([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Log]
	void Log([Tag]int intParam, bool boolParam);

	[Log]
	IDisposable? LogScope([Tag]int intParam, bool boolParam);

	[Counter]
	bool Counter(int counterValue, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicTelemetry);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicTelemetryGen_GeneratesTelemetry() {
		// Arrange
		const string basicTelemetry = @"
using Purview.Telemetry.Activities;
using Purview.Telemetry.Logging;
using Purview.Telemetry.Metrics;

namespace Testing;

[ActivitySource(""activity-source"")]
[Logger]
[Meter]
public interface ITestTelemetry
{
	[Activity]
	void Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Event]
	void Event([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Context]
	void Context([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Log]
	void Log([Tag]int intParam, bool boolParam);

	[Log]
	IDisposable? LogScope([Tag]int intParam, bool boolParam);

	[Counter]
	bool Counter(int counterValue, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicTelemetry);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicEventWithException_GeneratesTelemetry() {
		// Arrange
		const string basicTelemetry = @"
using Purview.Telemetry.Activities;

namespace Testing;

[ActivitySource(""activity-source"")]
public interface ITestTelemetry
{
	[Event]
	void Event([Baggage]string stringParam, [Tag]int intParam, bool boolParam, Exception anException);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicTelemetry);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicEventWithExceptionAndEscape_GeneratesTelemetry() {
		// Arrange
		const string basicTelemetry = @"
using Purview.Telemetry.Activities;

namespace Testing;

[ActivitySource(""activity-source"")]
public interface ITestTelemetry
{
	[Event]
	void Event([Baggage]string stringParam, [Tag]int intParam, bool boolParam, Exception anException, [Escape]bool escape);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicTelemetry);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicEventWithExceptionAndDisabledOTelExceptionRulesAndEscape_GeneratesTelemetry() {
		// Arrange
		const string basicTelemetry = @"
using Purview.Telemetry.Activities;

namespace Testing;

[ActivitySource(""activity-source"")]
public interface ITestTelemetry
{
	[Event(UseRecordExceptionRules = false)]
	void EventMethod([Baggage]string stringParam, [Tag]int intParam, bool boolParam, Exception anException, [Escape]bool escape);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicTelemetry);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicEventWithExplicitExceptionAndNamedExceptionAndRulesAreFalse_GeneratesTelemetry() {
		// Arrange
		const string basicTelemetry = @"
using Purview.Telemetry.Activities;

namespace Testing;

[ActivitySource(""activity-source"")]
public interface ITestTelemetry
{
	[Event(name: ""exception"", UseRecordExceptionRules = false)]
	void EventMethod([Baggage]string stringParam, [Tag]int intParam, bool boolParam, Exception anException, [Escape]bool escape);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicTelemetry);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenBasicEventWithExplicitExceptionAndEventIsNamedExceptionAndRulesAreTrue_GeneratesTelemetry() {
		// Arrange
		const string basicTelemetry = @"
using Purview.Telemetry.Activities;

namespace Testing;
	
[ActivitySource(""activity-source"")]
public interface ITestTelemetry
{
	[Event(name: ""exception"", UseRecordExceptionRules = true)]
	void EventMethod([Baggage]string stringParam, [Tag]int intParam, bool boolParam, Exception anException, [Escape]bool escape);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicTelemetry);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	async public Task Generate_GivenDuplicateTelemetryGen_GeneratesDiagnostics() {
		// Arrange
		const string basicTelemetry = @"
using Purview.Telemetry.Activities;
using Purview.Telemetry.Logging;
using Purview.Telemetry.Metrics;

namespace Testing;

[ActivitySource(""activity-source"")]
[Logger]
[Meter]
public interface ITestTelemetry
{
	[Activity]
	[Log]
	void Activity([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Event]
	[Counter]
	void Event([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Context]
	[Activity]
	void Context([Baggage]string stringParam, [Tag]int intParam, bool boolParam);

	[Log]
	[Counter]
	void Log([Tag]int intParam, bool boolParam);

	[Log]
	[Activity]
	IDisposable? LogScope([Tag]int intParam, bool boolParam);

	[Counter]
	[Event]
	[Log]
	void Counter(int counterValue, [Tag]int intParam, bool boolParam);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicTelemetry);

		// Assert
		await TestHelpers.Verify(generationResult, s => s.ScrubInlineGuids(), validateNonEmptyDiagnostics: true, validationCompilation: false);
	}
}
