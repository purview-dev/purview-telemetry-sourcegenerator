namespace Purview.Telemetry.Metrics;

[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class ObservableGaugeAttribute : System.Attribute {
	public ObservableGaugeAttribute() {
	}

	public ObservableGaugeAttribute(string name, string? unit = null, string? description = null, bool throwOnAlreadyInitialized = false) {
		Name = name;
		Unit = unit;
		Description = description;
		ThrowOnAlreadyInitialized = throwOnAlreadyInitialized;
	}

	public string? Name { get; set; }

	public string? Unit { get; set; }

	public string? Description { get; set; }

	public bool ThrowOnAlreadyInitialized { get; set; }
}
