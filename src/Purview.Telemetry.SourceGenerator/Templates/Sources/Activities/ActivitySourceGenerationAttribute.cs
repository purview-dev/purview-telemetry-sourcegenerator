namespace Purview.Telemetry.Activities;

/// <summary>
/// Determines the default Activity Source name for generated Activities.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class ActivitySourceGenerationAttribute : System.Attribute {
	/// <summary>
	/// Constructs a new <see cref="ActivitySourceGenerationAttribute"/>.
	/// </summary>
	/// <param name="name">The name of the activity source.</param>
	/// <param name="defaultToTags">Determines if the default for method parameters are Tags (default) or Baggage.</param>
	/// <exception cref="ArgumentNullException">If the <paramref name="name"/> is null, empty or whitespace.</exception>
	public ActivitySourceGenerationAttribute(string name, bool defaultToTags = true) {
		if (string.IsNullOrWhiteSpace(name)) {
			throw new System.ArgumentNullException(nameof(name));
		}

		Name = name;
		DefaultToTags = defaultToTags;
	}

	/// <summary>
	/// The default Activity Source name to use.
	/// </summary>
	public string Name { get; }

	public bool DefaultToTags { get; set; } = true;

	public string? BaggageAndTagPrefix { get; set; }

	public string BaggageAndTagSeparator { get; set; } = ".";

	public bool LowercaseBaggageAndTagKeys { get; set; } = true;
}
