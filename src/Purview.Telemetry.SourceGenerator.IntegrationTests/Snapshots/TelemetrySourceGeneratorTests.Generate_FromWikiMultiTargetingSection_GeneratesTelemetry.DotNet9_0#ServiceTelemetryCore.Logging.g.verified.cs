﻿//HintName: ServiceTelemetryCore.Logging.g.cs
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Purview.Telemetry.SourceGenerator
//     on {Scrubbed}.
//
//     Changes to this file may cause incorrect behaviour and will be lost
//     when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable 1591 // publicly visible type or member must be documented

#nullable enable

sealed partial class ServiceTelemetryCore : IServiceTelemetry
{
	readonly Microsoft.Extensions.Logging.ILogger<IServiceTelemetry> _logger;

	static readonly System.Action<Microsoft.Extensions.Logging.ILogger, int, string, System.Exception?> _processingEntityAction = Microsoft.Extensions.Logging.LoggerMessage.Define<int, string>(Microsoft.Extensions.Logging.LogLevel.Information, default, "ServiceTelemetry.ProcessingEntity: entityId: {EntityId}, property1: {Property1}");

	[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public void ProcessingEntity(int entityId, string property1)
	{
		if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Information))
		{
			return;
		}

		_processingEntityAction(_logger, entityId, property1, null);
	}

}
