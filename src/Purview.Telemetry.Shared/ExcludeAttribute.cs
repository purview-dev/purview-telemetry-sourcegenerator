namespace Purview.Telemetry;

/// <summary>
/// Excludes the method from any activity, logging or metric generation.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
sealed public class ExcludeAttribute : Attribute {
}
