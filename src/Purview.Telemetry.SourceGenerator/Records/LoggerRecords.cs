using System.Collections.Immutable;
using Purview.Telemetry.Logging;

namespace Purview.Telemetry.SourceGenerator.Targets;

record LoggerGenerationTarget(
	string ClassNameToGenerate,
	string? ClassNamespace, string[] ParentClasses,
	string? FullNamespace, string FullyQualifiedName,

	string InterfaceName, string FullyQualifiedInterfaceName,

	LoggerTargetAttributeRecord LoggerTargetAttribute,
	LogGeneratedLevel DefaultLevel,

	ImmutableArray<LogEntryMethodGenerationTarget> LogEntryMethods
);

record LoggerTargetAttributeRecord(
	AttributeValue<LogGeneratedLevel> DefaultLevel,

	AttributeStringValue ClassName,

	AttributeStringValue CustomPrefix,
	AttributeValue<LogPrefixType> PrefixType
);

record LoggerDefaultsAttributeRecord(
	AttributeValue<LogGeneratedLevel> DefaultLevel
);

record LogEntryAttributeRecord(
	AttributeValue<LogGeneratedLevel> Level,
	AttributeStringValue MessageTemplate,
	AttributeValue<int> EventId,

	AttributeStringValue Name
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

	ImmutableArray<LogEntryMethodParameterTarget> Parameters
) {
	public int GetParameterCount(bool includingException = true)
		=> includingException
			? Parameters.Length
			: Parameters.Count(p => !p.IsException);

	public bool HasExceptionParameter => Parameters.Any(p => p.IsException);

	public LogEntryMethodParameterTarget? GetExceptionParameter()
		=> Parameters.FirstOrDefault(p => p.IsException);
}

record LogEntryMethodParameterTarget(
	string Name,
	string UpperCasedName,
	string FullyQualifiedType,
	bool IsNullable,

	bool IsException
) {
}
