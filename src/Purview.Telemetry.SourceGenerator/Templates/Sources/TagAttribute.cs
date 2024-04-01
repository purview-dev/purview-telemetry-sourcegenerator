namespace Purview.Telemetry;

[System.AttributeUsage(System.AttributeTargets.Parameter, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class TagAttribute : System.Attribute {
	public TagAttribute() {
	}

	public TagAttribute(bool skipOnNullOrEmpty) {
		SkipOnNullOrEmpty = skipOnNullOrEmpty;
	}

	public TagAttribute(string? name, bool skipOnNullOrEmpty = false) {
		Name = name;
		SkipOnNullOrEmpty = skipOnNullOrEmpty;
	}

	public string? Name { get; set; }

	public bool SkipOnNullOrEmpty { get; set; } = false;
}
