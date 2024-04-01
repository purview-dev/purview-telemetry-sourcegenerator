using System.Collections.Immutable;
using Purview.Telemetry.SourceGenerator.Templates;

namespace Purview.Telemetry;

partial class Constants {
	static public class Logging {
		public const int MaxNonExceptionParameters = 6;
		public const string DefaultLogLevelConstantName = "DEFAULT_LOGLEVEL";

		public const string LoggerFieldName = "_logger";

		public const int DefaultLevel = 2;

		readonly static public TemplateInfo LoggerGenerationAttribute = TemplateInfo.Create("Purview.Telemetry.Logging.LoggerGenerationAttribute");
		readonly static public TemplateInfo LoggerAttribute = TemplateInfo.Create("Purview.Telemetry.Logging.LoggerAttribute");
		readonly static public TemplateInfo LogAttribute = TemplateInfo.Create("Purview.Telemetry.Logging.LogAttribute");
		readonly static public TemplateInfo LogPrefixType = TemplateInfo.Create("Purview.Telemetry.Logging.LogPrefixType");

		readonly static public ImmutableDictionary<int, TypeInfo> LogLevelTypeMap = new Dictionary<int, TypeInfo> {
			{ 0, MicrosoftExtensions.LogLevel_Trace },
			{ 1, MicrosoftExtensions.LogLevel_Debug },
			{ 2, MicrosoftExtensions.LogLevel_Information },
			{ 3, MicrosoftExtensions.LogLevel_Warning },
			{ 4, MicrosoftExtensions.LogLevel_Error },
			{ 5, MicrosoftExtensions.LogLevel_Critical },
			{ 6, MicrosoftExtensions.LogLevel_None },
		}.ToImmutableDictionary();

		static public TemplateInfo[] GetTemplates() => [
			LoggerGenerationAttribute,
			LoggerAttribute,
			LogAttribute,
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
