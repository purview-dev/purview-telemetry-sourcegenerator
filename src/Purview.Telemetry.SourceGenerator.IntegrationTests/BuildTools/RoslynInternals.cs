using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Purview.Telemetry.SourceGenerator.BuildTools;

// these types borrowed from Roslyn's internal implementations of the abstract types
sealed class InMemoryAdditionalText(string path, string content, (string key, string value)[]? options = default) : AdditionalText {
	readonly SourceText _content = SourceText.From(content, Encoding.UTF8);

	public AnalyzerConfigOptions GetOptions() {
		if (options is null || options.Length == 0) {
			return InMemoryConfigOptions.Empty;
		}

		var builder = ImmutableDictionary.CreateBuilder<string, string>(AnalyzerConfigOptions.KeyComparer);
		foreach ((var key, var value) in options) {
			builder.Add(Literals.AdditionalFileMetadataPrefix + key, value);
		}

		return new InMemoryConfigOptions(builder.ToImmutable());
	}

	override public string Path { get; } = path;

	override public SourceText GetText(CancellationToken cancellationToken = default)
		=> _content;

	private class InMemoryConfigOptions(ImmutableDictionary<string, string> values) : AnalyzerConfigOptions {
		static public AnalyzerConfigOptions Empty { get; } = new InMemoryConfigOptions(ImmutableDictionary<string, string>.Empty);

		override public bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
			=> values.TryGetValue(key, out value);
	}
}

sealed internal class TestAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider {
	readonly ImmutableDictionary<object, AnalyzerConfigOptions> _treeDict;

	static public TestAnalyzerConfigOptionsProvider Empty { get; } = new(ImmutableDictionary<object, AnalyzerConfigOptions>.Empty, TestAnalyzerConfigOptions.Empty);

	internal TestAnalyzerConfigOptionsProvider(
		ImmutableDictionary<object, AnalyzerConfigOptions> treeDict,
		AnalyzerConfigOptions globalOptions) {
		_treeDict = treeDict;
		GlobalOptions = globalOptions;
	}

	override public AnalyzerConfigOptions GlobalOptions { get; }

	override public AnalyzerConfigOptions GetOptions(SyntaxTree tree)
		=> _treeDict.TryGetValue(tree, out var options) ? options : TestAnalyzerConfigOptions.Empty;

	override public AnalyzerConfigOptions GetOptions(AdditionalText textFile)
		=> _treeDict.TryGetValue(textFile, out var options)
			? options
			: TestAnalyzerConfigOptions.Empty;

	internal TestAnalyzerConfigOptionsProvider WithAdditionalTreeOptions(ImmutableDictionary<object, AnalyzerConfigOptions> treeDict)
		=> new(_treeDict.AddRange(treeDict), GlobalOptions);

	internal TestAnalyzerConfigOptionsProvider WithGlobalOptions(AnalyzerConfigOptions globalOptions)
		=> new(_treeDict, globalOptions);
}

sealed internal class TestAnalyzerConfigOptions(ImmutableDictionary<string, string> properties) : AnalyzerConfigOptions {
	static public TestAnalyzerConfigOptions Empty { get; } = new(ImmutableDictionary.Create<string, string>(KeyComparer));

	override public bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
		=> properties.TryGetValue(key, out value);
}
