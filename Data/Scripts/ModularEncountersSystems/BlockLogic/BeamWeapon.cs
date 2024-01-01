using ModularEncountersSystems.API;
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
using VRage.Utils;
using VRageMath;

namespace ModularEncountersSystems.BlockLogic {

	public enum BeamWeaponMode {
	
		None,
		Prefire,
		Fire,
		PostFire
	
	}

	public class BeamWeapon : BaseBlockLogic, IBlockLogic, IWeapon {

		public WeaponStatusEnum Status { get { return _status; } set { _status = value; } }
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
		internal Vector3D _hitOverride;
		internal List<MyLineSegmentOverlapResult<MyEntity>> _hitEntities;
		internal List<IMyVoxelBase> _hitVoxels;
		internal List<Vector3I> _penetrateCells;

		//DamageCooldowns
		internal short _cooldownRegular;
		internal short _cooldownExplosive;
		internal short _cooldownTesla;
		internal short _cooldownShield;

		//PostFire
		internal short _postfireTicks;

		//Interface
		internal List<IWeapon> _subWeapons;
		internal bool _readyToFire;
		internal WeaponStatusEnum _status;


		public BeamWeapon(BlockEntity weapon, Beam beamData) {

			Setup(weapon);

			BlockEntity = weapon;
			Weapon = weapon.Block as IMyFunctionalBlock;
			BeamData = beamData;
			_mode = BeamWeaponMode.None;
			_subWeapons = new List<IWeapon>();

			_hitEntities = new List<MyLineSegmentOverlapResult<MyEntity>>();
			_hitVoxels = new List<IMyVoxelBase>();
			_hitOverride = Vector3D.Zero;

			_penetrateCells = new List<Vector3I>();

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

					_cooldownRegular = 0;
					_cooldownExplosive = 0;
					_cooldownTesla = 0;
					_cooldownShield = 0;

					_hitOverride = Vector3D.Zero;

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
			var startCoords = Vector3D.Zero;
			var startDir = Weapon.WorldMatrix.Forward;

			if (!BeamData.Behavior.TurretBlock)
				startCoords = Weapon.WorldMatrix.Forward * BeamData.Behavior.PadDistanceFromOrigin + Weapon.WorldMatrix.Translation;
			else {
				
				//TODO: Elaborate This, In Another Method
				startCoords = Weapon.WorldMatrix.Forward * BeamData.Behavior.PadDistanceFromOrigin + Weapon.WorldMatrix.Translation;
				startDir = Weapon.WorldMatrix.Forward;

			}
				

			try {

				_hitEntities.Clear();
				_hitVoxels.Clear();
				var result = CollisionHelper.CheapRaycast(startCoords, Weapon.WorldMatrix.Forward, BeamData.Behavior.MaxRange, Weapon.SlimBlock.CubeGrid, _hitEntities, _hitVoxels);

				if (_server && result.HitType != CollisionType.None)
					ProcessDamageAndEffects(result);

				if (result.HitType != CollisionType.None)
					_hitDistance = _hitOverride != Vector3D.Zero ? (Vector3D.Distance(Weapon.GetPosition(), _hitOverride) - BeamData.Behavior.PadDistanceFromOrigin) : (Vector3D.Distance(Weapon.GetPosition(), result.HitPosition) - BeamData.Behavior.PadDistanceFromOrigin);
				else
					_hitDistance = BeamData.Behavior.MaxRange;

			} catch (Exception e) {

				SpawnLogger.Write("Exception Caught During Beam Weapon Hit Calculation", SpawnerDebugEnum.Error);
				SpawnLogger.Write(e.ToString(), SpawnerDebugEnum.Error);

			}
		
		}

		internal void ProcessDamageAndEffects(CollisionResultSimple result) {

			if (BeamData.Damage.RegularDamage) {

				_cooldownRegular -= 5;

				if (_cooldownRegular <= 0) {

					_cooldownRegular = BeamData.Damage.RegularDamageCooldown;
					RegularDamage(result);

				}
			
			}

		}

