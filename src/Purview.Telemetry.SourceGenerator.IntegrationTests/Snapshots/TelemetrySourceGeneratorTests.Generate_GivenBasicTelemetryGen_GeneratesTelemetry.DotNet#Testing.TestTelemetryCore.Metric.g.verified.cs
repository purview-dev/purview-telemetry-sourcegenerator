﻿//HintName: Testing.TestTelemetryCore.Metric.g.cs
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
	sealed partial class TestTelemetryCore : Testing.ITestTelemetry
	{
		System.Diagnostics.Metrics.Meter _meter;

		System.Diagnostics.Metrics.Counter<System.Int32> _counterInstrument;

		public TestTelemetryCore(Microsoft.Extensions.Logging.ILogger<Testing.ITestTelemetry> logger, System.Diagnostics.Metrics.IMeterFactory meterFactory)
		{
			_logger = logger;
			InitializeMeters(meterFactory);
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		void InitializeMeters(System.Diagnostics.Metrics.IMeterFactory meterFactory)
		{
			if (_meter != null)
			{
				throw new System.Exception("The metrics have already been initialized.");
			}

			System.Collections.Generic.Dictionary<string, object?> meterTags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateMeterTags(meterTags);

			_meter = meterFactory.Create(new System.Diagnostics.Metrics.MeterOptions("")
			{
				Version = null,
				Tags = meterTags
			});

			_counterInstrument = _meter.CreateCounter<System.Int32>(name: "Counter", unit: null, description: null, tags: null);
		}

		partial void PopulateMeterTags(System.Collections.Generic.Dictionary<string, object?> meterTags);

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Counter(int counterValue, int intParam, bool boolParam)
		{
			System.Diagnostics.TagList counterTagList = new System.Diagnostics.TagList();

			counterTagList.Add("intparam", intParam);
			counterTagList.Add("boolparam", boolParam);

			_counterInstrument.Add(counterValue, tagList: counterTagList);
		}
	}
}
