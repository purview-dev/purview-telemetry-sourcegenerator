﻿//HintName: ContextAttribute.g.cs
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

namespace Purview.Telemetry.Activities;

/// <summary>
/// Determines if the methods parameters should be
/// added to the current <see cref="global::System.Diagnostics.Activity"/>, using
/// either the <see cref="global::Purview.Telemetry.TagAttribute"/>,
/// the <see cref="global::Purview.Telemetry.BaggageAttribute"/> or inferred.
/// </summary>
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
[global::System.AttributeUsage(global::System.AttributeTargets.Method, AllowMultiple = false)]
[global::System.Diagnostics.Conditional("PURVIEW_TELEMETRY_ATTRIBUTES")]
sealed class ContextAttribute : global::System.Attribute
{
}
