namespace Purview.Telemetry.Metrics;

[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
sealed public class UpDownCounterAttribute : System.Attribute {
	public UpDownCounterAttribute() {
	}

	public UpDownCounterAttribute(string name, string? unit, string? description) {
		Name = name;
		Unit = unit;
		Description = description;
	}

	public string? Name { get; set; }

	public string? Unit { get; set; }

	public string? Description { get; set; }
}
