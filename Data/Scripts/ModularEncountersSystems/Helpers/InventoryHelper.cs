using ModularEncountersSystems.API;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Spawning.Profiles;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;
using static ModularEncountersSystems.API.WcApiDef;

namespace ModularEncountersSystems.Helpers {
	public static class InventoryHelper {

		private static Dictionary<MyDefinitionId, MyPhysicalItemDefinition> _cachedItemDefinitions = new Dictionary<MyDefinitionId, MyPhysicalItemDefinition>();

		public static void ReplenishGridSystems(IMyCubeGrid grid, ImprovedSpawnGroup spawnGroup) {

			try {

				var blocks = BlockCollectionHelper.GetBlocksOfType<IMyTerminalBlock>(grid);

				foreach (var block in blocks) {

					if (block?.SlimBlock.BlockDefinition == null || !block.HasInventory)
						continue;

					MyInventory inventory = (block as MyEntity).GetInventory();

					if (inventory == null)
						continue;

					//Weapons (WeaponCore)
					if (BlockManager.AllWeaponCoreBlocks.Contains(block.SlimBlock.BlockDefinition.Id)) {

						WeaponCoreReplenishment(block, inventory, spawnGroup);
						continue;
					
					}

					//Weapons (Regular)
					if (block as IMyUserControllableGun != null) {

						SimpleBlockReplenish(block, inventory, spawnGroup);
						continue;

					}

					//Reactors
					if (block as IMyReactor != null) {

						var powerDef = block.SlimBlock.BlockDefinition as MyReactorDefinition;

						if (powerDef != null) {

							var totalFuelAdd = (MyFixedPoint)powerDef.MaxPowerOutput;
							SimpleBlockReplenish(block, inventory, spawnGroup, (double)totalFuelAdd);

						}

						continue;

					}

					//GasGenerators
					if (block as IMyGasGenerator != null) {

						SimpleBlockReplenish(block, inventory, spawnGroup);
						continue;

					}

					//Parachute
					if (block as IMyParachute != null) {

						SimpleBlockReplenish(block, inventory, spawnGroup);
						continue;

					}

				}

			} catch (Exception e) {

				SpawnLogger.Write("Grid Replenish Failed With Error", SpawnerDebugEnum.Error, true);
				SpawnLogger.Write(e.ToString(), SpawnerDebugEnum.Error, true);
			
			}
		
		}

		public static void SimpleBlockReplenish(IMyTerminalBlock block, MyInventory inventory, ImprovedSpawnGroup spawnGroup, double cap = -1) {

			var items = GetCompatibleItemTypes(null, inventory);

			foreach (var item in items) {

				if (item.SubtypeName == "NATO_5p56x45mm")
					continue;

				var amount = GetMaxAddCountForReplenish(block, inventory, item, spawnGroup.ReplenishProfiles, spawnGroup.IgnoreGlobalReplenishProfiles, cap);
				AddItemsToInventory(inventory, item, (float)amount);
				break;

			}

		}

		public static bool AddItemsToInventory(MyInventory inventory, MyDefinitionId itemId, float amount = -1, bool ignoreConstraintCheck = false, string source = null) {

			if (source != null) {
			
				
			
			}

			var itemDef = GetItemDefinition(itemId);

			if (itemDef == null) {

				//SpawnLogger.Write(" - Inventory Item Definition Null", SpawnerDebugEnum.PostSpawn);
				return false;

			}
				

			float freeSpace = (float)(inventory.MaxVolume - inventory.CurrentVolume);
			var amountToAdd = Math.Floor(freeSpace / itemDef.Volume);

			if (amountToAdd > amount && amount > -1) {

				amountToAdd = amount;

			}

			var canAdd = ignoreConstraintCheck || inventory.CanItemsBeAdded((MyFixedPoint)amountToAdd, itemId);

			if (amountToAdd > 0 && canAdd) {

				//SpawnLogger.Write(string.Format("Replenish Added {0} of {1} to Inventory Block.", amountToAdd, itemId), SpawnerDebugEnum.PostSpawn);
				inventory.AddItems((MyFixedPoint)amountToAdd, MyObjectBuilderSerializer.CreateNewObject(itemId));
				return true;

			} else {

				//SpawnLogger.Write(string.Format(" - Add Failed For Reasons: [Amount : {0}] [Can Add : {1}]", amountToAdd, canAdd), SpawnerDebugEnum.PostSpawn);

			}

			return false;

		}

