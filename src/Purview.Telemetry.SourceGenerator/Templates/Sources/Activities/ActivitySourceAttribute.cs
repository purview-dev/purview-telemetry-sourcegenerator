﻿namespace Purview.Telemetry.Activities;

/// <summary>
/// Marker attribute required for <see cref="global::System.Diagnostics.Activity"/>
/// and <see cref="global::System.Diagnostics.ActivityEvent"/> generation.
/// </summary>
{CodeGen}
[global::System.AttributeUsage(global::System.AttributeTargets.Interface, AllowMultiple = false)]
[global::System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed class ActivitySourceAttribute : global::System.Attribute
{
	/// <summary>
	/// Constructs a new <see cref="ActivitySourceAttribute"/>.
	/// </summary>
	public ActivitySourceAttribute()
	{
	}

	/// <summary>
	/// Constructs a new <see cref="ActivitySourceAttribute"/> specifying the <see cref="Name"/>.
	/// </summary>
	/// <param name="name">The <see cref="Name"/>.</param>
	public ActivitySourceAttribute(string name)
	{
		Name = name;
	}

	/// <summary>
	/// Sets the name for the generated <see cref="global::System.Diagnostics.ActivitySource.Name"/>,
	/// overriding the <see cref="global::Purview.Telemetry.Activities.ActivitySourceGenerationAttribute.Name"/>.
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	/// Specifies the default when inferring between
	/// <see cref="global::Purview.Telemetry.TagAttribute"/> or
	/// <see cref="global::Purview.Telemetry.Activities.BaggageAttribute"/>, unless
	/// explicitly marked.
	/// </summary>
	public bool DefaultToTags { get; set; } = true;

	/// <summary>
	/// Prefix used to when generating the tag or baggage name. Prepended
	/// before the <see cref="global::Purview.Telemetry.TagAttribute.Name"/> or
	/// <see cref="global::Purview.Telemetry.Activities.BaggageAttribute.Name"/>.
	/// </summary>
	public string? BaggageAndTagPrefix { get; set; }

	/// <summary>
	/// Determines if the <see cref="Name"/> (or <see cref="global::Purview.Activities.ActivitySourceGenerationAttribute.BaggageAndTagPrefix"/>)
	/// is used as a prefix, before the <see cref="global::Purview.Telemetry.BaggageAndTagPrefix"/>.
	/// </summary>
	public bool IncludeActivitySourcePrefix { get; set; } = true;

	/// <summary>
	/// Determines if the <see cref="global::Purview.Telemetry.TagAttribute.Name"/> or
	/// <see cref="global::Purview.Telemetry.Activities.BaggageAttribute.Name"/> (including
	/// any prefixes) are lowercased.
	/// </summary>
	public bool LowercaseBaggageAndTagKeys { get; set; } = true;
}
