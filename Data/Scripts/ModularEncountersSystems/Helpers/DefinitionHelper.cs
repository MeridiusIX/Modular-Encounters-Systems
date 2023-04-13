using ModularEncountersSystems.API;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Manipulation;
using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Game;

namespace ModularEncountersSystems.Helpers {
	public static class DefinitionHelper {

		//All
		public static DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> AllDefinitions = new DictionaryValuesReader<MyDefinitionId, MyDefinitionBase>();

		//Blocks
		public static List<MyCubeBlockDefinition> AllBlockDefinitions = new List<MyCubeBlockDefinition>();
		public static Dictionary<MyDefinitionId, MyCubeBlockDefinition> AllBlockDefinitionsDictionary = new Dictionary<MyDefinitionId, MyCubeBlockDefinition>();
		public static Dictionary<MyDefinitionId, float> BatteryMaxCapacityReference = new Dictionary<MyDefinitionId, float>();
		public static Dictionary<MyDefinitionId, MyReactorDefinition> ReactorDefinitions = new Dictionary<MyDefinitionId, MyReactorDefinition>();
		public static List<string> RivalAiControlModules = new List<string>();
		public static float HighestAntennaRange = 0;

		public static Dictionary<MyDefinitionId, MyWeaponBlockDefinition> WeaponBlockReferences = new Dictionary<MyDefinitionId, MyWeaponBlockDefinition>();
		public static List<MyDefinitionId> WeaponBlockIDs = new List<MyDefinitionId>();

		//Items
		public static Dictionary<MyDefinitionId, MyPhysicalItemDefinition> AllItemDefinitions = new Dictionary<MyDefinitionId, MyPhysicalItemDefinition>();

		public static Dictionary<MyDefinitionId, MyAmmoMagazineDefinition> NormalAmmoMagReferences = new Dictionary<MyDefinitionId, MyAmmoMagazineDefinition>();
		public static Dictionary<MyDefinitionId, MyAmmoDefinition> NormalAmmoReferences = new Dictionary<MyDefinitionId, MyAmmoDefinition>();

		public static Dictionary<MyDefinitionId, List<MyDefinitionId>> WeaponCoreAmmoReferences = new Dictionary<MyDefinitionId, List<MyDefinitionId>>();

		public static List<MyDefinitionId> ModSpecificItems = new List<MyDefinitionId>();

		//Entity Components
		public static List<MyComponentDefinitionBase> EntityComponentDefinitions = new List<MyComponentDefinitionBase>();

		//Weight
		public static Dictionary<MyDefinitionId, float> ItemWeightReference = new Dictionary<MyDefinitionId, float>();
		public static Dictionary<MyDefinitionId, float> BlockWeightReference = new Dictionary<MyDefinitionId, float>();

		//Volume
		public static Dictionary<MyDefinitionId, float> ItemVolumeReference = new Dictionary<MyDefinitionId, float>();
		public static Dictionary<MyDefinitionId, float> WeaponVolumeReference = new Dictionary<MyDefinitionId, float>();

		//Grids
		public static List<string> DropContainerNames = new List<string>();


		public static void Setup() {

			AllDefinitions = MyDefinitionManager.Static.GetAllDefinitions();

			SetupItems();
			
			SetupBlocks();

			SetupBlueprints();

			SetupEntityComponents();

			SetupDropPods();

			ArmorModuleReplacement.Setup();

			//Build List of RivalAI Control Module SubtypeNames
			RivalAiControlModules.Add("RivalAIRemoteControlSmall");
			RivalAiControlModules.Add("RivalAIRemoteControlLarge");
			RivalAiControlModules.Add("K_Imperial_Dropship_Guild_RC");
			RivalAiControlModules.Add("K_TIE_Fighter_RC");
			RivalAiControlModules.Add("K_NewRepublic_EWing_RC");
			RivalAiControlModules.Add("K_Imperial_RC_Largegrid");
			RivalAiControlModules.Add("K_TIE_Drone_Core");
			RivalAiControlModules.Add("K_Imperial_SpeederBike_FakePilot");
			RivalAiControlModules.Add("K_Imperial_ProbeDroid_Top_II");
			RivalAiControlModules.Add("K_Imperial_DroidCarrier_DroidBrain");
			RivalAiControlModules.Add("K_Imperial_DroidCarrier_DroidBrain_Aggressor");

			MES_SessionCore.UnloadActions += Unload;

		}

