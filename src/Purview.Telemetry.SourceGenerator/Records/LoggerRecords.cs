using System.Collections.Immutable;
using Purview.Telemetry.Logging;
using Purview.Telemetry.SourceGenerator.Templates;

namespace Purview.Telemetry.SourceGenerator.Records;

record LoggerTarget(
	TelemetryGenerationAttributeRecord TelemetryGeneration,
	GenerationType GenerationType,

	string ClassNameToGenerate,
	string? ClassNamespace, string[] ParentClasses,
	string? FullNamespace, string FullyQualifiedName,

	string InterfaceName, string FullyQualifiedInterfaceName,

	LoggerAttributeRecord LoggerAttribute,
	LogGeneratedLevel DefaultLevel,

	ImmutableArray<LogTarget> LogMethods
);

record LogTarget(
	string MethodName,
	bool IsScoped,
	string LoggerActionFieldName,

	bool UnknownReturnType,

	int? EventId,
	LogGeneratedLevel Level,
	string MessageTemplate,
	string LogName,

	TypeInfo MSLevel,

	ImmutableArray<LogParameterTarget> Parameters,
	ImmutableArray<LogParameterTarget> ParametersSansException,

	LogParameterTarget? ExceptionParameter,
	bool HasMultipleExceptions,

	Microsoft.CodeAnalysis.Location? MethodLocation
,
	bool InferredErrorLevel,

	TargetGeneration TargetGenerationState
) {
	public int TotalParameterCount => Parameters.Length;

	public int ParameterCount => ParametersSansException.Length;
}

record LogParameterTarget(
	string Name,
	string UpperCasedName,
	string FullyQualifiedType,
	bool IsNullable,

	bool IsException
);
