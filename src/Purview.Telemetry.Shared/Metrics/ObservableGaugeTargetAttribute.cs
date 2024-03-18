namespace Purview.Telemetry.Metrics;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class ObservableGaugeTargetAttribute : InstrumentAttributeBase {
	public ObservableGaugeTargetAttribute() {
	}

	public ObservableGaugeTargetAttribute(string name, string? unit = null, string? description = null, bool throwOnAlreadyInitialized = false)
		: base(name, unit, description) {
		ThrowOnAlreadyInitialized = throwOnAlreadyInitialized;
	}

	public bool ThrowOnAlreadyInitialized { get; set; }
}
