using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Purview.Telemetry.SourceGenerator.Records;

record LoggerTarget(
	TelemetryGenerationAttributeRecord TelemetryGeneration,
	GenerationType GenerationType,

	string ClassNameToGenerate,
	string? ClassNamespace,
	string[] ParentClasses,
	string? FullNamespace,
	string FullyQualifiedName,

	string InterfaceName,
	string FullyQualifiedInterfaceName,

	LoggerAttributeRecord LoggerAttribute,
	int DefaultLevel,

	ImmutableArray<LogMethodTarget> LogMethods,
	ImmutableDictionary<string, Location[]> DuplicateMethods,

	bool UseMSLoggingTelemetryBasedGeneration)
{
	public TelemetryDiagnosticDescriptor? Failure { get; init; }

	public static LoggerTarget Failed(TelemetryDiagnosticDescriptor diagnostic)
		=> new(
		null!,
		GenerationType.None,
		null!,
		null,
		null!,
		null,
		null!,
		null!,
		null!,
		null!,
		0,
		[],
		null!,
		false)
		{
			Failure = diagnostic
		};
}

record LogMethodTarget(
	string MethodName,
	bool IsScoped,
	string LoggerActionFieldName,

	bool UnknownReturnType,

	string LogName,
	int? EventId,

	string MessageTemplate,
	ImmutableArray<MessageTemplateHole> TemplateProperties,
	bool TemplateIsOrdinalBased,
	bool TemplateIsNamedBased,

	string MSLevel,

	ImmutableArray<LogParameterTarget> Parameters,
	ImmutableArray<LogParameterTarget> ParametersSansException,

	LogParameterTarget? ExceptionParameter,
	bool HasMultipleExceptions,

	Location? MethodLocation,

	bool InferredErrorLevel,

	TargetGeneration TargetGenerationState
)
{
	public int TotalParameterCount => Parameters.Length;

	public int ParameterCount => ParametersSansException.Length;
}

record LogParameterTarget(
	string Name,
	string UpperCasedName,
	string FullyQualifiedType,

	bool IsNullable,
	bool IsException,
	bool IsFirstException,

	bool IsIEnumerable,
	bool IsArray,

	bool IsComplexType,

	LogPropertiesAttributeRecord? LogPropertiesAttribute,
	ImmutableArray<LogPropertiesParameterDetails>? LogProperties,

	ExpandEnumerableAttributeRecord? ExpandEnumerableAttribute)
{
	public bool UsedInTemplate => ReferencedHoles.Count > 0;

	public List<MessageTemplateHole> ReferencedHoles { get; } = [];
}

record LogPropertiesParameterDetails(
	string PropertyName,
	bool IsNullable
);
