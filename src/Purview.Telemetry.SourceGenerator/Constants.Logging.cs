using System.Collections.Immutable;

using Purview.Telemetry.SourceGenerator.Templates;

namespace Purview.Telemetry;

partial class Constants
{
	public static class Logging
	{
		public const int MaxNonExceptionParameters = 6;
		public const string DefaultLogLevelConstantName = "DEFAULT_LOGLEVEL";

		public const string LoggerFieldName = "_logger";

		public const int DefaultLevel = 2;

		public static readonly TemplateInfo LoggerGenerationAttribute = TemplateInfo.Create("Purview.Telemetry.Logging.LoggerGenerationAttribute");
		public static readonly TemplateInfo LoggerAttribute = TemplateInfo.Create("Purview.Telemetry.Logging.LoggerAttribute");
		public static readonly TemplateInfo LogAttribute = TemplateInfo.Create("Purview.Telemetry.Logging.LogAttribute");
		public static readonly TemplateInfo LogPrefixType = TemplateInfo.Create("Purview.Telemetry.Logging.LogPrefixType");

		public static readonly ImmutableDictionary<int, TypeInfo> LogLevelTypeMap = new Dictionary<int, TypeInfo> {
			{ 0, MicrosoftExtensions.LogLevel_Trace },
			{ 1, MicrosoftExtensions.LogLevel_Debug },
			{ 2, MicrosoftExtensions.LogLevel_Information },
			{ 3, MicrosoftExtensions.LogLevel_Warning },
			{ 4, MicrosoftExtensions.LogLevel_Error },
			{ 5, MicrosoftExtensions.LogLevel_Critical },
			{ 6, MicrosoftExtensions.LogLevel_None },
		}.ToImmutableDictionary();

		public static TemplateInfo[] GetTemplates() => [
			LoggerGenerationAttribute,
			LoggerAttribute,
			LogAttribute,
			LogPrefixType
		];

		public static class MicrosoftExtensions
		{
			public const string Namespace = "Microsoft.Extensions.Logging";

			public static readonly TypeInfo ILogger = TypeInfo.Create(Namespace + ".ILogger");
			public static readonly TypeInfo LoggerMessage = TypeInfo.Create(Namespace + ".LoggerMessage");
			public static readonly TypeInfo LogLevel = TypeInfo.Create(Namespace + ".LogLevel");
			public static readonly TypeInfo EventId = TypeInfo.Create(Namespace + ".EventId");

			public static readonly TypeInfo LogLevel_Trace = TypeInfo.Create(LogLevel.FullName + ".Trace");
			public static readonly TypeInfo LogLevel_Debug = TypeInfo.Create(LogLevel.FullName + ".Debug");
			public static readonly TypeInfo LogLevel_Information = TypeInfo.Create(LogLevel.FullName + ".Information");
			public static readonly TypeInfo LogLevel_Warning = TypeInfo.Create(LogLevel.FullName + ".Warning");
			public static readonly TypeInfo LogLevel_Error = TypeInfo.Create(LogLevel.FullName + ".Error");
			public static readonly TypeInfo LogLevel_Critical = TypeInfo.Create(LogLevel.FullName + ".Critical");
			public static readonly TypeInfo LogLevel_None = TypeInfo.Create(LogLevel.FullName + ".None");
		}
	}
}
