﻿namespace Purview.Telemetry.Metrics;

[System.AttributeUsage(System.AttributeTargets.Interface | System.AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class MeterAttribute : System.Attribute {
	public MeterAttribute() {
	}

	public MeterAttribute(string name) {
		Name = name;
	}

	/// <summary>
	/// Optional. Gets/ sets the name of the metric.
	/// </summary>
	public string? Name { get; set; }

	public string? InstrumentPrefix { get; set; }

	public bool IncludeAssemblyInstrumentPrefix { get; set; } = true;

	public bool LowercaseInstrumentName { get; set; } = true;

	public bool LowercaseTagKeys { get; set; } = true;
}