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

		var activitySourceAttribute = SharedHelpers.GetActivitySourceAttribute(semanticModel, logger, token);
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

		var telemetryGeneration = SharedHelpers.GetTelemetryGenerationAttribute(interfaceSymbol, semanticModel, logger, token);

		return new(
			TelemetryGeneration: telemetryGeneration,

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
		ActivitySourceAttributeRecord? activitySource,
		SemanticModel semanticModel,
		INamedTypeSymbol interfaceSymbol,
		IGenerationLogger? logger,
		CancellationToken token) {

		token.ThrowIfCancellationRequested();

		var prefix = GeneratePrefix(activitySource, activityTarget, token);
		var defaultToTags = activitySource?.DefaultToTags?.IsSet == true
			? activitySource.DefaultToTags.Value!.Value
			: activityTarget.DefaultToTags.Value!.Value;
		var lowercaseBaggageAndTagKeys = activityTarget.LowercaseBaggageAndTagKeys!.Value!.Value;

		List<ActivityMethodGenerationTarget> methodTargets = [];
		foreach (var method in interfaceSymbol.GetMembers().OfType<IMethodSymbol>()) {
			token.ThrowIfCancellationRequested();

			if (Utilities.ContainsAttribute(method, Constants.Activities.ActivityExcludeAttribute, token)) {
				logger?.Debug($"Skipping {interfaceSymbol.Name}.{method.Name}, explicitly excluded.");
				continue;
			}

			var isActivity = IsActivity(method, semanticModel, logger, token,
				out var activityGenAttribute,
				out var activityEventAttribute
			);
			var activityOrEventName = activityGenAttribute?.Name?.Value ?? activityEventAttribute?.Name?.Value;
			if (string.IsNullOrWhiteSpace(activityOrEventName)) {
				activityOrEventName = method.Name;
			}

			logger?.Debug($"Found {(isActivity ? "activity" : "event")} method {interfaceSymbol.Name}.{method.Name}.");

			var parameters = GetActivityParameters(method, prefix, defaultToTags, lowercaseBaggageAndTagKeys, semanticModel, logger, token);
			var baggageParameters = parameters.Where(m => m.ParamDestination == ActivityParameterDestination.Baggage).ToImmutableArray();
			var tagParameters = parameters.Where(m => m.ParamDestination == ActivityParameterDestination.Tag).ToImmutableArray();

			var returnType = method.ReturnsVoid
				? Constants.System.VoidKeyword
				: Utilities.GetFullyQualifiedName(method.ReturnType);

			methodTargets.Add(new(
				MethodName: method.Name,
				ReturnType: returnType,
				IsNullableReturn: method.ReturnType.NullableAnnotation == NullableAnnotation.Annotated,
				ActivityOrEventName: activityOrEventName!,
				MethodLocation: method.Locations.FirstOrDefault(),

				ActivityAttribute: activityGenAttribute,
				ActivityEventAttribute: activityEventAttribute,

				IsActivity: isActivity,

				Parameters: parameters,
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
			if (Utilities.TryContainsAttribute(parameter, Constants.Shared.TagAttribute, token, out var attribute)) {
				logger?.Debug($"Found explicit tag: {parameter.Name}.");
				destination = ActivityParameterDestination.Tag;
			}
			else if (Utilities.TryContainsAttribute(parameter, Constants.Activities.BaggageAttribute, token, out attribute)) {
				logger?.Debug($"Found explicit baggage: {parameter.Name}.");
				destination = ActivityParameterDestination.Baggage;
			}
			else if (Constants.Activities.SystemDiagnostics.Activity.Equals(parameter.Type)) {
				destination = ActivityParameterDestination.Activity;
			}
			else if (Constants.Activities.SystemDiagnostics.ActivityTagsCollection.Equals(parameter.Type)
				|| Constants.Activities.SystemDiagnostics.ActivityTagIEnumerable.Equals(parameter.Type)
				|| Constants.System.TagList.Equals(parameter.Type)) {
				destination = ActivityParameterDestination.TagsEnumerable;
			}
			else if (Constants.Activities.SystemDiagnostics.ActivityContext.Equals(parameter.Type)
				|| (parameter.Name == Constants.Activities.ParentIdParameterName && Utilities.IsString(parameter.Type))) {
				destination = ActivityParameterDestination.ParentContextOrId;
			}
			else if (Constants.Activities.SystemDiagnostics.ActivityLink.Equals(parameter.Type)
				|| Constants.Activities.SystemDiagnostics.ActivityLinkArray.Equals(parameter.Type)
				|| Constants.Activities.SystemDiagnostics.ActivityLinkIEnumerable.Equals(parameter.Type)) {
				destination = ActivityParameterDestination.LinksEnumerable;
			}
			else if (parameter.Name == Constants.Activities.StartTimeParameterName && Constants.System.DateTimeOffset.Equals(parameter.Type)) {
				destination = ActivityParameterDestination.StartTime;
			}
			else {
				logger?.Debug($"Inferring {(defaultToTags ? "tag" : "baggage")}: {parameter.Name}.");
				// destination is already set to default.
			}

			TagOrBaggageAttributeRecord? tagOrBaggageAttribute = null;
			if (attribute != null) {
				tagOrBaggageAttribute = SharedHelpers.GetTagOrBaggageAttribute(attribute, semanticModel, logger, token);
			}

			var parameterName = parameter.Name;
			var parameterType = parameter.Type.ToDisplayString().TrimEnd('?');
			var generatedName = GenerateParameterName(tagOrBaggageAttribute?.Name?.Value ?? parameterName, prefix, lowercaseBaggageAndTagKeys);

			parameterTargets.Add(new(
				ParameterName: parameterName,
				ParameterType: parameterType,
				IsNullable: parameter.Type.NullableAnnotation == NullableAnnotation.Annotated,
				GeneratedName: generatedName,
				ParamDestination: destination,
				SkipOnNullOrEmpty: GetSkipOnNullOrEmptyValue(tagOrBaggageAttribute),
				Location: parameter.Locations.FirstOrDefault()
			));
		}

		return [.. parameterTargets];
	}

	static bool IsActivity(IMethodSymbol method, SemanticModel semanticModel, IGenerationLogger? logger, CancellationToken token, out ActivityGenAttributeRecord? activityGenAttribute, out ActivityEventAttributeRecord? eventAttribute) {
		activityGenAttribute = null;
		eventAttribute = null;

		token.ThrowIfCancellationRequested();

		if (Utilities.TryContainsAttribute(method, Constants.Activities.ActivityGenAttribute, token, out var attributeData)) {
			activityGenAttribute = SharedHelpers.GetActivityGenAttribute(attributeData!, semanticModel, logger, token);

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
}
