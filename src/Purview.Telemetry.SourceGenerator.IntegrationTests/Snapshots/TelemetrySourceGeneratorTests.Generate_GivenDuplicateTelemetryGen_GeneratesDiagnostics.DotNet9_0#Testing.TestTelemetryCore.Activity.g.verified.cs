﻿//HintName: Testing.TestTelemetryCore.Activity.g.cs
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

namespace Testing
{
	sealed partial class TestTelemetryCore : global::Testing.ITestTelemetry
	{
		readonly static global::System.Diagnostics.ActivitySource _activitySource = new("activity-source");

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		static void RecordExceptionInternal(global::System.Diagnostics.Activity? activity, global::System.Exception? exception, bool escape)
		{
			if (activity == null || exception == null)
			{
				return;
			}

			global::System.Diagnostics.ActivityTagsCollection tagsCollection = new();			tagsCollection.Add("exception.escaped", escape);
			tagsCollection.Add("exception.message", exception.Message);
			tagsCollection.Add("exception.type", exception.GetType().FullName);
			tagsCollection.Add("exception.stacktrace", exception.StackTrace);

			global::System.Diagnostics.ActivityEvent recordExceptionEvent = new(name: "exception", timestamp: default, tags: tagsCollection);

			activity.AddEvent(recordExceptionEvent);
		}

	}
}
