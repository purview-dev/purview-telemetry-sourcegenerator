using Purview.Telemetry;
using Purview.Telemetry.Metrics;

namespace TelemetryRoslynTestHarness.Interfaces.Telemetry;

[MeterTarget]
public interface IBasicMetrics {
	[ObservableCounterTarget]
	void ObservableCounter(Func<int> f, [Tag] int intParam, bool boolParam);
}
