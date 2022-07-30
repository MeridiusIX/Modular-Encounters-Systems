using ModularEncountersSystems.Helpers;
using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Profiles {

	public class ImprovedSpawnGroup {

		public bool SpawnGroupEnabled;
		public bool MesSpawnGroup;
		public string SpawnGroupName;
		public MySpawnGroupDefinition SpawnGroup;

		public SpawnConditionsProfile PersistentConditions;
		public bool UseFirstConditionsAsPersistent;

		public List<SpawnConditionsProfile> SpawnConditionsProfiles;
		public List<string> SpawnConditionGroups;

		public string FactionOverride;

		public int Frequency;
		public bool IgnoreCleanupRules;

		public bool ReplenishSystems;
		public List<ReplenishmentProfile> ReplenishProfiles;
		public bool IgnoreGlobalReplenishProfiles;

		public bool UseNonPhysicalAmmo;
		public bool RemoveContainerContents;
		public bool InitializeStoreBlocks;
		public List<string> ContainerTypesForStoreOrders;

		public List<ManipulationProfile> ManipulationProfiles;
		public List<string> ManipulationGroups;
		public ManipulationProfile WeaponRandomizationOverrideProfile;

		public bool UseAutoPilotInSpace;
		public double PauseAutopilotAtPlayerDistance;

		public bool PreventOwnershipChange; //Implement / Doc

		public ImprovedSpawnGroup() {

			SpawnGroupEnabled = true;
			MesSpawnGroup = false;
			SpawnGroupName = "";
			SpawnGroup = null;

			PersistentConditions = null;
			UseFirstConditionsAsPersistent = false;

			SpawnConditionsProfiles = new List<SpawnConditionsProfile>();
			SpawnConditionsProfiles.Add(new SpawnConditionsProfile());

			SpawnConditionGroups = new List<string>();

			FactionOverride = "";

			Frequency = 0;

			IgnoreCleanupRules = false;

			ReplenishSystems = false;
			ReplenishProfiles = new List<ReplenishmentProfile>();
			IgnoreGlobalReplenishProfiles = false;

			UseNonPhysicalAmmo = false;
			RemoveContainerContents = false;
			InitializeStoreBlocks = false;
			ContainerTypesForStoreOrders = new List<string>();

			ManipulationProfiles = new List<ManipulationProfile>();
			ManipulationProfiles.Add(new ManipulationProfile());
			WeaponRandomizationOverrideProfile = null;

			ManipulationGroups = new List<string>();

			UseAutoPilotInSpace = false;
			PauseAutopilotAtPlayerDistance = -1;

			PreventOwnershipChange = false;

		}

		public void InitTags(MySpawnGroupDefinition spawnGroup) {

			if (string.IsNullOrWhiteSpace(spawnGroup.DescriptionText))
				return;

			var improveSpawnGroup = this;
			MesSpawnGroup = true;
			var descSplit = spawnGroup.DescriptionText.Split('\n');

			improveSpawnGroup.SpawnGroup = spawnGroup;
			improveSpawnGroup.SpawnGroupName = spawnGroup.Id.SubtypeName;

			bool setDampeners = false;
			bool setAtmoRequired = false;
			bool setForceStatic = false;

			SpawnConditionsProfiles[0].ProfileSubtypeId = SpawnGroupName;
			SpawnConditionsProfiles[0].InitTags(spawnGroup.DescriptionText);
			ManipulationProfiles[0].ProfileSubtypeId = SpawnGroupName;
			ManipulationProfiles[0].InitTags(spawnGroup.DescriptionText);

			foreach (var tagRaw in descSplit) {

				var tag = tagRaw.Trim();

				//SpawnGroupEnabled
				if (tag.StartsWith("[SpawnGroupEnabled:") == true) {

					TagParse.TagBoolCheck(tag, ref improveSpawnGroup.SpawnGroupEnabled);

				}

				//PersistentConditions
				if (tag.StartsWith("[PersistentConditions:") == true) {

					TagParse.TagSpawnConditionsProfileCheck(tag, ref improveSpawnGroup.PersistentConditions);

				}

				//UseFirstConditionsAsPersistent
				if (tag.StartsWith("[UseFirstConditionsAsPersistent:") == true) {

					TagParse.TagBoolCheck(tag, ref improveSpawnGroup.UseFirstConditionsAsPersistent);

				}

				//SpawnConditionsProfiles
				if (tag.StartsWith("[SpawnConditionsProfiles:") == true) {

					TagParse.TagSpawnConditionsProfileCheck(tag, ref improveSpawnGroup.SpawnConditionsProfiles);

				}

				//SpawnConditionGroups
				if (tag.StartsWith("[SpawnConditionGroups:") == true) {

					TagParse.TagStringListCheck(tag, ref improveSpawnGroup.SpawnConditionGroups);

				}

				//FactionOverride
				if (tag.StartsWith("[FactionOverride:") == true) {

					TagParse.TagStringCheck(tag, ref improveSpawnGroup.FactionOverride);

				}

				//Frequency
				improveSpawnGroup.Frequency = (int)Math.Round((double)spawnGroup.Frequency * 10);

				//IgnoreCleanupRules
				if (tag.StartsWith("[IgnoreCleanupRules:") == true) {

					TagParse.TagBoolCheck(tag, ref improveSpawnGroup.IgnoreCleanupRules);

				}

				//ReplenishSystems
				if (tag.StartsWith("[ReplenishSystems:") == true) {

					TagParse.TagBoolCheck(tag, ref improveSpawnGroup.ReplenishSystems);

				}

				//ReplenishProfiles
				if (tag.StartsWith("[ReplenishProfiles:") == true) {

					TagParse.TagReplenishProfileCheck(tag, ref improveSpawnGroup.ReplenishProfiles);

				}

				//IgnoreGlobalReplenishProfiles
				if (tag.StartsWith("[IgnoreGlobalReplenishProfiles:") == true) {

					TagParse.TagBoolCheck(tag, ref improveSpawnGroup.IgnoreGlobalReplenishProfiles);

				}

				//UseNonPhysicalAmmo
				if (tag.StartsWith("[UseNonPhysicalAmmo:") == true) {

					TagParse.TagBoolCheck(tag, ref improveSpawnGroup.UseNonPhysicalAmmo);

				}

				//RemoveContainerContents
				if (tag.StartsWith("[RemoveContainerContents:") == true) {

					TagParse.TagBoolCheck(tag, ref improveSpawnGroup.RemoveContainerContents);

				}

				//InitializeStoreBlocks
				if (tag.StartsWith("[InitializeStoreBlocks:") == true) {

					TagParse.TagBoolCheck(tag, ref improveSpawnGroup.InitializeStoreBlocks);

				}

				//ContainerTypesForStoreOrders
				if (tag.StartsWith("[ContainerTypesForStoreOrders:") == true) {

					TagParse.TagStringListCheck(tag, ref improveSpawnGroup.ContainerTypesForStoreOrders);

				}

				//UseAutoPilotInSpace
				if (tag.StartsWith("[UseAutoPilotInSpace:") == true) {

					TagParse.TagBoolCheck(tag, ref improveSpawnGroup.UseAutoPilotInSpace);

				}

				//ManipulationProfiles
				if (tag.StartsWith("[ManipulationProfiles:") == true) {

					TagParse.TagManipulationProfileCheck(tag, ref improveSpawnGroup.ManipulationProfiles);

				}

				//ManipulationGroups
				if (tag.StartsWith("[ManipulationGroups:") == true) {

					TagParse.TagStringListCheck(tag, ref improveSpawnGroup.ManipulationGroups);

				}

				//WeaponRandomizationOverrideProfile
				if (tag.StartsWith("[WeaponRandomizationOverrideProfile:") == true) {

					TagParse.TagManipulationProfileCheck(tag, ref improveSpawnGroup.WeaponRandomizationOverrideProfile);

				}

				//PauseAutopilotAtPlayerDistance
				if (tag.StartsWith("[PauseAutopilotAtPlayerDistance:") == true) {

					TagParse.TagDoubleCheck(tag, ref improveSpawnGroup.PauseAutopilotAtPlayerDistance);

				}

				//PreventOwnershipChange
				if (tag.StartsWith("[PreventOwnershipChange:") == true) {

					TagParse.TagBoolCheck(tag, ref improveSpawnGroup.PreventOwnershipChange);

				}

			}

			//Spawn Condition Groups
			foreach (var conditionName in SpawnConditionGroups) {

				if (string.IsNullOrWhiteSpace(conditionName))
					continue;

				SpawnConditionsGroup group = null;

				if (ProfileManager.SpawnConditionGroups.TryGetValue(conditionName, out group)) {

					foreach (var condition in group.SpawnConditionProfiles) {

						if (!SpawnConditionsProfiles.Contains(condition))
							SpawnConditionsProfiles.Add(condition);

					}
				
				}
			
			}

			//Manipulation Groups
			foreach (var manipulationName in ManipulationGroups) {

				if (string.IsNullOrWhiteSpace(manipulationName))
					continue;

				ManipulationGroup group = null;

				if (ProfileManager.ManipulationGroups.TryGetValue(manipulationName, out group)) {

					foreach (var condition in group.ManipulationProfiles) {

						if (!ManipulationProfiles.Contains(condition))
							ManipulationProfiles.Add(condition);

					}

				}

			}

			if (SpawnGroupManager.AddonDetected) {

				foreach (var manipulation in ManipulationProfiles) {

					if (manipulation.ModulesForArmorReplacement.Count > 0 && manipulation.ReplaceArmorBlocksWithModules) {

						improveSpawnGroup.SpawnGroupEnabled = false;
						break;

					}
					
				}

			}
				
		}

	}

}