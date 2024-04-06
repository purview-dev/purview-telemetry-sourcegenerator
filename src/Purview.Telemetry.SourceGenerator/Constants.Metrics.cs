using System.Collections.Immutable;
using Purview.Telemetry.SourceGenerator.Records;
using Purview.Telemetry.SourceGenerator.Templates;

namespace Purview.Telemetry;

partial class Constants {
	static public class Metrics {
		public const string MeterInitializationMethod = "InitializeMeters";
		public const string MeterFactoryParameterName = "meterFactory";

		public const string InstrumentSeparatorDefault = ".";
		public const bool LowercaseInstrumentNameDefault = true;
		public const bool LowercaseTagKeysDefault = true;

		readonly static public TemplateInfo MeterGenerationAttribute = TemplateInfo.Create("Purview.Telemetry.Metrics.MeterGenerationAttribute");
		readonly static public TemplateInfo MeterAttribute = TemplateInfo.Create("Purview.Telemetry.Metrics.MeterAttribute");

		readonly static public TemplateInfo InstrumentMeasurementAttribute = TemplateInfo.Create("Purview.Telemetry.Metrics.InstrumentMeasurementAttribute");

		readonly static public TemplateInfo CounterAttribute = TemplateInfo.Create("Purview.Telemetry.Metrics.CounterAttribute");
		readonly static public TemplateInfo UpDownCounterAttribute = TemplateInfo.Create("Purview.Telemetry.Metrics.UpDownCounterAttribute");
		readonly static public TemplateInfo HistogramAttribute = TemplateInfo.Create("Purview.Telemetry.Metrics.HistogramAttribute");

		readonly static public TemplateInfo ObservableCounterAttribute = TemplateInfo.Create("Purview.Telemetry.Metrics.ObservableCounterAttribute");
		readonly static public TemplateInfo ObservableUpDownCounterAttribute = TemplateInfo.Create("Purview.Telemetry.Metrics.ObservableUpDownCounterAttribute");
		readonly static public TemplateInfo ObservableGaugeAttribute = TemplateInfo.Create("Purview.Telemetry.Metrics.ObservableGaugeAttribute");

		readonly static public TemplateInfo[] ValidInstrumentAttributes = [
			CounterAttribute,
			UpDownCounterAttribute,
			HistogramAttribute,
			ObservableCounterAttribute,
			ObservableUpDownCounterAttribute,
			ObservableGaugeAttribute,
		];

		readonly static public string[] ValidMeasurementKeywordTypes = [
			System.ByteKeyword,
			System.ShortKeyword,
			System.IntKeyword,
			System.LongKeyword,
			System.DoubleKeyword,
			System.FloatKeyword,
			System.DecimalKeyword
		];

		readonly static public TypeInfo[] ValidMeasurementTypes = [
			System.Byte,
			System.Int16,
			System.Int32,
			System.Int64,
			System.Double,
			System.Single,
			System.Decimal
		];

		readonly static public ImmutableDictionary<InstrumentTypes, TypeInfo> InstrumentTypeMap = new Dictionary<InstrumentTypes, TypeInfo> {
			{ InstrumentTypes.Counter, SystemDiagnostics.Counter },
			{ InstrumentTypes.UpDownCounter, SystemDiagnostics.UpDownCounter },
			{ InstrumentTypes.Histogram, SystemDiagnostics.Histogram },
			{ InstrumentTypes.ObservableCounter, SystemDiagnostics.ObservableCounter },
			{ InstrumentTypes.ObservableGauge, SystemDiagnostics.ObservableGauge },
			{ InstrumentTypes.ObservableUpDownCounter, SystemDiagnostics.ObservableUpDownCounter },
		}.ToImmutableDictionary();

		static public TemplateInfo[] GetTemplates() => [
			MeterGenerationAttribute,
			MeterAttribute,

			InstrumentMeasurementAttribute,

			CounterAttribute,
			UpDownCounterAttribute,
			HistogramAttribute,

			ObservableCounterAttribute,
			ObservableGaugeAttribute,
			ObservableUpDownCounterAttribute
		];

		static public class SystemDiagnostics {
			public const string ObservableCounterNoun = "ObservableCounter";
			public const string ObservableGaugeNoun = "ObservableGauge";
			public const string ObservableUpDownCounterNoun = "ObservableUpDownCounter";

			readonly static string SystemDiagnosticsMetricsNamespace = SystemDiagnosticsNamespace + ".Metrics";

			readonly static public TypeInfo Meter = TypeInfo.Create(SystemDiagnosticsMetricsNamespace + ".Meter");
			readonly static public TypeInfo IMeterFactory = TypeInfo.Create(SystemDiagnosticsMetricsNamespace + ".IMeterFactory");
			readonly static public TypeInfo MeterOptions = TypeInfo.Create(SystemDiagnosticsMetricsNamespace + ".MeterOptions");

			readonly static public TypeInfo Counter = TypeInfo.Create(SystemDiagnosticsMetricsNamespace + ".Counter"); // <>
			readonly static public TypeInfo UpDownCounter = TypeInfo.Create(SystemDiagnosticsMetricsNamespace + ".UpDownCounter"); // <>
			readonly static public TypeInfo Histogram = TypeInfo.Create(SystemDiagnosticsMetricsNamespace + ".Histogram"); // <>

			readonly static public TypeInfo ObservableCounter = TypeInfo.Create(SystemDiagnosticsMetricsNamespace + "." + ObservableCounterNoun); // <>
			readonly static public TypeInfo ObservableGauge = TypeInfo.Create(SystemDiagnosticsMetricsNamespace + "." + ObservableGaugeNoun); // <>
			readonly static public TypeInfo ObservableUpDownCounter = TypeInfo.Create(SystemDiagnosticsMetricsNamespace + "." + ObservableUpDownCounterNoun); // <>

			// Also supports IEnumerable<Measurement>.
			readonly static public TypeInfo Measurement = TypeInfo.Create(SystemDiagnosticsMetricsNamespace + ".Measurement"); // <>
		}
	}
}
