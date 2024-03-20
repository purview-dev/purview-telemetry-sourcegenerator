using Purview.Telemetry.SourceGenerator.Templates;

namespace Purview.Telemetry;

partial class Constants {
	static public class Logging {
		public const int MaxNonExceptionParameters = 6;
		public const string DefaultLogLevelConstantName = "DEFAULT_LOGLEVEL";

		public const string LoggerFieldName = "_logger";

		public const Telemetry.Logging.LogGeneratedLevel DefaultLevel = Telemetry.Logging.LogGeneratedLevel.Information;

		readonly static public TemplateInfo LoggerGenerationAttribute = TemplateInfo.Create<Telemetry.Logging.LoggerGenerationAttribute>();
		readonly static public TemplateInfo LoggerAttribute = TemplateInfo.Create<Telemetry.Logging.LoggerAttribute>();
		readonly static public TemplateInfo LogAttribute = TemplateInfo.Create<Telemetry.Logging.LogAttribute>();
		readonly static public TemplateInfo LogGeneratedLevel = TemplateInfo.Create<Telemetry.Logging.LogGeneratedLevel>();
		readonly static public TemplateInfo LogPrefixType = TemplateInfo.Create<Telemetry.Logging.LogPrefixType>();

		static public TemplateInfo[] GetTemplates() => [
			LoggerGenerationAttribute,
			LoggerAttribute,
			LogAttribute,
			LogGeneratedLevel,
			LogPrefixType
		];

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
	}
}
