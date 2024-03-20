namespace Purview.Telemetry.Logging;

/// <summary>
/// Marker attribute required for Log generation.
/// </summary>
[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class LoggerAttribute : Attribute {
	public LoggerAttribute() {

	}

	public LoggerAttribute(LogGeneratedLevel defaultLevel, string? customPrefix = null) {
		DefaultLevel = defaultLevel;
		CustomPrefix = customPrefix;

		if (CustomPrefix != null) {
			PrefixType = LogPrefixType.Custom;
		}
	}

	/// <summary>
	/// Optional. Gets the <see cref="LogGeneratedLevel">level</see> of the
	/// log entry. Defaults to <see cref="LogGeneratedLevel.Information"/>, unless there is
	/// an <see cref="Exception"/> parameter and no-other override is defined.
	/// </summary>
	public LogGeneratedLevel DefaultLevel { get; set; } = LogGeneratedLevel.Information;

	/// <summary>
	/// Optional. The prefix used to generate the log entry.
	/// </summary>
	public string? CustomPrefix { get; set; }

	/// <summary>
	/// Specifies the mode used to generate or override the prefix for the log entry.
	/// </summary>
	public LogPrefixType PrefixType { get; set; } = LogPrefixType.Default;
}
