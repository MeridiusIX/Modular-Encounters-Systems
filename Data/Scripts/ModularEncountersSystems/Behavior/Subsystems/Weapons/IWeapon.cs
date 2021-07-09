using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;

namespace ModularEncountersSystems.Behavior.Subsystems.Weapons {
	public interface IWeapon {

		double AmmoAcceleration();
		double AmmoInitialVelocity();
		double AmmoVelocity();
		IMyFunctionalBlock Block();
		IMyEntity CurrentTarget();
		void DetermineWeaponReadiness();
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
		void SetTarget(IMyEntity entity);
		void ToggleFire();
		

	}
}
