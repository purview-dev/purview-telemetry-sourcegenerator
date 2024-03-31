using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Helpers;

static class Utilities {
	readonly static SymbolDisplayFormat _symbolDisplayFormat = new(
		typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces
	);

	static public TargetGeneration IsValidGenerationTarget(IMethodSymbol method, GenerationType generationType, GenerationType requestedType) {
		var attributes = method.GetAttributes();
		var activityCount = attributes.Count(m =>
			Constants.Activities.ActivityAttribute.Equals(m)
			|| Constants.Activities.EventAttribute.Equals(m)
			|| Constants.Activities.ContextAttribute.Equals(m)
		);
		var loggingCount = attributes.Count(Constants.Logging.LogAttribute.Equals);
		var metricsCount = attributes.Count(m =>
			Constants.Metrics.CounterAttribute.Equals(m)
			|| Constants.Metrics.UpDownCounterAttribute.Equals(m)
			|| Constants.Metrics.HistogramAttribute.Equals(m)
			|| Constants.Metrics.ObservableCounterAttribute.Equals(m)
			|| Constants.Metrics.ObservableGaugeAttribute.Equals(m)
			|| Constants.Metrics.ObservableUpDownCounterAttribute.Equals(m)
		);

		var count = activityCount + loggingCount + metricsCount;
		var inferenceNotSupportedWithMultiTargeting = false;
		var multiGenerationTargetsNotSupported = false;
		if (generationType != requestedType) {
			// This means it's multi-target generation so we need everything to be explicit.
			if (count == 0) {
				inferenceNotSupportedWithMultiTargeting = true;
			}
		}

		if (count > 1) {
			multiGenerationTargetsNotSupported = true;
		}

		var isValid = !multiGenerationTargetsNotSupported && !inferenceNotSupportedWithMultiTargeting;
		if (isValid) {

			if (generationType.HasFlag(GenerationType.Activities) && requestedType == GenerationType.Activities) {
				isValid = loggingCount == 0 && metricsCount == 0;
			}

			if (generationType.HasFlag(GenerationType.Logging) && requestedType == GenerationType.Logging) {
				isValid = activityCount == 0 && metricsCount == 0;
			}

			if (generationType.HasFlag(GenerationType.Metrics) && requestedType == GenerationType.Metrics) {
				isValid = activityCount == 0 && loggingCount == 0;
			}
		}

		return new(
			IsValid: isValid,
			RaiseInferenceNotSupportedWithMultiTargeting: inferenceNotSupportedWithMultiTargeting,
			RaiseMultiGenerationTargetsNotSupported: multiGenerationTargetsNotSupported
		);
	}

	static public string WithNull(this string value)
		=> value + "?";

	static public StringBuilder AggressiveInlining(this StringBuilder builder, int indent)
	=> builder.Append(indent, Constants.System.AggressiveInlining);

	static public StringBuilder IfDefines(this StringBuilder builder, string condition, int indent, params string[] values) {
		builder
			.AppendLine()
			.Append("#if ")
			.AppendLine(condition)
			.AppendTabs(indent)
		;

		foreach (var value in values) {
			builder.Append(value);
		}

		builder
			.AppendLine()
			.AppendLine("#endif")
		;

		return builder;
	}

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

		List<string> parentClasses = [];
		while (parentClass != null) {
			parentClasses.Insert(0, parentClass.Identifier.Text);

			parentClass = parentClass.Parent as ClassDeclarationSyntax;
		}

		if (parentClasses.Count == 0) {
			return null;
		}

		return string.Join(".", parentClasses);
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

	static public string GetFullyQualifiedName(ITypeSymbol namedType, bool trimNullableAnnotation = true) {
		var result = namedType.ToDisplayString(_symbolDisplayFormat) ?? namedType.ToString();
		if (trimNullableAnnotation && namedType.NullableAnnotation == NullableAnnotation.Annotated) {
			result = result.TrimEnd('?');
		}

		return result;
	}

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
				fullNamespace += ".";
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
		=> parameterType == (Constants.System.IEnumerable.FullName + "<" + fullTypeName + ">")
		|| parameterType.StartsWith(Constants.System.IEnumerable.FullName + "<" + fullTypeName, StringComparison.Ordinal);

