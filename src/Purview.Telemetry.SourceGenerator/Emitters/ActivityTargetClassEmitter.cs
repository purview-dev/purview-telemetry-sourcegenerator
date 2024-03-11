using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Emitters;

static partial class ActivityTargetClassEmitter {
	static public void GenerateImplementation(ActivityGenerationTarget target, SourceProductionContext context, IGenerationLogger? logger) {
		logger?.Debug($"Generating {target.FullyQualifiedName} class for activity target.");

	}
}