		public static double GetMaxAddCountForReplenish(IMyTerminalBlock block, MyInventory inventory, MyDefinitionId itemId, List<ReplenishmentProfile> profiles, bool ignoreGlobalProfiles, double initialCap = -1) {

			MyPhysicalItemDefinition item = GetItemDefinition(itemId);

			if (item == null)
				return 0;

			double result = 0;
			float freeSpace = (float)(inventory.MaxVolume - inventory.CurrentVolume);

			if (initialCap <= -1) {

				result = Math.Floor(freeSpace / item.Volume);

			} else {

				var resultB = Math.Floor(freeSpace / item.Volume);

				if (resultB > initialCap)
					result = initialCap;
				else
					result = resultB;

			}

			//SpawnLogger.Write(string.Format("Pre Weapon Checking for Mass Restrictions on Ammo: {0}", itemId), SpawnerDebugEnum.PostSpawn);

			if (DefinitionHelper.WeaponBlockIDs.Contains(block.SlimBlock.BlockDefinition.Id)) {

				//SpawnLogger.Write(string.Format("Checking for Mass Restrictions on Ammo: {0}", itemId), SpawnerDebugEnum.PostSpawn);

				if (Settings.Grids.UseMaxAmmoInventoryWeight) {

					var currentMass = (float)inventory.CurrentMass;

					if (currentMass >= Settings.Grids.MaxAmmoInventoryWeight)
						return 0;

					float itemMass = 0;

					if (DefinitionHelper.ItemWeightReference.TryGetValue(itemId, out itemMass)) {

						var allowedCount = Math.Floor((Settings.Grids.MaxAmmoInventoryWeight - currentMass) / itemMass);

						if (allowedCount == 0)
							allowedCount = 1;

						if (result > allowedCount)
							result = allowedCount;

						//SpawnLogger.Write(string.Format("Allowed Ammo Weight For {0} set to {1} from Allowed Count {2}.", itemId, result, allowedCount), SpawnerDebugEnum.PostSpawn);

					}
					
				}

			}

			foreach (var profile in profiles) {

				if (profile.RestrictedItems.Contains(item.Id)) {

					result = 0;
					break;

				}

				float limit = 0;

				if (profile.MaxItems.TryGetValue(item.Id, out limit))
					if (limit < result)
						result = limit;
			
			}

			if (result == 0)
				return result;

			if (!ignoreGlobalProfiles) {

				foreach (var profileName in Settings.Grids.GlobalReplenishmentProfiles) {

					ReplenishmentProfile profile = null;

					if (!ProfileManager.ReplenishmentProfiles.TryGetValue(profileName, out profile)) {

						SpawnLogger.Write("Global Replenishment Profile Name [" + profileName + "] Has No Registered Profile.", SpawnerDebugEnum.PostSpawn);
						continue;

					}
						

					if (profile.RestrictedItems.Contains(item.Id)) {

						result = 0;
						break;

					}

					float limit = 0;

					if (profile.MaxItems.TryGetValue(item.Id, out limit)) {

						if (limit < result) {

							//SpawnLogger.Write(string.Format("{0} Has Replenishment Limit of {1}", item.Id, limit), SpawnerDebugEnum.PostSpawn);
							result = limit;

						}

					}
					
				}
			
			}

			return result;
		
		}

		public static void WeaponCoreReplenishment(IMyTerminalBlock block, MyInventory inventory, ImprovedSpawnGroup spawnGroup) {

			//SpawnLogger.Write(string.Format("Replenishing WeaponCore Ammo For Block: {0}", block.SlimBlock.BlockDefinition.Id), SpawnerDebugEnum.PostSpawn);
			APIs.WeaponCore.DisableRequiredPower(block as MyEntity);
			var ammoList = GetCompatibleWeaponCoreAmmos(block);

			if (ammoList.Count == 0) {

				//SpawnLogger.Write(string.Format(" - Compatible Ammo: 0"), SpawnerDebugEnum.PostSpawn);
				return;

			}
				
			//Fill Ammos - 
			int totalMagazines = 0;
			int totalLoopRuns = 0;
			var maxMagazinesPerAmmoType = new Dictionary<MyDefinitionId, Vector2I>();

			foreach (var ammoId in ammoList) {

				double maxAmmo = GetMaxAddCountForReplenish(block, inventory, ammoId, spawnGroup.ReplenishProfiles, spawnGroup.IgnoreGlobalReplenishProfiles);

				if (!maxMagazinesPerAmmoType.ContainsKey(ammoId))
					maxMagazinesPerAmmoType.Add(ammoId, new Vector2I(0, (int)maxAmmo));

			}

			int maxLoopRuns = 150; //TODO: Make This A Config Somewhere?

			while (totalLoopRuns < maxLoopRuns) {

				totalLoopRuns++;
				bool noLoop = true;
				bool addedItem = false;

				foreach (var ammoId in ammoList) {

					if (Settings.Grids.UseMaxAmmoInventoryWeight && (float)inventory.CurrentMass >= Settings.Grids.MaxAmmoInventoryWeight) {

						//SpawnLogger.Write(string.Format(" - Ammo Weight Exceeded or Reached"), SpawnerDebugEnum.PostSpawn);
						noLoop = true;
						break;
					
					}

					if (ammoId.SubtypeName == "Energy")
						continue;

					Vector2I ammoAdd = Vector2I.Zero;

					if (!maxMagazinesPerAmmoType.TryGetValue(ammoId, out ammoAdd) || ammoAdd.X >= ammoAdd.Y)
						continue;

					noLoop = false;

					if (AddItemsToInventory(inventory, ammoId, 1, true)) {

						ammoAdd.X++;
						maxMagazinesPerAmmoType[ammoId] = ammoAdd;
						totalMagazines++;
						addedItem = true;

					} else {

						//SpawnLogger.Write(string.Format(" - Item [{0}] Could Not Be Added To Inventory", ammoId), SpawnerDebugEnum.PostSpawn);
					
					}

				}

				if (noLoop || !addedItem)
					break;

			}

		}

