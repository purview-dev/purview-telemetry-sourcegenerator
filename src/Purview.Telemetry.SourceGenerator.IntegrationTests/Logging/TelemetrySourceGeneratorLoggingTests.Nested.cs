namespace Purview.Telemetry.SourceGenerator.Logging;

partial class TelemetrySourceGeneratorLoggingTests {
	[Theory]
	[InlineData("Testing.Test1")]
	[InlineData("Testing.Test1.Test2")]
	[InlineData("Testing.Test1.Test2.Test3")]
	async public Task Generate_GivenLoggerWithNamespaces_GeneratesScopedLogEntry(string @namespace) {
		// Arrange
		string basicLogger = @$"
using Purview.Telemetry.Logging;

namespace {@namespace};

[LoggerTarget]
public interface ITestLogger {{
	IDisposable Log(string stringParam, int intParam);
}}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.ScrubInlineGuids().UseHashedParameters(@namespace));
	}

	[Theory]
	[InlineData("Testing.Test1")]
	[InlineData("Testing.Test1.Test2")]
	[InlineData("Testing.Test1.Test2.Test3")]
	async public Task Generate_GivenLoggerWithNamespacesAndNestedClass_GeneratesScopedLogEntry(string @namespace) {
		// Arrange
		string basicLogger = @$"
using Purview.Telemetry.Logging;

namespace {@namespace};

public partial class TestClass1 {{
	[LoggerTarget]
	public interface ITestLogger {{
		IDisposable Log(string stringParam, int intParam);
	}}
}}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.ScrubInlineGuids().UseHashedParameters(@namespace));
	}

	[Theory]
	[InlineData("Testing.Test1")]
	[InlineData("Testing.Test1.Test2")]
	[InlineData("Testing.Test1.Test2.Test3")]
	async public Task Generate_GivenLoggerWithNamespacesAndNestedClasses_GeneratesScopedLogEntry(string @namespace) {
		// Arrange
		string basicLogger = @$"
using Purview.Telemetry.Logging;

namespace {@namespace};

public partial class TestClass1 {{
	public partial class TestClass2 {{
		public partial class TestClass3 {{
			[LoggerTarget]
			public interface ITestLogger {{
				IDisposable Log(string stringParam, int intParam);
			}}
		}}
	}}
}}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicLogger);

		// Assert
		await TestHelpers.Verify(generationResult, c => c.ScrubInlineGuids().UseHashedParameters(@namespace));
	}
}

