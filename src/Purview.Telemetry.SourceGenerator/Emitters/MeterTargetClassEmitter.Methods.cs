using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator.Emitters;

partial class MeterTargetClassEmitter {
	static int EmitMethods(MeterGenerationTarget target, StringBuilder builder, int indent, SourceProductionContext context, IGenerationLogger? logger) {
		indent++;

		foreach (var methodTarget in target.InstrumentationMethods) {
			context.CancellationToken.ThrowIfCancellationRequested();

			EmitMethod(builder, indent, methodTarget, context, logger);
		}

		return --indent;
	}

	static void EmitMethod(StringBuilder builder, int indent, InstrumentMethodGenerationTarget methodTarget, SourceProductionContext context, IGenerationLogger? logger) {
		context.CancellationToken.ThrowIfCancellationRequested();

		builder
			.AppendLine()
			.Append(indent, "public ", withNewLine: false)
			.Append(methodTarget.ReturnType)
;

		if (methodTarget.IsNullableReturn) {
			builder.Append('?');
		}

		builder
			.Append(' ')
			.Append(methodTarget.MethodName)
			.Append('(')
		;

		var index = 0;
		foreach (var parameter in methodTarget.Parameters) {
			context.CancellationToken.ThrowIfCancellationRequested();

			if (parameter.IsMeasurement) {
				var type = methodTarget.InstrumentMeasurementType;
				if (methodTarget.MeasurementParameter!.IsMeasurement) {
					type = Constants.Metrics.SystemDiagnostics.Measurement.MakeGeneric(type);
				}

				if (methodTarget.MeasurementParameter!.IsIEnumerable) {
					type = Constants.System.IEnumerable.MakeGeneric(type);
				}

				type = Constants.System.Func.MakeGeneric(type);

				builder
					.Append(type)
				;
			}
			else {
				builder
					.Append(parameter.ParameterType)
				;
			}

			if (parameter.IsNullable) {
				builder.Append('?');
			}

			builder
				.Append(' ')
				.Append(parameter.ParameterName)
			;

			if (index < methodTarget.Parameters.Length - 1) {
				builder
					.Append(", ")
				;
			}

			index++;
		}

		builder
			.AppendLine(')')
			.Append(indent, '{')
		;

		if (methodTarget.IsObservable) {
			EmitObservableInstrumentBodyTest(builder, indent, methodTarget, logger);
		}

		var tagVariableName = EmitTags(builder, indent, methodTarget, context, logger);

		if (methodTarget.IsObservable) {
			EmitObservableInstrumentBody(builder, indent, methodTarget, tagVariableName, logger);
		}
		else {
			EmitInstrumentBody(builder, indent, methodTarget, tagVariableName, logger);
		}

		builder
			.Append(indent, '}')
		;
	}

	static void EmitObservableInstrumentBodyTest(StringBuilder builder, int indent, InstrumentMethodGenerationTarget method, IGenerationLogger? logger) {
		indent++;

		builder
			.Append(indent, "if (", withNewLine: false)
			.Append(method.FieldName)
			.AppendLine(" != null)")
			.Append(indent, '{')
		;

		if (method.InstrumentAttribute?.ThrowOnAlreadyInitialized?.Value == true) {
			builder
				.Append(indent + 1, "throw new ", withNewLine: false)
				.Append(Constants.System.Exception)
				.Append("(\"")
				.Append(method.MetricName)
				.AppendLine(" has already been initialized.\");")
			;
		}
		else {
			builder
				.Append(indent + 1, "return;")
			;
		}

		builder
			.Append(indent, '}')
			.AppendLine()
		;
	}

	static void EmitObservableInstrumentBody(StringBuilder builder, int indent, InstrumentMethodGenerationTarget method, string tagVariableName, IGenerationLogger? logger) {
		indent++;

		builder
			.Append(indent, method.FieldName, withNewLine: false)
			.Append(" = ")
			.Append(_meterFactoryFieldName)
			.Append(".Create")
			.Append(method.InstrumentAttribute!.InstrumentType)
			.Append('<')
			.Append(method.InstrumentMeasurementType)
			.Append(">(")
			.Append(method.MetricName.Wrap())
			.Append(", ")
			.Append(method.MeasurementParameter!.ParameterName)
			.Append(", tags: ")
			.Append(tagVariableName)
			.AppendLine(");")
		;
	}

	static void EmitInstrumentBody(StringBuilder builder, int indent, InstrumentMethodGenerationTarget methodTarget, string tagVariableName, IGenerationLogger? logger) {
		indent++;

		var instrumentMeasureMethodName = methodTarget.InstrumentAttribute!.InstrumentType == InstrumentTypes.Histogram
			? "Record"
			: "Add";

		builder
			.Append(indent, methodTarget.FieldName, withNewLine: false)
			.Append('.')
			.Append(instrumentMeasureMethodName)
			.Append('(')
			.Append(methodTarget.MeasurementParameter?.ParameterName ?? "1")
			.Append(", tags: ")
			.Append(tagVariableName)
			.AppendLine(");")
		;
	}

	static string EmitTags(StringBuilder builder, int indent, InstrumentMethodGenerationTarget methodTarget, SourceProductionContext context, IGenerationLogger? logger) {
		if (methodTarget.Tags.Length == 0) {
			return "null";
		}

		indent++;

		var tagVariableName = Utilities.LowercaseFirstChar(methodTarget.MethodName + "TagsList");
		var tagType = Constants.System.Dictionary
				.MakeGeneric(
					Constants.System.StringKeyword,
					Constants.System.ObjectKeyword + "?"
				);

		builder
			.Append(indent, tagType, withNewLine: false)
			.Append(' ')
			.Append(tagVariableName)
			.Append(" = new ")
			.Append(tagType)
			.AppendLine("();")
			.AppendLine()
		;

		foreach (var param in methodTarget.Tags) {
			if (param.SkipOnNullOrEmpty) {
				builder
					.Append(indent, "if (", withNewLine: false)
					.Append(param.ParameterName)
					.AppendLine(" != default)")
					.Append(indent, "{")
				;

				indent++;
			}

			builder
				.Append(indent, tagVariableName, withNewLine: false)
				.Append(".Add(")
				.Append(param.GeneratedName.Wrap())
				.Append(", ")
				.Append(param.ParameterName)
				.AppendLine(");")
			;

			if (param.SkipOnNullOrEmpty) {
				indent--;

				builder
					.Append(indent, "}")
					.AppendLine()
				;
			}
		}

		builder.AppendLine();

		return tagVariableName;
	}
}
