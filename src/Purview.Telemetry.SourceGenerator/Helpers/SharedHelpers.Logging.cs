using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Helpers;

partial class SharedHelpers
{
	public static LogAttributeRecord? GetLogAttribute(
		ISymbol symbol,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token
	)
	{
		if (!Utilities.TryContainsAttribute(symbol, [Constants.Logging.LogAttribute, .. Constants.Logging.SpecificLogAttributes], token, out var attributeData, out var matchingTemplate))
			return null;

		AttributeValue<int>? level = null;
		if (matchingTemplate != Constants.Logging.LogAttribute)
			level = new(Constants.Logging.SpecificLogAttributesToLevel[matchingTemplate!]);

		AttributeStringValue? messageTemplate = null;
		AttributeValue<int>? eventId = null;
		AttributeStringValue? nameValue = null;

		if (!AttributeParser(attributeData!,
		(name, value) =>
		{
			if (name.Equals(nameof(LogAttributeRecord.Level), StringComparison.OrdinalIgnoreCase))
				level = new((int)value);
			else if (name.Equals(nameof(LogAttributeRecord.MessageTemplate), StringComparison.OrdinalIgnoreCase))
				messageTemplate = new((string)value);
			else if (name.Equals(nameof(LogAttributeRecord.EventId), StringComparison.OrdinalIgnoreCase))
				eventId = new((int)value);
			else if (name.Equals(nameof(LogAttributeRecord.Name), StringComparison.OrdinalIgnoreCase))
				nameValue = new((string)value);
		}, semanticModel, logger, token))
		{
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			Level: level ?? new(Constants.Logging.DefaultLevel),
			MessageTemplate: messageTemplate ?? new(),
			EventId: eventId ?? new(),
			Name: nameValue ?? new()
		);
	}

	public static LoggerAttributeRecord? GetLoggerAttribute(
		ISymbol symbol,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token)
	{
		if (!Utilities.TryContainsAttribute(symbol, Constants.Logging.LoggerAttribute, token, out var attributeData))
			return null;

		AttributeValue<int>? defaultLevel = null;
		AttributeStringValue? customPrefix = null;
		AttributeValue<int>? prefixType = null;
		AttributeValue<bool>? disableMSLoggingTelemetryGeneration = null;

		if (!AttributeParser(attributeData!,
		(name, value) =>
		{
			if (name.Equals(nameof(LoggerAttributeRecord.DefaultLevel), StringComparison.OrdinalIgnoreCase))
				defaultLevel = new((int)value);
			else if (name.Equals(nameof(LoggerAttributeRecord.CustomPrefix), StringComparison.OrdinalIgnoreCase))
				customPrefix = new((string)value);
			else if (name.Equals(nameof(LoggerAttributeRecord.PrefixType), StringComparison.OrdinalIgnoreCase))
				prefixType = new((int)value);
			else if (name.Equals(nameof(LoggerAttributeRecord.DisableMSLoggingTelemetryGeneration), StringComparison.OrdinalIgnoreCase))
				disableMSLoggingTelemetryGeneration = new((bool)value);
		}, semanticModel, logger, token))
		{
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			DefaultLevel: defaultLevel ?? new(),
			CustomPrefix: customPrefix ?? new(),
			PrefixType: prefixType ?? new(),
			DisableMSLoggingTelemetryGeneration: disableMSLoggingTelemetryGeneration ?? new()
		);
	}

	public static LoggerGenerationAttributeRecord? GetLoggerGenerationAttribute(
		ISymbol symbol,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token)
	{
		if (!Utilities.TryContainsAttribute(symbol, Constants.Logging.LoggerGenerationAttribute, token, out var attributeData))
			return null;

		AttributeValue<int>? defaultLevel = null;
		AttributeValue<bool>? disableMSLoggingTelemetryGeneration = null;

		if (!AttributeParser(attributeData!,
		(name, value) =>
		{
			if (name.Equals(nameof(LoggerGenerationAttributeRecord.DefaultLevel), StringComparison.OrdinalIgnoreCase))
				defaultLevel = new((int)value);
			else if (name.Equals(nameof(LoggerGenerationAttributeRecord.DisableMSLoggingTelemetryGeneration), StringComparison.OrdinalIgnoreCase))
				disableMSLoggingTelemetryGeneration = new((bool)value);
		}, semanticModel, logger, token))
		{
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			DefaultLevel: defaultLevel ?? new(),
			DisableMSLoggingTelemetryGeneration: disableMSLoggingTelemetryGeneration ?? new()
		);
	}

	public static LogPropertiesAttributeRecord? GetLogPropertiesAttribute(
		ISymbol symbol,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token)
	{
		if (!Utilities.TryContainsAttribute(symbol, Constants.Logging.MicrosoftExtensions.LogPropertiesAttribute, token, out var attributeData))
			return null;

		AttributeValue<bool>? omitReferenceName = null;
		AttributeValue<bool>? skipNullProperties = null;
		AttributeValue<bool>? transitive = null;

		if (!AttributeParser(attributeData!,
		(name, value) =>
		{
			if (name.Equals(nameof(LogPropertiesAttributeRecord.OmitReferenceName), StringComparison.OrdinalIgnoreCase))
				omitReferenceName = new((bool)value);
			else if (name.Equals(nameof(LogPropertiesAttributeRecord.SkipNullProperties), StringComparison.OrdinalIgnoreCase))
				skipNullProperties = new((bool)value);
			else if (name.Equals(nameof(LogPropertiesAttributeRecord.Transitive), StringComparison.OrdinalIgnoreCase))
				transitive = new((bool)value);
		}, semanticModel, logger, token))
		{
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			OmitReferenceName: omitReferenceName ?? new(),
			SkipNullProperties: skipNullProperties ?? new(),
			Transitive: transitive ?? new()
		);
	}

	public static ExpandEnumerableAttributeRecord? GetExpandEnumerableAttribute(
		ISymbol symbol,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token)
	{
		if (!Utilities.TryContainsAttribute(symbol, Constants.Logging.ExpandEnumerableAttribute, token, out var attributeData))
			return null;

		AttributeValue<int>? maximumValueCount = null;

		if (!AttributeParser(attributeData!, (name, value) =>
		{
			if (name.Equals(nameof(ExpandEnumerableAttributeRecord.MaximumValueCount), StringComparison.OrdinalIgnoreCase))
				maximumValueCount = new((int)value);
		}, semanticModel, logger, token))
		{
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			MaximumValueCount: maximumValueCount ?? new(5)
		);
	}

	public static LoggerGenerationAttributeRecord? GetLoggerGenerationAttribute(SemanticModel semanticModel, IGenerationLogger? logger, CancellationToken token)
		=> GetLoggerGenerationAttribute(semanticModel.Compilation.Assembly, semanticModel, logger, token);

	public static bool IsLogMethod(IMethodSymbol method, CancellationToken token)
		=> Utilities.ContainsAttribute(method, [Constants.Logging.LogAttribute, .. Constants.Logging.SpecificLogAttributes], token);
}
