# Tagging Parameters

Back to [README.md](../README.md).

## Basics

The `TagAttribute` type is hosted within the `Purview.Telemetry` namespace. It is used within [Activity](ACTIVITIES.md) and [Metric](METRICS.md) generation to determine how parameters are used within the parent generation target.

### TagAttribute

Used to determine when a parameter is used to set a tag.

| Name | Type | Description |
| -- | -- | -- |
| Name | `string` | Determines the name of the tag. Defaults to `null`, meaning the name of the parameter is used. |
| SkipOnNullOrEmpty | `bool` | Determines if the parameter is skipped (not added) when it's `null` or default. The default value is `false`. |
