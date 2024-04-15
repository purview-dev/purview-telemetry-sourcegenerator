using Xunit.Abstractions;

namespace Purview.Telemetry.SourceGenerator.Metrics;

public partial class TelemetrySourceGeneratorMetricsTests(ITestOutputHelper testOutputHelper) : IncrementalSourceGeneratorTestBase<TelemetrySourceGenerator>(testOutputHelper)
{
}
