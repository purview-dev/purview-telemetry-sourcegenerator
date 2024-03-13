namespace Purview.Telemetry.Metrics;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class MeasurementTagAttribute : Attribute {
	public MeasurementTagAttribute() {
	}

	public MeasurementTagAttribute(string name) {
		Name = name;
	}

	/// <summary>
	/// Optional. Gets the overridden name of the tag.
	/// </summary>
	public string? Name { get; set; }
}
