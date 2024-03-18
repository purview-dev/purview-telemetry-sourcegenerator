using Purview.Telemetry.Activities;

namespace Purview.Telemetry.SourceGenerator.Records;

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

record ActivityGenAttributeRecord(
	AttributeStringValue Name,
	AttributeValue<ActivityGeneratedKind> Kind,
	AttributeValue<bool> CreateOnly
);

record ActivityEventAttributeRecord(
	AttributeStringValue Name
);
