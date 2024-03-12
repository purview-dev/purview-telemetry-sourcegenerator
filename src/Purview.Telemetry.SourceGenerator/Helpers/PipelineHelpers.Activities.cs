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

		var activitySourceAttribute = GetActivitySourceAttribute(semanticModel, logger, token);
		var activitySourceName = activitySourceAttribute?.Name?.IsSet == true
			? activitySourceAttribute.Name.Value!
			: activityTargetAttribute.ActivitySource.IsSet
				? activityTargetAttribute.ActivitySource.Value!
				: null;

		var fullNamespace = Utilities.GetFullNamespace(interfaceDeclaration, true);
		var activityMethods = BuildActivityMethods(
			activityTargetAttribute,
			activitySourceAttribute,
			semanticModel,
			interfaceSymbol,
			logger,
			token);

		return new(
			ClassNameToGenerate: className,
			ClassNamespace: Utilities.GetNamespace(interfaceDeclaration),

			ParentClasses: Utilities.GetParentClasses(interfaceDeclaration),
			FullNamespace: fullNamespace,
			FullyQualifiedName: fullNamespace + className,

			InterfaceName: interfaceSymbol.Name,
			FullyQualifiedInterfaceName: fullNamespace + interfaceSymbol.Name,

			ActivitySourceAttribute: activitySourceAttribute,
			ActivitySourceName: activitySourceName,

			ActivityTargetAttributeRecord: activityTargetAttribute,

			ActivityMethods: activityMethods
		);
	}

	static ImmutableArray<ActivityMethodGenerationTarget> BuildActivityMethods(
		ActivityTargetAttributeRecord activityTarget,
		ActivitySourceAttributeRecord? activitySourceAttribute,
		SemanticModel semanticModel,
		INamedTypeSymbol interfaceSymbol,
		IGenerationLogger? logger,
		CancellationToken token) {

		token.ThrowIfCancellationRequested();

		var prefix = GeneratePrefix(activitySourceAttribute, activityTarget, token);
		var defaultToTags = activitySourceAttribute?.DefaultToTags?.IsSet == true
			? activitySourceAttribute.DefaultToTags.Value!.Value
			: activityTarget.DefaultToTags.Value!.Value;
		var lowercaseBaggageAndTagKeys = activityTarget.LowercaseBaggageAndTagKeys!.Value!.Value;

		List<ActivityMethodGenerationTarget> methodTargets = [];
		foreach (var method in interfaceSymbol.GetMembers().OfType<IMethodSymbol>()) {
			token.ThrowIfCancellationRequested();

			if (Utilities.ContainsAttribute(method, Constants.Activities.ActivityExcludeAttribute, token)) {
				logger?.Debug($"Skipping {interfaceSymbol.Name}.{method.Name}, explicitly excluded.");
				continue;
			}

			var isActivity = IsActivity(method, semanticModel, logger, token, out var activityAttribute, out var activityEventAttribute);
			var activityOrEventName = activityAttribute?.Name?.Value ?? activityEventAttribute?.Name?.Value;
			if (string.IsNullOrWhiteSpace(activityOrEventName)) {
				activityOrEventName = method.Name;
			}

			logger?.Debug($"Found {(isActivity ? "activity" : "event")} method {interfaceSymbol.Name}.{method.Name}.");

			var parameters = GetActivityParameters(method, prefix, defaultToTags, lowercaseBaggageAndTagKeys, semanticModel, logger, token);
			var baggageParameters = parameters.Where(m => m.ParamDestination == ActivityParameterDestination.Baggage).ToImmutableArray();
			var tagParameters = parameters.Where(m => m.ParamDestination == ActivityParameterDestination.Tag).ToImmutableArray();
			var activityParam = parameters.FirstOrDefault(m => Constants.Activities.SystemDiagnostics.Activity.Equals(m.ParameterType));
			var activityAccessorName = activityParam?.ParameterName ?? $"{Constants.Activities.ActivityAttribute}.Current";

			methodTargets.Add(new(
				MethodName: method.Name,
				ReturnType: Utilities.GetFullyQualifiedName(method.ReturnType),
				ActivityOrEventName: activityOrEventName!,

				ActivityAttribute: activityAttribute,
				ActivityEventAttribute: activityEventAttribute,

				IsActivity: isActivity,
				ActivityAccessorName: activityAccessorName,

				Baggage: baggageParameters,
				Tags: tagParameters
			));
		}

		return [.. methodTargets];
	}

	static ImmutableArray<ActivityMethodParameterTarget> GetActivityParameters(IMethodSymbol method,
		string? prefix,
		bool defaultToTags,
		bool lowercaseBaggageAndTagKeys,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {
		List<ActivityMethodParameterTarget> parameterTargets = [];

		foreach (var parameter in method.Parameters) {
			token.ThrowIfCancellationRequested();

			var destination = defaultToTags ? ActivityParameterDestination.Tag : ActivityParameterDestination.Baggage;
			if (Utilities.TryContainsAttribute(parameter, Constants.Activities.TagAttribute, token, out var attribute)) {
				logger?.Debug($"Found explicit tag: {parameter.Name}.");
				destination = ActivityParameterDestination.Tag;
			}
			else if (Utilities.TryContainsAttribute(parameter, Constants.Activities.BaggageAttribute, token, out attribute)) {
				logger?.Debug($"Found explicit baggage: {parameter.Name}.");
				destination = ActivityParameterDestination.Baggage;
			}
			else {
				if (Constants.Activities.SystemDiagnostics.Activity.Equals(parameter.Type)
					|| Constants.Activities.SystemDiagnostics.ActivityTagsCollection.Equals(parameter.Type)
					|| Constants.Activities.SystemDiagnostics.ActivityContext.Equals(parameter.Type)
					|| Constants.Activities.SystemDiagnostics.ActivityLink.Equals(parameter.Type)
					|| Constants.Activities.SystemDiagnostics.ActivityLinkArray.Equals(parameter.Type)
					|| Constants.Activities.SystemDiagnostics.ActivityLinkIEnumerable.Equals(parameter.Type)
					|| Constants.Activities.SystemDiagnostics.ActivityTagIEnumerable.Equals(parameter.Type)
					|| Constants.Activities.SystemDiagnostics.ActivityTagsCollection.Equals(parameter.Type)) {
					destination = ActivityParameterDestination.Useful;

					logger?.Debug($"Found a useful parameter: {parameter.Name} ({parameter.Type}).");
				}
				else {
					logger?.Debug($"Inferring {(defaultToTags ? "tag" : "baggage")}: {parameter.Name}.");
				}
			}

			TagOrBaggageAttributeRecord? tagOrBaggageAttribute = null;
			if (attribute != null) {
				tagOrBaggageAttribute = SharedHelpers.GetTagOrBaggageAttribute(attribute, semanticModel, logger, token);
			}

			var parameterName = parameter.Name;
			var parameterType = parameter.Type.ToDisplayString();
			var generatedName = GenerateParameterName(tagOrBaggageAttribute?.Name?.Value ?? parameterName, prefix, lowercaseBaggageAndTagKeys);

			parameterTargets.Add(new(
				ParameterName: parameterName,
				ParameterType: parameterType,
				GeneratedName: generatedName,
				ParamDestination: destination,
				SkipOnNullOrEmpty: (tagOrBaggageAttribute?.SkipOnNullOrEmpty?.IsSet) != true
					|| tagOrBaggageAttribute!.SkipOnNullOrEmpty!.Value!.Value
			));
		}

		return [.. parameterTargets];
	}

	static string GenerateParameterName(string name, string? prefix, bool lowercaseBaggageAndTagKeys) {
		if (lowercaseBaggageAndTagKeys) {
			name = name.ToLowerInvariant();
		}

		return $"{prefix}{name}";
	}

	static bool IsActivity(IMethodSymbol method, SemanticModel semanticModel, IGenerationLogger? logger, CancellationToken token, out ActivityAttributeRecord? activityAttribute, out ActivityEventAttributeRecord? eventAttribute) {
		activityAttribute = null;
		eventAttribute = null;

		token.ThrowIfCancellationRequested();

		if (Utilities.TryContainsAttribute(method, Constants.Activities.ActivityAttribute, token, out var attributeData)) {
			activityAttribute = SharedHelpers.GetActivityAttribute(attributeData!, semanticModel, logger, token);

			logger?.Debug($"Found explicit activity: {method.Name}.");

			return true;
		}

		if (Utilities.TryContainsAttribute(method, Constants.Activities.ActivityEventAttribute, token, out attributeData)) {
			eventAttribute = SharedHelpers.GetActivityEventAttribute(attributeData!, semanticModel, logger, token);

			logger?.Debug($"Found explicit event: {method.Name}.");

			return false;
		}

		var returnType = method.ReturnType;
		if (Constants.Activities.SystemDiagnostics.Activity.Equals(returnType)) {
			//|| Constants.System.IDisposable.Equals(returnType)) {

			logger?.Debug($"Inferring activity due to return type ({returnType.ToDisplayString()}): {method.Name}.");

			return true;
		}

		if (method.Name.EndsWith("Event", StringComparison.Ordinal)) {
			logger?.Debug($"Inferring event as the method name ends in 'Event': {method.Name}.");

			return false;
		}

		logger?.Debug($"Defaulting to activity: {method.Name}.");

		return true;
	}

	static string? GeneratePrefix(ActivitySourceAttributeRecord? activitySourceAttribute, ActivityTargetAttributeRecord activityTarget, CancellationToken token) {
		token.ThrowIfCancellationRequested();

		string? prefix = null;
		var separator = activitySourceAttribute?.BaggageAndTagSeparator?.IsSet == true
			? activitySourceAttribute.BaggageAndTagSeparator.Value
			: ".";

		if (activitySourceAttribute?.BaggageAndTagPrefix?.IsSet == true) {
			prefix = activitySourceAttribute.BaggageAndTagPrefix.Value;
			if (activitySourceAttribute.BaggageAndTagSeparator != null) {
				prefix += separator;
			}
		}

		if (activityTarget.BaggageAndTagPrefix.IsSet) {
			if (activityTarget.IncludeActivitySourcePrefix.Value == true) {
				prefix += activityTarget.BaggageAndTagPrefix.Value + separator;
			}
			else {
				prefix = activityTarget.BaggageAndTagPrefix.Value + separator;
			}
		}

		return prefix;
	}

	static ActivitySourceAttributeRecord? GetActivitySourceAttribute(SemanticModel semanticModel, IGenerationLogger? logger, CancellationToken token) {
		token.ThrowIfCancellationRequested();

		if (!Utilities.TryContainsAttribute(semanticModel.Compilation.Assembly, Constants.Activities.ActivitySourceAttribute, token, out var attributeData))
			return null;

		return SharedHelpers.GetActivitySourceAttribute(attributeData!, semanticModel, logger, token);
	}
}
