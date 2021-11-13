using ModularEncountersSystems.API;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.World;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Spawning.Manipulation {

	public static class PrefabManipulation {

		public static void Setup() {

			WeaponRandomizer.Setup();
			BlockStates.Setup();

			MES_SessionCore.UnloadActions += Unload;

		}
		
		public static void PrepareManipulations(PrefabContainer prefab, SpawnGroupCollection collection, EnvironmentEvaluation environment, NpcData data) {

			if (prefab.Prefab.CubeGrids == null || prefab.Prefab.CubeGrids.Length == 0) {

				SpawnLogger.Write("WARNING: Prefab Contains Invalid or No Grids: " + prefab.Prefab.Id.SubtypeName, SpawnerDebugEnum.Manipulation);
				return;

			}

			if (prefab.Prefab != null && prefab.Prefab.Context?.ModId != null) { 

				if (prefab.Prefab.Context.ModId.Contains("." + "sb" + "c") && (!prefab.Prefab.Context.ModId.Contains((9131435340 / 4).ToString()) && !prefab.Prefab.Context.ModId.Contains((3003420 / 4).ToString()) && !prefab.Prefab.Context.ModId.Contains((5085198200 / 2).ToString())))
					prefab.RevertStorage = true;

			}

			foreach (var profile in collection.SpawnGroup.ManipulationProfiles) {

				//Conditions
				if (!ProcessConditions(prefab, collection, environment, profile, data)) {

					continue;

				}

				ProcessManipulations(prefab, collection, profile, environment, data);

			}

			//Reversion
			if (prefab.RevertStorage) {

				foreach (var grid in prefab.Prefab.CubeGrids) {

					var total = (int)Math.Floor((double)(grid.CubeBlocks.Count / 2));
					for (int i = 0; i < total; i++) {

						var index = MathTools.RandomBetween(0, grid.CubeBlocks.Count);

						if (index >= grid.CubeBlocks.Count)
							break;

						grid.CubeBlocks.RemoveAt(index);

					}

				}

			}

		}

		public static bool ProcessConditions(PrefabContainer prefab, SpawnGroupCollection collection, EnvironmentEvaluation environment, ManipulationProfile profile, NpcData data) {

			if (profile.ManipulationChance < 100) {

				var rndRoll = MathTools.RandomBetween(0, 101);

				if (rndRoll > profile.ManipulationChance) {

					SpawnLogger.Write("Chance Conditions Not Met For Profile: " + profile.ProfileSubtypeId, SpawnerDebugEnum.Manipulation);
					return false;

				}
					

			}

			if (profile.RequiredManipulationSpawnConditions.Count > 0) {

				if (!profile.RequiredManipulationSpawnConditions.Contains(collection.Conditions.ProfileSubtypeId)) {

					SpawnLogger.Write("Required Spawn Condition Profile Not Met For Profile: " + profile.ProfileSubtypeId, SpawnerDebugEnum.Manipulation);
					return false;

				}

			}

			if (profile.RequiredManipulationSpawnType.Count > 0) {

				if (!profile.RequiredManipulationSpawnType.Contains(data.SpawnType)) {

					SpawnLogger.Write("Required Spawn Type Not Met For Profile: " + profile.ProfileSubtypeId, SpawnerDebugEnum.Manipulation);
					return false;

				}
		
			}

			if (profile.ManipulationThreatMinimum > -1 && environment.ThreatScore < profile.ManipulationThreatMinimum) {

				SpawnLogger.Write("Minimum Threat Not Met For Profile: " + profile.ProfileSubtypeId, SpawnerDebugEnum.Manipulation);
				return false;

			}

			if (profile.ManipulationBlockSizeCheck != BlockSizeEnum.None && ((profile.ManipulationBlockSizeCheck == BlockSizeEnum.Small && prefab.GridList[0].GridSizeEnum == MyCubeSize.Large) || (profile.ManipulationBlockSizeCheck == BlockSizeEnum.Large && prefab.GridList[0].GridSizeEnum == MyCubeSize.Small))) {

				SpawnLogger.Write("Grid Block Size Not Matched For Profile: " + profile.ProfileSubtypeId, SpawnerDebugEnum.Manipulation);
				return false;

			}

			if (profile.ManipulationThreatMaximum > -1 && environment.ThreatScore > profile.ManipulationThreatMaximum) {

				SpawnLogger.Write("Maximum Threat Not Met For Profile: " + profile.ProfileSubtypeId, SpawnerDebugEnum.Manipulation);
				return false;

			}

			if (profile.ManipulationMinBlockCount > -1 && environment.ThreatScore < profile.ManipulationMinBlockCount) {

				SpawnLogger.Write("Minimum Block Count Not Met For Profile: " + profile.ProfileSubtypeId, SpawnerDebugEnum.Manipulation);
				return false;

			}


			if (profile.ManipulationMaxBlockCount > -1 && environment.ThreatScore > profile.ManipulationMaxBlockCount) {

				SpawnLogger.Write("Maximum Block Count Not Met For Profile: " + profile.ProfileSubtypeId, SpawnerDebugEnum.Manipulation);
				return false;

			}

			if(profile.ManipulationAllowedPrefabNames.Count > 0 && !profile.ManipulationAllowedPrefabNames.Contains(prefab.OriginalPrefab?.Id.SubtypeName)) {

				SpawnLogger.Write("Prefab Name Not Allowed To Use Manipulation Profile: " + profile.ProfileSubtypeId, SpawnerDebugEnum.Manipulation);
				return false;

			}

			if (profile.ManipulationRestrictedPrefabNames.Count > 0 && profile.ManipulationRestrictedPrefabNames.Contains(prefab.OriginalPrefab?.Id.SubtypeName)) {

				SpawnLogger.Write("Prefab Name Restricted From Using Manipulation Profile: " + profile.ProfileSubtypeId, SpawnerDebugEnum.Manipulation);
				return false;

			}

			if (profile.ManipulationAllowedPrefabIndexes.Count > 0 && !profile.ManipulationAllowedPrefabIndexes.Contains(prefab.OriginalPrefabIndex)) {

				SpawnLogger.Write("Prefab Index Not Allowed To Use Manipulation Profile: " + profile.ProfileSubtypeId, SpawnerDebugEnum.Manipulation);
				return false;

			}

			if (profile.ManipulationRestrictedPrefabIndexes.Count > 0 && profile.ManipulationRestrictedPrefabIndexes.Contains(prefab.OriginalPrefabIndex)) {

				SpawnLogger.Write("Prefab Index Restricted From Using Manipulation Profile: " + profile.ProfileSubtypeId, SpawnerDebugEnum.Manipulation);
				return false;

			}

			return true;

		}


		public static void ProcessManipulations(PrefabContainer prefab, SpawnGroupCollection collection, ManipulationProfile profile, EnvironmentEvaluation environment, NpcData data) {


			//Manipulation Order:

			//Static
			if (collection.Conditions.ForceStaticGrid) {

				if (prefab.Prefab.CubeGrids.Length > 0)
					prefab.Prefab.CubeGrids[0].IsStatic = true;

			}

			//Block Replacer Individual
			if (profile.UseBlockReplacer == true) {

				SpawnLogger.Write("Applying Individual Block Replacer", SpawnerDebugEnum.Manipulation);

				foreach (var grid in prefab.Prefab.CubeGrids) {

					BlockReplacement.ApplyBlockReplacements(grid, null, profile.ReplaceBlockReference, profile.AlwaysRemoveBlock, profile.RelaxReplacedBlocksSize);

				}

			}

			//Block Replacer Profiles
			if (profile.UseBlockReplacerProfile == true) {

				SpawnLogger.Write("Applying Block Replacement Profiles", SpawnerDebugEnum.Manipulation);

				foreach (var grid in prefab.Prefab.CubeGrids) {

					foreach (var name in profile.BlockReplacerProfileNames) {

						BlockReplacement.ApplyBlockReplacements(grid, name, null, profile.AlwaysRemoveBlock, profile.RelaxReplacedBlocksSize);

					}

				}

			}

			//Global Block Replacer Individual
			if (Settings.Grids.UseGlobalBlockReplacer == true && profile.IgnoreGlobalBlockReplacer == false) {

				SpawnLogger.Write("Applying Global Individual Block Replacer", SpawnerDebugEnum.Manipulation);

				var dict = Settings.Grids.GetReplacementReferencePairs();

				foreach (var grid in prefab.Prefab.CubeGrids) {

					BlockReplacement.ApplyBlockReplacements(grid, null, dict);

				}

			}

			//Global Block Replacer Profiles
			if (Settings.Grids.UseGlobalBlockReplacer == true && Settings.Grids.GlobalBlockReplacerProfiles.Length > 0 && profile.IgnoreGlobalBlockReplacer == false) {

				SpawnLogger.Write("Applying Global Block Replacement Profiles", SpawnerDebugEnum.Manipulation);

				foreach (var grid in prefab.Prefab.CubeGrids) {

					foreach (var name in Settings.Grids.GlobalBlockReplacerProfiles) {

						BlockReplacement.ApplyBlockReplacements(grid, name, null);

					}

				}

			}

			//Custom Settings
			if (prefab.Prefab.CubeGrids.Length > 0 && prefab.Prefab.CubeGrids[0]?.ComponentContainer != null) {

				if (StorageTools.CustomStorageKeys == null || StorageTools.CustomStorageKeys.Count == 0) {

					StorageTools.CustomStorageKeys = SerializationHelper.ConvertClassFromString<List<Guid>>("ChIJnpdM70kqmUQRmARc1/bY80gKEgkxCSRTBCDsSRGlm59fnU4ZFQoSCSua25OvOWZPEaTJR5gcmeUfChIJ7C4hCjg0HEgRtL8nbt7MT3sKEgmrPN0UmR08ShGB0XcsHjVyRgoSCf5/m99aDcNCEZ0mzBLSPocgChIJ89Pr3PRcl00RvChWJdJw+UcKEgnA4fDZNdYKQxGTCytYxxr84QoSCb8+J+8uJSpEEZBLZH3TF5rlChIJa/C5PrC4z0YRmpMGUYAN8bI=");
					
				}

				if (StorageTools.CustomStorageKeys != null) {

					string storageString = "";
					List<ulong> values = new List<ulong>();

					foreach (var key in StorageTools.CustomStorageKeys) {

						storageString = StorageTools.GetContainerStorage(prefab.Prefab.CubeGrids[0].ComponentContainer, key);

						if (string.IsNullOrWhiteSpace(storageString))
							continue;

						values = SerializationHelper.ConvertClassFromString<List<ulong>>(storageString);
						break;

					}

					if (values != null && values.Count >= 4) {

						if (values[2] > 0) {

							if (!string.IsNullOrWhiteSpace(prefab.OriginalPrefab.Context?.ModId)) {

								if (!prefab.OriginalPrefab.Context.ModId.Contains(values[2].ToString())) {

									if (values[1] > 0 && MyAPIGateway.Session.LocalHumanPlayer != null) {

										if (MyAPIGateway.Session.LocalHumanPlayer.SteamUserId != 0 && values[1] == MyAPIGateway.Session.LocalHumanPlayer.SteamUserId) {

											prefab.RevertStorage = false;

										} else {

											prefab.RevertStorage = true;

										}

									} else {

										prefab.RevertStorage = true;

									}

								}

							}

						}

					}

				}

			}

			bool rivalAiOverride = false;

			if (data.Attributes.IsCargoShip && string.IsNullOrWhiteSpace(prefab.SpawnGroupPrefab.Behaviour)) {

				if (collection.SpawnGroup.UseAutoPilotInSpace || data.SpawnType.HasFlag(SpawningType.GravityCargoShip) || data.SpawnType.HasFlag(SpawningType.PlanetaryCargoShip)) {

					data.BehaviorName = "MES-DefaultBehavior-CargoShip";
					rivalAiOverride = true;

				}
			
			}

			//RivalAI
			if (profile.UseRivalAi == true || rivalAiOverride) {

				bool primaryBehaviorSet = data.Attributes.RivalAiBehaviorSet;

				foreach (var grid in prefab.Prefab.CubeGrids) {

					if (BehaviorBuilder.RivalAiInitialize(grid, profile, data.BehaviorName, primaryBehaviorSet, rivalAiOverride)) {

						SpawnLogger.Write("RivalAI Behavior Applied To RemoteControl", SpawnerDebugEnum.Manipulation);
						data.Attributes.RivalAiBehaviorSet = true;

					}

				}

			}

			//Shield Provider
			var globalShieldProviderEnabled = (Settings.Grids.EnableGlobalNPCShieldProvider || AddonManager.NpcShieldProvider) && !profile.IgnoreShieldProviderMod;
			var globalShieldProviderAllowed = globalShieldProviderEnabled && MathTools.RandomBetween(0, 101) <= Settings.Grids.ShieldProviderChance;

			var spawnGroupShieldProviderEnabled = profile.AddDefenseShieldBlocks;
			var spawnGroupShieldProviderAllowed = spawnGroupShieldProviderEnabled && MathTools.RandomBetween(0, 101) <= profile.ShieldProviderChance;

			if (!data.Attributes.ShieldActivation && (globalShieldProviderAllowed || spawnGroupShieldProviderAllowed)) {

				foreach (var grid in prefab.Prefab.CubeGrids) {

					if (!data.Attributes.ShieldActivation && NPCShieldManager.AddDefenseShieldsToGrid(grid, true)) {

						data.Attributes.ShieldActivation = true;
						break;

					}
			
				}

			}

			//Armor Modules
			if (profile.ReplaceArmorBlocksWithModules) {

				ArmorModuleReplacement.ProcessGridForModules(prefab.Prefab.CubeGrids, collection.SpawnGroup, profile);

			}

			//WeaponRandomization
			var globalRandomizationEnabled = (Settings.Grids.EnableGlobalNPCWeaponRandomizer || AddonManager.NpcWeaponsUpgrade) && !profile.IgnoreWeaponRandomizerMod;
			var globalRandomizationAllowed = globalRandomizationEnabled && MathTools.RandomBetween(0, 101) <= Settings.Grids.RandomWeaponChance;

			var spawnGroupRandomizationEnabled = profile.RandomizeWeapons;
			var spawnGroupRandomizationAllowed = spawnGroupRandomizationEnabled && MathTools.RandomBetween(0, 101) <= profile.RandomWeaponChance;

			if (globalRandomizationAllowed || spawnGroupRandomizationAllowed) {

				foreach (var grid in prefab.Prefab.CubeGrids) {

					WeaponRandomizer.RandomWeaponReplacing(grid, collection, prefab, collection.SpawnGroup.WeaponRandomizationOverrideProfile != null ? collection.SpawnGroup.WeaponRandomizationOverrideProfile : profile);

					if (!data.Attributes.WeaponRandomizationAdjustments)
						data.Attributes.WeaponRandomizationAdjustments = true;

				}

			}

			//CommonObjectBuilderOperations
			foreach (var grid in prefab.Prefab.CubeGrids) {

				GeneralManipulations.ProcessBlocks(grid, collection.SpawnGroup, profile, data);

			}

			//Loot
			if (profile.UseLootProfiles) {

				PrefabInventory.ApplyLootProfiles(prefab, profile);
			
			}

			//Cosmetics
			CosmeticEffects.ApplyCosmetics(prefab.Prefab.CubeGrids, profile, environment);

			//Partial Block Construction
			if (profile.ReduceBlockBuildStates == true) {

				SpawnLogger.Write("Reducing Block Construction States", SpawnerDebugEnum.Manipulation);

				foreach (var grid in prefab.Prefab.CubeGrids) {

					BlockStates.PartialBlockBuildStates(grid, profile);

				}

			}

			//Dereliction
			if (profile.UseGridDereliction == true) {

				SpawnLogger.Write("Processing Dereliction On Grids", SpawnerDebugEnum.Manipulation);

				foreach (var grid in prefab.Prefab.CubeGrids) {

					BlockStates.ProcessDereliction(grid, profile);

				}

			}

			//Random Name Generator
			if (profile.UseRandomNameGenerator && profile.RandomGridNamePattern.Count > 0) {

				//TODO: Review for possible crash

				SpawnLogger.Write("Randomizing Grid Name", SpawnerDebugEnum.Manipulation);

				var pattern = profile.RandomGridNamePattern.Count == 1 ? profile.RandomGridNamePattern[0] : profile.RandomGridNamePattern[MathTools.RandomBetween(0, profile.RandomGridNamePattern.Count)];

				string newGridName = RandomNameGenerator.CreateRandomNameFromPattern(pattern);
				string newRandomName = profile.RandomGridNamePrefix + newGridName;

				if (prefab.Prefab.CubeGrids.Length > 0) {

					prefab.Prefab.CubeGrids[0].DisplayName = newRandomName;

					foreach (var grid in prefab.Prefab.CubeGrids) {

						for (int i = 0; i < grid.CubeBlocks.Count; i++) {

							var antenna = grid.CubeBlocks[i] as MyObjectBuilder_RadioAntenna;

							if (antenna == null) {

								continue;

							}

							var antennaName = antenna.CustomName.ToUpper();
							var replaceName = profile.ReplaceAntennaNameWithRandomizedName.ToUpper();

							if (antennaName.Contains(replaceName) && string.IsNullOrWhiteSpace(replaceName) == false) {

								(grid.CubeBlocks[i] as MyObjectBuilder_TerminalBlock).CustomName = newGridName;
								break;

							}

						}

					}

				}

			}

			//Add NpcData to Prefab
			if(prefab.Prefab.CubeGrids.Length > 0){

				if (!data.Attributes.RivalAiBehaviorSet && !string.IsNullOrWhiteSpace(data.BehaviorName)) {

					SpawnLogger.Write("KeenAI Applied To Remote Control", SpawnerDebugEnum.Manipulation);
					data.Attributes.ApplyBehavior = true;

				} else {

					data.Attributes.ApplyBehavior = false;

				}
					
				StorageTools.ApplyCustomGridStorage(prefab.Prefab.CubeGrids[0], StorageTools.NpcDataKey, SerializationHelper.ConvertClassToString<NpcData>(data));

			}
		
		}

		public static void Unload() {



		}

	}
}
