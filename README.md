# Purview Telemetry Source Generator

Generates [ActivitySource](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activitysource), [High-performance logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging) and [Metrics](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.metrics) based on methods on an interface, enabling fast iteration cycles, dependency injection and substitutes for testing.

The latest version is available on [NuGet](https://www.nuget.org/packages/Purview.Telemetry.SourceGenerator/), and supports generating for the following frameworks:

* .NET Framework 4.7.2
* .NET Framework 4.8
* .NET 7
* .NET 8

```csharp
[ActivitySource]
[Logger]
[Meter]
interface IServiceTelemetry
{
    [Activity]
    Activity? StartsAnActivity(string tagStringParam, [Baggage]int entityId);

    [Event]
    void AnInterestingEvent(Activity? activity, float aTagValue);

    [Context]
    void InterestingInfo(Activity? activity, float anotherTagValue, int intTagValue);

    [Log]
    void ProcessingEntity(int entityId, string property);

    [AutoCounter]
    void AnAutoIncrementCounter([Tag]int entityId);
}
```

For more information see the [wiki](https://github.com/purview-dev/purview-telemetry-sourcegenerator/wiki).
