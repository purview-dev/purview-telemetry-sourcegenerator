namespace Purview.Telemetry.Logging;

#if !EXCLUDE_PURVIEW_TELEMETRY_LOGGING

/// <summary>
/// The types of prefixes that can be used for the log entry name generation.
/// </summary>
enum LogPrefixType
{
	/// <summary>
	/// The name of the interface without the "I" prefix or "Log", "Logger" or "Telemetry" suffixes.
	/// 
	/// For example, IRepositoryLog, IRepositoryLogger, or IRepositoryTelemetry would all be "Repository". 
	/// </summary>
	Default = 0,

	/// <summary>
	/// The name of the interface used as the source for generation.
	/// </summary>
	Interface,

	/// <summary>
	/// The name of the class, either specified or generated.
	/// </summary>
	Class,

	/// <summary>
	/// Uses the custom name specified by <see cref="global::Purview.Telemetry.Logging.LoggerAttribute.CustomPrefix"/>. This is used when
	/// the <see cref="global::Purview.Telemetry.Logging.LoggerAttribute.CustomPrefix"/> is set
	/// regardless of <see cref="global::Purview.Telemetry.Logging.LoggerAttribute.PrefixType"/>.
	/// </summary>
	Custom,

	/// <summary>
	/// No suffix is added to the name.
	/// </summary>
	NoSuffix
}

#endif
