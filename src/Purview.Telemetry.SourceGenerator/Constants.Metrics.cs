using System.Collections.Immutable;
using Purview.Telemetry.SourceGenerator.Records;
using Purview.Telemetry.SourceGenerator.Templates;

namespace Purview.Telemetry;

partial class Constants {
	static public class Metrics {
		readonly static public TemplateInfo MeterTargetAttribute = TemplateInfo.Create<Telemetry.Metrics.MeterTargetAttribute>();
		readonly static public TemplateInfo MeterGenerationAttribute = TemplateInfo.Create<Telemetry.Metrics.MeterGenerationAttribute>();

		readonly static public TemplateInfo InstrumentMeasurementAttribute = TemplateInfo.Create<Telemetry.Metrics.InstrumentMeasurementAttribute>();

		readonly static public TemplateInfo CounterTargetAttribute = TemplateInfo.Create<Telemetry.Metrics.CounterTargetAttribute>();
		readonly static public TemplateInfo UpDownCounterTargetAttribute = TemplateInfo.Create<Telemetry.Metrics.UpDownCounterTargetAttribute>();
		readonly static public TemplateInfo HistogramTargetAttribute = TemplateInfo.Create<Telemetry.Metrics.HistogramTargetAttribute>();

		readonly static public TemplateInfo ObservableCounterTargetAttribute = TemplateInfo.Create<Telemetry.Metrics.ObservableCounterTargetAttribute>();
		readonly static public TemplateInfo ObservableUpDownCounterTargetAttribute = TemplateInfo.Create<Telemetry.Metrics.ObservableUpDownCounterTargetAttribute>();
		readonly static public TemplateInfo ObservableGaugeTargetAttribute = TemplateInfo.Create<Telemetry.Metrics.ObservableGaugeTargetAttribute>();

		readonly static public TemplateInfo MeterExcludeAttribute = TemplateInfo.Create<Telemetry.Metrics.MeterExcludeAttribute>();

		readonly static public TemplateInfo[] ValidInstrumentAttributes = [
			CounterTargetAttribute,
			UpDownCounterTargetAttribute,
			HistogramTargetAttribute,
			ObservableCounterTargetAttribute,
			ObservableUpDownCounterTargetAttribute,
			ObservableGaugeTargetAttribute,
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

		static public TemplateInfo[] GetTemplates() => [
			MeterTargetAttribute,
			MeterGenerationAttribute,

			InstrumentMeasurementAttribute,

			CounterTargetAttribute,
			UpDownCounterTargetAttribute,
			HistogramTargetAttribute,

			ObservableCounterTargetAttribute,
			ObservableGaugeTargetAttribute,
			ObservableUpDownCounterTargetAttribute,

			MeterExcludeAttribute
		];
	}
}
