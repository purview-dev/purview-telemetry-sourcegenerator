using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Purview.Telemetry.SourceGenerator.Records;

record ActivityGenerationTarget(
	TelemetryGenerationAttributeRecord TelemetryGeneration,
	GenerationType GenerationType,

	string ClassNameToGenerate,
	string ClassNamespace,

	string[] ParentClasses,
	string? FullNamespace,
	string? FullyQualifiedName,

	string InterfaceName,
	string FullyQualifiedInterfaceName,

	ActivitySourceGenerationAttributeRecord? ActivitySourceAttribute,
	string? ActivitySourceName,

	ImmutableArray<ActivityMethodGenerationTarget> ActivityMethods
,
	ActivitySourceAttributeRecord ActivityTargetAttributeRecord
);

record ActivityMethodGenerationTarget(
	string MethodName,
	string ReturnType,
	bool IsNullableReturn,
	string ActivityOrEventName,

	Location? MethodLocation,

	ActivityAttributeRecord? ActivityAttribute,
	EventAttributeRecord? EventAttribute,

	ActivityMethodType MethodType,

	ImmutableArray<ActivityMethodParameterTarget> Parameters,
	ImmutableArray<ActivityMethodParameterTarget> Baggage,
	ImmutableArray<ActivityMethodParameterTarget> Tags,

	TargetGeneration TargetGenerationState
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

enum ActivityMethodType {
	Activity,
	Event,
	Context
}
