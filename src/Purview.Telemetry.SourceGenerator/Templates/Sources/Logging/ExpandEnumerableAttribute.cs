﻿#if !EXCLUDE_PURVIEW_TELEMETRY_LOGGING

namespace Purview.Telemetry.Logging;

/// <summary>
/// Determines if an array/ enumerable property should be expanded into
/// individual elements. This only works when the <code>Microsoft.Extensions.Telemetry.Abstractions</code>
/// NuGet package is included.
/// </summary>
{CodeGen}
[global::System.AttributeUsage(global::System.AttributeTargets.Parameter, AllowMultiple = false)]
[global::System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed class ExpandEnumerableAttribute : global::System.Attribute
{
	/// <summary>
	/// Creates a new instance of the <see cref="ExpandEnumerableAttribute"/>, optionally
	/// specifying the <see cref="MaximumValueCount"/> property to determine the maximum
	/// number of elements to include.
	/// </summary>
	/// <param name="maximumValueCount">Specifies the <see cref="MaximumValueCount"/>, defaults to 5.</param>
	public ExpandEnumerableAttribute(int maximumValueCount = 5)
	{
		MaximumValueCount = maximumValueCount;
	}

	/// <summary>
	/// Optional. Determines the number of the elements to render in the logging
	/// properties. If the value is <code>null</code>, then there is no limit to the
	/// number of elements to include. However, this is NOT recommended.
	/// </summary>
	public int? MaximumValueCount { get; set; }
}

#endif
