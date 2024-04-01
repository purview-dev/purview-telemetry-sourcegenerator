namespace Purview.Telemetry.Logging;

/// <summary>
/// Marker attribute required for Log generation.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Interface, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class LoggerAttribute : System.Attribute {
	public LoggerAttribute() {

	}

	public LoggerAttribute(Microsoft.Extensions.Logging.LogLevel defaultLevel, string? customPrefix = null) {
		DefaultLevel = defaultLevel;
		CustomPrefix = customPrefix;

		if (CustomPrefix != null) {
			PrefixType = LogPrefixType.Custom;
		}
	}

	/// <summary>
	/// Optional. Gets the <see cref="Microsoft.Extensions.Logging.LogLevel">level</see> of the
	/// log entry. Defaults to <see cref="Microsoft.Extensions.Logging.LogLevel.Information"/>, unless there is
	/// an <see cref="Exception"/> parameter and no-other override is defined.
	/// </summary>
	public Microsoft.Extensions.Logging.LogLevel DefaultLevel { get; set; } = Microsoft.Extensions.Logging.LogLevel.Information;

	/// <summary>
	/// Optional. The prefix used to generate the log entry.
	/// </summary>
	public string? CustomPrefix { get; set; }

	/// <summary>
	/// Specifies the mode used to generate or override the prefix for the log entry.
	/// </summary>
	public LogPrefixType PrefixType { get; set; } = LogPrefixType.Default;
}
