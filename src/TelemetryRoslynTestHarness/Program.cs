using System.Diagnostics;
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

		var logger = sP.GetService<IBasicLogger>();

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
