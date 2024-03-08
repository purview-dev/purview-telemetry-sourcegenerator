namespace Purview.Telemetry.SourceGenerator.Targets;

record AttributeValue<T>
	where T : struct {
	public AttributeValue(T? value) {
		Value = value;
		IsSet = true;
	}

	public AttributeValue() {
		IsSet = false;
	}

	public T? Value { get; }

	public bool IsSet { get; }
}

record AttributeStringValue {
	public AttributeStringValue(string? value) {
		Value = value;
		IsSet = true;
	}

	public AttributeStringValue() {
		IsSet = false;
	}

	public string? Value { get; }

	public bool IsSet { get; }
}
