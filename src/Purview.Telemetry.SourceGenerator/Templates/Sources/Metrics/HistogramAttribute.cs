namespace Purview.Telemetry.Metrics;

[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
sealed public class HistogramAttribute : System.Attribute {
	public HistogramAttribute() {
	}

	public HistogramAttribute(string name, string? unit = null, string? description = null) {
		Name = name;
		Unit = unit;
		Description = description;
	}

	public string? Name { get; set; }

	public string? Unit { get; set; }

	public string? Description { get; set; }
}
