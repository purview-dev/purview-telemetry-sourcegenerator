using Xunit.Abstractions;

namespace Purview.Telemetry.SourceGenerator;

public partial class TelemetrySourceGeneratorTests(ITestOutputHelper testOutputHelper) : IncrementalSourceGeneratorTestBase<TelemetrySourceGenerator>(testOutputHelper) {
}
