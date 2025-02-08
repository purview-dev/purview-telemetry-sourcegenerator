﻿namespace Purview.Telemetry.Metrics;

/// <summary>
/// Marker attribute, used to indicating a meter, or group of instruments.
/// </summary>
[global::System.AttributeUsage(global::System.AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
[global::System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed class MeterAttribute : global::System.Attribute
{
	/// <summary>
	/// Creates a new <see cref="MeterAttribute" />.
	/// </summary>
	public MeterAttribute()
	{
	}

	/// <summary>
	/// Creates a new <see cref="MeterAttribute" />, specifying the <paramref name="name"/>.
	/// </summary>
	/// <param name="name">Specifies the <see cref="Name"/>.</param>
	public MeterAttribute(string name)
	{
		Name = name;
	}

	/// <summary>
	/// Optional. Gets/ sets the name of the meter, used for creating
	/// a named grouped of instruments.
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	/// Optional, gets/ sets the prefix used when generating the instrument name.
	/// </summary>
	public string? InstrumentPrefix { get; set; }

	/// <summary>
	/// Optional, determines if <see cref="MeterGenerationAttribute.InstrumentPrefix" /> is
	/// included in the generated name.
	/// </summary>
	public bool IncludeAssemblyInstrumentPrefix { get; set; } = true;

	/// <summary>
	/// Determines if the <see cref="CounterAttribute.Name"/>, <see cref="HistogramAttribute.Name"/>, 
	/// <see cref="UpDownCounterAttribute.Name"/>, <see cref="ObservableCounterAttribute.Name"/>,
	/// <see cref="ObservableGaugeAttribute.Name"/> or <see cref="ObservableUpDownCounterAttribute.Name"/> (including
	/// any prefixes) are lowercased.
	/// </summary>
	public bool LowercaseInstrumentName { get; set; } = true;

	/// <summary>
	/// Determines if the <see cref="global::Purview.Telemetry.TagAttribute.Name"/> (including
	/// any prefixes) are lowercased.
	/// </summary>
	public bool LowercaseTagKeys { get; set; } = true;
}
