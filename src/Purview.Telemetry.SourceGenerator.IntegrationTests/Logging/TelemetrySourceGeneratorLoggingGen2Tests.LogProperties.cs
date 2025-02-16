﻿namespace Purview.Telemetry.SourceGenerator.Logging;

partial class TelemetrySourceGeneratorLoggingGen2Tests
{
	[Fact]
	public async Task Generate_GivenMethodWithLogProperty_GeneratesIndividualProperties()
	{
		// Arrange
		var basicLogger = @$"
using Purview.Telemetry.Logging;
using Microsoft.Extensions.Logging;

namespace Testing;

[Logger]
public interface ITestLogger
{{
	void LogWeather([LogProperties]WeatherForecast weather);
}}

public class WeatherForecast
{{
	public DateTime Date {{ get; set; }}
	public int TemperatureC {{ get; set; }}
	public string Summary {{ get; set; }}
}}
";

		// Act
		var generationResult = await GenerateAsync(basicLogger, includeLoggerTypes: IncludeLoggerTypes.Telemetry);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenMethodWithExceptionUsedInTemplate_UsesPassInException()
	{
		// Arrange
		var basicLogger = @$"
using Purview.Telemetry.Logging;
using Microsoft.Extensions.Logging;

namespace Testing;

[Logger]
public interface ITestLogger
{{
	[Log(MessageTemplate = ""v = {{v}} Exception = {{ex}}"")]
	void Log(string v, Exception ex);
}}
";

		// Act
		var generationResult = await GenerateAsync(basicLogger, includeLoggerTypes: IncludeLoggerTypes.Telemetry);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenMethodWithLogPropertyOmit_GeneratesIndividualProperties()
	{
		// Arrange
		var basicLogger = @$"
using Purview.Telemetry.Logging;
using Microsoft.Extensions.Logging;

namespace Testing;

[Logger]
public interface ITestLogger
{{
	void LogWeatherWithOmit([LogProperties(OmitReferenceName = true)]WeatherForecast weather);
}}

public class WeatherForecast
{{
	public DateTime Date {{ get; set; }}
	public int TemperatureC {{ get; set; }}
	public string Summary {{ get; set; }}
}}
";

		// Act
		var generationResult = await GenerateAsync(basicLogger, includeLoggerTypes: IncludeLoggerTypes.Telemetry);

		// Assert
		await TestHelpers.Verify(generationResult);
	}


	[Fact]
	public async Task Generate_GivenMethodWithLogPropertySkipNull_GeneratesIndividualProperties()
	{
		// Arrange
		var basicLogger = @$"
using Purview.Telemetry.Logging;
using Microsoft.Extensions.Logging;

namespace Testing;

[Logger]
public interface ITestLogger
{{
	void LogWeather([LogProperties(SkipNullProperties = true)]WeatherForecast weather);
}}

public class WeatherForecast
{{
	public DateTime Date {{ get; set; }}
	public int TemperatureC {{ get; set; }}
	public string Summary {{ get; set; }}
}}
";

		// Act
		var generationResult = await GenerateAsync(basicLogger, includeLoggerTypes: IncludeLoggerTypes.Telemetry);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenMethodWithLogPropertySkipNullAndOmit_GeneratesIndividualProperties()
	{
		// Arrange
		var basicLogger = @$"
using Purview.Telemetry.Logging;
using Microsoft.Extensions.Logging;

namespace Testing;

[Logger]
public interface ITestLogger
{{
	void LogWeather([LogProperties(SkipNullProperties = true, OmitReferenceName = true)]WeatherForecast weather);
}}

public class WeatherForecast
{{
	public DateTime Date {{ get; set; }}
	public int TemperatureC {{ get; set; }}
	public string Summary {{ get; set; }}
}}
";

		// Act
		var generationResult = await GenerateAsync(basicLogger, includeLoggerTypes: IncludeLoggerTypes.Telemetry);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

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

	[Fact]
	public async Task Generate_GivenMethodWithLogPropertyIgnore_GeneratesIndividualProperties()
	{
		// Arrange
		var basicLogger = @$"
using Purview.Telemetry.Logging;
using Microsoft.Extensions.Logging;

namespace Testing;

[Logger]
public interface ITestLogger
{{
	void LogWeather([LogProperties]WeatherForecast weather);
}}

public class WeatherForecast
{{
	public DateTime Date {{ get; set; }}
	public int TemperatureC {{ get; set; }}
	public string Summary {{ get; set; }}

	[LogPropertyIgnore]
	public string IgnoreMe {{ get; set; }}
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
