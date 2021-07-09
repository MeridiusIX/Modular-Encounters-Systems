using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Tasks;
using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Watchers {
	public static class EventWatcher {

		public static Action<GridEntity, Vector3D> JumpRequested;
		public static Action<GridEntity, Vector3D, Vector3D> JumpCompleted;

		public static void Setup() {

			MyVisualScriptLogicProvider.GridJumped += GridJumped;
			MES_SessionCore.UnloadActions += Unload;
		
		}

		public static void GridJumped(long identityId, string entityName, long entityId) {

			GridEntity grid = null;

			for (int i = GridManager.Grids.Count - 1; i >= 0; i--) {

				var thisGrid = GridManager.Grids[i];

				if (!thisGrid.ActiveEntity() || thisGrid.GetEntityId() != entityId)
					continue;

				grid = thisGrid;
				break;

			}

			JumpRequested?.Invoke(grid, grid.GetPosition());
			TaskProcessor.Tasks.Add(new GridJumpTracker(grid));

		}

		public static void Unload() {

			MyVisualScriptLogicProvider.GridJumped -= GridJumped;

		}

	}

}
