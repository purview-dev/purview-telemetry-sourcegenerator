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
	sealed partial class TestLoggerCore : Testing.ITestLogger
	{
		readonly global::Microsoft.Extensions.Logging.ILogger<Testing.ITestLogger> _logger;

		public TestLoggerCore(global::Microsoft.Extensions.Logging.ILogger<Testing.ITestLogger> logger)
		{
			_logger = logger;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void LogEntryWithGenericTypeParam(global::System.Collections.Generic.IDictionary<string, int> paramName)
		{
			if (!_logger.IsEnabled(global::Microsoft.Extensions.Logging.LogLevel.Information))
			{
				return;
			}

			var state = global::Microsoft.Extensions.Logging.LoggerMessageHelper.ThreadLocalState;
			state.ReserveTagSpace(2);

			state.TagArray[0] = new("{OriginalFormat}", "LogEntryWithGenericTypeParam: ParamName = {ParamName}");
			state.TagArray[1] = new("paramName", paramName);

			_logger.Log(
				global::Microsoft.Extensions.Logging.LogLevel.Information,
				new (842060863, "LogEntryWithGenericTypeParam"),
				state,
				null,
				// GENERATE CODEGEN ATTRIB
				static string (s, _) =>
				{
					var tmp0 = s.TagArray[0].Value ?? "(null)";
				// TODO!!
					return string.Empty;
				}
			);

			state.Clear();
		}

	}
}
