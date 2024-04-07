# Generation Options

Back to [README.md](../README.md).

## Basics

The class name is generated from the parent interface with any initial `I` removed. I.e. `ICacheServiceLogger` would become `CacheServiceLogger`. Then `Core` is appended, so the final result would be a class named `CacheServiceLoggerCore`.

The resulting class is also partial, internal and sealed. It is also generated within the owning namespace and any nested classes of the source interface.

### Controlling the Generation

By applying the `TelemetryGenerationAttribute` from the `Purview.Telemetry` namespace you can control some aspects of generation.

This attribute is permitted on both the assembly and interface. Although take care when using the `ClassName` and `DependencyInjectionClassName` at the assembly level, as they will clash.

| Name | Type | Description |
| -- | -- | -- |
| GenerateDependencyExtension | bool | Controls if the dependency injection class and extension method is generated. This defaults to true. |
| ClassName | string? | Explicitly set the name of the class to be generated, rather than using the interface name. This defaults to null, meaning the interface name is used. |
| DependencyInjectionClassName | string? | Similar to `ClassName`, but explicitly sets the name of the DI class name. |

### Excluding Methods

You can exclude any method on an interface by applying the `ExcludeAttribute`, in the `Purview.Telemetry` namespace.

As the generated class is partial, you can implement any methods you required in isolation.
