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

		System.Diagnostics.Metrics.ObservableGauge<System.Byte>? _metricInstrument = null;

		public TestMetricsCore(System.Diagnostics.Metrics.IMeterFactory meterFactory)
		{
			System.Collections.Generic.Dictionary<string, object?> meterTags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateMeterTags(meterTags);

			_meter = meterFactory.Create(new System.Diagnostics.Metrics.MeterOptions("testing-meter")
			{
				Version = null,
				Tags = meterTags
			});

		}

		partial void PopulateMeterTags(System.Collections.Generic.Dictionary<string, object?> meterTags);

		public void Metric(System.Func<System.Diagnostics.Metrics.Measurement<System.Byte>> f, int intParam, bool boolParam)
		{
			if (_metricInstrument != null)
			{
				return;
			}

			System.Diagnostics.TagList metricTagList = new System.Diagnostics.TagList();

			metricTagList.Add("intparam", intParam);
			metricTagList.Add("boolparam", boolParam);

			_metricInstrument = _meter.CreateObservableGauge<System.Byte>("an-observablegauge-name-property", f, unit: "biscuits-property", description: "biscuit sales per-capita-property.", tags: metricTagList);
		}
	}
}
