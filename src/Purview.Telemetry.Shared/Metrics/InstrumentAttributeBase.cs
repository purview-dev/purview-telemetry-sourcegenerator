namespace Purview.Telemetry.Metrics;

#if NETSTANDARD1_6_OR_GREATER

[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
abstract public class InstrumentAttributeBase : Attribute {
	protected InstrumentAttributeBase() {
	}

	protected InstrumentAttributeBase(string name, string? unit = null, string? description = null) {
		Name = name;
		Unit = unit;
		Description = description;
	}

	public string? Name { get; set; }

	public string? Unit { get; set; }

	public string? Description { get; set; }
}

#endif
