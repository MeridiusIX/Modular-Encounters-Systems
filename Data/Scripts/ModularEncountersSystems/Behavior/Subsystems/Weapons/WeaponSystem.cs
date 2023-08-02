using System;
using System.Collections.Generic;
using System.Text;
using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior.Subsystems.Profiles;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Manipulation;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;
using static ModularEncountersSystems.API.WcApiDef;

namespace ModularEncountersSystems.Behavior.Subsystems.Weapons {
	public class WeaponSystem {

		public string ProfileSubtypeId { get { return _behavior.BehaviorSettings.WeaponsSystemProfile; } set { _behavior.BehaviorSettings.WeaponsSystemProfile = value; } }

		public WeaponSystemReference Data { 
			get {

				if (ProfileSubtypeId != _data?.ProfileSubtypeId) {

					if (!string.IsNullOrWhiteSpace(ProfileSubtypeId) && ProfileManager.WeaponProfiles.TryGetValue(ProfileSubtypeId, out _data))
						return _data;

					ProfileSubtypeId = "MES-Weapons-GenericStandard";

					if (ProfileManager.WeaponProfiles.TryGetValue(ProfileSubtypeId, out _data))
						return _data;

					BehaviorLogger.Write("Behavior Could Not Find Weapon Profile or Backup Weapon Profile. Behavior May Critically Fail", BehaviorDebugEnum.Error, true);

				}

				return _data;
			
			}
		}
		private WeaponSystemReference _data;

		//Non-Configurable
		private IMyRemoteControl _remoteControl;

		private IBehavior _behavior;

		public List<IWeapon> AllWeapons;
		public List<IWeapon> TurretControllers;
		public List<IWeapon> StaticWeapons;
		public List<IWeapon> Turrets;

		private DateTime _collisionTimer;
		private DateTime _lastAiFocusSetTime;

		private bool _parallelWorkInProgress;
		private bool _pendingBarrageTrigger;
		private int _barrageWeaponIndex;

		private List<WaypointModificationEnum> _allowedFlags;
		private List<WaypointModificationEnum> _restrictedFlags;

		public Dictionary<Direction, double> MaxStaticRangesPerDirection;

		//WeaponLead Stuff

		public IWeapon PrimaryStaticWeapon;
		
		
		public bool WaypointIsTarget;
		public double IndirectWaypointTargetDistance;

		public bool IncomingHomingProjectiles;
		public DateTime LastHomingTargetCheck;
		public bool GetRandomHomingTarget;

		public bool PendingTargetLockOn;

		public WeaponSystem(IMyRemoteControl remoteControl, IBehavior behavior) {

			_behavior = behavior;

			if (remoteControl == null || !MyAPIGateway.Entities.Exist(remoteControl?.SlimBlock?.CubeGrid))
				return;

			_remoteControl = remoteControl;
			
			_allowedFlags = new List<WaypointModificationEnum>();
			_allowedFlags.Add(WaypointModificationEnum.TargetIsInitialWaypoint);
			_allowedFlags.Add(WaypointModificationEnum.WeaponLeading);
			_allowedFlags.Add(WaypointModificationEnum.CollisionLeading);

			_restrictedFlags = new List<WaypointModificationEnum>();
			_restrictedFlags.Add(WaypointModificationEnum.Collision);
			_restrictedFlags.Add(WaypointModificationEnum.Offset);
			_restrictedFlags.Add(WaypointModificationEnum.PlanetPathing);
			_restrictedFlags.Add(WaypointModificationEnum.PlanetPathingAscend);
			_restrictedFlags.Add(WaypointModificationEnum.WaterPathing);
			_restrictedFlags.Add(WaypointModificationEnum.EscortPathing);
			_restrictedFlags.Add(WaypointModificationEnum.CircleTarget);

			AllWeapons = new List<IWeapon>();
			TurretControllers = new List<IWeapon>();
			StaticWeapons = new List<IWeapon>();
			Turrets = new List<IWeapon>();

			_collisionTimer = MyAPIGateway.Session.GameDateTime;
			_lastAiFocusSetTime = MyAPIGateway.Session.GameDateTime;

			MaxStaticRangesPerDirection = new Dictionary<Direction, double>();
			MaxStaticRangesPerDirection.Add(Direction.Forward, 0);
			MaxStaticRangesPerDirection.Add(Direction.Backward, 0);
			MaxStaticRangesPerDirection.Add(Direction.Up, 0);
			MaxStaticRangesPerDirection.Add(Direction.Down, 0);
			MaxStaticRangesPerDirection.Add(Direction.Left, 0);
			MaxStaticRangesPerDirection.Add(Direction.Right, 0);

			WaypointIsTarget = false;
			IndirectWaypointTargetDistance = -1;

			IncomingHomingProjectiles = false;
			LastHomingTargetCheck = DateTime.MinValue;
			GetRandomHomingTarget = false;

			PendingTargetLockOn = false;

		}

