using Purview.Telemetry.SourceGenerator.Templates;
using Purview.Telemetry.SourceGenerator.Templates.Shims;

// This this as the non-SourceGenerator namespace...
namespace Purview.Telemetry;

static partial class Constants {
	public const string SystemDiagnosticsNamespace = "System.Diagnostics";

	readonly static string SystemDiagnosticsMetricsNamespace = SystemDiagnosticsNamespace + ".Metrics";

	static public TemplateInfo[] GetAllTemplates() {
		return [
			.. Shared.GetTemplates(),
			.. Activities.GetTemplates(),
			.. Logging.GetTemplates(),
			.. Metrics.GetTemplates()
		];
	}

	static public class Shared {
		public const string VoidKeyword = "void";

		public const string StringKeyword = "string";
		public const string BoolKeyword = "bool";

		public const string ByteKeyword = "byte";
		public const string ShortKeyword = "short";
		public const string IntKeyword = "int";
		public const string LongKeyword = "long";
		public const string FloatKeyword = "float";
		public const string DoubleKeyword = "double";
		public const string DecimalKeyword = "decimal";

		public const string Activity = nameof(Activity);

		//readonly static public TemplateInfo GenerateServiceCollectionExtensionAttribute = TemplateInfo.Create<GenerateServiceCollectionExtensionAttribute>();
		//readonly static public TemplateInfo SkipGenerationAttribute = TemplateInfo.Create<SkipGenerationAttribute>();
		//readonly static public TemplateInfo TelemetryGenerationAttribute = TemplateInfo.Create<TelemetryGenerationAttribute>();

		readonly static public TemplateInfo TelemetryDisposableWrapper = TemplateInfo.Create("Purview.Telemetry.TelemetryDisposableWrapper", attachHeader: false);

		readonly static public TypeInfo Func = TypeInfo.Create("System.Func"); // <>

		readonly static public TypeInfo String = TypeInfo.Create("System.String");
		readonly static public TypeInfo Byte = TypeInfo.Create("System.Byte");
		readonly static public TypeInfo Int16 = TypeInfo.Create("System.Int16");
		readonly static public TypeInfo Int32 = TypeInfo.Create("System.Int32");
		readonly static public TypeInfo Int64 = TypeInfo.Create("System.Int64");
		readonly static public TypeInfo Single = TypeInfo.Create("System.Single");
		readonly static public TypeInfo Double = TypeInfo.Create("System.Double");
		readonly static public TypeInfo Decimal = TypeInfo.Create("System.Decimal");

		readonly static public TypeInfo DateTimeOffset = TypeInfo.Create("System.DateTimeOffset");
		readonly static public TypeInfo IEnumerable = TypeInfo.Create("System.Collections.Generic.IEnumerable"); // <>
		readonly static public TypeInfo ConcurrentDictionary = TypeInfo.Create("System.Collections.Concurrent.ConcurrentDictionary"); // <>
		readonly static public TypeInfo IDisposable = TypeInfo.Create("System.IDisposable");
		readonly static public TypeInfo Exception = TypeInfo.Create("System.Exception");

		readonly static public TypeInfo TagList = TypeInfo.Create(SystemDiagnosticsNamespace + ".TagList");

		static public TemplateInfo[] GetTemplates()
			=> new[] {
				//GenerateServiceCollectionExtensionAttribute,
				//SkipGenerationAttribute,
				//TelemetryGenerationAttribute,

				TelemetryDisposableWrapper
			};
	}

	static public class Activities {
		public const string ActivitySourceFieldName = "_activitySource";
		public const string ActivityVariableName = "activity";

		public const string StatusCode_Key = "otel.status_code";
		public const string StatusDescription_Key = "otel.status_description";

		public const string Tag_ExceptionEventName = "exception";
		public const string Tag_ExceptionType = "exception.type";
		public const string Tag_ExceptionMessage = "exception.message";
		public const string Tag_ExceptionStackTrace = "exception.stacktrace";

		readonly static public TypeInfo Activity = TypeInfo.Create(SystemDiagnosticsNamespace + ".Activity");
		readonly static public TypeInfo ActivitySource = TypeInfo.Create(SystemDiagnosticsNamespace + ".ActivitySource");
		readonly static public TypeInfo ActivityEvent = TypeInfo.Create(SystemDiagnosticsNamespace + ".ActivityEvent");
		readonly static public TypeInfo ActivityContext = TypeInfo.Create(SystemDiagnosticsNamespace + ".ActivityContext");
		readonly static public TypeInfo ActivityKind = TypeInfo.Create(SystemDiagnosticsNamespace + ".ActivityKind");
		readonly static public TypeInfo ActivityTagsCollection = TypeInfo.Create(SystemDiagnosticsNamespace + ".ActivityTagsCollection");
		readonly static public TypeInfo ActivityTagIEnumerable = TypeInfo.Create("System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object?>>");

