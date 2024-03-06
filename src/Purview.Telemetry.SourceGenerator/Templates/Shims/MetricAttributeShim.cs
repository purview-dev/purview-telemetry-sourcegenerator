using Purview.Telemetry.SourceGenerator.Templates.Shims;

namespace Purview.Telemetry.Metrics;

sealed class MetricAttributeShim : MetricAttributeBase {
	public MetricAttributeShim(string name, MetricTypes metricType) {
		Name = name;
		MetricType = metricType;
	}

	public MetricTypes MetricType { get; }

	public bool AutoIncrement { get; set; }
}
