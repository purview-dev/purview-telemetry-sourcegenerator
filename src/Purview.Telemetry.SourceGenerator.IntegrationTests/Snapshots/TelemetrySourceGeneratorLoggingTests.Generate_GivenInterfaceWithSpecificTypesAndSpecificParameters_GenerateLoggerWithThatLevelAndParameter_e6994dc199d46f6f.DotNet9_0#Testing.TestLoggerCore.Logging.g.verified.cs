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
		readonly Microsoft.Extensions.Logging.ILogger<Testing.ITestLogger> _logger;

		static readonly System.Action<Microsoft.Extensions.Logging.ILogger, string, int, bool, System.Exception?> _logAction = Microsoft.Extensions.Logging.LoggerMessage.Define<string, int, bool>(Microsoft.Extensions.Logging.LogLevel.Warning, default, "Test.Log: stringParam: {StringParam}, intParam: {IntParam}, boolParam: {BoolParam}");
		static readonly System.Action<Microsoft.Extensions.Logging.ILogger, string, int, bool, System.Exception?> _log_EventId_1Action = Microsoft.Extensions.Logging.LoggerMessage.Define<string, int, bool>(Microsoft.Extensions.Logging.LogLevel.Warning, new Microsoft.Extensions.Logging.EventId(100, "Test.Log_EventId_1"), "Test.Log_EventId_1: stringParam: {StringParam}, intParam: {IntParam}, boolParam: {BoolParam}");
		static readonly System.Action<Microsoft.Extensions.Logging.ILogger, string, int, bool, System.Exception?> _log_EventId_3Action = Microsoft.Extensions.Logging.LoggerMessage.Define<string, int, bool>(Microsoft.Extensions.Logging.LogLevel.Warning, new Microsoft.Extensions.Logging.EventId(100, "Test.Log_EventId_3"), "Test.Log_EventId_3: stringParam: {StringParam}, intParam: {IntParam}, boolParam: {BoolParam}");
		static readonly System.Action<Microsoft.Extensions.Logging.ILogger, string, int, bool, System.Exception?> _log_MessageTemplate_1Action = Microsoft.Extensions.Logging.LoggerMessage.Define<string, int, bool>(Microsoft.Extensions.Logging.LogLevel.Warning, default, "template");
		static readonly System.Action<Microsoft.Extensions.Logging.ILogger, string, int, bool, System.Exception?> _log_MessageTemplate_2Action = Microsoft.Extensions.Logging.LoggerMessage.Define<string, int, bool>(Microsoft.Extensions.Logging.LogLevel.Warning, default, "template");
		static readonly System.Action<Microsoft.Extensions.Logging.ILogger, string, int, bool, System.Exception?> _log_MessageTemplate_3Action = Microsoft.Extensions.Logging.LoggerMessage.Define<string, int, bool>(Microsoft.Extensions.Logging.LogLevel.Warning, default, "template");

		public TestLoggerCore(Microsoft.Extensions.Logging.ILogger<Testing.ITestLogger> logger)
		{
			_logger = logger;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Log(string stringParam, int intParam, bool boolParam)
		{
			if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Warning))
			{
				return;
			}

			_logAction(_logger, stringParam, intParam, boolParam, null);
		}


		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Log_EventId_1(string stringParam, int intParam, bool boolParam)
		{
			if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Warning))
			{
				return;
			}

			_log_EventId_1Action(_logger, stringParam, intParam, boolParam, null);
		}


		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Log_EventId_3(string stringParam, int intParam, bool boolParam)
		{
			if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Warning))
			{
				return;
			}

			_log_EventId_3Action(_logger, stringParam, intParam, boolParam, null);
		}


		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Log_MessageTemplate_1(string stringParam, int intParam, bool boolParam)
		{
			if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Warning))
			{
				return;
			}

			_log_MessageTemplate_1Action(_logger, stringParam, intParam, boolParam, null);
		}


		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Log_MessageTemplate_2(string stringParam, int intParam, bool boolParam)
		{
			if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Warning))
			{
				return;
			}

			_log_MessageTemplate_2Action(_logger, stringParam, intParam, boolParam, null);
		}


		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Log_MessageTemplate_3(string stringParam, int intParam, bool boolParam)
		{
			if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Warning))
			{
				return;
			}

			_log_MessageTemplate_3Action(_logger, stringParam, intParam, boolParam, null);
		}

	}
}
