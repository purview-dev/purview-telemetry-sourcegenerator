using Purview.Telemetry.Logging;

namespace Purview.Telemetry.SourceGenerator.Records;

record LoggerTargetAttributeRecord(
	AttributeValue<LogGeneratedLevel> DefaultLevel,

	AttributeStringValue ClassName,

	AttributeStringValue CustomPrefix,
	AttributeValue<LogPrefixType> PrefixType
);

record LoggerDefaultsAttributeRecord(
	AttributeValue<LogGeneratedLevel> DefaultLevel
);

record LogEntryAttributeRecord(
	AttributeValue<LogGeneratedLevel> Level,
	AttributeStringValue MessageTemplate,
	AttributeValue<int> EventId,

	AttributeStringValue Name
);
