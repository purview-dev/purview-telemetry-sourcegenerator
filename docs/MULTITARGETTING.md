# Multi-Targetting

Back to [README.md](../README.md).

## Basics

You may also specify a combination of activity, logging or metric target generation on a single interface. However, inferring method usage is disabled when one or more targets exist. This means each method must be explicitly marked with a **single** generation target.

Generation does not support multiple targets. A log method for example may not also be an activity-based or metric target. 

Multiple targets will generate the `TSG1002` diagnostic, and a method that does not contain a any generation target attribute, or the excluded attribute will result in the `TSG1001` diagnostic.
