﻿//HintName: MeterGenerationAttribute.g.cs
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

namespace Purview.Telemetry.Metrics;

#if NETSTANDARD1_6_OR_GREATER

[System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed class MeterGenerationAttribute : System.Attribute {
	public MeterGenerationAttribute(string? instrumentPrefix = null, bool lowercaseInstrumentName = true, bool lowercaseTagKeys = true) {
		InstrumentPrefix = instrumentPrefix;
		LowercaseInstrumentName = lowercaseInstrumentName;
		LowercaseTagKeys = lowercaseTagKeys;
	}

	public string? InstrumentPrefix { get; set; }

	public string InstrumentSeparator { get; set; } = ".";

	public bool LowercaseInstrumentName { get; set; } = true;

	public bool LowercaseTagKeys { get; set; } = true;
}

#endif
