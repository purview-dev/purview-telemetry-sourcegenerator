namespace Purview.Telemetry;

/// <summary>
/// Excludes the method from any activity, logging or meter generation.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
sealed public class ExcludeAttribute : System.Attribute {
}
