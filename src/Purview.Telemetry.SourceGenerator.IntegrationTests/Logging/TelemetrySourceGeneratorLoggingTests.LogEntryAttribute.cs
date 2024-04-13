namespace Purview.Telemetry.SourceGenerator.Logging;

partial class TelemetrySourceGeneratorLoggingTests
{
	[Theory]
	[MemberData(nameof(GetEntryNames))]
	public async Task Generate_GivenLogTargetWithEntryName_GenerateLogger(string logTargetName)
	{
		// Arrange
		string basicLogger = @$"
using Purview.Telemetry.Logging;

namespace Testing;

[Logger]
public interface ITestLogger {{
	[Log(Name = ""{logTargetName}"")]
	void Log(string stringParam, int intParam, bool boolParam);
}}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.UseHashedParameters(logTargetName));
	}

	[Theory]
	[MemberData(nameof(GetPrefixAndEntryNames))]
	public async Task Generate_GivenLogTargetWithPrefixAndEntryName_GenerateLogger(string type, string logTargetName)
	{
		// Arrange
		string prefixType = type switch
		{
			"Custom" => type + ", CustomPrefix = \"custom-prefix\"",
			_ => type
		};

		string basicLogger = @$"
using Purview.Telemetry.Logging;

namespace Testing;

[Logger(PrefixType = LogPrefixType.{prefixType})]
public interface ITestLogger {{
	[Log(Name = ""{logTargetName}"")]
	void Log(string stringParam, int intParam, bool boolParam);
}}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.UseHashedParameters(prefixType, logTargetName));
	}

	public static TheoryData<string, string> GetPrefixAndEntryNames()
	{
		TheoryData<string, string> data = [];

		string[] prefixes = ["Default", "Custom", "Interface", "Class", "NoSuffix"];

		foreach (string type in prefixes)
		{
			foreach (string entryName in TestEntryNames)
			{
				data.Add(type, entryName);
			}
		}

		return data;
	}

	public static TheoryData<string> GetEntryNames()
	{
		TheoryData<string> data = [];

		foreach (string entryName in TestEntryNames)
		{
			data.Add(entryName);
		}

		return data;
	}

	static readonly string[] TestEntryNames = [
		"LogNameSetViaLogTargetAttribute",
		"CustomLogNameSetViaLogTargetAttribute",
		"123",
		"custom-log-entry-name"
	];
}
