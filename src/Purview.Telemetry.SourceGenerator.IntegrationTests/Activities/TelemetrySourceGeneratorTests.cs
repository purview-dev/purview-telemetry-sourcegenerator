using Xunit.Abstractions;

namespace Purview.Telemetry.SourceGenerator.Activities;

public partial class TelemetrySourceGeneratorTests(ITestOutputHelper testOutputHelper) : IncrementalSourceGeneratorTestBase<TelemetrySourceGenerator>(testOutputHelper) {
}
