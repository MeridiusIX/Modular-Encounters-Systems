using ModularEncountersSystems.API;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.World;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Manipulation {

	//This class is dedicated to adding modified blocks from the Defense Shields mod
	//to NPC Grids when spawned through MES.

	public static class NPCShieldManager {

		public static bool DefenseShieldModLoaded = false;
		public static bool NPCShieldProviderModLoaded = false;

		public static bool InitBlockList = false;

		public static List<IMyCubeGrid> NewlyAddedGridsForShieldProcessing = new List<IMyCubeGrid>();

		private static MyDefinitionId _smallGridController = new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "NPCControlSB");
		private static MyDefinitionId _largeGridController = new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "NPCControlLB");

		private static MyDefinitionId _smallGridEmitter = new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "NPCEmitterSB");
		private static MyDefinitionId _largeGridEmitter = new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "NPCEmitterLB");

		private static List<MyDefinitionId> _armorTypes = new List<MyDefinitionId>();
		private static List<MyDefinitionId> _emitterTypes = new List<MyDefinitionId>();

		private static Dictionary<IMyCubeGrid, IMyUpgradeModule> _activeNpcShields = new Dictionary<IMyCubeGrid, IMyUpgradeModule>();

		public static bool IsGlobalShieldProviderEnabled() {

			return AddonManager.NpcShieldProvider || Settings.Grids.EnableGlobalNPCShieldProvider;

		}

		public static bool AddDefenseShieldsToGrid(MyObjectBuilder_CubeGrid cubeGrid, bool spawnGroupAdd) {

			if (!InitBlockList) {

				InitBlockList = true;
				InitializeArmorBlockList();
			
			}

			SpawnLogger.Write("DSP: Check for Mod", SpawnerDebugEnum.Manipulation);
			if (!AddonManager.DefenseShields)
				return false;

			SpawnLogger.Write("DSP: Check if Spawn Eligible", SpawnerDebugEnum.Manipulation);
			if (!IsGlobalShieldProviderEnabled() || !spawnGroupAdd)
				return false;

			MyObjectBuilder_CubeBlock minArmor = null;
			MyObjectBuilder_CubeBlock maxArmor = null;

			SpawnLogger.Write("DSP: Get Min and Max armors", SpawnerDebugEnum.Manipulation);
			foreach (var block in cubeGrid.CubeBlocks) {

				if (!_armorTypes.Contains(block.GetId()))
					continue;

				if (minArmor == null) {

					minArmor = block;
					continue;

				}

				if (maxArmor == null) {

					maxArmor = block;
					continue;

				}

				if (Vector3I.Min(block.Min, minArmor.Min) == (Vector3I)block.Min) {

					minArmor = block;
					continue;

				}

				if (Vector3I.Max(block.Min, minArmor.Min) == (Vector3I)block.Min) {

					maxArmor = block;

				}

			}

			SpawnLogger.Write("DSP: Check if Min and Max armors are null", SpawnerDebugEnum.Manipulation);
			if (minArmor == null || maxArmor == null) {

				SpawnLogger.Write(string.Format("Min Null {0}", minArmor == null), SpawnerDebugEnum.Manipulation);
				SpawnLogger.Write(string.Format("Max Null {0}", maxArmor == null), SpawnerDebugEnum.Manipulation);
				return false;

			}
				

			SpawnLogger.Write("DSP: Build Shield Blocks and replace armor with them.", SpawnerDebugEnum.Manipulation);
			SerializableDefinitionId emitterId = new SerializableDefinitionId();
			SerializableDefinitionId controllerId = new SerializableDefinitionId();

			if (cubeGrid.GridSizeEnum == MyCubeSize.Large) {

				emitterId = _largeGridEmitter;
				controllerId = _largeGridController;

			} else {

				emitterId = _smallGridEmitter;
				controllerId = _smallGridController;

			}

			var emitter = MyObjectBuilderSerializer.CreateNewObject(emitterId) as MyObjectBuilder_CubeBlock;
			var controller = MyObjectBuilderSerializer.CreateNewObject(controllerId) as MyObjectBuilder_CubeBlock;

			emitter.BlockOrientation = minArmor.BlockOrientation;
			emitter.Min = minArmor.Min;
			emitter.ColorMaskHSV = minArmor.ColorMaskHSV;
			emitter.Owner = minArmor.Owner;

			controller.BlockOrientation = maxArmor.BlockOrientation;
			controller.Min = maxArmor.Min;
			controller.ColorMaskHSV = maxArmor.ColorMaskHSV;
			controller.Owner = maxArmor.Owner;

			cubeGrid.CubeBlocks.Remove(minArmor);
			cubeGrid.CubeBlocks.Remove(maxArmor);

			cubeGrid.CubeBlocks.Add(emitter);
			cubeGrid.CubeBlocks.Add(controller);

			return true;

		}

		public static void ActivateShieldsForNPC(IMyCubeGrid cubeGrid, bool usingSpawnerShields) {

			if (!AddonManager.DefenseShields || (!IsGlobalShieldProviderEnabled() && !usingSpawnerShields))
				return;

			if (cubeGrid.BigOwners.Count == 0) {

				return;

			} else {

				foreach (var owner in cubeGrid.BigOwners) {

					if (!IsNPC(owner))
						return;

				}

			}

			//Get Blocks, filter shields
			var blocks = BlockCollectionHelper.GetBlocksOfType<IMyUpgradeModule>(cubeGrid);

			foreach (var block in blocks) {

				var blockId = block.SlimBlock.BlockDefinition.Id;

				if (_emitterTypes.Contains(blockId)) {

					if (!IsNPC(block.OwnerId))
						return;

					SpawnLogger.Write("LoS Requirement For NPC Shields Removed", SpawnerDebugEnum.API);
					APIs.Shields.SetSkipLos(block);
					APIs.Shields.PointAttackShield(block, block.GetPosition(), block.EntityId, -1000000, true, false);
					block.OwnershipChanged += ShieldBlockOwnershipChange;
					block.SlimBlock.CubeGrid.OnBlockOwnershipChanged += ShieldBlockGridOwnershipChanged;
					block.SlimBlock.CubeGrid.OnGridSplit += ShieldGridSplit;

				}

			}

		}

		public static void InitializeArmorBlockList() {

			_armorTypes.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorBlock"));
			_armorTypes.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallBlockArmorBlock"));
			_armorTypes.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHeavyBlockArmorBlock"));
			_armorTypes.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorBlock"));
			_emitterTypes.Add(new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "NPCControlSB"));
			_emitterTypes.Add(new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "NPCControlLB"));
			_emitterTypes.Add(new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "EmitterST"));
			_emitterTypes.Add(new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "EmitterL"));
			_emitterTypes.Add(new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "EmitterS"));
			_emitterTypes.Add(new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "EmitterLA"));
			_emitterTypes.Add(new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "EmitterSA"));


		}

		private static void ShieldBlockOwnershipChange(IMyTerminalBlock block) {

			if (!IsNPC(block.OwnerId)) {

				block.OwnershipChanged -= ShieldBlockOwnershipChange;
				block.SlimBlock.CubeGrid.OnBlockOwnershipChanged -= ShieldBlockGridOwnershipChanged;
				block.SlimBlock.CubeGrid.OnGridSplit -= ShieldGridSplit;
				_activeNpcShields.Remove(block.SlimBlock.CubeGrid);
				//TODO: Set LoS Requirement to True

			}

		}

		private static void ShieldBlockGridOwnershipChanged(IMyCubeGrid cubeGrid) {

			IMyUpgradeModule shield = null;

			if (!_activeNpcShields.TryGetValue(cubeGrid, out shield))
				return;

			if (shield?.SlimBlock?.CubeGrid != null || !MyAPIGateway.Entities.Exist(shield?.SlimBlock?.CubeGrid) || !cubeGrid.IsSameConstructAs(shield.SlimBlock.CubeGrid)) {

				_activeNpcShields.Remove(cubeGrid);
				return;

			}

			bool npcOwned = true;

			if (cubeGrid.BigOwners.Count == 0) {

				npcOwned = false;

			} else {

				foreach (var owner in cubeGrid.BigOwners) {

					if (!IsNPC(owner))
						npcOwned = false;

				}

			}

			if (npcOwned)
				return;

			//TODO: Set LoS Requirement to True

		}

		private static void ShieldGridSplit(IMyCubeGrid gridA, IMyCubeGrid gridB) {

			IMyUpgradeModule shield = null;

			gridA.OnGridSplit -= ShieldGridSplit;
			gridB.OnGridSplit -= ShieldGridSplit;

			if (!_activeNpcShields.TryGetValue(gridA, out shield) && !_activeNpcShields.TryGetValue(gridA, out shield)) {

				return;

			}

			if (shield?.SlimBlock?.CubeGrid == null || !MyAPIGateway.Entities.Exist(shield?.SlimBlock?.CubeGrid)) {

				return;

			}

			if (!_activeNpcShields.ContainsKey(shield.SlimBlock.CubeGrid)) {

				if (shield.SlimBlock.CubeGrid == gridA) {

					gridB.OnBlockOwnershipChanged -= ShieldBlockGridOwnershipChanged;
					_activeNpcShields.Remove(gridB);


				} else {

					gridA.OnBlockOwnershipChanged -= ShieldBlockGridOwnershipChanged;
					_activeNpcShields.Remove(gridA);

				}

				shield.SlimBlock.CubeGrid.OnBlockOwnershipChanged += ShieldBlockGridOwnershipChanged;
				_activeNpcShields.Add(shield.SlimBlock.CubeGrid, shield);

			}

			shield.SlimBlock.CubeGrid.OnGridSplit += ShieldGridSplit;
			ShieldBlockGridOwnershipChanged(shield.SlimBlock.CubeGrid);

		}

		public static bool IsNPC(long identity) {

			if (MyAPIGateway.Players.TryGetSteamId(identity) > 0 || identity == 0) {

				return false;

			}

			return true;

		}

	}
}
