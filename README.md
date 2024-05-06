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
interface IEntityStoreTelemetry
{
    /// <summary>
    /// Creates and starts an Activity and adds the parameters as Tags and Baggage.
    /// </summary>
    [Activity]
    void GettingEntityFromStore(int entityId, [Baggage]string serviceUrl);

    /// <summary>
    /// Adds an ActivityEvent to the Activity with the parameters as Tags.
    /// </summary>
    [Event]
    void GetDuration(int durationInMS);

    /// <summary>
    /// Adds the parameters as Baggage to the Activity.
    /// </summary>
    [Context]
    void RetrievedEntity(float totalValue, int lastUpdatedByUserId);

    /// <summary>
    /// Generates a structured log message using an ILogger.
    /// </summary>
    [Log]
    void ProcessingEntity(int entityId, string updateState);

    /// <summary>
    /// Adds 1 to a Counter<T> with the entityId as a Tag.
    /// </summary>
    [AutoCounter]
    void RetrievingEntity(int entityId);
}
```

For more information see the [wiki](https://github.com/purview-dev/purview-telemetry-sourcegenerator/wiki).
