using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Helpers;

partial class PipelineHelpers {
	static public bool HasActivityTargetAttribute(SyntaxNode _, CancellationToken __) => true;

	static public ActivityGenerationTarget? BuildActivityTransform(GeneratorAttributeSyntaxContext context, IGenerationLogger? logger, CancellationToken token) {
		token.ThrowIfCancellationRequested();

		if (context.TargetNode is not InterfaceDeclarationSyntax interfaceDeclaration) {
			logger?.Error($"Could not find interface syntax from the target node '{context.TargetNode.Flatten()}'.");
			return null;
		}

		if (context.TargetSymbol is not INamedTypeSymbol interfaceSymbol) {
			logger?.Error($"Could not find interface symbol '{interfaceDeclaration.Flatten()}'.");
			return null;
		}

		var semanticModel = context.SemanticModel;
		var activityTargetAttribute = SharedHelpers.GetActivityTargetAttribute(context.Attributes[0], semanticModel, logger, token);
		if (activityTargetAttribute == null) {
			logger?.Error($"Could not find {Constants.Activities.ActivityTargetAttribute} when one was expected '{interfaceDeclaration.Flatten()}'.");
			return null;
		}

		var className = activityTargetAttribute.ClassName.IsSet
			? activityTargetAttribute.ClassName.Value!
			: GenerateClassName(interfaceSymbol.Name);

		var activitySource = GetActivitySourceAttribute(semanticModel, logger, token);
		var activitySourceName = activitySource?.Name?.IsSet == true
			? activitySource.Name.Value!
			: activityTargetAttribute.ActivitySource.IsSet
				? activityTargetAttribute.ActivitySource.Value!
				: "TODO";

		var fullNamespace = Utilities.GetFullNamespace(interfaceDeclaration, true);
		var activityMethods = BuildActivityMethods(
			className,
			activityTargetAttribute,
			context,
			semanticModel,
			interfaceSymbol,
			logger,
			token);

		return new(
			ClassName: className,
			ClassNamespace: Utilities.GetNamespace(interfaceDeclaration),
			ParentClasses: Utilities.GetParentClasses(interfaceDeclaration),
			FullNamespace: fullNamespace,
			FullyQualifiedName: fullNamespace + className,

			InterfaceName: interfaceSymbol.Name,
			FullyQualifiedInterfaceName: fullNamespace + interfaceSymbol.Name,

			ActivitySourceName: activitySourceName,

			ActivityMethods: activityMethods
		);
	}

	static ImmutableArray<ActivityMethodGenerationTarget> BuildActivityMethods(
		string className,
		ActivityTargetAttributeRecord activityTarget,
		GeneratorAttributeSyntaxContext context,
		SemanticModel semanticModel,
		INamedTypeSymbol interfaceSymbol,
		IGenerationLogger? logger,
		CancellationToken token) {

		token.ThrowIfCancellationRequested();

		List<ActivityMethodGenerationTarget> methodTargets = [];
		foreach (var method in interfaceSymbol.GetMembers().OfType<IMethodSymbol>()) {
			if (Utilities.ContainsAttribute(method, Constants.Activities.ActivityExcludeAttribute, token)) {
				logger?.Debug($"Skipping {interfaceSymbol.Name}.{method.Name}, explicitly excluded.");
				continue;
			}

			logger?.Debug($"Found method {interfaceSymbol.Name}.{method.Name}.");
		}

		return [.. methodTargets];
	}

	static ActivitySourceAttributeRecord? GetActivitySourceAttribute(SemanticModel semanticModel, IGenerationLogger? logger, CancellationToken token) {
		token.ThrowIfCancellationRequested();

		if (!Utilities.TryContainsAttribute(semanticModel.Compilation.Assembly, Constants.Activities.ActivitySourceAttribute, token, out var attributeData))
			return null;

		return SharedHelpers.GetActivitySourceAttribute(attributeData!, semanticModel, logger, token);
	}
}