		public void InitTags(string existingWeaponSystem) {

			if (string.IsNullOrWhiteSpace(_remoteControl?.CustomData) == false) {

				var descSplit = _remoteControl.CustomData.Split('\n');

				foreach (var tag in descSplit) {

					//WeaponsSystem
					if (tag.Contains("[WeaponsSystem:") || tag.Contains("[WeaponSystem:")) {

						TagParse.TagStringCheck(tag, ref _behavior.BehaviorSettings.WeaponsSystemProfile);

					}

				}

			}

		}

		public void Setup() {

			var blocks = new List<IMySlimBlock>();
			var grids = new List<IMyCubeGrid>();
			MyAPIGateway.GridGroups.GetGroup(_remoteControl.SlimBlock.CubeGrid, GridLinkTypeEnum.Physical, grids);
			GridManager.GetBlocksFromGrid<IMyTerminalBlock>(_remoteControl.SlimBlock.CubeGrid, blocks, true);
			//BehaviorLogger.Write("Weapon Scan Linked Grids: " + grids.Count, BehaviorDebugEnum.BehaviorSetup);
			//BehaviorLogger.Write("WCAPI Ready: " + APIs.WeaponCore.IsReady.ToString(), BehaviorDebugEnum.Weapon);
			//BehaviorLogger.Write(string.Format("All WC: {0} /// All WCS: {1} /// All WCT: {2}", Utilities.AllWeaponCoreBlocks.Count, Utilities.AllWeaponCoreGuns.Count, Utilities.AllWeaponCoreTurrets.Count), BehaviorDebugEnum.Weapon);

			foreach (var slimBlock in blocks) {

				IWeapon weapon = null;
				var block = slimBlock.FatBlock as IMyTerminalBlock;

				//BehaviorLogger.Write(block.CustomName + " Has Core Weapon: " + APIs.WeaponCore.HasCoreWeapon(block).ToString(), BehaviorDebugEnum.Weapon);

				if (block as IMyTurretControlBlock != null) {

					weapon = new ControllerWeapon(block, _remoteControl, _behavior);
					AllWeapons.Add(weapon);
					TurretControllers.Add(weapon);
					continue;

				}

				if (APIs.WeaponCoreApiLoaded && BlockManager.AllWeaponCoreBlocks.Contains(block.SlimBlock.BlockDefinition.Id)) {

					var weaponsInBlock = new Dictionary<string, int>();
					APIs.WeaponCore.GetBlockWeaponMap(block, weaponsInBlock);

					BehaviorLogger.Write(block.CustomName + ": Core Weapons In Block: " + weaponsInBlock.Keys.Count, BehaviorDebugEnum.BehaviorSetup);
					foreach (var weaponName in weaponsInBlock.Keys) {

						WeaponDefinition weaponDef = new WeaponDefinition();

						foreach (var definition in APIs.WeaponCore.WeaponDefinitions) {

							if (definition.HardPoint.PartName == weaponName) {

								weaponDef = definition;
								break;

							}

						}

						weapon = new CoreWeapon(block, _remoteControl, _behavior, weaponDef, weaponsInBlock[weaponName]);

						if (!weapon.IsValid()) {

							BehaviorLogger.Write(block.CustomName + " Is Not Valid", BehaviorDebugEnum.BehaviorSetup);
							continue;

						}

						BehaviorLogger.Write(block.CustomName + " Is WeaponCore", BehaviorDebugEnum.BehaviorSetup);

						if (weapon.IsStaticGun()) {

							AllWeapons.Add(weapon);
							StaticWeapons.Add(weapon);

						} else {

							AllWeapons.Add(weapon);
							Turrets.Add(weapon);

						}

						continue;

					}

					APIs.WeaponCore.DisableRequiredPower(block as MyEntity);

					continue;

				} else if (block as IMyLargeTurretBase != null || block as IMyUserControllableGun != null) {

					weapon = new RegularWeapon(block, _remoteControl, _behavior);

				} else {

					continue;

				}

				if (!weapon.IsValid()) {

					BehaviorLogger.Write(block.CustomName + " Is Not Valid", BehaviorDebugEnum.BehaviorSetup);
					continue;

				}

				BehaviorLogger.Write(block.CustomName + " Is RegularWeapon", BehaviorDebugEnum.BehaviorSetup);

				if (weapon.IsStaticGun()) {

					AllWeapons.Add(weapon);
					StaticWeapons.Add(weapon);

				} else {

					AllWeapons.Add(weapon);
					Turrets.Add(weapon);

				}

			}

			foreach (var weapon in TurretControllers) {

				var block = weapon.Block() as IMyTurretControlBlock;

				if (block == null)
					continue;

				string key = "";

				if (block.Storage == null || !block.Storage.TryGetValue(StorageTools.MesTurretControllerKey, out key))
					continue;

				for (int i = StaticWeapons.Count - 1; i >= 0; i--) {

					var storage = StaticWeapons[i]?.Block()?.Storage;

					if (storage == null)
						continue;

					var toolKey = "";

					if (!storage.TryGetValue(StorageTools.MesTurretControllerKey, out toolKey))
						continue;

					if (key != toolKey)
						continue;

					weapon.SubWeapons.Add(StaticWeapons[i]);
					StaticWeapons.RemoveAt(i);

				}

				BehaviorLogger.Write("Turret Controller [" + block.CustomName + "] Sub-Weapons: " + weapon.SubWeapons.Count, BehaviorDebugEnum.BehaviorSetup);

			}

			BehaviorLogger.Write(string.Format("{0}: Weapons Registered - Static: {1} - Turret: {2} - Controller: {3}", _remoteControl.CubeGrid.CustomName, StaticWeapons.Count, Turrets.Count, TurretControllers.Count), BehaviorDebugEnum.BehaviorSetup);

		}

