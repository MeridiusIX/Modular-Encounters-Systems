using ModularEncountersSystems.Helpers;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRage.Game;
using VRageMath;
using Sandbox.Game.Gui;
using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Logging;
using VRage.Game.ModAPI;
using SpaceEngineers.Game.ModAPI;

namespace ModularEncountersSystems.Behavior.Subsystems.Weapons {
	public class ControllerWeapon : BaseWeapon, IWeapon{

		internal IMyTurretControlBlock _controller;

		public ControllerWeapon(IMyTerminalBlock block, IMyRemoteControl remoteControl, IBehavior behavior) : base(block, remoteControl, behavior) {

			if (!_isValid)
				return;

			_controller = block as IMyTurretControlBlock;

		}

		public bool HasAmmo() {

			bool result = false;

			for (int i = _subWeapons.Count - 1; i >= 0; i--) {

				if (_subWeapons[i].IsValid() && _subWeapons[i].IsActive() && _subWeapons[i].HasAmmo()) {

					result = true;
					break;
				
				}

			}

			return result;

		}

		//-------------------------------------------------------
		//------------START INTERFACE METHODS--------------------
		//-------------------------------------------------------

		public bool CanLockOnGrid(IMyCubeGrid target) {

			if (target == null || _controller == null)
				return false;

			if (target.Closed || _block.Closed)
				return false;

			//Ammo Firing Range Check
			if (Vector3D.Distance(target.GetPosition(), _block.GetPosition()) > MaxAmmoTrajectory())
				return false;

			//LOS Check
			var elevationGrid = _controller.ElevationRotor?.TopGrid;

			if (elevationGrid == null)
				return false;

			if (!TurretHasLOS(target.GetPosition(), elevationGrid.WorldAABB.Center, _behavior.CurrentGrid?.LinkedGrids, true))
				return false;

			return true;

		}

		public IMyEntity CurrentTarget() {

			if (!IsValid() || !IsActive())
				return null;

			return _controller.Target;

		}

		public void DetermineWeaponReadiness(bool usingTurretController = false) {

			if (!IsValid() || !IsActive())
				return;

			var controller = Block() as IMyTurretControlBlock;
			var result = false;

			for (int i = _subWeapons.Count - 1; i >= 0; i--) {

				_subWeapons[i].DetermineWeaponReadiness(true);

				if (_subWeapons[i].ReadyToFire) {

					if (controller.IsAimed) {

						result = true;

						if (_subWeapons[i].PendingAmmoRefill)
							_pendingAmmoRefill = true;

					} else {

						_subWeapons[i].ReadyToFire = false;

					}
					
				}

			}

			_readyToFire = result;

		}

		public void FireOnce() {

			if (!IsValid() || !IsActive())
				return;

			for (int i = _subWeapons.Count - 1; i >= 0; i--) {

				if (_subWeapons[i].IsValid() && _subWeapons[i].IsActive()) {

					_subWeapons[i].FireOnce();

				}

			}

		}

		public override bool IsActive() {

			if (!base.IsActive())
				return false;

			if (_controller == null)
				return false;

			if (_controller.AzimuthRotor == null || !_controller.AzimuthRotor.IsFunctional || !_controller.AzimuthRotor.IsWorking)
				return false;

			if (_controller.ElevationRotor == null || !_controller.ElevationRotor.IsFunctional || !_controller.ElevationRotor.IsWorking)
				return false;

			var result = false;

			for (int i = _subWeapons.Count - 1; i >= 0; i--) {

				if (_subWeapons[i].IsValid() && _subWeapons[i].IsActive() && _subWeapons[i].HasAmmo()) {

					result = true;
					break;

				}

			}

			return result;

		}

		public override bool IsBarrageWeapon() {

			return false;

		}

		public override float MaxAmmoTrajectory() {

			if (SubWeapons == null)
				return 0;

			float maxRange = 0;

			for (int i = SubWeapons.Count - 1; i >= 0; i--) {

				var sub = SubWeapons[i];

				if (sub == null || !sub.IsValid())
					continue;

				var thisRange = sub.MaxAmmoTrajectory();

				if (thisRange > maxRange)
					maxRange = thisRange;
			
			}

			_ammoMaxTrajectory = _ammoMaxTrajectory;
			return _ammoMaxTrajectory;

		}

		public override void ReplenishAmmo() {

			if (!_pendingAmmoRefill)
				return;

			_pendingAmmoRefill = false;

			for (int i = _subWeapons.Count - 1; i >= 0; i--) {

				if (_subWeapons[i].IsValid() && _subWeapons[i].IsActive()) {

					_subWeapons[i].ReplenishAmmo();

				}

			}

		}

		public void SetLockOnTarget() {

			PendingLockOn = false;

			if (!IsValid())
				return;

			_controller.TrackTarget(LockOnTarget);

		}

		public void SetTarget(IMyEntity entity) {

			if (!IsValid() || !IsActive())
				return;

			_controller.TrackTarget(entity);

		}

		public void ToggleFire() {

			return;

			if (!IsValid() || !IsActive())
				return;

			for (int i = _subWeapons.Count - 1; i >= 0; i--) {

				if (_subWeapons[i].IsValid() && _subWeapons[i].IsActive() && _subWeapons[i].IsWeaponCore) {

					_subWeapons[i].ToggleFire();

				}

			}

		}

	}

}
