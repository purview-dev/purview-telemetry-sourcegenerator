namespace Purview.Telemetry.Activities;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
sealed public class EscapeAttribute : Attribute {
}
