namespace Purview.Telemetry.Activities;

/// <summary>
/// Marker attribute required for explicitly setting a
/// parameter as baggage when generating and <see cref="System.Diagnostics.Activity"/>
/// or an <see cref="System.Diagnostics.ActivityEvent"/>.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Parameter, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class BaggageAttribute : System.Attribute {
	/// <summary>
	/// Create a new <see cref="BaggageAttribute"/>.
	/// </summary>
	public BaggageAttribute() {
	}

	/// <summary>
	/// Create a new <see cref="BaggageAttribute"/> and sets the <see cref="SkipOnNullOrEmpty"/>
	/// property.
	/// </summary>
	public BaggageAttribute(bool skipOnNullOrEmpty) {
		SkipOnNullOrEmpty = skipOnNullOrEmpty;
	}

	/// <summary>
	/// Create a new <see cref="BaggageAttribute"/> and sets the <see cref="Name"/>
	/// and optionally the <see cref="SkipOnNullOrEmpty"/> property.
	/// </summary>
	/// <param name="name">Sets the <see cref="Name"/>.</param>
	/// <param name="skipOnNullOrEmpty">Optionally sets the <see cref="SkipOnNullOrEmpty"/> (defaults to false).</param>
	public BaggageAttribute(string? name, bool skipOnNullOrEmpty = false) {
		Name = name;
		SkipOnNullOrEmpty = skipOnNullOrEmpty;
	}

	/// <summary>
	/// Specifies the name of the baggage item. If null, empty or whitespace
	/// defaults to the name of the parameter.
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	/// Determines if the parameter should be skipped when the value is a default value.
	/// </summary>
	public bool SkipOnNullOrEmpty { get; set; } = false;
}
