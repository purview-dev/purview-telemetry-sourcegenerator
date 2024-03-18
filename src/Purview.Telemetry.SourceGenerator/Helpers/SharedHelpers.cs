﻿using System.Globalization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Helpers;

static partial class SharedHelpers {
	static public bool AttributeParser(
		AttributeData attributeData,
		Action<string, object> namedArguments,
		SemanticModel? semanticModel,
		IGenerationLogger? logger,
		CancellationToken cancellationToken) {
		logger?.Debug($"Found attribute: {attributeData}");

		if (semanticModel != null && HasErrors(attributeData, semanticModel, logger, cancellationToken)) {
			logger?.Warning($"Attribute has error: {attributeData}");
			return false;
		}

		var constructorMethod = attributeData.AttributeConstructor;
		if (constructorMethod == null) {
			logger?.Warning("Could not locate the attribute's constructor.");

			return false;
		}

		if (attributeData.ConstructorArguments.Any(t => t.Kind == TypedConstantKind.Error)) {
			logger?.Warning("Constructor arguments have an error.");

			return false;
		}

		if (attributeData.NamedArguments.Any(t => t.Value.Kind == TypedConstantKind.Error)) {
			logger?.Warning("Named arguments have an error.");

			return false;
		}

		// supports: [DefaultLogLevel(LogGeneratedLevel.Information)]
		// supports: [DefaultLogLevel(level: LogGeneratedLevel.Information)]
		var items = attributeData.ConstructorArguments;
		if (items.Length > 0) {
			for (var i = 0; i < items.Length; i++) {
				cancellationToken.ThrowIfCancellationRequested();

				if (items[i].IsNull) {
					continue;
				}

				var name = constructorMethod.Parameters[i].Name;
				var value = Utilities.GetTypedConstantValue(items[i])!;
				if (Constants.System.String.Equals(constructorMethod.Parameters[i].Type)) {
					var v = (string)value;
					if (string.IsNullOrWhiteSpace(v)) {
						continue;
					}
				}

				namedArguments(name, value);
			}
		}

		// argument syntax takes parameters. e.g. Level = LogGeneratedLevel.Information
		// supports: e.g. [DefaultLogLevel(Level = LogGeneratedLevel.Information )]
		if (attributeData.NamedArguments.Any()) {
			foreach (var namedArgument in attributeData.NamedArguments) {
				cancellationToken.ThrowIfCancellationRequested();

				var value = Utilities.GetTypedConstantValue(namedArgument.Value)!;
				if (namedArgument.Value.Type == null) {
					logger?.Error($"Named argument {namedArgument.Key}'s type could not be determined.");
					continue;
				}

				if (Constants.System.String.Equals(namedArgument.Value.Type)) {
					var v = (string)value;
					if (string.IsNullOrWhiteSpace(v)) {
						continue;
					}
				}

				namedArguments(namedArgument.Key, value!);
			}
		}

		return true;
	}

	static public bool AttributeParser(
		AttributeSyntax attributeSyntax,
		Action<string, string> namedArguments,
		Action<string, OutputType>? logger,
		CancellationToken cancellationToken) {
		logger?.Invoke($"Found attribute (syntax): {attributeSyntax}", OutputType.Debug);

		var arguments = attributeSyntax.ArgumentList?.Arguments;
		if (arguments != null) {
			foreach (var argument in arguments) {
				cancellationToken.ThrowIfCancellationRequested();
				var name = argument.NameEquals?.Name.ToString() ?? argument.DescendantNodes().OfType<IdentifierNameSyntax>().FirstOrDefault()?.ToString();
				var value = argument.Expression.ToString();
				if (name == null || value == null) {
					continue;
				}

				namedArguments(name!, value);
			}
		}

		return true;
	}

	static bool HasErrors(AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken cancellationToken) {
		if (attributeData.ApplicationSyntaxReference?.GetSyntax(cancellationToken) is not AttributeSyntax attributeSyntax) {
			return false;
		}

		var diagnostics = semanticModel.GetDiagnostics(attributeSyntax.Span, cancellationToken);
		if (diagnostics.Length > 0 && logger != null) {
			var d = diagnostics.Select(m => m.GetMessage(CultureInfo.InvariantCulture));
			logger.Debug("Attribute has diagnostics: \n" + string.Join("\n - ", d));
		}

		return diagnostics.Any(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
	}

	static public TagOrBaggageAttributeRecord? GetTagOrBaggageAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {

		AttributeStringValue? nameValue = null;
		AttributeValue<bool>? skipOnNullOrEmpty = null;

		if (!AttributeParser(attributeData,
		(name, value) => {
			if (name.Equals(nameof(TagAttribute.Name), StringComparison.OrdinalIgnoreCase)) {
				nameValue = new((string)value);
			}
			else if (name.Equals(nameof(TagAttribute.SkipOnNullOrEmpty), StringComparison.OrdinalIgnoreCase)) {
				skipOnNullOrEmpty = new((bool)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			Name: nameValue ?? new(),
			SkipOnNullOrEmpty: skipOnNullOrEmpty ?? new(Constants.Shared.SkipOnNullOrEmptyDefault)
		);
	}

	static public TelemetryGenerationAttributeRecord GetTelemetryGenerationAttribute(ISymbol type, SemanticModel semanticModel, IGenerationLogger? logger, CancellationToken token) {
		token.ThrowIfCancellationRequested();

		if (!Utilities.TryContainsAttribute(type, Constants.Shared.TelemetryGenerationAttribute, token, out var attributeData)) {
			if (!Utilities.TryContainsAttribute(semanticModel.Compilation.Assembly, Constants.Shared.TelemetryGenerationAttribute, token, out attributeData)) {
				return createDefault();
			}
		}

		return GetTelemetryGenerationAttribute(attributeData!, semanticModel, logger, token)
			?? createDefault();

		static TelemetryGenerationAttributeRecord createDefault()
			=> new(
				GenerateDependencyExtension: new(Constants.Shared.GenerateDependencyExtensionDefault),
				ClassNameTemplate: new(Constants.Shared.ClassNameTemplateDefault)
			);
	}

	static TelemetryGenerationAttributeRecord? GetTelemetryGenerationAttribute(
		AttributeData attributeData,
		SemanticModel semanticModel,
		IGenerationLogger? logger,
		CancellationToken token) {

		AttributeValue<bool>? generateDependencyExtension = null;
		AttributeStringValue? classNameTemplate = null;

		if (!AttributeParser(attributeData,
		(name, value) => {
			if (name.Equals(nameof(TelemetryGenerationAttribute.GenerateDependencyExtension), StringComparison.OrdinalIgnoreCase)) {
				generateDependencyExtension = new((bool)value);
			}
			else if (name.Equals(nameof(TelemetryGenerationAttribute.ClassNameTemplate), StringComparison.OrdinalIgnoreCase)) {
				classNameTemplate = new((string)value);
			}
		}, semanticModel, logger, token)) {
			// Failed to parse correctly, so null it out.
			return null;
		}

		return new(
			GenerateDependencyExtension: generateDependencyExtension ?? new(Constants.Shared.GenerateDependencyExtensionDefault),
			ClassNameTemplate: classNameTemplate ?? new(Constants.Shared.ClassNameTemplateDefault)
		);
	}
}
