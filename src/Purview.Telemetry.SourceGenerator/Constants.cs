using Purview.Telemetry.SourceGenerator.Templates;

// This as the non-SourceGenerator namespace...
namespace Purview.Telemetry;

static partial class Constants
{
	public const string SystemDiagnosticsNamespace = "System.Diagnostics";
	public const string EmbedAttributesHashDefineName = "PURVIEW_TELEMETRY_ATTRIBUTES";

	public static TemplateInfo[] GetAllTemplates()
	{
		return [
			.. Activities.GetTemplates(),
			.. Logging.GetTemplates(),
			.. Metrics.GetTemplates(),
			.. Shared.GetTemplates()
		];
	}

	public static partial class Shared
	{
		public static readonly TemplateInfo TagAttribute = TemplateInfo.Create("Purview.Telemetry.TagAttribute");
		public static readonly TemplateInfo ExcludeAttribute = TemplateInfo.Create("Purview.Telemetry.ExcludeAttribute");
		public static readonly TemplateInfo TelemetryGenerationAttribute = TemplateInfo.Create("Purview.Telemetry.TelemetryGenerationAttribute");

		public static TemplateInfo[] GetTemplates() => [
			TagAttribute,
			ExcludeAttribute,
			TelemetryGenerationAttribute,
		];
	}

	public static partial class DependencyInjection
	{
		public const string DependencyInjectionNamespace = "Microsoft.Extensions.DependencyInjection";

		public static readonly TypeInfo IServiceCollection = TypeInfo.Create(DependencyInjectionNamespace + ".IServiceCollection");
		public static readonly TypeInfo ServiceDescriptor = TypeInfo.Create(DependencyInjectionNamespace + ".ServiceDescriptor");
		public static readonly TypeInfo ServiceLifetime = TypeInfo.Create(DependencyInjectionNamespace + ".ServiceLifetime");

		public static readonly string Singleton = ServiceLifetime + "." + nameof(Singleton);
	}

	public static class Diagnostics
	{
		public const string Usage = nameof(Usage);

		public static class Activity
		{
			public const string Usage = nameof(Activity) + "." + nameof(Usage);
		}

		public static class Logging
		{
			public const string Usage = nameof(Logging) + "." + nameof(Usage);
		}

		public static class Metrics
		{
			public const string Usage = nameof(Metrics) + "." + nameof(Usage);
		}
	}
}
