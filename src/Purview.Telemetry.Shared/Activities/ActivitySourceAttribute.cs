using System.Diagnostics;

namespace Purview.Telemetry.Activities;

/// <summary>
/// Determines the default Activity Source name for generated Activities.
/// </summary>
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = false)]
[Conditional(Constants.EmbedAttributesHashDefineName)]
sealed public class ActivitySourceAttribute : Attribute {
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
