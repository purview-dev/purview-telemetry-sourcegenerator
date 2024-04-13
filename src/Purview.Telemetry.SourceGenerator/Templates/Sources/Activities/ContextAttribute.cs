namespace Purview.Telemetry.Activities;

/// <summary>
/// Determines if the methods parameters should be
/// added to the current <see cref="System.Diagnostics.Activity"/>, using
/// either the <see cref="Purview.Telemetry.TagAttribute"/>,
/// the <see cref="BaggageAttribute"/> or inferred.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
sealed class ContextAttribute : System.Attribute
{
}
