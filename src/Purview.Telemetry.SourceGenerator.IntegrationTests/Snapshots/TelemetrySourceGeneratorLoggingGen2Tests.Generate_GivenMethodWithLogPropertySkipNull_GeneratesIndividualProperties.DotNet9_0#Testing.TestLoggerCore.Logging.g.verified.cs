﻿//HintName: Testing.TestLoggerCore.Logging.g.cs
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Purview.Telemetry.SourceGenerator
//     on {Scrubbed}.
//
//     Changes to this file may cause incorrect behaviour and will be lost
//     when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable 1591 // publicly visible type or member must be documented

#nullable enable

namespace Testing
{
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	sealed partial class TestLoggerCore : global::Testing.ITestLogger
	{
		readonly global::Microsoft.Extensions.Logging.ILogger<global::Testing.ITestLogger> _logger;

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		public TestLoggerCore(global::Microsoft.Extensions.Logging.ILogger<global::Testing.ITestLogger> logger)
		{
			_logger = logger;
		}

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void LogWeather(global::Testing.WeatherForecast weather)
		{
			if (!_logger.IsEnabled(global::Microsoft.Extensions.Logging.LogLevel.Information))
			{
				return;
			}

			var state = global::Microsoft.Extensions.Logging.LoggerMessageHelper.ThreadLocalState;
			state.ReserveTagSpace(2);

			state.TagArray[0] = new("{OriginalFormat}", "LogWeather: Weather = {Weather}");
			state.TagArray[1] = new("weather", weather);

			state.AddTag("weather.Date", weather?.Date);
			state.AddTag("weather.TemperatureC", weather?.TemperatureC);
			{
				var tmp = weather?.Summary;
				if (tmp != null)
				{
					state.AddTag("weather.Summary", tmp);
				}
			}

			_logger.Log(
				global::Microsoft.Extensions.Logging.LogLevel.Information,
				new (1268453319, nameof(LogWeather)),
				state,
				null,
				[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
				static string (s, _) =>
				{
					var v0 = s.TagArray[1].Value ?? "(null)";

#if NET
					return string.Create(global::System.Globalization.CultureInfo.InvariantCulture, $"LogWeather: Weather = {v0}");
#else
					return global::System.FormattableString.Invariant($"LogWeather: Weather = {v0}");
#endif
				}
			);

			state.Clear();
		}

	}
}
