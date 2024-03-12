using Xunit.Abstractions;

namespace Purview.Telemetry.SourceGenerator.Activities;

public partial class TelemetrySourceGeneratorActivityTests(ITestOutputHelper testOutputHelper) : IncrementalSourceGeneratorTestBase<TelemetrySourceGenerator>(testOutputHelper) {
}
