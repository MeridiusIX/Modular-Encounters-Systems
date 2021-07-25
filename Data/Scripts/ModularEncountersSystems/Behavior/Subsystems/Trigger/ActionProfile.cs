using ProtoBuf;
using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.ObjectBuilders;
using VRageMath;
using ModularEncountersSystems.Logging;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger {

	[ProtoContract]
	public class ActionProfile {

		[ProtoMember(2)]
		public ChatProfile ChatDataDefunct;

		[ProtoMember(8)]
		public SpawnProfile SpawnerDefunct;

		[ProtoMember(59)]
		public string ProfileSubtypeId;

		[ProtoMember(125)]
		public List<ChatProfile> ChatData;

		[ProtoMember(126)]
		public List<SpawnProfile> Spawner;

		[ProtoIgnore]
		public ActionReferenceProfile ActionReference {

			get {

				if (_actionReference == null) {

					ProfileManager.ActionReferenceProfiles.TryGetValue(ProfileSubtypeId, out _actionReference);
				
				}

				return _actionReference;
			
			}
		
		}

		[ProtoIgnore]
		private ActionReferenceProfile _actionReference;

		public ActionProfile() {

			ChatData = new List<ChatProfile>();
			ChatDataDefunct = new ChatProfile();

			Spawner = new List<SpawnProfile>();
			SpawnerDefunct = new SpawnProfile();

			ProfileSubtypeId = "";

		}

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					//ChatData
					if (tag.Contains("[ChatData:") == true) {

						string tempValue = "";
						TagParse.TagStringCheck(tag, ref tempValue);
						bool gotChat = false;

						if (string.IsNullOrWhiteSpace(tempValue) == false) {

							byte[] byteData = { };

							if (ProfileManager.ChatObjectTemplates.TryGetValue(tempValue, out byteData) == true) {

								try {

									var profile = MyAPIGateway.Utilities.SerializeFromBinary<ChatProfile>(byteData);

									if (profile != null) {

										ChatData.Add(profile);
										gotChat = true;

									} else {

										BehaviorLogger.Write("Deserialized Chat Profile was Null", BehaviorDebugEnum.Error, true);

									}

								} catch (Exception e) {

									BehaviorLogger.Write("Caught Exception While Attaching to Action Profile:", BehaviorDebugEnum.Error, true);
									BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Error, true);

								}

							} else {

								ProfileManager.ReportProfileError(tempValue, "Chat Profile Not Registered in Profile Manager");

							}

						}

						if (!gotChat)
							ProfileManager.ReportProfileError(tempValue, "Provided Chat Profile Could Not Be Loaded in Trigger: " + ProfileSubtypeId);

					}

					//Spawner
					if (tag.Contains("[Spawner:") == true) {

						string tempValue = "";
						TagParse.TagStringCheck(tag, ref tempValue);
						bool gotSpawn = false;

						if (string.IsNullOrWhiteSpace(tempValue) == false) {

							byte[] byteData = { };

							if (ProfileManager.SpawnerObjectTemplates.TryGetValue(tempValue, out byteData) == true) {

								try {

									var profile = MyAPIGateway.Utilities.SerializeFromBinary<SpawnProfile>(byteData);

									if (profile != null) {

										Spawner.Add(profile);
										gotSpawn = true;

									}

								} catch (Exception) {



								}

							}

						}

						if (!gotSpawn)
							ProfileManager.ReportProfileError(tempValue, "Provided Spawn Profile Could Not Be Loaded In Profile: " + ProfileSubtypeId);


					}

				}

			}

		}

	}

}
