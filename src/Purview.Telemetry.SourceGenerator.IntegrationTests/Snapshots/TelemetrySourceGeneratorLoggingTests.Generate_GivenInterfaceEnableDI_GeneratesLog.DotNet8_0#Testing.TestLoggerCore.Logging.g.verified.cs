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
		readonly Microsoft.Extensions.Logging.ILogger<Testing.ITestLogger> _logger = default!;

		static readonly System.Action<Microsoft.Extensions.Logging.ILogger, System.String, System.Int32, System.Boolean, System.Exception?> _logAction = Microsoft.Extensions.Logging.LoggerMessage.Define<System.String, System.Int32, System.Boolean>(Microsoft.Extensions.Logging.LogLevel.Information, default, "Test.Log: stringParam: {StringParam}, intParam: {IntParam}, boolParam: {BoolParam}");

		public TestLoggerCore(Microsoft.Extensions.Logging.ILogger<Testing.ITestLogger> logger)
		{
			_logger = logger;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Log(System.String stringParam, System.Int32 intParam, System.Boolean boolParam)
		{
			if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Information))
			{
				return;
			}

			_logAction(_logger, stringParam, intParam, boolParam, null);
		}

	}
}
