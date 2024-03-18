using Microsoft.CodeAnalysis;
using Purview.Telemetry.Logging;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Helpers;

partial class SharedHelpers {
	static public LogEntryAttributeRecord? GetLogEntryAttribute(
		ISymbol symbol,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token
	) {
		if (!Utilities.TryContainsAttribute(symbol, Constants.Logging.LogTargetAttribute, token, out var attributeData)) {
			return null;
		}

		AttributeValue<LogGeneratedLevel>? level = null;
		AttributeStringValue? messageTemplate = null;
		AttributeValue<int>? eventId = null;
		AttributeStringValue? nameValue = null;

		if (!AttributeParser(attributeData!,
		(name, value) => {
			if (name.Equals(nameof(LogTargetAttribute.Level), StringComparison.OrdinalIgnoreCase)) {
				level = new((LogGeneratedLevel)value);
			}
			else if (name.Equals(nameof(LogTargetAttribute.MessageTemplate), StringComparison.OrdinalIgnoreCase)) {
				messageTemplate = new((string)value);
			}
			else if (name.Equals(nameof(LogTargetAttribute.EventId), StringComparison.OrdinalIgnoreCase)) {
				eventId = new((int)value);
			}
			else if (name.Equals(nameof(LogTargetAttribute.EntryName), StringComparison.OrdinalIgnoreCase)) {
				nameValue = new((string)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			Level: level ?? new(),
			MessageTemplate: messageTemplate ?? new(),
			EventId: eventId ?? new(),
			Name: nameValue ?? new()
		);
	}

	static public LoggerTargetAttributeRecord? GetLoggerTargetAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {

		AttributeValue<LogGeneratedLevel>? defaultLevel = null;
		AttributeStringValue? className = null;
		AttributeStringValue? customPrefix = null;
		AttributeValue<LogPrefixType>? prefixType = null;

		if (!AttributeParser(attributeData,
		(name, value) => {
			if (name.Equals(nameof(LoggerTargetAttribute.DefaultLevel), StringComparison.OrdinalIgnoreCase)) {
				defaultLevel = new((LogGeneratedLevel)value);
			}
			else if (name.Equals(nameof(LoggerTargetAttribute.ClassName), StringComparison.OrdinalIgnoreCase)) {
				className = new((string)value);
			}
			else if (name.Equals(nameof(LoggerTargetAttribute.CustomPrefix), StringComparison.OrdinalIgnoreCase)) {
				customPrefix = new((string)value);
			}
			else if (name.Equals(nameof(LoggerTargetAttribute.PrefixType), StringComparison.OrdinalIgnoreCase)) {
				prefixType = new((LogPrefixType)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			DefaultLevel: defaultLevel ?? new(),
			ClassName: className ?? new(),
			CustomPrefix: customPrefix ?? new(),
			PrefixType: prefixType ?? new()
		);
	}

	static public LoggerDefaultsAttributeRecord? GetLoggerDefaultsAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {

		AttributeValue<LogGeneratedLevel>? defaultLevel = null;

		if (!AttributeParser(attributeData,
		(name, value) => {
			if (name.Equals(nameof(LoggerGenerationAttribute.DefaultLevel), StringComparison.OrdinalIgnoreCase)) {
				defaultLevel = new((LogGeneratedLevel)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			DefaultLevel: defaultLevel ?? new()
		);
	}
}