		internal static void SetupItems() {

			var errorDebug = new StringBuilder();

			try {

				var physicalItems = MyDefinitionManager.Static.GetPhysicalItemDefinitions();

				ModSpecificItems.Add(new MyDefinitionId(typeof(MyObjectBuilder_ConsumableItem), Encoding.UTF8.GetString(Convert.FromBase64String("SmV0cGFja0luaGliaXRvckJsb2NrZXI="))));
				ModSpecificItems.Add(new MyDefinitionId(typeof(MyObjectBuilder_ConsumableItem), Encoding.UTF8.GetString(Convert.FromBase64String("RHJpbGxJbmhpYml0b3JCbG9ja2Vy"))));
				ModSpecificItems.Add(new MyDefinitionId(typeof(MyObjectBuilder_ConsumableItem), Encoding.UTF8.GetString(Convert.FromBase64String("UGxheWVySW5oaWJpdG9yQmxvY2tlcg=="))));

				foreach (var item in physicalItems) {

					errorDebug.Clear();
					errorDebug.Append("Trying Definition As Item Definition").AppendLine();
					errorDebug.Append(item.Id).AppendLine();

					errorDebug.Append("Adding To Main Item Cache").AppendLine();
					//Main Item
					if (!AllItemDefinitions.ContainsKey(item.Id))
						AllItemDefinitions.Add(item.Id, item);

					errorDebug.Append("Adding Item Weight To Cache").AppendLine();
					//Weight
					if (!ItemWeightReference.ContainsKey(item.Id))
						ItemWeightReference.Add(item.Id, item.Mass);

					errorDebug.Append("Adding Item Volume To Cache").AppendLine();
					//Volume
					if (!ItemVolumeReference.ContainsKey(item.Id))
						ItemVolumeReference.Add(item.Id, item.Volume);

					errorDebug.Append("Checking if Item is Ammo").AppendLine();
					//Ammo
					if (item as MyAmmoMagazineDefinition != null) {

						errorDebug.Append("Item is Ammo").AppendLine();
						var ammoMag = item as MyAmmoMagazineDefinition;

						errorDebug.Append("Caching Ammo Magazine").AppendLine();
						if (!NormalAmmoMagReferences.ContainsKey(item.Id))
							NormalAmmoMagReferences.Add(item.Id, ammoMag);

						try {

							errorDebug.Append("Done Caching Ammo Magazine").AppendLine();
							var ammo = MyDefinitionManager.Static.GetAmmoDefinition(ammoMag.AmmoDefinitionId);
							errorDebug.Append("Caching Ammo Definition").AppendLine();

							if (ammo != null && !NormalAmmoReferences.ContainsKey(item.Id))
								NormalAmmoReferences.Add(item.Id, ammo);

						} catch (Exception e) {

							SpawnLogger.Write("WARNING: Ammo Magazine " + ammoMag.Id.ToString() + " Does Not Have A Valid MyAmmoDefinition Attached. Please Inform The Weapon Author.", SpawnerDebugEnum.Error, true);

						}

						errorDebug.Append("Done with Ammo Operations").AppendLine();

					}

					//Mod Items
					if (ModSpecificItems.Contains(item.Id)) {

						item.MinimalPricePerUnit = 100000000 / 2;
						item.CanSpawnFromScreen = false;

					}

					errorDebug.Append("Done with Item Operations").AppendLine();

				}


			} catch (Exception e) {

				SpawnLogger.Write("Encountered Definition Error During MES Item Setup", SpawnerDebugEnum.Error, true);
				SpawnLogger.Write(errorDebug.ToString(), SpawnerDebugEnum.Error, true);
				throw;

			}

		}

