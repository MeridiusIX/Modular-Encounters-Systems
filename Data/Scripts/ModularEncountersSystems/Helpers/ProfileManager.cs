using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Behavior.Subsystems.Weapons;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Events;
using ModularEncountersSystems.Events.Action;
using ModularEncountersSystems.Events.Condition;
using ModularEncountersSystems.Files;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.Zones;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Utils;

namespace ModularEncountersSystems.Helpers {
	public static class ProfileManager {

		public static List<MyDefinitionId> AllMesProfileIds = new List<MyDefinitionId>();

		//Spawner Related Profiles
		public static Dictionary<string, BlockReplacementProfile> BlockReplacementProfiles = new Dictionary<string, BlockReplacementProfile>();
		public static Dictionary<string, BotSpawnProfile> BotSpawnProfiles = new Dictionary<string, BotSpawnProfile>();
		public static Dictionary<string, ReplenishmentProfile> ReplenishmentProfiles = new Dictionary<string, ReplenishmentProfile>();
		public static Dictionary<string, DerelictionProfile> DerelictionProfiles = new Dictionary<string, DerelictionProfile>();
		public static Dictionary<string, LootGroup> LootGroups = new Dictionary<string, LootGroup>();
		public static Dictionary<string, LootProfile> LootProfiles = new Dictionary<string, LootProfile>();
		public static Dictionary<string, ManipulationGroup> ManipulationGroups = new Dictionary<string, ManipulationGroup>();
		public static Dictionary<string, ManipulationProfile> ManipulationProfiles = new Dictionary<string, ManipulationProfile>();
		public static Dictionary<string, PrefabDataProfile> PrefabDataProfiles = new Dictionary<string, PrefabDataProfile>();
		public static Dictionary<string, SafeZoneProfile> SafeZoneProfiles = new Dictionary<string, SafeZoneProfile>();
		public static Dictionary<string, ShipyardProfile> ShipyardProfiles = new Dictionary<string, ShipyardProfile>();
		public static Dictionary<string, SpawnConditionsGroup> SpawnConditionGroups = new Dictionary<string, SpawnConditionsGroup>();
		public static Dictionary<string, SpawnConditionsProfile> SpawnConditionProfiles = new Dictionary<string, SpawnConditionsProfile>();
		public static Dictionary<string, StaticEncounter> StaticEncounters = new Dictionary<string, StaticEncounter>();
		public static Dictionary<string, StoreProfile> StoreProfiles = new Dictionary<string, StoreProfile>();
		public static Dictionary<string, SuitUpgradesProfile> SuitUpgradesProfiles = new Dictionary<string, SuitUpgradesProfile>();
		public static Dictionary<MyDefinitionId, WeaponModRulesProfile> WeaponModRulesProfiles = new Dictionary<MyDefinitionId, WeaponModRulesProfile>();
		public static Dictionary<string, Zone> ZoneProfiles = new Dictionary<string, Zone>();
		public static Dictionary<string, ZoneConditionsProfile> ZoneConditionsProfiles = new Dictionary<string, ZoneConditionsProfile>();

		public static Dictionary<string, string> BehaviorTemplates = new Dictionary<string, string>();
		public static Dictionary<string, BehaviorSubclass> BehaviorTypes = new Dictionary<string, BehaviorSubclass>();

		public static Dictionary<string, byte[]> ActionObjectTemplates = new Dictionary<string, byte[]>();
		//public static Dictionary<string, byte[]> AutopilotObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> ChatObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> ConditionObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> SpawnerObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> TargetObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> TriggerObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> TriggerGroupObjectTemplates = new Dictionary<string, byte[]>();

		public static Dictionary<string, AutoPilotProfile> AutoPilotProfiles = new Dictionary<string, AutoPilotProfile>();
		public static Dictionary<string, ActionReferenceProfile> ActionReferenceProfiles = new Dictionary<string, ActionReferenceProfile>();
		public static Dictionary<string, ConditionReferenceProfile> ConditionReferenceProfiles = new Dictionary<string, ConditionReferenceProfile>();
		public static Dictionary<string, CommandProfile> CommandProfiles = new Dictionary<string, CommandProfile>();
		public static Dictionary<string, TargetProfile> TargetProfiles = new Dictionary<string, TargetProfile>();
		public static Dictionary<string, WaypointProfile> WaypointProfiles = new Dictionary<string, WaypointProfile>();
		public static Dictionary<string, WeaponSystemReference> WeaponProfiles = new Dictionary<string, WeaponSystemReference>();

