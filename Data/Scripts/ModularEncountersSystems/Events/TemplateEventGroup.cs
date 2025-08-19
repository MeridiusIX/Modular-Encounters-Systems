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



		public long AddEventsAsInsertible(List<string> replacekeys, List<string> replacevalues, long uniqueValue = -1, List<long> PlayerIds = null)
		{
			long _uniqueValue = 0;

			if(uniqueValue == -1)
            {
				_uniqueValue = MyAPIGateway.Session.GameDateTime.Ticks;
			}
            else
            {
				_uniqueValue = uniqueValue;
            }



            var keys = new List<string>(replacekeys);
            var values = new List<string>(replacevalues);

            keys.Add("{InstanceId}");
            values.Add(_uniqueValue.ToString());

            foreach (var eventname in EventIds)
            {
				var customdata = "";
				if (!ProfileManager.TemplateEventProfiles.TryGetValue(eventname, out customdata))
				{
					continue;
				}

				var _event = new Event();



                _event.InitAsInsertable(eventname, _uniqueValue, keys, values);
				EventManager.EventsList.Add(_event);



            }

            return _uniqueValue;

		}
	}

}
