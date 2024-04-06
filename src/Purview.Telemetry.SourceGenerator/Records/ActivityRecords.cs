using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Purview.Telemetry.SourceGenerator.Records;

record ActivitySourceTarget(
	TelemetryGenerationAttributeRecord  TelemetryGeneration,
	GenerationType GenerationType,

	string ClassNameToGenerate,
	string? ClassNamespace,

	string[] ParentClasses,
	string? FullNamespace,
	string? FullyQualifiedName,

	string InterfaceName,
	string FullyQualifiedInterfaceName,

	ActivitySourceGenerationAttributeRecord? ActivitySourceAttribute,
	string? ActivitySourceName,

	ImmutableArray<ActivityBasedGenerationTarget> ActivityMethods
,
	ActivitySourceAttributeRecord  ActivityTargetAttributeRecord
);

record ActivityBasedGenerationTarget(
	string MethodName,
	string ReturnType,
	bool IsNullableReturn,
	string ActivityOrEventName,

	Location? MethodLocation,

	ActivityAttributeRecord? ActivityAttribute,
	EventAttributeRecord? EventAttribute,

	ActivityMethodType MethodType,

	ImmutableArray<ActivityBasedParameterTarget> Parameters,
	ImmutableArray<ActivityBasedParameterTarget> Baggage,
	ImmutableArray<ActivityBasedParameterTarget> Tags,

	TargetGeneration TargetGenerationState
);

record ActivityBasedParameterTarget(
	string ParameterName,
	string ParameterType,
	bool IsNullable,
	string GeneratedName,
	ActivityParameterDestination ParamDestination,
	bool SkipOnNullOrEmpty,
	bool IsException,
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
	Timestamp,
	Escape
}

enum ActivityMethodType {
	Activity,
	Event,
	Context
}
