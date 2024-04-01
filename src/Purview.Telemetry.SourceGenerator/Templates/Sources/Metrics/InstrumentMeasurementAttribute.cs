namespace Purview.Telemetry.Metrics;

[System.AttributeUsage(System.AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
sealed public class InstrumentMeasurementAttribute : System.Attribute {
}

