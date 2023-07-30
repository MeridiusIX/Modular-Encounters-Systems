using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;
using static ModularEncountersSystems.API.WcApiDef;
using static ModularEncountersSystems.API.WcApiDef.WeaponDefinition;

namespace ModularEncountersSystems.Behavior.Subsystems.Weapons {
	public class CoreWeapon : BaseWeapon, IWeapon{

		internal int _weaponId;

		internal Dictionary<string, MyDefinitionId> _ammoToMagazine = new Dictionary<string, MyDefinitionId>();
		internal Dictionary<string, AmmoDef> _ammoToDefinition = new Dictionary<string, AmmoDef>();
		internal bool _requiresPhysicalAmmo;
		internal bool _beamAmmo;
		
		internal bool _flareAmmo;

		internal WeaponDefinition _weaponDefinition;

		public CoreWeapon(IMyTerminalBlock block, IMyRemoteControl remoteControl, IBehavior behavior, WeaponDefinition weaponDefinition, int weaponId) : base(block, remoteControl, behavior) {

			if (BlockManager.AllWeaponCoreGuns.Contains(block.SlimBlock.BlockDefinition.Id)) {

				//BehaviorLogger.Write(block.CustomName + " Is WeaponCore Static Weapon", BehaviorDebugEnum.Weapon);
				_isStatic = true;

			} else {

				//BehaviorLogger.Write(block.CustomName + " Is WeaponCore Turret Weapon", BehaviorDebugEnum.Weapon);
				_isTurret = true;

			}

			_weaponDefinition = weaponDefinition;
			_weaponId = weaponId;
			_isWeaponCore = true;

			//Rate Of Fire
			_rateOfFire = _weaponDefinition.HardPoint.Loading.RateOfFire;

			//Get Ammo Stuff

			//BehaviorLogger.Write(_block.CustomName + " Available Ammo Check", BehaviorDebugEnum.Weapon);
			if (_weaponDefinition.Ammos.Length > 0) {

				foreach (var ammo in _weaponDefinition.Ammos) {

					//BehaviorLogger.Write(string.Format(" - {0} / {1}", ammo.AmmoMagazine, ammo.AmmoRound), BehaviorDebugEnum.Weapon);

					if (!_ammoToMagazine.ContainsKey(ammo.AmmoRound))
						_ammoToMagazine.Add(ammo.AmmoRound, new MyDefinitionId(typeof(MyObjectBuilder_AmmoMagazine), ammo.AmmoMagazine));

					if (!_ammoToDefinition.ContainsKey(ammo.AmmoRound))
						_ammoToDefinition.Add(ammo.AmmoRound, ammo);

				}

			} else {

				//BehaviorLogger.Write(_block.CustomName + " Has No WC Ammo Definitions", BehaviorDebugEnum.Weapon);
				_isValid = false;
			
			}

		}

