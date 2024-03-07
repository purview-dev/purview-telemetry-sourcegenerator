using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Purview.Telemetry.SourceGenerator.Helpers;

static class Utilities {
	readonly static SymbolDisplayFormat _symbolDisplayFormat = new(
		typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces
	);

	static public StringBuilder AppendTabs(this StringBuilder builder, int tabs) {
		for (var i = 0; i < tabs; i++) {
			builder.Append('\t');
		}

		return builder;
	}

	static public StringBuilder Append(this StringBuilder builder, int tabs, char value, bool withNewLine = true) {
		builder
			.AppendTabs(tabs)
			.Append(value);

		if (withNewLine) {
			builder.AppendLine();
		}

		return builder;
	}

	static public StringBuilder Append(this StringBuilder builder, int tabs, string value, bool withNewLine = true) {
		builder
			.AppendTabs(tabs)
			.Append(value);

		if (withNewLine) {
			builder.AppendLine();
		}

		return builder;
	}

	//static public StringBuilder AppendLines(this StringBuilder builder, int lineCount = 2) {
	//	for (var i = 0; i < lineCount; i++) {
	//		builder.AppendLine();
	//	}

	//	return builder;
	//}

	static public StringBuilder AppendLine(this StringBuilder builder, char @char)
		=> builder
			.Append(@char)
			.AppendLine();

	//static public StringBuilder AppendWrap(this StringBuilder builder, string value, char c = '"')
	//	=> builder
	//			.Append(c)
	//			.Append(value)
	//			.Append(c);

	static public string Wrap(this string value, char c = '"')
		=> c + value + c;

	static public string Strip(this string value, char c = '"') {
		if (value.Length > 1 && value[0] == c) {
			value = value.Substring(1);
		}

		if (value.Length > 1 && value[value.Length - 1] == c) {
			value = value.Substring(0, value.Length - 1);
		}

		return value;
	}

	//static public string? GetMemberIdentity(MemberDeclarationSyntax memberSyntax) {
	//	if (memberSyntax is MethodDeclarationSyntax method) {
	//		return method.Identifier.ValueText;
	//	}
	//	else if (memberSyntax is PropertyDeclarationSyntax property) {
	//		return property.Identifier.ValueText;
	//	}
	//	else if (memberSyntax is FieldDeclarationSyntax field) {
	//		var variable = field.Declaration.Variables.FirstOrDefault();
	//		return variable?.Identifier.ValueText;
	//	}
	//	else if (memberSyntax is EventFieldDeclarationSyntax @event) {
	//		var variable = @event.Declaration.Variables.FirstOrDefault();
	//		return variable?.Identifier.ValueText;
	//	}
	//	else if (memberSyntax is IndexerDeclarationSyntax indexer) {
	//		return indexer.ToString();
	//	}

	//	return null;
	//}

	static public ClassDeclarationSyntax? GetParentClass(SyntaxNode? node) {
		while (node != null) {
			if (node.Parent is ClassDeclarationSyntax classNode) {
				return classNode;
			}

			node = node.Parent;
		}

		return null;
	}

	static public string[] GetParentClasses(TypeDeclarationSyntax classDeclaration) {
		var parentClass = classDeclaration.Parent as ClassDeclarationSyntax;

		List<string> parentClassList = [];
		while (parentClass != null) {
			parentClassList.Add(parentClass.Identifier.Text);

			parentClass = parentClass.Parent as ClassDeclarationSyntax;
		}

		return [.. parentClassList];
	}

	static public string? GetParentClassesAsNamespace(TypeDeclarationSyntax classDeclaration) {
		var parentClass = classDeclaration.Parent as ClassDeclarationSyntax;
		string? parentClasses = null;

		while (parentClass != null) {
			if (parentClasses != null) {
				parentClasses += "+";
			}

			parentClasses += parentClass.Identifier.Text;

			parentClass = parentClass.Parent as ClassDeclarationSyntax;
		}

		return parentClasses;
	}

	static public string GetNamespace(TypeDeclarationSyntax typeSymbol) {
		// Determine the namespace the type is declared in, if any
		var potentialNamespaceParent = typeSymbol.Parent;
		while (potentialNamespaceParent != null &&
			   potentialNamespaceParent is not NamespaceDeclarationSyntax
			   && potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax) {
			potentialNamespaceParent = potentialNamespaceParent.Parent;
		}

		if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceParent) {
			var @namespace = namespaceParent.Name.ToString();
			while (true) {
				if (namespaceParent.Parent is not NamespaceDeclarationSyntax namespaceParentParent) {
					break;
				}

				namespaceParent = namespaceParentParent;
				@namespace = $"{namespaceParent.Name}.{@namespace}";
			}

			return @namespace;
		}

		return string.Empty;
	}

	static public string GetFullyQualifiedName(ITypeSymbol namedType)
		=> namedType.ToDisplayString(_symbolDisplayFormat);

	//static public string GetFullyQualifiedName(TypeDeclarationSyntax type)
	//	=> GetFullNamespace(type, true) + type.Identifier.Text;

	static public string? GetFullNamespace(TypeDeclarationSyntax type, bool includeTrailingSeparator) {
		var typeNamespace = GetNamespace(type);
		var parentClasses = GetParentClassesAsNamespace(type);

		string? fullNamespace = null;
		if (typeNamespace != null) {
			fullNamespace = typeNamespace;
		}

		if (parentClasses != null) {
			if (fullNamespace != null) {
				fullNamespace += ".";
			}

			fullNamespace += parentClasses;

			if (includeTrailingSeparator) {
				fullNamespace += "+";
			}
		}
		else if (includeTrailingSeparator && fullNamespace != null) {
			fullNamespace += ".";
		}

		return fullNamespace;
	}

	static public object? GetTypedConstantValue(TypedConstant arg)
		=> arg.Kind == TypedConstantKind.Array
			? arg.Values
			: arg.Value;

	static public IncrementalValuesProvider<TSource> WhereNotNull<TSource>(this IncrementalValuesProvider<TSource> source)
		=> source.Where(static m => m is not null);

	static public bool IsEnumerableOrArray(string parameterType, string fullTypeName)
		=> IsArray(parameterType, fullTypeName)
			|| IsEnumerable(parameterType, fullTypeName);

	static public bool IsArray(string parameterType, string fullTypeName)
		=> parameterType == (fullTypeName + "[]");

	static public bool IsEnumerable(string parameterType, string fullTypeName)
		=> parameterType == (Constants.Shared.IEnumerable.FullName + "<" + fullTypeName + ">")
		|| parameterType.StartsWith(Constants.Shared.IEnumerable.FullName + "<" + fullTypeName, StringComparison.Ordinal);

	static public bool IsString(string type)
		=> type == Constants.Shared.String.Name
			|| type == Constants.Shared.String.FullName
			|| type == Constants.Shared.StringKeyword;

	static public bool IsExceptionType(ITypeSymbol? typeSymbol) {
		if (typeSymbol == null)
			return false;

		if (Constants.Shared.Exception.Equals(typeSymbol))
			return true;

		return IsExceptionType(typeSymbol.BaseType);
	}

	static public string Flatten(this SyntaxNode syntax)
		=> syntax.WithoutTrivia()
			.ToString()
			.Flatten();

	static public string Flatten(this SyntaxToken syntax)
		=> syntax.WithoutTrivia()
			.ToString()
			.Flatten();

	static public string Flatten(this string value)
		=> Regex.Replace(value, @"\s+", " ", RegexOptions.None, TimeSpan.FromMilliseconds(2000));
}
