namespace Purview.Telemetry.Logging;

/// <summary>
/// Excludes the method from any log entry generation.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
sealed public class LogExcludeAttribute : Attribute {
}
