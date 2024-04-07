# Multi-Targetting

Back to [README.md](../README.md).

## Basics

You may also specify a combination of activity, logging or metric generation on a single interface. However, inferring method usage is disabled. This means each method must be explicitly marked with a **single** generation target.

Generation does not support multiple targets. A log method for example may not also be an activity-based or metric target.
