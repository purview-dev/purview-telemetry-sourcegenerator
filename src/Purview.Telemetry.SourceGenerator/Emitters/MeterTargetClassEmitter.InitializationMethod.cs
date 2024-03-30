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
			.AgressiveInlining(indent)
			.Append(indent, "void ", withNewLine: false)
			.Append(Constants.Metrics.MeterInitializationMethod)
			.Append('(')
			.Append(Constants.Metrics.SystemDiagnostics.IMeterFactory)
			.Append(' ')
			.Append(Constants.Metrics.MeterFactoryParameterName)
			.AppendLine(')')
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
			.AppendLine("(\"The metrics have already been initialized.\");")
			.Append(indent, '}')
			.AppendLine()
		;

		builder
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
			.AppendLine()
		;

		builder
			.Append(indent, _meterFieldName, withNewLine: false)
			.Append(" = ")
			.Append(Constants.Metrics.MeterFactoryParameterName)
			.Append(".Create(new ")
			.Append(Constants.Metrics.SystemDiagnostics.MeterOptions)
			.Append('(')
			.Append(target.MeterName!.Wrap())
			.AppendLine(')')
			.Append(indent, '{')
			.Append(indent + 1, "Version = ", withNewLine: false)
			.AppendLine("null,")
			.Append(indent + 1, "Tags = ", withNewLine: false)
			.AppendLine(meterTagsVariableName)
			.Append(indent, "});")
			.AppendLine()
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
			var unit = method.InstrumentAttribute!.Unit?.Value?.Wrap();
			var description = method.InstrumentAttribute!.Description?.Value?.Wrap();

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
						.Append(unit ?? Constants.System.NullKeyword)
						.Append(", description: ")
						.Append(description ?? Constants.System.NullKeyword)
						.AppendLine(", tags: null);")
					;
		}
	}
}
