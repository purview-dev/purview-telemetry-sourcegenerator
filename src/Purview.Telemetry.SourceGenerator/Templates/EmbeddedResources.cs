﻿using System.Globalization;
using System.Reflection;
using System.Text;

namespace Purview.Telemetry.SourceGenerator.Templates;

sealed class EmbeddedResources {
	readonly Assembly _ownerAssembly = typeof(EmbeddedResources).Assembly;
	readonly string _namespaceRoot = typeof(EmbeddedResources).Namespace;

	// Make sure this is above any calls to LoadTemplateForEmitting.
	readonly string _autoGeneratedHeader;

	EmbeddedResources() {
		_autoGeneratedHeader = LoadEmbeddedResource("AutoGeneratedHeader.cs");
	}

	static public EmbeddedResources Instance { get; } = new();

	string LoadEmbeddedResource(string resourceName) {
		resourceName = $"{_namespaceRoot}.Sources.{resourceName}";

		var resourceStream = _ownerAssembly.GetManifestResourceStream(resourceName);
		if (resourceStream is null) {
			var existingResources = _ownerAssembly.GetManifestResourceNames();
			throw new ArgumentException($"Could not find embedded resource {resourceName}. Available resource names: {string.Join(", ", existingResources)}");
		}

		using StreamReader reader = new(resourceStream, Encoding.UTF8);

		return reader.ReadToEnd();
	}

	public string AddHeader(string text) {
		return _autoGeneratedHeader
			.Replace("{DateTime}", DateTimeOffset.UtcNow.ToString(CultureInfo.InvariantCulture))
			+ "\n\r"
			+ text;
	}

	public string LoadTemplateForEmitting(TemplateInfo templateInfo, bool attachHeader = true) {
		var source = templateInfo.Source == null
			? null : templateInfo.Source + ".";

		var resource = LoadEmbeddedResource($"{source}{templateInfo.Name}.cs")
			.Replace("sealed public ", "sealed ");

		if (!attachHeader)
			return resource;

		return AddHeader("#if " + Constants.EmbedAttributesHashDefineName + @"

" + resource
			   + @"
#endif");
	}
}