	static public bool IsBoolean(ITypeSymbol type)
		=> Constants.System.Boolean.Equals(type);

	static public bool IsBoolean(string type)
		=> type == Constants.System.BoolKeyword
			|| Constants.System.Boolean.Equals(type);

	static public bool IsString(ITypeSymbol type)
		=> type.ToDisplayString() == Constants.System.StringKeyword
			|| Constants.System.String.Equals(type);

	static public bool IsString(string type)
		=> type == Constants.System.StringKeyword
			|| Constants.System.String.Equals(type);

	static public bool IsExceptionType(ITypeSymbol? typeSymbol) {
		while (typeSymbol != null) {
			if (Constants.System.Exception.Equals(typeSymbol)) {
				return true;
			}

			typeSymbol = typeSymbol.BaseType;
		}

		return false;
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

	static public Templates.TypeInfo ConvertToMSLogLevel(Logging.LogGeneratedLevel? level)
		=> level switch {
			Logging.LogGeneratedLevel.Trace => Constants.Logging.MicrosoftExtensions.LogLevel_Trace,
			Logging.LogGeneratedLevel.Debug => Constants.Logging.MicrosoftExtensions.LogLevel_Debug,
			Logging.LogGeneratedLevel.Information => Constants.Logging.MicrosoftExtensions.LogLevel_Information,
			Logging.LogGeneratedLevel.Warning => Constants.Logging.MicrosoftExtensions.LogLevel_Warning,
			Logging.LogGeneratedLevel.Error => Constants.Logging.MicrosoftExtensions.LogLevel_Error,
			Logging.LogGeneratedLevel.Critical => Constants.Logging.MicrosoftExtensions.LogLevel_Critical,
			_ => Constants.Logging.MicrosoftExtensions.LogLevel_None,
		};

	static public bool ContainsAttribute(ISymbol symbol, Templates.TypeInfo typeInfo, CancellationToken token)
		=> TryContainsAttribute(symbol, typeInfo, token, out _);

	static public bool TryContainsAttribute(ISymbol symbol, Templates.TypeInfo typeInfo, CancellationToken token, out AttributeData? attributeData) {
		attributeData = null;

		var attributes = symbol.GetAttributes();
		foreach (var attribute in attributes) {
			token.ThrowIfCancellationRequested();

			if (attribute.AttributeClass != null && typeInfo.Equals(attribute.AttributeClass)) {
				attributeData = attribute;
				return true;
			}
		}

		return false;
	}

	static public bool ContainsAttribute(ISymbol symbol, Templates.TemplateInfo templateInfo, CancellationToken token)
		=> TryContainsAttribute(symbol, templateInfo, token, out _);

	static public bool TryContainsAttribute(ISymbol symbol, Templates.TemplateInfo templateInfo, CancellationToken token, out AttributeData? attributeData) {
		attributeData = null;

		var attributes = symbol.GetAttributes();
		foreach (var attribute in attributes) {
			token.ThrowIfCancellationRequested();

			if (attribute.AttributeClass != null && templateInfo.Equals(attribute.AttributeClass)) {
				attributeData = attribute;
				return true;
			}
		}

		return false;
	}

	static public string LowercaseFirstChar(string value) {
		if (value.Length > 0) {
			var firstChar = char.ToLowerInvariant(value[0]);
			value = firstChar + value.Substring(1);
		}

		return value;
	}

	static public string UppercaseFirstChar(string value) {
		if (value.Length > 0) {
			var firstChar = char.ToUpperInvariant(value[0]);
			value = firstChar + value.Substring(1);
		}

		return value;
	}
}