		public void SetupReferences(IBehavior behavior) {

			_behavior = behavior;

		}

		public void PrepareWeapons() {

			try {

				var timeSpan = MyAPIGateway.Session.GameDateTime - _collisionTimer;

				if (timeSpan.TotalMilliseconds >= 1000) {

					_collisionTimer = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.Collision.RunSecondaryCollisionChecks();

				}

				CheckIfWaypointIsTarget();
				CheckForIncomingHomingProjectiles();

				foreach (var gun in StaticWeapons) {

					gun.DetermineWeaponReadiness();

				}

				foreach (var turret in Turrets) {

					turret.DetermineWeaponReadiness();

				}

				foreach (var controller in TurretControllers) {

					controller.DetermineWeaponReadiness();

				}

				RefreshMaxStaticRangeReferences();
				PrepareWeaponLockOn();

			} catch (Exception e) {

				BehaviorLogger.Write("Exception While Preparing Weapons", BehaviorDebugEnum.Weapon);
				BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Weapon);

			}
			

		}

		public void ProcessWeaponReloads() {

			foreach (var weapon in StaticWeapons) {

				weapon.ReplenishAmmo();

			}

			foreach (var weapon in Turrets) {

				weapon.ReplenishAmmo();

			}

			foreach (var weapon in TurretControllers) {

				weapon.ReplenishAmmo();

			}

		}

		public void CheckIfWaypointIsTarget() {

			IndirectWaypointTargetDistance = -1;

			foreach (var waypointType in _restrictedFlags) {

				if (_behavior.AutoPilot.IndirectWaypointType.HasFlag(waypointType)) {

					if (_behavior.AutoPilot.Targeting.HasTarget()) {

						var dirFromTargetToWaypoint = Vector3D.Normalize(_behavior.AutoPilot.RefBlockMatrixRotation.Translation - _behavior.AutoPilot.GetCurrentWaypoint());
						var dirFromTargetToNpc = Vector3D.Normalize(_behavior.AutoPilot.RefBlockMatrixRotation.Translation - _behavior.AutoPilot.Targeting.TargetLastKnownCoords);
						var angleToWaypoint = VectorHelper.GetAngleBetweenDirections(dirFromTargetToWaypoint, dirFromTargetToNpc);

						if (angleToWaypoint <= Data.WeaponMaxAngleFromTarget) {

							//BehaviorLogger.Write("Invalid Waypoint Lines Up With Target", BehaviorDebugEnum.Weapon);
							IndirectWaypointTargetDistance = Vector3D.Distance(_behavior.AutoPilot.RefBlockMatrixRotation.Translation, _behavior.AutoPilot.Targeting.TargetLastKnownCoords);
							break;

						}
		
					}

					WaypointIsTarget = false;
					return;
				
				}
			
			}

			foreach (var waypointType in _allowedFlags) {

				if (_behavior.AutoPilot.DirectWaypointType.HasFlag(waypointType)) {

					WaypointIsTarget = true;
					return;

				}

			}

			
			WaypointIsTarget = false;

		}

