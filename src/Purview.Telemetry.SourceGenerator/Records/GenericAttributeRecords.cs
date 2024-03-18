﻿namespace Purview.Telemetry.SourceGenerator.Records;

record TagOrBaggageAttributeRecord(
	AttributeStringValue Name,
	AttributeValue<bool> SkipOnNullOrEmpty
);

record TelemetryGenerationAttributeRecord(
	AttributeValue<bool> GenerateDependencyExtension,
	AttributeStringValue ClassNameTemplate
);