		internal static void SetupBlocks() {

			var errorDebug = new StringBuilder();

			try {

				foreach (var def in AllDefinitions) {

					errorDebug.Clear();
					errorDebug.Append("Trying Definition As Block Definition").AppendLine();
					errorDebug.Append(def.Id).AppendLine();

					var block = def as MyCubeBlockDefinition;

					if (block != null) {

						errorDebug.Append("Is Block Definition").AppendLine();
						AllBlockDefinitions.Add(block);

						if (!AllBlockDefinitionsDictionary.ContainsKey(block.Id))
							AllBlockDefinitionsDictionary.Add(block.Id, block);

						if (!BlockWeightReference.ContainsKey(block.Id)) {

							float totalWeight = 0;

							errorDebug.Append("Getting Block Weight Data").AppendLine();
							foreach (var comp in block.Components) {

								float weight = 0;

								if (ItemWeightReference.TryGetValue(comp.Definition.Id, out weight)) {

									totalWeight = weight * comp.Count;

								}

							}

							BlockWeightReference.Add(block.Id, totalWeight);
							errorDebug.Append("Added Block Weight Data").AppendLine();

						}

						//Battery Max Capacity
						var battery = block as MyBatteryBlockDefinition;

						if (battery != null) {

							errorDebug.Append("Block Is Battery, Getting Battery Data").AppendLine();
							if (!BatteryMaxCapacityReference.ContainsKey(battery.Id))
								BatteryMaxCapacityReference.Add(battery.Id, battery.MaxStoredPower);

							errorDebug.Append("Done Getting Battery Data").AppendLine();
							continue;

						}

						//Antenna
						var antenna = block as MyRadioAntennaDefinition;

						if (antenna != null) {

							if (antenna.MaxBroadcastRadius > HighestAntennaRange)
								HighestAntennaRange = antenna.MaxBroadcastRadius;

							continue;

						}

						//Beacon
						var beacon = block as MyBeaconDefinition;

						if (beacon != null) {

							if (beacon.MaxBroadcastRadius > HighestAntennaRange)
								HighestAntennaRange = beacon.MaxBroadcastRadius;

							continue;

						}

						//Weapon
						var weapon = block as MyWeaponBlockDefinition;

						if (weapon != null) {

							errorDebug.Append("Block Is Weapon").AppendLine();

							if (!WeaponBlockReferences.ContainsKey(weapon.Id)) {

								WeaponBlockReferences.Add(weapon.Id, weapon);
								WeaponBlockIDs.Add(weapon.Id);

							}

							errorDebug.Append("Caching Block Inventory Volume").AppendLine();

							if (!WeaponVolumeReference.ContainsKey(weapon.Id))
								WeaponVolumeReference.Add(weapon.Id, weapon.InventoryMaxVolume);

							errorDebug.Append("Done Getting Weapon Data").AppendLine();
							continue;

						}

						//Weapon-Sorter
						var weaponSorter = block as MyConveyorSorterDefinition;

						if (weaponSorter != null && BlockManager.AllWeaponCoreBlocks.Contains(block.Id)) {

							errorDebug.Append("Block is WeaponCore Sorter Based Block").AppendLine();

							if (!WeaponBlockIDs.Contains(weaponSorter.Id))
								WeaponBlockIDs.Add(weaponSorter.Id);

							errorDebug.Append("Caching Block Inventory Volume").AppendLine();

							if (!WeaponVolumeReference.ContainsKey(weaponSorter.Id))
								WeaponVolumeReference.Add(weaponSorter.Id, weaponSorter.InventorySize.X * weaponSorter.InventorySize.Y * weaponSorter.InventorySize.Z);

							errorDebug.Append("Done Getting Weapon Data").AppendLine();
							continue;

						}

					}

				}

			} catch (Exception e) {

				SpawnLogger.Write("Encountered Definition Error During MES Block Setup", SpawnerDebugEnum.Error, true);
				SpawnLogger.Write(errorDebug.ToString(), SpawnerDebugEnum.Error, true);
				throw;

			}

		}

		internal static void SetupBlueprints() {

			var errorDebug = new StringBuilder();

			try {

				var blueprints = MyDefinitionManager.Static.GetBlueprintDefinitions();
				var resultList = new List<MyBlueprintDefinitionBase.Item>();

				foreach (var id in blueprints) {

					errorDebug.Clear();
					errorDebug.Append("Trying Definition As Blueprint Definition").AppendLine();
					errorDebug.Append(id.Id).AppendLine();

					if (id.Results == null)
						continue;

					resultList.Clear();
					bool doChange = false;

					foreach (var result in id.Results) {

						if (ModSpecificItems.Contains(result.Id)) {

							var newResult = new MyBlueprintDefinitionBase.Item();
							newResult.Id = new MyDefinitionId(typeof(MyObjectBuilder_ConsumableItem), Encoding.UTF8.GetString(Convert.FromBase64String("Q291bnRlcmZlaXRJbmhpYml0b3JCbG9ja2Vy")));
							newResult.Amount = result.Amount;
							doChange = true;

						} else {

							resultList.Add(result);

						}

					}

					if (doChange)
						id.Results = resultList.ToArray();

					errorDebug.Append("Done With Blueprint Operation").AppendLine();

				}


			} catch (Exception e){

				SpawnLogger.Write("Encountered Definition Error During MES Blueprint Setup", SpawnerDebugEnum.Error, true);
				SpawnLogger.Write(errorDebug.ToString(), SpawnerDebugEnum.Error, true);
				throw;

			}
		
		}

