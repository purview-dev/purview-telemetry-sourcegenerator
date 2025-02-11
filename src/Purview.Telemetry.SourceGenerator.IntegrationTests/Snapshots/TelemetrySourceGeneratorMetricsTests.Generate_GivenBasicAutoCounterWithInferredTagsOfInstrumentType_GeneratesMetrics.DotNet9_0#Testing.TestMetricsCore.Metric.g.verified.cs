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

		System.Diagnostics.Metrics.Counter<int>? _autoCounterInstrument = null;

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

			_meter = meterFactory.Create(new System.Diagnostics.Metrics.MeterOptions("testing-meter")
			{
				Version = null,
				Tags = meterTags
			});

			System.Collections.Generic.Dictionary<string, object?> autoCounterTags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateAutoCounterTags(autoCounterTags);

			_autoCounterInstrument = _meter.CreateCounter<int>(name: "autocounter", unit: null, description: null, tags: autoCounterTags);
		}

		partial void PopulateMeterTags(System.Collections.Generic.Dictionary<string, object?> meterTags);

		partial void PopulateAutoCounterTags(System.Collections.Generic.Dictionary<string, object?> instrumentTags);

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void AutoCounter(int intParam)
		{
			if (_autoCounterInstrument == null)
			{
				return;
			}

			System.Diagnostics.TagList autoCounterTagList = new System.Diagnostics.TagList();

			autoCounterTagList.Add("intparam", intParam);

			_autoCounterInstrument.Add(1, tagList: autoCounterTagList);
		}
	}
}
