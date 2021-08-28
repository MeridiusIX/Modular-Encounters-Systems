using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems.Weapons {
	public abstract class BaseWeapon {

		internal IMyFunctionalBlock _block;
		internal IMyRemoteControl _remoteControl;

		internal IBehavior _behavior;
		internal WeaponSystem _weaponSystem;

		internal bool _isStatic;
		internal bool _isTurret;
		internal bool _isWeaponCore;
		internal bool _isBarrageWeapon;
		internal bool _isOnSubgrid;

		internal Direction _direction;

		internal float _rateOfFire;

		internal MyDefinitionId _currentAmmoMagazine;
		internal string _ammoRound;
		internal float _ammoAmount;
		internal float _ammoMaxTrajectory;
		internal float _ammoInitialVelocity;
		internal float _ammoAcceleration;
		internal float _ammoMaxVelocity;

		internal bool _checkBarrageWeapon;

		internal bool _pendingAmmoRefill;
		internal int _ammoRefills;

		internal bool _closed;
		internal bool _functional;
		internal bool _working;
		internal bool _enabled;
		internal bool _readyToFire;
		internal bool _firing;
		internal DateTime _lastFireTime;

		internal bool _isValid;

		internal MyInventory _inventory;

		public BaseWeapon(IMyTerminalBlock block, IMyRemoteControl remoteControl, IBehavior behavior) {

			_block = block as IMyFunctionalBlock;

			if (_block == null)
				return;

			_remoteControl = remoteControl;
			_behavior = behavior;
			_weaponSystem = behavior.AutoPilot.Weapons;

			_isStatic = false;
			_isTurret = false;
			_isWeaponCore = false;
			_isBarrageWeapon = false;
			_isOnSubgrid = _block.CubeGrid != _remoteControl.CubeGrid;

			_direction = Direction.None;

			_currentAmmoMagazine = new MyDefinitionId();

			_ammoRefills = 0;
			_ammoAmount = 0;
			_ammoMaxTrajectory = 0;
			_ammoInitialVelocity = 0;
			_ammoAcceleration = 0;
			_ammoMaxVelocity = 0;

			_checkBarrageWeapon = false;

			_closed = false;
			_functional = false;
			_working = false;
			_enabled = false;
			_readyToFire = false;
			_firing = false;
			_lastFireTime = MyAPIGateway.Session.GameDateTime;

			_isValid = true;

			_inventory = (block as MyEntity).GetInventory();

			_block.IsWorkingChanged += WorkingChange;
			WorkingChange(_block);

			_block.EnabledChanged += EnableChange;
			EnableChange(_block);

			_block.OnClose += CloseChange;

		}

		public void WorkingChange(IMyCubeBlock block) {

			_functional = block.IsFunctional;
			_working = block.IsWorking;

		}

		public void EnableChange(IMyTerminalBlock block) {

			_enabled = _block.Enabled;

		}

		public void CloseChange(IMyEntity entity) {

			_closed = entity.MarkedForClose || entity.Closed;

		}

		public bool StaticWeaponAlignedToTarget(bool mustContactHostile = false) {

			var waypointCoords = _behavior.AutoPilot.GetCurrentWaypoint();
			var targetCoords = _behavior.AutoPilot.Targeting.TargetLastKnownCoords;
			var shootableCoords = Vector3D.Zero;
			var collisionCoords = Vector3D.Zero;

			bool isWaypointTarget = _weaponSystem.WaypointIsTarget;
			bool hasValidTarget = _behavior.AutoPilot.Targeting.HasTarget();

			bool canShootWaypoint = false;
			bool canShootTarget = false;
			bool canShootCollision = false;

			var trajectory = _weaponSystem.MaxStaticWeaponRange > -1 ? MathHelper.Clamp(_ammoMaxTrajectory, 0, _weaponSystem.MaxStaticWeaponRange) : _ammoMaxTrajectory;

			//Waypoint Details
			if (isWaypointTarget) {

				canShootWaypoint = StaticDistanceAndAngleCheck(waypointCoords, trajectory);

			}

			//Target Details
			if (!canShootWaypoint && hasValidTarget) {

				canShootTarget = StaticDistanceAndAngleCheck(targetCoords, trajectory);

			}

			shootableCoords = canShootWaypoint ? waypointCoords : (canShootTarget ? targetCoords : Vector3D.Zero);
			var maxTargetTrajectory = shootableCoords != Vector3D.Zero ? Vector3D.Distance(shootableCoords, _block.GetPosition()) : trajectory;

			//Collision
			var collision = _behavior.AutoPilot.Collision.GetResult(_direction);
			bool hasCollision = false;
			bool hostile = false;
			bool collisionIsGrid = false;

			if (collision == null) {

				//Logger.Write(" - Collision Null", BehaviorDebugEnum.Weapon);

			} else {

				if (collision.HasTarget(maxTargetTrajectory)) {

					if ((collision.Type == CollisionType.Voxel && !collision.CollisionIsWater) || collision.Type == CollisionType.Safezone) {

						//Logger.Write(" - Voxel Collision or SafeZone", BehaviorDebugEnum.Weapon);
						hasCollision = true;

					}

					if (collision.Type == CollisionType.Grid || collision.Type == CollisionType.Shield) {

						hasCollision = true;
						collisionIsGrid = true;
						bool hostileEntity = collision.GridRelation == TargetRelationEnum.Enemy || collision.ShieldRelation == TargetRelationEnum.Enemy;

						if (!hostileEntity && _behavior.AutoPilot.DistanceToCurrentWaypoint > collision.GetCollisionDistance()) {

							//Logger.Write(" - Target On Other Side Of Friendly Grid", BehaviorDebugEnum.Weapon);
							hasCollision = true;

						}

						if (hostileEntity) {

							collisionCoords = collision.GetCollisionCoords();
							hostile = true;

						}

					}

				}
			
			}

			if (!canShootWaypoint && !canShootTarget)
				return false;

			if (hasCollision && !hostile)
				return false;

			if (mustContactHostile && !hostile)
				return false;

			return true;
		
		}

		public bool StaticDistanceAndAngleCheck(Vector3D coords, double trajectory) {

			var directionToTarget = Vector3D.Normalize(coords - _block.GetPosition());
			var allowedAngle = VectorHelper.GetAngleBetweenDirections(directionToTarget, _block.WorldMatrix.Forward) <= _weaponSystem.WeaponMaxAngleFromTarget;
			var dist = _weaponSystem.IndirectWaypointTargetDistance > -1 ? _weaponSystem.IndirectWaypointTargetDistance : Vector3D.Distance(_block.GetPosition(), coords);
			var reqTrajectory = dist > trajectory ? trajectory : trajectory - (trajectory - dist);
			var distFromMaxTrajectory = Vector3D.Distance(coords, _block.WorldMatrix.Forward * reqTrajectory + _block.GetPosition());

			if (allowedAngle && distFromMaxTrajectory < _weaponSystem.WeaponMaxBaseDistanceTarget && dist < trajectory) {

				return true;

			}

			return false;

		}

		//-------------------------------------------------------
		//------------START INTERFACE METHODS--------------------
		//-------------------------------------------------------

		public double AmmoAcceleration() {

			return _ammoAcceleration;
		
		}

		public double AmmoInitialVelocity() {

			return _ammoInitialVelocity;

		}

		public double AmmoVelocity() {

			return _ammoMaxVelocity;

		}

		public IMyFunctionalBlock Block() {

			return _block;
		
		}

		public Direction GetDirection() {

			return _direction;
		
		}

		public virtual bool IsActive() {

			return _functional && _working && _enabled;

		}

		public virtual bool IsBarrageWeapon() {

			return _isBarrageWeapon;

		}

		public bool IsReadyToFire() {

			return _readyToFire;

		}

		public bool IsStaticGun() {

			return _isStatic;

		}

		public bool IsTurret() {

			return _isTurret;
		
		}

		public virtual bool IsValid() {

			if (!_isValid)
				return false;

			if (_closed && _isValid) {

				_isValid = false;
				return false;

			}
				

			if (_remoteControl?.SlimBlock?.CubeGrid != null) {

				if (_block.CubeGrid == _remoteControl.CubeGrid) {

					return true;
				
				}

				if (_block.SlimBlock.CubeGrid.IsInSameLogicalGroupAs(_remoteControl.SlimBlock.CubeGrid)) {

					return true;
				
				}

			}

			_isValid = false;
			return false;

		}

		public float MaxAmmoTrajectory() {

			return _ammoMaxTrajectory;
		
		}

		public void ReplenishAmmo() {

			if (!_pendingAmmoRefill)
				return;

			_pendingAmmoRefill = false;

			BehaviorLogger.Write("Sending Replenish Request For Weapon: " + _block?.SlimBlock?.BlockDefinition?.Id ?? "Null", BehaviorDebugEnum.Weapon);
			InventoryHelper.AddItemsToInventory(_inventory, _currentAmmoMagazine, _weaponSystem.AmmoReplenishClipAmount);
			_ammoRefills++;

		}

		public void SetDirection(Direction direction) {

			_direction = direction;

		}

	}

}
