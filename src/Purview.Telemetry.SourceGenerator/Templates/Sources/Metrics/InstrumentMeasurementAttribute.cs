namespace Purview.Telemetry.Metrics;

/// <summary>
/// Determines if the parameter is an instrument measurement.
/// </summary>
[global::System.AttributeUsage(global::System.AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
[global::System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
sealed class InstrumentMeasurementAttribute : global::System.Attribute
{
}

