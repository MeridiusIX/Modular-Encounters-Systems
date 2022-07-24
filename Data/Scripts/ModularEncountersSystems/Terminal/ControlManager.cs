using ModularEncountersSystems.Core;
using ModularEncountersSystems.Spawning.Manipulation;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Terminal {
	public static class ControlManager {

		internal static MyDefinitionId _consoleTableId = new MyDefinitionId(typeof(MyObjectBuilder_Projector), "LargeBlockConsole");

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

			if (block.SlimBlock.BlockDefinition.Id == _consoleTableId) {
				
				if (block.Storage != null && block.Storage.ContainsKey(StorageTools.MesShipyardKey)) {

					ShipyardControls.DisplayControls(block, controls);
					return;

				}

			}
				
		}

		public static void Unload() {

			MyAPIGateway.TerminalControls.CustomControlGetter -= ModifyControls;

		}

	}
}