		public void CheckForIncomingHomingProjectiles() {

			if (!Data.UseAntiSmartWeapons || !APIs.WeaponCoreApiLoaded)
				return;

			IncomingHomingProjectiles = APIs.WeaponCore.GetProjectilesLockedOn(_remoteControl as MyEntity).Item1;

		}

		public void CheckForPotentialHomingTargets() {

			if (!Data.AllowHomingWeaponMultiTargeting || !APIs.WeaponCoreApiLoaded)
				return;

			var timeSpan = MyAPIGateway.Session.GameDateTime - LastHomingTargetCheck;

			if (timeSpan.TotalSeconds < Data.MultiTargetCheckCooldown) {

				GetRandomHomingTarget = false;
				return;

			}

			LastHomingTargetCheck = MyAPIGateway.Session.GameDateTime;
			GetRandomHomingTarget = true;

		}

		public void RefreshMaxStaticRangeReferences() {

			double forward = 0;
			double backward = 0;
			double up = 0;
			double down = 0;
			double left = 0;
			double right = 0;

			foreach (var weapon in StaticWeapons) {

				if (!weapon.IsValid() && !weapon.IsActive())
					continue;

				var remMatrix = _remoteControl.WorldMatrix;
				var wepMatrix = weapon.Block().WorldMatrix;

				if (wepMatrix.Forward == remMatrix.Forward || VectorHelper.GetAngleBetweenDirections(remMatrix.Forward, wepMatrix.Forward) < 2) {

					weapon.SetDirection(Direction.Forward);

					if (weapon.MaxAmmoTrajectory() > forward)
						forward = weapon.MaxAmmoTrajectory();

				}

				if (wepMatrix.Forward == remMatrix.Backward || VectorHelper.GetAngleBetweenDirections(remMatrix.Backward, wepMatrix.Forward) < 2) {

					weapon.SetDirection(Direction.Backward);

					if (weapon.MaxAmmoTrajectory() > backward)
						backward = weapon.MaxAmmoTrajectory();

				}

				if (wepMatrix.Forward == remMatrix.Up || VectorHelper.GetAngleBetweenDirections(remMatrix.Up, wepMatrix.Forward) < 2) {

					weapon.SetDirection(Direction.Up);

					if (weapon.MaxAmmoTrajectory() > up)
						up = weapon.MaxAmmoTrajectory();

				}

				if (wepMatrix.Forward == remMatrix.Down || VectorHelper.GetAngleBetweenDirections(remMatrix.Down, wepMatrix.Forward) < 2) {

					weapon.SetDirection(Direction.Down);

					if (weapon.MaxAmmoTrajectory() > down)
						down = weapon.MaxAmmoTrajectory();

				}

				if (wepMatrix.Forward == remMatrix.Left || VectorHelper.GetAngleBetweenDirections(remMatrix.Left, wepMatrix.Forward) < 2) {

					weapon.SetDirection(Direction.Left);

					if (weapon.MaxAmmoTrajectory() > left)
						left = weapon.MaxAmmoTrajectory();

				}

				if (wepMatrix.Forward == remMatrix.Right || VectorHelper.GetAngleBetweenDirections(remMatrix.Right, wepMatrix.Forward) < 2) {

					weapon.SetDirection(Direction.Right);

					if (weapon.MaxAmmoTrajectory() > right)
						right = weapon.MaxAmmoTrajectory();

				}

			}

			MaxStaticRangesPerDirection[Direction.Forward] = forward;
			MaxStaticRangesPerDirection[Direction.Backward] = backward;
			MaxStaticRangesPerDirection[Direction.Up] = up;
			MaxStaticRangesPerDirection[Direction.Down] = down;
			MaxStaticRangesPerDirection[Direction.Left] = left;
			MaxStaticRangesPerDirection[Direction.Right] = right;

		}