		private void StaticWeaponReadiness() {

			//BehaviorLogger.Write("WC Check Static ", BehaviorDebugEnum.Weapon);
			var trajectory = _weaponSystem.Data.MaxStaticWeaponRange > -1 ? MathHelper.Clamp(_ammoMaxTrajectory, 0, _weaponSystem.Data.MaxStaticWeaponRange) : _ammoMaxTrajectory;

			//Homing Weapon
			if (_homingAmmo) {

				if (_weaponSystem.Data.AllowHomingWeaponMultiTargeting) {
				
					//TODO: Later Date
				
				} else {

					if (_behavior.AutoPilot.Targeting.HasTarget()) {

						bool threatMatch = false;

						foreach (var threatType in _weaponDefinition.Targeting.Threats) {

							if (threatType == TargetingDef.Threat.Characters && _behavior.AutoPilot.Targeting.Target.GetEntityType() == EntityType.Player) {

								BehaviorLogger.Write(" - Homing Threat Is Matched To Player", BehaviorDebugEnum.Weapon);
								threatMatch = true;
								break;

							}

							if (threatType == TargetingDef.Threat.Grids && _behavior.AutoPilot.Targeting.Target.GetEntityType() == EntityType.Grid) {

								BehaviorLogger.Write(" - Homing Threat Is Matched To Grid", BehaviorDebugEnum.Weapon);
								threatMatch = true;
								break;

							}

						}

						if (!threatMatch) {

							Status = WeaponStatusEnum.NoHomingTarget;
							//BehaviorLogger.Write(" - No Autopilot Target To assign For Homing Ammo", BehaviorDebugEnum.Weapon);
							_readyToFire = false;
							return;

						}

					} else {

						Status = WeaponStatusEnum.NoHomingTarget;
						//BehaviorLogger.Write(" - No Autopilot Target To assign For Homing Ammo", BehaviorDebugEnum.Weapon);
						_readyToFire = false;
						return;

					}

					if (_behavior.BehaviorSettings.HomingWeaponRangeOverride != -1 && trajectory > _behavior.BehaviorSettings.HomingWeaponRangeOverride)
						trajectory = _behavior.BehaviorSettings.HomingWeaponRangeOverride;

					if (trajectory < _behavior.AutoPilot.Targeting.Target.Distance(_block.GetPosition())) {

						Status = WeaponStatusEnum.OutsideHomingRange;
						//BehaviorLogger.Write(" - Target Out Of Range For Homing Ammo", BehaviorDebugEnum.Weapon);
						_readyToFire = false;
						return;

					}
				
				}

				Status = WeaponStatusEnum.ReadyToFire;
				BehaviorLogger.Write(" - Got Target For Homing Shot", BehaviorDebugEnum.Weapon);
				return;

			}

			//Flare Weapon
			if (_flareAmmo) {

				if (!_weaponSystem.Data.UseAntiSmartWeapons || !_weaponSystem.IncomingHomingProjectiles) {

					Status = WeaponStatusEnum.NoIncomingHomingProjectiles;
					//BehaviorLogger.Write(" - No Incoming Homing Weapons For Firing Flares", BehaviorDebugEnum.Weapon);
					_readyToFire = false;
					return;

				}

				Status = WeaponStatusEnum.ReadyToFire;
				//BehaviorLogger.Write(" - Homing Threats Detected For Firing Flares", BehaviorDebugEnum.Weapon);
				return;
			
			}

			//---------------------
			//----Other Weapons----
			//---------------------

			_readyToFire = StaticWeaponAlignedToTarget(_beamAmmo);
			BehaviorLogger.Write("WC Ready To Fire: " + _readyToFire, BehaviorDebugEnum.Weapon);

		}

		public bool CanLockOnGrid(IMyCubeGrid target) {

			if (_isStatic) {

				//BehaviorLogger.Write(" - Non Static Weapon Cannot Be Used For MES Homing Management", BehaviorDebugEnum.Weapon);
				return false;

			}
				

			if (target == null || _block == null) {

				//BehaviorLogger.Write(" - Homing Target or Block Null", BehaviorDebugEnum.Weapon);
				return false;

			}

			if (target.Closed || _block.Closed) {

				//BehaviorLogger.Write(" - Homing Target or Block Closed", BehaviorDebugEnum.Weapon);
				return false;

			}

			//Ammo Firing Range Check
			if (Vector3D.Distance(target.GetPosition(), _block.GetPosition()) > MaxAmmoTrajectory()) {

				//BehaviorLogger.Write(" - Homing Target Further Than Max Ammo Trajectory", BehaviorDebugEnum.Weapon);
				return false;

			}

			//LOS Check
			if (!_homingAmmo && !TurretHasLOS(target.GetPosition(), _block.GetPosition(), _behavior.CurrentGrid?.LinkedGrids)) {

				//BehaviorLogger.Write(" - Non homing ammo LOS fail", BehaviorDebugEnum.Weapon);
				return false;

			}

			return true;

		}

