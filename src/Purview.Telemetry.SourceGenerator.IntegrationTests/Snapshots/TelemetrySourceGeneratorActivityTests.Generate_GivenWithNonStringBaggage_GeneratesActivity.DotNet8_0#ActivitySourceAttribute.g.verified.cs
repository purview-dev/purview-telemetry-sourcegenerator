﻿//HintName: ActivitySourceAttribute.g.cs
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Purview.Telemetry.SourceGenerator
//     on {Scrubbed}.
//
//     Changes to this file may cause incorrect behaviour and will be lost
//     when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable 1591 // publicly visible type or member must be documented

#nullable enable

namespace Purview.Telemetry.Activities;

/// <summary>
/// Marker attribute required for <see cref="System.Diagnostics.Activity"/>
/// and <see cref="System.Diagnostics.ActivityEvent"/> generation.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Interface, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed class ActivitySourceAttribute : System.Attribute {
	/// <summary>
	/// Constructs a new <see cref="ActivitySourceAttribute"/>.
	/// </summary>
	public ActivitySourceAttribute() {
	}

	/// <summary>
	/// Constructs a new <see cref="ActivitySourceAttribute"/> specifying the <see cref="Name"/>.
	/// </summary>
	/// <param name="name">The <see cref="Name"/>.</param>
	public ActivitySourceAttribute(string name) {
		Name = name;
	}

	/// <summary>
	/// Sets the name for the generated <see cref="System.Diagnostics.ActivitySource.Name"/>,
	/// overriding the <see cref="ActivitySourceGenerationAttribute.Name"/>.
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	/// Specifies the default when inferring between
	/// <see cref="Purview.Telemetry.TagAttribute"/> or
	/// <see cref="Purview.Telemetry.Activities.BaggageAttribute"/>, unless
	/// explicitly marked.
	/// </summary>
	public bool DefaultToTags { get; set; } = true;

	/// <summary>
	/// Prefix used to when generating the tag or baggage name. Prepended
	/// before the <see cref="Purview.Telemetry.TagAttribute.Name"/> or
	/// <see cref="Purview.Telemetry.Activities.BaggageAttribute.Name"/>.
	/// </summary>
	public string? BaggageAndTagPrefix { get; set; }

	/// <summary>
	/// Determines if the <see cref="Name"/> (or <see cref="ActivitySourceGenerationAttribute.Name"/>)
	/// is used as a prefix, before the <see cref="BaggageAndTagPrefix"/>.
	/// </summary>
	public bool IncludeActivitySourcePrefix { get; set; } = true;

	/// <summary>
	/// Determines if the <see cref="Purview.Telemetry.TagAttribute.Name"/> or
	/// <see cref="Purview.Telemetry.Activities.BaggageAttribute.Name"/> (including
	/// any prefixes) are lowercased.
	/// </summary>
	public bool LowercaseBaggageAndTagKeys { get; set; } = true;
}
