using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class LoggerTargetClassEmitter {
	static int EmitMethods(LoggerGenerationTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		indent++;

		foreach (var methodTarget in target.LogEntryMethods) {
			context.CancellationToken.ThrowIfCancellationRequested();

			if (methodTarget.HasMultipleExceptions) {
				continue;
			}

			if (methodTarget.ParameterCount > Constants.Logging.MaxNonExceptionParameters) {
				continue;
			}

			EmitLogActionMethod(builder, indent, methodTarget, context, logger);
		}

		return --indent;
	}

	static void EmitLogActionMethod(StringBuilder builder, int indent, LogEntryMethodGenerationTarget methodTarget, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		logger?.Debug($"Building logging method: {methodTarget.MethodName}");

		builder
			.AppendLine()
			.Append(indent, "public ", withNewLine: false)
		;

		if (methodTarget.IsScoped) {
			builder.Append(Constants.System.IDisposable);
		}
		else {
			builder.Append(Constants.System.VoidKeyword);
		}

		builder
			.Append(' ')
			.Append(methodTarget.MethodName)
			.Append('(')
		;

		EmitParametersAsMethodArgumentList(methodTarget, builder, context);

		builder
			.Append(')')
			.AppendLine()
			.Append(indent, '{')
		;

		if (methodTarget.IsScoped) {
			builder
				.Append(indent + 1, "return ", withNewLine: false)
				.Append(methodTarget.LoggerActionFieldName)
				.Append("(_logger");
		}
		else {
			builder
				.Append(indent + 1, "if (!_logger.IsEnabled(", withNewLine: false)
				.Append(methodTarget.MSLevel)
				.Append(')')
				.Append(')')
				.AppendLine()
				.Append(indent + 2, "return;")
				.AppendLine()
				.Append(indent + 1, methodTarget.LoggerActionFieldName, withNewLine: false)
				.Append("(_logger")
			;
		}

		foreach (var parameter in methodTarget.ParametersSansException) {
			builder
				.Append(", ")
				.Append(parameter.Name)
			;
		}

		if (methodTarget.ExceptionParameter != null) {
			builder
				.Append(", ")
				.Append(methodTarget.ExceptionParameter.Name);
		}
		else if (!methodTarget.IsScoped) {
			builder
				.Append(", ")
				.Append("null")
			;
		}

		builder
			.AppendLine(");")
		;

		builder
			.Append(indent, '}')
			.AppendLine()
		;
	}

	static void EmitParametersAsMethodArgumentList(LogEntryMethodGenerationTarget methodTarget, StringBuilder builder, SourceProductionContext context) {
		for (var i = 0; i < methodTarget.TotalParameterCount; i++) {
			context.CancellationToken.ThrowIfCancellationRequested();

			builder
				.Append(methodTarget.AllParameters[i].FullyQualifiedType)
			;

			if (methodTarget.AllParameters[i].IsNullable) {
				builder
					.Append('?')
				;
			}

			builder
				.Append(' ')
				.Append(methodTarget.AllParameters[i].Name);

			if (i < methodTarget.TotalParameterCount - 1) {
				builder.Append(", ");
			}
		}
	}
}
