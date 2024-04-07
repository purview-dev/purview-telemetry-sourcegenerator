# Logging

Back to [README.md](../README.md).

## Basics

The types are hosted within the `Purview.Telemetry.Logging` namespace.

To signal an interface for logging target generation, decorate the interface with the `LoggerAttribute`. The signal for a method is the `LogAttribute`, where you can control various aspects of it's generation.

We also support the generation of scoped loggers.

All log generation is achieved using the [LoggerMessage](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loggermessage) class, as part of the [high-performance logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging) libraries.

> **Note**: that the maximum number of parameters allowed by the `LoggerMessage` class is **6**, plus one optional `Exception`.
> You must also reference the `Microsoft.Extensions.Logging` package on [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.Logging/).

### Log Generation

To generate a log entry, the method should be decorated with the `LogAttribute` and return either `void`, in the case of a non-scoped log entry. Or `IDisposable`/ `IDisposable?` for scoped log entries.

The parameters, if any, are used by the `LogAttribute.MessageTemplate` and form part of the structured log generation.

There are a number of options such as the level, name, event Id and message template that can be adjusted via the `LogAttribute`.

## Inferring

When not using multi-targeting you can avoid adding the `LogAttribute` explicitly.

You can set the default log level using either the `LoggerAttribute.DefaultLevel` on the interface, or the `LoggerGenerationAttribute.DefaultLevel` on the assembly.

If no level is specified on the method, and an `Exception` is defined the level is changed to an `Error` automatically. This also raises a diagnostic, that can safely be ignored or disabled.

## Types

### LogAttribute

Used to define the log entry details on a method.

| Name | Type | Description |
| -- | -- | -- |
| Level | [Microsoft.Extensions.Logging.LogLevel](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loglevel) | Determines the level used when defining the log entry. Also available on construction. Defaults to Information. |
| MessageTemplate | string? | Defines the template used to populate the log entry method. If one is not specified, it will automatically be generated based on the prefixes and the available parameters. Also available on construction. Defaults to null. |
| EventId | int? | Used when generating the [EventId](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.eventid). Also available on construction. Defaults to null. One will be generated if one is not supplied. |
| Name | string? | The name of the log entry, if one is not defined then the name of the method is used. Also available on construction. Defaults to null. |

### LoggerAttribute

Used to enrol an interface in the source generation process.

| Name | Type | Description |
| -- | -- | -- |
| DefaultLevel | [Microsoft.Extensions.Logging.LogLevel](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loglevel) | Determines the default level to use when one is not provided. Also available on construction. Defaults to Information. |
| CustomPrefix | string? | Used when generating the log entry name's prefix. If this is set, the `PrefixType` is automatically set to `Custom`. Also available on construction. Defaults to null. |
| PrefixType | `Purview.Telemetry.Logging.LogPrefixType` | Determines the type of prefix to use when generating the log entry name. Defaults to `Default`. |

### LoggerGenerationAttribute

Used to control defaults at the assembly level.

| Name | Type | Description |
| -- | -- | -- |
| DefaultLevel | [Microsoft.Extensions.Logging.LogLevel](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loglevel) | Determines the default level to use when one is not provided. Also available on construction. Defaults to Information. |

### LogPrefixType

When generating a log entry name, provides options to customise the prefix.

| Value | Description |
| -- | -- |
| Default | The name of the interface without the "I" prefix or "Log", "Logger" or "Telemetry" suffixes. |
| Interface | Uses the name of the interface. |
| Class | The name of the class used for generation. This can be specified using the `TelemetryGenerationAttribute.ClassName` property, or through auto generation. |
| Custom | Used when the `LoggerAttribute.CustomPrefix` is set. |
| NoSuffix | Does not generate any suffix. |
