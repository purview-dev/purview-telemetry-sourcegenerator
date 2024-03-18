﻿namespace Purview.Telemetry.Activities;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class ActivityTargetAttribute : Attribute {

	public ActivityTargetAttribute() {
	}

	public ActivityTargetAttribute(string name) {
		Name = name;
	}

	public ActivityTargetAttribute(ActivityGeneratedKind kind) {
		Kind = kind;
	}

	public ActivityTargetAttribute(string name, ActivityGeneratedKind kind, bool createOnly = false) {
		Name = name;
		Kind = kind;
		CreateOnly = createOnly;
	}

	/// <summary>
	/// Optional. Gets the name of the activity.
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	/// Optional. Gets the <see cref="ActivityGeneratedKind">kind</see> of the
	/// activity. Defaults to <see cref="ActivityGeneratedKind.Internal"/>.
	/// </summary>
	public ActivityGeneratedKind Kind { get; set; } = ActivityGeneratedKind.Internal;

	/// <summary>
	/// If true, the Activity is crated via ActivitySource.CreateActivity, meaning it is not started by default. Otherwise
	/// ActivitySource.StartActivity is used. The default is false.
	/// </summary>
	public bool CreateOnly { get; set; }
}
