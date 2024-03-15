namespace Purview.Telemetry.Metrics;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
sealed public class InstrumentMeasurementAttribute : Attribute {
}
