﻿namespace Purview.Telemetry.Logging;

/// <summary>
/// Marker attribute required for Log generation.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class LogTargetAttribute : Attribute {
	public LogTargetAttribute() {
	}

	public LogTargetAttribute(string messageTemplate) {
		MessageTemplate = messageTemplate;
	}

	public LogTargetAttribute(int eventId) {
		EventId = eventId;
	}

	public LogTargetAttribute(LogGeneratedLevel level, string? messageTemplate = null, string? entryName = null) {
		Level = level;
		MessageTemplate = messageTemplate;
		EntryName = entryName;
	}

	public LogTargetAttribute(int eventId, LogGeneratedLevel level, string? messageTemplate = null, string? entryName = null) {
		Level = level;
		MessageTemplate = messageTemplate;
		EventId = eventId;
		EntryName = entryName;
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
	public string? EntryName { get; set; }
}
