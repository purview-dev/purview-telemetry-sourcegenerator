using Purview.Telemetry.SourceGenerator.Templates;

namespace Purview.Telemetry;

partial class Constants {
	static public partial class Activities {
		static public string DefaultActivitySourceName { get; set; } = "purview";

		public const string ActivitySourceFieldName = "_activitySource";
		public const string ActivityVariableName = "activity";

		public const string ParentIdParameterName = "parentId";
		public const string StartTimeParameterName = "startTime";

		public const string StatusCode_Key = "otel.status_code";
		public const string StatusDescription_Key = "otel.status_description";

		public const string Tag_ExceptionEventName = "exception";
		public const string Tag_ExceptionEscaped = "exception.escaped";
		public const string Tag_ExceptionType = "exception.type";
		public const string Tag_ExceptionMessage = "exception.message";
		public const string Tag_ExceptionStackTrace = "exception.stacktrace";

		public const string RecordExceptionMethodName = "RecordExceptionInternal";

		readonly static public TemplateInfo ActivitySourceGenerationAttribute = TemplateInfo.Create<Telemetry.Activities.ActivitySourceGenerationAttribute>();
		readonly static public TemplateInfo ActivitySourceAttribute = TemplateInfo.Create<Telemetry.Activities.ActivitySourceAttribute>();
		readonly static public TemplateInfo ActivityAttribute = TemplateInfo.Create<Telemetry.Activities.ActivityAttribute>();
		readonly static public TemplateInfo EventAttribute = TemplateInfo.Create<Telemetry.Activities.EventAttribute>();
		readonly static public TemplateInfo ContextAttribute = TemplateInfo.Create<Telemetry.Activities.ContextAttribute>();
		readonly static public TemplateInfo BaggageAttribute = TemplateInfo.Create<Telemetry.Activities.BaggageAttribute>();
		readonly static public TemplateInfo EscapedAttribute = TemplateInfo.Create<Telemetry.Activities.EscapedAttribute>();
		readonly static public TemplateInfo ActivityGeneratedKind = TemplateInfo.Create<Telemetry.Activities.ActivityGeneratedKind>();

		static public TemplateInfo[] GetTemplates() => [
				ActivitySourceGenerationAttribute,
				ActivitySourceAttribute,
				ActivityAttribute,
				EventAttribute,
				ContextAttribute,
				BaggageAttribute,
				EscapedAttribute,
				ActivityGeneratedKind
			];

		static public class SystemDiagnostics {
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
		}
	}
}