		internal void RegularDamage(CollisionResultSimple result) {

			if (result.HitType == CollisionType.Grid && result.HitObject != null) {

				if (!BeamData.Damage.PenetrativeDamage) {

					result.HitObject.DoDamage(BeamData.Damage.RegularDamageAmount, MyStringHash.GetOrCompute("MesEnergyWeapon"), true, null, Weapon.EntityId, result.HitEntity.EntityId);

				} else {

					_penetrateCells.Clear();
					var hitGrid = result.HitEntity as IMyCubeGrid;
					var remainingDist = BeamData.Behavior.MaxRange - Vector3D.Distance(Weapon.GetPosition(), result.HitPosition);
					hitGrid.RayCastCells(result.HitPosition, remainingDist * Weapon.WorldMatrix.Forward + result.HitPosition, _penetrateCells);
					var damagePool = BeamData.Damage.RegularDamageAmount;
					var furthestHit = Vector3D.Zero;

					foreach (var cell in _penetrateCells) {

						if (damagePool <= 0)
							break;

						var worldCell = hitGrid.GridIntegerToWorld(cell);

						if (Vector3D.Distance(worldCell, Weapon.GetPosition()) > BeamData.Behavior.MaxRange)
							break;

						furthestHit = worldCell;

						if (hitGrid.WorldAABB.Contains(worldCell) == ContainmentType.Disjoint)
							break;

						var block = hitGrid.GetCubeBlock(cell);

						if (block == null)
							continue;

						var blockHealth = (float)Math.Round(block.BuildIntegrity - block.CurrentDamage, 3);

						if (blockHealth > damagePool) {

							block.DoDamage(damagePool, MyStringHash.GetOrCompute("MesEnergyWeapon"), true, null, Weapon.EntityId, result.HitEntity.EntityId);

						} else {

							block.DoDamage(blockHealth + 1, MyStringHash.GetOrCompute("MesEnergyWeapon"), true, null, Weapon.EntityId, result.HitEntity.EntityId);
							damagePool -= (blockHealth + 1);

						}

					}

					if (furthestHit != Vector3D.Zero)
						_hitOverride = furthestHit;

				}

				
				return;
			
			}

			if (result.HitType == CollisionType.Player && result.HitObject != null) {

				result.HitObject.DoDamage(BeamData.Damage.RegularDamageAmount, MyStringHash.GetOrCompute("MesEnergyWeapon"), true, null, Weapon.EntityId, result.HitEntity.EntityId);

			}

			if (result.HitType == CollisionType.Shield) {

				APIs.Shields.PointAttackShield(result.HitEntity as IMyTerminalBlock, result.HitPosition, Weapon.EntityId, BeamData.Damage.RegularDamageAmount, true, true);
			
			}
		
		}

		internal void ExplosiveHit(CollisionResultSimple result) {

			MyExplosionTypeEnum explosionType = MyExplosionTypeEnum.WARHEAD_EXPLOSION_50;

			if (BeamData.Damage.ExplosionDamageRadius < 2f) {

				explosionType = MyExplosionTypeEnum.WARHEAD_EXPLOSION_02;

			} else if (BeamData.Damage.ExplosionDamageRadius < 15f) {

				explosionType = MyExplosionTypeEnum.WARHEAD_EXPLOSION_15;

			} else if (BeamData.Damage.ExplosionDamageRadius < 30f) {

				explosionType = MyExplosionTypeEnum.WARHEAD_EXPLOSION_30;

			}

			MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
			myExplosionInfo.PlayerDamage = 0f;
			myExplosionInfo.OriginEntity = Weapon.EntityId;
			myExplosionInfo.Damage = BeamData.Damage.ExplosionDamageAmount;
			myExplosionInfo.ExplosionType = explosionType;
			myExplosionInfo.ExplosionSphere = new BoundingSphereD(result.HitPosition, BeamData.Damage.ExplosionDamageRadius);
			myExplosionInfo.LifespanMiliseconds = 700;
			myExplosionInfo.ParticleScale = 1f;
			myExplosionInfo.Direction = Vector3.Down;
			myExplosionInfo.VoxelExplosionCenter = result.HitPosition;

			var fakeExplosionFlagsTemp = (FakeExplosionFlags.CREATE_DEBRIS | FakeExplosionFlags.AFFECT_VOXELS | FakeExplosionFlags.APPLY_FORCE_AND_DAMAGE | FakeExplosionFlags.CREATE_DECALS | FakeExplosionFlags.CREATE_PARTICLE_EFFECT | FakeExplosionFlags.CREATE_SHRAPNELS | FakeExplosionFlags.APPLY_DEFORMATION);
			//myExplosionInfo.ExplosionFlags = (MyExplosionFlags.CREATE_DEBRIS | MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION);
			Enum.TryParse((fakeExplosionFlagsTemp.ToString()), out myExplosionInfo.ExplosionFlags);

			myExplosionInfo.VoxelCutoutScale = 1f;
			myExplosionInfo.PlaySound = true;
			myExplosionInfo.ApplyForceAndDamage = true;

			myExplosionInfo.StrengthImpulse = BeamData.Damage.ExplosionImpulse;

			myExplosionInfo.ObjectsRemoveDelayInMiliseconds = 40;
			myExplosionInfo.CreateParticleEffect = true;
			myExplosionInfo.AffectVoxels = BeamData.Damage.ExplosiveDamagesVoxels;
			MyExplosionInfo explosionInfo = myExplosionInfo;
			MyExplosions.AddExplosion(ref explosionInfo);

			if (result.HitType == CollisionType.Shield) {

				APIs.Shields.PointAttackShield(result.HitEntity as IMyTerminalBlock, result.HitPosition, Weapon.EntityId, BeamData.Damage.RegularDamageAmount * 5, true, true);

			}

		}

		public void DrawBeams() {
		
			
		
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
