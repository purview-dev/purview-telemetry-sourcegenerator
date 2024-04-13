﻿namespace Purview.Telemetry.Activities;

/// <summary>
/// Determines the default <see cref="System.Diagnostics.ActivitySource.Name" /> for generated
/// <see cref="System.Diagnostics.Activity">activities</see> and <see cref="System.Diagnostics.ActivityEvent">events</see>.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed class ActivitySourceGenerationAttribute : System.Attribute
{
	/// <summary>
	/// Constructs a new <see cref="ActivitySourceGenerationAttribute"/>.
	/// </summary>
	/// <param name="name">The name of the activity source.</param>
	/// <param name="defaultToTags">Determines if the default for method parameters are Tags (default) or Baggage.</param>
	/// <exception cref="ArgumentNullException">If the <paramref name="name"/> is null, empty or whitespace.</exception>
	public ActivitySourceGenerationAttribute(string name, bool defaultToTags = true)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new System.ArgumentNullException(nameof(name));
		}

		Name = name;
		DefaultToTags = defaultToTags;
	}

	/// <summary>
	/// Specifies the default <see cref="System.Diagnostics.ActivitySource.Name"/> to use.
	/// </summary>
	public string Name { get; }

	/// <summary>
	/// Specifies the default used when inferring between
	/// <see cref="Purview.Telemetry.TagAttribute"/>
	/// or <see cref="Purview.Telemetry.Activities.BaggageAttribute"/>, unless
	/// explicitly marked. Overridden when specifying <see cref="ActivitySourceAttribute.DefaultToTags"/>.
	/// </summary>
	public bool DefaultToTags { get; set; } = true;

	/// <summary>
	/// Prefix used to when generating the tag or baggage name. Prepended
	/// before the <see cref="Purview.Telemetry.TagAttribute.Name"/> or
	/// <see cref="Purview.Telemetry.Activities.BaggageAttribute.Name"/>, unless
	/// explicitly marked. Overridden when specifying <see cref="ActivitySourceAttribute.BaggageAndTagPrefix"/>.
	/// </summary>
	public string? BaggageAndTagPrefix { get; set; }

	/// <summary>
	/// Determines the separator used between the <see cref="System.Diagnostics.ActivitySource.Name"/> and
	/// the various prefix options. The default is a period.
	/// </summary>
	public string BaggageAndTagSeparator { get; set; } = ".";

	/// <summary>
	/// Determines if the <see cref="Purview.Telemetry.TagAttribute.Name"/> or
	/// <see cref="Purview.Telemetry.Activities.BaggageAttribute.Name"/> (including
	/// any prefixes) are lowercased, unless
	/// explicitly marked. Overridden when specifying <see cref="ActivitySourceAttribute.LowercaseBaggageAndTagKeys"/>.
	/// </summary>
	public bool LowercaseBaggageAndTagKeys { get; set; } = true;
}
