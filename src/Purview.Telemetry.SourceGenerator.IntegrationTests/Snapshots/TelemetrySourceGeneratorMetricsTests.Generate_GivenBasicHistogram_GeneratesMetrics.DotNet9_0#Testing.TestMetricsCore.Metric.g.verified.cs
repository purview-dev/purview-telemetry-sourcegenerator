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
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	sealed partial class TestMetricsCore : global::Testing.ITestMetrics
	{
		global::System.Diagnostics.Metrics.Meter _meter = default!;

		global::System.Diagnostics.Metrics.Histogram<int>? _histogramInstrument = null;
		global::System.Diagnostics.Metrics.Histogram<int>? _histogram1Instrument = null;

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		public TestMetricsCore(global::System.Diagnostics.Metrics.IMeterFactory meterFactory)
		{
			InitializeMeters(meterFactory);
		}

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		void InitializeMeters(global::System.Diagnostics.Metrics.IMeterFactory meterFactory)
		{
			if (_meter != null)
			{
				throw new global::System.Exception("The meters have already been initialized.");
			}

			global::System.Collections.Generic.Dictionary<string, object?> meterTags = new();

			PopulateMeterTags(meterTags);

			_meter = meterFactory.Create(new global::System.Diagnostics.Metrics.MeterOptions("testing-meter")
			{
				Version = null,
				Tags = meterTags
			});

			global::System.Collections.Generic.Dictionary<string, object?> histogramTags = new();

			PopulateHistogramTags(histogramTags);

			_histogramInstrument = _meter.CreateHistogram<int>(name: "histogram", unit: null, description: null, tags: histogramTags);
			global::System.Collections.Generic.Dictionary<string, object?> histogram1Tags = new();

			PopulateHistogram1Tags(histogram1Tags);

			_histogram1Instrument = _meter.CreateHistogram<int>(name: "histogram1", unit: null, description: null, tags: histogram1Tags);
		}

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		partial void PopulateMeterTags(global::System.Collections.Generic.Dictionary<string, object?> meterTags);

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		partial void PopulateHistogramTags(global::System.Collections.Generic.Dictionary<string, object?> instrumentTags);

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		partial void PopulateHistogram1Tags(global::System.Collections.Generic.Dictionary<string, object?> instrumentTags);

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Histogram(int counterValue, int intParam, bool boolParam)
		{
			if (_histogramInstrument == null)
			{
				return;
			}

			global::System.Diagnostics.TagList histogramTagList = new();

			histogramTagList.Add("intparam", intParam);
			histogramTagList.Add("boolparam", boolParam);

			_histogramInstrument.Record(counterValue, tagList: histogramTagList);
		}
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Histogram1(int counterValue, int intParam, bool boolParam)
		{
			if (_histogram1Instrument == null)
			{
				return;
			}

			global::System.Diagnostics.TagList histogram1TagList = new();

			histogram1TagList.Add("intparam", intParam);
			histogram1TagList.Add("boolparam", boolParam);

			_histogram1Instrument.Record(counterValue, tagList: histogram1TagList);
		}
	}
}
