﻿//HintName: EntityStoreTelemetryCoreDIExtension.DependencyInjection.g.cs
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

namespace Microsoft.Extensions.DependencyInjection
{
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	static class EntityStoreTelemetryCoreDIExtension
	{
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		static public Microsoft.Extensions.DependencyInjection.IServiceCollection AddEntityStoreTelemetry(this Microsoft.Extensions.DependencyInjection.IServiceCollection services)
		{
			return services.AddSingleton<IEntityStoreTelemetry, EntityStoreTelemetryCore>();
		}
	}
}
