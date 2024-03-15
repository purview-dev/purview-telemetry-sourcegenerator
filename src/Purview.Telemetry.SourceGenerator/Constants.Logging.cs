﻿using Purview.Telemetry.SourceGenerator.Templates;

namespace Purview.Telemetry;

partial class Constants {
	static public class Logging {
		public const int MaxNonExceptionParameters = 6;
		public const string DefaultLogLevelConstantName = "DEFAULT_LOGLEVEL";

		public const Telemetry.Logging.LogGeneratedLevel DefaultLevel = Telemetry.Logging.LogGeneratedLevel.Information;

		readonly static public TemplateInfo LogEntryAttribute = TemplateInfo.Create<Telemetry.Logging.LogEntryAttribute>();
		readonly static public TemplateInfo LogGeneratedLevel = TemplateInfo.Create<Telemetry.Logging.LogGeneratedLevel>();
		readonly static public TemplateInfo LoggerDefaultsAttribute = TemplateInfo.Create<Telemetry.Logging.LoggerDefaultsAttribute>();
		readonly static public TemplateInfo LoggerTargetAttribute = TemplateInfo.Create<Telemetry.Logging.LoggerTargetAttribute>();
		readonly static public TemplateInfo LogPrefixType = TemplateInfo.Create<Telemetry.Logging.LogPrefixType>();
		readonly static public TemplateInfo LogExcludeAttribute = TemplateInfo.Create<Telemetry.Logging.LogExcludeAttribute>();

		static public TemplateInfo[] GetTemplates() => [
			LogEntryAttribute,
			LogGeneratedLevel,
			LoggerDefaultsAttribute,
			LoggerTargetAttribute,
			LogPrefixType,
			LogExcludeAttribute
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
