using Purview.Telemetry.Logging;

namespace TelemetryRoslynTestHarness.Interfaces.Telemetry;

[Logger]
public interface IBasicLogger
{
	void BasicNoAttributeOrParams();
}
