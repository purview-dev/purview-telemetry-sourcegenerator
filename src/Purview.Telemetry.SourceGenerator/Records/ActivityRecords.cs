using System.Collections.Immutable;

namespace Purview.Telemetry.SourceGenerator.Records;

record ActivityGenerationTarget(
	string ClassName,
	string ClassNamespace,
	string[] ParentClasses,
	string? FullNamespace,
	string? FullyQualifiedName,

	string InterfaceName,
	string FullyQualifiedInterfaceName,

	string ActivitySourceName,

	ImmutableArray<ActivityMethodGenerationTarget> ActivityMethods
);

record ActivityTargetAttributeRecord(
	AttributeStringValue ActivitySource,

	AttributeStringValue ClassName
);

record ActivitySourceAttributeRecord(
	AttributeStringValue Name
);

record ActivityMethodGenerationTarget(
);
