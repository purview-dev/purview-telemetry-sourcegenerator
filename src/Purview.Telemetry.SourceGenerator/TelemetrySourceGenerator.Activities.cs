using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.Activities;
using Purview.Telemetry.SourceGenerator.Emitters;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator;

partial class TelemetrySourceGenerator {
	static void RegisterActivitiesGeneration(IncrementalGeneratorInitializationContext context, IGenerationLogger? logger) {
		// Transform
		Func<GeneratorAttributeSyntaxContext, CancellationToken, ActivitySourceTarget?> activityTargetTransform =
			logger == null
				? static (context, cancellationToken) => PipelineHelpers.BuildActivityTransform(context, null, cancellationToken)
				: (context, cancellationToken) => PipelineHelpers.BuildActivityTransform(context, logger, cancellationToken);

		// Register
		var activityTargetsPredicate = context.SyntaxProvider
			.ForAttributeWithMetadataName(
				Constants.Activities.ActivitySourceAttribute,
				static (node, token) => PipelineHelpers.HasActivityTargetAttribute(node, token),
				activityTargetTransform
			)
			.WhereNotNull()
			.WithTrackingName($"{nameof(TelemetrySourceGenerator)}_{nameof(ActivitySourceAttribute)}");

		// Build generation (static vs. non-static is for the logger).
		Action<SourceProductionContext, (Compilation Compilation, ImmutableArray<ActivitySourceTarget?> Targets)> generationActivityAction =
			logger == null
				? static (spc, source) => GenerateActivitiesTargets(source.Targets, spc, null)
				: (spc, source) => GenerateActivitiesTargets(source.Targets, spc, logger);

		// Register with the source generator.
		var activityTargets
			= context.CompilationProvider.Combine(activityTargetsPredicate.Collect());

		context.RegisterSourceOutput(
			source: activityTargets,
			action: generationActivityAction
		);
	}

	static void GenerateActivitiesTargets(ImmutableArray<ActivitySourceTarget?> targets, SourceProductionContext spc, IGenerationLogger? logger) {
		if (targets.Length == 0) {
			return;
		}

		try {
			foreach (var target in targets) {
				logger?.Debug($"Activity generation target: {target!.FullyQualifiedName}");

				ActivitySourceTargetClassEmitter.GenerateImplementation(target!, spc, logger);
			}
		}
		catch (Exception ex) {
			logger?.Error($"A fatal error occurred while executing the source generation stage: {ex}");

			TelemetryDiagnostics.Report(spc.ReportDiagnostic, TelemetryDiagnostics.General.FatalExecutionDuringExecution, null, ex);
		}
	}
}