		public bool HasAmmo() {

			bool gotAmmoDetails = false;
			bool gotAmmoResult = false;

			if (_currentAmmoMagazine == new MyDefinitionId()) {

				gotAmmoDetails = true;
				gotAmmoResult = GetAmmoDetails();

			}

			if (MyAPIGateway.Session.CreativeMode || !_requiresPhysicalAmmo)
				return true;

			if (_inventory.GetItemAmount(_currentAmmoMagazine) == 0) {

				if (_weaponSystem.Data.UseAmmoReplenish && _ammoRefills < _weaponSystem.Data.MaxAmmoReplenishments) {

					_pendingAmmoRefill = true;

				} else {

					return false;

				}

			}

			if(!gotAmmoDetails)
				gotAmmoResult = GetAmmoDetails();


			return gotAmmoResult;
		
		}

		private bool GetAmmoDetails() {

			//BehaviorLogger.Write(string.Format(" - Getting Ammo Details For Core Weapon: {0}", _block.CustomName), BehaviorDebugEnum.Weapon);
			var currentAmmo = APIs.WeaponCore.GetActiveAmmo(_block as MyEntity, _weaponId);
			//BehaviorLogger.Write(string.Format(" - CurrentAmmo For Core Weapon {0}: {1}", _block.CustomName, currentAmmo ?? "N/A"), BehaviorDebugEnum.Weapon);

			if (_ammoRound != currentAmmo) {

				//BehaviorLogger.Write(" - Create New Ammo Def", BehaviorDebugEnum.Weapon);
				var ammoDef = new AmmoDef();

				if (currentAmmo != null) {

					//BehaviorLogger.Write(" - Try Get From Ammo-To-Def", BehaviorDebugEnum.Weapon);
					_ammoToDefinition.TryGetValue(currentAmmo, out ammoDef);
					//BehaviorLogger.Write(" - Try Get From Ammo-To-Def", BehaviorDebugEnum.Weapon);

				} else {

					//BehaviorLogger.Write(" - Try Get From Index 0", BehaviorDebugEnum.Weapon);
					ammoDef = _weaponDefinition.Ammos[0];

				}

				if (!string.IsNullOrWhiteSpace(ammoDef?.AmmoRound)) {

					//BehaviorLogger.Write(" - Populate Ammo Data", BehaviorDebugEnum.Weapon);
					_ammoRound = currentAmmo;
					_currentAmmoMagazine = new MyDefinitionId(typeof(MyObjectBuilder_AmmoMagazine), ammoDef.AmmoMagazine);
					_requiresPhysicalAmmo = _currentAmmoMagazine.SubtypeName != "Energy";
					_beamAmmo = ammoDef.Beams.Enable;
					_homingAmmo = ammoDef.Trajectory.Guidance == AmmoDef.TrajectoryDef.GuidanceType.Smart || ammoDef.Trajectory.Guidance == AmmoDef.TrajectoryDef.GuidanceType.TravelTo;
					_flareAmmo = ammoDef.AreaEffect.AreaEffect == AmmoDef.AreaDamageDef.AreaEffectType.AntiSmart;
					_ammoMaxTrajectory = ammoDef.Trajectory.MaxTrajectory;
					_ammoMaxVelocity = _beamAmmo ? -1 : ammoDef.Trajectory.DesiredSpeed;
					_ammoInitialVelocity = _beamAmmo ? -1 : ammoDef.Trajectory.DesiredSpeed;
					_ammoAcceleration = _beamAmmo ? -1 : ammoDef.Trajectory.AccelPerSec;

				} else {

					//BehaviorLogger.Write(" - AmmoDef Was Null", BehaviorDebugEnum.Weapon);
					return false;

				}

			}

			return true;

		}

		public IMyEntity CurrentTarget() {

			return !IsValid() ? null : APIs.WeaponCore.GetWeaponTarget(_block as MyEntity, _weaponId).Item4;

		}

		public void DetermineWeaponReadiness(bool usingTurretController = false) {

			BehaviorLogger.Write(_block.CustomName + " WC Check Readiness", BehaviorDebugEnum.Weapon);

			_readyToFire = true;

			//Valid
			if (!IsValid() || !IsActive()) {

				BehaviorLogger.Write(string.Format(" - Core Valid/Active Check Failed: {0} / {1}", IsValid(), IsActive()), BehaviorDebugEnum.Weapon);
				Status = WeaponStatusEnum.NonActiveEntity;
				_readyToFire = false;
				return;

			}

			//Ammo
			if (!HasAmmo()) {

				BehaviorLogger.Write(string.Format(" - AmmoRound: {0} /// AmmoMag: {1}", _ammoRound ?? "null", _currentAmmoMagazine.SubtypeName ?? "null"), BehaviorDebugEnum.Weapon);
				Status = WeaponStatusEnum.NoAmmo;
				_readyToFire = false;
				return;

			}

			//WeaponCoreReadyFireCheck
			if (_isStatic && !usingTurretController)
				StaticWeaponReadiness();

		}

