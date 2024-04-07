# Activities

Back to [README.md](../README.md).

## Basics

The types are hosted within the `Purview.Telemetry.Activities` namespace, with the exception of the `TagAttribute` which is defined within `Purview.Telemetry`.

To signal an interface for activity target generation, decorate the interface with the `ActivitySourceAttribute`. By default the activity source name is set to `purview`, however you can override this on the interface or assembly level:

* Interface: `ActivitySourceAttribute.Name`
* Assembly: `ActivitySourceGenerationAttribute.Name`

> Warning `TSG3001` is generated if this value has not been overridden.

### Activity, Event or Context

When defining methods there are three types of supported:

1. Activity methods that generates either a started or un-started [Activity](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activity).
2. Event methods that generates an [ActivityEvent](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activityevent) that will be attached to an Activity.
3. Context methods that add either tags or baggage to the current Activity.

#### Activity

Decorate the method with the `ActivityAttribute` to explicitly define an Activity method.

There are a number of parameters options that will be used to pass directly to the either `ActivitySource.CreateActivity` or `ActivitySource.StartSource`.

To populate the `tags` parameter, its type must be any of the following:

* [ActivityTagsCollection](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.ActivityTagsCollection)
* `System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object?>>`
* [TagList](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.taglist)

To populate the `parentContext` parameter, its type must be [ActivityContext](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activitycontext).

Alternatively, to populate the `parentId` parameter, the name of the parameter must be `parentId` and it's type must be a `string`.

To populate the `links` parameter, it's type must be an array of `IEnumerable<>` of [ActivityLink](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activitylink).

In the case of starting an activity, you can also specify the `startTime` parameter by making sure the name is `startTime` and the type is `DateTimeOffset`.

Other properties can be tagged with either the `TagAttribute` or `BaggageAttribute` to specify where the parameters are applied.

The return type must be either `void`, `Activity` or `Activity?`. When specifying an activity as a return type, this will return the created or started activity.

#### Events

Decorate the method with the `EventAttribute` to explicitly define an Event method. This will generate a method that will create an `ActivityEvent` and attach it to the either specified `Activity` or `Activity.Current`.

To specify and activity explicitly, the parameter type must be either `Activity` or `Activity?`.

To populate the `tags` parameter, its type must be any of the following:

* [ActivityTagsCollection](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.ActivityTagsCollection)
* `System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object?>>`
* [TagList](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.taglist)

To specify the `timestamp` parameter, making sure the name is `timestamp` and the type is `DateTimeOffset`.

Other properties can be tagged with either the `TagAttribute` or `BaggageAttribute` to specify where the parameters are applied.

The return type must be either `void`, `Activity` or `Activity?`. When returning an activity, either the one specified as a parameter is used, or `Activity.Current`.

##### Exceptions

