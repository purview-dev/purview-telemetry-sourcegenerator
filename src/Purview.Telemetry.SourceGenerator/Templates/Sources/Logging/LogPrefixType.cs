namespace Purview.Telemetry.Logging;

/// <summary>
/// The types of prefixes that can be used for the log entry name generation.
/// </summary>
public enum LogPrefixType {
	/// <summary>
	/// The name of the interface without the "I" prefix or "Log", "Logger" or "Telemetry" suffixes.
	/// </summary>
	Default,

	/// <summary>
	/// The name of the interface.
	/// </summary>
	Interface,

	/// <summary>
	/// The name of the class either specified or generated.
	/// </summary>
	Class,

	/// <summary>
	/// Uses the custom name specified by <see cref="LoggerAttribute.CustomPrefix"/>. This is used when
	/// the <see cref="LoggerAttribute.CustomPrefix"/> is set
	/// regardless of <see cref="LoggerAttribute.PrefixType"/>.
	/// </summary>
	Custom,

	/// <summary>
	/// No suffix is added to the name.
	/// </summary>
	NoSuffix
}
