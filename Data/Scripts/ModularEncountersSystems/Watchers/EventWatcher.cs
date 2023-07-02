using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Tasks;
using Sandbox.Game;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Watchers {
	public static class EventWatcher {

		public static Action<GridEntity, Vector3D> JumpRequested;
		public static Action<GridEntity, Vector3D, Vector3D> JumpCompleted;
		public static Action<IMyButtonPanel, int, long> ButtonPressed;

		public static Dictionary<long, IMyButtonPanel> ButtonPanels = new Dictionary<long, IMyButtonPanel>();


		public static void Setup() {

			MyVisualScriptLogicProvider.GridJumped += GridJumped;
			MyVisualScriptLogicProvider.ButtonPressedTerminalName += ButtonPress;
			MES_SessionCore.UnloadActions += Unload;
		
		}

		public static void ButtonPress(string name, int index, long playerId, long blockId) {

			IMyButtonPanel panel = null;

			if (!ButtonPanels.TryGetValue(blockId, out panel)) {

				//MyVisualScriptLogicProvider.ShowNotificationToAll("Couldn't Find Button Block", 4000);
				return;

			}
				

			ButtonPressed?.Invoke(panel, index, playerId);
		
		}

		public static void GridJumped(long identityId, string entityName, long entityId) {

			GridEntity grid = null;

			for (int i = GridManager.Grids.Count - 1; i >= 0; i--) {

				var thisGrid = GridManager.Grids[i];

				if (thisGrid == null || !thisGrid.ActiveEntity() || thisGrid.GetEntityId() != entityId)
					continue;

				grid = thisGrid;
				break;

			}

			if (grid == null)
				return;

			//MyVisualScriptLogicProvider.ShowNotificationToAll("Jump: " + grid.CubeGrid.CustomName, 3000);
			JumpRequested?.Invoke(grid, grid.GetPosition());
			TaskProcessor.Tasks.Add(new GridJumpTracker(grid));

		}

		public static void Unload() {

			MyVisualScriptLogicProvider.GridJumped -= GridJumped;
			MyVisualScriptLogicProvider.ButtonPressedTerminalName -= ButtonPress;

		}

	}

}
