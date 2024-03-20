﻿namespace Purview.Telemetry;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface, AllowMultiple = false)]
[System.Diagnostics.Conditional(Constants.EmbedAttributesHashDefineName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed public class TelemetryGenerationAttribute : Attribute {
	public TelemetryGenerationAttribute() {
	}

	public TelemetryGenerationAttribute(bool generateDependencyExtension, string? className = null, string? dependencyInjectionClassName = null) {
		GenerateDependencyExtension = generateDependencyExtension;
		ClassName = className;
		DependencyInjectionClassName = dependencyInjectionClassName;
	}

	public TelemetryGenerationAttribute(string className, string? dependencyInjectionClassName = null) {
		ClassName = className;
		DependencyInjectionClassName = dependencyInjectionClassName;
	}

	public bool GenerateDependencyExtension { get; set; } = Constants.Shared.GenerateDependencyExtensionDefault;

	public string? ClassName { get; set; }

	public string? DependencyInjectionClassName { get; set; }
}