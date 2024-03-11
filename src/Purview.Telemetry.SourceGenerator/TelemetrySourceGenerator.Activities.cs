using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.Logging;
using Purview.Telemetry.SourceGenerator.Emitters;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator;

partial class TelemetrySourceGenerator {
	static void RegisterActivitiesGeneration(IncrementalGeneratorInitializationContext context, IGenerationLogger? logger) {
		// Transform
		Func<GeneratorAttributeSyntaxContext, CancellationToken, ActivityGenerationTarget?> activityTargetTransform =
			logger == null
				? static (context, cancellationToken) => PipelineHelpers.BuildActivityTransform(context, null, cancellationToken)
				: (context, cancellationToken) => PipelineHelpers.BuildActivityTransform(context, logger, cancellationToken);

		// Register
		var loggerTargetsPredicate = context.SyntaxProvider
			.ForAttributeWithMetadataName(
				Constants.Activities.ActivityTargetAttribute,
				static (node, token) => PipelineHelpers.HasActivityTargetAttribute(node, token),
				activityTargetTransform
			)
			.WhereNotNull()
			.WithTrackingName($"{nameof(TelemetrySourceGenerator)}_{nameof(LoggerTargetAttribute)}");

		// Build generation (static vs. non-static is for the logger).
		Action<SourceProductionContext, (Compilation Compilation, ImmutableArray<ActivityGenerationTarget?> Targets)> generationLoggerAction =
			logger == null
				? static (spc, source) => GenerateActivitiesTargets(source.Targets, spc, null)
				: (spc, source) => GenerateActivitiesTargets(source.Targets, spc, logger);

		// Register with the source generator.
		var activityTargets
			= context.CompilationProvider.Combine(loggerTargetsPredicate.Collect());

		context.RegisterSourceOutput(
			source: activityTargets,
			action: generationLoggerAction
		);
	}

	static void GenerateActivitiesTargets(ImmutableArray<ActivityGenerationTarget?> targets, SourceProductionContext spc, IGenerationLogger? logger) {
		if (targets.Length == 0) {
			return;
		}

		try {
			foreach (var target in targets) {
				logger?.Debug($"Activity generation target: {target!.FullyQualifiedName}");

				ActivityTargetClassEmitter.GenerateImplementation(target!, spc, logger);
			}
		}
		catch (Exception ex) {
			logger?.Error($"A fatal error occurred while executing the source generation stage: {ex}");

			TelemetryDiagnostics.Report(spc.ReportDiagnostic, TelemetryDiagnostics.General.FatalExecutionDuringExecution, null, ex);
		}
	}
}
