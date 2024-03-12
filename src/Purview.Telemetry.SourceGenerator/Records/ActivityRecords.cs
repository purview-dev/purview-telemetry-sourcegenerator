using System.Collections.Immutable;
using Purview.Telemetry.Activities;

namespace Purview.Telemetry.SourceGenerator.Records;

record ActivityGenerationTarget(
	string ClassNameToGenerate,
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
	string MethodName,
	string ReturnType,
	string ActivityOrEventName,

	ActivityAttributeRecord? ActivityAttribute,
	ActivityEventAttributeRecord? ActivityEventAttribute,

	bool IsActivity,

	string ActivityAccessorName,

	ImmutableArray<ActivityMethodParameterTarget> Baggage,
	ImmutableArray<ActivityMethodParameterTarget> Tags
);

record ActivityMethodParameterTarget(
	string ParameterName,
	string ParameterType,
	string GeneratedName,
	ActivityParameterDestination ParamDestination,
	bool SkipOnNullOrEmpty
);

enum ActivityParameterDestination {
	Useful,
	Tag,
	Baggage
}

record TagOrBaggageAttributeRecord(
	AttributeStringValue Name,
	AttributeValue<bool> SkipOnNullOrEmpty
);

record ActivityAttributeRecord(
	AttributeStringValue Name,
	AttributeValue<ActivityGeneratedKind> Kind,
	AttributeValue<bool> CreateOnly
);

record ActivityEventAttributeRecord(
	AttributeStringValue Name
);
