namespace Purview.Telemetry.Metrics;

#if NETSTANDARD1_6_OR_GREATER

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
sealed public class UpDownCounterAttribute : InstrumentAttributeBase {
	public UpDownCounterAttribute() {
	}

	public UpDownCounterAttribute(string name, string? unit, string? description)
		: base(name, unit, description) {
	}
}

#endif
