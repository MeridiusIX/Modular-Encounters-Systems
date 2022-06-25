using ModularEncountersSystems.Behavior.Subsystems.Weapons;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Files;
using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace ModularEncountersSystems.BlockLogic {
	public class BeamWeapon : BaseBlockLogic, IBlockLogic, IWeapon {

		public IMyFunctionalBlock Weapon;
		public Beam BeamData;

		internal bool _server;
		internal bool _dedicated;

		internal Direction _direction;

		internal bool _firing;
		internal bool _continuousFire;

		//Prefire
		internal bool _prefireComplete;
		internal short _prefireTicks;

		//Fire
		internal bool _firingComplete;
		internal short _firingTicks;

		//PostFire
		internal bool _postfireComplete;
		internal short _postfireTicks;

		//Interface
		internal List<IWeapon> _subWeapons;
		internal bool _readyToFire;

		public BeamWeapon(BlockEntity weapon, Beam beamData) {

			Setup(weapon);

			Weapon = weapon.Block as IMyFunctionalBlock;
			BeamData = beamData;
			_subWeapons = new List<IWeapon>();

		}

		internal override void RunTick1() {
		
			
		
		}

		//////////////////////////////////////////////////
		//////////////// INTERFACE LOGIC /////////////////
		//////////////////////////////////////////////////

		public IMyFunctionalBlock Block() { return Weapon; }

		public bool ReadyToFire { get { return false; } set { } }

		public void DetermineWeaponReadiness(bool usingTurretController = false) {
		
			
		
		}

		public void FireOnce() {
		
			
		
		}

		public Direction GetDirection() {

			return _direction;

		}

		public bool IsActive() {

			return _isWorking;
		
		}

		public bool IsBarrageWeapon() {

			return BeamData.Behavior.BarrageCapable;
		
		}

		public bool IsValid() {

			return _isValid;
		
		}

		public bool IsReadyToFire() {

			return _isValid && _isWorking;
		
		}

		public float MaxAmmoTrajectory() {

			return (float)BeamData.Behavior.MaxRange;

		}

		public void SetDirection(Direction direction) {
			
			_direction = direction;
		
		}

		public void ToggleFire() {
		
			
		
		}

		//////////////////////////////////////////////////
		///////////// INTERFACE DUMMY LOGIC //////////////
		//////////////////////////////////////////////////

		public bool IsWeaponCore { get { return false; } }

		public bool PendingAmmoRefill { get { return false; } }

		public List<IWeapon> SubWeapons { get { return _subWeapons; } }

		public bool UsesTurretController { get { return false; } set {  } }

		public bool PendingLockOn { get { return false; } set {  } }

		public bool HasLockOn { get { return false; } set { } }

		public IMyCubeGrid LockOnTarget { get { return null; } set {  } }

		public bool IsHoming { get { return false; } set { } }

		public double AmmoAcceleration() { return 99999; }

		public double AmmoInitialVelocity() { return 99999; }

		public double AmmoVelocity() { return 99999; }

		public bool CanLockOnGrid(IMyCubeGrid cubeGrid) { return false; }

		public IMyEntity CurrentTarget() { return null; }		
		public bool HasAmmo() { return true; }
		public bool IsStaticGun() { return true; }
		public bool IsTurret() { return false; }
		public void ReplenishAmmo() { }
		
		public void SetLockOnTarget() { }
		public void SetTarget(IMyEntity entity) { }
		

	}

}