		readonly static public TypeInfo ActivityLink = TypeInfo.Create(SystemDiagnosticsNamespace + ".ActivityLink");
		readonly static public TypeInfo ActivityLinkIEnumerable = TypeInfo.Create("System.Collections.Generic.IEnumerable<System.Diagnostics.ActivityLink>");
		readonly static public TypeInfo ActivityLinkArray = TypeInfo.Create(SystemDiagnosticsNamespace + ".ActivityLink[]");

		readonly static public TemplateInfo ActivityAttribute = TemplateInfo.Create<Telemetry.Activities.ActivityAttribute>();
		readonly static public TemplateInfo ActivityGeneratedKind = TemplateInfo.Create<Telemetry.Activities.ActivityGeneratedKind>();
		readonly static public TemplateInfo ActivitySourceAttribute = TemplateInfo.Create<Telemetry.Activities.ActivitySourceAttribute>();
		//readonly static public TemplateInfo ActivityMetadataAttribute = TemplateInfo.Create<Telemetry.Activities.ActivityMetadataAttribute>();
		//readonly static public TemplateInfo BaggageAttribute = TemplateInfo.Create<Telemetry.Activities.BaggageAttribute>();
		//readonly static public TemplateInfo EventAttribute = TemplateInfo.Create<Telemetry.Activities.EventAttribute>();
		//readonly static public TemplateInfo TagAttribute = TemplateInfo.Create<Telemetry.Activities.TagAttribute>();

		//readonly static public TemplateInfo ExcludeFromActivityOrEventAttribute = TemplateInfo.Create<Telemetry.Activities.ExcludeFromActivityOrEventAttribute>();

		static public TemplateInfo[] GetTemplates()
			=> [
				ActivityAttribute,
				ActivityGeneratedKind,
				ActivitySourceAttribute,
				//ActivityMetadataAttribute,
				//BaggageAttribute,
				//EventAttribute,
				//TagAttribute,
				//ExcludeFromActivityOrEventAttribute
			];
	}

	static public class Logging {
		public const int MaxNonExceptionParameters = 6;

		//readonly static public TemplateInfo DefaultLogGenerationSettingsAttribute = TemplateInfo.Create<Telemetry.Logging.DefaultLogGenerationSettingsAttribute>();
		//readonly static public TemplateInfo LogAttribute = TemplateInfo.Create<Telemetry.Logging.LogAttribute>();
		readonly static public TemplateInfo LogGeneratedLevel = TemplateInfo.Create<Telemetry.Logging.LogGeneratedLevel>();
		readonly static public TemplateInfo LoggerTargetAttribute = TemplateInfo.Create<Telemetry.Logging.LoggerTargetAttribute>();
		//readonly static public TemplateInfo LogOutputAttribute = TemplateInfo.Create<Telemetry.Logging.LogOutputAttribute>();
		//readonly static public TemplateInfo ExcludeFromLogAttribute = TemplateInfo.Create<Telemetry.Logging.ExcludeFromLogAttribute>();

		static public class MicrosoftExtensions {
			public const string Namespace = "Microsoft.Extensions.Logging";

			readonly static public TypeInfo ILogger = TypeInfo.Create(Namespace + ".ILogger");
			readonly static public TypeInfo LoggerMessage = TypeInfo.Create(Namespace + ".LoggerMessage");
			readonly static public TypeInfo LogLevel = TypeInfo.Create(Namespace + ".LogLevel");
			readonly static public TypeInfo EventId = TypeInfo.Create(Namespace + ".EventId");

			readonly static public TypeInfo LogLevel_Trace = TypeInfo.Create(LogLevel.FullName + ".Trace");
			readonly static public TypeInfo LogLevel_Debug = TypeInfo.Create(LogLevel.FullName + ".Debug");
			readonly static public TypeInfo LogLevel_Information = TypeInfo.Create(LogLevel.FullName + ".Information");
			readonly static public TypeInfo LogLevel_Warning = TypeInfo.Create(LogLevel.FullName + ".Warning");
			readonly static public TypeInfo LogLevel_Error = TypeInfo.Create(LogLevel.FullName + ".Error");
			readonly static public TypeInfo LogLevel_Critical = TypeInfo.Create(LogLevel.FullName + ".Critical");
			readonly static public TypeInfo LogLevel_None = TypeInfo.Create(LogLevel.FullName + ".None");
		}

		static public TemplateInfo[] GetTemplates() => [
			LogGeneratedLevel,
			LoggerTargetAttribute,
		//DefaultLogGenerationSettingsAttribute,
		//LogAttribute,
		//LogGeneratedLevelAttribute,
		//LogOutputAttribute,
		//ExcludeFromLogAttribute
		];
	}

	static public class Metrics {
		public const string MeterFieldName = "_meter";

		//readonly static public TemplateInfo MeterAttribute = TemplateInfo.Create<Telemetry.Metrics.MeterAttribute>();

		//readonly static public TemplateInfo MeasurementValueAttribute = TemplateInfo.Create<Telemetry.Metrics.MeasurementValueAttribute>();
		//readonly static public TemplateInfo MeasurementTagAttribute = TemplateInfo.Create<Telemetry.Metrics.MeasurementTagAttribute>();

