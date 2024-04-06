namespace Purview.Telemetry.Activities;

/// <summary>
/// Used during <see cref="System.Diagnostics.ActivityEvent"/> generation
/// when specifying an <see cref="System.Exception"/>. When true, determines if the
/// exception should be marked as escaped, i.e. the exception caused the
/// process/ action to end unexpectedly.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Parameter, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
sealed public class EscapeAttribute : System.Attribute {
}
