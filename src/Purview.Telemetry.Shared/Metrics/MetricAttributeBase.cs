namespace Purview.Telemetry.Metrics;

[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
abstract public class MetricAttributeBase : Attribute {
	protected MetricAttributeBase() {
	}

	protected MetricAttributeBase(string name, string? unit = null, string? description = null) {
		Name = name;
		Unit = unit;
		Description = description;
	}

	public string? Name { get; set; }

	public string? Unit { get; set; }

	public string? Description { get; set; }
}
