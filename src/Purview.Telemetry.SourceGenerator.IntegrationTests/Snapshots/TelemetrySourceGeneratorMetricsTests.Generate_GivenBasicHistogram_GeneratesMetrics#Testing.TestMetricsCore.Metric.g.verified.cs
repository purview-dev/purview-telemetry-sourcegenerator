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

		readonly System.Diagnostics.Metrics.Histogram<System.Int32> _histogramInstrument;
		readonly System.Diagnostics.Metrics.Histogram<System.Int32> _histogram1Instrument;

		public TestMetricsCore(System.Diagnostics.Metrics.IMeterFactory meterFactory)
		{
			System.Collections.Generic.Dictionary<string, object?> meterTags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateMeterTags(meterTags);

			_meter = meterFactory.Create(new System.Diagnostics.Metrics.MeterOptions("testing-meter")
			{
				Version = null,
				Tags = meterTags
			});

			_histogramInstrument = _meter.CreateHistogram<System.Int32>(name: "Histogram", unit: null, description: null, tags: null);
			_histogram1Instrument = _meter.CreateHistogram<System.Int32>(name: "Histogram1", unit: null, description: null, tags: null);
		}

		partial void PopulateMeterTags(System.Collections.Generic.Dictionary<string, object?> meterTags);

		public void Histogram(int counterValue, int intParam, bool boolParam)
		{
			System.Diagnostics.TagList histogramTagList = new System.Diagnostics.TagList();

			histogramTagList.Add("intparam", intParam);
			histogramTagList.Add("boolparam", boolParam);

			_histogramInstrument.Record(counterValue, tagList: histogramTagList);
		}

		public void Histogram1(int counterValue, int intParam, bool boolParam)
		{
			System.Diagnostics.TagList histogram1TagList = new System.Diagnostics.TagList();

			histogram1TagList.Add("intparam", intParam);
			histogram1TagList.Add("boolparam", boolParam);

			_histogram1Instrument.Record(counterValue, tagList: histogram1TagList);
		}
	}
}
