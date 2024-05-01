namespace Purview.Telemetry.Activities;

/// <summary>
/// Marker attribute used to control the generation
/// of <see cref="System.Diagnostics.ActivityEvent">events</see>.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed class EventAttribute : System.Attribute
{
	/// <summary>
	/// Generates a new <see cref="EventAttribute"/>.
	/// </summary>
	public EventAttribute()
	{
	}

	/// <summary>
	/// Generates a new <see cref="EventAttribute"/>, specifying the <see cref="Name"/> and optionally
	/// the <see cref="UseRecordExceptionRules"/> property and/ or <see cref="RecordExceptionAsEscaped"/>.
	/// </summary>
	public EventAttribute(string name, bool useRecordExceptionRules = true, bool recordExceptionAsEscaped = true)
	{
		Name = name;
		UseRecordExceptionRules = useRecordExceptionRules;
		RecordExceptionAsEscaped = recordExceptionAsEscaped;
	}

	/// <summary>
	/// Optional. Gets/ sets the name of the event. If null, empty or whitespace
	/// then the name of the method is used.
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	/// Determines if the event should use OpenTelemetry exception recording rules.
	/// </summary>
	public bool UseRecordExceptionRules { get; set; } = true;

	/// <summary>
	/// Determines if a recorded exception (when <see cref="UseRecordExceptionRules"/> is true and an exception parameter exists)
	/// if the exception prevented the operation from completing (true) or if the exception was caught and handled (false)
	/// and did not affect the operation. Alternatively, use the <see cref="EscapeAttribute"/> to override this value by
	/// providing a value dynamically.
	/// </summary>
	public bool RecordExceptionAsEscaped { get; set; } = true;
}
