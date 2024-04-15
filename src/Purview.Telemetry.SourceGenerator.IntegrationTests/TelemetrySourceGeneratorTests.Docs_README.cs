namespace Purview.Telemetry.SourceGenerator;

partial class TelemetrySourceGeneratorTests
{
	[Fact]
	public async Task Generate_FromREADMEActivitiesSection_GeneratesTelemetry()
	{
		// Arrange
		const string basicTelemetry = @"
using Purview.Telemetry.Activities;

[ActivitySource(""some-activity"")]
interface IActivityTelemetry
{
    [Activity]
    void GettingItemFromCache([Baggage]string key, [Tag]string itemType);

    [Event(""cachemiss"")]
    void Miss();

    [Event(""cachehit"")]
    void Hit();

    [Event]
    void Error(Exception ex);

    [Event]
    void Finished([Tag]TimeSpan duration);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicTelemetry, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_FromREADMELoggingSection_GeneratesTelemetry()
	{
		// Arrange
		const string basicTelemetry = @"
using Purview.Telemetry.Logging;
using Microsoft.Extensions.Logging;

[Logger]
interface ILoggingTelemetry
{
    [Log(LogLevel.Information)]
    IDisposable? ProcessingWorkItem(Guid id);

    [Log(LogLevel.Trace)]
    void ProcessingItemType(ItemTypes itemType);

    [Log(LogLevel.Error)]
    void FailedToProcessWorkItem(Exception ex);

    [Log(LogLevel.Information)]
    void ProcessingComplete(bool success, TimeSpan duration);
}

enum ItemTypes
{
	Unknown,
	File,
	Folder,
	Link
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicTelemetry, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_FromREADMEMetricsSection_GeneratesTelemetry()
	{
		// Arrange
		const string basicTelemetry = @"
using Purview.Telemetry.Metrics;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

[Meter]
interface IMeterTelemetry
{
    [Counter(AutoIncrement = true)]
    void AutoIncrementMeter([Tag]string someValue);

    [Counter]
    void CounterMeter([InstrumentMeasurement]int measurement, [Tag]float someValue);

    [Histogram]
    void HistogramMeter([InstrumentMeasurement]int measurement, [Tag]int someValue, [Tag]bool anotherValue);

    [ObservableCounter]
    void ObservableCounterMeter(Func<float> measurement, [Tag]double someValue);

    [ObservableGauge]
    void ObservableGaugeMeter(Func<Measurement<float>> measurement, [Tag]double someValue);

    [ObservableUpDownCounter]
    void ObservableUpDownCounter(Func<IEnumerable<Measurement<byte>>> measurement, [Tag]double someValue);

    [UpDownCounter]
    void UpDownCounterMeter([InstrumentMeasurement]decimal measurement, [Tag]byte someValue);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicTelemetry, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_FromREADMEMultiTargetingSection_GeneratesTelemetry()
	{
		// Arrange
		const string basicTelemetry = @"
using Purview.Telemetry.Activities;
using Purview.Telemetry.Logging;
using Purview.Telemetry.Metrics;
using System.Diagnostics;

[ActivitySource(""multi-targetting"")]
[Logger]
[Meter]
interface IServiceTelemetry
{
    [Activity]
    Activity? StartAnActivity(int tagIntParam, [Baggage]string entityId);

    [Event]
    void AnInterestingEvent(Activity? activity, float aTagValue);

    [Context]
    void InterestingInfo(Activity? activity, float anotherTagValue, int intTagValue);

    [Log]
    void ProcessingEntity(int entityId, string property1);

    [Counter(AutoIncrement = true)]
    void AnAutoIncrement([Tag]int value);
}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicTelemetry, disableDependencyInjection: false);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