		//readonly static public TemplateInfo CounterAttribute = TemplateInfo.Create<Telemetry.Metrics.CounterAttribute>();
		//readonly static public TemplateInfo UpDownCounterAttribute = TemplateInfo.Create<Telemetry.Metrics.UpDownCounterAttribute>();
		//readonly static public TemplateInfo HistogramAttribute = TemplateInfo.Create<Telemetry.Metrics.HistogramAttribute>();

		//readonly static public TemplateInfo ObservableCounterAttribute = TemplateInfo.Create<Telemetry.Metrics.ObservableCounterAttribute>();
		//readonly static public TemplateInfo ObservableGaugeAttribute = TemplateInfo.Create<Telemetry.Metrics.ObservableGaugeAttribute>();
		//readonly static public TemplateInfo ObservableUpDownCounterAttribute = TemplateInfo.Create<Telemetry.Metrics.ObservableUpDownCounterAttribute>();

		//readonly static public TemplateInfo ExcludeFromMetricAttribute = TemplateInfo.Create<Telemetry.Metrics.ExcludeFromMetricAttribute>();

		readonly static public TypeInfo Meter = TypeInfo.Create(SystemDiagnosticsMetricsNamespace + ".Meter");

		readonly static public TypeInfo Counter = TypeInfo.Create(SystemDiagnosticsMetricsNamespace + ".Counter"); // <>
		readonly static public TypeInfo UpDownCounter = TypeInfo.Create(SystemDiagnosticsMetricsNamespace + ".UpDownCounter"); // <>
		readonly static public TypeInfo Histogram = TypeInfo.Create(SystemDiagnosticsMetricsNamespace + ".Histogram"); // <>

		// Also supports IEnumerable<Measurement>.
		readonly static public TypeInfo Measurement = TypeInfo.Create(SystemDiagnosticsMetricsNamespace + ".Measurement"); // <>

		readonly static public TemplateInfo[] ValidMetricTypes =
		[
		//	CounterAttribute,
		//	UpDownCounterAttribute,
		//	HistogramAttribute,

		//	ObservableCounterAttribute,
		//	ObservableGaugeAttribute,
		//	ObservableUpDownCounterAttribute
		];

		readonly static public Dictionary<MetricTypes, TypeInfo> MetricTypeToInfoMap = new() {
			{ MetricTypes.Counter, Counter },
			{ MetricTypes.UpDownCounter, UpDownCounter },
			{ MetricTypes.Histogram, Histogram },
		};

		//readonly static public Dictionary<string, MetricTypes> MetricTypeMap = new()  {
		//	{ CounterAttribute.FullName, MetricTypes.Counter },
		//	{ CounterAttribute.Name, MetricTypes.Counter },

		//	{ UpDownCounterAttribute.FullName, MetricTypes.UpDownCounter },
		//	{ UpDownCounterAttribute.Name, MetricTypes.UpDownCounter },

		//	{ HistogramAttribute.FullName, MetricTypes.Histogram },
		//	{ HistogramAttribute.Name, MetricTypes.Histogram },

		//	{ ObservableCounterAttribute.FullName, MetricTypes.ObservableCounter },
		//	{ ObservableCounterAttribute.Name, MetricTypes.ObservableCounter },

		//	{ ObservableGaugeAttribute.FullName, MetricTypes.ObservableGauge },
		//	{ ObservableGaugeAttribute.Name, MetricTypes.ObservableGauge },

		//	{ ObservableUpDownCounterAttribute.FullName, MetricTypes.ObservableUpDownCounter },
		//	{ ObservableUpDownCounterAttribute.Name, MetricTypes.ObservableUpDownCounter }
		//};

		//readonly static public string[] ValidMeasurementKeywordTypes = new[] {
		//	Shared.ByteKeyword,
		//	Shared.ShortKeyword,
		//	Shared.IntKeyword,
		//	Shared.LongKeyword,
		//	Shared.FloatKeyword,
		//	Shared.DoubleKeyword,
		//	Shared.DecimalKeyword
		//};

		//readonly static public TypeInfo[] ValidMeasurementTypes = new[] {
		//	Shared.Byte,
		//	Shared.Int16,
		//	Shared.Int32,
		//	Shared.Int64,
		//	Shared.Single,
		//	Shared.Double,
		//	Shared.Decimal
		//};

		static public TemplateInfo[] GetTemplates() => [
		//	MeterAttribute,

		//	MeasurementTagAttribute,
		//	MeasurementValueAttribute,

		//	CounterAttribute,
		//	UpDownCounterAttribute,
		//	HistogramAttribute,

		//	ObservableCounterAttribute,
		//	ObservableGaugeAttribute,
		//	ObservableUpDownCounterAttribute,

		//	ExcludeFromMetricAttribute
		];
	}

	static public class Diagnostics {
		static public class Activity {
			public const string Usage = nameof(Activity) + "." + nameof(Usage);
		}
	}
}
