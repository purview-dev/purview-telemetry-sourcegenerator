using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Emitters;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator;

partial class TelemetrySourceGenerator
{
	static void RegisterMetricsGeneration(IncrementalGeneratorInitializationContext context, IGenerationLogger? logger)
	{
		// Transform
		Func<GeneratorAttributeSyntaxContext, CancellationToken, MeterTarget?> meterTargetTransform =
			logger == null
				? static (context, cancellationToken) => PipelineHelpers.BuildMeterTransform(context, null, cancellationToken)
				: (context, cancellationToken) => PipelineHelpers.BuildMeterTransform(context, logger, cancellationToken);

		// Register
		var meterTargetsPredicate = context.SyntaxProvider
			.ForAttributeWithMetadataName(
				Constants.Metrics.MeterAttribute,
				static (node, token) => PipelineHelpers.HasMeterTargetAttribute(node, token),
				meterTargetTransform
			)
			.WhereNotNull()
			.WithTrackingName($"{nameof(TelemetrySourceGenerator)}_Meters");

		// Build generation (static vs. non-static is for the logger).
		Action<SourceProductionContext, (Compilation Compilation, ImmutableArray<MeterTarget?> Targets)> generationMeterAction =
			logger == null
				? static (spc, source) => GenerateMeterTargets(source.Targets, spc, null)
				: (spc, source) => GenerateMeterTargets(source.Targets, spc, logger);

		// Register with the source generator.
		var meterTargets
			= context.CompilationProvider.Combine(meterTargetsPredicate.Collect());

		context.RegisterImplementationSourceOutput(
			source: meterTargets,
			action: generationMeterAction
		);
	}

	static void GenerateMeterTargets(ImmutableArray<MeterTarget?> targets, SourceProductionContext spc, IGenerationLogger? logger)
	{
		if (targets.Length == 0)
			return;

		if (targets.Any(m => m!.Failure != null))
		{
			var target = targets.First(m => m!.Failure != null)!;

			TelemetryDiagnostics.Report(spc.ReportDiagnostic, target.Failure!);

			return;
		}

		try
		{
			foreach (var target in targets)
			{
				logger?.Debug($"Meter generation target: {target!.FullyQualifiedName}");

				MeterTargetClassEmitter.GenerateImplementation(target!, spc, logger);
			}
		}
		catch (Exception ex)
		{
			logger?.Error($"A fatal error occurred while executing the source generation stage: {ex}");

			TelemetryDiagnostics.Report(spc.ReportDiagnostic, TelemetryDiagnostics.General.FatalExecutionDuringExecution, ex);
		}
	}
}