		public double GetMaxRange(Direction direction) {

			double range = 0;
			MaxStaticRangesPerDirection.TryGetValue(direction, out range);

			return (range > Data.MaxStaticWeaponRange) ? Data.MaxStaticWeaponRange : range;

		}

		public void DeterminePrimaryStaticWeapon() {



			foreach (var weapon in StaticWeapons) {

				if (weapon.GetDirection() != Direction.Forward)
					continue;
			
			}
		
		}
		public int GetActiveGunCount() {

			int result = 0;

			foreach (var weapon in this.StaticWeapons) {

				if (weapon.IsValid() && weapon.IsActive() && weapon.HasAmmo())
					result++;

			}

			return result;

		}
		public int GetActiveTurretCount() {

			int result = 0;

			foreach (var weapon in this.Turrets) {

				if (weapon.IsValid() && weapon.IsActive() && weapon.HasAmmo())
					result++;

			}

			foreach (var weapon in this.TurretControllers) {

				if (weapon.IsValid() && weapon.IsActive() && weapon.HasAmmo())
					result++;

			}

			return result;

		}

		public int GetActiveWeaponCount() {

			int result = 0;

			foreach (var weapon in this.StaticWeapons) {

				if (weapon.IsValid() && weapon.IsActive() && weapon.HasAmmo())
					result++;

			}

			foreach (var weapon in this.Turrets) {

				if (weapon.IsValid() && weapon.IsActive() && weapon.HasAmmo())
					result++;

			}

			foreach (var weapon in this.TurretControllers) {

				if (weapon.IsValid() && weapon.IsActive() && weapon.HasAmmo())
					result++;

			}

			return result;

		}

		public void FireWeapons() {

			if (APIs.WeaponCoreApiLoaded) {

				try {

					if (_behavior.AutoPilot.Targeting.HasTarget()) {

						var existingFocus = APIs.WeaponCore.GetAiFocus(_behavior.RemoteControl?.SlimBlock?.CubeGrid as MyEntity);

						if (existingFocus == null && (MyAPIGateway.Session.GameDateTime - _lastAiFocusSetTime).TotalMilliseconds > 2000) {

							_lastAiFocusSetTime = MyAPIGateway.Session.GameDateTime;
							BehaviorLogger.Write("Setting WeaponCore AI Focus", BehaviorDebugEnum.Weapon);
							var gridEntity = _behavior.RemoteControl?.SlimBlock?.CubeGrid as MyEntity;
							var targetEntity = _behavior.AutoPilot.Targeting.Target?.GetParentEntity() as MyEntity;
							APIs.WeaponCore.SetAiFocus(gridEntity, targetEntity);

							if (APIs.WeaponCore.GetAiFocus(_behavior.RemoteControl?.SlimBlock?.CubeGrid as MyEntity) == null) {

								BehaviorLogger.Write("WeaponCore AI Focus Null After Assignment. Grid / Target null check: " + (gridEntity == null).ToString() + " / " + (targetEntity == null).ToString(), BehaviorDebugEnum.Weapon);
								BehaviorLogger.Write("Grid / Target Entities cast as IMyCubeGrid reporting null: " + (gridEntity as IMyCubeGrid == null).ToString() + " / " + (targetEntity as IMyCubeGrid == null).ToString(), BehaviorDebugEnum.Weapon);

							}

						}

					}

				} catch (Exception e) {

					BehaviorLogger.Write("Caught Exception In WeaponCore SetAiFocus", BehaviorDebugEnum.Error);
					BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Error);

				}

			}

			if (_pendingBarrageTrigger) {

				BehaviorLogger.Write("Pending Parallel For Barrage", BehaviorDebugEnum.Weapon);
				_pendingBarrageTrigger = false;
				FireBarrageWeapons();

			}

			ProcessWeaponLockOn();

