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


	public class EventGroupProfile {


		public string ProfileSubtypeId;

		public List<Event> Events;

		public List<EventCondition> Conditions;

		//This the only thing we need to save
		public DateTime StartDate;


		//[ProtoIgnore]
		//public List<string> ExistingEvents;

		public EventGroupProfile() {

			Events = new List<Event>();
			ProfileSubtypeId = "";
			//ExistingEvents = new List<string>();

		}

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					//Events
					if (tag.Contains("[Events:") == true) {

						bool gotTrigger = false;
						var tempValue = "";
						TagParse.TagStringCheck(tag, ref tempValue);


						foreach (Event eventobject in EventManager.EventsList){
							
							if(eventobject.ProfileSubtypeId == tempValue)
							{
								Events.Add(eventobject);

							}

						}



					}

				}

			}

		}

	}

}
