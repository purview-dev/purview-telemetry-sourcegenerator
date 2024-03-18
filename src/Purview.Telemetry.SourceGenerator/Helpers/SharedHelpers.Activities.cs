﻿using Microsoft.CodeAnalysis;
using Purview.Telemetry.Activities;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Helpers;
partial class SharedHelpers {
	static public ActivitySourceAttributeRecord? GetActivitySourceAttribute(SemanticModel semanticModel, IGenerationLogger? logger, CancellationToken token) {
		token.ThrowIfCancellationRequested();

		if (!Utilities.TryContainsAttribute(semanticModel.Compilation.Assembly, Constants.Activities.ActivitySourceGenerationAttribute, token, out var attributeData))
			return null;

		return GetActivitySourceAttribute(attributeData!, semanticModel, logger, token);
	}

	static public ActivityTargetAttributeRecord? GetActivityTargetAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {

		AttributeStringValue? activitySource = null;
		AttributeStringValue? className = null;
		AttributeValue<bool>? defaultToTags = null;
		AttributeStringValue? baggageAndTagPrefix = null;
		AttributeValue<bool>? includeActivitySourcePrefix = null;
		AttributeValue<bool>? lowercaseBaggageAndTagKeys = null;

		if (!AttributeParser(attributeData,
		(name, value) => {
			if (name.Equals(nameof(ActivitySourceTargetAttribute.ActivitySource), StringComparison.OrdinalIgnoreCase)) {
				activitySource = new((string)value);
			}
			else if (name.Equals(nameof(ActivitySourceTargetAttribute.ClassName), StringComparison.OrdinalIgnoreCase)) {
				className = new((string)value);
			}
			else if (name.Equals(nameof(ActivitySourceTargetAttribute.DefaultToTags), StringComparison.OrdinalIgnoreCase)) {
				defaultToTags = new((bool)value);
			}
			else if (name.Equals(nameof(ActivitySourceTargetAttribute.BaggageAndTagPrefix), StringComparison.OrdinalIgnoreCase)) {
				baggageAndTagPrefix = new((string)value);
			}
			else if (name.Equals(nameof(ActivitySourceTargetAttribute.IncludeActivitySourcePrefix), StringComparison.OrdinalIgnoreCase)) {
				includeActivitySourcePrefix = new((bool)value);
			}
			else if (name.Equals(nameof(ActivitySourceTargetAttribute.LowercaseBaggageAndTagKeys), StringComparison.OrdinalIgnoreCase)) {
				lowercaseBaggageAndTagKeys = new((bool)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			ActivitySource: activitySource ?? new(),
			ClassName: className ?? new(),
			DefaultToTags: defaultToTags ?? new(true),
			BaggageAndTagPrefix: baggageAndTagPrefix ?? new(),
			IncludeActivitySourcePrefix: includeActivitySourcePrefix ?? new(true),
			LowercaseBaggageAndTagKeys: lowercaseBaggageAndTagKeys ?? new(true)
		);
	}

	static public ActivitySourceAttributeRecord? GetActivitySourceAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {

		AttributeStringValue? nameValue = null;
		AttributeValue<bool>? defaultToTags = null;
		AttributeStringValue? baggageAndTagPrefix = null;
		AttributeStringValue? baggageAndTagSeparator = null;
		AttributeValue<bool>? lowercaseBaggageAndTagKeys = null;

		if (!AttributeParser(attributeData,
		(name, value) => {
			if (name.Equals(nameof(ActivitySourceGenerationAttribute.Name), StringComparison.OrdinalIgnoreCase)) {
				nameValue = new((string)value);
			}
			else if (name.Equals(nameof(ActivitySourceGenerationAttribute.DefaultToTags), StringComparison.OrdinalIgnoreCase)) {
				defaultToTags = new((bool)value);
			}
			else if (name.Equals(nameof(ActivitySourceGenerationAttribute.BaggageAndTagPrefix), StringComparison.OrdinalIgnoreCase)) {
				baggageAndTagPrefix = new((string)value);
			}
			else if (name.Equals(nameof(ActivitySourceGenerationAttribute.BaggageAndTagSeparator), StringComparison.OrdinalIgnoreCase)) {
				baggageAndTagSeparator = new((string)value);
			}
			else if (name.Equals(nameof(ActivitySourceGenerationAttribute.LowercaseBaggageAndTagKeys), StringComparison.OrdinalIgnoreCase)) {
				lowercaseBaggageAndTagKeys = new((bool)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			Name: nameValue ?? new(),
			DefaultToTags: defaultToTags ?? new(),
			BaggageAndTagPrefix: baggageAndTagPrefix ?? new(),
			BaggageAndTagSeparator: baggageAndTagSeparator ?? new("."),
			LowercaseBaggageAndTagKeys: lowercaseBaggageAndTagKeys ?? new(true)
		);
	}

	static public ActivityGenAttributeRecord? GetActivityGenAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {

		AttributeStringValue? nameValue = null;
		AttributeValue<ActivityGeneratedKind>? kind = null;
		AttributeValue<bool>? createOnly = null;

		if (!AttributeParser(attributeData,
		(name, value) => {
			if (name.Equals(nameof(ActivityTargetAttribute.Name), StringComparison.OrdinalIgnoreCase)) {
				nameValue = new((string)value);
			}
			else if (name.Equals(nameof(ActivityTargetAttribute.Kind), StringComparison.OrdinalIgnoreCase)) {
				kind = new((ActivityGeneratedKind)value);
			}
			else if (name.Equals(nameof(ActivityTargetAttribute.CreateOnly), StringComparison.OrdinalIgnoreCase)) {
				createOnly = new((bool)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			Name: nameValue ?? new(),
			Kind: kind ?? new(ActivityGeneratedKind.Internal),
			CreateOnly: createOnly ?? new()
		);
	}

	static public ActivityEventAttributeRecord? GetActivityEventAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {

		AttributeStringValue? nameValue = null;

		if (!AttributeParser(attributeData,
		(name, value) => {
			if (name.Equals(nameof(EventTargetAttribute.Name), StringComparison.OrdinalIgnoreCase)) {
				nameValue = new((string)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			Name: nameValue ?? new()
		);
	}
}
