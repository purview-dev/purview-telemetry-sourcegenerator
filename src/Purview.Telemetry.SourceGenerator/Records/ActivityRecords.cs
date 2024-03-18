using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.Activities;

namespace Purview.Telemetry.SourceGenerator.Records;

record ActivityGenerationTarget(
	TelemetryGenerationAttributeRecord TelemetryGeneration,

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
	ActivityTargetAttributeRecord ActivityTargetAttributeRecord
);

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
	bool IsNullableReturn,
	string ActivityOrEventName,

	Location? MethodLocation,

	ActivityGenAttributeRecord? ActivityAttribute,
	ActivityEventAttributeRecord? ActivityEventAttribute,

	bool IsActivity,

	ImmutableArray<ActivityMethodParameterTarget> Parameters,
	ImmutableArray<ActivityMethodParameterTarget> Baggage,
	ImmutableArray<ActivityMethodParameterTarget> Tags
);

record ActivityMethodParameterTarget(
	string ParameterName,
	string ParameterType,
	bool IsNullable,
	string GeneratedName,
	ActivityParameterDestination ParamDestination,
	bool SkipOnNullOrEmpty,
	Location? Location
);

enum ActivityParameterDestination {
	Tag,
	Baggage,
	ParentContextOrId,
	TagsEnumerable,
	LinksEnumerable,
	Activity,
	StartTime,
	Timestamp
}

record ActivityGenAttributeRecord(
	AttributeStringValue Name,
	AttributeValue<ActivityGeneratedKind> Kind,
	AttributeValue<bool> CreateOnly
);

record ActivityEventAttributeRecord(
	AttributeStringValue Name
);
