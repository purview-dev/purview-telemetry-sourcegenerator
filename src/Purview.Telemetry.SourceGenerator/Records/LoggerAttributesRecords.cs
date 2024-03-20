using Purview.Telemetry.Logging;

namespace Purview.Telemetry.SourceGenerator.Records;

record LoggerAttributeRecord(
	AttributeValue<LogGeneratedLevel> DefaultLevel,

	AttributeStringValue CustomPrefix,
	AttributeValue<LogPrefixType> PrefixType
);

record LoggerGenerationAttributeRecord(
	AttributeValue<LogGeneratedLevel> DefaultLevel
);

record LogAttributeRecord(
	AttributeValue<LogGeneratedLevel> Level,
	AttributeStringValue MessageTemplate,
	AttributeValue<int> EventId,

	AttributeStringValue Name
);
