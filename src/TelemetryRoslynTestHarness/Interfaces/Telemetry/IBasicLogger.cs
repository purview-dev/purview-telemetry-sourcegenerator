using Purview.Telemetry.Logging;

namespace TelemetryRoslynTestHarness.Interfaces.Telemetry;

[LoggerTarget]
public interface IBasicLogger {
	void BasicNoAttributeOrParams();
}
