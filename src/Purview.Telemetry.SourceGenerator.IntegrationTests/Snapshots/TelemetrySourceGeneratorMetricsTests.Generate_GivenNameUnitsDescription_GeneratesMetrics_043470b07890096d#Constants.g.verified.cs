﻿//HintName: Constants.g.cs
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Purview.Telemetry.SourceGenerator
//     on {Scrubbed}.
//
//     Changes to this file may cause incorrect behaviour and will be lost
//     when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable 1591 // publicly visible type or member must be documented

#if PURVIEW_TELEMETRY_EMBED_ATTRIBUTES

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

#endif
