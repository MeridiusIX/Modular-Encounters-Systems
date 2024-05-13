using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Entities {
	public static class BlockManager {

		public static Dictionary<MyDefinitionId, MyCubeBlockDefinition> BlockDefinitions = new Dictionary<MyDefinitionId, MyCubeBlockDefinition>();
		public static List<string> BlockSubtypeIds = new List<string>();

		public static List<MyDefinitionId> NanobotBlockIds = new List<MyDefinitionId>();
		public static List<MyDefinitionId> RivalAiBlockIds = new List<MyDefinitionId>();
		public static List<MyDefinitionId> ShieldBlockIds = new List<MyDefinitionId>();

		public static List<MyDefinitionId> AllWeaponCoreBlocks = new List<MyDefinitionId>();
		public static List<MyDefinitionId> AllWeaponCoreGuns = new List<MyDefinitionId>();
		public static List<MyDefinitionId> AllWeaponCoreTurrets = new List<MyDefinitionId>();

		public static Action<BlockEntity> BlockAdded;

		public static void Setup() {

			//Get All Block Definitions

			foreach (var block in DefinitionHelper.AllBlockDefinitions) {

				if (block == null)
					continue;

				if (!BlockSubtypeIds.Contains(block.Id.SubtypeName))
					BlockSubtypeIds.Add(block.Id.SubtypeName);

				if (!BlockSubtypeIds.Contains(block.Id.ToString()))
					BlockSubtypeIds.Add(block.Id.ToString());

				if (block != null && !BlockDefinitions.ContainsKey(block.Id))
					BlockDefinitions.Add(block.Id, block);

			}

			//Register One-Off Blocks
			ShieldBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Refinery), "LWTSX_DamageAbsorber"));
			ShieldBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Refinery), "LargeShipSmallShieldGeneratorBase"));
			ShieldBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Refinery), "LargeShipLargeShieldGeneratorBase"));
			ShieldBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Refinery), "SmallShipSmallShieldGeneratorBase"));
			ShieldBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_Refinery), "SmallShipMicroShieldGeneratorBase"));
			ShieldBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "EmitterST"));
			ShieldBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "EmitterL"));
			ShieldBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "EmitterS"));
			ShieldBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "EmitterLA"));
			ShieldBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "EmitterSA"));
			ShieldBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "NPCEmitterLB"));
			ShieldBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_UpgradeModule), "NPCEmitterSB"));

			RivalAiBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "RivalAIRemoteControlSmall"));
			RivalAiBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "RivalAIRemoteControlLarge"));
			RivalAiBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_Imperial_Dropship_Guild_RC"));
			RivalAiBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_TIE_Fighter_RC"));
			RivalAiBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_Imperial_SpeederBike_FakePilot"));
			RivalAiBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_Imperial_ProbeDroid_Top_II"));
			RivalAiBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_Imperial_DroidCarrier_DroidBrain"));
			RivalAiBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_Imperial_DroidCarrier_DroidBrain_Aggressor"));
			RivalAiBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_NewRepublic_EWing_RC"));
			RivalAiBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_Imperial_RC_Largegrid"));
			RivalAiBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "K_TIE_Drone_Core"));
			RivalAiBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_RemoteControl), "GFA_SG_TIEFighter_Viewport_RivalAI"));

			NanobotBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_ShipWelder), "SELtdSmallNanobotBuildAndRepairSystem"));
			NanobotBlockIds.Add(new MyDefinitionId(typeof(MyObjectBuilder_ShipWelder), "SELtdLargeNanobotBuildAndRepairSystem"));

			//Register


			MES_SessionCore.UnloadActions += Unload;

		}

		public static void GetBlocksOfType<T>(List<BlockEntity> results) where T : class {

			for (int i = GridManager.Grids.Count - 1; i >= 0; i--) {

				var grid = GridManager.Grids[i];

				if (!grid.ActiveEntity())
					continue;

				for (int j = grid.AllTerminalBlocks.Count - 1; j >= 0; j--) {

					var block = grid.AllTerminalBlocks[j];

					if (!block.ActiveEntity())
						continue;

					var validBlock = block.Block as T;

					if (validBlock != null && !results.Contains(block))
						results.Add(block);

				}
			
			}
		
		}

		public static void GetBlocksOfTypes(List<BlockEntity> results, List<MyDefinitionId> types){

			for (int i = GridManager.Grids.Count - 1; i >= 0; i--) {

				var grid = GridManager.Grids[i];

				if (!grid.ActiveEntity())
					continue;

				for (int j = grid.AllTerminalBlocks.Count - 1; j >= 0; j--) {

					var block = grid.AllTerminalBlocks[j];

					if (!block.ActiveEntity())
						continue;

					if (types.Contains(block.Block.SlimBlock.BlockDefinition.Id) && !results.Contains(block))
						results.Add(block);

				}

			}

		}

		public static void Unload() {

			BlockDefinitions.Clear();

			ShieldBlockIds.Clear();
			NanobotBlockIds.Clear();
			RivalAiBlockIds.Clear();

			AllWeaponCoreBlocks.Clear();
			AllWeaponCoreGuns.Clear();
			AllWeaponCoreTurrets.Clear();

		}

	}
}
