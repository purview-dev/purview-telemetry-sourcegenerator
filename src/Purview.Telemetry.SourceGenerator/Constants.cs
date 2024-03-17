using Purview.Telemetry.SourceGenerator.Templates;

// This as the non-SourceGenerator namespace...
namespace Purview.Telemetry;

static partial class Constants {
	public const string SystemDiagnosticsNamespace = "System.Diagnostics";

	static public TemplateInfo[] GetAllTemplates() {
		return [
			.. Activities.GetTemplates(),
			.. Logging.GetTemplates(),
			.. Metrics.GetTemplates(),
			.. Shared.GetTemplates()
		];
	}

	static public partial class Shared {
		readonly static public TemplateInfo TagAttribute = TemplateInfo.Create<TagAttribute>();
		readonly static public TemplateInfo TelemetryGenerationAttribute = TemplateInfo.Create<TelemetryGenerationAttribute>();
		readonly static public TemplateInfo Constants = TemplateInfo.Create(typeof(Constants).FullName);

		static public TemplateInfo[] GetTemplates() => [
			TagAttribute,
			TelemetryGenerationAttribute,
			Constants
		];
	}

	static public partial class DependencyInjection {
		public const string DependencyInjectionNamespace = "Microsoft.Extensions.DependencyInjection";

		readonly static public TypeInfo IServiceCollection = TypeInfo.Create(DependencyInjectionNamespace + ".IServiceCollection");
		readonly static public TypeInfo ServiceDescriptor = TypeInfo.Create(DependencyInjectionNamespace + ".ServiceDescriptor");
		readonly static public TypeInfo ServiceLifetime = TypeInfo.Create(DependencyInjectionNamespace + ".ServiceLifetime");

		readonly static public string Singleton = ServiceLifetime + "." + nameof(Singleton);
	}

	static public class Diagnostics {
		public const string Usage = nameof(Usage);

		static public class Activity {
			public const string Usage = nameof(Activity) + "." + nameof(Usage);
		}

		static public class Logging {
			public const string Usage = nameof(Logging) + "." + nameof(Usage);
		}

		static public class Metrics {
			public const string Usage = nameof(Metrics) + "." + nameof(Usage);
		}
	}
}
