﻿namespace Purview.Telemetry.SourceGenerator.Metrics;

partial class TelemetrySourceGeneratorMetricsTests {
	[Theory]
	[MemberData(nameof(NameUnitsDescriptorData))]
	async public Task Generate_GivenNameUnitsDescription_GeneratesMetrics(string attribute, string measurementParameter) {
		// Arrange
		string basicMetric = @$"
using Purview.Telemetry.Metrics;
using System.Diagnostics.Metrics;
using System.Collections.Generic;

namespace Testing;

[MeterTarget(""testing-meter"")]
public interface ITestMetrics {{
	[{attribute}]
	void Metric({measurementParameter}, [Tag]int intParam, [Tag]bool boolParam);
}}
";

		// Act
		GenerationResult generationResult = await GenerateAsync(basicMetric);

		// Assert
		await TestHelpers.Verify(generationResult, s => s.UseHashedParameters(attribute, measurementParameter));
	}

	static public TheoryData<string, string> NameUnitsDescriptorData {
		get {
			TheoryData<string, string> data = [];

			data.Add("CounterTarget(name: \"a-counter-name-param\", unit: \"cakes-param\", description: \"cake sales per-capita-param.\")", "int counterValue");
			data.Add("CounterTarget(Name = \"a-counter-name-property\", Unit = \"cakes-property\", Description = \"cake sales per-capita-property.\")", "byte counterValue");

			data.Add("UpDownCounterTarget(name: \"an-updown-counter-name-param\", unit: \"sponges-param\", description: \"sponge sales per-capita-param.\")", "int counterValue");
			data.Add("UpDownCounterTarget(Name = \"an-updown-counter-name-property\", Unit = \"sponges-property\", Description = \"sponge sales per-capita-property.\")", "byte counterValue");

			data.Add("ObservableCounterTarget(name: \"an-observablecounter-name-param\", unit: \"pie-param\", description: \"pie sales per-capita-param.\")", "Func<int> f");
			data.Add("ObservableCounterTarget(Name = \"an-observablecounter-name-property\", Unit = \"pie-property\", Description = \"pie sales per-capita-property.\")", "Func<byte> f");

			data.Add("ObservableGaugeTarget(name: \"an-observablegauge-name-param\", unit: \"biscuits-param\", description: \"biscuit ake sales per-capita-param.\")", "Func<Measurement<int>> f");
			data.Add("ObservableGaugeTarget(Name = \"an-observablegauge-name-property\", Unit = \"biscuits-property\", Description = \"biscuit sales per-capita-property.\")", "Func<Measurement<byte>> f");

			data.Add("ObservableUpDownCounterTarget(name: \"an-observableupdowncounter-name-param\", unit: \"beer-param\", description: \"beer sales per-capita-param.\")", "Func<IEnumerable<Measurement<int>>> f");
			data.Add("ObservableUpDownCounterTarget(Name = \"an-observableupdowncounter-name-property\", Unit = \"beer-property\", Description = \"beer sales per-capita-property.\")", "Func<IEnumerable<Measurement<byte>>> f");

			return data;
		}
	}
}
