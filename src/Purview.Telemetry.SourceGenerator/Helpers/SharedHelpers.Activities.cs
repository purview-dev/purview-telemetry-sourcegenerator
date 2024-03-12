﻿using Microsoft.CodeAnalysis;
using Purview.Telemetry.Activities;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Helpers;
partial class SharedHelpers {
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
			if (name.Equals(nameof(ActivityTargetAttribute.ActivitySource), StringComparison.OrdinalIgnoreCase)) {
				activitySource = new((string)value);
			}
			else if (name.Equals(nameof(ActivityTargetAttribute.ClassName), StringComparison.OrdinalIgnoreCase)) {
				className = new((string)value);
			}
			else if (name.Equals(nameof(ActivityTargetAttribute.DefaultToTags), StringComparison.OrdinalIgnoreCase)) {
				defaultToTags = new((bool)value);
			}
			else if (name.Equals(nameof(ActivityTargetAttribute.BaggageAndTagPrefix), StringComparison.OrdinalIgnoreCase)) {
				baggageAndTagPrefix = new((string)value);
			}
			else if (name.Equals(nameof(ActivityTargetAttribute.IncludeActivitySourcePrefix), StringComparison.OrdinalIgnoreCase)) {
				includeActivitySourcePrefix = new((bool)value);
			}
			else if (name.Equals(nameof(ActivityTargetAttribute.LowercaseBaggageAndTagKeys), StringComparison.OrdinalIgnoreCase)) {
				lowercaseBaggageAndTagKeys = new((bool)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			ActivitySource: activitySource ?? new(),
			ClassName: className ?? new(),
			DefaultToTags: defaultToTags ?? new(),
			BaggageAndTagPrefix: baggageAndTagPrefix ?? new(),
			IncludeActivitySourcePrefix: includeActivitySourcePrefix ?? new(),
			LowercaseBaggageAndTagKeys: lowercaseBaggageAndTagKeys ?? new()
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
			if (name.Equals(nameof(ActivitySourceAttribute.Name), StringComparison.OrdinalIgnoreCase)) {
				nameValue = new((string)value);
			}
			else if (name.Equals(nameof(ActivitySourceAttribute.DefaultToTags), StringComparison.OrdinalIgnoreCase)) {
				defaultToTags = new((bool)value);
			}
			else if (name.Equals(nameof(ActivitySourceAttribute.BaggageAndTagPrefix), StringComparison.OrdinalIgnoreCase)) {
				baggageAndTagPrefix = new((string)value);
			}
			else if (name.Equals(nameof(ActivitySourceAttribute.BaggageAndTagSeparator), StringComparison.OrdinalIgnoreCase)) {
				baggageAndTagSeparator = new((string)value);
			}
			else if (name.Equals(nameof(ActivitySourceAttribute.LowercaseBaggageAndTagKeys), StringComparison.OrdinalIgnoreCase)) {
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
			BaggageAndTagSeparator: baggageAndTagSeparator ?? new(),
			LowercaseBaggageAndTagKeys: lowercaseBaggageAndTagKeys ?? new()
		);
	}

	static public TagOrBaggageAttributeRecord? GetTagOrBaggageAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {

		AttributeStringValue? nameValue = null;
		AttributeValue<bool>? skipOnNullOrEmpty = null;

		if (!AttributeParser(attributeData,
		(name, value) => {
			if (name.Equals(nameof(TagAttribute.Name), StringComparison.OrdinalIgnoreCase)) {
				nameValue = new((string)value);
			}
			else if (name.Equals(nameof(TagAttribute.SkipOnNullOrEmpty), StringComparison.OrdinalIgnoreCase)) {
				skipOnNullOrEmpty = new((bool)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			Name: nameValue ?? new(),
			SkipOnNullOrEmpty: skipOnNullOrEmpty ?? new()
		);
	}

	static public ActivityAttributeRecord? GetActivityAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {

		AttributeStringValue? nameValue = null;
		AttributeValue<ActivityGeneratedKind>? kind = null;
		AttributeValue<bool>? createOnly = null;

		if (!AttributeParser(attributeData,
		(name, value) => {
			if (name.Equals(nameof(ActivityAttribute.Name), StringComparison.OrdinalIgnoreCase)) {
				nameValue = new((string)value);
			}
			else if (name.Equals(nameof(ActivityAttribute.Kind), StringComparison.OrdinalIgnoreCase)) {
				kind = new((ActivityGeneratedKind)value);
			}
			else if (name.Equals(nameof(ActivityAttribute.CreateOnly), StringComparison.OrdinalIgnoreCase)) {
				createOnly = new((bool)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			Name: nameValue ?? new(),
			Kind: kind ?? new(),
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
			if (name.Equals(nameof(ActivityEventAttribute.Name), StringComparison.OrdinalIgnoreCase)) {
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
