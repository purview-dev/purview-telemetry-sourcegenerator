﻿namespace Purview.Telemetry.Activities;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class EventAttribute : Attribute {

	public EventAttribute() {
	}

	public EventAttribute(string name) {
		Name = name;
	}

	/// <summary>
	/// Optional. Gets/ sets the name of the event.
	/// </summary>
	public string? Name { get; set; }
}