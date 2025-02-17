namespace Purview.Telemetry.SourceGenerator.Logging;
partial class TelemetrySourceGeneratorLoggingGen2Tests
{
	[Theory]
	[MemberData(nameof(ExpandableArrays))]
	public async Task Generate_GivenMethodWithExpandableArrayOrEnumerable_GeneratesCorrectElements(string parameter)
	{
		// Arrange
		var basicLogger = @$"
using Purview.Telemetry.Logging;
using Microsoft.Extensions.Logging;

namespace Testing;

[Logger]
public interface ITestLogger
{{
	void Log({parameter});
}}
";

		// Act
		var generationResult = await GenerateAsync(basicLogger, includeLoggerTypes: IncludeLoggerTypes.Telemetry);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	public static TheoryData<string> ExpandableArrays
	{
		get
		{
			TheoryData<string> data = [];

			data.Add("[ExpandEnumerable]string[] paramValue");
			data.Add("[ExpandEnumerable]System.Collections.Generic.IEnumerable<string> paramValue");
			data.Add("[ExpandEnumerable]System.Collections.Generic.ICollection<string> paramValue");
			data.Add("[ExpandEnumerable]System.Collections.Generic.IDictionary<string, int> paramValue");

			return data;
		}
	}
}
