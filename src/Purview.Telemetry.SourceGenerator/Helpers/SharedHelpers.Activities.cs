﻿using Microsoft.CodeAnalysis;
using Purview.Telemetry.Activities;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Helpers;
partial class SharedHelpers {
	static public ActivitySourceGenerationAttributeRecord? GetActivitySourceGenerationAttribute(SemanticModel semanticModel, IGenerationLogger? logger, CancellationToken token) {
		token.ThrowIfCancellationRequested();

		if (!Utilities.TryContainsAttribute(semanticModel.Compilation.Assembly, Constants.Activities.ActivitySourceGenerationAttribute, token, out var attributeData))
			return null;

		return GetActivitySourceGenerationAttribute(attributeData!, semanticModel, logger, token);
	}

	static public ActivitySourceAttributeRecord? GetActivitySourceAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {

		AttributeStringValue? nameValue = null;
		AttributeValue<bool>? defaultToTags = null;
		AttributeStringValue? baggageAndTagPrefix = null;
		AttributeValue<bool>? includeActivitySourcePrefix = null;
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
			else if (name.Equals(nameof(ActivitySourceAttribute.IncludeActivitySourcePrefix), StringComparison.OrdinalIgnoreCase)) {
				includeActivitySourcePrefix = new((bool)value);
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
			DefaultToTags: defaultToTags ?? new(true),
			BaggageAndTagPrefix: baggageAndTagPrefix ?? new(),
			IncludeActivitySourcePrefix: includeActivitySourcePrefix ?? new(true),
			LowercaseBaggageAndTagKeys: lowercaseBaggageAndTagKeys ?? new(true)
		);
	}

	static public ActivitySourceGenerationAttributeRecord? GetActivitySourceGenerationAttribute(
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

	static public ActivityAttributeRecord? GetActivityGenAttribute(
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
			Kind: kind ?? new(ActivityGeneratedKind.Internal),
			CreateOnly: createOnly ?? new()
		);
	}

	static public EventAttributeRecord? GetActivityEventAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {

		AttributeStringValue? nameValue = null;
		AttributeValue<bool>? useRecordExceptionRules = null;
		AttributeValue<bool>? recordExceptionEscape = null;

		if (!AttributeParser(attributeData,
		(name, value) => {
			if (name.Equals(nameof(EventAttribute.Name), StringComparison.OrdinalIgnoreCase)) {
				nameValue = new((string)value);
			}
			else if (name.Equals(nameof(EventAttribute.UseRecordExceptionRules), StringComparison.OrdinalIgnoreCase)) {
				useRecordExceptionRules = new((bool)value);
			}
			else if (name.Equals(nameof(EventAttribute.RecordExceptionEscape), StringComparison.OrdinalIgnoreCase)) {
				recordExceptionEscape = new((bool)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			Name: nameValue ?? new(),
			UseRecordExceptionRules: useRecordExceptionRules ?? new(),
			RecordExceptionEscape: recordExceptionEscape ?? new()
		);
	}

	static public bool IsActivityMethod(IMethodSymbol method, CancellationToken token) {
		return Utilities.ContainsAttribute(method, Constants.Activities.ActivityAttribute, token)
			|| Utilities.ContainsAttribute(method, Constants.Activities.EventAttribute, token)
			|| Utilities.ContainsAttribute(method, Constants.Activities.ContextAttribute, token); ;
	}
}
