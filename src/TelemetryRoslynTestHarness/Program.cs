using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TelemetryRoslynTestHarness.Interfaces.Telemetry;

namespace TelemetryRoslynTestHarness;

class Program {

	readonly static System.Func<Microsoft.Extensions.Logging.ILogger, System.String, System.Int32, System.IDisposable?> _logAction = Microsoft.Extensions.Logging.LoggerMessage.DefineScope<System.String, System.Int32>("Test.Log: stringParam: {StringParam}, intParam: {IntParam}");

	static readonly System.Func<Microsoft.Extensions.Logging.ILogger, System.Int32, System.Boolean, System.IDisposable?> _logScopeAction = Microsoft.Extensions.Logging.LoggerMessage.DefineScope<System.Int32, System.Boolean>("TestTelemetry.LogScope: intParam: {IntParam}, boolParam: {BoolParam}");

	static void Main(string[] args) {
		Console.WriteLine("Hello, World!");

		IDisposable? log = _logScopeAction((ILogger)default!, 1, true);

		IServiceCollection col = new ServiceCollection();

		//col.AddIBasicLogger();

		var sP = col.BuildServiceProvider();

		var logger = sP.GetService<IBasicLogger>();

		ActivitySource xx = new("asda");

		var at = new ActivityTagsCollection();

		xx.StartActivity("", kind: ActivityKind.Internal, parentId: "", tags: at);

		var a = Activity.Current;

		ActivityEvent e = new("", tags: at);

		IServiceCollection xxxxxx;
		//new ServiceDescriptor(typeof(IINTERFACE), typeof(CLASSIMPL), ServiceLifetime.Singleton);

		a?.AddEvent(e);

#if NET8_0_OR_GREATER
		IMeterFactory mF = (IMeterFactory)default!;
#endif

		Meter m =
#if NET8_0_OR_GREATER
			mF.Create(new MeterOptions("Hello") {
				Version = "Version"
			});
#else
			new("Hello", "Version");
#endif

		var counterInstrument = m.CreateCounter<System.Int32>(name: "Counter", unit: null, description: null
#if !NET7_0
, tags: null
#endif
			);

	}
}
