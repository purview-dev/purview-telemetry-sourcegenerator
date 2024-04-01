namespace Purview.Telemetry.SourceGenerator.Records;

record LoggerAttributeRecord(
	AttributeValue<int> DefaultLevel,

	AttributeStringValue CustomPrefix,
	AttributeValue<int> PrefixType
);

record LoggerGenerationAttributeRecord(
	AttributeValue<int> DefaultLevel
);

record LogAttributeRecord(
	AttributeValue<int> Level,
	AttributeStringValue MessageTemplate,
	AttributeValue<int> EventId,

	AttributeStringValue Name
);
