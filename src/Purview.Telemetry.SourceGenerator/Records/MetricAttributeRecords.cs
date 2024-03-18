namespace Purview.Telemetry.SourceGenerator.Records;

record InstrumentAttributeRecord(
	AttributeStringValue Name,
	AttributeStringValue? Unit,
	AttributeStringValue? Description,
	AttributeValue<bool>? AutoIncrement,
	AttributeValue<bool>? ThrowOnAlreadyInitialized,
	InstrumentTypes InstrumentType
) {
	public bool IsAutoIncrement => AutoIncrement?.Value ?? false;

	public bool IsObservable => InstrumentType is InstrumentTypes.ObservableCounter or InstrumentTypes.ObservableGauge or InstrumentTypes.ObservableUpDownCounter;
}

record MeterAssemblyAttributeRecord(
	AttributeStringValue InstrumentPrefix,
	AttributeStringValue InstrumentSeparator,
	AttributeValue<bool> LowercaseInstrumentName,
	AttributeValue<bool> LowercaseTagKeys
);

record MeterTargetAttributeRecord(
	AttributeStringValue Name,
	AttributeStringValue ClassName,
	AttributeStringValue InstrumentPrefix,
	AttributeValue<bool> IncludeAssemblyInstrumentPrefix,
	AttributeValue<bool> LowercaseInstrumentName,
	AttributeValue<bool> LowercaseTagKeys
);
