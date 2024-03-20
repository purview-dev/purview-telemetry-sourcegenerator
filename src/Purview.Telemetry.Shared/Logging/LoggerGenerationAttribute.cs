﻿namespace Purview.Telemetry.Logging;

/// <summary>
/// Sets defaults for the generation of loggers and log entries.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class LoggerGenerationAttribute : Attribute {
	public LoggerGenerationAttribute() {
	}

	public LoggerGenerationAttribute(LogGeneratedLevel defaultLevel) {
		DefaultLevel = defaultLevel;
	}

	/// <summary>
	/// Gets/ sets the default <see cref="LogGeneratedLevel">level</see> of the
	/// logger. Defaults to <see cref="LogGeneratedLevel.Information"/>.
	/// </summary>
	public LogGeneratedLevel DefaultLevel { get; set; } = LogGeneratedLevel.Information;
}