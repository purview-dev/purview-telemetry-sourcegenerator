﻿//HintName: Testing.Test1.TestClass1.TestClass2.TestClass3.TestLoggerCore.Logging.g.cs
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
		partial class TestClass2
		{
			partial class TestClass3
			{
				[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
				sealed partial class TestLoggerCore : global::Testing.Test1.TestClass1.TestClass2.TestClass3.ITestLogger
				{
					readonly global::Microsoft.Extensions.Logging.ILogger<global::Testing.Test1.TestClass1.TestClass2.TestClass3.ITestLogger> _logger;

					static readonly global::System.Func<global::Microsoft.Extensions.Logging.ILogger, string, int, global::System.IDisposable?> _logAction = global::Microsoft.Extensions.Logging.LoggerMessage.DefineScope<string, int>("Log: StringParam = {StringParam}, IntParam = {IntParam}");

					[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
					public TestLoggerCore(global::Microsoft.Extensions.Logging.ILogger<global::Testing.Test1.TestClass1.TestClass2.TestClass3.ITestLogger> logger)
					{
						_logger = logger;
					}

					[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
					[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
					public global::System.IDisposable? Log(string stringParam, int intParam)
					{
						return _logAction(_logger, stringParam, intParam);
					}

				}
			}
		}
	}
}
