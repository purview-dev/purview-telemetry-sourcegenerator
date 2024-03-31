using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Purview.Telemetry.SourceGenerator.BuildTools;
using Purview.Telemetry.SourceGenerator.Helpers;
using Xunit.Abstractions;

namespace Purview.Telemetry.SourceGenerator;

abstract public class IncrementalSourceGeneratorTestBase<TGenerator> : SourceGeneratorTestBase<ISourceGenerator>
	where TGenerator : class, IIncrementalGenerator {
	protected IncrementalSourceGeneratorTestBase(ITestOutputHelper? testOutputHelper = null, bool throwOnLoggedOnError = true)
		: base(testOutputHelper, throwOnLoggedOnError) {
		ThrowOnLoggedOnError = throwOnLoggedOnError;
	}

	override protected ISourceGenerator Generator {
		get {
			var obj = Activator.CreateInstance<TGenerator>();
			ConfigureGenerator(obj);

			return obj.AsSourceGenerator();
		}
	}
}

abstract public class SourceGeneratorTestBase<TGenerator>(ITestOutputHelper? testOutputHelper = null, bool throwOnLoggedOnError = true)
	where TGenerator : ISourceGenerator {

	virtual protected bool ThrowOnLoggedOnError { get; set; } = throwOnLoggedOnError;

	virtual protected TGenerator Generator {
		get {
			var obj = Activator.CreateInstance<TGenerator>();
			ConfigureGenerator(obj);

			return obj;
		}
	}

	protected void ConfigureGenerator(object generator) {
		GuardGenerator(generator);

		if (generator is ILogSupport logging && testOutputHelper is not null) {
			logging.SetLogOutput((message, outputType) => {
				var prefix = outputType switch {
					OutputType.Debug => "DBG",
					OutputType.Diagnostic => "DIA",
					OutputType.Warning => "WRN",
					OutputType.Error => "ERR",
					_ => "???",
				};

				testOutputHelper.WriteLine(prefix + ": " + message);

				if (ThrowOnLoggedOnError) {
					outputType.Should().NotBe(OutputType.Error, message);
				}
			});
		}
	}

	static protected AdditionalText Text(string content, bool autoIncludeUsings = true)
	=> new InMemoryAdditionalText($"{Guid.NewGuid()}", (autoIncludeUsings ? TestHelpers.DefaultUsingSet : "") + content);

	static protected AdditionalText Text(string path, string content, bool autoIncludeUsings = true)
		=> new InMemoryAdditionalText(path, (autoIncludeUsings ? TestHelpers.DefaultUsingSet : "") + content);

	static protected AdditionalText[] Texts(params (string path, string content)[] pairs)
		=> pairs.Select(pair => new InMemoryAdditionalText(pair.path, pair.content)).ToArray();

	static protected AdditionalText[] Texts(params (string path, string content, (string key, string value)[]? options)[] pairs)
		=> pairs.Select(pair => new InMemoryAdditionalText(pair.path, pair.content, pair.options)).ToArray();

	static protected ImmutableDictionary<string, string> Options(params (string key, string value)[] pairs)
		=> pairs.ToImmutableDictionary(pair => pair.key, pair => pair.value);

	async protected Task<Compilation> GetCompilationAsync(GenerationResult generationResult, CancellationToken cancellationToken = default) {
		List<SyntaxTree> nodes = [];
		foreach (var tree in generationResult.Result.GeneratedTrees) {
			nodes.Add((await tree.GetRootAsync(cancellationToken)).SyntaxTree);
		}

		return generationResult.Compilation.AddSyntaxTrees(nodes);
	}

	protected Type GetType(GenerationResult result, string typeName) {
		var assembly = GetAssembly(result);

		return assembly.GetType(typeName, true).Should().NotBeNull().And.Subject;
	}

	protected Assembly GetAssembly(GenerationResult result) {
		Assembly assembly;
		using (var stream = new MemoryStream()) {
			var emitResult = result.Compilation.Emit(stream);
			emitResult.Should().NotBeNull();
			emitResult.Success.Should().BeTrue(string.Join("\n", emitResult.Diagnostics));

			assembly = Assembly.Load(stream.GetBuffer());
		}

		return assembly;
	}

	async protected Task<GenerationResult> GenerateAsync(
		string csharpDocument,
		AdditionalText[]? additionalTexts = null,
		ImmutableDictionary<string, string>? globalOptions = null,
		Func<Project, Project>? projectModifier = null,
		bool disableDependencyInjection = true,
		bool autoIncludeUsings = true,
		bool debugLog = true) {
		return await GenerateAsync(Text(csharpDocument, autoIncludeUsings: autoIncludeUsings), additionalTexts, globalOptions, projectModifier, disableDependencyInjection, debugLog);
	}

	async protected Task<GenerationResult> GenerateAsync(
		AdditionalText csharpDocument,
		AdditionalText[]? additionalTexts = null,
		ImmutableDictionary<string, string>? globalOptions = null,
		Func<Project, Project>? projectModifier = null,
		bool disableDependencyInjection = true,
		bool debugLog = true) {
		return await GenerateAsync([csharpDocument], additionalTexts, globalOptions, projectModifier, disableDependencyInjection, debugLog);
	}

	async protected Task<GenerationResult> GenerateAsync(
			AdditionalText[] csharpDocuments,
			AdditionalText[]? additionalTexts = null,
			ImmutableDictionary<string, string>? globalOptions = null,
			Func<Project, Project>? projectModifier = null,
			bool disableDependencyInjection = true,
			bool debugLog = true) {

		CSharpParseOptions parseOptions = new(kind: SourceCodeKind.Regular, documentationMode: DocumentationMode.Parse);

		globalOptions ??= ImmutableDictionary<string, string>.Empty;
		if (debugLog) {
			globalOptions = globalOptions.SetItem("purview_debug_log", "true");
		}

		var optionsProvider = TestAnalyzerConfigOptionsProvider
			.Empty
			.WithGlobalOptions(new TestAnalyzerConfigOptions(globalOptions));

		if (disableDependencyInjection) {
			csharpDocuments =
			[
				.. csharpDocuments,
				Text("[assembly: Purview.Telemetry.TelemetryGeneration(GenerateDependencyExtension = false)]", autoIncludeUsings: false)
			];
		}

		if (additionalTexts is not null && additionalTexts.Length != 0) {
			var map = ImmutableDictionary.CreateBuilder<object, AnalyzerConfigOptions>();
			foreach (var text in additionalTexts) {
				if (text is InMemoryAdditionalText mem) {
					map.Add(text, mem.GetOptions());
				}
			}

			optionsProvider = optionsProvider.WithAdditionalTreeOptions(map.ToImmutable());
		}

		GeneratorDriver driver = CSharpGeneratorDriver.Create([Generator], additionalTexts: additionalTexts, parseOptions: parseOptions, optionsProvider: optionsProvider);
		(var _, var compilation) = await ObtainProjectAndCompilationAsync(projectModifier, csharpDocuments);

		var result = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);
		if (testOutputHelper is object) {
			foreach (var d in diagnostics) {
				testOutputHelper.WriteLine(d.ToString());
			}
		}

		var runResult = result.GetRunResult();

		runResult.Results
			.Where(m => m.Exception != null)
			.Select(m => m.Exception)
			.Should()
			.BeEmpty();

		return new(runResult, diagnostics, outputCompilation);
	}

	static void GuardGenerator(object generator) {
		var generatorType = generator.GetType();

		if (!generatorType.IsDefined(typeof(GeneratorAttribute))) {
			throw new InvalidOperationException($"Type is not marked [Generator]: {generatorType}.");
		}
	}

	virtual protected bool ReferenceCore => true;

	async protected Task<(Project Project, Compilation Compilation)> ObtainProjectAndCompilationAsync(Func<Project, Project>? projectModifier = null, AdditionalText[]? csharpDocuments = null) {
		AdhocWorkspace workspace = new();
		var project = workspace.AddProject(typeof(SourceGeneratorTestBase<>).Namespace, LanguageNames.CSharp);

		project = project
			.WithCompilationOptions(project.CompilationOptions!.WithOutputKind(OutputKind.DynamicallyLinkedLibrary))
			.AddMetadataReference(MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location));

		if (csharpDocuments != null && csharpDocuments.Length > 0) {
			foreach (var csDoc in csharpDocuments) {
				project = project.AddDocument(csDoc.Path, csDoc.GetText()!).Project;
			}
		}

		project = SetupProject(project);

		if (ReferenceCore) {
			project = project
				.AddMetadataReference(MetadataReference.CreateFromFile(Assembly.Load("netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51").Location))

#if NET7_0_OR_GREATER
				.AddMetadataReference(MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location))