		public static Dictionary<string, MyDefinitionBase> DatapadTemplates = new Dictionary<string, MyDefinitionBase>();
		public static Dictionary<string, StoreItemsContainer> StoreItemContainers = new Dictionary<string, StoreItemsContainer>();
		public static Dictionary<string, TextTemplate> TextTemplates = new Dictionary<string, TextTemplate>();
		public static Dictionary<string, DialogueBank> DialogueBanks = new Dictionary<string, DialogueBank>();

		public static List<string> ErrorProfiles = new List<string>();

		public static AutoPilotProfile DefaultAutoPilotSettings = new AutoPilotProfile();

		public static Dictionary<string, PlayerCondition> PlayerConditions = new Dictionary<string, PlayerCondition>();
		//Events 

		public static Dictionary<string, byte[]> EventActionObjectTemplates = new Dictionary<string, byte[]>();

		/*
		public static Dictionary<string, EventController> MainEvents = new Dictionary<string, EventController>();
		*/

		public static Dictionary<string, EventProfile> EventProfiles = new Dictionary<string, EventProfile>();
		public static Dictionary<string, EventCondition> EventConditions = new Dictionary<string, EventCondition>();
		public static Dictionary<string, EventActionReferenceProfile> EventActionReferenceProfiles = new Dictionary<string, EventActionReferenceProfile>();

		public static void Setup() {

			//First Phase
			foreach (var component in DefinitionHelper.EntityComponentDefinitions) {

				if (!BlockReplacementProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Block Replacement]")) {

					BlockReplacementProfiles.Add(component.Id.SubtypeName, new BlockReplacementProfile(component.DescriptionText));
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!DerelictionProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Dereliction]")) {

					var profile = new DerelictionProfile(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					DerelictionProfiles.Add(component.Id.SubtypeName, profile);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!LootProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Loot]")) {

					var profile = new LootProfile();
					profile.InitTags(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					LootProfiles.Add(component.Id.SubtypeName, profile);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!LootProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Shipyard]")) {

					var profile = new ShipyardProfile();
					profile.InitTags(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					ShipyardProfiles.Add(component.Id.SubtypeName, profile);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!StoreProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Store]")) {

					var profile = new StoreProfile();
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					profile.InitTags(component.DescriptionText);
					StoreProfiles.Add(component.Id.SubtypeName, profile);
					AllMesProfileIds.Add(component.Id);
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

					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!ZoneProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Zone]")) {

					var zone = new Zone();
					zone.InitTags(component.DescriptionText);
					zone.ProfileSubtypeId = component.Id.SubtypeName;
					ZoneProfiles.Add(component.Id.SubtypeName, zone);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!ZoneConditionsProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Zone Conditions]")) {

