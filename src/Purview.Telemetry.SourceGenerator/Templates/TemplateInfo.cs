using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Purview.Telemetry.SourceGenerator.Templates;

record TemplateInfo(string Name, string FullName, string Namespace, string? Source)
	: IEquatable<NameSyntax>, IEquatable<AttributeSyntax>, IEquatable<ISymbol>, IEquatable<string>, IEquatable<AttributeData> {
	public string TemplateData { get; private set; } = default!;

	public bool Equals(string other)
		=> other == Name || other == FullName;

	public bool Equals(NameSyntax other) {
		var isAttribute = Name.EndsWith("Attribute", StringComparison.Ordinal);

		var name = other.ToString();

		var result = Equals(name);
		if (!result && isAttribute) {
			result = Equals(name + "Attribute");
		}

		return result;
	}

	public bool Equals(AttributeSyntax other)
		=> Equals(other.Name);

	public bool Equals(ISymbol other)
		=> Equals(other.ToString());

	public bool Equals(AttributeData other)
		=> (other.AttributeClass != null) && Equals(other.AttributeClass);

	static public TemplateInfo Create<T>(bool attachHeader = true)
		=> Create(typeof(T).FullName, attachHeader);

	static public TemplateInfo Create(string fullTypeName, bool attachHeader = true) {
		var parts = fullTypeName.Split('.');

		var typeName = RemoveGenericTypeInfo(parts.Last());
		fullTypeName = RemoveGenericTypeInfo(fullTypeName);
		var @namespace = fullTypeName.Substring(0, fullTypeName.Length - (typeName.Length + 1));
		var source = @namespace.Split('.');
		var isRootSources = source.Length == 2;

		TemplateInfo templateInfo = new(typeName, fullTypeName, @namespace, isRootSources ? null : source.Last());
		templateInfo.TemplateData = EmbeddedResources.Instance.LoadTemplateForEmitting(templateInfo, attachHeader);

		return templateInfo;
	}

	static string RemoveGenericTypeInfo(string identifier) {
		var idx = identifier.IndexOf('`');
		if (idx > -1) {
			identifier = identifier.Substring(0, idx);
		}

		return identifier;
	}

	static public implicit operator string(TemplateInfo templateInfo)
		=> templateInfo.FullName;
}
