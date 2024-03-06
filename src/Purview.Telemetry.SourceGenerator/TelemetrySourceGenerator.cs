using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;

namespace Purview.Telemetry.SourceGenerator;

sealed public partial class TelemetrySourceGenerator : IIncrementalGenerator, ILogSupport {
	IGenerationLogger? _logger;

	public void Initialize(IncrementalGeneratorInitializationContext context) {
		// Register all of the shared attributes we need.
		context.RegisterPostInitializationOutput(ctx => {
			foreach (var template in Constants.GetAllTemplates()) {
				_logger?.Debug($"Adding {template.Name} as source.");

				ctx.AddSource($"{template.Name}.g.cs", template.TemplateData);
			}
		});

		RegisterLoggerGeneration(context, _logger);
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
