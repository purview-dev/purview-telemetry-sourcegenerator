﻿//HintName: Testing.TestActivitiesCore.Activity.g.cs
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
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
	sealed partial class TestActivitiesCore : global::Testing.ITestActivities
	{
		readonly static global::System.Diagnostics.ActivitySource _activitySource = new("testing-activity-source");

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

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public System.Diagnostics.Activity? Activity()
		{
			if (!_activitySource.HasListeners())
			{
				return null;
			}

			global::System.Diagnostics.Activity? activityActivity = _activitySource.StartActivity(name: "Activity", kind: global::System.Diagnostics.ActivityKind.Internal, parentId: default, tags: default, links: default, startTime: default);

			return activityActivity;
		}

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public System.Diagnostics.Activity Context(System.Diagnostics.Activity? activity, string stringParam, int intParam, bool boolParam)
		{
			if (!_activitySource.HasListeners())
			{
				return null!;
			}

			if (activity != null)
			{
				activity.SetTag("intparam", intParam);
				activity.SetTag("boolparam", boolParam);
				activity.SetBaggage("stringparam", stringParam);
			}

			return activity;
		}

		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Purview.Telemetry.SourceGenerator", "0.1.0.0")]
		[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public System.Diagnostics.Activity? ContextWithNullableReturnActivity(System.Diagnostics.Activity? activity, string stringParam, int intParam, bool boolParam)
		{
			if (!_activitySource.HasListeners())
			{
				return null;
			}

			if (activity != null)
			{
				activity.SetTag("intparam", intParam);
				activity.SetTag("boolparam", boolParam);
				activity.SetBaggage("stringparam", stringParam);
			}

			return activity;
		}

	}
}
