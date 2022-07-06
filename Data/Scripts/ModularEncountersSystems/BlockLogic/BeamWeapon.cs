using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Behavior.Subsystems.Weapons;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Files;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.BlockLogic {

	public enum BeamWeaponMode {
	
		None,
		Prefire,
		Fire,
		PostFire
	
	}

	public class BeamWeapon : BaseBlockLogic, IBlockLogic, IWeapon {

		public BlockEntity BlockEntity;
		public IMyFunctionalBlock Weapon;
		public Beam BeamData;

		internal bool _server;
		internal bool _dedicated;

		internal Direction _direction;

		//Request
		internal bool _requestFire;
		internal bool _continuousFire;
		internal bool _firing;

		//Mode
		internal BeamWeaponMode _mode;

		//Prefire
		internal short _prefireTicks;

		//Fire
		internal short _firingTicks;
		internal short _damageTicks;

		//Hit
		internal IMyEntity _hitEntity;
		internal CollisionType _hitType;
		internal double _hitDistance;
		internal List<MyLineSegmentOverlapResult<MyEntity>> _hitEntities;
		internal List<IMyVoxelBase> _hitVoxels;

		//PostFire
		internal short _postfireTicks;

		//Interface
		internal List<IWeapon> _subWeapons;
		internal bool _readyToFire;

		public BeamWeapon(BlockEntity weapon, Beam beamData) {

			Setup(weapon);

			BlockEntity = weapon;
			Weapon = weapon.Block as IMyFunctionalBlock;
			BeamData = beamData;
			_mode = BeamWeaponMode.None;
			_subWeapons = new List<IWeapon>();

			_hitEntities = new List<MyLineSegmentOverlapResult<MyEntity>>();
			_hitVoxels = new List<IMyVoxelBase>();

		}

		internal override void RunTick1() {

			if (BlockEntity == null || !BlockEntity.ActiveEntity()) {

				_isValid = false;
				return;
			
			}

			if (!_firing) {

				if (_requestFire || _continuousFire) {

					_firing = true;
					_requestFire = false;

					_prefireTicks = 0;
					_firingTicks = 0;
					_damageTicks = 5;
					_postfireTicks = 0;

					_mode = BeamWeaponMode.Prefire;

				} else {

					_useTick1 = false;
					return;

				}

			} else {

				_requestFire = false;

			}

			if (_mode == BeamWeaponMode.Prefire) {

				Prefire();
			
			}

			if (_mode == BeamWeaponMode.Fire) {

				Firing();

			}

			if (_mode == BeamWeaponMode.PostFire) {

				PostFire();

			}

		}

		public void Prefire() {

			//Setup Prefire
			if (_prefireTicks == 0) {

				if (_isClientPlayer && !string.IsNullOrWhiteSpace(BeamData.Effect.PrefireSound)) {

					MyVisualScriptLogicProvider.PlaySingleSoundAtPositionLocal(BeamData.Effect.PrefireSound, Weapon.GetPosition());

				}

			}

			_prefireTicks++;

			if (_prefireTicks >= BeamData.Behavior.PrefireTicks) {

				_mode = BeamWeaponMode.Fire;

			}

		}

		public void Firing() {

			//Setup Fire
			if (_firingTicks == 0) {

				if (_isClientPlayer && !string.IsNullOrWhiteSpace(BeamData.Effect.PrefireSound)) {

					MyVisualScriptLogicProvider.PlaySingleSoundAtPositionLocal(BeamData.Effect.FireSound, Weapon.GetPosition());

				}

			}

			_firingTicks++;
			_damageTicks++;

			if (_damageTicks >= 5) {

				_damageTicks = 0;
				CalculateHits();

			}

			if (_server)
				ProcessDamageAndEffects();

			if (_isClientPlayer)
				DrawBeams();

			if (_firingTicks >= BeamData.Behavior.FireTicks) {

				_mode = BeamWeaponMode.PostFire;
			
			}

		}

		public void PostFire() {

			//Setup PostFire
			if (_postfireTicks == 0) {

				if (_isClientPlayer && !string.IsNullOrWhiteSpace(BeamData.Effect.PostfireSound)) {

					MyVisualScriptLogicProvider.PlaySingleSoundAtPositionLocal(BeamData.Effect.PostfireSound, Weapon.GetPosition());

				}

			}

			_postfireTicks++;

			if (_postfireTicks >= BeamData.Behavior.PostfireTicks) {

				_firing = false;
				_mode = BeamWeaponMode.None;
			
			}

		}

		public void CalculateHits() {

			//Self Grid
			var startCoords = Weapon.WorldMatrix.Forward * BeamData.Behavior.PadDistanceFromOrigin + Weapon.WorldMatrix.Translation;

			try {

				_hitEntities.Clear();
				_hitVoxels.Clear();
				var result = CollisionHelper.CheapRaycast(startCoords, Weapon.WorldMatrix.Forward, BeamData.Behavior.MaxRange, Weapon.SlimBlock.CubeGrid, _hitEntities, _hitVoxels);
			
			} catch (Exception e) {

				SpawnLogger.Write("Exception Caught During Beam Weapon Hit Calculation", SpawnerDebugEnum.Error);
				SpawnLogger.Write(e.ToString(), SpawnerDebugEnum.Error);

			}
		
		}

		public void DrawBeams() {
		
			
		
		}

		public void ProcessDamageAndEffects() {
		
			
		
		}

		//////////////////////////////////////////////////
		//////////////// INTERFACE LOGIC /////////////////
		//////////////////////////////////////////////////

		public IMyFunctionalBlock Block() { return Weapon; }

		public bool ReadyToFire { get { return false; } set { } }

		public void DetermineWeaponReadiness(bool usingTurretController = false) {
		
			//TODO
		
		}

		public void FireOnce() {

			//TODO

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

			//TODO

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
