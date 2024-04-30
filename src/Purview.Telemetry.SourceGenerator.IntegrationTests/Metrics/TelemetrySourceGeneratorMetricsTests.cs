using Xunit.Abstractions;

namespace Purview.Telemetry.SourceGenerator.Metrics;

public partial class TelemetrySourceGeneratorMetricsTests(ITestOutputHelper testOutputHelper) : IncrementalSourceGeneratorTestBase<TelemetrySourceGenerator>(testOutputHelper)
{
	[Theory]
	[MemberData(nameof(TelemetrySourceGeneratorTests.BasicGenericParameters), MemberType = typeof(TelemetrySourceGeneratorTests))]
	public async Task Generate_GivenMethodWithBasicGenericParams_GeneratesEntryCorrectly(string parameterType)
	{
		// Arrange
		var basicActivity = @$"
using Purview.Telemetry.Metrics;

namespace Testing;

[Meter(""testing-meter"")]
public interface ITestMetrics {{
	[Counter(AutoIncrement = true)]
	void Counter_AutoIncrement({parameterType} genericParameter);

	[Counter]
	void Counter([InstrumentMeasurement]int value, {parameterType} genericParameter);

	[Histogram]
	void Histogram([InstrumentMeasurement]int value, {parameterType} genericParameter);

	[UpDownCounter]
	void UpDownCounter([InstrumentMeasurement]int value, {parameterType} genericParameter);

	[ObservableCounter]
	void ObservableCounter(Func<int> valueFunc, {parameterType} genericParameter);

	[ObservableGauge]
	void ObservableGauge(Func<int> valueFunc, {parameterType} genericParameter);

	[ObservableUpDownCounter]
	void ObservableUpDownCounter(Func<int> valueFunc, {parameterType} genericParameter);
}}
";

		// Act
		var generationResult = await GenerateAsync(basicActivity);

		// Assert
		await TestHelpers.Verify(generationResult, c => c
			.ScrubInlineGuids()
			.UseHashedParameters(parameterType)
		);
	}
}
