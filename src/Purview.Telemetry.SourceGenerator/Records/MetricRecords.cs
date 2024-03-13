namespace Purview.Telemetry.SourceGenerator.Records;

record MetricAttributeRecord(string Name, MetricTypes MetricType, bool AutoIncrement);

enum MetricTypes {
	Counter,
	UpDownCounter,
	Histogram,

	ObservableCounter,
	ObservableGauge,
	ObservableUpDownCounter
}
