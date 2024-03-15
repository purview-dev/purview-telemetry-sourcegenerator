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

		static public TemplateInfo[] GetTemplates() => [
			TagAttribute
		];
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
