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

#if PURVIEW_TELEMETRY_EMBED_ATTRIBUTES

namespace Purview.Telemetry.Activities;

/// <summary>
/// Determines the default Activity Source name for generated Activities.
/// </summary>
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = false)]
sealed class ActivitySourceAttribute : Attribute {
	/// <summary>
	/// Constructs a new <see cref="ActivitySourceAttribute"/>.
	/// </summary>
	/// <param name="activitySource">The name of the activity source.</param>
	/// <exception cref="ArgumentNullException">If the <paramref name="activitySource"/> is null, empty or whitespace.</exception>
	public ActivitySourceAttribute(string activitySource) {
		if (string.IsNullOrWhiteSpace(activitySource)) {
			throw new ArgumentNullException(nameof(activitySource));
		}

		ActivitySource = activitySource;
	}

	/// <summary>
	/// The default activity source name to use.
	/// </summary>
	public string ActivitySource { get; }
}

#endif

