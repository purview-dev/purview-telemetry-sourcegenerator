using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using Purview.Telemetry.SourceGenerator.Templates;

namespace Purview.Telemetry.SourceGenerator;

static partial class TestHelpers {
	readonly static Assembly _ownerAssembly = typeof(TestHelpers).Assembly;
	readonly static string _namespaceRoot = typeof(TestHelpers).Namespace!;

	readonly static public string DefaultUsingSet = @$"
using System;
using Purview.Telemetry;

";

	static public string Wrap(this string value, char c = '"')
		=> c + value + c;

	static public string LoadEmbeddedResource(string folder, string resourceName) {
		resourceName = $"{_namespaceRoot}.Resources.{folder}.{resourceName}";

		var resourceStream = _ownerAssembly.GetManifestResourceStream(resourceName);
		if (resourceStream is null) {
			var existingResources = _ownerAssembly.GetManifestResourceNames();
			throw new ArgumentException($"Could not find embedded resource {resourceName}. Available resource names: {string.Join(", ", existingResources)}");
		}

		using StreamReader reader = new(resourceStream, Encoding.UTF8);

		return reader.ReadToEnd();
	}

	static public bool IsModifierPresent(MemberDeclarationSyntax member, SyntaxKind modifier)
		=> member.Modifiers.Any(m => m.IsKind(modifier));

	static public List<string> GetCasePermutations(string input) {
		List<string> result = [];

		if (string.IsNullOrWhiteSpace(input)) {
			result.Add(input);
			return result;
		}

		char currentChar = input[0];
		string remainder = input.Substring(1);
		List<string> remainderPermutations = GetCasePermutations(remainder);

		if (char.IsLetter(currentChar)) {
			foreach (string s in remainderPermutations) {
				result.Add(char.ToLower(currentChar, System.Globalization.CultureInfo.InvariantCulture) + s);
				result.Add(char.ToUpper(currentChar, System.Globalization.CultureInfo.InvariantCulture) + s);
			}
		}
		else {
			foreach (string s in remainderPermutations) {
				result.Add(currentChar + s);
			}
		}

		return result;
	}

	async static public Task Verify(GenerationResult generationResult,
		Action<SettingsTask>? config = null,
		bool validateNonEmptyDiagnostics = false,
		bool whenValidatingDiagnosticsIgnoreNonErrors = false,
		bool validationCompilation = true,
		bool autoVerifyTemplates = true) {

		var verifierTask = Verifier
			.Verify(generationResult.Result)
			.UseDirectory("Snapshots")
			.DisableRequireUniquePrefix()
			.DisableDateCounting()
			.UniqueForTargetFrameworkAndVersion(typeof(TestHelpers).Assembly)
			.ScrubInlineDateTimeOffsets("yyyy-MM-dd HH:mm:ss zzzz") // 2024-22-02 14:43:22 +00:00
			.AutoVerify(file => {
				if (autoVerifyTemplates) {
					foreach (TemplateInfo template in Constants.GetAllTemplates()) {
						string potentialName = $"#{template.Name}.g.";

						if (file.IndexOf(potentialName, StringComparison.Ordinal) > -1) {
							return true;
						}
					}
				}

				return false;
			})
		;

		config?.Invoke(verifierTask);

		//verifierTask = verifierTask.AutoVerify();

		await verifierTask;

		var diag = generationResult.Diagnostics.AsEnumerable();
		if (whenValidatingDiagnosticsIgnoreNonErrors) {
			diag = diag.Where(m => m.Severity == DiagnosticSeverity.Error);
		}

		if (validateNonEmptyDiagnostics) {
			diag.Should().NotBeEmpty();
		}
		else {
			diag.Should().BeEmpty();
		}

		if (!validationCompilation) {
			return;
		}

#if NET7_0_OR_GREATER
		await
#endif
			using MemoryStream ms = new();

		EmitResult result = generationResult.Compilation.Emit(ms);

		if (!result.Success) {
			result
				.Diagnostics
				.Where(m => !m.Id.StartsWith("TSG", StringComparison.Ordinal))
				.Should()
				.BeEmpty(string.Join(Environment.NewLine, result.Diagnostics.Select(d => d.ToString() + Environment.NewLine + "-----------------------------------------------------")));
		}
	}
}
