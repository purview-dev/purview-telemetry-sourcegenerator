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
		public void LogEntryWithCustomExceptionType(global::System.NullReferenceException nrf)
		{
			if (!_logger.IsEnabled(global::Microsoft.Extensions.Logging.LogLevel.Error))
			{
				return;
			}

			var state = global::Microsoft.Extensions.Logging.LoggerMessageHelper.ThreadLocalState;
			state.ReserveTagSpace(1);

			state.TagArray[0] = new("{OriginalFormat}", "LogEntryWithCustomExceptionType");

			_logger.Log(
				global::Microsoft.Extensions.Logging.LogLevel.Error,
				new (427053149, nameof(LogEntryWithCustomExceptionType)),
				state,
				nrf,
				[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
				static string (s, _) =>
				{
#if NET
					return string.Create(global::System.Globalization.CultureInfo.InvariantCulture, $"LogEntryWithCustomExceptionType");
#else
					return global::System.FormattableString.Invariant($"LogEntryWithCustomExceptionType");
#endif
				}
			);

			state.Clear();
		}

	}
}
