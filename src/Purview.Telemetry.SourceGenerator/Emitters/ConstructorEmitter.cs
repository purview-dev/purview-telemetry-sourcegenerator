using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Emitters;

static class ConstructorEmitter {
	const string _loggerParameterName = "logger";

	static public int EmitCtor(
		GenerationType requestingType,
		GenerationType generationType,
		string classNameToGenerate,
		string fullyQualifiedInterfaceName,
		StringBuilder builder,
		int indent,
		SourceProductionContext context,
		IGenerationLogger? logger) {

		context.CancellationToken.ThrowIfCancellationRequested();

		if (!SharedHelpers.ShouldEmit(requestingType, generationType)) {
			logger?.Debug($"Skipping constructor emit for {requestingType} ({generationType}).");

			return indent;
		}

		indent++;

		builder
			.AppendLine()
			.Append(indent, "public ", withNewLine: false)
			.Append(classNameToGenerate)
			.Append('(')
		;

		EmitParameters(generationType, fullyQualifiedInterfaceName, builder, indent);

		builder
			.AppendLine(')')
			.Append(indent, '{')
		;

		EmitBody(generationType, indent, builder);

		builder
			.Append(indent, '}')
		;

		return --indent;
	}

	static void EmitParameters(GenerationType generationType, string? loggerFullyQualifiedInterfaceName, StringBuilder builder, int indent) {
		if (generationType.HasFlag(GenerationType.Logging)) {
			builder
				.Append(Constants.Logging.MicrosoftExtensions.ILogger)
				.Append('<')
				.Append(loggerFullyQualifiedInterfaceName)
				.Append("> ")
				.Append(_loggerParameterName)
			;
		}

		if (generationType.HasFlag(GenerationType.Metrics)) {
			builder
				.AppendLine()
				.AppendLine("#if NET8_OR_GREATER")
			;

			if (generationType.HasFlag(GenerationType.Logging)) {
				builder
					.Append(", ")
				;
			}

			builder
				.Append(indent + 1, Constants.Metrics.SystemDiagnostics.IMeterFactory, withNewLine: false)
				.Append(' ')
				.AppendLine(Constants.Metrics.MeterFactoryParameterName)
			;

			builder
				.AppendLine("#endif")
				.AppendTabs(indent)
			;
		}
	}

	static void EmitBody(GenerationType generationType, int indent, StringBuilder builder) {
		if (generationType.HasFlag(GenerationType.Logging)) {
			builder
				.Append(indent + 1, Constants.Logging.LoggerFieldName, withNewLine: false)
				.Append(" = ")
				.Append(_loggerParameterName)
				.AppendLine(';');
			;
		}

		if (generationType.HasFlag(GenerationType.Metrics)) {
			builder
				.Append(indent + 1, Constants.Metrics.MeterInitializationMethod, withNewLine: false)
				.AppendLine('(')
			;

			builder
				.AppendLine("#if NET8_OR_GREATER")
				.Append(indent + 2, Constants.Metrics.MeterFactoryParameterName)
				.AppendLine("#endif")
			;

			builder
				.Append(indent + 1, ");")
			;
		}
	}
}
