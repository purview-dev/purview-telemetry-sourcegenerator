namespace Purview.Telemetry.Activities;

/// <summary>
/// Excludes the method from any activity generation.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
sealed public class ActivityExcludeAttribute : Attribute {
}
