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
		public void LogEntryWithGenericTypeParam(global::System.Collections.Generic.IEnumerable<string> paramName)
		{
			if (!_logger.IsEnabled(global::Microsoft.Extensions.Logging.LogLevel.Information))
			{
				return;
			}

			var state = global::Microsoft.Extensions.Logging.LoggerMessageHelper.ThreadLocalState;
			state.ReserveTagSpace(2);

			state.TagArray[0] = new("{OriginalFormat}", "LogEntryWithGenericTypeParam: ParamName = {ParamName}");
			state.TagArray[1] = new("paramName", paramName == null ? null : global::Microsoft.Extensions.Logging.LoggerMessageHelper.Stringify(paramName));

			_logger.Log(
				global::Microsoft.Extensions.Logging.LogLevel.Information,
				new (842060863, "LogEntryWithGenericTypeParam"),
				state,
				null,
				[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
				static string (s, _) =>
				{
					var v0 = s.TagArray[1].Value ?? "(null)";

#if NET
					return string.Create(global::System.Globalization.CultureInfo.InvariantCulture, $"LogEntryWithGenericTypeParam: ParamName = {v0}");
#else
					return global::System.FormattableString.Invariant($"LogEntryWithGenericTypeParam: ParamName = {v0}");
#endif
				}
			);

			state.Clear();
		}

	}
}
