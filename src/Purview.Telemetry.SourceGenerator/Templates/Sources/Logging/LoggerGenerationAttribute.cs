namespace Purview.Telemetry.Logging;

#if !EXCLUDE_PURVIEW_TELEMETRY_LOGGING

/// <summary>
/// Sets defaults for the generation of loggers and log entries.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed class LoggerGenerationAttribute : System.Attribute
{
	/// <summary>
	/// Creates a new instance of <see cref="LoggerGenerationAttribute"/>.
	/// </summary>
	public LoggerGenerationAttribute()
	{
	}

	/// <summary>
	/// Creates a new instance of <see cref="LoggerGenerationAttribute"/> with the specified
	/// <see cref="DefaultLevel"/>.
	/// </summary>
	/// <param name="defaultLevel"></param>
	public LoggerGenerationAttribute(Microsoft.Extensions.Logging.LogLevel defaultLevel)
	{
		DefaultLevel = defaultLevel;
	}

	/// <summary>
	/// Gets/ sets the default <see cref="Microsoft.Extensions.Logging.LogLevel">level</see> of the
	/// logger. Defaults to <see cref="Microsoft.Extensions.Logging.LogLevel.Information"/>.
	/// </summary>
	public Microsoft.Extensions.Logging.LogLevel DefaultLevel { get; set; } = Microsoft.Extensions.Logging.LogLevel.Information;
}

#endif
