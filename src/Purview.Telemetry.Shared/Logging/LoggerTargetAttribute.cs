using System.Diagnostics;

namespace Purview.Telemetry.Logging;

/// <summary>
/// Marker attribute required for Log generation.
/// </summary>
[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
[Conditional(Constants.EmbedAttributesHashDefineName)]
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

	public LoggerTargetAttribute(LogGeneratedLevel defaultLevel, string? className = null) {
		DefaultLevel = defaultLevel;
		ClassName = className;
	}

	/// <summary>
	/// The inclusive starting event id for this logger.
	/// </summary>
	public int StartingEventId { get; set; }

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
}
