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


		public TestLoggerCore(global::Microsoft.Extensions.Logging.ILogger<Testing.ITestLogger> logger)
		{
			_logger = logger;
		}
	}
}
