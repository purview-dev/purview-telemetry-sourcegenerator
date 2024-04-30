# Purview Telemetry Source Generator

Generates [ActivitySource](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activitysource), [High-performance logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging) and [Metrics](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.metrics) based on methods on an interface, this enables fast iteration cycles and substitutes for testing.

Use the latest version available on [NuGet](https://www.nuget.org/packages/Purview.Telemetry.SourceGenerator/), and supports the following frameworks:

* .NET Framework 4.7.2
* .NET Framework 4.8
* .NET 7
* .NET 8

For more information see:

* [Activities](./docs/ACTIVITIES.md) generation target
* [Logging](./docs/LOGGING.md) generation target
* [Metrics](./docs/METRICS.md) generation target
* [Multi-Targeting](./docs/MULTITARGETING.md) information
* [Generation](./docs/GENERATION.md) options

All marker attributes are generated as conditional, meaning they will not be present in your build. However, you can define `PURVIEW_TELEMETRY_ATTRIBUTES` as a build constant to retain them. They are generated as internal to avoid exposing them outside of the assembly.

## Basic Examples

The following examples all contain explicit definitions, meaning each example explicitly applies the appropriate attributes. Inferring certain actions or values is also supported and will be detailed in each sub-section.

The documentation for each generation target ([activity](./docs/ACTIVITIES.md), [logging](./docs/LOGGING.md) and [metrics](./docs/METRICS.md)) contains information on what can be inferred.

By default each interface used as a source for generation includes an extension method for registering it with an `IServiceCollection`. More details can be found in [Generation](./docs/GENERATION.md).

> You can mix-and-match generation targets within a single interface, however the ability to infer is more limited. This is called [multi-targetting](./docs/MULTITARGETTING.md).
> In .NET, Activities, Events and Metrics refer to additional properties captured at creation, recording or observation time as **tags**. However, in Open Telemetry these are referred to as **attributes**. As this source generator makes extensive use of marker attributes to control source code generation we will use the term tags to mean these properties, and attributes as the .NET [Attribute](https://learn.microsoft.com/en-us/dotnet/api/system.attribute) type not as the Open Telemetry definition.

### Activities

Basic example of an activity-based telemetry interface.

There is one Activity (`GettingItemFromCache`) and the rest are events, calling these will add an `ActivityEvent` to the `Activity.Current` activity. Alternatively, you can pass in an `Activity` as a parameter to the event, which will be used in it's place.

You can also return an `Activity?` from the `GettingItemFromCache` method and then pass that into the events.

The `[Tag]` and `[Baggage]` attributes on the parameters will add the values to the activity or event.

```csharp
[ActivitySource("some-activity")]
interface IActivityTelemetry
{
    [Activity]
    void GettingItemFromCache([Baggage]string key, [Tag]string itemType);

    [Event("cachemiss")]
    void Miss();

    [Event("cachehit")]
    void Hit();

    [Event]
    void Error(Exception ex);

    [Event]
    void Finished([Tag]TimeSpan duration);
}
```

More information can be found [here](./docs/ACTIVITIES.md).

### Logging

Basic example of a high-performance structured logging-based interface.

Note the `ProcessingWorkItem` method returns an `IDisposable?`, this is a scoped log entry.

All of the parameters are passed into the logger methods as properties.

```csharp
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
```

More information can be found [here](./docs/LOGGING.md).

### Metrics

This example shows each meter type currently supported. Note the `Counter` attribute is demoed twice. Once with `AutoIncrement` set to `true`, this means the measurement value is automatically set to increment by 1 each time the method is called. In the other example (where `AutoIncrement` is `false`, which is the default default) the measurement value is specified explicitly as a parameter using the `InstrumentMeasurementAttribute`.

Non-auto increment meters must specify a measurement with one of the valid types: `byte`, `short`, `int`, `long`, `float`, `double`, and `decimal`.

> Observable types must always have a `System.Func<>` with one of the following supported types:
>
> * Any one of the following supported measurement types: `byte`, `short`, `int`, `long`, `float`, `double`, or `decimal`
> * `Measurement<T>` where `T` is one of valid measurement types above.
> * `IEnumerable<Measurement<T>>` where `T` is one of valid measurement types above.

As with activities, you can add a `[Tag]` to the parameters and they'll be included at recording time for the instrument.

> **Note**: tags are not supported on .NET 7 for meters, and will be ignored.

```csharp
[Meter]
interface IMeterTelemetry
{
    [Counter(AutoIncrement = true)]
    void AutoIncrementMeter([Tag]string someValue);

    [Counter]
    void CounterMeter([InstrumentMeasurement]int measurement, [Tag]float someValue);

    [AutoCounter]
    void AutoCounterMeter([Tag]float someValue);

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
```

More information can be found [here](./docs/METRICS.md).

## Multi-Targetting

In this example, all method-based targets are explicitly set as inferring their usage is not support when using multi-targetting.

```csharp
[ActivitySource("multi-targetting")]
[Logger]
[Meter]
interface IServiceTelemetry
{
    [Activity]
    Activity? StartAnActivity(string tagStringParam, [Baggage]int entityId);

    [Event]
    void AnInterestingEvent(Activity? activity, float aTagValue);

    [Context]
    void InterestingInfo(Activity? activity, float anotherTagValue, int intTagValue);

    [Log]
    void ProcessingEntity(int entityId, string property1);

    [Counter(AutoIncrement = true)]
    void AnAutoIncrement([Tag]int value);
}
```

More information can be found [here](./docs/MULTITARGETTING.md).
