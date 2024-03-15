using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;

namespace Purview.Telemetry.SourceGenerator;

[Generator]
sealed public partial class TelemetrySourceGenerator : IIncrementalGenerator, ILogSupport {
	IGenerationLogger? _logger;

	public void Initialize(IncrementalGeneratorInitializationContext context) {
		// Register all of the shared attributes we need.
		context.RegisterPostInitializationOutput(ctx => {
			foreach (var template in Constants.GetAllTemplates()) {
				_logger?.Debug($"Adding {template.Name} as {template.GetGeneratedFilename()}.");

				ctx.AddSource(template.GetGeneratedFilename(), template.TemplateData);
			}
		});

		RegisterActivitiesGeneration(context, _logger);
		RegisterLoggerGeneration(context, _logger);
		RegisterMetricsGeneration(context, _logger);
	}

	void ILogSupport.SetLogOutput(Action<string, OutputType> action) {
		if (action == null) {
			_logger = null;
		}
		else {
			_logger = new Logger(action);
		}
	}
}
