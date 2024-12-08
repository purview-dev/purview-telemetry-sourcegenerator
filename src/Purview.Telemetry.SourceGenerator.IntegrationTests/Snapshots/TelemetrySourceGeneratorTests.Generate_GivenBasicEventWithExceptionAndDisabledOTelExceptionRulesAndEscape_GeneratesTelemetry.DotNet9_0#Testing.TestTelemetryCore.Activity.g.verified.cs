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
	sealed partial class TestTelemetryCore : Testing.ITestTelemetry
	{
		readonly static System.Diagnostics.ActivitySource _activitySource = new System.Diagnostics.ActivitySource("activity-source");

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
		public System.Diagnostics.Activity? Activity()
		{
			if (!_activitySource.HasListeners())
			{
				return null;
			}

			System.Diagnostics.Activity? activityActivity = _activitySource.StartActivity(name: "Activity", kind: System.Diagnostics.ActivityKind.Internal, parentId: default, tags: default, links: default, startTime: default);

			return activityActivity;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public void EventMethod(System.Diagnostics.Activity? activity, string stringParam, int intParam, bool boolParam, System.Exception anException, bool escape)
		{
			if (!_activitySource.HasListeners())
			{
				return;
			}

			if (activity != null)
			{
				System.Diagnostics.ActivityTagsCollection tagsCollectionEventMethod = new System.Diagnostics.ActivityTagsCollection();
				tagsCollectionEventMethod.Add("intparam", intParam);
				tagsCollectionEventMethod.Add("boolparam", boolParam);
				tagsCollectionEventMethod.Add("anexception", anException.ToString());

				System.Diagnostics.ActivityEvent activityEventEventMethod = new System.Diagnostics.ActivityEvent(name: "EventMethod", timestamp: default, tags: tagsCollectionEventMethod);

				activity.AddEvent(activityEventEventMethod);

				activity.SetBaggage("stringparam", stringParam);
			}
		}

	}
}
