namespace Purview.Telemetry.Metrics;

/// <summary>
/// Determines if the parameter is an instrument measurement.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
sealed class InstrumentMeasurementAttribute : System.Attribute
{
}

