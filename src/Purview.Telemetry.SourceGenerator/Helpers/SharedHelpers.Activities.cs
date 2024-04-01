using Microsoft.CodeAnalysis;
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
			if (name.Equals("Name", StringComparison.OrdinalIgnoreCase)) {
				nameValue = new((string)value);
			}
			else if (name.Equals("DefaultToTags", StringComparison.OrdinalIgnoreCase)) {
				defaultToTags = new((bool)value);
			}
			else if (name.Equals("BaggageAndTagPrefix", StringComparison.OrdinalIgnoreCase)) {
				baggageAndTagPrefix = new((string)value);
			}
			else if (name.Equals("IncludeActivitySourcePrefix", StringComparison.OrdinalIgnoreCase)) {
				includeActivitySourcePrefix = new((bool)value);
			}
			else if (name.Equals("LowercaseBaggageAndTagKeys", StringComparison.OrdinalIgnoreCase)) {
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
			if (name.Equals("Name", StringComparison.OrdinalIgnoreCase)) {
				nameValue = new((string)value);
			}
			else if (name.Equals("DefaultToTags", StringComparison.OrdinalIgnoreCase)) {
				defaultToTags = new((bool)value);
			}
			else if (name.Equals("BaggageAndTagPrefix", StringComparison.OrdinalIgnoreCase)) {
				baggageAndTagPrefix = new((string)value);
			}
			else if (name.Equals("BaggageAndTagSeparator", StringComparison.OrdinalIgnoreCase)) {
				baggageAndTagSeparator = new((string)value);
			}
			else if (name.Equals("LowercaseBaggageAndTagKeys", StringComparison.OrdinalIgnoreCase)) {
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
		AttributeValue<int>? kind = null;
		AttributeValue<bool>? createOnly = null;

		if (!AttributeParser(attributeData,
		(name, value) => {
			if (name.Equals("Name", StringComparison.OrdinalIgnoreCase)) {
				nameValue = new((string)value);
			}
			else if (name.Equals("Kind", StringComparison.OrdinalIgnoreCase)) {
				kind = new((int)value);
			}
			else if (name.Equals("CreateOnly", StringComparison.OrdinalIgnoreCase)) {
				createOnly = new((bool)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			Name: nameValue ?? new(),
			Kind: kind ?? new(Constants.Activities.DefaultActivityKind),
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
			if (name.Equals("Name", StringComparison.OrdinalIgnoreCase)) {
				nameValue = new((string)value);
			}
			else if (name.Equals("UseRecordExceptionRules", StringComparison.OrdinalIgnoreCase)) {
				useRecordExceptionRules = new((bool)value);
			}
			else if (name.Equals("RecordExceptionAsEscaped", StringComparison.OrdinalIgnoreCase)) {
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
