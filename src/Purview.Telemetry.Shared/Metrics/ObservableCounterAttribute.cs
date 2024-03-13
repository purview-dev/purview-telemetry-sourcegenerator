namespace Purview.Telemetry.Metrics;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
sealed public class ObservableCounterAttribute : MetricAttributeBase {
	public ObservableCounterAttribute() {
	}

	public ObservableCounterAttribute(string name, string? unit = null, string? description = null)
		: base(name, unit, description) {
	}
}
