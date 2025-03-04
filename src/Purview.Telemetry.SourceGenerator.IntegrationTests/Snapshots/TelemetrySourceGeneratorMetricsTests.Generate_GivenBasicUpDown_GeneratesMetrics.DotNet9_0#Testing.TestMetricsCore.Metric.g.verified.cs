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

		global::System.Diagnostics.Metrics.UpDownCounter<int>? _upDownInstrument = null;
		global::System.Diagnostics.Metrics.UpDownCounter<int>? _upDown2Instrument = null;

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

			global::System.Collections.Generic.Dictionary<string, object?> upDownTags = new();

			PopulateUpDownTags(upDownTags);

			_upDownInstrument = _meter.CreateUpDownCounter<int>(name: "updown", unit: null, description: null, tags: upDownTags);
			global::System.Collections.Generic.Dictionary<string, object?> upDown2Tags = new();

			PopulateUpDown2Tags(upDown2Tags);

			_upDown2Instrument = _meter.CreateUpDownCounter<int>(name: "updown2", unit: null, description: null, tags: upDown2Tags);
		}

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		partial void PopulateMeterTags(global::System.Collections.Generic.Dictionary<string, object?> meterTags);

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		partial void PopulateUpDownTags(global::System.Collections.Generic.Dictionary<string, object?> instrumentTags);

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		partial void PopulateUpDown2Tags(global::System.Collections.Generic.Dictionary<string, object?> instrumentTags);

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void UpDown(int counterValue, int intParam, bool boolParam)
		{
			if (_upDownInstrument == null)
			{
				return;
			}

			global::System.Diagnostics.TagList upDownTagList = new();

			upDownTagList.Add("intparam", intParam);
			upDownTagList.Add("boolparam", boolParam);

			_upDownInstrument.Add(counterValue, tagList: upDownTagList);
		}
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void UpDown2(int counterValue, int intParam, bool boolParam)
		{
			if (_upDown2Instrument == null)
			{
				return;
			}

			global::System.Diagnostics.TagList upDown2TagList = new();

			upDown2TagList.Add("intparam", intParam);
			upDown2TagList.Add("boolparam", boolParam);

			_upDown2Instrument.Add(counterValue, tagList: upDown2TagList);
		}
	}
}
