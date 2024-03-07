namespace Purview.Telemetry.Logging;

/// <summary>
/// Marker attribute required for Log generation.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class LoggerEntryAttribute : Attribute {
	public LoggerEntryAttribute() {

	}

	public LoggerEntryAttribute(string messageTemplate) {
		MessageTemplate = messageTemplate;
	}

	public LoggerEntryAttribute(int eventId) {
		EventId = eventId;
	}

	public LoggerEntryAttribute(LogGeneratedLevel level, string? messageTemplate = null, int? eventId = null) {
		Level = level;
		MessageTemplate = messageTemplate;
		EventId = eventId;
	}

	/// <summary>
	/// Optional. Gets/ sets the <see cref="LogGeneratedLevel">level</see> of the
	/// log entry. Defaults to <see cref="LogGeneratedLevel.Information"/>, unless there is
	/// an <see cref="Exception"/> parameter and no-other override is defined.
	/// </summary>
	public LogGeneratedLevel Level { get; set; } = LogGeneratedLevel.Information;

	/// <summary>
	/// Optional. The message template used for the log entry, otherwise one is
	/// generated based on the parameters.
	/// </summary>
	public string? MessageTemplate { get; set; }

	/// <summary>
	/// Optional. The event Id for this log entry. If one is not specified, one is automatically generated.
	/// </summary>
	public int? EventId { get; set; }

	/// <summary>
	/// Optional. Gets/ set the name of the log entry. If one is not specified, the method name is used.
	/// </summary>
	public string? Name { get; set; }
}
