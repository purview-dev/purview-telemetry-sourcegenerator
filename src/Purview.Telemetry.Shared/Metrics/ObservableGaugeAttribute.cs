namespace Purview.Telemetry.Metrics;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
sealed public class ObservableGaugeAttribute : MetricAttributeBase {
	public ObservableGaugeAttribute() {
	}

	public ObservableGaugeAttribute(string name, string? unit = null, string? description = null)
		: base(name, unit, description) {
	}
}
