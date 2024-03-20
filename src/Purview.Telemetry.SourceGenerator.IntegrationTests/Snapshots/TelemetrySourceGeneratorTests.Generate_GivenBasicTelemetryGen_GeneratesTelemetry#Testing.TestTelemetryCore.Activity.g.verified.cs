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

		public void Activity(string stringParam, int intParam, bool boolParam)
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
		}

		public void Event(string stringParam, int intParam, bool boolParam)
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
		}

		public void Context(string stringParam, int intParam, bool boolParam)
		{
			if (System.Diagnostics.Activity.Current != null)
			{
				System.Diagnostics.Activity.Current.SetTag("intparam", intParam);
				System.Diagnostics.Activity.Current.SetTag("boolparam", boolParam);
				System.Diagnostics.Activity.Current.SetBaggage("stringparam", stringParam);
			}
		}

	}
}