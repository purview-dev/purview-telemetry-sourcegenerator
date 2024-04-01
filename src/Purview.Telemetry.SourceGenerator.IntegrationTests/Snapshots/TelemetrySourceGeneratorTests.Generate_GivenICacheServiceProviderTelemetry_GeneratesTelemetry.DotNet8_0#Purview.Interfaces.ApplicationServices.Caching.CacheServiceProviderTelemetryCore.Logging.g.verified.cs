﻿//HintName: Purview.Interfaces.ApplicationServices.Caching.CacheServiceProviderTelemetryCore.Logging.g.cs
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

namespace Purview.Interfaces.ApplicationServices.Caching
{
	sealed partial class CacheServiceProviderTelemetryCore : Purview.Interfaces.ApplicationServices.Caching.ICacheServiceProviderTelemetry
	{
		readonly Microsoft.Extensions.Logging.ILogger<Purview.Interfaces.ApplicationServices.Caching.ICacheServiceProviderTelemetry> _logger = default!;

		static readonly System.Action<Microsoft.Extensions.Logging.ILogger, System.Int32, System.Exception?> _failedToDeserializePayloadAction = Microsoft.Extensions.Logging.LoggerMessage.Define<System.Int32>(Microsoft.Extensions.Logging.LogLevel.Information, default, "CacheServiceProviderTelemetry.FailedToDeserializePayload: dataLength: {DataLength}");
		static readonly System.Action<Microsoft.Extensions.Logging.ILogger, System.String, System.Exception?> _failedToGetFromCacheAction = Microsoft.Extensions.Logging.LoggerMessage.Define<System.String>(Microsoft.Extensions.Logging.LogLevel.Information, default, "CacheServiceProviderTelemetry.FailedToGetFromCache: key: {Key}");
		static readonly System.Action<Microsoft.Extensions.Logging.ILogger, System.String, System.Exception?> _failedToRefreshAction = Microsoft.Extensions.Logging.LoggerMessage.Define<System.String>(Microsoft.Extensions.Logging.LogLevel.Information, default, "CacheServiceProviderTelemetry.FailedToRefresh: cacheKey: {CacheKey}");
		static readonly System.Action<Microsoft.Extensions.Logging.ILogger, System.String, System.Exception?> _failedToRemoveAction = Microsoft.Extensions.Logging.LoggerMessage.Define<System.String>(Microsoft.Extensions.Logging.LogLevel.Information, default, "CacheServiceProviderTelemetry.FailedToRemove: key: {Key}");
		static readonly System.Action<Microsoft.Extensions.Logging.ILogger, System.String?, System.Exception?> _failedToSerializePayloadAction = Microsoft.Extensions.Logging.LoggerMessage.Define<System.String?>(Microsoft.Extensions.Logging.LogLevel.Information, default, "CacheServiceProviderTelemetry.FailedToSerializePayload: fullName: {FullName}");
		static readonly System.Action<Microsoft.Extensions.Logging.ILogger, System.String, System.Exception?> _failedToSetValueInCacheAction = Microsoft.Extensions.Logging.LoggerMessage.Define<System.String>(Microsoft.Extensions.Logging.LogLevel.Information, default, "CacheServiceProviderTelemetry.FailedToSetValueInCache: key: {Key}");
		static readonly System.Action<Microsoft.Extensions.Logging.ILogger, System.String?, System.Boolean, System.Exception?> _usingDistributedCacheAction = Microsoft.Extensions.Logging.LoggerMessage.Define<System.String?, System.Boolean>(Microsoft.Extensions.Logging.LogLevel.Information, default, "CacheServiceProviderTelemetry.UsingDistributedCache: fullName: {FullName}, isNullCache: {IsNullCache}");

		public CacheServiceProviderTelemetryCore(Microsoft.Extensions.Logging.ILogger<Purview.Interfaces.ApplicationServices.Caching.ICacheServiceProviderTelemetry> logger)
		{
			_logger = logger;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void FailedToDeserializePayload(System.Int32 dataLength, System.Exception ex)
		{
			if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Information))
			{
				return;
			}

			_failedToDeserializePayloadAction(_logger, dataLength, ex);
		}


		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void FailedToGetFromCache(System.String key, System.Exception ex)
		{
			if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Information))
			{
				return;
			}

			_failedToGetFromCacheAction(_logger, key, ex);
		}


		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void FailedToRefresh(System.String cacheKey, System.Exception ex)
		{
			if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Information))
			{
				return;
			}

			_failedToRefreshAction(_logger, cacheKey, ex);
		}


		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void FailedToRemove(System.String key, System.Exception ex)
		{
			if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Information))
			{
				return;
			}

			_failedToRemoveAction(_logger, key, ex);
		}


		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void FailedToSerializePayload(System.String? fullName, System.Exception ex)
		{
			if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Information))
			{
				return;
			}

			_failedToSerializePayloadAction(_logger, fullName, ex);
		}


		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void FailedToSetValueInCache(System.String key, System.Exception ex)
		{
			if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Information))
			{
				return;
			}

			_failedToSetValueInCacheAction(_logger, key, ex);
		}


		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void UsingDistributedCache(System.String? fullName, System.Boolean isNullCache)
		{
			if (!_logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Information))
			{
				return;
			}

			_usingDistributedCacheAction(_logger, fullName, isNullCache, null);
		}

	}
}