#endif

				.AddMetadataReference(MetadataReference.CreateFromFile(typeof(System.ComponentModel.EditorBrowsableAttribute).Assembly.Location))
				.AddMetadataReference(MetadataReference.CreateFromFile(typeof(IServiceProvider).Assembly.Location))

				//.AddMetadataReference(MetadataReference.CreateFromFile(typeof(LogGeneratedLevel).Assembly.Location))

				.AddMetadataReference(MetadataReference.CreateFromFile(typeof(Microsoft.Extensions.Logging.LogLevel).Assembly.Location))
				.AddMetadataReference(MetadataReference.CreateFromFile(typeof(System.Diagnostics.Activity).Assembly.Location))
				.AddMetadataReference(MetadataReference.CreateFromFile(typeof(System.Diagnostics.Metrics.Meter).Assembly.Location))
				.AddMetadataReference(MetadataReference.CreateFromFile(typeof(Microsoft.Extensions.DependencyInjection.IServiceCollection).Assembly.Location))
			;
		}

		project = projectModifier?.Invoke(project) ?? project;

		var compilation = await project.GetCompilationAsync();
		return (project, compilation!);
	}

	virtual protected Project SetupProject(Project project)
		=> project;
}

public record GenerationResult(GeneratorDriverRunResult Result, ImmutableArray<Diagnostic> Diagnostics, Compilation Compilation);
