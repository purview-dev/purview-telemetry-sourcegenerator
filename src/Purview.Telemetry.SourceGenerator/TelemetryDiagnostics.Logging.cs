﻿using Microsoft.CodeAnalysis;
using Purview.Telemetry.SourceGenerator.Records;

namespace Purview.Telemetry.SourceGenerator;

partial class TelemetryDiagnostics
{
	// Start at 2000
	public static class Logging
	{
		public static readonly TelemetryDiagnosticDescriptor MultipleExceptionsDefined = new(
			Id: "TSG2000",
			Title: "Too many exception parameters",
			Description: "Only a single exceptions parameter is permitted.",
			Category: Constants.Diagnostics.Logging.Usage,
			Severity: DiagnosticSeverity.Error
		);

		public static readonly TelemetryDiagnosticDescriptor MaximumLogEntryParametersExceeded = new(
			Id: "TSG2001",
			Title: "More than 6 parameters",
			Description: $"The maximum number of parameters (excluding optional Exception) is {Constants.Logging.MaxNonExceptionParameters}",
			Category: Constants.Diagnostics.Logging.Usage,
			Severity: DiagnosticSeverity.Error
		);

		public static readonly TelemetryDiagnosticDescriptor InferringErrorLogLevel = new(
			Id: "TSG2002",
			Title: "Inferring error log level",
			Description: "Because an exception parameter was defined and no log level was defined the level was inferred to be Error. Consider explicitly defining the required level.",
			Category: Constants.Diagnostics.Logging.Usage,
			Severity: DiagnosticSeverity.Info
		);

		public static readonly TelemetryDiagnosticDescriptor MSLoggingNotReferencedButAttemptedUse = new(
			Id: "TSG2003",
			Title: "Could not find a reference to Microsoft.Extensions.Logging.ILogger, but a generation was attempted.",
			Description: "No reference was found for the ILogger type, no log generation is possible. Add a reference to the appropriate NuGet package, such as Microsoft.Extensions.Logging.",
			Category: Constants.Diagnostics.Logging.Usage,
			Severity: DiagnosticSeverity.Error
		);

		public static readonly TelemetryDiagnosticDescriptor MSLoggingNotReferenced = new(
			Id: "TSG2004",
			Title: "Could not find a reference to Microsoft.Extensions.Logging.ILogger, skipping log attributes.",
			Description: "No reference was found for the ILogger type, no log generation is possible so no logging attributes will be added. Add a reference to the appropriate NuGet package, such as Microsoft.Extensions.Logging.",
			Category: Constants.Diagnostics.Logging.Usage,
			Severity: DiagnosticSeverity.Info
		);
	}
}
