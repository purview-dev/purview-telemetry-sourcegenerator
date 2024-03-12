using Purview.Telemetry.Logging;

namespace Purview.Telemetry.SourceGenerator.Logging;
partial class TelemetrySourceGeneratorLoggingTests {
	[Theory]
	[MemberData(nameof(GetEntryNames))]
	async public Task Generate_GivenLogEntryWithEntryName_GenerateLogger(string logEntryName) {
		// Arrange
		string basicLogger = @$"
using Purview.Telemetry.Logging;

namespace Testing;

[LoggerTarget]
public interface ITestLogger {{
	[LogEntry(EntryName = ""{logEntryName}"")]
	void Log(string stringParam, int intParam, bool boolParam);
}}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.UseHashedParameters(logEntryName));
	}

	[Theory]
	[MemberData(nameof(GetPrefixAndEntryNames))]
	async public Task Generate_GivenLogEntryWithPrefixAndEntryName_GenerateLogger(LogPrefixType type, string logEntryName) {
		// Arrange
		string prefixType = type switch {
			LogPrefixType.Default => "",
			LogPrefixType.Custom => ", CustomPrefix = \"custom-prefix\"",
			_ => ""
		};

		string basicLogger = @$"
using Purview.Telemetry.Logging;

namespace Testing;

[LoggerTarget(PrefixType = LogPrefixType.{type}{prefixType})]
public interface ITestLogger {{
	[LogEntry(EntryName = ""{logEntryName}"")]
	void Log(string stringParam, int intParam, bool boolParam);
}}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.UseHashedParameters(type, logEntryName));
	}

	static public TheoryData<LogPrefixType, string> GetPrefixAndEntryNames() {
		TheoryData<LogPrefixType, string> data = [];

		foreach (LogPrefixType type in Enum.GetValues<LogPrefixType>()) {
			foreach (string entryName in _testEntryNames) {
				data.Add(type, entryName);
			}
		}

		return data;
	}

	static public TheoryData<string> GetEntryNames() {
		TheoryData<string> data = [];

		foreach (string entryName in _testEntryNames) {
			data.Add(entryName);
		}

		return data;
	}

	readonly static string[] _testEntryNames = [
		"LogNameSetViaLogEntryAttribute",
		"CustomLogNameSetViaLogEntryAttribute",
		"123",
		"custom-log-entry-name"
	];
}

