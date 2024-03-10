using Purview.Telemetry.Logging;

namespace Purview.Telemetry.SourceGenerator.Logging;
partial class TelemetrySourceGeneratorTests {
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
		await TestHelpers.Verify(generationResult);
	}

	[Theory]
	[MemberData(nameof(GetPrefixAndEntryNames))]
	async public Task Generate_GivenLogEntryWithPrefixAndEntryName_GenerateLogger(LogPrefixType type, string logEntryName) {
		// Arrange
		string prefixType = type switch {
			LogPrefixType.Default => "",
			LogPrefixType.Custom => ", CustomPrefix = \"custom-prefix\"",
			_ => throw new NotImplementedException(),
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
		await TestHelpers.Verify(generationResult);
	}

	static public TheoryData<LogPrefixType, string> GetPrefixAndEntryNames() {
		TheoryData<LogPrefixType, string> data = [];

		TheoryData<string> entryNames = GetEntryNames();
		foreach (LogPrefixType type in Enum.GetValues<LogPrefixType>()) {
			foreach (string entryName in entryNames.Cast<string>()) {
				data.Add(type, entryName);
			}
		}

		return data;
	}

	static public TheoryData<string> GetEntryNames() {
		TheoryData<string> data = [];

		data.Add("LogNameSetViaLogEntryAttribute");
		data.Add("CustomLogNameSetViaLogEntryAttribute");
		data.Add("123");
		data.Add("custom-log-entry-name");

		return data;
	}
}

