using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Helpers;

static partial class PipelineHelpers
{
	static string GenerateClassName(string name)
	{
		if (name[0] == 'I')
			name = name.Substring(1);

		return name + "Core";
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase")]
	static string GenerateParameterName(string name, string? prefix, bool lowercase)
	{
		if (lowercase)
			name = name.ToLowerInvariant();

		return $"{prefix}{name}";
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0075:Simplify conditional expression", Justification = "Don't 'simplify' this as changing the default value of the skipOnNullOrEmpty parameter will change the behaviour")]
	static bool GetSkipOnNullOrEmptyValue(TagOrBaggageAttributeRecord? tagOrBaggageAttribute)
		=> tagOrBaggageAttribute?.SkipOnNullOrEmpty.IsSet == true
			? tagOrBaggageAttribute.SkipOnNullOrEmpty.Value!.Value
			: false;

	static ImmutableDictionary<string, Location[]> BuildDuplicateMethods(INamedTypeSymbol interfaceSymbol)
	{
		var methods = interfaceSymbol.GetMembers().OfType<IMethodSymbol>();
		Dictionary<string, List<Location>> dict = [];
		foreach (var method in methods)
		{
			if (dict.TryGetValue(method.Name, out var list))
				list.AddRange(method.Locations);
			else
				dict[method.Name] = [.. method.Locations];
		}

		return dict
			.Where(m => m.Value.Count > 1)
			.ToImmutableDictionary(m => m.Key, m => m.Value.ToArray());
	}
}