			foreach (var weapon in StaticWeapons) {

				weapon.ToggleFire();

			}


		}

		public void FireBarrageWeapons() {

			if (_parallelWorkInProgress) {

				BehaviorLogger.Write("Pending Parallel For Barrage", BehaviorDebugEnum.Weapon);
				_pendingBarrageTrigger = true;
				return;

			}

			int weaponCount = this.StaticWeapons.Count;
			int iteratedWeapons = 0;

			if (weaponCount > 1) {

				while (true) {

					_barrageWeaponIndex++;
					iteratedWeapons++;

					if (_barrageWeaponIndex >= weaponCount) {

						_barrageWeaponIndex = 0;

					}

					var weapon = StaticWeapons[_barrageWeaponIndex];

					if (weapon.IsBarrageWeapon() && weapon.IsActive() && weapon.IsReadyToFire()) {

						weapon.FireOnce();
						break;

					}

					if (iteratedWeapons >= weaponCount)
						break;

				}

			} else if (weaponCount == 1) {

				var weapon = StaticWeapons[0];

				if (weapon.IsActive() && weapon.IsReadyToFire()) {

					weapon.FireOnce();

				}

			}

		}

		public bool HasWorkingWeapons() {

			foreach (var weapon in this.StaticWeapons) {

				if (weapon.IsValid() && weapon.IsActive() && weapon.HasAmmo())
					return true;

			}

			foreach (var weapon in this.Turrets) {

				if (weapon.IsValid() && weapon.IsActive() && weapon.HasAmmo())
					return true;

			}

			foreach (var weapon in this.TurretControllers) {

				if (weapon.IsValid() && weapon.IsActive() && weapon.HasAmmo())
					return true;

			}

			return false;

		}

		public long GetTurretTarget() {

			var resultList = new List<IMyEntity>();
			IMyEntity closestEntity = null;
			double closestEntityDistance = -1;

			foreach (var weapon in Turrets) {

				if (!weapon.IsValid() || !weapon.IsActive())
					continue;

				var entity = weapon.CurrentTarget();

				if (entity == null)
					continue;

				//TODO: Add some additional filters?

				double distance = Vector3D.Distance(weapon.Block().GetPosition(), entity.GetPosition());

				if (closestEntityDistance == -1 || distance < closestEntityDistance) {

					closestEntity = entity;
					closestEntityDistance = distance;

				}

			}

			return closestEntity != null ? closestEntity.EntityId : 0;

		}

		public void GetAmmoSpeedDetails(Direction direction, out double velocity, out double initialVelocity, out double acceleration) {

			velocity = 0;
			initialVelocity = 0;
			acceleration = 0;

			Dictionary<MyTuple<double, double, double>, int> commonVelocity = new Dictionary<MyTuple<double, double, double>, int>();

			foreach (var weapon in StaticWeapons) {

				if (weapon.GetDirection() != direction)
					continue;

				var ammoData = new MyTuple<double, double, double>(weapon.AmmoAcceleration(), weapon.AmmoInitialVelocity(), weapon.AmmoVelocity());

				if (commonVelocity.ContainsKey(ammoData)) {

					commonVelocity[ammoData]++;


				} else {

					commonVelocity.Add(ammoData, 1);

				}

			}

			int highestValue = 0;

			foreach (var data in commonVelocity.Keys) {

				int amount = 0;

				if (commonVelocity.TryGetValue(data, out amount)) {

					if (amount > highestValue) {

						highestValue = amount;
						acceleration = data.Item1;
						initialVelocity = data.Item2;
						velocity = data.Item3;

					}
				
				}
			
			}

		}

		public void PrepareWeaponLockOn() {

			if (!PendingTargetLockOn || _behavior.AutoPilot.Targeting.LockOnTarget == null || !_behavior.AutoPilot.Targeting.LockOnTarget.ActiveEntity())
				return;

			var grid = _behavior.AutoPilot.Targeting.LockOnTarget.GetEntity() as IMyCubeGrid;

			if (grid == null) {

				PendingTargetLockOn = false;
				return;

			}

			foreach (var weapon in AllWeapons) {

				if (weapon.CanLockOnGrid(grid)) {

					BehaviorLogger.Write(weapon.Block().CustomName + " Pending Lock On", BehaviorDebugEnum.Weapon);
					weapon.LockOnTarget = grid;
					weapon.PendingLockOn = true;
				
				}
			
			}

		}

		public void ProcessWeaponLockOn() {

			if (!PendingTargetLockOn)
				return;

			PendingTargetLockOn = false;

			foreach (var weapon in AllWeapons) {

				if (!weapon.PendingLockOn || weapon.LockOnTarget == null)
					continue;

				weapon.SetLockOnTarget();
				
			
			}
		
		}

	}

	public enum WeaponType {

		StaticNormal,
		TurretNormal,
		WeaponCoreStatic,
		WeaponCoreTurret,
		WeaponCoreSorterStatic,
		WeaponCoreSorterTurret,

	}

}
