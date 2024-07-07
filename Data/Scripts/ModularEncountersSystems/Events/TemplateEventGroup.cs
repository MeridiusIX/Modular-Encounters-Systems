using ProtoBuf;
using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Events.Condition;

namespace ModularEncountersSystems.Events
{
	public class TemplateEventGroup {


		public string ProfileSubtypeId;

		public List<string> EventIds;


		public TemplateEventGroup() {

			EventIds = new List<string>();
			ProfileSubtypeId = "";


		}

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					//Events
					if (tag.Contains("[TemplateEventIds:") == true) {

						bool gotTrigger = false;
						var tempValue = "";
						TagParse.TagStringCheck(tag, ref tempValue);

						EventIds.Add(tempValue);
					}

				}

			}

		}



		public void AddEventsAsInsertible(List<string> replacekeys, List<string> replacevalues)
		{

			var uniqueValue = MyAPIGateway.Session.GameDateTime.Ticks;

            foreach (var eventname in EventIds)
            {

				var customdata = "";
				if (!ProfileManager.TemplateEventProfiles.TryGetValue(eventname, out customdata))
				{
					continue;
				}

				var _event = new Event();
				_event.InitAsInsertable(eventname, uniqueValue, replacekeys, replacevalues);

				EventManager.EventsList.Add(_event);
			}



		}
	}

}
