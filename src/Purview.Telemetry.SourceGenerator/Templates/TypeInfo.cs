using Microsoft.CodeAnalysis;

namespace Purview.Telemetry.SourceGenerator.Templates;

record TypeInfo(string Name, string FullName, string Namespace) : IEquatable<string> {
	public bool Equals(string other)
		=> other == Name || other == FullName;

	public bool Equals(ITypeSymbol other) {
		if (other == null)
			return false;

		var typeName = other.ToString();
		if (other.NullableAnnotation == NullableAnnotation.Annotated)
			typeName = typeName.TrimEnd('?');

		var idx = typeName.IndexOf('<');
		if (idx > -1)
			typeName = typeName.Substring(0, idx);

		return Equals(typeName) || Equals(other.Name);
	}

	override public string ToString()
		=> FullName;

	static public implicit operator string(TypeInfo typeInfo)
		=> typeInfo.FullName;

	static public TypeInfo Create(string fullName) {
		if (string.IsNullOrWhiteSpace(fullName))
			throw new ArgumentNullException(nameof(fullName));

		var @parts = fullName.Split('.');

		return new(parts.LastOrDefault() ?? fullName, fullName, string.Join(".", parts.Take(parts.Length - 2)));
	}
	static public TypeInfo Create<T>()
		=> Create(typeof(T).FullName);
}
