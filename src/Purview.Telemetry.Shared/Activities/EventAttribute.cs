namespace Purview.Telemetry.Activities;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class EventAttribute : Attribute {

	public EventAttribute() {
	}

	public EventAttribute(string name, bool useRecordExceptionRules = Constants.Activities.UseRecordExceptionRulesDefault, bool recordExceptionEscape = Constants.Activities.RecordExceptionEscapeDefault) {
		Name = name;
		UseRecordExceptionRules = useRecordExceptionRules;
		RecordExceptionEscape = recordExceptionEscape;
	}

	/// <summary>
	/// Optional. Gets/ sets the name of the event.
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	/// Determines if the event should use OpenTelemetry exception recording rules.
	/// </summary>
	public bool UseRecordExceptionRules { get; set; } = Constants.Activities.UseRecordExceptionRulesDefault;

	/// <summary>
	/// Determines if a recorded exception (when <see cref="UseRecordExceptionRules"/> is true and an exception parameter exists)
	/// if the exception prevented the operation from completing (true) or if the exception was caught and handled (false)
	/// and did not affect the operation. Alternatives using the <see cref="EscapeAttribute"/> can override this value dynamically.
	/// </summary>
	public bool RecordExceptionEscape { get; set; } = Constants.Activities.RecordExceptionEscapeDefault;
}
