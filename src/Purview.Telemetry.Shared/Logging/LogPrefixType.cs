namespace Purview.Telemetry.Logging;

/// <summary>
/// The types of prefixes that can be used for the log entry.
/// </summary>
public enum LogPrefixType {
	/// <summary>
	/// The name of the interface without the "I" prefix and "Log", "Logger" or "Telemetry" suffix.
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
	/// Uses the custom name specified in the attribute. This is used when
	/// the <see cref="LoggerTargetAttribute.CustomPrefix"/> is specified
	/// regardless of <see cref="LoggerTargetAttribute.PrefixType"/>.
	/// </summary>
	Custom,

	/// <summary>
	/// No suffix is added to the name.
	/// </summary>
	NoSuffix
}
