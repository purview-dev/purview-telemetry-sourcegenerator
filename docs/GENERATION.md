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
| GenerateDependencyExtension | `bool` | Controls if the dependency injection class and extension method is generated. This defaults to `true`. |
| ClassName | `string?` | Explicitly set the name of the class to be generated, rather than using the interface name. This defaults to `null`, meaning the interface name is used. |
| DependencyInjectionClassName | `string?` | Similar to `ClassName`, but explicitly sets the name of the DI class name. |

### Dependency Injection

By default each interface source will have a generated dependency injection extension method, extending the [IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection).

The generated method takes on the `Add{InterfaceName}` format, note that the interface name has any starting `I` removed. For example, given the `ICacheServiceTelemetry` interface, this would generate an extension method called `AddCacheServiceTelemetry`  within the `Microsoft.Extensions.DependencyInjection` namespace.

When called, the source interface is added to the services collection as a singleton.

The generation can be disabled by setting `TelemetryGenerationAttribute.GenerateDependencyExtension` to `false` at the assembly level or on a per-interface basis.

> **Note**: this requires adding the NuGet package [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection).

### Excluding Methods

You can exclude any method on an interface by applying the `ExcludeAttribute`, in the `Purview.Telemetry` namespace.

As the generated telemetry class is partial, you can implement any excluded methods you require in isolation.
