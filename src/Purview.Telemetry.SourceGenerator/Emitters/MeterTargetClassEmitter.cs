using System.Text;
using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Helpers;
using Purview.Telemetry.SourceGenerator.Records;
using Purview.Telemetry.SourceGenerator.Templates;

namespace Purview.Telemetry.SourceGenerator.Emitters;

static partial class MeterTargetClassEmitter {
	readonly static string _dictionaryStringObject = Constants.System.Dictionary.MakeGeneric(
		Constants.System.StringKeyword,
		Constants.System.ObjectKeyword.WithNull()
	);

	const string _meterFactoryFieldName = "_meterFactory";
	const string _meterVariableName = "meter";
	const string _partialMeterTagsMethod = "PopulateMeterTags";

	static public void GenerateImplementation(MeterGenerationTarget target, SourceProductionContext context, IGenerationLogger? logger) {
		StringBuilder builder = new();

		builder
			.AppendLine("#nullable enable")
			.AppendLine()
		;

		logger?.Debug($"Generating metric class for: {target.FullyQualifiedName}");

		var indent = EmitHelpers.EmitNamespaceStart(target.ClassNamespace, target.ParentClasses, builder, context.CancellationToken);
		indent = EmitHelpers.EmitClassStart(target.ClassNameToGenerate, target.FullyQualifiedInterfaceName, builder, indent, context.CancellationToken);

		indent = EmitFields(target, builder, indent, context, logger);
		indent = EmitCtor(target, builder, indent, context, logger);
		indent = EmitMethods(target, builder, indent, context, logger);

		EmitHelpers.EmitClassEnd(builder, indent);
		EmitHelpers.EmitNamespaceEnd(target.ParentClasses, indent, builder, context.CancellationToken);

		var sourceText = EmbeddedResources.Instance.AddHeader(builder.ToString());
		var hintName = $"{target.FullyQualifiedName}.Metric.g.cs";

		context.AddSource(hintName, Microsoft.CodeAnalysis.Text.SourceText.From(sourceText, Encoding.UTF8));
	}
}
