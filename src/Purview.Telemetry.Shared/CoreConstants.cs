namespace Purview.Telemetry;

// Do not rename to filename.
static partial class Constants {
	public const string EmbedAttributesHashDefineName = "PURVIEW_TELEMETRY_ATTRIBUTES";

	static public partial class Shared {
		public const bool SkipOnNullOrEmptyDefault = false;

		public const bool GenerateDependencyExtensionDefault = true;
	}

	static public partial class Activities {
		public const bool UseRecordExceptionRulesDefault = true;

		public const bool RecordExceptionEscapedDefault = true;
	}
}
