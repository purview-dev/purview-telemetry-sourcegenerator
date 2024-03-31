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

		System.Diagnostics.Metrics.Counter<System.Int32>? _counterInstrument = null;

		public TestMetricsCore(
#if NET8_OR_GREATER
			System.Diagnostics.Metrics.IMeterFactory meterFactory
#endif
		)
		{
			InitializeMeters(
#if NET8_OR_GREATER
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
			_counterInstrument = _meter.CreateCounter<System.Int32>(name: "Counter", unit: null, description: null
#if !NET7_0
				, tags: null
#endif
			);
		}

#if NET8_OR_GREATER
		partial void PopulateMeterTags(System.Collections.Generic.Dictionary<string, object?> meterTags);
#endif

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Counter(int counterValue, int intParam, bool boolParam)
		{
			if (_counterInstrument == null)
			{
				return;
			}

			System.Diagnostics.TagList counterTagList = new System.Diagnostics.TagList();

			counterTagList.Add("intparam", intParam);
			counterTagList.Add("boolparam", boolParam);

			_counterInstrument.Add(counterValue, tagList: counterTagList);
		}
	}
}
