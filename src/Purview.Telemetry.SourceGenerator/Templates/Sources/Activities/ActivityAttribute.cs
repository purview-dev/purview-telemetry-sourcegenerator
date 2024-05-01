namespace Purview.Telemetry.Activities;

/// <summary>
/// Marker attribute used to control the generation
/// of <see cref="System.Diagnostics.Activity">activities</see>.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed class ActivityAttribute : System.Attribute
{
	/// <summary>
	/// Constructs a new <see cref="ActivityAttribute"/>.
	/// </summary>
	public ActivityAttribute()
	{
	}

	/// <summary>
	/// Constructs a new <see cref="ActivityAttribute"/> and specifies the <see cref="Name"/>.
	/// </summary>
	/// <param name="name">Specifies the <see cref="Name"/>.</param>
	public ActivityAttribute(string name)
	{
		Name = name;
	}

	/// <summary>
	/// Constructs a new <see cref="ActivityAttribute"/> and specifies the <see cref="Kind"/>.
	/// </summary>
	/// <param name="kind">Specifies the <see cref="Kind"/>.</param>
	public ActivityAttribute(System.Diagnostics.ActivityKind kind)
	{
		Kind = kind;
	}

	/// <summary>
	/// Constructs a new <see cref="ActivityAttribute"/> and specifies the <see cref="Name" /> and
	/// optionally the <see cref="Kind"/> and/ or <see cref="CreateOnly"/>.
	/// </summary>
	/// <param name="name">Specifies the <see cref="Name"/>.</param>
	/// <param name="kind">Optionally specifies the <see cref="Kind"/>.</param>
	/// <param name="createOnly">Optionally specifies <see cref="CreateOnly"/>.</param>
	public ActivityAttribute(string name, System.Diagnostics.ActivityKind kind, bool createOnly = false)
	{
		Name = name;
		Kind = kind;
		CreateOnly = createOnly;
	}

	/// <summary>
	/// Optional. Gets/ sets the name of the <see cref="System.Diagnostics.Activity"/>.
	/// If this is not specified, the name of the method is used.
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	/// Optional. Gets/ sets the <see cref="System.Diagnostics.ActivityKind">kind</see> of the
	/// activity. Defaults to <see cref="System.Diagnostics.ActivityKind.Internal"/>.
	/// </summary>
	public System.Diagnostics.ActivityKind Kind { get; set; } = System.Diagnostics.ActivityKind.Internal;

	/// <summary>
	/// If true, the <see cref="System.Diagnostics.Activity"/> is created using
	/// <see cref="System.Diagnostics.ActivitySource.CreateActivity(string, System.Diagnostics.ActivityKind, string?, IEnumerable{KeyValuePair{string, object?}}?, IEnumerable{System.Diagnostics.ActivityLink}?, System.Diagnostics.ActivityIdFormat)" />, meaning it is not started by default. Otherwise
	/// <see cref="System.Diagnostics.ActivitySource.StartActivity(string, System.Diagnostics.ActivityKind, string?, IEnumerable{KeyValuePair{string, object?}}?, IEnumerable{System.Diagnostics.ActivityLink}?, DateTimeOffset)" />is used. The default is false.
	/// </summary>
	public bool CreateOnly { get; set; }
}
