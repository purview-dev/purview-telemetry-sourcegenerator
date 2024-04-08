# Generation Options

Back to [README.md](../README.md).

## Basics

The class name is generated from the parent interface with any initial `I` removed. I.e. `ICacheServiceTelemetry` would become `CacheServiceTelemetry`. Then `Core` is appended, so the final result would be a class named `CacheServiceTelemetryCore`.

The resulting class is also partial, internal and sealed. It is also generated within the owning namespace and any nested classes of the source interface.

### Controlling the Generation

By applying the `TelemetryGenerationAttribute` from the `Purview.Telemetry` namespace you can control some aspects of generation.

This attribute is permitted on both the assembly and interface. Although take care when using the `ClassName` and `DependencyInjectionClassName` at the assembly level, as they will clash.

| Name | Type | Description |
| -- | -- | -- |
| GenerateDependencyExtension | bool | Controls if the dependency injection class and extension method is generated. This defaults to true. |
| ClassName | string? | Explicitly set the name of the class to be generated, rather than using the interface name. This defaults to null, meaning the interface name is used. |
| DependencyInjectionClassName | string? | Similar to `ClassName`, but explicitly sets the name of the DI class name. |

### Dependency Injection

By default each interface source will have a generated dependency injection extension method, extending the [IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection).

The format takes on the `Add{InterfaceName}` format. For example, given the `ICacheServiceTelemetry` example, this would generate an extension method called `AddICacheServiceTelemetry` and generated within the `Microsoft.Extensions.DependencyInjection` namespace.

When called, the source interface is added to the services collection as a singleton.

This can be disabled by setting `TelemetryGenerationAttribute.GenerateDependencyExtension` to false at the assembly or on a per-interface basis.

> **Note**: this requires adding the NuGet package [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection).

### Excluding Methods

You can exclude any method on an interface by applying the `ExcludeAttribute`, in the `Purview.Telemetry` namespace.

As the generated class is partial, you can implement any methods you required in isolation.
