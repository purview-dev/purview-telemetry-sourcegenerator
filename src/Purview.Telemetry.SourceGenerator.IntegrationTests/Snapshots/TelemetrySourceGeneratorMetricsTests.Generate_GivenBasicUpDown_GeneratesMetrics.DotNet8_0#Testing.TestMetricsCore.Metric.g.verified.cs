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

		System.Diagnostics.Metrics.UpDownCounter<int>? _upDownInstrument = null;
		System.Diagnostics.Metrics.UpDownCounter<int>? _upDown2Instrument = null;

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

#if !NET7_0

			System.Collections.Generic.Dictionary<string, object?> upDownTags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateUpDownTags(upDownTags);

#endif

			_upDownInstrument = _meter.CreateUpDownCounter<int>(name: "updown", unit: null, description: null
#if !NET7_0
				, tags: upDownTags
#endif
			);

#if !NET7_0

			System.Collections.Generic.Dictionary<string, object?> upDown2Tags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateUpDown2Tags(upDown2Tags);

#endif

			_upDown2Instrument = _meter.CreateUpDownCounter<int>(name: "updown2", unit: null, description: null
#if !NET7_0
				, tags: upDown2Tags
#endif
			);
		}

#if NET8_0_OR_GREATER

		partial void PopulateMeterTags(System.Collections.Generic.Dictionary<string, object?> meterTags);

#endif

#if !NET7_0

		partial void PopulateUpDownTags(System.Collections.Generic.Dictionary<string, object?> instrumentTags);

		partial void PopulateUpDown2Tags(System.Collections.Generic.Dictionary<string, object?> instrumentTags);

#endif

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void UpDown(int counterValue, int intParam, bool boolParam)
		{
			if (_upDownInstrument == null)
			{
				return;
			}

			System.Diagnostics.TagList upDownTagList = new System.Diagnostics.TagList();

			upDownTagList.Add("intparam", intParam);
			upDownTagList.Add("boolparam", boolParam);

			_upDownInstrument.Add(counterValue, tagList: upDownTagList);
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void UpDown2(int counterValue, int intParam, bool boolParam)
		{
			if (_upDown2Instrument == null)
			{
				return;
			}

			System.Diagnostics.TagList upDown2TagList = new System.Diagnostics.TagList();

			upDown2TagList.Add("intparam", intParam);
			upDown2TagList.Add("boolparam", boolParam);

			_upDown2Instrument.Add(counterValue, tagList: upDown2TagList);
		}
	}
}
