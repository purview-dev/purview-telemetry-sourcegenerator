﻿//HintName: MeterTelemetryCore.Metric.g.cs
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

[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
sealed partial class MeterTelemetryCore : global::IMeterTelemetry
{
	global::System.Diagnostics.Metrics.Meter _meter = default!;

	global::System.Diagnostics.Metrics.Counter<int>? _autoCounterMeterInstrument = null;
	global::System.Diagnostics.Metrics.Counter<int>? _autoIncrementMeterInstrument = null;
	global::System.Diagnostics.Metrics.Counter<int>? _counterMeterInstrument = null;
	global::System.Diagnostics.Metrics.Histogram<int>? _histogramMeterInstrument = null;
	global::System.Diagnostics.Metrics.ObservableCounter<float>? _observableCounterMeterInstrument = null;
	global::System.Diagnostics.Metrics.ObservableGauge<float>? _observableGaugeMeterInstrument = null;
	global::System.Diagnostics.Metrics.ObservableUpDownCounter<byte>? _observableUpDownCounterInstrument = null;
	global::System.Diagnostics.Metrics.UpDownCounter<decimal>? _upDownCounterMeterInstrument = null;

	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	public MeterTelemetryCore(global::System.Diagnostics.Metrics.IMeterFactory meterFactory)
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

		_meter = meterFactory.Create(new global::System.Diagnostics.Metrics.MeterOptions("MeterTelemetry")
		{
			Version = null,
			Tags = meterTags
		});

		global::System.Collections.Generic.Dictionary<string, object?> autoCounterMeterTags = new();

		PopulateAutoCounterMeterTags(autoCounterMeterTags);

		_autoCounterMeterInstrument = _meter.CreateCounter<int>(name: "autocountermeter", unit: null, description: null, tags: autoCounterMeterTags);
		global::System.Collections.Generic.Dictionary<string, object?> autoIncrementMeterTags = new();

		PopulateAutoIncrementMeterTags(autoIncrementMeterTags);

		_autoIncrementMeterInstrument = _meter.CreateCounter<int>(name: "autoincrementmeter", unit: null, description: null, tags: autoIncrementMeterTags);
		global::System.Collections.Generic.Dictionary<string, object?> counterMeterTags = new();

		PopulateCounterMeterTags(counterMeterTags);

		_counterMeterInstrument = _meter.CreateCounter<int>(name: "countermeter", unit: null, description: null, tags: counterMeterTags);
		global::System.Collections.Generic.Dictionary<string, object?> histogramMeterTags = new();

		PopulateHistogramMeterTags(histogramMeterTags);

		_histogramMeterInstrument = _meter.CreateHistogram<int>(name: "histogrammeter", unit: null, description: null, tags: histogramMeterTags);
		global::System.Collections.Generic.Dictionary<string, object?> upDownCounterMeterTags = new();

		PopulateUpDownCounterMeterTags(upDownCounterMeterTags);

		_upDownCounterMeterInstrument = _meter.CreateUpDownCounter<decimal>(name: "updowncountermeter", unit: null, description: null, tags: upDownCounterMeterTags);
	}

	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	partial void PopulateMeterTags(global::System.Collections.Generic.Dictionary<string, object?> meterTags);

	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	partial void PopulateAutoCounterMeterTags(global::System.Collections.Generic.Dictionary<string, object?> instrumentTags);

	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	partial void PopulateAutoIncrementMeterTags(global::System.Collections.Generic.Dictionary<string, object?> instrumentTags);

	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	partial void PopulateCounterMeterTags(global::System.Collections.Generic.Dictionary<string, object?> instrumentTags);

	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	partial void PopulateHistogramMeterTags(global::System.Collections.Generic.Dictionary<string, object?> instrumentTags);

	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	partial void PopulateUpDownCounterMeterTags(global::System.Collections.Generic.Dictionary<string, object?> instrumentTags);

	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public void AutoCounterMeter(string someValue)
	{
		if (_autoCounterMeterInstrument == null)
		{
			return;
		}

		global::System.Diagnostics.TagList autoCounterMeterTagList = new();

		autoCounterMeterTagList.Add("somevalue", someValue);

		_autoCounterMeterInstrument.Add(1, tagList: autoCounterMeterTagList);
	}
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public void AutoIncrementMeter(string someValue)
	{
		if (_autoIncrementMeterInstrument == null)
		{
			return;
		}

		global::System.Diagnostics.TagList autoIncrementMeterTagList = new();

		autoIncrementMeterTagList.Add("somevalue", someValue);

		_autoIncrementMeterInstrument.Add(1, tagList: autoIncrementMeterTagList);
	}
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public void CounterMeter(int measurement, float someValue)
	{
		if (_counterMeterInstrument == null)
		{
			return;
		}

		global::System.Diagnostics.TagList counterMeterTagList = new();

		counterMeterTagList.Add("somevalue", someValue);

		_counterMeterInstrument.Add(measurement, tagList: counterMeterTagList);
	}
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public void HistogramMeter(int measurement, int someValue, bool anotherValue)
	{
		if (_histogramMeterInstrument == null)
		{
			return;
		}

		global::System.Diagnostics.TagList histogramMeterTagList = new();

		histogramMeterTagList.Add("somevalue", someValue);
		histogramMeterTagList.Add("anothervalue", anotherValue);

		_histogramMeterInstrument.Record(measurement, tagList: histogramMeterTagList);
	}
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public void ObservableCounterMeter(System.Func<float> measurement, double someValue)
	{
		if (_observableCounterMeterInstrument != null)
		{
			return;
		}

		global::System.Diagnostics.TagList observableCounterMeterTagList = new();

		observableCounterMeterTagList.Add("somevalue", someValue);

		_observableCounterMeterInstrument = _meter.CreateObservableCounter<float>("observablecountermeter", measurement, unit: null, description: null
			, tags: observableCounterMeterTagList
		);
	}
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public void ObservableGaugeMeter(global::System.Func<global::System.Diagnostics.Metrics.Measurement<float>> measurement, double someValue)
	{
		if (_observableGaugeMeterInstrument != null)
		{
			return;
		}

		global::System.Diagnostics.TagList observableGaugeMeterTagList = new();

		observableGaugeMeterTagList.Add("somevalue", someValue);

		_observableGaugeMeterInstrument = _meter.CreateObservableGauge<float>("observablegaugemeter", measurement, unit: null, description: null
			, tags: observableGaugeMeterTagList
		);
	}
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public void ObservableUpDownCounter(global::System.Func<global::System.Collections.Generic.IEnumerable<global::System.Diagnostics.Metrics.Measurement<byte>>> measurement, double someValue)
	{
		if (_observableUpDownCounterInstrument != null)
		{
			return;
		}

		global::System.Diagnostics.TagList observableUpDownCounterTagList = new();

		observableUpDownCounterTagList.Add("somevalue", someValue);

		_observableUpDownCounterInstrument = _meter.CreateObservableUpDownCounter<byte>("observableupdowncounter", measurement, unit: null, description: null
			, tags: observableUpDownCounterTagList
		);
	}
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public void UpDownCounterMeter(decimal measurement, byte someValue)
	{
		if (_upDownCounterMeterInstrument == null)
		{
			return;
		}

		global::System.Diagnostics.TagList upDownCounterMeterTagList = new();

		upDownCounterMeterTagList.Add("somevalue", someValue);

		_upDownCounterMeterInstrument.Add(measurement, tagList: upDownCounterMeterTagList);
	}
}
