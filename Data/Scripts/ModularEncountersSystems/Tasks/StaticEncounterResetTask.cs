using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using System;

namespace ModularEncountersSystems.Tasks {

	public class StaticEncounterResetTask : TaskItem, ITaskItem {

		internal StaticEncounterResetSite _site;
		internal long _requesterId;
		internal int _ticksWaited;

		public StaticEncounterResetTask(StaticEncounterResetSite site, long requesterId) {

			_tickTrigger = 10;
			_isValid = true;
			_site = site;
			_requesterId = requesterId;
			_ticksWaited = 0;

		}

		public override void Run() {

			try {

				_ticksWaited += _tickTrigger;

				foreach (var cubeGrid in _site.Grids) {

					if (cubeGrid != null && !cubeGrid.Closed) {

						if (_ticksWaited >= StaticEncounterReset.CloseTimeoutTicks) {

							StaticEncounterReset.Report(_site.SpawnGroupName, "FAILED: the old grids were not removed in time; the encounter was not reset", _requesterId, "Red");
							Finish();

						}

						return;

					}

				}

				StaticEncounterReset.CompleteReset(_site, _requesterId);

			} catch (Exception e) {

				SpawnLogger.Write("Static Encounter Reset Error: " + e.ToString(), SpawnerDebugEnum.Error, true);
				StaticEncounterReset.Report(_site.SpawnGroupName, "FAILED: an error occurred, see the game log", _requesterId, "Red");

			}

			Finish();

		}

		private void Finish() {

			_isValid = false;
			StaticEncounterReset.InProgress.Remove(_site.SpawnGroupName);

		}

	}

}
