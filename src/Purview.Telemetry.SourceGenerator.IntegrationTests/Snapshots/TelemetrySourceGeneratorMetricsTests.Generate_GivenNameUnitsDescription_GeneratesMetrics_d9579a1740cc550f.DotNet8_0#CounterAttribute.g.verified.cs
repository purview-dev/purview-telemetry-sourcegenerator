﻿//HintName: CounterAttribute.g.cs
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

namespace Purview.Telemetry.Metrics;

[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed class CounterAttribute : System.Attribute {
	public CounterAttribute() {
	}

	public CounterAttribute(bool autoIncrement) {
		AutoIncrement = autoIncrement;
	}

	public CounterAttribute(string name, string? unit = null, string? description = null, bool autoIncrement = false) {
		Name = name;
		Unit = unit;
		Description = description;
		AutoIncrement = autoIncrement;
	}

	public string? Name { get; set; }

	public string? Unit { get; set; }

	public string? Description { get; set; }

	public bool AutoIncrement { get; set; }
}