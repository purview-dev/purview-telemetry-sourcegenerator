using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Helpers;

static partial class PipelineHelpers {
	static string GenerateClassName(string name) {
		if (name[0] == 'I') {
			name = name.Substring(1);
		}

		return name + "Core";
	}

	static string GenerateParameterName(string name, string? prefix, bool lowercase) {
		if (lowercase) {
			name = name.ToLowerInvariant();
		}

		return $"{prefix}{name}";
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0075:Simplify conditional expression", Justification = "Don't 'simplify' this as changing the default value of the skipOnNullOrEmpty parameter will change the behaviour")]
	static bool GetSkipOnNullOrEmptyValue(TagOrBaggageAttributeRecord? tagOrBaggageAttribute)
		=> tagOrBaggageAttribute?.SkipOnNullOrEmpty?.IsSet == true
				? tagOrBaggageAttribute!.SkipOnNullOrEmpty!.Value!.Value
				: Constants.Shared.SkipOnNullOrEmptyDefault;
}
