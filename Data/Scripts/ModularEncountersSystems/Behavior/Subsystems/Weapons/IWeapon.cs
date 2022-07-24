using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace ModularEncountersSystems.Behavior.Subsystems.Weapons {
	public interface IWeapon {

		WeaponStatusEnum Status { get; set; }
		bool IsWeaponCore { get; }
		bool PendingAmmoRefill { get; }
		bool ReadyToFire { get; set; }
		List<IWeapon> SubWeapons { get; }
		bool UsesTurretController { get; set; }
		bool PendingLockOn { get; set; }
		bool HasLockOn { get; set; }
		IMyCubeGrid LockOnTarget { get; set; }
		bool IsHoming { get; set; }
		double AmmoAcceleration();
		double AmmoInitialVelocity();
		double AmmoVelocity();
		IMyFunctionalBlock Block();
		bool CanLockOnGrid(IMyCubeGrid cubeGrid);
		IMyEntity CurrentTarget();
		void DetermineWeaponReadiness(bool usingTurretController = false);
		void FireOnce(); //Fires The Weapon Once
		Direction GetDirection();
		bool HasAmmo();
		bool IsActive(); //Checks if weapon is On / Working / Fucntional
		bool IsBarrageWeapon(); //Checks if Static Gun is able to be Barrage Fired
		bool IsStaticGun();
		bool IsTurret();
		bool IsValid(); //Checks if weapon was registered properly, is part of same cubegrid, etc
		bool IsReadyToFire();
		float MaxAmmoTrajectory();
		void ReplenishAmmo();
		void SetDirection(Direction direction);
		void SetLockOnTarget();
		void SetTarget(IMyEntity entity);
		void ToggleFire();
		

	}
}
