namespace Purview.Telemetry.SourceGenerator.Helpers;

static partial class PipelineHelpers {
	static string GenerateClassName(string name) {
		if (name[0] == 'I') {
			name = name.Substring(1);
		}

		return name + "Core";
	}
}
