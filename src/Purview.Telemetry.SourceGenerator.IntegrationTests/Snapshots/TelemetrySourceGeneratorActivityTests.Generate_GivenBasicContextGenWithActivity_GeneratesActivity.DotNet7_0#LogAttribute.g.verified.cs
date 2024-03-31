﻿//HintName: LogAttribute.g.cs
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Purview.Telemetry.SourceGenerator
//     on {Scrubbed}.
//
//     Changes to this file may cause incorrect behaviour and will be lost
//     when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable 1591 // publicly visible type or member must be documented

#if PURVIEW_TELEMETRY_ATTRIBUTES

namespace Purview.Telemetry.Logging;

/// <summary>
/// Marker attribute required for Log generation.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed class LogAttribute : Attribute {
	public LogAttribute() {
	}

	public LogAttribute(string messageTemplate) {
		MessageTemplate = messageTemplate;
	}

	public LogAttribute(int eventId) {
		EventId = eventId;
	}

	public LogAttribute(LogGeneratedLevel level, string? messageTemplate = null, string? name = null) {
		Level = level;
		MessageTemplate = messageTemplate;
		Name = name;
	}

	public LogAttribute(int eventId, LogGeneratedLevel level, string? messageTemplate = null, string? name = null) {
		Level = level;
		MessageTemplate = messageTemplate;
		EventId = eventId;
		Name = name;
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

#endif
