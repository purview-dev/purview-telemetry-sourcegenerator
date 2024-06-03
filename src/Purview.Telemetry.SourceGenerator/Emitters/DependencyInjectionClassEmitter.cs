using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;
using Purview.Telemetry.SourceGenerator.Templates;

namespace Purview.Telemetry.SourceGenerator.Emitters;

static class DependencyInjectionClassEmitter
{
	public static void GenerateImplementation(
		GenerationType requestingType,
		TelemetryGenerationAttributeRecord attribute,
		GenerationType generationType,

		string implementationClassName,
		string sourceInterfaceName,

		string? fullyQualifiedNamespace,

		SourceProductionContext context,
		IGenerationLogger? logger)
	{
		context.CancellationToken.ThrowIfCancellationRequested();

		if (attribute.GenerateDependencyExtension.Value == false)
		{
			logger?.Debug("Skipping dependency injection emit.");
			return;
		}

		if (!SharedHelpers.ShouldEmit(requestingType, generationType))
		{
			logger?.Debug($"Skipping dependency injection emit for {requestingType} ({generationType}).");
			return;
		}

		StringBuilder builder = new();

		var classNameToGenerate = attribute.DependencyInjectionClassName.Value;
		if (string.IsNullOrWhiteSpace(classNameToGenerate))
			classNameToGenerate = implementationClassName + "DIExtension";

		logger?.Debug($"Generating service dependency class {classNameToGenerate} for: {fullyQualifiedNamespace}{sourceInterfaceName}");

		context.CancellationToken.ThrowIfCancellationRequested();

		builder
			.Append("namespace ")
			.AppendLine(Constants.DependencyInjection.DependencyInjectionNamespace)
			.AppendLine('{')
		;

		builder
			.Append(1, "[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]")
			.Append(1, "static class ", withNewLine: false)
			.Append(classNameToGenerate)
			.AppendLine()
			.Append(1, '{')
		;

		EmitMethod(
			builder,
			2,

			implementationClassName,
			sourceInterfaceName,
			fullyQualifiedNamespace,

			logger,
			context.CancellationToken
		);

		EmitHelpers.EmitClassEnd(builder, 1);

		builder.AppendLine('}');

		var sourceText = EmbeddedResources.Instance.AddHeader(builder.ToString());
		var hintName = $"{fullyQualifiedNamespace}{classNameToGenerate}.DependencyInjection.g.cs";

		context.AddSource(hintName, Microsoft.CodeAnalysis.Text.SourceText.From(sourceText, Encoding.UTF8));
	}

	static void EmitMethod(StringBuilder builder, int indent, string className, string interfaceName, string? fullyQualifiedNamespace, IGenerationLogger? logger, CancellationToken token)
	{
		token.ThrowIfCancellationRequested();

		logger?.Debug($"Emitting DI method for {interfaceName}.");

		var methodName = interfaceName;
		if (methodName[0] == 'I')
			methodName = methodName.Substring(1);

		methodName = $" Add{methodName}";

		builder
			.AggressiveInlining(indent)
			.Append(indent, "static public ", withNewLine: false)
			.Append(Constants.DependencyInjection.IServiceCollection)
			.Append(methodName)
			.Append("(this ")
			.Append(Constants.DependencyInjection.IServiceCollection)
			.AppendLine(" services)")
			.Append(indent, '{')
			.Append(indent + 1, "return services.AddSingleton<", withNewLine: false)
			// serviceType...
			.Append(fullyQualifiedNamespace)
			.Append(interfaceName)
			.Append(", ")
			// implementationType
			.Append(fullyQualifiedNamespace)
			.Append(className)
			.AppendLine(">();")
			.Append(indent, '}')
		;
	}
}
