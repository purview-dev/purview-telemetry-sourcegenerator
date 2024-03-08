using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Targets;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class LoggerTargetClassEmitter {
	static int EmitFields(LoggerGenerationTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		indent++;

		builder
			.Append(indent, "const ", withNewLine: false)
			.Append(Constants.Logging.MicrosoftExtensions.LogLevel)
			.Append(' ')
			.Append(Constants.Logging.DefaultLogLevelConstantName)
			.Append(" = ")
			.Append(Utilities.ConvertToMSLogLevel(target.DefaultLevel))
			.AppendLine(';');
		;

		builder
			.AppendLine()
			.Append(indent, "readonly ", withNewLine: false)
			.Append(Constants.Logging.MicrosoftExtensions.ILogger)
			.Append('<')
			.Append(target.FullyQualifiedInterfaceName)
			.Append('>')
			.Append(' ')
			.Append("_logger;")
			.AppendLine()
			.AppendLine()
		;

		foreach (var methodTarget in target.LogEntryMethods) {
			context.CancellationToken.ThrowIfCancellationRequested();

			EmitLogActionField(builder, indent, methodTarget);
		}

		return --indent;
	}

	static void EmitLogActionField(StringBuilder builder, int indent, LogEntryMethodGenerationTarget methodTarget) {
		builder
			.Append(indent, "static readonly ", withNewLine: false)
			.Append(methodTarget.IsScoped ? Constants.System.Func : Constants.System.Action)
			.Append('<')
			.Append(Constants.Logging.MicrosoftExtensions.ILogger)
			.Append(", ")
		;

		var parameterCount = methodTarget.GetParameterCount(includingException: false);
		foreach (var parameter in methodTarget.Parameters) {
			if (parameter.IsException) {
				continue;
			}

			builder.Append(parameter.FullyQualifiedType);
			if (parameter.IsNullable) {
				builder.Append('?');
			}

			builder.Append(", ");
		}

		if (methodTarget.IsScoped) {
			builder
				.Append(Constants.System.IDisposable)
				.Append("> ")
			;
		}
		else {
			builder
				.Append(Constants.System.Exception)
				.Append("> ")
			;
		}

		builder
			.Append(methodTarget.LoggerActionFieldName)
			.Append(" = ")
			.Append(Constants.Logging.MicrosoftExtensions.LoggerMessage)
			.Append(".Define")
		;

		if (methodTarget.IsScoped) {
			builder
				.Append("Scope")
			;
		}

		if (parameterCount > 0) {
			builder.Append('<');

			var i = 0;
			foreach (var parameter in methodTarget.Parameters) {
				if (parameter.IsException) {
					continue;
				}

				builder
					.Append(parameter.FullyQualifiedType)
				;

				if (parameter.IsNullable) {
					builder
						.Append('?')
					;
				}

				if (i < parameterCount)
					builder
						.Append(", ")
					;

				i++;
			}

			builder.Append('>');
		}

		builder.Append('(');

		if (!methodTarget.IsScoped) {
			builder
				.Append(Utilities.ConvertToMSLogLevel(methodTarget.Level))
				.Append(", ")
			;

			if (methodTarget.EventId != null) {
				builder
					.Append("new ")
					.Append(Constants.Logging.MicrosoftExtensions.EventId)
					.Append('(')
					.Append(methodTarget.EventId.Value)
					.Append(", \"")
					.Append(methodTarget.LogEntryName)
					.Append("\"), ")
				;
			}
			else
				builder.Append("default, ");
		}

		builder
			.Append('"')
			.Append(methodTarget.MessageTemplate)
			.Append('"')
			.AppendLine(");")
		;
	}
}
