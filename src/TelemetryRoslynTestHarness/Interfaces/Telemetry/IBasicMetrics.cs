using Purview.Telemetry;
using Purview.Telemetry.Metrics;

namespace TelemetryRoslynTestHarness.Interfaces.Telemetry;

[MeterTarget]
public interface IBasicMetrics {
	[ObservableCounter]
	void ObservableCounter(Func<int> f, [Tag] int intParam, bool boolParam);
}
