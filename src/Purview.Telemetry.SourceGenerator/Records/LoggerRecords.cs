using Purview.Telemetry.Logging;

namespace Purview.Telemetry.SourceGenerator.Targets;

record LoggerTarget(
	string ClassNameToGenerate,
	string? ClassNamespace, string[] ParentClasses,
	string? FullNamespace, string FullyQualifiedName,

	string InterfaceName, string FullyQualifiedInterfaceName,

	LoggerTargetAttributeRecord LoggerTargetAttribute,
	LoggerDefaultsAttributeRecord? LoggerDefaultsAttribute
);

record LoggerTargetAttributeRecord(
	AttributeValue<int> StartingEventId,
	AttributeValue<LogGeneratedLevel> DefaultLevel,
	AttributeStringValue ClassName,
	AttributeStringValue CustomPrefix,
	AttributeValue<LogPrefixType> PrefixType
);

record LoggerDefaultsAttributeRecord(
	AttributeValue<LogGeneratedLevel> DefaultLevel
);
