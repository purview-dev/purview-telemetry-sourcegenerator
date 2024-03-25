using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.DependencyInjection;
using TelemetryRoslynTestHarness.Interfaces.Telemetry;

namespace TelemetryRoslynTestHarness;

class Program {

	readonly static System.Func<Microsoft.Extensions.Logging.ILogger, System.String, System.Int32, System.IDisposable?> _logAction = Microsoft.Extensions.Logging.LoggerMessage.DefineScope<System.String, System.Int32>("Test.Log: stringParam: {StringParam}, intParam: {IntParam}");

	static void Main(string[] args) {
		Console.WriteLine("Hello, World!");

		IServiceCollection col = new ServiceCollection();

		//col.AddIBasicLogger();

		var sP = col.BuildServiceProvider();

		var logger = sP.GetService< IBasicLogger>();

		ActivitySource xx = new("asda");

		var at = new ActivityTagsCollection();

		xx.StartActivity("", kind: ActivityKind.Internal, parentId: "", tags: at);

		var a = Activity.Current;

		ActivityEvent e = new("", tags: at);

		IServiceCollection xxxxxx;
		//new ServiceDescriptor(typeof(IINTERFACE), typeof(CLASSIMPL), ServiceLifetime.Singleton);

		a?.AddEvent(e);
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

		TagList counterTagsList = new();
		_counter = meter.CreateCounter<int>("");
		_counter.Add(1, tagList: counterTagsList);

		_histogram = meter.CreateHistogram<float>("");
		_histogram.Record(value: 1, tagList: counterTagsList);

		_upDownCounter = meter.CreateUpDownCounter<int>("");
		_upDownCounter.Add(1, tagList: counterTagsList);

		Func<float> f1 = null!;
		Func<Measurement<int>> f2 = null!;
		Func<IEnumerable<Measurement<int>>> f3 = null!;

		_observableCounter = meter.CreateObservableCounter<float>("", f1, null, null, tags: counterTagsList);
		_observableGauge = meter.CreateObservableGauge<System.Int32>("", f2, null, null, tags: counterTagsList);
		_observableUpDownCounter = meter.CreateObservableUpDownCounter<int>("", f3, null, null, tags: counterTagsList);
	}

	partial void MeterTags(Dictionary<string, object?> meterTags);
}
