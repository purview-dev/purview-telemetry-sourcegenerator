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

		public System.Diagnostics.Activity Activity(string stringParam, int intParam, bool boolParam)
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

		public System.Diagnostics.Activity Event(string stringParam, int intParam, bool boolParam)
		{
			if (System.Diagnostics.Activity.Current != null)
			{
				System.Diagnostics.ActivityTagsCollection tagsCollectionEvent = new System.Diagnostics.ActivityTagsCollection();
				tagsCollectionEvent.Add("intparam", intParam);
				tagsCollectionEvent.Add("boolparam", boolParam);
				System.Diagnostics.ActivityEvent activityEventEvent = new System.Diagnostics.ActivityEvent(name: "Event", timestamp: default, tags: tagsCollectionEvent);

				System.Diagnostics.Activity.Current.AddEvent(activityEventEvent);
				System.Diagnostics.Activity.Current.SetBaggage("stringparam", stringParam);
			}

			return System.Diagnostics.Activity.Current;
		}

	}
}
