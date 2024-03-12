using System.Diagnostics;
using TelemetryRoslynTestHarness.Interfaces.Telemetry;

namespace TelemetryRoslynTestHarness;

class Program {
	static void Main(string[] args) {
		Console.WriteLine("Hello, World!");

		IBasicLogger logger = new BasicLoggerCore(null);

		var a = Activity.Current;


	}
}
