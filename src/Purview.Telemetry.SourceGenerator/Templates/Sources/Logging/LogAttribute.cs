﻿namespace Purview.Telemetry.Logging;

/// <summary>
/// Marker attribute required for log entry generation, based on
/// high-performance <see cref="Microsoft.Extensions.Logging.LoggerMessage"/>.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class LogAttribute : System.Attribute {
	/// <summary>
	/// Creates a new instance of the <see cref="LogAttribute"/> class.
	/// </summary>
	public LogAttribute() {
	}

	/// <summary>
	/// Creates a new instance of the <see cref="LogAttribute"/>, specifying the <see cref="MessageTemplate"/>.
	/// </summary>
	/// <param name="messageTemplate">Specifies the <see cref="MessageTemplate"/>.</param>
	public LogAttribute(string messageTemplate) {
		MessageTemplate = messageTemplate;
	}

	/// <summary>
	/// Creates a new instance of the <see cref="LogAttribute"/>, specifying the <see cref="EventId"/>.
	/// </summary>
	/// <param name="eventId">Specifies the <see cref="EventId"/>.</param>
	public LogAttribute(int eventId) {
		EventId = eventId;
	}

	/// <summary>
	/// Creates a new instance of the <see cref="LogAttribute"/>, specifying the <see cref="Level"/>,
	/// optionally the <see cref="MessageTemplate"/> and <see cref="Name"/>.
	/// </summary>
	/// <param name="level">Specifies the <see cref="Level"/>.</param>
	/// <param name="messageTemplate">Optionally specifies the <see cref="MessageTemplate"/>.</param>
	/// <param name="name">Optionally specifies the <see cref="Name"/>.</param>
	public LogAttribute(Microsoft.Extensions.Logging.LogLevel level, string? messageTemplate = null, string? name = null) {
		Level = level;
		MessageTemplate = messageTemplate;
		Name = name;
	}

	/// <summary>
	/// Creates a new instance of the <see cref="LogAttribute"/>, specifying the <see cref="EventId"/>
	/// and the <see cref="Level"/>, optionally the <see cref="MessageTemplate"/> and <see cref="Name"/>.
	/// </summary>
	/// <param name="eventId">Specifies the <see cref="EventId"/>.</param>
	/// <param name="level">Specifies the <see cref="Level"/>.</param>
	/// <param name="messageTemplate">Optionally specifies the <see cref="MessageTemplate"/>.</param>
	/// <param name="name">Optionally specifies the <see cref="Name"/>.</param>
	public LogAttribute(int eventId, Microsoft.Extensions.Logging.LogLevel level, string? messageTemplate = null, string? name = null) {
		Level = level;
		MessageTemplate = messageTemplate;
		EventId = eventId;
		Name = name;
	}

	/// <summary>
	/// Optional. Gets/ sets the <see cref="Microsoft.Extensions.Logging.LogLevel">level</see> of the
	/// log entry. Defaults to <see cref="Microsoft.Extensions.Logging.LogLevel.Information"/>, unless there is
	/// an <see cref="Exception"/> parameter and no-other override is defined.
	/// </summary>
	public Microsoft.Extensions.Logging.LogLevel Level { get; set; } = Microsoft.Extensions.Logging.LogLevel.Information;

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
