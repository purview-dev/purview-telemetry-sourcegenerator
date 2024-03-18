using Microsoft.CodeAnalysis;
using Purview.Telemetry.Metrics;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Helpers;

partial class SharedHelpers {
	static public MeterAssemblyAttributeRecord? GetMeterAssemblyAttribute(SemanticModel semanticModel, IGenerationLogger? logger, CancellationToken token) {
		token.ThrowIfCancellationRequested();

		if (!Utilities.TryContainsAttribute(semanticModel.Compilation.Assembly, Constants.Metrics.MeterGenerationAttribute, token, out var attributeData))
			return null;

		return GetMeterAssemblyAttribute(attributeData!, semanticModel, logger, token);
	}

	static public MeterTargetAttributeRecord? GetMeterTargetAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {

		AttributeStringValue? nameValue = null;
		AttributeStringValue? className = null;
		AttributeStringValue? instrumentPrefix = null;
		AttributeValue<bool>? includeAssemblyInstrumentPrefix = null;
		AttributeValue<bool>? lowercaseInstrumentName = null;
		AttributeValue<bool>? lowercaseTagKeys = null;

		if (!AttributeParser(attributeData,
		(name, value) => {
			if (name.Equals(nameof(MeterTargetAttribute.Name), StringComparison.OrdinalIgnoreCase)) {
				nameValue = new((string)value);
			}
			else if (name.Equals(nameof(MeterTargetAttribute.ClassName), StringComparison.OrdinalIgnoreCase)) {
				className = new((string)value);
			}
			else if (name.Equals(nameof(MeterTargetAttribute.InstrumentPrefix), StringComparison.OrdinalIgnoreCase)) {
				instrumentPrefix = new((string)value);
			}
			else if (name.Equals(nameof(MeterTargetAttribute.IncludeAssemblyInstrumentPrefix), StringComparison.OrdinalIgnoreCase)) {
				includeAssemblyInstrumentPrefix = new((bool)value);
			}
			else if (name.Equals(nameof(MeterTargetAttribute.LowercaseInstrumentName), StringComparison.OrdinalIgnoreCase)) {
				lowercaseInstrumentName = new((bool)value);
			}
			else if (name.Equals(nameof(MeterTargetAttribute.LowercaseTagKeys), StringComparison.OrdinalIgnoreCase)) {
				lowercaseTagKeys = new((bool)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			Name: nameValue ?? new(),
			ClassName: className ?? new(),
			InstrumentPrefix: instrumentPrefix ?? new(),
			IncludeAssemblyInstrumentPrefix: includeAssemblyInstrumentPrefix ?? new(true),
			LowercaseInstrumentName: lowercaseInstrumentName ?? new(true),
			LowercaseTagKeys: lowercaseTagKeys ?? new(true)
		);
	}

	static public MeterAssemblyAttributeRecord? GetMeterAssemblyAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {

		AttributeStringValue? instrumentPrefix = null;
		AttributeStringValue? instrumentSeparator = null;
		AttributeValue<bool>? lowercaseInstrumentName = null;
		AttributeValue<bool>? lowercaseTagKeys = null;

		if (!AttributeParser(attributeData,
		(name, value) => {
			if (name.Equals(nameof(MeterGenerationAttribute.InstrumentPrefix), StringComparison.OrdinalIgnoreCase)) {
				instrumentPrefix = new((string)value);
			}
			else if (name.Equals(nameof(MeterGenerationAttribute.InstrumentSeparator), StringComparison.OrdinalIgnoreCase)) {
				instrumentSeparator = new((string)value);
			}
			else if (name.Equals(nameof(MeterGenerationAttribute.LowercaseInstrumentName), StringComparison.OrdinalIgnoreCase)) {
				lowercaseInstrumentName = new((bool)value);
			}
			else if (name.Equals(nameof(MeterGenerationAttribute.LowercaseTagKeys), StringComparison.OrdinalIgnoreCase)) {
				lowercaseTagKeys = new((bool)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			InstrumentPrefix: instrumentPrefix ?? new(),
			InstrumentSeparator: instrumentSeparator ?? new(),
			LowercaseInstrumentName: lowercaseInstrumentName ?? new(),
			LowercaseTagKeys: lowercaseTagKeys ?? new()
		);
	}

	static public InstrumentAttributeRecord? GetInstrumentAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {

		if (attributeData.AttributeClass == null) {
			logger?.Error($"Unable to find AttributeClass for {attributeData}.");
			return null;
		}

		AttributeStringValue? nameValue = null;
		AttributeStringValue? unit = null;
		AttributeStringValue? description = null;
		AttributeValue<bool>? autoIncrement = null;
		AttributeValue<bool>? throwOnAlreadyInitialized = null;

		if (!AttributeParser(attributeData,
		(name, value) => {
			if (name.Equals(nameof(InstrumentAttributeBase.Name), StringComparison.OrdinalIgnoreCase)) {
				nameValue = new((string)value);
			}
			else if (name.Equals(nameof(InstrumentAttributeBase.Unit), StringComparison.OrdinalIgnoreCase)) {
				unit = new((string)value);
			}
			else if (name.Equals(nameof(InstrumentAttributeBase.Description), StringComparison.OrdinalIgnoreCase)) {
				description = new((string)value);
			}
			else if (name.Equals(nameof(CounterTargetAttribute.AutoIncrement), StringComparison.OrdinalIgnoreCase)) {
				autoIncrement = new((bool)value);
			}
			else if (name.Equals(nameof(ObservableCounterTargetAttribute.ThrowOnAlreadyInitialized), StringComparison.OrdinalIgnoreCase)) {
				throwOnAlreadyInitialized = new((bool)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		InstrumentTypes instrumentType;
		if (Constants.Metrics.CounterTargetAttribute.Equals(attributeData.AttributeClass)) {
			instrumentType = InstrumentTypes.Counter;
		}
		else if (Constants.Metrics.HistogramTargetAttribute.Equals(attributeData.AttributeClass)) {
			instrumentType = InstrumentTypes.Histogram;
		}
		else if (Constants.Metrics.UpDownCounterTargetAttribute.Equals(attributeData.AttributeClass)) {
			instrumentType = InstrumentTypes.UpDownCounter;
		}
		else if (Constants.Metrics.ObservableCounterTargetAttribute.Equals(attributeData.AttributeClass)) {
			instrumentType = InstrumentTypes.ObservableCounter;
		}
		else if (Constants.Metrics.ObservableUpDownCounterTargetAttribute.Equals(attributeData.AttributeClass)) {
			instrumentType = InstrumentTypes.ObservableUpDownCounter;
		}
		else if (Constants.Metrics.ObservableGaugeTargetAttribute.Equals(attributeData.AttributeClass)) {
			instrumentType = InstrumentTypes.ObservableGauge;
		}
		else {
			logger?.Error($"Unknown instrument type {attributeData.AttributeClass}.");

			return null;
		}

		return new(
			Name: nameValue ?? new(),
			Unit: unit ?? new(),
			Description: description ?? new(),
			AutoIncrement: autoIncrement ?? new(),
			ThrowOnAlreadyInitialized: throwOnAlreadyInitialized ?? new(),
			InstrumentType: instrumentType
		);
	}

	static public bool IsValidMeasurementValueType(ITypeSymbol type) =>
		Array.FindIndex(Constants.Metrics.ValidMeasurementKeywordTypes, m => m == type.ToDisplayString()) > -1
			|| Array.FindIndex(Constants.Metrics.ValidMeasurementTypes, m => m.Equals(type)) > -1;
}
