namespace Purview.Telemetry.Activities;

[System.AttributeUsage(System.AttributeTargets.Parameter, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
sealed public class EscapeAttribute : System.Attribute {
}
