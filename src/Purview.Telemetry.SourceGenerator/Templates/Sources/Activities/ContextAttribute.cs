namespace Purview.Telemetry.Activities;

[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
sealed public class ContextAttribute : System.Attribute {
}
