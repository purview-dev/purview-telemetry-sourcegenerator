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
	sealed partial class TestActivitiesCore : Testing.ITestActivities
	{
		readonly static System.Diagnostics.ActivitySource _activitySource = new System.Diagnostics.ActivitySource("testing-activity-source");

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		static void RecordExceptionInternal(System.Diagnostics.Activity? activity, System.Exception? exception, bool escape)
		{
			if (activity == null || exception == null)
			{
				return;
			}

			System.Diagnostics.ActivityTagsCollection tagsCollection = new System.Diagnostics.ActivityTagsCollection();
			tagsCollection.Add("exception.escaped", escape);
			tagsCollection.Add("exception.message", exception.Message);
			tagsCollection.Add("exception.type", exception.GetType().FullName);
			tagsCollection.Add("exception.stacktrace", exception.StackTrace);

			System.Diagnostics.ActivityEvent recordExceptionEvent = new System.Diagnostics.ActivityEvent(name: "exception", timestamp: default, tags: tagsCollection);

			activity.AddEvent(recordExceptionEvent);
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public System.Diagnostics.Activity? Activity(string? stringParam, int? intParam, bool? boolParam)
		{
			System.Diagnostics.Activity? activityActivity = _activitySource.StartActivity(name: "Activity", kind: System.Diagnostics.ActivityKind.Internal, parentId: default, tags: default, links: default, startTime: default);

			if (activityActivity != null)
			{
				activityActivity.SetTag("intparam", intParam);
				activityActivity.SetTag("boolparam", boolParam);
			}

			if (activityActivity != null)
			{
				activityActivity.SetBaggage("stringparam", stringParam);
			}

			return activityActivity;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public System.Diagnostics.Activity? ActivityWithNullableParams(string? stringParam, int? intParam, bool? boolParam)
		{
			System.Diagnostics.Activity? activityActivityWithNullableParams = _activitySource.StartActivity(name: "ActivityWithNullableParams", kind: System.Diagnostics.ActivityKind.Internal, parentId: default, tags: default, links: default, startTime: default);

			if (activityActivityWithNullableParams != null)
			{
				activityActivityWithNullableParams.SetTag("intparam", intParam);
				activityActivityWithNullableParams.SetTag("boolparam", boolParam);
			}

			if (activityActivityWithNullableParams != null)
			{
				activityActivityWithNullableParams.SetBaggage("stringparam", stringParam);
			}

			return activityActivityWithNullableParams;
		}

	}
}
