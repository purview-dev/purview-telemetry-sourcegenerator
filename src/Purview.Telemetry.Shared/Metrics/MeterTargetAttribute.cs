namespace Purview.Telemetry.Metrics;

[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class MeterTargetAttribute : Attribute {
	public MeterTargetAttribute() {
	}

	public MeterTargetAttribute(string name) {
		Name = name;
	}

	/// <summary>
	/// Optional. Gets/ sets the name of the metric.
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	/// Overrides the class name generated for this activity target.
	/// </summary>
	public string? ClassName { get; set; }

	public string? InstrumentPrefix { get; set; }

	public bool IncludeAssemblyInstrumentPrefix { get; set; } = true;

	public bool LowercaseInstrumentName { get; set; } = true;

	public bool LowercaseTagKeys { get; set; } = true;
}
