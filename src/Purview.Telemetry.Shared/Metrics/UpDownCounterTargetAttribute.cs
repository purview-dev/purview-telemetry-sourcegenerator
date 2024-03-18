namespace Purview.Telemetry.Metrics;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
sealed public class UpDownCounterTargetAttribute : InstrumentAttributeBase {
	public UpDownCounterTargetAttribute() {
	}

	public UpDownCounterTargetAttribute(string name, string? unit, string? description)
		: base(name, unit, description) {
	}
}
