using System.Diagnostics;

namespace Purview.Telemetry.Activities;

/// <summary>
/// Marker attribute required for Activity generation.
/// </summary>
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = false)]
[Conditional(Constants.EmbedAttributesHashDefineName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class ActivityTargetAttribute : Attribute {
	/// <summary>
	/// Constructs a new <see cref="ActivityTargetAttribute"/>.
	/// </summary>
	public ActivityTargetAttribute() {
	}

	/// <summary>
	/// Constructs a new <see cref="ActivityTargetAttribute"/>.
	/// </summary>
	/// <param name="activitySource">The <see cref="ActivitySource"/>.</param>
	/// <param name="className">An optional <see cref="ClassName"/>.</param>
	public ActivityTargetAttribute(string activitySource, string? className = null) {
		ActivitySource = activitySource;
		ClassName = className;
	}

	/// <summary>
	/// Overrides the generated activity source name.
	/// </summary>
	public string? ActivitySource { get; set; }

	/// <summary>
	/// Overrides the class name generated for this activity target.
	/// </summary>
	public string? ClassName { get; set; }
}