					var zone = new ZoneConditionsProfile();
					zone.ProfileSubtypeId = component.Id.SubtypeName;
					zone.InitTags(component.DescriptionText);
					ZoneConditionsProfiles.Add(component.Id.SubtypeName, zone);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!BotSpawnProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Bot Spawn]")) {

					var bot = new BotSpawnProfile();
					bot.InitTags(component.DescriptionText);
					bot.ProfileSubtypeId = component.Id.SubtypeName;
					BotSpawnProfiles.Add(component.Id.SubtypeName, bot);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if ((component.DescriptionText.Contains("[RivalAI Chat]") || component.DescriptionText.Contains("[MES AI Chat]")) && ChatObjectTemplates.ContainsKey(component.Id.SubtypeName) == false) {

					var chatObject = new ChatProfile();
					chatObject.InitTags(component.DescriptionText);
					chatObject.ProfileSubtypeId = component.Id.SubtypeName;
					var chatBytes = MyAPIGateway.Utilities.SerializeToBinary<ChatProfile>(chatObject);
					//Logger.WriteLog("Chat Profile Added: " + component.Id.SubtypeName);
					ChatObjectTemplates.Add(component.Id.SubtypeName, chatBytes);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if ((component.DescriptionText.Contains("[RivalAI Spawn]") || component.DescriptionText.Contains("[MES AI Spawn]")) && SpawnerObjectTemplates.ContainsKey(component.Id.SubtypeName) == false) {

					var spawnerObject = new SpawnProfile();
					spawnerObject.InitTags(component.DescriptionText);
					spawnerObject.ProfileSubtypeId = component.Id.SubtypeName;
					var spawnerBytes = MyAPIGateway.Utilities.SerializeToBinary<SpawnProfile>(spawnerObject);
					//Logger.WriteLog("Spawner Profile Added: " + component.Id.SubtypeName);
					SpawnerObjectTemplates.Add(component.Id.SubtypeName, spawnerBytes);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!SafeZoneProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES SafeZone]")) {

					var profile = new SafeZoneProfile(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					profile.InitTags(component.DescriptionText);
					SafeZoneProfiles.Add(component.Id.SubtypeName, profile);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

			}

			//Second Phase
			foreach (var component in DefinitionHelper.EntityComponentDefinitions) {

				if (!LootGroups.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Loot Group]")) {

					var profile = new LootGroup(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					LootGroups.Add(component.Id.SubtypeName, profile);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!ReplenishmentProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Replenishment]")) {

					ReplenishmentProfiles.Add(component.Id.SubtypeName, new ReplenishmentProfile(component.DescriptionText));
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!ManipulationProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Manipulation]")) {

					var profile = new ManipulationProfile(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					ManipulationProfiles.Add(component.Id.SubtypeName, profile);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!SpawnConditionProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Spawn Conditions]")) {

					var profile = new SpawnConditionsProfile();
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					profile.InitTags(component.DescriptionText);
					SpawnConditionProfiles.Add(component.Id.SubtypeName, profile);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!StaticEncounters.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Static Encounter]")) {

					var profile = new StaticEncounter();
					profile.InitTags(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					StaticEncounters.Add(component.Id.SubtypeName, profile);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!SuitUpgradesProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Suit Upgrades]")) {

					var profile = new SuitUpgradesProfile();
					profile.InitTags(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					SuitUpgradesProfiles.Add(component.Id.SubtypeName, profile);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if ((component.DescriptionText.Contains("[RivalAI Action]") || component.DescriptionText.Contains("[MES AI Action]")) && ActionObjectTemplates.ContainsKey(component.Id.SubtypeName) == false) {

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
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if ((component.DescriptionText.Contains("[RivalAI Condition]") || component.DescriptionText.Contains("[MES AI Condition]")) && ConditionObjectTemplates.ContainsKey(component.Id.SubtypeName) == false) {

					var conditionObject = new ConditionProfile();
					conditionObject.InitTags(component.DescriptionText);
					conditionObject.ProfileSubtypeId = component.Id.SubtypeName;

					var conditionReference = new ConditionReferenceProfile();
					conditionReference.InitTags(component.DescriptionText);
					conditionReference.ProfileSubtypeId = component.Id.SubtypeName;

					var conditionBytes = MyAPIGateway.Utilities.SerializeToBinary<ConditionProfile>(conditionObject);
					//Logger.WriteLog("Condition Profile Added: " + component.Id.SubtypeName);
					ConditionObjectTemplates.Add(component.Id.SubtypeName, conditionBytes);
					ConditionReferenceProfiles.Add(component.Id.SubtypeName, conditionReference);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if ((component.DescriptionText.Contains("[RivalAI Target]") || component.DescriptionText.Contains("[MES AI Target]")) && TargetObjectTemplates.ContainsKey(component.Id.SubtypeName) == false) {

					var targetObject = new TargetProfile();
					targetObject.InitTags(component.DescriptionText);
					targetObject.ProfileSubtypeId = component.Id.SubtypeName;
					TargetProfiles.Add(targetObject.ProfileSubtypeId, targetObject);
					var targetBytes = MyAPIGateway.Utilities.SerializeToBinary<TargetProfile>(targetObject);
					//Logger.WriteLog("Target Profile Added: " + component.Id.SubtypeName);
					TargetObjectTemplates.Add(component.Id.SubtypeName, targetBytes);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!EventActionReferenceProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Event Action]")) {

					var actionReference = new EventActionReferenceProfile();
					actionReference.InitTags(component.DescriptionText);
					actionReference.ProfileSubtypeId = component.Id.SubtypeName;

					var actionObject = new EventActionProfile();
					actionObject.InitTags(component.DescriptionText);
					actionObject.ProfileSubtypeId = component.Id.SubtypeName;

					var targetBytes = MyAPIGateway.Utilities.SerializeToBinary<EventActionProfile>(actionObject);
					EventActionObjectTemplates.Add(component.Id.SubtypeName, targetBytes);
					EventActionReferenceProfiles.Add(component.Id.SubtypeName, actionReference);
					AllMesProfileIds.Add(component.Id);

				}

				if (!EventConditions.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Event Condition]")) {

					var EventCondition = new EventCondition();
					EventCondition.InitTags(component.DescriptionText);
					EventCondition.ProfileSubtypeId = component.Id.SubtypeName;
					EventConditions.Add(component.Id.SubtypeName, EventCondition);
					AllMesProfileIds.Add(component.Id);
					continue;
				}

				if (!PlayerConditions.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Player Condition]"))
				{

					var PlayerCondition = new PlayerCondition();
					PlayerCondition.InitTags(component.DescriptionText);
					PlayerCondition.ProfileSubtypeId = component.Id.SubtypeName;
					PlayerConditions.Add(component.Id.SubtypeName, PlayerCondition);
					AllMesProfileIds.Add(component.Id);
					continue;
				}





				
			}

			//Third Phase
			foreach (var component in DefinitionHelper.EntityComponentDefinitions) {

				if (!ManipulationGroups.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Manipulation Group]")) {

					var profile = new ManipulationGroup(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					ManipulationGroups.Add(component.Id.SubtypeName, profile);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!SpawnConditionGroups.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Spawn Conditions Group]")) {

					var profile = new SpawnConditionsGroup(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					SpawnConditionGroups.Add(component.Id.SubtypeName, profile);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if ((component.DescriptionText.Contains("[RivalAI Trigger]") || component.DescriptionText.Contains("[MES AI Trigger]")) && TriggerObjectTemplates.ContainsKey(component.Id.SubtypeName) == false) {

					var triggerObject = new TriggerProfile();
					triggerObject.InitTags(component.DescriptionText);
					triggerObject.ProfileSubtypeId = component.Id.SubtypeName;
					var triggerBytes = MyAPIGateway.Utilities.SerializeToBinary<TriggerProfile>(triggerObject);
					//Logger.WriteLog("Trigger Profile Added: " + component.Id.SubtypeName);
					TriggerObjectTemplates.Add(component.Id.SubtypeName, triggerBytes);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!EventProfiles.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Event]")) {

					var profile = new EventProfile();
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					profile.InitTags(component.DescriptionText);
					EventProfiles.Add(component.Id.SubtypeName, profile);
					AllMesProfileIds.Add(component.Id);
					continue;
				}

			}

			//Forth Phase
			foreach (var component in DefinitionHelper.EntityComponentDefinitions) {

				if (!SpawnConditionGroups.ContainsKey(component.Id.SubtypeName) && component.DescriptionText.Contains("[MES Prefab Data]")) {

					var profile = new PrefabDataProfile();
					profile.InitTags(component.DescriptionText);
					profile.ProfileSubtypeId = component.Id.SubtypeName;
					PrefabDataProfiles.Add(component.Id.SubtypeName, profile);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if (!AllMesProfileIds.Contains(component.Id) && component.DescriptionText.Contains("[MES Prefab Gravity]")) {

					var profile = new PrefabGravityProfile();
					profile.InitTags(component.DescriptionText);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if ((component.DescriptionText.Contains("[RivalAI Autopilot]") || component.DescriptionText.Contains("[MES AI Autopilot]")) && AutoPilotProfiles.ContainsKey(component.Id.SubtypeName) == false) {

					var autopilotObject = new AutoPilotProfile();
					autopilotObject.InitTags(component.DescriptionText);
					autopilotObject.ProfileSubtypeId = component.Id.SubtypeName;
					AutoPilotProfiles.Add(component.Id.SubtypeName, autopilotObject);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if ((component.DescriptionText.Contains("[RivalAI TriggerGroup]") || component.DescriptionText.Contains("[MES AI TriggerGroup]")) && TriggerObjectTemplates.ContainsKey(component.Id.SubtypeName) == false) {

					var triggerObject = new TriggerGroupProfile();
					triggerObject.InitTags(component.DescriptionText);
					triggerObject.ProfileSubtypeId = component.Id.SubtypeName;
					var triggerBytes = MyAPIGateway.Utilities.SerializeToBinary<TriggerGroupProfile>(triggerObject);
					//Logger.WriteLog("Trigger Profile Added: " + component.Id.SubtypeName);
					TriggerGroupObjectTemplates.Add(component.Id.SubtypeName, triggerBytes);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if ((component.DescriptionText.Contains("[RivalAI Waypoint]") || component.DescriptionText.Contains("[MES AI Waypoint]")) && WaypointProfiles.ContainsKey(component.Id.SubtypeName) == false) {

					var waypoint = new WaypointProfile();
					waypoint.InitTags(component.DescriptionText);
					waypoint.ProfileSubtypeId = component.Id.SubtypeName;
					WaypointProfiles.Add(component.Id.SubtypeName, waypoint);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if ((component.DescriptionText.Contains("[RivalAI Weapons]") || component.DescriptionText.Contains("[MES AI Weapons]")) && WeaponProfiles.ContainsKey(component.Id.SubtypeName) == false) {

					var weapons = new WeaponSystemReference();
					weapons.InitTags(component.DescriptionText);
					weapons.ProfileSubtypeId = component.Id.SubtypeName;
					WeaponProfiles.Add(component.Id.SubtypeName, weapons);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if ((component.DescriptionText.Contains("[RivalAI Command]") || component.DescriptionText.Contains("[MES AI Command]")) && CommandProfiles.ContainsKey(component.Id.SubtypeName) == false) {

					var command = new CommandProfile();
					command.InitTags(component.DescriptionText);
					command.ProfileSubtypeId = component.Id.SubtypeName;
					CommandProfiles.Add(component.Id.SubtypeName, command);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

				if ((component.Id.SubtypeName.StartsWith("RivalAI-Datapad")) && !DatapadTemplates.ContainsKey(component.Id.SubtypeName)) {

					DatapadTemplates.Add(component.Id.SubtypeName, component);
					AllMesProfileIds.Add(component.Id);
					continue;

				}

			}

			//Fifth Phase
			foreach (var component in DefinitionHelper.EntityComponentDefinitions) {

				if ((component.DescriptionText.Contains("[RivalAI Behavior]") || component.DescriptionText.Contains("[Rival AI Behavior]") || component.DescriptionText.Contains("[MES AI Behavior]")) && BehaviorTemplates.ContainsKey(component.Id.SubtypeName) == false) {

					BehaviorTemplates.Add(component.Id.SubtypeName, component.DescriptionText);
					AllMesProfileIds.Add(component.Id);

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

			AutoPilotProfile profile = null;

			if (AutoPilotProfiles.TryGetValue(profileSubtypeId, out profile)) {

				if (profile != null || !string.IsNullOrWhiteSpace(profile.ProfileSubtypeId))
					return profile;

			}

			BehaviorLogger.Write("Warning: Autopilot Profile for " + profileSubtypeId + " Not Found!", BehaviorDebugEnum.BehaviorSetup);

			if (!string.IsNullOrWhiteSpace(defaultBehavior)) {

				if (AutoPilotProfiles.TryGetValue("RAI-Generic-Autopilot-" + defaultBehavior, out profile)) {

					if (profile != null || !string.IsNullOrWhiteSpace(profile.ProfileSubtypeId))
						return profile;

				}

			}

			BehaviorLogger.Write("Warning: Backup Autopilot Profile for " + defaultBehavior + " Not Found!", BehaviorDebugEnum.BehaviorSetup);

			return new AutoPilotProfile();

		}

		public static TextTemplate GetTextTemplate(string name) {

			TextTemplate template = null;

			if (TextTemplates.TryGetValue(name, out template))
				return template;

			string path = "Data\\TextTemplates\\" + name;

			foreach (var mod in MyAPIGateway.Session.Mods) {

				if (!MyAPIGateway.Utilities.FileExistsInModLocation(path, mod)) {

					continue;
				
				}

				//MyVisualScriptLogicProvider.ShowNotificationToAll("File Exists", 4000);

				try {

					var reader = MyAPIGateway.Utilities.ReadFileInModLocation(path, mod);
					string configcontents = reader.ReadToEnd();
					template = MyAPIGateway.Utilities.SerializeFromXML<TextTemplate>(configcontents);

				} catch (Exception exc) {

					continue;

				}

				if (template != null)
					break;

			}

			if (template == null)
				return null;

			TextTemplates[name] = template;
			return template;

		}

		public static StoreItemsContainer GetStoreItemContainer(string name) {

			StoreItemsContainer template = null;

			if (StoreItemContainers.TryGetValue(name, out template))
				return template;

			string path = "Data\\StoreItems\\" + name;

			foreach (var mod in MyAPIGateway.Session.Mods) {

				if (!MyAPIGateway.Utilities.FileExistsInModLocation(path, mod)) {

					continue;

				}

				SpawnLogger.Write("StoreItemsContainer Exists: " + name, SpawnerDebugEnum.Dev);

				try {

					var reader = MyAPIGateway.Utilities.ReadFileInModLocation(path, mod);
					string configcontents = reader.ReadToEnd();
					template = MyAPIGateway.Utilities.SerializeFromXML<StoreItemsContainer>(configcontents);

				} catch (Exception exc) {

					SpawnLogger.Write("Could Not Serialize From XML: " + name, SpawnerDebugEnum.Error);
					SpawnLogger.Write(exc.ToString(), SpawnerDebugEnum.Error);
					continue;

				}

				if (template != null)
					break;

			}

			if (template == null)
				return null;

			if (template.StoreItemsList == null)
				template.StoreItemsList = new List<StoreItem>();

			if (template.StoreItems != null) {

				for (int i = template.StoreItems.Length - 1; i >= 0; i--) {

					var item = template.StoreItems[i];
					template.StoreItemsList.Add(item);

					if (item.ItemSubtypeIds == null || item.ItemSubtypeIds.Length == 0)
						continue;

					foreach (var id in item.ItemSubtypeIds) {

						template.StoreItemsList.Add(new StoreItem(id, item));

					}

				}

				if (template.StoreItems.Length != template.StoreItemsList.Count)
					template.StoreItems = template.StoreItemsList.ToArray();

			}

			StoreItemContainers[name] = template;
			return template;

		}

		public static DialogueBank GetDialogueBank(string name)
		{

			DialogueBank dialogueBank = null;

			if (DialogueBanks.TryGetValue(name, out dialogueBank))
				return dialogueBank;

			string path = $"Data\\DialogueBanks\\{name}";

			foreach (var mod in MyAPIGateway.Session.Mods)
			{
				if (!MyAPIGateway.Utilities.FileExistsInModLocation(path, mod))
					continue;

				try
				{
					var reader = MyAPIGateway.Utilities.ReadFileInModLocation(path, mod);
					string configContents = reader.ReadToEnd();
					dialogueBank = MyAPIGateway.Utilities.SerializeFromXML<DialogueBank>(configContents);
				}
				catch (Exception exc)
				{
					SpawnLogger.Write($"Could not deserialize XML for dialogue bank: {name}", SpawnerDebugEnum.Error);
					SpawnLogger.Write(exc.ToString(), SpawnerDebugEnum.Error);
					continue;
				}

				if (dialogueBank != null)
					break;
			}

			if (dialogueBank == null)
				return null;

			if (dialogueBank.DialogueCues == null)
				dialogueBank.DialogueCues = new List<DialogueCue>();

			DialogueBanks[name] = dialogueBank;
			return dialogueBank;

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
