namespace Purview.Telemetry;

// Do not rename to filename.
static partial class Constants {
	public const string EmbedAttributesHashDefineName = "PURVIEW_TELEMETRY_EMBED_ATTRIBUTES";

	static public string DefaultActivitySourceName { get; set; } = "purview";

	static public partial class Shared {
		public const bool SkipOnNullOrEmptyDefault = false;

		public const bool GenerateDependencyExtensionDefault = true;

		public const string ClassNameTemplateDefault = "{GeneratedClassName}Extensions";
	}
}