		public void FireOnce() {

			if (_isTurret)
				return;

			if (_isValid && IsActive() && _readyToFire) {

				if (IsHoming) {
				

				
				}

				//BehaviorLogger.Write(_block.CustomName + " Fire Once", BehaviorDebugEnum.Weapon);
				APIs.WeaponCore.FireWeaponOnce(_block as MyEntity, false, _weaponId);

			}
	
		}

		public override bool IsBarrageWeapon() {

			if (!_checkBarrageWeapon) {

				_checkBarrageWeapon = true;

				if (_isStatic && _weaponSystem.Data.UseBarrageFire) {

					_isBarrageWeapon = _rateOfFire < _weaponSystem.Data.MaxFireRateForBarrageWeapons;

				}

			}

			return _isBarrageWeapon;

		}

		public void SetLockOnTarget() {

			PendingLockOn = false;

			if (!IsValid())
				return;

			APIs.WeaponCore.SetWeaponTarget(_block as MyEntity, LockOnTarget as MyEntity, _weaponId);
			//APIs.WeaponCore.SetAiFocus(_block, LockOnTarget);

		}

		public void SetTarget(IMyEntity entity) {

			if (!IsValid())
				return;

			APIs.WeaponCore.SetWeaponTarget(_block as MyEntity, entity as MyEntity, _weaponId);
			//APIs.WeaponCore.SetAiFocus(_block, entity);

		}

		public void ToggleFire() {

			if (_isTurret)
				return;

			BehaviorLogger.Write(_block.CustomName + " Valid:  " + _isValid, BehaviorDebugEnum.Weapon);
			BehaviorLogger.Write(_block.CustomName + " Active: " + IsActive(), BehaviorDebugEnum.Weapon);
			BehaviorLogger.Write(_block.CustomName + " Ready:  " + _readyToFire, BehaviorDebugEnum.Weapon);
			//BehaviorLogger.Write(_block.CustomName + " Barra:  " + _isBarrageWeapon, BehaviorDebugEnum.Weapon);
			//BehaviorLogger.Write(_block.CustomName + " Firing: " + _firing, BehaviorDebugEnum.Weapon);

			/*
			if (IsHoming && _readyToFire) {

				var blockEntity = _block as MyEntity;
				var targetEntity = _behavior.AutoPilot.Targeting.Target.GetEntity() as MyEntity;
				//BehaviorLogger.Write(_block.CustomName + " Homing Block Null:  " + (blockEntity == null), BehaviorDebugEnum.Weapon);
				//BehaviorLogger.Write(_block.CustomName + " Homing Target Null: " + (targetEntity == null), BehaviorDebugEnum.Weapon);

				APIs.WeaponCore.SetAiFocus(blockEntity, targetEntity);
				APIs.WeaponCore.SetWeaponTarget(blockEntity, targetEntity, _weaponId);
			
			}
			*/

			if (_isValid && IsActive() && _readyToFire && !_isBarrageWeapon) {

				if (!_firing) {

					BehaviorLogger.Write(_block.CustomName + " Start Fire", BehaviorDebugEnum.Weapon);
					_firing = true;
					APIs.WeaponCore.ToggleWeaponFire(_block as MyEntity, true, false, _weaponId);

				}

			} else {

				if (_firing) {

					//BehaviorLogger.Write(_block.CustomName + " End Fire", BehaviorDebugEnum.Weapon);
					_firing = false;
					APIs.WeaponCore.ToggleWeaponFire(_block as MyEntity, false, false, _weaponId);

				}

			}

		}

	}
}
