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

		System.Diagnostics.Metrics.Counter<int>? _counterInstrument = null;
		System.Diagnostics.Metrics.Counter<byte>? _counter2Instrument = null;
		System.Diagnostics.Metrics.Counter<long>? _counter3Instrument = null;
		System.Diagnostics.Metrics.Counter<short>? _counter4Instrument = null;
		System.Diagnostics.Metrics.Counter<double>? _counter5Instrument = null;
		System.Diagnostics.Metrics.Counter<float>? _counter6Instrument = null;
		System.Diagnostics.Metrics.Counter<decimal>? _counter7Instrument = null;

		public TestMetricsCore(System.Diagnostics.Metrics.IMeterFactory meterFactory)
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

			System.Collections.Generic.Dictionary<string, object?> counterTags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateCounterTags(counterTags);

			_counterInstrument = _meter.CreateCounter<int>(name: "counter", unit: null, description: null, tags: counterTags);
			System.Collections.Generic.Dictionary<string, object?> counter2Tags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateCounter2Tags(counter2Tags);

			_counter2Instrument = _meter.CreateCounter<byte>(name: "counter2", unit: null, description: null, tags: counter2Tags);
			System.Collections.Generic.Dictionary<string, object?> counter3Tags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateCounter3Tags(counter3Tags);

			_counter3Instrument = _meter.CreateCounter<long>(name: "counter3", unit: null, description: null, tags: counter3Tags);
			System.Collections.Generic.Dictionary<string, object?> counter4Tags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateCounter4Tags(counter4Tags);

			_counter4Instrument = _meter.CreateCounter<short>(name: "counter4", unit: null, description: null, tags: counter4Tags);
			System.Collections.Generic.Dictionary<string, object?> counter5Tags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateCounter5Tags(counter5Tags);

			_counter5Instrument = _meter.CreateCounter<double>(name: "counter5", unit: null, description: null, tags: counter5Tags);
			System.Collections.Generic.Dictionary<string, object?> counter6Tags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateCounter6Tags(counter6Tags);

			_counter6Instrument = _meter.CreateCounter<float>(name: "counter6", unit: null, description: null, tags: counter6Tags);
			System.Collections.Generic.Dictionary<string, object?> counter7Tags = new System.Collections.Generic.Dictionary<string, object?>();

			PopulateCounter7Tags(counter7Tags);

			_counter7Instrument = _meter.CreateCounter<decimal>(name: "counter7", unit: null, description: null, tags: counter7Tags);
		}

		partial void PopulateMeterTags(System.Collections.Generic.Dictionary<string, object?> meterTags);

		partial void PopulateCounterTags(System.Collections.Generic.Dictionary<string, object?> instrumentTags);

		partial void PopulateCounter2Tags(System.Collections.Generic.Dictionary<string, object?> instrumentTags);

		partial void PopulateCounter3Tags(System.Collections.Generic.Dictionary<string, object?> instrumentTags);

		partial void PopulateCounter4Tags(System.Collections.Generic.Dictionary<string, object?> instrumentTags);

		partial void PopulateCounter5Tags(System.Collections.Generic.Dictionary<string, object?> instrumentTags);

		partial void PopulateCounter6Tags(System.Collections.Generic.Dictionary<string, object?> instrumentTags);

		partial void PopulateCounter7Tags(System.Collections.Generic.Dictionary<string, object?> instrumentTags);

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
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Counter2(byte counterValue, int intParam, bool boolParam)
		{
			if (_counter2Instrument == null)
			{
				return;
			}

			System.Diagnostics.TagList counter2TagList = new System.Diagnostics.TagList();

			counter2TagList.Add("intparam", intParam);
			counter2TagList.Add("boolparam", boolParam);

			_counter2Instrument.Add(counterValue, tagList: counter2TagList);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Counter3(long counterValue, int intParam, bool boolParam)
		{
			if (_counter3Instrument == null)
			{
				return;
			}

			System.Diagnostics.TagList counter3TagList = new System.Diagnostics.TagList();

			counter3TagList.Add("intparam", intParam);
			counter3TagList.Add("boolparam", boolParam);

			_counter3Instrument.Add(counterValue, tagList: counter3TagList);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Counter4(short counterValue, int intParam, bool boolParam)
		{
			if (_counter4Instrument == null)
			{
				return;
			}

			System.Diagnostics.TagList counter4TagList = new System.Diagnostics.TagList();

			counter4TagList.Add("intparam", intParam);
			counter4TagList.Add("boolparam", boolParam);

			_counter4Instrument.Add(counterValue, tagList: counter4TagList);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Counter5(double counterValue, int intParam, bool boolParam)
		{
			if (_counter5Instrument == null)
			{
				return;
			}

			System.Diagnostics.TagList counter5TagList = new System.Diagnostics.TagList();

			counter5TagList.Add("intparam", intParam);
			counter5TagList.Add("boolparam", boolParam);

			_counter5Instrument.Add(counterValue, tagList: counter5TagList);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Counter6(float counterValue, int intParam, bool boolParam)
		{
			if (_counter6Instrument == null)
			{
				return;
			}

			System.Diagnostics.TagList counter6TagList = new System.Diagnostics.TagList();

			counter6TagList.Add("intparam", intParam);
			counter6TagList.Add("boolparam", boolParam);

			_counter6Instrument.Add(counterValue, tagList: counter6TagList);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Counter7(decimal counterValue, int intParam, bool boolParam)
		{
			if (_counter7Instrument == null)
			{
				return;
			}

			System.Diagnostics.TagList counter7TagList = new System.Diagnostics.TagList();

			counter7TagList.Add("intparam", intParam);
			counter7TagList.Add("boolparam", boolParam);

			_counter7Instrument.Add(counterValue, tagList: counter7TagList);
		}
	}
}
