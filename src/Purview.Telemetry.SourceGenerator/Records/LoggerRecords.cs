﻿using System.Collections.Immutable;
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

	ImmutableArray<LogTarget> LogMethods,
	ImmutableDictionary<string, Location[]> DuplicateMethods
);

record LogTarget(
	string MethodName,
	bool IsScoped,
	string LoggerActionFieldName,

	bool UnknownReturnType,

	string LogName,
	int? EventId,
	string MessageTemplate,
	string MSLevel,

	ImmutableArray<LogParameterTarget> Parameters,
	ImmutableArray<LogParameterTarget> ParametersSansException,

	LogParameterTarget? ExceptionParameter,
	bool HasMultipleExceptions,

	Location? MethodLocation
,
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

	bool IsException
);
