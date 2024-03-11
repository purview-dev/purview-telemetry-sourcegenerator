using System.Collections.Immutable;

namespace Purview.Telemetry.SourceGenerator.Records;

record ActivityGenerationTarget(
	string ClassName,
	string ClassNamespace,
	string[] ParentClasses,
	string? FullNamespace,
	string? FullyQualifiedName,

	string InterfaceName,
	string FullyQualifiedInterfaceName,

	ActivitySourceAttributeRecord? ActivitySourceAttribute,
	string? ActivitySourceName,

	ImmutableArray<ActivityMethodGenerationTarget> ActivityMethods
,
	ActivityTargetAttributeRecord ActivityTargetAttributeRecord);

record ActivityTargetAttributeRecord(
	AttributeStringValue ActivitySource,
	AttributeStringValue ClassName,
	AttributeValue<bool> DefaultToTags,
	AttributeStringValue BaggageAndTagPrefix,
	AttributeValue<bool> IncludeActivitySourcePrefix,
	AttributeValue<bool> LowercaseBaggageAndTagKeys
);

record ActivitySourceAttributeRecord(
	AttributeStringValue Name,
	AttributeValue<bool> DefaultToTags,
	AttributeStringValue BaggageAndTagPrefix,
	AttributeStringValue BaggageAndTagSeparator,
	AttributeValue<bool> LowercaseBaggageAndTagKeys
);

record ActivityMethodGenerationTarget(
	bool IsEvent
);
