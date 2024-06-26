﻿using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Helpers;

partial class SharedHelpers
{
	public static MeterGenerationAttributeRecord? GetMeterGenerationAttribute(SemanticModel semanticModel, IGenerationLogger? logger, CancellationToken token)
	{
		token.ThrowIfCancellationRequested();

		return Utilities.TryContainsAttribute(semanticModel.Compilation.Assembly, Constants.Metrics.MeterGenerationAttribute, token, out var attributeData)
			? GetMeterGenerationAttribute(attributeData!, semanticModel, logger, token)
			: null;
	}

	public static MeterAttributeRecord? GetMeterAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token)
	{
		AttributeStringValue? nameValue = null;
		AttributeStringValue? instrumentPrefix = null;
		AttributeValue<bool>? includeAssemblyInstrumentPrefix = null;
		AttributeValue<bool>? lowercaseInstrumentName = null;
		AttributeValue<bool>? lowercaseTagKeys = null;

		if (!AttributeParser(attributeData,
		(name, value) =>
		{
			if (name.Equals("Name", StringComparison.OrdinalIgnoreCase))
				nameValue = new((string)value);
			else if (name.Equals("InstrumentPrefix", StringComparison.OrdinalIgnoreCase))
				instrumentPrefix = new((string)value);
			else if (name.Equals("IncludeAssemblyInstrumentPrefix", StringComparison.OrdinalIgnoreCase))
				includeAssemblyInstrumentPrefix = new((bool)value);
			else if (name.Equals("LowercaseInstrumentName", StringComparison.OrdinalIgnoreCase))
				lowercaseInstrumentName = new((bool)value);
			else if (name.Equals("LowercaseTagKeys", StringComparison.OrdinalIgnoreCase))
				lowercaseTagKeys = new((bool)value);
		}, semanticModel, logger, token))
		{
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			Name: nameValue ?? new(),
			InstrumentPrefix: instrumentPrefix ?? new(),
			IncludeAssemblyInstrumentPrefix: includeAssemblyInstrumentPrefix ?? new(true),
			LowercaseInstrumentName: lowercaseInstrumentName ?? new(Constants.Metrics.LowercaseInstrumentNameDefault),
			LowercaseTagKeys: lowercaseTagKeys ?? new(Constants.Metrics.LowercaseTagKeysDefault)
		);
	}

	public static MeterGenerationAttributeRecord? GetMeterGenerationAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token)
	{
		AttributeStringValue? instrumentPrefix = null;
		AttributeStringValue? instrumentSeparator = null;
		AttributeValue<bool>? lowercaseInstrumentName = null;
		AttributeValue<bool>? lowercaseTagKeys = null;

		if (!AttributeParser(attributeData,
		(name, value) =>
		{
			if (name.Equals("InstrumentPrefix", StringComparison.OrdinalIgnoreCase))
				instrumentPrefix = new((string)value);
			else if (name.Equals("InstrumentSeparator", StringComparison.OrdinalIgnoreCase))
				instrumentSeparator = new((string)value);
			else if (name.Equals("LowercaseInstrumentName", StringComparison.OrdinalIgnoreCase))
				lowercaseInstrumentName = new((bool)value);
			else if (name.Equals("LowercaseTagKeys", StringComparison.OrdinalIgnoreCase))
				lowercaseTagKeys = new((bool)value);
		}, semanticModel, logger, token))
		{
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			InstrumentPrefix: instrumentPrefix ?? new(),
			InstrumentSeparator: instrumentSeparator ?? new(Constants.Metrics.InstrumentSeparatorDefault),
			LowercaseInstrumentName: lowercaseInstrumentName ?? new(Constants.Metrics.LowercaseInstrumentNameDefault),
			LowercaseTagKeys: lowercaseTagKeys ?? new(Constants.Metrics.LowercaseTagKeysDefault)
		);
	}

	public static InstrumentAttributeRecord? GetInstrumentAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token)
	{
		if (attributeData.AttributeClass == null)
		{
			logger?.Error($"Unable to find AttributeClass for {attributeData}.");
			return null;
		}

		AttributeStringValue? nameValue = null;
		AttributeStringValue? unit = null;
		AttributeStringValue? description = null;
		AttributeValue<bool>? autoIncrement = null;
		AttributeValue<bool>? throwOnAlreadyInitialized = null;

		if (!AttributeParser(attributeData, (name, value) =>
		{
			if (name.Equals("Name", StringComparison.OrdinalIgnoreCase))
				nameValue = new((string)value);
			else if (name.Equals("Unit", StringComparison.OrdinalIgnoreCase))
				unit = new((string)value);
			else if (name.Equals("Description", StringComparison.OrdinalIgnoreCase))
				description = new((string)value);
			else if (name.Equals("AutoIncrement", StringComparison.OrdinalIgnoreCase))
				autoIncrement = new((bool)value);
			else if (name.Equals("ThrowOnAlreadyInitialized", StringComparison.OrdinalIgnoreCase))
				throwOnAlreadyInitialized = new((bool)value);
		}, semanticModel, logger, token))
		{
			// Failed to parse correctly, so null it out.
			return null;
		}

		InstrumentTypes instrumentType;
		var isAutoCounter = Constants.Metrics.AutoCounterAttribute.Equals(attributeData.AttributeClass);
		if (isAutoCounter ||
			Constants.Metrics.CounterAttribute.Equals(attributeData.AttributeClass))
		{
			instrumentType = InstrumentTypes.Counter;

			if (isAutoCounter)
				autoIncrement = new(true);
		}
		else if (Constants.Metrics.HistogramAttribute.Equals(attributeData.AttributeClass))
			instrumentType = InstrumentTypes.Histogram;
		else if (Constants.Metrics.UpDownCounterAttribute.Equals(attributeData.AttributeClass))
			instrumentType = InstrumentTypes.UpDownCounter;
		else if (Constants.Metrics.ObservableCounterAttribute.Equals(attributeData.AttributeClass))
			instrumentType = InstrumentTypes.ObservableCounter;
		else if (Constants.Metrics.ObservableUpDownCounterAttribute.Equals(attributeData.AttributeClass))
			instrumentType = InstrumentTypes.ObservableUpDownCounter;
		else if (Constants.Metrics.ObservableGaugeAttribute.Equals(attributeData.AttributeClass))
			instrumentType = InstrumentTypes.ObservableGauge;
		else
		{
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

	public static bool IsValidMeasurementValueType(ITypeSymbol type) =>
		Array.FindIndex(Constants.Metrics.ValidMeasurementKeywordTypes, m => m == type.ToDisplayString()) > -1
		|| Array.FindIndex(Constants.Metrics.ValidMeasurementTypes, m => m.Equals(type)) > -1;

	public static bool TryGetInstrumentAttribute(IMethodSymbol method, CancellationToken token, out AttributeData? attributeData)
	{
		attributeData = null;
		foreach (var instrumentAttribute in Constants.Metrics.ValidInstrumentAttributes)
		{
			if (Utilities.TryContainsAttribute(method, instrumentAttribute, token, out attributeData))
				return true;
		}

		return false;
	}

	public static bool IsInstrument(IMethodSymbol method, CancellationToken token)
	{
		foreach (var instrumentAttribute in Constants.Metrics.ValidInstrumentAttributes)
		{
			if (Utilities.TryContainsAttribute(method, instrumentAttribute, token, out _))
				return true;
		}

		return false;
	}
}
