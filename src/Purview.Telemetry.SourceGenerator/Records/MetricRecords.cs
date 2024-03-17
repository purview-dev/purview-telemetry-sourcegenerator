using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Purview.Telemetry.SourceGenerator.Records;

record MeterGenerationTarget(
	TelemetryGenerationAttributeRecord TelemetryGeneration,

	string ClassNameToGenerate,
	string ClassNamespace,

	string[] ParentClasses,
	string? FullNamespace,
	string? FullyQualifiedName,

	string InterfaceName,
	string FullyQualifiedInterfaceName,

	string? MeterName,

	MeterAssemblyAttributeRecord? MeterAssembly,

	ImmutableArray<InstrumentMethodGenerationTarget> InstrumentationMethods
);

record InstrumentMethodGenerationTarget(
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

	ImmutableArray<TelemetryDiagnosticDescriptor> ErrorDiagnostics
);

record InstrumentAttributeRecord(
	AttributeStringValue Name,
	AttributeStringValue? Unit,
	AttributeStringValue? Description,
	AttributeValue<bool>? AutoIncrement,
	AttributeValue<bool>? ThrowOnAlreadyInitialized,
	InstrumentTypes InstrumentType
) {
	public bool IsAutoIncrement => AutoIncrement?.Value ?? false;

	public bool IsObservable => InstrumentType is InstrumentTypes.ObservableCounter or InstrumentTypes.ObservableGauge or InstrumentTypes.ObservableUpDownCounter;
}

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

record MeterAssemblyAttributeRecord(
	AttributeStringValue InstrumentPrefix,
	AttributeStringValue InstrumentSeparator,
	AttributeValue<bool> LowercaseInstrumentName,
	AttributeValue<bool> LowercaseTagKeys
);

record MeterTargetAttributeRecord(
	AttributeStringValue Name,
	AttributeStringValue ClassName,
	AttributeStringValue InstrumentPrefix,
	AttributeValue<bool> IncludeAssemblyInstrumentPrefix,
	AttributeValue<bool> LowercaseInstrumentName,
	AttributeValue<bool> LowercaseTagKeys
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
