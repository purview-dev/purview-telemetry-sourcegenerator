# Purview Telemetry Source Generator

Generates [ActivitySource](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activitysource), [ILogger](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger) and [Metrics](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.metrics) based on methods on an interface, this enables faster iteration cycles and the ability to easily substitute in unit tests.

Use the latest version available on [NuGet](https://www.nuget.org/packages/Purview.Telemetry.SourceGenerator/), which supports the following frameworks:

- .NET Framework 4.7.2, or higher
- .NET 8 or higher

Reference in your `Directory.Build.props` or `.csproj` file:

```xml
<PackageReference Include="Purview.Telemetry.SourceGenerator" Version="3.0.0-prerelease.7">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
</PackageReference>
```

Example of a multi-target telemetry interface:

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

For more information see the [wiki](https://github.com/purview-dev/purview-telemetry-sourcegenerator/wiki).

> To see the generated output for the above, see the [`Generated Output`](https://github.com/purview-dev/purview-telemetry-sourcegenerator/wiki/Generated-Output) page in the wiki.

## Example Project

Checkout the [.NET Aspire Sample](https://github.com/purview-dev/purview-telemetry-sourcegenerator/tree/main/samples/SampleApp) Project to see the Activities, Logging, and Metrics working with the Aspire Dashboard.

Some documentation is available in the [wiki](https://github.com/purview-dev/purview-telemetry-sourcegenerator/wiki/Sample-Application).

> This sample project has [`EmitCompilerGeneratedFiles`](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration-generator#enable-the-configuration-source-generator) set to `true`, so you can easily see the generated output.

## Notes on Logging Generation

There are two different types of logging generation, based on either the Microsoft Logging Extensions NuGet packages that are referenced in your project, or configuration via attributes. See the [Logging](https://github.com/purview-dev/purview-telemetry-sourcegenerator/wiki/Logging) page in the wiki for more details.
