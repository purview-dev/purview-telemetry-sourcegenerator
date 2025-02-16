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

		global::System.Diagnostics.Metrics.Counter<int>? _metricInstrument = null;

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

			global::System.Collections.Generic.Dictionary<string, object?> metricTags = new();

			PopulateMetricTags(metricTags);

			_metricInstrument = _meter.CreateCounter<int>(name: "a-counter-name-param", unit: "cakes-param", description: "cake sales per-capita-param.", tags: metricTags);
		}

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		partial void PopulateMeterTags(global::System.Collections.Generic.Dictionary<string, object?> meterTags);

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		partial void PopulateMetricTags(global::System.Collections.Generic.Dictionary<string, object?> instrumentTags);

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Metric(int counterValue, int intParam, bool boolParam)
		{
			if (_metricInstrument == null)
			{
				return;
			}

			global::System.Diagnostics.TagList metricTagList = new();

			metricTagList.Add("intparam", intParam);
			metricTagList.Add("boolparam", boolParam);

			_metricInstrument.Add(counterValue, tagList: metricTagList);
		}
	}
}
