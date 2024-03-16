namespace Purview.Telemetry.Activities;

/// <summary>
/// Excludes the method from any activity generation.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
sealed public class ActivityExcludeAttribute : Attribute {
}