When an `Exception` parameter is specified, the default it to treat it with the rules as defined by [Exceptions](https://opentelemetry.io/docs/specs/otel/trace/exceptions/).

This means the exception is populated to an events called `exception` with the following tags:

* `exception.escaped` - this is true by default, but you can also pass in a value of your own by decorating a `bool` parameter with the `EscapeAttribute`.
* `exception.message` - which will correspond to `Exception.Message`.
* `exception.stacktrace` - which will correspond to `Exception.StackTrace`.
* `exception.type` - which will correspond to called `GetType` on the exception, and using the `Type.FullName` property.

This behaviour can be overridden by other options on the `EventAttribute`. See the 'Types' section.

#### Context

Decorate the method with the `ContextAttribute` to explicitly define a context method. This will generate a method that will populate tags and/ or baggage on either a specified `Activity` or `Activity.Current`.

To specify and activity explicitly, the parameter type must be either `Activity` or `Activity?`.

Other properties can be tagged with either the `TagAttribute` or `BaggageAttribute` to specify where the parameters are applied.

The return type must be either `void`, `Activity` or `Activity?`. When returning an activity, either the one specified as a parameter is used, or `Activity.Current`.

## Inferring

### Activity, Event or Context Method

There are a few rules that allow you to skip explicitly specifying the type to generate. If the type is specified, these rules do not apply.

If the method name ends with `Event` (case-sensitive), then this is the equivalent of using `EventAttribute`.

If the method name ends with `Context` (case-sensitive), then this is the equivalent of using `ContextAttribute`.

Any that does not meet match those patterns will default to creating an activity.

### Tags or Baggage

If you decorate you parameter with either `TagAttribute` or `BaggageAttribute`, this will stop any inference.

However, any non-decorated parameter can be treated as either a tag or baggage based on setting:

* `ActivitySourceAttribute.DefaultToTags` on the interface to default to using tags (true) or baggage (false). The default is true.
* `ActivitySourceGenerationAttribute.DefaultToTags` on the assembly to default to using tags (true) or baggage (false). The default is true.

See also the [TagAttribute](./TAGATTRIBUTE.md) and `BaggageAttribute` further down.

## Types

### ActivityAttribute

Used to define the Activity creation on a method.

| Name | Type | Description |
| -- | -- | -- |
| Name | string | Determines the name of the activity. If this is not provided, the name of the method is used. Also available on construction. |
| Kind | [ActivityKind](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activitykind) | Determines the kind used to create the activity. The default is `Internal`. This is also available on construction. |
| CreateOnly | bool | Determines if the created activity is started or not. The default is false. If CreateOnly is specified as true, you must return the `Activity`/ `Activity?` from the method. |

### ActivitySourceAttribute

Used to define the creation of the [ActivitySource](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activitysource) on an interface.

| Name | Type | Description |
| -- | -- | -- |
| Name | string | Determines the name of the activity source. If this is not provided, either `ActivitySourceGenerationAttribute.Name` is used, or  `purview` is used. A warning is generated is a custom name is not defined anywhere. Also available on construction. |
| DefaultToTags | bool | Determines if tags (true) are used, or baggage (false) when no `TagAttribute` or `BaggageAttribute` is defined on any parameters. Special-case parameters are matched first. |
| BaggageAndTagPrefix | string | Determines the prefix to use for the tag or baggage name. The default is null. Useful for grouping. |
| IncludeActivitySourcePrefix | bool | Determines if the `ActivitySourceGenerationAttribute.BaggageAndTagSeparator`, if set, is used to generate a prefix. The default is true. |
| LowercaseBaggageAndTagKeys | bool | Determines if the name of the tag or baggage is lower-cased. The default is true. |

### ActivitySourceGenerationAttribute

Used to define the creation of the [ActivitySource](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activitysource) on an assembly.

| Name | Type | Description |
| -- | -- | -- |
| Name | string | Determines the name of the activity source when one is not defined on an interface. Defaults to `purview`. |
| DefaultToTags | bool | Determines if tags (true) are used, or baggage (false) when no `TagAttribute` or `BaggageAttribute` is defined on any parameters. Special-case parameters are matched first. |
| BaggageAndTagPrefix | string | Determines the prefix to use for the tag or baggage name. The default is null. Useful for grouping. |
| BaggageAndTagSeparator | string | Determines the separator use when generating prefixes. The default is `.`. |
| LowercaseBaggageAndTagKeys | bool | Determines if the name of the tag or baggage is lower-cased. The default is true. |

### BaggageAttribute

Used to determine when a parameter is used to set baggage on either an Activity or Event.

| Name | Type | Description |
| -- | -- | -- |
| Name | string | Determines the name of the baggage. Defaults to null, meaning the name of the parameter is used. |
| SkipOnNullOrEmpty | bool | Determines if the parameter is skipped (not added) when it's null or default. The default value is false. |

### ContextAttribute

Used to determine when a method adds to parameters as either tags or baggage to an activity.

There are no properties on this attribute.

### EscapeAttribute

Used to determine when a parameter is used as the escape value on an event-based method. The parameter must be a boolean.

See [Exceptions](https://opentelemetry.io/docs/specs/otel/trace/exceptions/) for more details.

There are no properties on this attribute.

### EventAttribute

Used to determine when a method creates an `ActivityEvent`.

| Name | Type | Description |
| -- | -- | -- |
| Name | string | Determines the name of the event. If this is not provided, the name of the method is used. Also available on construction. |
| UseRecordExceptionRules | bool | Determines if the Open Telemetry [exception](https://opentelemetry.io/docs/specs/otel/trace/exceptions/) rules should be followed when one of the parameters is an `Exception`. The default is true. |
| RecordExceptionAsEscaped | bool | Determines what value is used for the `exception.escaped` value when `UseRecordExceptionRules` is true, and an exception is present. This can also be overridden using the `EscapeAttribute` The default is true. |
