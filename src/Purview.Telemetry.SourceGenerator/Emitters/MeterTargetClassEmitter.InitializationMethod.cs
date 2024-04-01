using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class MeterTargetClassEmitter {
	static int EmitInitializationMethod(MeterTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		indent++;

		const string meterTagsVariableName = "meterTags";

		builder
			.AppendLine()
			.AggressiveInlining(indent)
			.Append(indent, "void ", withNewLine: false)
			.Append(Constants.Metrics.MeterInitializationMethod)
			.Append('(')
			.IfDefines("NET8_0_OR_GREATER", indent + 1, Constants.Metrics.SystemDiagnostics.IMeterFactory, " ", Constants.Metrics.MeterFactoryParameterName)
			.Append(indent, ')')
			.Append(indent, '{')
		;

		indent++;

		builder
			.Append(indent, "if (", withNewLine: false)
			.Append(_meterFieldName)
			.AppendLine(" != null)")
			.Append(indent, '{')
			.Append(indent + 1, "throw new ", withNewLine: false)
			.Append(Constants.System.Exception)
			.AppendLine("(\"The meters have already been initialized.\");")
			.Append(indent, '}')
			.AppendLine()
		;

		builder
			.AppendLine("#if NET8_0_OR_GREATER")
			.Append(indent, _dictionaryStringObject, withNewLine: false)
			.Append(' ')
			.Append(meterTagsVariableName)
			.Append(" = new ")
			.Append(_dictionaryStringObject)
			.AppendLine("();")
			.AppendLine()
		;

		builder
			.Append(indent, _partialMeterTagsMethod, withNewLine: false)
			.Append('(')
			.Append(meterTagsVariableName)
			.AppendLine(");")
			.AppendLine("#endif")
			.AppendLine()
		;

		builder
			.Append(indent, _meterFieldName, withNewLine: false)
			.AppendLine(" = ")
		;

		builder
			.AppendLine("#if NET8_0_OR_GREATER")
			.Append(indent + 1, Constants.Metrics.MeterFactoryParameterName, withNewLine: false)
			.Append(".Create(new ")
			.Append(Constants.Metrics.SystemDiagnostics.MeterOptions)
			.Append('(')
			.Append(target.MeterName!.Wrap())
			.AppendLine(')')
			.Append(indent + 1, '{')
			.Append(indent + 2, "Version = ", withNewLine: false)
			.AppendLine("null,") // We'll support version later.
			.Append(indent + 2, "Tags = ", withNewLine: false)
			.AppendLine(meterTagsVariableName)
			.Append(indent + 1, "});")
			.AppendLine("#else")
		;

		builder
			.Append(indent + 1, "new ", withNewLine: false)
			.Append(Constants.Metrics.SystemDiagnostics.Meter)
			.Append("(name: ")
			.Append(target.MeterName!.Wrap())
			.Append(", version: ")
			.Append("null") // We'll support version later.
			.AppendLine(");")
			.AppendLine("#endif")
		;

		foreach (var method in target.InstrumentationMethods) {
			EmitInitialiseInstrumentVariable(method, builder, indent, context, logger);
		}

		indent--;

		builder
			.Append(indent, '}')
		;

		return --indent;
	}

	static void EmitInitialiseInstrumentVariable(InstrumentTarget method, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		if (method.ErrorDiagnostics.Length > 0) {
			// Already raised diagnostic.
			return;
		}

		if (!method.TargetGenerationState.IsValid) {
			return;
		}

		if (!method.IsObservable) {
			var unit = method.InstrumentAttribute?.Unit?.Value?.Wrap() ?? Constants.System.NullKeyword;
			var description = method.InstrumentAttribute?.Description?.Value?.Wrap() ?? Constants.System.NullKeyword;
			var tagVariableType = Constants.System.Dictionary
					.MakeGeneric(Constants.System.StringKeyword, Constants.System.ObjectKeyword + "?");
			var tagVariableName = Utilities.LowercaseFirstChar(method.MethodName) + "Tags";

			builder
				.AppendLine()
				.AppendLine("#if !NET7_0")
				.AppendLine()
			;

			builder
				.Append(indent, tagVariableType, withNewLine: false)
				.Append(' ')
				.Append(tagVariableName)
				.Append(" = new ")
				.Append(tagVariableType)
				.AppendLine("();")
				.AppendLine()
				.Append(indent, method.TagPopulateMethodName, withNewLine: false)
				.Append('(')
				.Append(tagVariableName)
				.AppendLine(");")
				.AppendLine()
			;

			builder
				.AppendLine("#endif")
				.AppendLine()
			;

			builder
				.Append(indent, method.FieldName, withNewLine: false)
				.Append(" = ")
				.Append(_meterFieldName)
				.Append(".Create")
				.Append(method.InstrumentAttribute!.InstrumentType)
				.Append('<')
				.Append(method.InstrumentMeasurementType)
				.Append(">(name: ")
				.Append(method.MetricName.Wrap())
				.Append(", unit: ")
				.Append(unit)
				.Append(", description: ")
				.Append(description)
				.AppendLine()
				.AppendLine("#if !NET7_0")
				.Append(indent + 1, ", tags: ", withNewLine: false)
				.AppendLine(tagVariableName)
				.AppendLine("#endif")
				.Append(indent, ");")
			;
		}
	}
}
