using ModularEncountersSystems.Core;
using ModularEncountersSystems.Spawning.Manipulation;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Terminal {
	public static class ControlManager {

		public static MyDefinitionId ShipyardBlockId = new MyDefinitionId(typeof(MyObjectBuilder_Projector), "MES-Blocks-ShipyardTerminal");
		public static MyDefinitionId SuitUpgradeBlockId = new MyDefinitionId(typeof(MyObjectBuilder_ButtonPanel), "MES-Blocks-SuitUpgradeStation");
		public static MyDefinitionId ResearchTerminalBlockId = new MyDefinitionId(typeof(MyObjectBuilder_ButtonPanel), "MES-Blocks-ResearchTerminal");
		public static MyDefinitionId ProceduralGridBlockId = new MyDefinitionId(typeof(MyObjectBuilder_Projector), "LargeBlockConsole");

		public static void Setup() {

			MyAPIGateway.TerminalControls.CustomControlGetter += ModifyControls;
			MES_SessionCore.UnloadActions += Unload;

		}

		public static void DebugGetControlListToClipboard(List<IMyTerminalControl> controls) {

			var sb = new StringBuilder();

			foreach (var control in controls) {

				sb.Append(control.Id).AppendLine();

			}

			VRage.Utils.MyClipboardHelper.SetClipboard(sb.ToString());

		}

		public static void ModifyControls(IMyTerminalBlock block, List<IMyTerminalControl> controls) {

			if (block.Storage == null)
				return;

			if (block.SlimBlock.BlockDefinition.Id == ShipyardBlockId) {
				
				if (block.Storage.ContainsKey(StorageTools.MesShipyardKey)) {

					ShipyardControls.DisplayControls(block, controls);
					return;

				}

			}

			if (block.SlimBlock.BlockDefinition.Id == SuitUpgradeBlockId) {

				if (block.Storage.ContainsKey(StorageTools.MesSuitModsKey)) {

					SuitModificationControls.DisplayControls(block, controls);
					return;

				}

			}

		}

		public static void RefreshMenu(IMyTerminalBlock block) {

			var cubeBlock = block as MyCubeBlock;

			if (cubeBlock?.IDModule != null) {

				var share = cubeBlock.IDModule.ShareMode;
				var owner = cubeBlock.IDModule.Owner;
				cubeBlock.ChangeOwner(0, share == MyOwnershipShareModeEnum.None ? MyOwnershipShareModeEnum.Faction : MyOwnershipShareModeEnum.None);
				cubeBlock.ChangeOwner(owner, share);

			}

		}

		public static void Unload() {

			MyAPIGateway.TerminalControls.CustomControlGetter -= ModifyControls;

		}

	}
}
