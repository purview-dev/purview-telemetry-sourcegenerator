﻿namespace Purview.Telemetry.Logging;

#if !EXCLUDE_PURVIEW_TELEMETRY_LOGGING

/// <summary>
/// Marker attribute required for Log generation.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Interface, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed class LoggerAttribute : System.Attribute
{
	/// <summary>
	/// Creates a new instance of the <see cref="LoggerAttribute"/> class.
	/// </summary>
	public LoggerAttribute()
	{

	}

	/// <summary>
	/// Creates a new instance of the <see cref="LoggerAttribute"/>, specifying the <see cref="DefaultLevel"/>
	/// and optionally the <see cref="CustomPrefix"/>.
	/// </summary>
	/// <param name="defaultLevel">The default <see cref="Microsoft.Extensions.Logging.LogLevel"/> to use
	/// when one is not specified.</param>
	/// <param name="customPrefix">If specified, also sets the <see cref="PrefixType"/> to <see cref="LogPrefixType.Custom"/>.</param>
	/// <param name="disableMSLoggingTelemetryGeneration">Disables the generation of the new style of telemetry generation for Microsoft.Extensions.Logging.</param>
	public LoggerAttribute(Microsoft.Extensions.Logging.LogLevel defaultLevel, string? customPrefix = null, bool disableMSLoggingTelemetryGeneration = false)
	{
		DefaultLevel = defaultLevel;
		CustomPrefix = customPrefix;
		DisableMSLoggingTelemetryGeneration = disableMSLoggingTelemetryGeneration;

		if (CustomPrefix != null)
		{
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
	/// Optional. The prefix used to when generating the log entry name.
	/// </summary>
	public string? CustomPrefix { get; set; }

	/// <summary>
	/// Specifies the mode used to generate or override the prefix for the log entry.
	/// </summary>
	public LogPrefixType PrefixType { get; set; } = LogPrefixType.Default;

	/// <summary>
	/// Disables the generation of the new style of telemetry generation for Microsoft.Extensions.Logging.
	/// 
	/// Defaults to false.
	/// </summary>
	public bool DisableMSLoggingTelemetryGeneration { get; set; }
}

#endif
