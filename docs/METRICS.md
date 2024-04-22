# Metrics

Back to [README.md](../README.md).

## Basics

The types are hosted within the `Purview.Telemetry.Metrics` namespace, with the exception of the `TagAttribute` which is defined within `Purview.Telemetry`.

To signal an interface for meter target generation, decorate the interface with the `MeterAttribute`. If the `MeterAttribute.Name` is not specified, the name of the interface, without the first 'I', will be used. For example `ICacheServiceTelemetry` would become `CacheServiceTelemetry`.

When creating the meter types, the [IMeterFactory](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.metrics.imeterfactory) is used by default, unless using .NET 7.

> **Note**: In the case of .NET 7, the meters are created using the [Meter](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.metrics.meter) class instead due to lack of support for the `IMeterFactory`.
> Also note that any tags passed to any meter via the parameters on a method will be be ignored in .NET 7.

### Initialisation

> **Note**: This does not apply to .NET 7.

During the initialisation of the class you can implement a partial method used to provide additional tags to any meters created by the `IMeterFactory`.

The signature is: `partial void PopulateMeterTags(System.Collections.Generic.Dictionary<string, object?> meterTags)`. You can add you custom tags to the `meterTags` to enable all meters created to include these tags.

### Meter Types

There are several supported meter types: counter, histogram, up-down counter, observable counter, observable gauge and observable up-down counter. Each are determined by the corresponding attribute:

* `CounterAttribute` generates the [Counter&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.metrics.counter-1) instrument.
* `HistogramAttribute` generates the [Histogram&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.metrics.histogram-1) instrument.
* `UpDownCounterAttribute` generates the [UpDownCounter&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.metrics.updowncounter-1) instrument.
* `ObservableCounterAttribute` generates the [ObservableCounter&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.metrics.observablecounter-1) instrument.
* `ObservableGaugeAttribute` generates the [ObservableGauge&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.metrics.observablegauge-1) instrument.
* `ObservableUpDownCounterAttribute` generates the [ObservableUpDownCounter&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.metrics.observableupdowncounter-1) instrument.

The measurement type used by the meter must be one of the following: `byte`, `short`, `int`, `long`, `float`, `double`, or `decimal`.

This is determined by decorating the desired parameter with the `InstrumentMeasurementAttribute`.

#### Counter, Histogram and Up/Down Counter

All of these counters types require a measurement parameter, with the exception of the counter when `CounterAttribute.AutoIncrement` is set to true.

If a measurement parameter is not defined, the first parameter with a matching type is used.

When using `CounterAttribute.AutoIncrement`, the meter increments by one each time the method is called.

#### ObservableCounter, ObservableGauge and ObservableUpDownCounter

Observable meter types must always have a `Func&lt;T&gt;` with one of the supported instrument types.

However, you may also use [Measurement&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.metrics.measurement-1) where `T` is one of the support instrument types.

You can provide multiple values at once using `IEnumerable&lt;Measurement&lt;T&gt;&gt;`, again `T` must be one of valid measurement types.

Examples:

```csharp
[ObservableCounter]
void Counter(Func<int> func);

[ObservableGauge]
void Gauge(Func<Measurement<int>> func);

[ObservableUpDownCounter]
void UpDownCounter(Func<IEnumerable<Measurement<int>>> func);
```

#### Tags

Other parameters on the method can be used as tags. This is implicit for non-measurement values, but can also be explicitly set to control generation through the use of the `TagAttribute`.

## Types

### CounterAttribute

Used to create a `Counter<T>` meter.

| Name | Type | Description |
| -- | -- | -- |
| AutoIncrement | bool | Determines if the meter should generate an auto-incrementing counter, rather than accepting a measurement value from one of the parameters Also available on construction. Defaults to false. |
| Name | string? | Determines the name of the meter. If this is not provided, the name of the method is used. Also available on construction. Defaults to null. |
| Unit | string? | Specifies the Unit used during meter generation. Also available on construction. Defaults to null. |
| Description | string? | Specifies the Description used during meter generation. Also available on construction. Defaults to null. |

### HistogramAttribute

Used to create a `Histogram<T>` meter.

| Name | Type | Description |
| -- | -- | -- |
| Name | string? | Determines the name of the meter. If this is not provided, the name of the method is used. Also available on construction. Defaults to null. |
| Unit | string? | Specifies the Unit used during meter generation. Also available on construction. Defaults to null. |
| Description | string? | Specifies the Description used during meter generation. Also available on construction. Defaults to null. |

### UpDownAttribute

Used to create a `UpDownCounter<T>` meter.

| Name | Type | Description |
| -- | -- | -- |
| Name | string? | Determines the name of the meter. If this is not provided, the name of the method is used. Also available on construction. Defaults to null. |
| Unit | string? | Specifies the Unit used during meter generation. Also available on construction. Defaults to null. |
| Description | string? | Specifies the Description used during meter generation. Also available on construction. Defaults to null. |

### ObservableCounterAttribute

Used to create a `ObservableUpDownCounter<T>` meter.

| Name | Type | Description |
| -- | -- | -- |
| Name | string? | Determines the name of the meter. If this is not provided, the name of the method is used. Also available on construction. Defaults to null. |
| Unit | string? | Specifies the Unit used during meter generation. Also available on construction. Defaults to null. |
| Description | string? | Specifies the Description used during meter generation. Also available on construction. Defaults to null. |
| ThrowOnAlreadyInitialized | bool | Determines if the method throws an exception or not if it is called more than once. Also available on construction. Defaults to false. |

### ObservableGaugeAttribute

Used to create a `ObservableGauge<T>` meter.

| Name | Type | Description |
| -- | -- | -- |
| Name | string? | Determines the name of the meter. If this is not provided, the name of the method is used. Also available on construction. Defaults to null. |
| Unit | string? | Specifies the Unit used during meter generation. Also available on construction. Defaults to null. |
| Description | string? | Specifies the Description used during meter generation. Also available on construction. Defaults to null. |
| ThrowOnAlreadyInitialized | bool | Determines if the method throws an exception or not if it is called more than once. Also available on construction. Defaults to false. |

### ObservableUpDownCounterAttribute

Used to create a `ObservableUpDownCounter<T>` meter.

| Name | Type | Description |
| -- | -- | -- |
| Name | string? | Determines the name of the meter. If this is not provided, the name of the method is used. Also available on construction. Defaults to null. |
| Unit | string? | Specifies the Unit used during meter generation. Also available on construction. Defaults to null. |
| Description | string? | Specifies the Description used during meter generation. Also available on construction. Defaults to null. |
| ThrowOnAlreadyInitialized | bool | Determines if the method throws an exception or not if it is called more than once. Also available on construction. Defaults to false. |
