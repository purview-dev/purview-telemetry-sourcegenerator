namespace Purview.Telemetry.Metrics;

#if NETSTANDARD1_6_OR_GREATER

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
sealed public class InstrumentMeasurementAttribute : Attribute {
}

#endif
