﻿//HintName: Testing.Test1.TestClass1.TestLoggerCore.Logging.g.cs
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

namespace Testing.Test1
{
	partial class TestClass1
	{
		sealed partial class TestLoggerCore : Testing.Test1.TestClass1.ITestLogger
		{
			readonly Microsoft.Extensions.Logging.ILogger<Testing.Test1.TestClass1.ITestLogger> _logger;

			static readonly System.Func<Microsoft.Extensions.Logging.ILogger, string, int, System.IDisposable?> _logAction = Microsoft.Extensions.Logging.LoggerMessage.DefineScope<string, int>("Log: StringParam = {StringParam}, IntParam = {IntParam}");

			public TestLoggerCore(global::Microsoft.Extensions.Logging.ILogger<Testing.Test1.TestClass1.ITestLogger> logger)
			{
				_logger = logger;
			}

			[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
			public System.IDisposable? Log(string stringParam, int intParam)
			{
				return _logAction(_logger, stringParam, intParam);
			}

		}
	}
}
