namespace Purview.Telemetry;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class TagAttribute : Attribute {
	public TagAttribute() {
	}

	public TagAttribute(bool skipOnNullOrEmpty) {
		SkipOnNullOrEmpty = skipOnNullOrEmpty;
	}

	public TagAttribute(string? name, bool skipOnNullOrEmpty = Constants.Shared.SkipOnNullOrEmptyDefault) {
		Name = name;
		SkipOnNullOrEmpty = skipOnNullOrEmpty;
	}

	public string? Name { get; set; }

	public bool SkipOnNullOrEmpty { get; set; } = Constants.Shared.SkipOnNullOrEmptyDefault;
}
