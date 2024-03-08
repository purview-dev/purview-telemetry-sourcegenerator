using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Targets;

namespace Purview.Telemetry.SourceGenerator.Emitters;

static partial class LoggerTargetClassEmitter {
	static public void GenerateImplementation(LoggerTarget target, SourceProductionContext context, IGenerationLogger? logger) {
		logger?.Debug($"Generating {target.FullyQualifiedName} class for log target.");

		GenerateLoggerClass(target, context, logger);
	}
}
