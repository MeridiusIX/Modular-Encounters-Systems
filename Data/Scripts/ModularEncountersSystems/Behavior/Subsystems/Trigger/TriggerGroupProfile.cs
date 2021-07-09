using ProtoBuf;
using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using ModularEncountersSystems.Logging;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger {

	[ProtoContract]
	public class TriggerGroupProfile {

		[ProtoMember(1)]
		public List<TriggerProfile> Triggers;

		[ProtoMember(2)]
		public string ProfileSubtypeId;

		[ProtoIgnore]
		public List<string> ExistingTriggers;

		public TriggerGroupProfile() {

			Triggers = new List<TriggerProfile>();
			ProfileSubtypeId = "";
			ExistingTriggers = new List<string>();

		}

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					//Triggers
					if (tag.Contains("[Triggers:") == true) {

						bool gotTrigger = false;
						var tempValue = "";
						TagParse.TagStringCheck(tag, ref tempValue);

						if (string.IsNullOrWhiteSpace(tempValue) == false) {

							byte[] byteData = { };

							if (ProfileManager.TriggerObjectTemplates.TryGetValue(tempValue, out byteData) == true) {

								try {

									var profile = MyAPIGateway.Utilities.SerializeFromBinary<TriggerProfile>(byteData);

									if (profile != null) {

										if (ExistingTriggers.Contains(profile.ProfileSubtypeId))
											continue;

										ExistingTriggers.Add(profile.ProfileSubtypeId);

										Triggers.Add(profile);
										gotTrigger = true;


									}

								} catch (Exception) {



								}

							}

						}

						if (!gotTrigger)
							BehaviorLogger.Write("Could Not Find Trigger Profile Associated To Tag: " + tag, BehaviorDebugEnum.Error);

					}

				}

			}

		}

	}

}
