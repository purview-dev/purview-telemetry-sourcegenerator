﻿using System.Text;
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

			EmitMethod(builder, indent, methodTarget, context, logger);
		}

		return --indent;
	}

	static void EmitMethod(StringBuilder builder, int indent, LogTarget methodTarget, SourceProductionContext context, IGenerationLogger? logger)
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

		EmitParametersAsMethodArgumentList(methodTarget, builder, context);

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

		// Should always be state, because we'll use the messageFormat. And we'll generate one if
		// one doesn't exist...
		if (!methodTarget.IsScoped)
		{
			// First check if the the Log Level is enabled.
			// if (!_logger.IsEnabled(LogLevel.Information)))
			// { return; };
			// ...but only if it's not been scoped.
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
		}

		// Output the state here...
		EmitStateContent(builder, indent, methodTarget, stateVarName, context, logger);

		if (methodTarget.IsScoped)
		{
			// When it's messageFormat, args[] then
			// the extension method expands them to their name and value
			// based on the messageFormat. It also adds the {OriginalFormat}~
			// to the end of the array.
			// So in this instance, it might be an ordered
			// list...

			// return _logger.BeginScope(state);
			builder
				.Append(indent, "return ", withNewLine: false)
				.Append(Constants.Logging.LoggerFieldName)
				.Append(".BeginScope(")
				.Append(stateVarName)
				.AppendLine(");")
			;
		}
		else
		{
			// Call the .Log method.
			var eventId = methodTarget.EventId ?? SharedHelpers.GetNonRandomizedHashCode(methodTarget.MethodName);
			builder
				.Append(indent, Constants.Logging.LoggerFieldName, withNewLine: false)
				.AppendLine(".Log(")
				// Log level
				.Append(indent + 1, methodTarget.MSLevel.WithGlobal().WithComma())
				// Event Id
				.Append(indent + 1, "new (", withNewLine: false)
				.Append(eventId)
				.Append(", \"")
				.Append(methodTarget.LogName)
				.AppendLine("\"),")
				// State
				.Append(indent + 1, stateVarName.WithComma())
				// Exception
				.Append(indent + 1, methodTarget.ExceptionParameter.OrNullKeyword().WithComma())
				// Message Template
				.Append(indent + 1, "// GENERATE CODEGEN ATTRIB")
				.Append(indent + 1, "static string (s, e) =>")
				.Append(indent + 1, "{")
				// TODO
				.Append(indent + 1, "// TODO!!")
				.Append(indent + 2, "return string.Empty;")
				.Append(indent + 1, "}")
				.Append(indent, ");")
			;

			builder
				.AppendLine()
				.Append(indent, stateVarName, withNewLine: false)
				.AppendLine(".Clear();");
		}

		builder
			.Append(--indent, '}')
			.AppendLine()
		;
	}

	static void EmitStateContent(StringBuilder builder, int indent, LogTarget methodTarget, string stateVarName, SourceProductionContext context, IGenerationLogger? logger)
	{
		var reservationCount = methodTarget.ParameterCount + 1;
		if (methodTarget.ExceptionParameter != null)
			reservationCount--;

		//foreach (var parameter in methodTarget.Parameters)
		//{
		//	if (parameter.Name == methodTarget.ExceptionParameter?.Name)
		//		// We need to skip over the exception parameter.
		//		continue;

		//	reservationCount++;
		//}

		// Create the state variable,
		// and reserve the required number of variables.
		builder
			.Append(indent, "var ", withNewLine: false)
			.Append(stateVarName)
			.Append(" = ")
			.Append(Constants.Logging.MicrosoftExtensions.LoggerMessageHelper.WithGlobal())
			.Append('.')
			.AppendLine("ThreadLocalState;")
			.Append(indent, stateVarName, withNewLine: false)
			.Append(".ReserveTagSpace(")
			.Append(reservationCount)
			.AppendLine(");")
			.AppendLine()
		;

		// Original format is always at 0.
		OutputState(builder.WithIndent(indent), stateVarName, "{OriginalFormat}", methodTarget.MessageTemplate.Wrap(), 0);

		var idx = 0;
		foreach (var parameter in methodTarget.Parameters)
		{
			if (parameter.Name == methodTarget.ExceptionParameter?.Name)
				// We need to skip over the exception parameter.
				continue;

			OutputState(builder.WithIndent(indent), stateVarName, parameter.Name, parameter.Name, ++idx);
		}

		builder.AppendLine();

		static void OutputState(StringBuilder builder, string stateVarName, string propertyName, string value, int? index)
		{
			builder
				.Append(stateVarName)
				.Append('.')
			;

			if (index.HasValue)
			{
				builder
					.Append("TagArray[")
					.Append(index.Value)
					.Append("] = new(");
			}
			else
				builder.Append(".SetTag(");

			builder
				.Append(propertyName.Wrap().WithComma())
				.Append(' ')
				.Append(value)
				.AppendLine(");")
			;
		}
	}

	static void EmitParametersAsMethodArgumentList(LogTarget methodTarget, StringBuilder builder, SourceProductionContext context)
	{
		for (var i = 0; i < methodTarget.TotalParameterCount; i++)
		{
			context.CancellationToken.ThrowIfCancellationRequested();

			if (methodTarget.Parameters[i].IsComplexType
				|| methodTarget.Parameters[i].IsIEnumerable)
				builder.Append("global::");

			builder.Append(methodTarget.Parameters[i].FullyQualifiedType);

			if (methodTarget.Parameters[i].IsNullable)
				builder.Append('?');

			builder
				.Append(' ')
				.Append(methodTarget.Parameters[i].Name);

			if (i < methodTarget.TotalParameterCount - 1)
				builder.Append(", ");
		}
	}

}
