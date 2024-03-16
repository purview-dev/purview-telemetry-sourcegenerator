using Purview.Telemetry.SourceGenerator.Templates;

namespace Purview.Telemetry;

partial class Constants {
	static public class Activities {
		public const string ActivitySourceFieldName = "_activitySource";
		public const string ActivityVariableName = "activity";

		public const string ParentIdParameterName = "parentId";
		public const string StartTimeParameterName = "startTime";

		public const string StatusCode_Key = "otel.status_code";
		public const string StatusDescription_Key = "otel.status_description";

		public const string Tag_ExceptionEventName = "exception";
		public const string Tag_ExceptionType = "exception.type";
		public const string Tag_ExceptionMessage = "exception.message";
		public const string Tag_ExceptionStackTrace = "exception.stacktrace";

		readonly static public TemplateInfo ActivitySourceAttribute = TemplateInfo.Create<Telemetry.Activities.ActivitySourceAttribute>();
		readonly static public TemplateInfo ActivityTargetAttribute = TemplateInfo.Create<Telemetry.Activities.ActivityTargetAttribute>();
		readonly static public TemplateInfo ActivityGenAttribute = TemplateInfo.Create<Telemetry.Activities.ActivityGenAttribute>();
		readonly static public TemplateInfo ActivityEventAttribute = TemplateInfo.Create<Telemetry.Activities.ActivityEventAttribute>();
		readonly static public TemplateInfo ActivityGeneratedKind = TemplateInfo.Create<Telemetry.Activities.ActivityGeneratedKind>();
		readonly static public TemplateInfo ActivityExcludeAttribute = TemplateInfo.Create<Telemetry.Activities.ActivityExcludeAttribute>();
		readonly static public TemplateInfo BaggageAttribute = TemplateInfo.Create<Telemetry.Activities.BaggageAttribute>();

		static public TemplateInfo[] GetTemplates() => [
				ActivitySourceAttribute,
				ActivityTargetAttribute,
				ActivityGenAttribute,
				ActivityEventAttribute,
				ActivityGeneratedKind,
				ActivityExcludeAttribute,
				BaggageAttribute
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
