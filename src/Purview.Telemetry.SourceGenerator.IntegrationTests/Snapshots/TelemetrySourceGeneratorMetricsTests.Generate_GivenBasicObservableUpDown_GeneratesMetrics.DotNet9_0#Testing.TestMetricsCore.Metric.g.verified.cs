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
		System.Diagnostics.Metrics.Meter _meter = default!;

		System.Diagnostics.Metrics.ObservableUpDownCounter<int>? _observableUpDownInstrument = null;
		System.Diagnostics.Metrics.ObservableUpDownCounter<int>? _observableUpDown2Instrument = null;
		System.Diagnostics.Metrics.ObservableUpDownCounter<int>? _observableUpDown3Instrument = null;

		public TestMetricsCore(global::System.Diagnostics.Metrics.IMeterFactory meterFactory)
		{
			InitializeMeters(meterFactory);
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		void InitializeMeters(System.Diagnostics.Metrics.IMeterFactory meterFactory)
		{
			if (_meter != null)
			{
				throw new System.Exception("The meters have already been initialized.");
			}

			System.Collections.Generic.Dictionary<string, object?> meterTags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateMeterTags(meterTags);

			_meter = meterFactory.Create(new System.Diagnostics.Metrics.MeterOptions("testing-observable-meter")
			{
				Version = null,
				Tags = meterTags
			});

		}

		partial void PopulateMeterTags(System.Collections.Generic.Dictionary<string, object?> meterTags);

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void ObservableUpDown(System.Func<int> f, int intParam, bool boolParam)
		{
			if (_observableUpDownInstrument != null)
			{
				return;
			}

			System.Diagnostics.TagList observableUpDownTagList = new System.Diagnostics.TagList();

			observableUpDownTagList.Add("intparam", intParam);
			observableUpDownTagList.Add("boolparam", boolParam);

			_observableUpDownInstrument = _meter.CreateObservableUpDownCounter<int>("observableupdown", f, unit: null, description: null
				, tags: observableUpDownTagList
			);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void ObservableUpDown2(System.Func<System.Diagnostics.Metrics.Measurement<int>> f, int intParam, bool boolParam)
		{
			if (_observableUpDown2Instrument != null)
			{
				throw new System.Exception("observableupdown2 has already been initialized.");
			}

			System.Diagnostics.TagList observableUpDown2TagList = new System.Diagnostics.TagList();

			observableUpDown2TagList.Add("intparam", intParam);
			observableUpDown2TagList.Add("boolparam", boolParam);

			_observableUpDown2Instrument = _meter.CreateObservableUpDownCounter<int>("observableupdown2", f, unit: null, description: null
				, tags: observableUpDown2TagList
			);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void ObservableUpDown3(System.Func<System.Collections.Generic.IEnumerable<System.Diagnostics.Metrics.Measurement<int>>> f, int intParam, bool boolParam)
		{
			if (_observableUpDown3Instrument != null)
			{
				return;
			}

			System.Diagnostics.TagList observableUpDown3TagList = new System.Diagnostics.TagList();

			observableUpDown3TagList.Add("intparam", intParam);
			observableUpDown3TagList.Add("boolparam", boolParam);

			_observableUpDown3Instrument = _meter.CreateObservableUpDownCounter<int>("observableupdown3", f, unit: null, description: null
				, tags: observableUpDown3TagList
			);
		}
	}
}
