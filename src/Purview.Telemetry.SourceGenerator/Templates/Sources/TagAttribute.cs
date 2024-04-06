﻿namespace Purview.Telemetry;

/// <summary>
/// Marker attribute to specify that a parameter should be included as a tag.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Parameter, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class TagAttribute : System.Attribute {
	/// <summary>
	/// Creates a new instance of a <see cref="TagAttribute"/>.
	/// </summary>
	public TagAttribute() {
	}

	/// <summary>
	/// Creates a new instance of a <see cref="TagAttribute"/> and specifies the
	/// <see cref="SkipOnNullOrEmpty"/> property.
	/// </summary>
	/// <param name="skipOnNullOrEmpty">Specifies the <see cref="SkipOnNullOrEmpty" />.</param>
	public TagAttribute(bool skipOnNullOrEmpty) {
		SkipOnNullOrEmpty = skipOnNullOrEmpty;
	}

	/// <summary>
	/// Creates a new instance of a <see cref="TagAttribute"/> and specifies the
	/// <see cref="Name" /> property, and optionally the
	/// <see cref="SkipOnNullOrEmpty"/> property.
	/// </summary>
	/// <param name="name">Specifies the key/ name of the tag.</param>
	/// <param name="skipOnNullOrEmpty">Optionally specifies the <see cref="SkipOnNullOrEmpty" />.</param>
	public TagAttribute(string? name, bool skipOnNullOrEmpty = false) {
		Name = name;
		SkipOnNullOrEmpty = skipOnNullOrEmpty;
	}

	/// <summary>
	/// Optionally specifies the key/ name of the tag. If one is not specified,
	/// the of the parameter is used.
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	/// Determines if the tag is skipped if it equals it's default value.
	/// Defaults to false.
	/// </summary>
	public bool SkipOnNullOrEmpty { get; set; } = false;
}
