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

		static readonly global::System.Action<global::Microsoft.Extensions.Logging.ILogger, System.Collections.Generic.IEnumerable<string>, global::System.Exception?> _logEntryWithGenericTypeParamAction = global::Microsoft.Extensions.Logging.LoggerMessage.Define<System.Collections.Generic.IEnumerable<string>>(global::Microsoft.Extensions.Logging.LogLevel.Information, new global::Microsoft.Extensions.Logging.EventId(842060863, "LogEntryWithGenericTypeParam"), "LogEntryWithGenericTypeParam: ParamName = {ParamName}");

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		public TestLoggerCore(global::Microsoft.Extensions.Logging.ILogger<global::Testing.ITestLogger> logger)
		{
			_logger = logger;
		}

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void LogEntryWithGenericTypeParam(System.Collections.Generic.IEnumerable<string> paramName)
		{
			if (!_logger.IsEnabled(global::Microsoft.Extensions.Logging.LogLevel.Information))
			{
				return;
			}

			_logEntryWithGenericTypeParamAction(_logger, paramName, null);
		}

	}
}
