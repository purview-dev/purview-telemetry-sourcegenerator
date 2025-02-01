# Purview Telemetry Source Generator

Generates [ActivitySource](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activitysource), [High-performance logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging) and [Metrics](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.metrics) based on methods on an interface, enabling fast iteration cycles, dependency injection and substitutes for testing.

The latest version is available on [NuGet](https://www.nuget.org/packages/Purview.Telemetry.SourceGenerator/), and supports generating for the following frameworks:

* .NET Framework 4.7.2
* .NET Framework 4.8
* .NET 8
* .NET 9

Reference in your .props or csproj file:

```xml
<PackageReference Include="Purview.Telemetry.SourceGenerator" Version="2.0.0">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
</PackageReference>
```

```csharp
using Purview.Telemetry.Activities;
using Purview.Telemetry.Logging;
using Purview.Telemetry.Metrics;

[ActivitySource]
[Logger]
[Meter]
interface IEntityStoreTelemetry
{
    /// <summary>
    /// Creates and starts an Activity and adds the parameters as Tags and Baggage.
    /// </summary>
    [Activity]
    Activity? GettingEntityFromStore(int entityId, [Baggage]string serviceUrl);

    /// <summary>
    /// Adds an ActivityEvent to the Activity with the parameters as Tags.
    /// </summary>
    [Event]
    void GetDuration(Activity? activity, int durationInMS);

    /// <summary>
    /// Adds the parameters as Baggage to the Activity.
    /// </summary>
    [Context]
    void RetrievedEntity(Activity? activity, float totalValue, int lastUpdatedByUserId);

    /// <summary>
    /// Generates a structured log message using an ILogger - defaults to Informational.
    /// </summary>
    [Log]
    void ProcessingEntity(int entityId, string updateState);

    /// <summary>
    /// Generates a structured log message using an ILogger, specifically defined as Informational.
    /// </summary>
    [Info]
    void ProcessingAnotherEntity(int entityId, string updateState);

    /// <summary>
    /// Adds 1 to a Counter<T> with the entityId as a Tag.
    /// </summary>
    [AutoCounter]
    void RetrievingEntity(int entityId);
}
```

Checkout the [.NET Aspire Sample](https://github.com/purview-dev/purview-telemetry-sourcegenerator/tree/main/samples/SampleApp) Project to see the Activities, Logging, and Metrics working with the dashaboard.

For more information see the [wiki](https://github.com/purview-dev/purview-telemetry-sourcegenerator/wiki).
