using ModularEncountersSystems.BlockLogic;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace ModularEncountersSystems.BlockLogic {

	public class DisposableBeaconLogic : BaseBlockLogic, IBlockLogic{
		
		IMyBeacon Beacon;
		bool IsWorking = false;

		float TicksSincePlayerNearby = 0;
		float TicksSinceWorking = 0;

		public DisposableBeaconLogic(BlockEntity block) {

			Setup(block);

		}

		internal override void Setup(BlockEntity block) {

			base.Setup(block);

			if (!_isServer) {

				_isValid = false;
				return;

			}

			Beacon = block.Block as IMyBeacon;
			Beacon.IsWorkingChanged += OnWorkingChange;
			_useTick100 = true;

			

		}

		internal override void RunTick100() {

			if (!Active)
				return;

			if (Settings.CustomBlocks.UseDisposableBeaconInactivity == true) {

				TicksSinceWorking += 100;

				if ((TicksSinceWorking / 60) / 60 >= Settings.CustomBlocks.DisposableBeaconRemovalTimerMinutes) {

					Beacon.CubeGrid.RazeBlock(Beacon.SlimBlock.Min);
					_isValid = false;
					return;

				}

			}

			if (Settings.CustomBlocks.UseDisposableBeaconPlayerDistance == true) {

				double closestDistance = -1;

				foreach (var player in PlayerManager.Players) {

					if (!player.ActiveEntity()) {

						continue;

					}

					var thisDist = player.Distance(Beacon.GetPosition());

					if (thisDist < closestDistance || closestDistance == -1) {

						closestDistance = thisDist;

					}

				}

				if (closestDistance >= Settings.CustomBlocks.DisposableBeaconPlayerDistanceTrigger) {

					TicksSincePlayerNearby += 100;

					if ((TicksSincePlayerNearby / 60) / 60 >= Settings.CustomBlocks.DisposableBeaconRemovalTimerMinutes) {

						Beacon.CubeGrid.RazeBlock(Beacon.SlimBlock.Min);
						_isValid = false;
						return;

					}

				} else {

					TicksSincePlayerNearby = 0;

				}

			}


		}

		void OnWorkingChange(IMyCubeBlock block) {

			if(block.IsWorking == false || block.IsFunctional == false) {

				IsWorking = false;
				return;

			}

			IsWorking = true;
			
		}

		internal override void Unload(IMyEntity entity = null) {

			base.Unload(entity);

			if (Beacon != null) {

				Beacon.IsWorkingChanged -= WorkingChanged;

			}

		}

	}
	
}