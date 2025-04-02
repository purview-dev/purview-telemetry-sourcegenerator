﻿namespace Purview.Telemetry;

/// <summary>
/// Marker attribute to control the generation of telemetry-based classes.
/// </summary>
[global::System.AttributeUsage(global::System.AttributeTargets.Assembly | global::System.AttributeTargets.Interface, AllowMultiple = false)]
[global::System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed class TelemetryGenerationAttribute : global::System.Attribute
{
	/// <summary>
	/// Initializes a new instance of the <see cref="TelemetryGenerationAttribute"/> class.
	/// </summary>
	public TelemetryGenerationAttribute()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TelemetryGenerationAttribute"/> class, and
	/// specifies the <see cref="GenerateDependencyExtension"/> property and optionally the
	/// <see cref="ClassName"/> and <see cref="DependencyInjectionClassName"/> properties.
	/// </summary>
	/// <param name="generateDependencyExtension">Specifies the <see cref="GenerateDependencyExtension"/>.</param>
	/// <param name="className">Optionally specifies the <see cref="ClassName"/>.</param>
	/// <param name="dependencyInjectionClassName">Optionally specifies the <see cref="DependencyInjectionClassName"/>.</param>
	public TelemetryGenerationAttribute(bool generateDependencyExtension, string? className = null, string? dependencyInjectionClassName = null)
	{
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
	public TelemetryGenerationAttribute(string className, string? dependencyInjectionClassName = null)
	{
		ClassName = className;
		DependencyInjectionClassName = dependencyInjectionClassName;
	}

	/// <summary>
	/// Determines if an extension method is created registering
	/// the source interface and the generated class with
	/// and <see cref="global::Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
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

	/// <summary>
	/// Determines if the generated dependency injection class is public.
	/// </summary>
	public bool DependencyInjectionClassIsPublic { get; set; }
}
