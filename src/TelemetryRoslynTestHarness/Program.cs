using System.Diagnostics;
using System.Diagnostics.Metrics;
using TelemetryRoslynTestHarness.Interfaces.Telemetry;

namespace TelemetryRoslynTestHarness;

class Program {
	static void Main(string[] args) {
		Console.WriteLine("Hello, World!");

		IBasicLogger logger = new BasicLoggerCore(null);

		var a = Activity.Current;

	}
}

partial class Service {
	readonly Counter<int> _counter;
	readonly Histogram<float> _histogram;
	readonly UpDownCounter<int> _upDownCounter;

	readonly ObservableCounter<float> _observableCounter;
	readonly ObservableGauge<int> _observableGauge;
	readonly ObservableUpDownCounter<int> _observableUpDownCounter;

	public Service(IMeterFactory meterFactory) {
		var meterTags = new Dictionary<string, object?>();

		MeterTags(meterTags);

		var meter = meterFactory.Create(new MeterOptions("sdf") {
			Version = "1.0.0",
			Tags = meterTags
		});

		_counter = meter.CreateCounter<int>("");
		//_counter.Add(delta: 1, tags: list) or auto

		_histogram = meter.CreateHistogram<float>("");
		//_histogram.Record(value: 1, tags: list)

		_upDownCounter = meter.CreateUpDownCounter<int>("");
		//_upDownCounter.Add(delta: 1, tags: list)

		Func<float> f1;
		Func<Measurement<float>> f2;
		Func<IEnumerable<Measurement<float>>> f3;

		_observableCounter = meter.CreateObservableCounter<float>("", () => 1);

		_observableGauge = meter.CreateObservableGauge<int>("", () => 1);
		_observableUpDownCounter = meter.CreateObservableUpDownCounter<int>("", () => 1);
	}

	partial void MeterTags(Dictionary<string, object?> meterTags);
}
