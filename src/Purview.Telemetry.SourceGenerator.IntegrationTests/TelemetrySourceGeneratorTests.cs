using Xunit.Abstractions;

namespace Purview.Telemetry.SourceGenerator;
public partial class TelemetrySourceGeneratorTests(ITestOutputHelper testOutputHelper) : IncrementalSourceGeneratorTestBase<TelemetrySourceGenerator>(testOutputHelper)
{
	[Fact]
	public async Task Generate_GivenGeneratedAttributes_GeneratesAsExpected()
	{
		// Arrange
		const string empty = @"
using Purview.Telemetry.Logging;

namespace Testing;

";

		// Act
		GenerationResult generationResult = await GenerateAsync(empty);

		// Assert
		await TestHelpers.Verify(generationResult, autoVerifyTemplates: false);
	}
}
