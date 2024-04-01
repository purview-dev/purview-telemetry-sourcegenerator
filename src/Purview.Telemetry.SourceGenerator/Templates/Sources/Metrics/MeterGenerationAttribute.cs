namespace Purview.Telemetry.Metrics;

[System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class MeterGenerationAttribute : System.Attribute {
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
