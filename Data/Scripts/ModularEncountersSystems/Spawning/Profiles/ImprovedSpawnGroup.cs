using ModularEncountersSystems.Helpers;
using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Profiles {

	public class ImprovedSpawnGroup {

		public bool SpawnGroupEnabled;
		public string SpawnGroupName;
		public MySpawnGroupDefinition SpawnGroup;

		public List<SpawnConditionsProfile> SpawnConditionsProfiles;

		public int Frequency;
		public string FactionOwner;
		public bool UseRandomMinerFaction;
		public bool UseRandomBuilderFaction;
		public bool UseRandomTraderFaction;
		public bool IgnoreCleanupRules;


		public bool ReplenishSystems;
		public List<ReplenishmentProfile> ReplenishProfiles;
		public bool IgnoreGlobalReplenishProfiles;

		public bool UseNonPhysicalAmmo;
		public bool RemoveContainerContents;
		public bool InitializeStoreBlocks;
		public List<string> ContainerTypesForStoreOrders;

		public List<ManipulationProfile> ManipulationProfiles;

		public bool UseAutoPilotInSpace;
		public double PauseAutopilotAtPlayerDistance;

		public bool PreventOwnershipChange; //Implement / Doc

		public ImprovedSpawnGroup() {

			SpawnGroupEnabled = true;
			SpawnGroupName = "";
			SpawnGroup = null;

			SpawnConditionsProfiles = new List<SpawnConditionsProfile>();
			SpawnConditionsProfiles.Add(new SpawnConditionsProfile());

			Frequency = 0;
			
			FactionOwner = "SPRT";
			UseRandomMinerFaction = false;
			UseRandomBuilderFaction = false;
			UseRandomTraderFaction = false;
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

			UseAutoPilotInSpace = false;
			PauseAutopilotAtPlayerDistance = -1;

			PreventOwnershipChange = false;

		}

		public void InitTags(MySpawnGroupDefinition spawnGroup) {

			if (string.IsNullOrWhiteSpace(spawnGroup.DescriptionText))
				return;

			var improveSpawnGroup = this;
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

				//SpawnConditionsProfiles
				if (tag.StartsWith("[SpawnConditionsProfiles:") == true) {

					TagParse.TagSpawnConditionsProfileCheck(tag, ref improveSpawnGroup.SpawnConditionsProfiles);

				}

				//Frequency
				improveSpawnGroup.Frequency = (int)Math.Round((double)spawnGroup.Frequency * 10);

				//FactionOwner
				if (tag.StartsWith("[FactionOwner:") == true) {

					TagParse.TagStringCheck(tag, ref improveSpawnGroup.FactionOwner);

					if (improveSpawnGroup.FactionOwner == "") {

						improveSpawnGroup.FactionOwner = "SPRT";

					}

				}

				//UseRandomMinerFaction
				if (tag.StartsWith("[UseRandomMinerFaction:") == true) {

					TagParse.TagBoolCheck(tag, ref improveSpawnGroup.UseRandomMinerFaction);

				}

				//UseRandomBuilderFaction
				if (tag.StartsWith("[UseRandomBuilderFaction:") == true) {

					TagParse.TagBoolCheck(tag, ref improveSpawnGroup.UseRandomBuilderFaction);

				}

				//UseRandomTraderFaction
				if (tag.StartsWith("[UseRandomTraderFaction:") == true) {

					TagParse.TagBoolCheck(tag, ref improveSpawnGroup.UseRandomTraderFaction);

				}

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

				//PauseAutopilotAtPlayerDistance
				if (tag.StartsWith("[PauseAutopilotAtPlayerDistance:") == true) {

					TagParse.TagDoubleCheck(tag, ref improveSpawnGroup.PauseAutopilotAtPlayerDistance);

				}

				//PreventOwnershipChange
				if (tag.StartsWith("[PreventOwnershipChange:") == true) {

					TagParse.TagBoolCheck(tag, ref improveSpawnGroup.PreventOwnershipChange);

				}

			}

		}

	}

}