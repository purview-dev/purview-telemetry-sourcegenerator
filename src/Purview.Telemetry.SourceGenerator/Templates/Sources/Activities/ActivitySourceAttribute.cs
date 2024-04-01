namespace Purview.Telemetry.Activities;

/// <summary>
/// Marker attribute required for Activity generation.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Interface, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class ActivitySourceAttribute : System.Attribute {
	/// <summary>
	/// Constructs a new <see cref="ActivitySourceAttribute"/>.
	/// </summary>
	public ActivitySourceAttribute() {
	}

	/// <summary>
	/// Constructs a new <see cref="ActivitySourceAttribute"/>.
	/// </summary>
	/// <param name="name">The <see cref="Name"/>.</param>
	public ActivitySourceAttribute(string name) {
		Name = name;
	}

	/// <summary>
	/// Overrides the generated activity source name.
	/// </summary>
	public string? Name { get; set; }

	public bool DefaultToTags { get; set; } = true;

	public string? BaggageAndTagPrefix { get; set; }

	public bool IncludeActivitySourcePrefix { get; set; } = true;

	public bool LowercaseBaggageAndTagKeys { get; set; } = true;
}
