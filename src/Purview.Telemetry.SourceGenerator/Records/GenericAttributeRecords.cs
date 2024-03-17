namespace Purview.Telemetry.SourceGenerator.Records;

record AttributeValue<T>
	where T : struct {
	public AttributeValue(T? value) {
		Value = value;
		IsSet = true;
	}

	public AttributeValue() {
		IsSet = false;
	}

	public T? Or(T value) {
		if (IsSet) {
			return Value;
		}

		return value;
	}

	public T? Value { get; }

	public bool IsSet { get; }

	static public implicit operator AttributeValue<T>(string? value)
		=> new(value);

	static public implicit operator T?(AttributeValue<T> value)
		=> value.Value;
}

record AttributeStringValue {
	public AttributeStringValue(string? value) {
		Value = value;
		IsSet = true;
	}

	public AttributeStringValue() {
		IsSet = false;
	}

	public string? Or(string value) {
		if (IsSet) {
			return Value;
		}

		return value;
	}

	public string? Value { get; }

	public bool IsSet { get; }

	static public implicit operator AttributeStringValue(string? value)
		=> new(value);

	static public implicit operator string?(AttributeStringValue value)
		=> value.Value;
}

record TagOrBaggageAttributeRecord(
	AttributeStringValue Name,
	AttributeValue<bool> SkipOnNullOrEmpty
);

record TelemetryGenerationAttributeRecord(
	AttributeValue<bool> GenerateDependencyExtension,
	AttributeStringValue ClassNameTemplate
);
