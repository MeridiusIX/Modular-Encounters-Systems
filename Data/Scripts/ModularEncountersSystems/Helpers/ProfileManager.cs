using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.Zones;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Helpers {
	public static class ProfileManager {

		//Spawner Related Profiles
		public static Dictionary<string, BlockReplacementProfile> BlockReplacementProfiles = new Dictionary<string, BlockReplacementProfile>();
		public static Dictionary<string, BotSpawnProfile> BotSpawnProfiles = new Dictionary<string, BotSpawnProfile>();
		public static Dictionary<string, ReplenishmentProfile> ReplenishmentProfiles = new Dictionary<string, ReplenishmentProfile>();
		public static Dictionary<string, DerelictionProfile> DerelictionProfiles = new Dictionary<string, DerelictionProfile>();
		public static Dictionary<string, LootGroup> LootGroups = new Dictionary<string, LootGroup>();
		public static Dictionary<string, LootProfile> LootProfiles = new Dictionary<string, LootProfile>();
		public static Dictionary<string, ManipulationGroup> ManipulationGroups = new Dictionary<string, ManipulationGroup>();
		public static Dictionary<string, ManipulationProfile> ManipulationProfiles = new Dictionary<string, ManipulationProfile>();
		public static Dictionary<string, SpawnConditionsGroup> SpawnConditionGroups = new Dictionary<string, SpawnConditionsGroup>();
		public static Dictionary<string, SpawnConditionsProfile> SpawnConditionProfiles = new Dictionary<string, SpawnConditionsProfile>();
		public static Dictionary<string, StaticEncounter> StaticEncounters = new Dictionary<string, StaticEncounter>();
		public static Dictionary<MyDefinitionId, WeaponModRulesProfile> WeaponModRulesProfiles = new Dictionary<MyDefinitionId, WeaponModRulesProfile>();
		public static Dictionary<string, Zone> ZoneProfiles = new Dictionary<string, Zone>();
		public static Dictionary<string, ZoneConditionsProfile> ZoneConditionsProfiles = new Dictionary<string, ZoneConditionsProfile>();

		public static Dictionary<string, string> BehaviorTemplates = new Dictionary<string, string>();
		public static Dictionary<string, BehaviorSubclass> BehaviorTypes = new Dictionary<string, BehaviorSubclass>();

		public static Dictionary<string, byte[]> ActionObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> AutopilotObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> ChatObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> ConditionObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> SpawnerObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> TargetObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> TriggerObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> TriggerGroupObjectTemplates = new Dictionary<string, byte[]>();

		public static Dictionary<string, AutoPilotProfile> AutoPilotProfiles = new Dictionary<string, AutoPilotProfile>();
		public static Dictionary<string, ActionReferenceProfile> ActionReferenceProfiles = new Dictionary<string, ActionReferenceProfile>();
		public static Dictionary<string, CommandProfile> CommandProfiles = new Dictionary<string, CommandProfile>();
		public static Dictionary<string, TargetProfile> TargetProfiles = new Dictionary<string, TargetProfile>();
		public static Dictionary<string, WaypointProfile> WaypointProfiles = new Dictionary<string, WaypointProfile>();

		public static Dictionary<string, MyDefinitionBase> DatapadTemplates = new Dictionary<string, MyDefinitionBase>();

		public static List<string> ErrorProfiles = new List<string>();

		public static AutoPilotProfile DefaultAutoPilotSettings = new AutoPilotProfile();

		public static void Setup() {

			//First Phase
			foreach (var component in DefinitionHelper.EntityComponentDefinitions) {

				if (!BlockReplacementProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Block Replacement]")) {

					BlockReplacementProfiles.Add(component.Id.SubtypeName, new BlockReplacementProfile(component.DescriptionText));
					continue;

				}

				if (!DerelictionProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Dereliction]")) {

					var profile = new DerelictionProfile(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					DerelictionProfiles.Add(component.Id.SubtypeName, profile);
					continue;

				}

				if (component.DescriptionText.Contains("[MES Weapon Mod Rules]")) {

					var weaponRule = new WeaponModRulesProfile();
					weaponRule.InitTags(component.DescriptionText);
					weaponRule.ProfileSubtypeId = component.Id.SubtypeName;

					if (!WeaponModRulesProfiles.ContainsKey(weaponRule.WeaponBlock)) {

						WeaponModRulesProfiles.Add(weaponRule.WeaponBlock, weaponRule);

					} else {

						//TODO: Log Dup Key

					}
					
					continue;

				}

				if (!ZoneProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Zone]")) {

					var zone = new Zone();
					zone.InitTags(component.DescriptionText);
					zone.ProfileSubtypeId = component.Id.SubtypeName;
					ZoneProfiles.Add(component.Id.SubtypeName, zone);
					continue;

				}

				if (!ZoneConditionsProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Zone Conditions]")) {

					var zone = new ZoneConditionsProfile();
					zone.InitTags(component.DescriptionText);
					zone.ProfileSubtypeId = component.Id.SubtypeName;
					ZoneConditionsProfiles.Add(component.Id.SubtypeName, zone);
					continue;

				}

				if (!BotSpawnProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Bot Spawn]")) {

					var bot = new BotSpawnProfile();
					bot.InitTags(component.DescriptionText);
					bot.ProfileSubtypeId = component.Id.SubtypeName;
					BotSpawnProfiles.Add(component.Id.SubtypeName, bot);
					continue;

				}

				if (component.DescriptionText.Contains("[RivalAI Chat]") == true && ChatObjectTemplates.ContainsKey(component.Id.SubtypeName) == false) {

					var chatObject = new ChatProfile();
					chatObject.InitTags(component.DescriptionText);
					chatObject.ProfileSubtypeId = component.Id.SubtypeName;
					var chatBytes = MyAPIGateway.Utilities.SerializeToBinary<ChatProfile>(chatObject);
					//Logger.WriteLog("Chat Profile Added: " + component.Id.SubtypeName);
					ChatObjectTemplates.Add(component.Id.SubtypeName, chatBytes);
					continue;

				}

				if (component.DescriptionText.Contains("[RivalAI Spawn]") == true && SpawnerObjectTemplates.ContainsKey(component.Id.SubtypeName) == false) {

					var spawnerObject = new SpawnProfile();
					spawnerObject.InitTags(component.DescriptionText);
					spawnerObject.ProfileSubtypeId = component.Id.SubtypeName;
					var spawnerBytes = MyAPIGateway.Utilities.SerializeToBinary<SpawnProfile>(spawnerObject);
					//Logger.WriteLog("Spawner Profile Added: " + component.Id.SubtypeName);
					SpawnerObjectTemplates.Add(component.Id.SubtypeName, spawnerBytes);
					continue;

				}

			}

			//Second Phase
			foreach (var component in DefinitionHelper.EntityComponentDefinitions) {

				if (!ReplenishmentProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Replenishment]")) {

					ReplenishmentProfiles.Add(component.Id.SubtypeName, new ReplenishmentProfile(component.DescriptionText));
					continue;

				}

				if (!ManipulationProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Manipulation]")) {

					var profile = new ManipulationProfile(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					ManipulationProfiles.Add(component.Id.SubtypeName, profile);
					continue;

				}

				if (!SpawnConditionProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Spawn Conditions]")) {

					var profile = new SpawnConditionsProfile();
					profile.InitTags(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					SpawnConditionProfiles.Add(component.Id.SubtypeName, profile);
					continue;

				}

				if (!StaticEncounters.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Static Encounter]")) {

					var profile = new StaticEncounter();
					profile.InitTags(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					StaticEncounters.Add(component.Id.SubtypeName, profile);
					continue;

				}

				if (component.DescriptionText.Contains("[RivalAI Action]") == true && ActionObjectTemplates.ContainsKey(component.Id.SubtypeName) == false) {

					var actionObject = new ActionProfile();
					actionObject.InitTags(component.DescriptionText);
					actionObject.ProfileSubtypeId = component.Id.SubtypeName;

					var actionReference = new ActionReferenceProfile();
					actionReference.InitTags(component.DescriptionText);
					actionReference.ProfileSubtypeId = component.Id.SubtypeName;

					var targetBytes = MyAPIGateway.Utilities.SerializeToBinary<ActionProfile>(actionObject);
					//Logger.WriteLog("Action Profile Added: " + component.Id.SubtypeName);
					ActionObjectTemplates.Add(component.Id.SubtypeName, targetBytes);
					ActionReferenceProfiles.Add(component.Id.SubtypeName, actionReference);
					continue;

				}

				if (component.DescriptionText.Contains("[RivalAI Condition]") == true && ChatObjectTemplates.ContainsKey(component.Id.SubtypeName) == false) {

					var conditionObject = new ConditionProfile();
					conditionObject.InitTags(component.DescriptionText);
					conditionObject.ProfileSubtypeId = component.Id.SubtypeName;
					var conditionBytes = MyAPIGateway.Utilities.SerializeToBinary<ConditionProfile>(conditionObject);
					//Logger.WriteLog("Condition Profile Added: " + component.Id.SubtypeName);
					ConditionObjectTemplates.Add(component.Id.SubtypeName, conditionBytes);
					continue;

				}

				if (component.DescriptionText.Contains("[RivalAI Target]") == true && TargetObjectTemplates.ContainsKey(component.Id.SubtypeName) == false) {

					var targetObject = new TargetProfile();
					targetObject.InitTags(component.DescriptionText);
					targetObject.ProfileSubtypeId = component.Id.SubtypeName;
					TargetProfiles.Add(targetObject.ProfileSubtypeId, targetObject);
					var targetBytes = MyAPIGateway.Utilities.SerializeToBinary<TargetProfile>(targetObject);
					//Logger.WriteLog("Target Profile Added: " + component.Id.SubtypeName);
					TargetObjectTemplates.Add(component.Id.SubtypeName, targetBytes);
					continue;

				}

			}

			//Third Phase
			foreach (var component in DefinitionHelper.EntityComponentDefinitions) {

				if (!ManipulationGroups.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Manipulation Group]")) {

					var profile = new ManipulationGroup(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					ManipulationGroups.Add(component.Id.SubtypeName, profile);
					continue;

				}

				if (!SpawnConditionGroups.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Spawn Conditions Group]")) {

					var profile = new SpawnConditionsGroup(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					SpawnConditionGroups.Add(component.Id.SubtypeName, profile);
					continue;

				}

				if (component.DescriptionText.Contains("[RivalAI Trigger]") == true && TriggerObjectTemplates.ContainsKey(component.Id.SubtypeName) == false) {

					var triggerObject = new TriggerProfile();
					triggerObject.InitTags(component.DescriptionText);
					triggerObject.ProfileSubtypeId = component.Id.SubtypeName;
					var triggerBytes = MyAPIGateway.Utilities.SerializeToBinary<TriggerProfile>(triggerObject);
					//Logger.WriteLog("Trigger Profile Added: " + component.Id.SubtypeName);
					TriggerObjectTemplates.Add(component.Id.SubtypeName, triggerBytes);
					continue;

				}

			}

			//Forth Phase
			foreach (var component in DefinitionHelper.EntityComponentDefinitions) {

				if (component.DescriptionText.Contains("[RivalAI Autopilot]") == true && AutopilotObjectTemplates.ContainsKey(component.Id.SubtypeName) == false) {

					var autopilotObject = new AutoPilotProfile();
					autopilotObject.InitTags(component.DescriptionText);
					autopilotObject.ProfileSubtypeId = component.Id.SubtypeName;
					var autopilotBytes = MyAPIGateway.Utilities.SerializeToBinary<AutoPilotProfile>(autopilotObject);
					//Logger.WriteLog("Trigger Profile Added: " + component.Id.SubtypeName);
					AutopilotObjectTemplates.Add(component.Id.SubtypeName, autopilotBytes);
					continue;

				}

				if (component.DescriptionText.Contains("[RivalAI TriggerGroup]") == true && TriggerObjectTemplates.ContainsKey(component.Id.SubtypeName) == false) {

					var triggerObject = new TriggerGroupProfile();
					triggerObject.InitTags(component.DescriptionText);
					triggerObject.ProfileSubtypeId = component.Id.SubtypeName;
					var triggerBytes = MyAPIGateway.Utilities.SerializeToBinary<TriggerGroupProfile>(triggerObject);
					//Logger.WriteLog("Trigger Profile Added: " + component.Id.SubtypeName);
					TriggerGroupObjectTemplates.Add(component.Id.SubtypeName, triggerBytes);
					continue;

				}

				if (component.DescriptionText.Contains("[RivalAI Waypoint]") == true && WaypointProfiles.ContainsKey(component.Id.SubtypeName) == false) {

					var waypoint = new WaypointProfile();
					waypoint.InitTags(component.DescriptionText);
					waypoint.ProfileSubtypeId = component.Id.SubtypeName;
					WaypointProfiles.Add(component.Id.SubtypeName, waypoint);
					continue;

				}

				if (component.DescriptionText.Contains("[RivalAI Command]") == true && CommandProfiles.ContainsKey(component.Id.SubtypeName) == false) {

					var command = new CommandProfile();
					command.InitTags(component.DescriptionText);
					command.ProfileSubtypeId = component.Id.SubtypeName;
					CommandProfiles.Add(component.Id.SubtypeName, command);
					continue;

				}

				if (component.Id.SubtypeName.StartsWith("RivalAI-Datapad") && !DatapadTemplates.ContainsKey(component.Id.SubtypeName)) {

					DatapadTemplates.Add(component.Id.SubtypeName, component);
					continue;

				}

			}

			//Fifth Phase
			foreach (var component in DefinitionHelper.EntityComponentDefinitions) {

				if ((component.DescriptionText.Contains("[RivalAI Behavior]") || component.DescriptionText.Contains("[Rival AI Behavior]")) && BehaviorTemplates.ContainsKey(component.Id.SubtypeName) == false) {

					BehaviorTemplates.Add(component.Id.SubtypeName, component.DescriptionText);

					if (!BehaviorTypes.ContainsKey(component.Id.SubtypeName)) {

						if (component.DescriptionText.Contains("[BehaviorName:CoreBehavior]")) {

							BehaviorTypes.Add(component.Id.SubtypeName, BehaviorSubclass.CoreBehavior);
							continue;

						}

						if (component.DescriptionText.Contains("[BehaviorName:CargoShip]")) {

							BehaviorTypes.Add(component.Id.SubtypeName, BehaviorSubclass.CargoShip);
							continue;

						}

						if (component.DescriptionText.Contains("[BehaviorName:Fighter]")) {

							BehaviorTypes.Add(component.Id.SubtypeName, BehaviorSubclass.Fighter);
							continue;

						}

						if (component.DescriptionText.Contains("[BehaviorName:HorseFighter]")) {

							BehaviorTypes.Add(component.Id.SubtypeName, BehaviorSubclass.HorseFighter);
							continue;

						}

						if (component.DescriptionText.Contains("[BehaviorName:Horsefly]")) {

							BehaviorTypes.Add(component.Id.SubtypeName, BehaviorSubclass.Horsefly);
							continue;

						}

						if (component.DescriptionText.Contains("[BehaviorName:Hunter]")) {

							BehaviorTypes.Add(component.Id.SubtypeName, BehaviorSubclass.Hunter);
							continue;

						}

						if (component.DescriptionText.Contains("[BehaviorName:Nautical]")) {

							BehaviorTypes.Add(component.Id.SubtypeName, BehaviorSubclass.Nautical);
							continue;

						}

						if (component.DescriptionText.Contains("[BehaviorName:Passive]")) {

							BehaviorTypes.Add(component.Id.SubtypeName, BehaviorSubclass.Passive);
							continue;

						}

						if (component.DescriptionText.Contains("[BehaviorName:Scout]")) {

							BehaviorTypes.Add(component.Id.SubtypeName, BehaviorSubclass.Scout);
							continue;

						}

						if (component.DescriptionText.Contains("[BehaviorName:Sniper]")) {

							BehaviorTypes.Add(component.Id.SubtypeName, BehaviorSubclass.Sniper);
							continue;

						}

						if (component.DescriptionText.Contains("[BehaviorName:Strike]")) {

							BehaviorTypes.Add(component.Id.SubtypeName, BehaviorSubclass.Strike);
							continue;

						}

					}

					continue;

				}

			}



			MES_SessionCore.UnloadActions += Unload;
		
		}

		public static AutoPilotProfile GetAutopilotProfile(string profileSubtypeId, string defaultBehavior = "") {

			//TODO: Move All Of This To Dictionary Since AutoPilotProfile is ReadOnly

			byte[] apBytes = null;

			if (AutopilotObjectTemplates.TryGetValue(profileSubtypeId, out apBytes)) {

				var ap = MyAPIGateway.Utilities.SerializeFromBinary<AutoPilotProfile>(apBytes);

				if (ap != null || !string.IsNullOrWhiteSpace(ap.ProfileSubtypeId))
					return ap;

			}

			BehaviorLogger.Write("Warning: Autopilot Profile for " + profileSubtypeId + " Not Found!", BehaviorDebugEnum.BehaviorSetup);

			if (!string.IsNullOrWhiteSpace(defaultBehavior)) {

				apBytes = null;

				if (AutopilotObjectTemplates.TryGetValue("RAI-Generic-Autopilot-" + defaultBehavior, out apBytes)) {

					var ap = MyAPIGateway.Utilities.SerializeFromBinary<AutoPilotProfile>(apBytes);

					if (ap != null || !string.IsNullOrWhiteSpace(ap.ProfileSubtypeId))
						return ap;

				}

			}

			BehaviorLogger.Write("Warning: Backup Autopilot Profile for " + defaultBehavior + " Not Found!", BehaviorDebugEnum.BehaviorSetup);

			return new AutoPilotProfile();

		}

		public static void ReportProfileError(string profileId, string reason) {

			if (ErrorProfiles.Contains(profileId))
				return;

			var profileIdSafe = !string.IsNullOrWhiteSpace(profileId) ? profileId : "(null)";

			SpawnLogger.Write(reason + ": [" + profileIdSafe + "]", SpawnerDebugEnum.Error, true);
			ErrorProfiles.Add(reason + ": [" + profileIdSafe + "]");

		}

		public static void Unload() {
		
			
		
		}

	}
}
