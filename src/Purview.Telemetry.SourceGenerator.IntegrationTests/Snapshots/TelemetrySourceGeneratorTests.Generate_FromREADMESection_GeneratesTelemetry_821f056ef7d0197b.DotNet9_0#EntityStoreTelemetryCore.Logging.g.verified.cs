﻿//HintName: EntityStoreTelemetryCore.Logging.g.cs
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

sealed partial class EntityStoreTelemetryCore : IEntityStoreTelemetry
{
	readonly Microsoft.Extensions.Logging.ILogger<IEntityStoreTelemetry> _logger;

	static readonly System.Action<Microsoft.Extensions.Logging.ILogger, int, string, System.Exception?> _logMessageAction = Microsoft.Extensions.Logging.LoggerMessage.Define<int, string>(Microsoft.Extensions.Logging.LogLevel.Information, new Microsoft.Extensions.Logging.EventId(1180592680, "LogMessage"), "LogMessage: EntityId = {EntityId}, UpdateState = {UpdateState}");
	static readonly System.Action<Microsoft.Extensions.Logging.ILogger, int, string, System.Exception?> _explicitInfoMessageAction = Microsoft.Extensions.Logging.LoggerMessage.Define<int, string>(Microsoft.Extensions.Logging.LogLevel.Information, new Microsoft.Extensions.Logging.EventId(1861353128, "ExplicitInfoMessage"), "ExplicitInfoMessage: EntityId = {EntityId}, UpdateState = {UpdateState}");

	[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public void LogMessage(int entityId, string updateState)
	{
		if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Information))
		{
			return;
		}

		_logMessageAction(_logger, entityId, updateState, null);
	}


	[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public void ExplicitInfoMessage(int entityId, string updateState)
	{
		if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Information))
		{
			return;
		}

		_explicitInfoMessageAction(_logger, entityId, updateState, null);
	}

}
