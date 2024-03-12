namespace Purview.Telemetry.Activities;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class BaggageAttribute : Attribute {
	public BaggageAttribute() {
	}

	public BaggageAttribute(bool skipOnNullOrEmpty) {
		SkipOnNullOrEmpty = skipOnNullOrEmpty;
	}

	public BaggageAttribute(string? name, bool skipOnNullOrEmpty) {
		Name = name;
		SkipOnNullOrEmpty = skipOnNullOrEmpty;
	}

	public string? Name { get; set; }

	public bool SkipOnNullOrEmpty { get; set; } = true;
}
