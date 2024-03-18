namespace Purview.Telemetry;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface, AllowMultiple = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class TelemetryGenerationAttribute : Attribute {
	public TelemetryGenerationAttribute() {
	}

	public TelemetryGenerationAttribute(bool generateDependencyExtension, string classNameTemplate = Constants.Shared.ClassNameTemplateDefault) {
		GenerateDependencyExtension = generateDependencyExtension;
		ClassNameTemplate = classNameTemplate ?? throw new ArgumentNullException(nameof(classNameTemplate));
	}

	public bool GenerateDependencyExtension { get; set; } = Constants.Shared.GenerateDependencyExtensionDefault;

	/// <summary>
	/// <![CDATA[
	/// Replacement options:
	/// <ul>
	///     <li>{GeneratedClassName} the generated class</li>
	///     <li>{InterfaceName} the source interface name</li>
	/// </ul>
	/// ]]>
	/// </summary>
	public string ClassNameTemplate { get; set; } = Constants.Shared.ClassNameTemplateDefault;
}
