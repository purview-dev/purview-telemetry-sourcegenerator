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

		System.Diagnostics.Metrics.ObservableCounter<int>? _counterInstrument = null;
		System.Diagnostics.Metrics.ObservableGauge<int>? _gaugeInstrument = null;
		System.Diagnostics.Metrics.ObservableUpDownCounter<int>? _upDownInstrument = null;

		public TestMetricsCore(
#if NET8_0_OR_GREATER
			System.Diagnostics.Metrics.IMeterFactory meterFactory
#endif
		)
		{
			InitializeMeters(
#if NET8_0_OR_GREATER
				meterFactory
#endif
			);
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		void InitializeMeters(
#if NET8_0_OR_GREATER
			System.Diagnostics.Metrics.IMeterFactory meterFactory
#endif
		)
		{
			if (_meter != null)
			{
				throw new System.Exception("The meters have already been initialized.");
			}

#if NET8_0_OR_GREATER
			System.Collections.Generic.Dictionary<string, object?> meterTags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateMeterTags(meterTags);
#endif

			_meter = 
#if NET8_0_OR_GREATER
				meterFactory.Create(new System.Diagnostics.Metrics.MeterOptions("testing-meter")
				{
					Version = null,
					Tags = meterTags
				});
#else
				new System.Diagnostics.Metrics.Meter(name: "testing-meter", version: null);
#endif
		}

#if NET8_0_OR_GREATER

		partial void PopulateMeterTags(System.Collections.Generic.Dictionary<string, object?> meterTags);

#endif

#if !NET7_0

#endif

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public bool Counter(System.Func<int> counterValue, int intParam, bool boolParam)
		{
			if (_counterInstrument != null)
			{
				return false;
			}

			System.Diagnostics.TagList counterTagList = new System.Diagnostics.TagList();

			counterTagList.Add("intparam", intParam);
			counterTagList.Add("boolparam", boolParam);

			_counterInstrument = _meter.CreateObservableCounter<int>("counter", counterValue, unit: null, description: null
#if !NET7_0
				, tags: counterTagList
#endif
			);

			return true;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public bool Gauge(System.Func<int> counterValue, int intParam, bool boolParam)
		{
			if (_gaugeInstrument != null)
			{
				return false;
			}

			System.Diagnostics.TagList gaugeTagList = new System.Diagnostics.TagList();

			gaugeTagList.Add("intparam", intParam);
			gaugeTagList.Add("boolparam", boolParam);

			_gaugeInstrument = _meter.CreateObservableGauge<int>("gauge", counterValue, unit: null, description: null
#if !NET7_0
				, tags: gaugeTagList
#endif
			);

			return true;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public bool UpDown(System.Func<int> counterValue, int intParam, bool boolParam)
		{
			if (_upDownInstrument != null)
			{
				return false;
			}

			System.Diagnostics.TagList upDownTagList = new System.Diagnostics.TagList();

			upDownTagList.Add("intparam", intParam);
			upDownTagList.Add("boolparam", boolParam);

			_upDownInstrument = _meter.CreateObservableUpDownCounter<int>("updown", counterValue, unit: null, description: null
#if !NET7_0
				, tags: upDownTagList
#endif
			);

			return true;
		}
	}
}