		public static HashSetReader<MyDefinitionId> GetCompatibleItemTypes(IMyTerminalBlock block, MyInventory inventory) {

			if (inventory == null) {

				var entity = block as MyEntity;

				if (block == null || !block.HasInventory) {

					return new HashSetReader<MyDefinitionId>();

				}

				inventory = entity.GetInventory();

			}

			if (inventory == null) {

				//Logger.Write("MyInventory is null", true);
				return new HashSetReader<MyDefinitionId>();

			}

			return inventory.Constraint.ConstrainedIds;

		}

		public static List<MyDefinitionId> GetCompatibleWeaponCoreAmmos(IMyTerminalBlock block) {

			List<MyDefinitionId> list = null;

			if (DefinitionHelper.WeaponCoreAmmoReferences.TryGetValue(block.SlimBlock.BlockDefinition.Id, out list))
				return list;

			list = new List<MyDefinitionId>();
			var weaponsInBlock = new Dictionary<string, int>();
			APIs.WeaponCore.GetBlockWeaponMap(block, weaponsInBlock);

			foreach (var weaponName in weaponsInBlock.Keys) {

				WeaponDefinition weaponDef = new WeaponDefinition();

				foreach (var definition in APIs.WeaponCore.WeaponDefinitions) {

					if (definition.HardPoint.PartName == weaponName) {

						weaponDef = definition;
						break;

					}

				}

				foreach (var ammo in weaponDef.Ammos) {

					var ammoMagDefId = new MyDefinitionId(typeof(MyObjectBuilder_AmmoMagazine), ammo.AmmoMagazine);

					if (!list.Contains(ammoMagDefId))
						list.Add(ammoMagDefId);
				
				}

			}

			DefinitionHelper.WeaponCoreAmmoReferences.Add(block.SlimBlock.BlockDefinition.Id, list);
			return list;

		}

		private static MyPhysicalItemDefinition GetItemDefinition(MyDefinitionId itemId) {

			MyPhysicalItemDefinition item = null;

			if (_cachedItemDefinitions.TryGetValue(itemId, out item))
				return item;

			if (DefinitionHelper.AllItemDefinitions.TryGetValue(itemId, out item)) {

				_cachedItemDefinitions.Add(itemId, item);

			}

			return item;

		}

		public static void NonPhysicalAmmoProcessing(IMyCubeGrid cubeGrid) {

			if (cubeGrid == null || MyAPIGateway.Entities.Exist(cubeGrid) == false) {

				return;

			}

			var blockList = BlockCollectionHelper.GetBlocksOfType<IMyUserControllableGun>(cubeGrid);

			foreach (var block in blockList) {

				try {

					if (block.GetInventory().Empty() == true) {

						continue;

					}

					if (BlockManager.AllWeaponCoreBlocks.Contains(block.SlimBlock.BlockDefinition.Id))
						continue;

					var firstItem = block.GetInventory().GetItems()[0];
					var ammoMagId = new MyDefinitionId(firstItem.Content.TypeId, firstItem.Content.SubtypeName);
					MyAmmoMagazineDefinition ammoMagDefinition = null;

					if (!DefinitionHelper.NormalAmmoMagReferences.TryGetValue(ammoMagId, out ammoMagDefinition)) {

						continue;

					}

					int amount = ammoMagDefinition.Capacity * (int)firstItem.Amount;

					var gunbase = (IMyGunObject<MyGunBase>)block;

					if (gunbase?.GunBase == null) {

						continue;

					}

					block.GetInventory().Clear();
					gunbase.GunBase.CurrentAmmo = amount;

				} catch (Exception e) {

					SpawnLogger.Write("Issue Processing Non-Physical Ammo For Grid: " + cubeGrid.CustomName + " - Block: " + block.CustomName, SpawnerDebugEnum.Error, true);

				}

			}

		}

		public static void ApplyContainerTypes(GridEntity grid) {

			var blocks = new List<IMySlimBlock>();
			grid.GetAllFatBlocks(blocks);

			foreach (var block in blocks) {

				if (block.FatBlock as IMyCargoContainer != null)
					continue;

				if (!block.FatBlock.HasInventory)
					continue;

				if (!StorageTools.HasStorageKey(block.FatBlock, StorageTools.MesContainerTypeKey))
					continue;

				var entity = block.FatBlock as MyEntity;
				var inventory = entity?.GetInventory();

				if (inventory == null)
					continue;

				var containerTypeId = block.FatBlock.Storage[StorageTools.MesContainerTypeKey];
				var containerType = MyDefinitionManager.Static.GetContainerTypeDefinition(containerTypeId);

				if (containerType == null)
					continue;

				inventory.GenerateContent(containerType);
			
			}

		}

	}
}
