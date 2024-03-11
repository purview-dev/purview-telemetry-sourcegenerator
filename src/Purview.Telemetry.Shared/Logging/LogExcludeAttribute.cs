namespace Purview.Telemetry.Logging;

/// <summary>
/// Excludes the method from any log entry generation.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
sealed public class LogExcludeAttribute : Attribute {
}
