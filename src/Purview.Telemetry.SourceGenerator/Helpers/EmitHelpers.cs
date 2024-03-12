using System.Text;

namespace Purview.Telemetry.SourceGenerator.Helpers;

static class EmitHelpers {
	static public int EmitNamespaceStart(string? classNamespace, string[] parentClasses, StringBuilder builder, CancellationToken token) {
		token.ThrowIfCancellationRequested();

		var indent = 0;
		if (classNamespace != null) {
			builder
				.Append("namespace ")
				.AppendLine(classNamespace)
			;

			builder
				.Append('{')
				.AppendLine();

			indent++;
		}

		if (parentClasses.Length > 0) {
			foreach (var parentClass in parentClasses.Reverse()) {
				builder
					.Append(indent, "partial class ", withNewLine: false)
					.Append(parentClass)
					.AppendLine()
					.Append(indent, "{");

				indent++;
			}
		}

		return indent++;
	}

	static public void EmitNamespaceEnd(string[] parentClasses, int indent, StringBuilder builder, CancellationToken token) {
		token.ThrowIfCancellationRequested();

		if (parentClasses.Length > 0) {
			foreach (var parentClass in parentClasses) {
				builder
					.Append(--indent, '}')
				;
			}
		}

		if (parentClasses != null) {
			builder
				.Append('}')
			;
		}
	}

	static public int EmitClassStart(string className, string fullyQualifiedInterface, StringBuilder builder, int indent, CancellationToken token) {
		token.ThrowIfCancellationRequested();

		builder
			.Append(indent, "sealed partial class ", withNewLine: false)
			.Append(className)
			.Append(" : ")
			.Append(fullyQualifiedInterface)
			.AppendLine()
			.Append(indent, '{')
		;

		return indent;
	}

	static public void EmitClassEnd(StringBuilder builder, int indent) {
		builder
			.Append(indent, '}')
		;
	}
}
