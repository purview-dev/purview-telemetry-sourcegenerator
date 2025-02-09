using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class LoggerGenTargetClassEmitter
{
	static int EmitMethods(LoggerTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger)
	{
		indent++;

		foreach (var methodTarget in target.LogMethods)
		{
			context.CancellationToken.ThrowIfCancellationRequested();

			if (!methodTarget.TargetGenerationState.IsValid)
				continue;

			EmitLogActionMethod(builder, indent, methodTarget, context, logger);
		}

		return --indent;
	}

	static void EmitLogActionMethod(StringBuilder builder, int indent, LogTarget methodTarget, SourceProductionContext context, IGenerationLogger? logger)
	{
		context.CancellationToken.ThrowIfCancellationRequested();

		logger?.Debug($"Building logging method: {methodTarget.MethodName}");

		builder
			.AppendLine()
			.AggressiveInlining(indent)
			.Append(indent, "public ", withNewLine: false)
		;

		if (methodTarget.IsScoped)
			builder.Append(Constants.System.IDisposable.WithGlobal().WithNullable());
		else
			builder.Append(Constants.System.VoidKeyword);

		builder
			.Append(' ')
			.Append(methodTarget.MethodName)
			.Append('(')
		;

		//EmitParametersAsMethodArgumentList(methodTarget, builder, context);

		builder
			.Append(')')
			.AppendLine()
			.Append(indent, '{')
		;

		indent++;

		// Output state here...then we can use it in
		// the scoped and none-scoped output.
		// If we have exceptions, output them to the state...
		// UNLESS it's NOT-scoped and then we take the FIRST
		// exception and output it as the exception parameter in
		// the Log method.

		var stateVarName = "state";
		var i = 0;
		while (methodTarget.Parameters.Any(m => m.Name == stateVarName))
		{
			stateVarName = "state_" + i;
			i++;
		}

		var hasState = true;

		if (methodTarget.IsScoped)
		{
			EmitStateContent(builder, indent, methodTarget, stateVarName, context, logger);

			// return _logger.BeginScope(state);
			// or
			// return _logger.BeginScope("MessageFormat", arg1, arg2, arg3, ...);

			// TODO: state.
			builder
				.Append(indent, "return ", withNewLine: false)
				.Append(Constants.Logging.LoggerFieldName)
				.Append(".BeginScope(")
			;

			if (hasState)
			{
				builder.Append(stateVarName);
			}
			else
			{
				// MESSAGE FORMAT
				builder.Append("TODO: MESSAGE TEMPLATE".Wrap());
			}

			builder.AppendLine(");");
		}
		else
		{
			// First check if the the Log Level is enabled.
			// if (!_logger.IsEnabled(LogLevel.Information)))
			// { return; };
			builder
				.Append(indent, "if (!", withNewLine: false)
				.Append(Constants.Logging.LoggerFieldName)
				.Append(".IsEnabled(")
				.Append(methodTarget.MSLevel.WithGlobal())
				.Append("))")
				.AppendLine()
				.Append(indent, '{')
				.Append(indent + 1, "return;")
				.Append(indent, '}')
				.AppendLine()
			;

			// Output the state here...
			EmitStateContent(builder, indent, methodTarget, stateVarName, context, logger);

			// Call the .Log method.
			var eventId = methodTarget.EventId ?? SharedHelpers.GetNonRandomizedHashCode(methodTarget.MethodName);
			builder
				.Append(indent, Constants.Logging.LoggerFieldName, withNewLine: false)
				.AppendLine(".Log(")
				// Log level
				.Append(indent + 1, methodTarget.MSLevel.WithGlobal().WithComma())
				// Event Id
				.Append(indent + 1, "new ", withNewLine: false)
				.Append(Constants.Logging.MicrosoftExtensions.EventId.WithGlobal())
				.Append('(')
				.Append(eventId)
				.Append(", \"")
				.Append(methodTarget.MethodName)
				.AppendLine("\"),")
				// State
				.Append(indent + 1, stateVarName.WithComma())
				// Exception
				.Append(indent + 1, methodTarget.ExceptionParameter.OrNullKeyword().WithComma())
				// Message Template
				.Append(indent + 1, "// GENERATE CODEGEN ATTRIB")
				.Append(indent + 1, "static string (", withNewLine: false)
				.Append(Constants.Logging.MicrosoftExtensions.LoggerMessageState.WithGlobal())
				.Append(" s, ")
				.Append(Constants.System.Exception.WithGlobal().WithNullable())
				.AppendLine(" e) => ")
				.Append(indent + 1, "{")
				// TODO
				.Append(indent + 2, "return string.Empty;")
				.Append(indent + 1, "}")
				.Append(indent, ");")
			;
		}

		builder
			.Append(--indent, '}')
			.AppendLine()
		;
	}

	static void EmitStateContent(StringBuilder builder, int indent, LogTarget methodTarget, string stateVarName, SourceProductionContext context, IGenerationLogger? logger)
	{
		var reservationCount = methodTarget.ParameterCount;
		if (methodTarget.ExceptionParameter != null)
			reservationCount--;

		foreach (var parameter in methodTarget.Parameters)
		{
			if (parameter.Name == methodTarget.ExceptionParameter?.Name)
				// We need to skip over the exception parameter.
				continue;

			reservationCount++;

		}

		// Create the state variable,
		// and reserve the required number of variables.
		builder
			.Append(indent, "var ", withNewLine: false)
			.Append(stateVarName)
			.Append(" = new ")
			.Append(Constants.Logging.MicrosoftExtensions.LoggerMessageState.WithGlobal())
			.Append('.')
			.AppendLine("ThreadLocalState;")
			.Append(indent, stateVarName, withNewLine: false)
			.Append(".ReserveTagSpace(")
			.Append(reservationCount)
			.AppendLine(");")
			.AppendLine()
		;

	}
}
