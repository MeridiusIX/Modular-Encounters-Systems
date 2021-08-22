using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Watchers;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Tasks {
	public class GridJumpTracker : TaskItem, ITaskItem {

		private int _timer;
		private GridEntity _grid;
		private Vector3D _previousCoords;

		public GridJumpTracker(GridEntity grid) {

			_tickTrigger = 10;
			_grid = grid;
			_previousCoords = grid.GetPosition();
		
		}

		public override void Run() {

			_timer++;

			if (!_grid.ActiveEntity() || _timer / 6 > 15) {

				_isValid = false;
				return;
			
			}

			var currentPos = _grid.GetPosition();
			var dist = Vector3D.Distance(_previousCoords, currentPos);
			
			//TODO: Account For Grid Speed And Etc
			if (dist < 2000) {

				_previousCoords = currentPos;
				return;

			}

			EventWatcher.JumpCompleted?.Invoke(_grid, _previousCoords, currentPos);
			_isValid = false;

		}

	}

}
