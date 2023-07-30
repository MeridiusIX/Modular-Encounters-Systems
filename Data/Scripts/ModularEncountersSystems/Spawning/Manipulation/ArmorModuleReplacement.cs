using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.World;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.ObjectBuilders;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Manipulation {

	public struct ArmorForReplacement {

		public List<MyObjectBuilder_CubeBlock> Blocks;
		public MyObjectBuilder_CubeBlock Block;

		public ArmorForReplacement(List<MyObjectBuilder_CubeBlock> blocks, MyObjectBuilder_CubeBlock block) {

			Blocks = blocks;
			Block = block;

		}

	}

	public static class ArmorModuleReplacement {

		public static List<MyDefinitionId> SmallArmor = new List<MyDefinitionId>();
		public static List<MyDefinitionId> LargeArmor = new List<MyDefinitionId>();

		public static List<MyDefinitionId> SmallModules = new List<MyDefinitionId>();
		public static List<MyDefinitionId> LargeModules = new List<MyDefinitionId>();

		public static List<string> ModuleSubtypes = new List<string>();

		private static Random _rnd = new Random();
		private static bool _setupComplete = false;

		public static void Setup() {

			_setupComplete = true;
			SmallArmor.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallBlockArmorBlock"));
			SmallArmor.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "SmallHeavyBlockArmorBlock"));
			LargeArmor.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeBlockArmorBlock"));
			LargeArmor.Add(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "LargeHeavyBlockArmorBlock"));

			SmallModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Nanobots-Small"));
			SmallModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-JumpDrive-Small"));
			SmallModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Jetpack-Small"));
			SmallModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Drill-Small"));
			SmallModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Player-Small"));
			SmallModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Energy-Small"));
			SmallModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_DefensiveCombatBlock), "SmallDefensiveCombat"));
			SmallModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_FlightMovementBlock), "SmallFlightMovement"));

			LargeModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Nanobots-Large"));
			LargeModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-JumpDrive-Large"));
			LargeModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Jetpack-Large"));
			LargeModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Drill-Large"));
			LargeModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Player-Large"));
			LargeModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), "MES-Suppressor-Energy-Large"));
			LargeModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_DefensiveCombatBlock), "LargeDefensiveCombat"));
			LargeModules.Add(new MyDefinitionId(typeof(MyObjectBuilder_FlightMovementBlock), "LargeFlightMovement"));

			foreach (var id in SmallModules)
				ModuleSubtypes.Add(id.SubtypeName);

			foreach (var id in LargeModules)
				ModuleSubtypes.Add(id.SubtypeName);

			var defs = MyDefinitionManager.Static.GetAllDefinitions();

			foreach (var subtype in ModuleSubtypes) {

				var id = new MyDefinitionId(typeof(MyObjectBuilder_RadioAntenna), subtype);
				var def = MyDefinitionManager.Static.GetCubeBlockDefinition(id);
				var block = def as MyRadioAntennaDefinition;

				if (def == null)
					continue;

				block.Enabled = true;
				block.MaxBroadcastRadius = 50000;
				block.Size = new Vector3I(1, 1, 1);

			}

		}

		public static void ProcessGridForModules(MyObjectBuilder_CubeGrid[] grids, ImprovedSpawnGroup spawnGroup, ManipulationProfile profile, NpcData data) {

			if (!_setupComplete)
				Setup();

			bool setArmor = false;
			List<MyDefinitionId> allowedArmor = null;
			List<MyDefinitionId> allowedModules = null;
			List<MyDefinitionId> usedModules = new List<MyDefinitionId>();
			var availableArmor = new List<ArmorForReplacement>();


			foreach (var grid in grids) {

				if (grid?.CubeBlocks == null)
					return;

				if (!setArmor) {

					setArmor = true;
					allowedArmor = grid.GridSizeEnum == MyCubeSize.Large ? LargeArmor : SmallArmor;
					allowedModules = grid.GridSizeEnum == MyCubeSize.Large ? LargeModules : SmallModules;

				}

				foreach (var block in grid.CubeBlocks) {

					if (allowedArmor.Contains(block.GetId()))
						availableArmor.Add(new ArmorForReplacement(grid.CubeBlocks, block));

				}

			}

			for (int i = 0; i < profile.ModulesForArmorReplacement.Count; i++) {

				if (availableArmor.Count == 0) {

					break;

				}

				var armorIndex = availableArmor.Count == 1 ? 0 : _rnd.Next(0, availableArmor.Count);

				if (usedModules.Contains(profile.ModulesForArmorReplacement[i]) || !allowedModules.Contains(profile.ModulesForArmorReplacement[i]) || !ReplaceArmorWithModule(availableArmor[armorIndex].Blocks, availableArmor[armorIndex].Block, profile.ModulesForArmorReplacement[i], data)) {

					continue;

				}

				availableArmor.RemoveAt(armorIndex);
				usedModules.Add(profile.ModulesForArmorReplacement[i]);

			}

		}

		public static bool ReplaceArmorWithModule(List<MyObjectBuilder_CubeBlock> blocks, MyObjectBuilder_CubeBlock oldBlock, SerializableDefinitionId newBlockId, NpcData data) {

			var newBlock = MyObjectBuilderSerializer.CreateNewObject(newBlockId) as MyObjectBuilder_CubeBlock;

			if (newBlock == null) {

				SpawnLogger.Write("Could Not Add Module To Prefab, It Does Not Exist: " + newBlockId.ToString(), SpawnerDebugEnum.Manipulation);
				return false;

			}

			newBlock.BlockOrientation = oldBlock.BlockOrientation;
			newBlock.Min = oldBlock.Min;
			newBlock.ColorMaskHSV = oldBlock.ColorMaskHSV;
			newBlock.Owner = oldBlock.Owner;

			blocks.Remove(oldBlock);
			blocks.Add(newBlock);

			SetDefaultInhibitorRanges(newBlock, data);

			return true;

		}

		public static void SetDefaultInhibitorRanges(MyObjectBuilder_CubeBlock block, NpcData data) {

			var antenna = block as MyObjectBuilder_RadioAntenna;

			if (antenna == null)
				return;

			if (antenna.SubtypeName.StartsWith("MES-Suppressor-Nanobots-")) {

				antenna.BroadcastRadius = 1000;
				data.Attributes.UseNanobotDisable = true;

			}

			if (antenna.SubtypeName.StartsWith("MES-Suppressor-JumpDrive-")) {

				antenna.BroadcastRadius = 6000;
				data.Attributes.UseJumpDisable = true;

			}

			if (antenna.SubtypeName.StartsWith("MES-Suppressor-Jetpack-")) {

				antenna.BroadcastRadius = 1000;
				data.Attributes.UseJetpackDisable = true;

			}

			if (antenna.SubtypeName.StartsWith("MES-Suppressor-Drill-")) {

				antenna.BroadcastRadius = 500;
				data.Attributes.UseDrillDisable = true;

			}

			if (antenna.SubtypeName.StartsWith("MES-Suppressor-Player-")) {

				antenna.BroadcastRadius = 1000;
				data.Attributes.UsePlayerDisable = true;

			}

			if (antenna.SubtypeName.StartsWith("MES-Suppressor-Energy-")) {

				antenna.BroadcastRadius = 1000;
				data.Attributes.UseEnergyDisable = true;

			}

		}

	}

}
