﻿//HintName: Testing.TestMetricsCore.Metric.g.cs
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
	sealed partial class TestMetricsCore : Testing.ITestMetrics
	{
		readonly System.Diagnostics.Metrics.Meter _meter;

		readonly System.Diagnostics.Metrics.Counter<System.Int32> _metricInstrument;

		public TestMetricsCore(System.Diagnostics.Metrics.IMeterFactory meterFactory)
		{
			System.Collections.Generic.Dictionary<string, object?> meterTags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateMeterTags(meterTags);

			_meter = meterFactory.Create(new System.Diagnostics.Metrics.MeterOptions("testing-meter")
			{
				Version = null,
				Tags = meterTags
			});

			_metricInstrument = _meter.CreateCounter<System.Int32>(name: "a-counter-name-param", unit: "cakes-param", description: "cake sales per-capita-param.", tags: null);
		}

		partial void PopulateMeterTags(System.Collections.Generic.Dictionary<string, object?> meterTags);

		public void Metric(int counterValue, int intParam, bool boolParam)
		{
			System.Diagnostics.TagList metricTagList = new System.Diagnostics.TagList();

			metricTagList.Add("intparam", intParam);
			metricTagList.Add("boolparam", boolParam);

			_metricInstrument.Add(counterValue, tagList: metricTagList);
		}
	}
}