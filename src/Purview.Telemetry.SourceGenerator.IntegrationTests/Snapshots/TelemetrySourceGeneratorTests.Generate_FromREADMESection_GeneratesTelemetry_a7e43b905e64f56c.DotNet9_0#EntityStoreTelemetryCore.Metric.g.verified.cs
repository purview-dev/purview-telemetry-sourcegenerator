﻿//HintName: EntityStoreTelemetryCore.Metric.g.cs
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
sealed partial class EntityStoreTelemetryCore : global::IEntityStoreTelemetry
{
	global::System.Diagnostics.Metrics.Meter _meter = default!;

	global::System.Diagnostics.Metrics.Counter<int>? _retrievingEntityInstrument = null;

	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	public EntityStoreTelemetryCore(global::Microsoft.Extensions.Logging.ILogger<global::IEntityStoreTelemetry> logger, global::System.Diagnostics.Metrics.IMeterFactory meterFactory)
	{
		_logger = logger;
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

		_meter = meterFactory.Create(new global::System.Diagnostics.Metrics.MeterOptions("EntityStoreTelemetry")
		{
			Version = null,
			Tags = meterTags
		});

		global::System.Collections.Generic.Dictionary<string, object?> retrievingEntityTags = new();

		PopulateRetrievingEntityTags(retrievingEntityTags);

		_retrievingEntityInstrument = _meter.CreateCounter<int>(name: "retrievingentity", unit: null, description: null, tags: retrievingEntityTags);
	}

	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	partial void PopulateMeterTags(global::System.Collections.Generic.Dictionary<string, object?> meterTags);

	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	partial void PopulateRetrievingEntityTags(global::System.Collections.Generic.Dictionary<string, object?> instrumentTags);

	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public void RetrievingEntity(int entityId)
	{
		if (_retrievingEntityInstrument == null)
		{
			return;
		}

		global::System.Diagnostics.TagList retrievingEntityTagList = new();

		retrievingEntityTagList.Add("entityid", entityId);

		_retrievingEntityInstrument.Add(1, tagList: retrievingEntityTagList);
	}
}
