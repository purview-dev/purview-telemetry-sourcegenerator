namespace Purview.Telemetry.Metrics;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class CounterAttribute : InstrumentAttributeBase {
	public CounterAttribute() {
	}

	public CounterAttribute(bool autoIncrement) {
		AutoIncrement = autoIncrement;
	}

	public CounterAttribute(string name, string? unit = null, string? description = null, bool autoIncrement = false)
		: base(name, unit, description) {
		AutoIncrement = autoIncrement;
	}

	public bool AutoIncrement { get; set; }
}
