using ModularEncountersSystems.API;
using ModularEncountersSystems.Helpers;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems.AutoPilot {
	public class CollisionResult {

		private CollisionSystem _collisionSystem;

		private DateTime _lastCollisionCheckTime;

		public Direction CollisionDirection;

		public Vector3D StartPosition;
		public Vector3D DirectionVector;
		public double Distance;
		public Vector3D EndPosition;
		public LineD Line;
		public RayD Ray;

		public bool CollisionIsWater;

		public bool CollisionDetected;
		public CollisionType Type;
		public bool CalculateTime;
		public double Time;

		public IMyEntity GridEntity;
		public IMyEntity VoxelEntity;
		public IMyEntity SafezoneEntity;
		public IMyEntity ShieldBlockEntity;
		public IMyEntity PlayerEntity;

		public Vector3D GridCoords;
		public Vector3D VoxelCoords;
		public Vector3D SafezoneCoords;
		public Vector3D ShieldCoords;
		public Vector3D PlayerCoords;

		public double GridDistance;
		public double VoxelDistance;
		public double SafezoneDistance;
		public double ShieldDistance;
		public double PlayerDistance;

		public TargetOwnerEnum GridOwner;
		public TargetOwnerEnum ShieldOwner;

		public TargetRelationEnum GridRelation;
		public TargetRelationEnum ShieldRelation;

		private List<MyLineSegmentOverlapResult<MyEntity>> _entityScanList;
		private List<MyVoxelBase> _voxelScanList;

		public CollisionResult(CollisionSystem collisionSystem, Direction direction) {

			CollisionDirection = direction;

			_collisionSystem = collisionSystem;
			_lastCollisionCheckTime = MyAPIGateway.Session.GameDateTime;
			_entityScanList = new List<MyLineSegmentOverlapResult<MyEntity>>();
			_voxelScanList = new List<MyVoxelBase>();

			StartPosition = Vector3D.Zero;
			DirectionVector = Vector3D.Zero;
			Distance = 0;
			ResetResults();

		}

		public void CalculateCollisions(Vector3D direction, double distance, bool calculateTime = false, bool useAsteroidAABB = false) {

			var timeSpan = MyAPIGateway.Session.GameDateTime - _lastCollisionCheckTime;

			if (timeSpan.TotalMilliseconds < 240)
				return;

			_lastCollisionCheckTime = MyAPIGateway.Session.GameDateTime;

			ResetResults();
			CalculateTime = calculateTime;
			DirectionVector = direction;
			Distance = GetDistanceFromWeapons(distance, CollisionDirection);
			StartPosition = _collisionSystem.Matrix.Translation;
			EndPosition = direction * distance + StartPosition;
			Line = new LineD(StartPosition, EndPosition);
			Ray = new RayD(StartPosition, direction);
			TargetIntersectionCheck();
			ShieldIntersectionCheck();
			VoxelCheckRequest(useAsteroidAABB);

		}

		public double GetDistanceFromWeapons(double defaultDistance, Direction direction) {

			double result = defaultDistance;
			var weaponDist = _collisionSystem.AutoPilot.Weapons.GetMaxRange(direction);

			if (defaultDistance > weaponDist)
				return defaultDistance;

			if (weaponDist > _collisionSystem.AutoPilot.Weapons.Data.MaxStaticWeaponRange && _collisionSystem.AutoPilot.Weapons.Data.MaxStaticWeaponRange > 0) {

				if (defaultDistance > _collisionSystem.AutoPilot.Weapons.Data.MaxStaticWeaponRange)
					return defaultDistance;
				else
					return _collisionSystem.AutoPilot.Weapons.Data.MaxStaticWeaponRange;

			}

			return weaponDist;

		}

		public void ResetResults() {

			CollisionDetected = false;
			Type = CollisionType.None;
			Time = 0;

			GridEntity = null;
			VoxelEntity = null;
			SafezoneEntity = null;
			ShieldBlockEntity = null;
			PlayerEntity = null;

			GridCoords = Vector3D.Zero;
			VoxelCoords = Vector3D.Zero;
			SafezoneCoords = Vector3D.Zero;
			ShieldCoords = Vector3D.Zero;
			PlayerCoords = Vector3D.Zero;

			GridDistance = -1;
			VoxelDistance = -1;
			SafezoneDistance = -1;
			ShieldDistance = -1;
			PlayerDistance = -1;

			GridOwner = TargetOwnerEnum.None;
			GridRelation = TargetRelationEnum.None;

		}

		public void DetermineClosestCollision() {

			double closestDist = -1;

			if (GridEntity != null) {

				if (Type == CollisionType.None || GridDistance < closestDist)
					Type = CollisionType.Grid;

			}

			if (VoxelEntity != null) {

				if (Type == CollisionType.None || VoxelDistance < closestDist)
					Type = CollisionType.Voxel;

			}

			if (SafezoneEntity != null) {

				if (Type == CollisionType.None || SafezoneDistance < closestDist)
					Type = CollisionType.Safezone;

			}

			if (ShieldBlockEntity != null) {

				if (Type == CollisionType.None || ShieldDistance < closestDist)
					Type = CollisionType.Shield;

			}

			if (PlayerEntity != null && _collisionSystem.AutoPilot.Data.AvoidPlayerCollisions) {

				if (Type == CollisionType.None || PlayerDistance < closestDist)
					Type = CollisionType.Player;

			}

			if (CalculateTime) {

				Time = GetCollisionDistance() / _collisionSystem.Velocity.Length();

			}

		}

		public bool HasTarget(double withinDistance = -1) {

			if (Type == CollisionType.None)
				return false;

			if (withinDistance > 0 && GetCollisionDistance() > withinDistance)
				return false;

			return true;

		}

		public bool CollisionImminent(int customTime = -1) {

			var timeTrigger = customTime != -1 ? customTime : _collisionSystem.CollisionTimeTrigger;
			return Type != CollisionType.None && CalculateTime && Time < timeTrigger && _collisionSystem.Velocity.Length() > _collisionSystem.MinimumSpeedForVelocityChecks;

		}

		public Vector3D GetCollisionCoords() {

			switch (Type) {

				case CollisionType.Grid:
					return GridCoords;
				case CollisionType.Voxel:
					return VoxelCoords;
				case CollisionType.Shield:
					return ShieldCoords;
				case CollisionType.Safezone:
					return SafezoneCoords;
				case CollisionType.Player:
					return PlayerCoords;

			}

			return Vector3D.Zero;

		}

		public double GetCollisionDistance() {

			switch (Type) {

				case CollisionType.Grid:
					return GridDistance;
				case CollisionType.Voxel:
					return VoxelDistance;
				case CollisionType.Shield:
					return ShieldDistance;
				case CollisionType.Safezone:
					return SafezoneDistance;
				case CollisionType.Player:
					return PlayerDistance;

			}

			return -1;

		}

		public IMyEntity GetCollisionEntity() {

			switch (Type) {

				case CollisionType.Grid:
					return GridEntity;
				case CollisionType.Voxel:
					return VoxelEntity;
				case CollisionType.Shield:
					return ShieldBlockEntity;
				case CollisionType.Safezone:
					return SafezoneEntity;
				case CollisionType.Player:
					return PlayerEntity;

			}

			return null;

		}

		public bool IsCollisionWithinDistance(double distance) {

			switch (Type) {

				case CollisionType.Grid:
					return distance <= GridDistance;
				case CollisionType.Voxel:
					return distance <= VoxelDistance;
				case CollisionType.Shield:
					return distance <= ShieldDistance;
				case CollisionType.Safezone:
					return distance <= SafezoneDistance;
				case CollisionType.Player:
					return distance <= PlayerDistance;

			}

			return false;

		}


		public double SafeZoneIntersectionCheck(MySafeZone zone) {

			var zoneEntity = zone as IMyEntity;

			if (zone.Shape == MySafeZoneShape.Sphere) {

				var newSphere = new BoundingSphereD(zoneEntity.PositionComp.WorldVolume.Center, zone.Radius);
				var result = Ray.Intersects(newSphere);

				if (result.HasValue == true) {

					return (double)result;

				}

			} else {

				var result = Ray.Intersects(zoneEntity.PositionComp.WorldAABB);

				if (result.HasValue == true) {

					return (double)result;

				}

			}

			return -1;

		}

		public void ShieldIntersectionCheck() {

			IMyEntity shieldEntity = null;

			if (APIs.ShieldsApiLoaded) {

				var api = APIs.Shields;
				var result = api.ClosestShieldInLine(Line, true);

				if (result.Item1.HasValue) {

					var dist = (double)result.Item1.Value;
					shieldEntity = result.Item2;

					ShieldCoords = StartPosition + Line.Direction * dist;
					ShieldDistance = dist;
					ShieldBlockEntity = shieldEntity;
					ShieldOwner = OwnershipHelper.GetOwnershipTypes((IMyTerminalBlock)shieldEntity);
					ShieldRelation = OwnershipHelper.GetTargetReputation(_collisionSystem.Owner, (IMyTerminalBlock)shieldEntity);

				}

			}

		}

		public void TargetIntersectionCheck() {

			_entityScanList.Clear();
			_voxelScanList.Clear();
			_entityScanList = new List<MyLineSegmentOverlapResult<MyEntity>>();
			MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref Line, _entityScanList);

			IMyCubeGrid closestGrid = null;
			double closestGridDistance = -1;

			MySafeZone closestZone = null;
			double closestZoneDistance = -1;

			IMyCharacter closestCharacter = null;
			double closestCharacterDistance = -1;

			foreach (var item in _entityScanList) {

				var targetGrid = item.Element as IMyCubeGrid;
				var targetZone = item.Element as MySafeZone;
				var targetVoxel = item.Element as MyVoxelBase;
				var targetPlayer = item.Element as IMyCharacter;

				if (targetGrid != null) {

					if (targetGrid == _collisionSystem.RemoteControl.SlimBlock.CubeGrid || _collisionSystem.RemoteControl.SlimBlock.CubeGrid.IsSameConstructAs(targetGrid)) {

						continue;

					}

					if (closestGrid == null || (closestGrid != null && closestGrid.Physics != null && item.Distance < closestGridDistance)) {

						closestGrid = targetGrid;
						closestGridDistance = item.Distance;

					}

				}

				if (targetZone != null) {

					if (closestZone == null || (closestZone != null && item.Distance < closestZoneDistance)) {

						closestZone = targetZone;
						closestZoneDistance = item.Distance;

					}

				}

				if (targetVoxel != null) {

					_voxelScanList.Add(targetVoxel);

				}

				if (targetPlayer != null) {

					if (closestCharacter == null || (closestCharacter != null && item.Distance < closestCharacterDistance)) {

						closestCharacter = targetPlayer;
						closestCharacterDistance = item.Distance;

					}
				
				}

			}

			if (closestGrid != null) {

				double minDist = 0;
				double maxDist = 0;
				bool boxCheckResult = closestGrid.PositionComp.WorldAABB.Intersect(ref Ray, out minDist, out maxDist);

				Vector3D startBox = boxCheckResult ? (minDist - 5) * DirectionVector + StartPosition : StartPosition;
				Vector3D endBox = boxCheckResult ? (maxDist + 5) * DirectionVector + StartPosition : EndPosition;

				var blockPos = closestGrid.RayCastBlocks(startBox, endBox);

				if (!blockPos.HasValue) {

					return;

				}

				IMySlimBlock slimBlock = closestGrid.GetCubeBlock(blockPos.Value);

				if (slimBlock == null) {

					return;

				}

				Vector3D blockPosition = Vector3D.Zero;
				slimBlock.ComputeWorldCenter(out blockPosition);


				GridCoords = blockPosition;
				GridDistance = Vector3D.Distance(blockPosition, StartPosition);
				GridEntity = closestGrid;
				GridOwner = OwnershipHelper.GetOwnershipTypes(closestGrid, false);
				GridRelation = OwnershipHelper.GetTargetReputation(_collisionSystem.Owner, closestGrid, false);

			}

			if (closestZone != null) {

				SafezoneEntity = closestZone;
				SafezoneCoords = closestZoneDistance * DirectionVector + StartPosition;
				SafezoneDistance = closestZoneDistance;

			}

			if (closestCharacter != null) {

				PlayerEntity = closestCharacter;
				PlayerCoords = PlayerEntity.PositionComp.WorldAABB.Center;
				PlayerDistance = closestCharacterDistance;

			}

		}

		public void VoxelCheckRequest(bool useAsteroidAABB = false) {

			if (_collisionSystem.AutoPilot.InGravity()) {

				var stepList = _collisionSystem.AutoPilot.GetPlanetPathSteps(StartPosition, DirectionVector, Distance, true);
				var planet = _collisionSystem.AutoPilot.GetCurrentPlanet();
				var planetCenter = planet.Center();
				int index = -1;
				bool underwater = false;

				for (int i = 0; i < stepList.Count; i++) {

					var step = stepList[i];
					var stepToCore = Vector3D.Distance(planetCenter, step);
					var stepSurfaceToCore = Vector3D.Distance(planetCenter, planet.SurfaceCoordsAtPosition(step));

					if (stepToCore < stepSurfaceToCore) {

						//Logger.Write("Planet Voxel Found: ", BehaviorDebugEnum.Collision);
						index = i == 0 ? 0 : i - 1;
						break;

					}

				}

				if (index >= 0) {

					CollisionIsWater = planet.IsPositionUnderwater(stepList[index]);
					VoxelEntity = planet.Planet;
					VoxelCoords = stepList[index];
					VoxelDistance = Vector3D.Distance(StartPosition, stepList[index]);

				}


			} else {

				MyVoxelBase closestVoxel = null;
				double closestDistance = -1;

				/*
				//Temp Debug Code Start
				_voxelScanList.Clear();
				var startCorner = StartPosition + new Vector3D(-2000, -2000, -2000);
				var endCorner = StartPosition + new Vector3D(2000, 2000, 2000);
				var box = new BoundingBoxD(startCorner, endCorner);
				MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, _voxelScanList);
				*/

				foreach (var voxel in _voxelScanList) {

					if (voxel == null || voxel.MarkedForClose)
						continue;

					var planet = voxel as MyPlanet;

					if (planet != null)
						continue;

					if (!useAsteroidAABB) {

						double minDist = 0;
						double maxDist = 0;
						bool boxCheckResult = voxel.PositionComp.WorldAABB.Intersect(ref Ray, out minDist, out maxDist);

						Vector3D startBox = boxCheckResult ? (minDist - 5) * DirectionVector + StartPosition : StartPosition;
						Vector3D endBox = boxCheckResult ? (maxDist + 5) * DirectionVector + StartPosition : EndPosition;
						Vector3D hitPosition = Vector3D.Zero;
						LineD voxelLine = new LineD(startBox, endBox);
						bool gotHit = CollisionHelper.VoxelIntersectionCheck(voxel, startBox, endBox, 25, ref hitPosition);

						if (gotHit) {

							var hitDist = Vector3D.Distance((Vector3D)hitPosition, StartPosition);

							if (closestVoxel == null || closestVoxel != null && hitDist < closestDistance) {

								closestVoxel = voxel;
								closestDistance = hitDist;

							}

						}

					} else {

						if (voxel.PositionComp.WorldAABB.Contains(StartPosition) == ContainmentType.Contains) {

							var hitDist = Vector3D.Distance(StartPosition, StartPosition + DirectionVector);

							if (closestVoxel == null || closestVoxel != null && hitDist < closestDistance) {

								closestVoxel = voxel;
								closestDistance = hitDist;

							}

						} else {

							double hitDist = 0;
							voxel.PositionComp.WorldAABB.Intersects(ref Line, out hitDist);

							if (closestVoxel == null || closestVoxel != null && hitDist < closestDistance) {

								closestVoxel = voxel;
								closestDistance = hitDist;

							}

						}

					}

				}

				if (closestDistance == -1) {

					return;

				}

				VoxelEntity = closestVoxel;
				VoxelDistance = closestDistance;
				VoxelCoords = closestDistance * DirectionVector + StartPosition;


			}

		}

	}

}
