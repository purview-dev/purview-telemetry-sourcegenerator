using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Purview.Telemetry.SourceGenerator.Records;

record MeterGenerationTarget(
	TelemetryGenerationAttributeRecord TelemetryGeneration,
	GenerationType GenerationType,

	string ClassNameToGenerate,
	string ClassNamespace,

	string[] ParentClasses,
	string? FullNamespace,
	string? FullyQualifiedName,

	string InterfaceName,
	string FullyQualifiedInterfaceName,

	string? MeterName,

	MeterGenerationAttributeRecord? MeterGeneration,

	ImmutableArray<InstrumentMethodTarget> InstrumentationMethods
);

record InstrumentMethodTarget(
	string MethodName,
	string ReturnType,
	bool ReturnsBool,
	bool IsNullableReturn,

	string FieldName,
	string InstrumentMeasurementType,
	bool IsObservable,

	string MetricName,

	Location? MethodLocation,

	InstrumentAttributeRecord? InstrumentAttribute,

	ImmutableArray<InstrumentMethodParameterTarget> Parameters,
	ImmutableArray<InstrumentMethodParameterTarget> Tags,
	InstrumentMethodParameterTarget? MeasurementParameter,

	ImmutableArray<TelemetryDiagnosticDescriptor> ErrorDiagnostics,

	TargetGeneration TargetGenerationState
);

record InstrumentMethodParameterTarget(
	string ParameterName,
	string ParameterType,

	bool IsFunc,
	bool IsIEnumerable,
	bool IsMeasurement,
	bool IsValidInstrumentType,

	string? InstrumentType,

	bool IsNullable,
	string GeneratedName,
	InstrumentParameterDestination ParamDestination,
	bool SkipOnNullOrEmpty,
	Location? Location
);

enum InstrumentTypes {
	Counter,
	UpDownCounter,
	Histogram,

	ObservableCounter,
	ObservableGauge,
	ObservableUpDownCounter
}

enum InstrumentParameterDestination {
	Tag,
	Measurement,
	Unknown
}
