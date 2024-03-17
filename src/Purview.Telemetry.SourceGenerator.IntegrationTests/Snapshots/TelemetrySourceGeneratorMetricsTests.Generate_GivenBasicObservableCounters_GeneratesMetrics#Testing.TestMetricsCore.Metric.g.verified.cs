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

		System.Diagnostics.Metrics.ObservableCounter<System.Int32>? _observableCounterInstrument = null;
		System.Diagnostics.Metrics.ObservableCounter<System.Int32>? _observableCounter2Instrument = null;
		System.Diagnostics.Metrics.ObservableCounter<System.Int32>? _observableCounter3Instrument = null;

		public TestMetricsCore(System.Diagnostics.Metrics.IMeterFactory meterFactory)
		{
			System.Collections.Generic.Dictionary<string, object?> meterTags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateMeterTags(meterTags);

			_meter = meterFactory.Create(new System.Diagnostics.Metrics.MeterOptions("testing-observable-meter")
			{
				Version = null,
				Tags = meterTags
			});

		}

		partial void PopulateMeterTags(System.Collections.Generic.Dictionary<string, object?> meterTags);

		public void ObservableCounter(System.Func<int> f, int intParam, bool boolParam)
		{
			if (_observableCounterInstrument != null)
			{
				return;
			}

			System.Diagnostics.TagList observableCounterTagList = new System.Diagnostics.TagList();

			observableCounterTagList.Add("intparam", intParam);
			observableCounterTagList.Add("boolparam", boolParam);

			_observableCounterInstrument = _meter.CreateObservableCounter<System.Int32>("ObservableCounter", f, unit: null, description: null, tags: observableCounterTagList);
		}

		public void ObservableCounter2(System.Func<System.Diagnostics.Metrics.Measurement<System.Int32>> f, int intParam, bool boolParam)
		{
			if (_observableCounter2Instrument != null)
			{
				throw new System.Exception("ObservableCounter2 has already been initialized.");
			}

			System.Diagnostics.TagList observableCounter2TagList = new System.Diagnostics.TagList();

			observableCounter2TagList.Add("intparam", intParam);
			observableCounter2TagList.Add("boolparam", boolParam);

			_observableCounter2Instrument = _meter.CreateObservableCounter<System.Int32>("ObservableCounter2", f, unit: null, description: null, tags: observableCounter2TagList);
		}

		public void ObservableCounter3(System.Func<System.Collections.Generic.IEnumerable<System.Diagnostics.Metrics.Measurement<System.Int32>>> f, int intParam, bool boolParam)
		{
			if (_observableCounter3Instrument != null)
			{
				return;
			}

			System.Diagnostics.TagList observableCounter3TagList = new System.Diagnostics.TagList();

			observableCounter3TagList.Add("intparam", intParam);
			observableCounter3TagList.Add("boolparam", boolParam);

			_observableCounter3Instrument = _meter.CreateObservableCounter<System.Int32>("ObservableCounter3", f, unit: null, description: null, tags: observableCounter3TagList);
		}
	}
}