﻿//HintName: TelemetryGenerationAttribute.g.cs
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

namespace Purview.Telemetry;

/// <summary>
/// Marker attribute to control the generation of telemetry-based classes.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Assembly | System.AttributeTargets.Interface, AllowMultiple = false)]
[System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed class TelemetryGenerationAttribute : System.Attribute {
	/// <summary>
	/// Initializes a new instance of the <see cref="TelemetryGenerationAttribute"/> class.
	/// </summary>
	public TelemetryGenerationAttribute() {
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TelemetryGenerationAttribute"/> class, and
	/// specifies the <see cref="GenerateDependencyExtension"/> property and optionally the
	/// <see cref="ClassName"/> and <see cref="DependencyInjectionClassName"/> properties.
	/// </summary>
	/// <param name="generateDependencyExtension">Specifies the <see cref="GenerateDependencyExtension"/>.</param>
	/// <param name="className">Optionally specifies the <see cref="ClassName"/>.</param>
	/// <param name="dependencyInjectionClassName">Optionally specifies the <see cref="DependencyInjectionClassName"/>.</param>
	public TelemetryGenerationAttribute(bool generateDependencyExtension, string? className = null, string? dependencyInjectionClassName = null) {
		GenerateDependencyExtension = generateDependencyExtension;
		ClassName = className;
		DependencyInjectionClassName = dependencyInjectionClassName;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TelemetryGenerationAttribute"/> class, and
	/// specifies the <see cref="ClassName"/> and optionally the <see cref="DependencyInjectionClassName"/> property.
	/// </summary>
	/// <param name="className">Specifies the <see cref="ClassName"/>.</param>
	/// <param name="dependencyInjectionClassName">Optionally specifies the <see cref="DependencyInjectionClassName"/>.</param>
	public TelemetryGenerationAttribute(string className, string? dependencyInjectionClassName = null) {
		ClassName = className;
		DependencyInjectionClassName = dependencyInjectionClassName;
	}

	/// <summary>
	/// Determines if an extension method is created registering
	/// the source interface and the generated class with
	/// and <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
	/// </summary>
	public bool GenerateDependencyExtension { get; set; } = true;

	/// <summary>
	/// Optionally specifies the name of the telemetry implementation class to use.
	/// Defaults to null. When null, uses the source interface name minus any starting 'I',
	/// and appends 'Core' to the end.
	/// </summary>
	public string? ClassName { get; set; }

	/// <summary>
	/// Optionally specifies the name of the dependency injection class to generation.
	/// </summary>
	public string? DependencyInjectionClassName { get; set; }
}
