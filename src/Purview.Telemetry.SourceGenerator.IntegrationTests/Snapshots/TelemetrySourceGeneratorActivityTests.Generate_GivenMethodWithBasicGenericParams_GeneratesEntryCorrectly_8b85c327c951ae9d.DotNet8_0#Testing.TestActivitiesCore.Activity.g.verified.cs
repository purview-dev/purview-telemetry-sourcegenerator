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
		readonly static System.Diagnostics.ActivitySource _activitySource = new System.Diagnostics.ActivitySource("purview.telemetry.sourcegenerator");

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
		public void Activity(System.Collections.Generic.IDictionary<string, int> paramName)
		{
			System.Diagnostics.Activity? activityActivity = _activitySource.StartActivity(name: "Activity", kind: System.Diagnostics.ActivityKind.Internal, parentId: default, tags: default, links: default, startTime: default);

			if (activityActivity != null)
			{
				activityActivity.SetTag("paramname", paramName);
			}
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Event(System.Collections.Generic.IDictionary<string, int> paramName)
		{
			if (System.Diagnostics.Activity.Current != null)
			{
				System.Diagnostics.ActivityTagsCollection tagsCollectionEvent = new System.Diagnostics.ActivityTagsCollection();
				tagsCollectionEvent.Add("paramname", paramName);

				System.Diagnostics.ActivityEvent activityEventEvent = new System.Diagnostics.ActivityEvent(name: "Event", timestamp: default, tags: tagsCollectionEvent);

				System.Diagnostics.Activity.Current.AddEvent(activityEventEvent);
			}
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void Context(System.Collections.Generic.IDictionary<string, int> paramName)
		{
			if (System.Diagnostics.Activity.Current != null)
			{
				System.Diagnostics.Activity.Current.SetTag("paramname", paramName);
			}
		}

	}
}
