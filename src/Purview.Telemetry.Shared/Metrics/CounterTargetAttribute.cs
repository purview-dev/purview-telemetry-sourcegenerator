namespace Purview.Telemetry.Metrics;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class CounterTargetAttribute : InstrumentAttributeBase {
	public CounterTargetAttribute() {
	}

	public CounterTargetAttribute(bool autoIncrement) {
		AutoIncrement = autoIncrement;
	}

	public CounterTargetAttribute(string name, string? unit = null, string? description = null, bool autoIncrement = false)
		: base(name, unit, description) {
		AutoIncrement = autoIncrement;
	}

	public bool AutoIncrement { get; set; }
}
