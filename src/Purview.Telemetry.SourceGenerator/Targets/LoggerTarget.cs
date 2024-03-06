namespace Purview.Telemetry.SourceGenerator.Targets;

record LoggerTarget(
	string ClassName,
	string? ClassNamespace, string[] ParentClasses,
	string? FullNamespace, string FullyQualifiedName
	) {
}