		internal static void SetupEntityComponents() {

			var errorDebug = new StringBuilder();

			try {

				var entityComps = MyDefinitionManager.Static.GetEntityComponentDefinitions();

				foreach (var comp in entityComps) {

					errorDebug.Clear();
					errorDebug.Append("Trying Definition As Entity Component Definition").AppendLine();
					errorDebug.Append(comp.Id).AppendLine();

					if (comp != null && !string.IsNullOrWhiteSpace(comp.DescriptionText)) {

						EntityComponentDefinitions.Add(comp);

					}

				}


			} catch (Exception e) {

				SpawnLogger.Write("Encountered Definition Error During MES Entity Component Setup", SpawnerDebugEnum.Error, true);
				SpawnLogger.Write(errorDebug.ToString(), SpawnerDebugEnum.Error, true);
				throw;

			}

		}

		internal static void SetupDropPods() {

			var errorDebug = new StringBuilder();

			try {

				//DropPods
				var containers = MyDefinitionManager.Static.GetDropContainerDefinitions();

				foreach (var container in containers.Keys) {

					errorDebug.Clear();
					errorDebug.Append("Trying Definition As Drop Container Definition").AppendLine();
					errorDebug.Append(container).AppendLine();

					MyDropContainerDefinition drop = null;

					errorDebug.Append("Checking Drop Container Dictionary").AppendLine();
					if (!containers.TryGetValue(container, out drop))
						continue;

					errorDebug.Append("Checking CubeGrids").AppendLine();
					if (drop.Prefab?.CubeGrids == null)
						continue;

					if (drop.Prefab.CubeGrids.Length == 0)
						continue;

					if (string.IsNullOrWhiteSpace(drop.Prefab.CubeGrids[0].DisplayName))
						continue;

					if (!DropContainerNames.Contains(drop.Prefab.CubeGrids[0].DisplayName))
						DropContainerNames.Add(drop.Prefab.CubeGrids[0].DisplayName);

					errorDebug.Append("Finished With Drop Container Definition").AppendLine();

				}


			} catch (Exception e) {

				SpawnLogger.Write("Encountered Definition Error During MES Entity Component Setup", SpawnerDebugEnum.Error, true);
				SpawnLogger.Write(errorDebug.ToString(), SpawnerDebugEnum.Error, true);
				throw;

			}

		}

		public static string GetBlockDefinitionInfo() {

			var sb = new StringBuilder();

			foreach (var def in AllBlockDefinitions) {

				if (def == null) {

					continue;

				}

				sb.Append("Block Name:           ").Append(def.DisplayNameText).AppendLine();
				sb.Append("Block ID:             ").Append(def.Id.ToString()).AppendLine();

				if (def.Context?.ModId != null) {

					if (string.IsNullOrWhiteSpace(def.Context.ModId) == false) {

						sb.Append("Mod ID:               ").Append(def.Context.ModId).AppendLine();

					}

				}
				
				sb.Append("Is Public:            ").Append(def.Public.ToString()).AppendLine();
				sb.Append("Size:                 ").Append(def.CubeSize.ToString()).AppendLine();

				if (BlockManager.AllWeaponCoreBlocks.Contains(def.Id)) {

					sb.Append("Is WeaponCore Static: ").Append(BlockManager.AllWeaponCoreGuns.Contains(def.Id)).AppendLine();
					sb.Append("Is WeaponCore Turret: ").Append(BlockManager.AllWeaponCoreTurrets.Contains(def.Id)).AppendLine();


				} else {

					sb.Append("Is Weapon:            ").Append(WeaponBlockIDs.Contains(def.Id)).AppendLine();

				}

				sb.AppendLine();

			}

			return sb.ToString();

		}

		public static void UnlockNpcBlocks() {

			foreach (var block in AllBlockDefinitions) {

				if (!string.IsNullOrWhiteSpace(block?.Context?.ModId)) {

					if (block.Context.ModId.Contains("1521905890") || block.Context.ModId.Contains("Modular Encounters Systems") || (block.Context?.ModId ?? "") == (MES_SessionCore.Instance.ModContext?.ModId ?? "N/A")) {

						block.Public = true;
					
					}
				
				}
			
			}
		
		}

		public static void Unload() {
		
			
		
		}

	}
}
