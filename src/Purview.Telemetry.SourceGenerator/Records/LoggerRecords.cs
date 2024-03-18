﻿using System.Collections.Immutable;
using Purview.Telemetry.Logging;
using Purview.Telemetry.SourceGenerator.Templates;

namespace Purview.Telemetry.SourceGenerator.Records;

record LoggerGenerationTarget(
	TelemetryGenerationAttributeRecord TelemetryGeneration,

	string ClassNameToGenerate,
	string? ClassNamespace, string[] ParentClasses,
	string? FullNamespace, string FullyQualifiedName,

	string InterfaceName, string FullyQualifiedInterfaceName,

	LoggerTargetAttributeRecord LoggerTargetAttribute,
	LogGeneratedLevel DefaultLevel,

	ImmutableArray<LogEntryMethodGenerationTarget> LogEntryMethods
);

record LogEntryMethodGenerationTarget(
	string MethodName,
	bool IsScoped,
	string LoggerActionFieldName,

	bool UnknownReturnType,

	int? EventId,
	LogGeneratedLevel Level,
	string MessageTemplate,
	string LogEntryName,

	TypeInfo MSLevel,

	ImmutableArray<LogEntryMethodParameterTarget> AllParameters,
	ImmutableArray<LogEntryMethodParameterTarget> ParametersSansException,

	LogEntryMethodParameterTarget? ExceptionParameter,
	bool HasMultipleExceptions,

	Microsoft.CodeAnalysis.Location? Location
,
	bool InferredErrorLevel) {
	public int TotalParameterCount => AllParameters.Length;

	public int ParameterCount => ParametersSansException.Length;
}

record LogEntryMethodParameterTarget(
	string Name,
	string UpperCasedName,
	string FullyQualifiedType,
	bool IsNullable,

	bool IsException
);
