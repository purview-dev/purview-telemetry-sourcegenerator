namespace Purview.Telemetry.Logging;

/// <summary>
/// Marker attribute required for Log generation.
/// </summary>
[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class LoggerTargetAttribute : Attribute {
	public LoggerTargetAttribute() {

	}

	public LoggerTargetAttribute(string className) {
		ClassName = className;
	}

	public LoggerTargetAttribute(int startingEventId) {
		StartingEventId = startingEventId;
	}

	public LoggerTargetAttribute(LogGeneratedLevel defaultLevel, string? className = null, string? customPrefix = null) {
		DefaultLevel = defaultLevel;
		ClassName = className;
		CustomPrefix = customPrefix;

		if (CustomPrefix != null) {
			PrefixType = LogPrefixType.Custom;
		}
	}

	/// <summary>
	/// The inclusive starting event Id for this logger, all other non-specified event Ids
	/// for this logger will be an incrementing value from this starting event Id.
	/// </summary>
	public int StartingEventId { get; set; } = 1;

	/// <summary>
	/// Optional. Gets the <see cref="LogGeneratedLevel">level</see> of the
	/// log entry. Defaults to <see cref="LogGeneratedLevel.Information"/>, unless there is
	/// an <see cref="Exception"/> parameter and no-other override is defined.
	/// </summary>
	public LogGeneratedLevel DefaultLevel { get; set; } = LogGeneratedLevel.Information;

	/// <summary>
	/// Overrides the class name generated for this activity target.
	/// </summary>
	public string? ClassName { get; set; }

	/// <summary>
	/// Optional. The prefix used to generate the log entry.
	/// </summary>
	public string? CustomPrefix { get; set; }

	/// <summary>
	/// Specifies the mode used to generate or override the prefix for the log entry.
	/// </summary>
	public LogPrefixType PrefixType { get; set; } = LogPrefixType.Default;
}
