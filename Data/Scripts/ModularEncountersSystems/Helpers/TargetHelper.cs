using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Weapons;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Helpers {

	public struct CollisionCheckResult {

		public bool HasTarget;
		public bool CollisionImminent;
		public Vector3D Coords;
		public double Distance;
		public double Time;
		public CollisionDetectType Type;
		public IMyEntity Entity;

		public CollisionCheckResult(bool empty) {

			HasTarget = false;
			CollisionImminent = false;
			Coords = Vector3D.Zero;
			Distance = 0;
			Time = 0;
			Type = CollisionDetectType.None;
			Entity = null;

		}

		public CollisionCheckResult(bool target, bool collisionImminent, Vector3D coords, double distance, double time, CollisionDetectType type, IMyEntity entity) {

			HasTarget = target;
			CollisionImminent = collisionImminent;
			Coords = coords;
			Distance = distance;
			Time = time;
			Type = type;
			Entity = entity;

		}

	}

	public static class TargetHelper{

		public static List<MyDefinitionId> ShieldBlockIDs = new List<MyDefinitionId>();
		
		public static Random Rnd = new Random();

		

		//EvaluateTargetWeaponsRange
		public static float EvaluateTargetTurretRange(IMyCubeGrid cubeGrid){
			
			float furthestWeaponDistance = 0;
			
			try{
				
				var gts = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(cubeGrid);
				var blockList = new List<IMyLargeTurretBase>();
				gts.GetBlocksOfType<IMyLargeTurretBase>(blockList);

				foreach(var turret in blockList){
					
					if(turret.IsFunctional == false){
						
						continue;
						
					}
					
					if(turret.GetInventory().Empty() == true){
						
						continue;
						
					}
					
					float range = turret.Range;
					
					if(range > furthestWeaponDistance){
						
						furthestWeaponDistance = range;
						
					}
					
				}
				
			}catch(Exception exc){
				
				BehaviorLogger.Write("Caught Error in EvaluateTargetWeaponsRange Method.", BehaviorDebugEnum.Error, true);
				return 0;
				
			}
			
			return furthestWeaponDistance;
			
		}

		public static List<IMyEntity> FilterBlocksByFamily(List<IMyEntity> entityList, BlockTargetTypes family, bool replaceResultWithDecoys = false) {

			var blockList = new List<IMyEntity>();
			var decoyList = new List<IMyEntity>();

			if(family.HasFlag(BlockTargetTypes.All) == true) {

				return entityList;

			}

			for(int i = entityList.Count - 1; i >= 0; i--) {

				var block = entityList[i] as IMyTerminalBlock;

				if(block == null) {

					continue;

				}

				//Decoys
				if(family.HasFlag(BlockTargetTypes.Decoys) == true) {

					if(block as IMyDecoy != null) {

						blockList.Add(block);
						decoyList.Add(block);
						continue;

					}

				}

				//Shields
				if(family.HasFlag(BlockTargetTypes.Shields) == true) {

					if(ShieldBlockIDs.Contains(block.SlimBlock.BlockDefinition.Id) == true) {

						blockList.Add(block);
						continue;

					}

				}

				//Containers
				if(family.HasFlag(BlockTargetTypes.Containers) == true) {

					if(block as IMyCargoContainer != null) {

						blockList.Add(block);
						continue;

					}

				}

				//GravityBlocks
				if(family.HasFlag(BlockTargetTypes.GravityBlocks) == true) {

					if(block as IMyGravityGeneratorBase != null || block as IMyArtificialMassBlock != null) {

						blockList.Add(block);
						continue;

					}

				}

				//Guns
				if(family.HasFlag(BlockTargetTypes.Guns) == true) {

					if(block as IMyUserControllableGun != null && block as IMyLargeTurretBase == null) {

						blockList.Add(block);
						continue;

					}

				}

				//JumpDrive
				if(family.HasFlag(BlockTargetTypes.JumpDrive) == true) {

					if(block as IMyJumpDrive != null) {

						blockList.Add(block);
						continue;

					}

				}

				//Power
				if(family.HasFlag(BlockTargetTypes.Power) == true) {

					if(block as IMyPowerProducer != null) {

						blockList.Add(block);
						continue;

					}

				}

				//Production
				if(family.HasFlag(BlockTargetTypes.Production) == true) {

					if(block as IMyProductionBlock != null || block as IMyGasGenerator != null) {

						blockList.Add(block);
						continue;

					}

				}

				//Propulsion
				if(family.HasFlag(BlockTargetTypes.Propulsion) == true) {

					if(block as IMyThrust != null || block as IMyGyro != null) {

						blockList.Add(block);
						continue;

					}

				}

				//ShipControllers
				if(family.HasFlag(BlockTargetTypes.ShipControllers) == true) {

					if(block as IMyShipController != null) {

						blockList.Add(block);
						continue;

					}

				}

				//Tools
				if(family.HasFlag(BlockTargetTypes.Tools) == true) {

					if(block as IMyShipToolBase != null) {

						blockList.Add(block);
						continue;

					}

				}

				//Turrets
				if(family.HasFlag(BlockTargetTypes.Turrets) == true) {

					if(block as IMyLargeTurretBase != null) {

						blockList.Add(block);
						continue;

					}

				}

				//Communications
				if(family.HasFlag(BlockTargetTypes.Communications) == true) {

					if(block as IMyRadioAntenna != null || block as IMyLaserAntenna != null) {

						blockList.Add(block);
						continue;

					}

				}

			}

			if(replaceResultWithDecoys == true && decoyList.Count > 0) {

				return decoyList;

			} else {
				
				return blockList;

			}

		}

		//GetAllBlocks
		

		public static PlayerEntity GetClosestPlayerWithReputation(Vector3D coords, long factionId, TriggerProfile control) {

			PlayerEntity closestPlayer = null;
			double closestPlayerDistance = 0;

			if (control == null)
				return null;

			foreach(var playerEnt in PlayerManager.Players) {

				if (playerEnt == null || !playerEnt.ActiveEntity())
					continue;

				var player = playerEnt.Player;

				if(player?.Controller?.ControlledEntity?.Entity == null || player.IsBot == true) {

					continue;

				}

				var playerRep = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(player.IdentityId, factionId);
				var distance = Vector3D.Distance(player.GetPosition(), coords);

				if (playerRep < control.MinPlayerReputation || playerRep > control.MaxPlayerReputation) {

					if (control.AllPlayersMustMatchReputation) {

						if (distance <= control.CustomReputationRangeCheck) {

							return null;
						
						}
					
					}

					continue;

				}

				if(closestPlayer == null) {

					closestPlayer = playerEnt;
					closestPlayerDistance = distance;
					continue;

				}

				if(distance < closestPlayerDistance) {

					closestPlayer = playerEnt;
					closestPlayerDistance = distance;

				}

			}

			return closestPlayer;

		}

		public static IMyEntity GetEntityAtDistance(Vector3D coords, List<IMyEntity> entityList, TargetSortEnum distanceEnum) {

			IMyEntity result = null;
			double closestDistance = -1;

			if(distanceEnum == TargetSortEnum.Random && entityList.Count > 0) {

				return entityList[Rnd.Next(0, entityList.Count)];

			}

			if(distanceEnum == TargetSortEnum.ClosestDistance) {

				foreach(var entity in entityList) {

					var distance = Vector3D.Distance(coords, entity.GetPosition());

					if(closestDistance == -1 || distance < closestDistance) {

						result = entity;
						closestDistance = distance;

					}

				}

			}

			if(distanceEnum == TargetSortEnum.FurthestDistance) {

				foreach(var entity in entityList) {

					var distance = Vector3D.Distance(coords, entity.GetPosition());

					if(closestDistance == -1 || distance > closestDistance) {

						result = entity;
						closestDistance = distance;

					}

				}

			}

			return result;

		}

		public static List<MySafeZone> GetSafeZones(){
			
			var entities = new HashSet<IMyEntity>();
			MyAPIGateway.Entities.GetEntities(entities);
			var safezoneList = new List<MySafeZone>();
			
			foreach(var zoneEntity in entities){
				
				var safezone = zoneEntity as MySafeZone;
				
				if(safezone == null){
					
					continue;
					
				}
				
				safezoneList.Add(safezone);
				
			}
			
			return safezoneList;
			
		}

		public static IMyEntity GetTargetFromId(long entityId, out TargetTypeEnum targetType) {

			targetType = TargetTypeEnum.None;
			IMyEntity result = null;

			if (MyAPIGateway.Entities.TryGetEntityById(entityId, out result) == false)
				return result;

			if (result as IMyCubeBlock != null) {

				targetType = TargetTypeEnum.Block;

			}

			if (result as IMyCubeGrid != null) {

				targetType = TargetTypeEnum.Grid;

			}

			if (result as IMyEngineerToolBase != null) {

				if (MyAPIGateway.Entities.TryGetEntityById((result as IMyEngineerToolBase).OwnerId, out result) == false)
					return null;

				targetType = TargetTypeEnum.Player;
			}

			if (result as IMyGunBaseUser != null) {

				result = (result as IMyGunBaseUser).Owner;
				targetType = TargetTypeEnum.Player;
			}

			return result;
		}

		public static IMyEntity GetAttackerParentEntity(long entityId, IMyCubeGrid cubeGrid) {

			IMyEntity entity = null;

			if (MyAPIGateway.Entities.TryGetEntityById(entityId, out entity)) {

				var parentEnt = entity.GetTopMostParent();

				if (parentEnt != null) {

					//Logger.Write("Damager Parent Entity Valid", BehaviorDebugEnum.General);
					var gridGroup = MyAPIGateway.GridGroups.GetGroup(cubeGrid, GridLinkTypeEnum.Physical);
					bool isSameGridConstrust = false;

					foreach (var grid in gridGroup) {

						if (grid.EntityId == parentEnt.EntityId) {

							//Logger.Write("Damager Parent Entity Was Same Grid", BehaviorDebugEnum.General);
							isSameGridConstrust = true;
							break;

						}

					}

					if (!isSameGridConstrust) {

						return parentEnt;

					}

				}

			}

			return null;
		
		}
		
		public static Vector2 GetTargetGridPower(IMyCubeGrid cubeGrid){
			
			Vector2 result = Vector2.Zero;
			float currentPower = 0;
			float maxPower = 0;
			
			try{
				
				var gts = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(cubeGrid);
				List<IMyPowerProducer> blockList = new List<IMyPowerProducer>();
				gts.GetBlocksOfType<IMyPowerProducer>(blockList);
				
				foreach(var block in blockList){
					
					if(block.IsFunctional == true || block.IsWorking){
						
						currentPower += block.CurrentOutput;
						maxPower += block.MaxOutput;
						
					}
				
				}
				
			}catch(Exception exc){
				
				result.X = -1;
				result.Y = -1;
				return result;
				
			}
			
			result.X = currentPower;
			result.Y = maxPower;
			return result;
			
		}

		//GetTargetShipSystem
		public static void GetFilteredBlockLists(IMyCubeGrid targetGrid, BlockTargetTypes systemTarget, out List<IMyTerminalBlock> targetBlocksList, out List<IMyTerminalBlock> decoyList){
			
			decoyList = new List<IMyTerminalBlock>();
			targetBlocksList = new List<IMyTerminalBlock>();
			
			try{
				
				var gts = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(targetGrid);
				List<IMyTerminalBlock> blockList = new List<IMyTerminalBlock>();
				gts.GetBlocksOfType<IMyTerminalBlock>(blockList);

				foreach(var block in blockList){
					
					if(block.IsFunctional == false){
						
						continue;
						
					}
					
					if(block as IMyDecoy != null){
						
						decoyList.Add(block);
						
						if(systemTarget == BlockTargetTypes.Decoys){
							
							targetBlocksList.Add(block);
							
						}

						continue;
						
					}
					
					if(systemTarget.HasFlag(BlockTargetTypes.All)){
						
						targetBlocksList.Add(block);
						continue;
						
					}
					
					if(systemTarget.HasFlag(BlockTargetTypes.Communications)){
						
						if(block as IMyLaserAntenna != null){
							
							targetBlocksList.Add(block);
							continue;
							
						}
						
						if(block as IMyBeacon != null){
							
							targetBlocksList.Add(block);
							continue;
							
						}

						if(block as IMyRadioAntenna != null){
							
							targetBlocksList.Add(block);
							continue;
							
						}
						
					}
					
					if(systemTarget.HasFlag(BlockTargetTypes.Containers)){
						
						if(block as IMyCargoContainer != null){
							
							targetBlocksList.Add(block);
							continue;
							
						}
						
					}
					
					if(systemTarget.HasFlag(BlockTargetTypes.GravityBlocks)){
						
						if(block as IMyGravityGeneratorBase != null || block as IMyArtificialMassBlock != null){
							
							targetBlocksList.Add(block);
							continue;
							
						}
						
					}
					
					if(systemTarget.HasFlag(BlockTargetTypes.Guns)){
						
						if(block as IMyUserControllableGun != null && block as IMyLargeTurretBase == null){
							
							targetBlocksList.Add(block);
							continue;
							
						}
						
					}
					
					if(systemTarget.HasFlag(BlockTargetTypes.JumpDrive)){
						
						if(block as IMyJumpDrive != null){
							
							targetBlocksList.Add(block);
							continue;
							
						}
						
					}
					
					if(systemTarget.HasFlag(BlockTargetTypes.Power)){
						
						if(block as IMyPowerProducer != null){
							
							targetBlocksList.Add(block);
							continue;
							
						}
						
					}
					
					if(systemTarget.HasFlag(BlockTargetTypes.Production)){
						
						if(block as IMyProductionBlock != null){
							
							targetBlocksList.Add(block);
							continue;
							
						}
						
						if(block as IMyGasGenerator != null){
							
							targetBlocksList.Add(block);
							continue;
							
						}
						
					}
					
					if(systemTarget.HasFlag(BlockTargetTypes.Propulsion)){
						
						if(block as IMyThrust != null){
							
							targetBlocksList.Add(block);
							continue;
							
						}
						
					}
					
					if(systemTarget.HasFlag(BlockTargetTypes.Shields)){
						
						if(ShieldBlockIDs.Contains(block.SlimBlock.BlockDefinition.Id) == true){
							
							targetBlocksList.Add(block);
							continue;
							
						}
						
					}
					
					if(systemTarget.HasFlag(BlockTargetTypes.ShipControllers)){
						
						if(block as IMyShipController != null){
							
							targetBlocksList.Add(block);
							continue;
							
						}
						
					}
					
					if(systemTarget.HasFlag(BlockTargetTypes.Tools)){
						
						if(block as IMyShipToolBase != null){
							
							targetBlocksList.Add(block);
							continue;
							
						}
						
					}
					
					if(systemTarget.HasFlag(BlockTargetTypes.Turrets)){
						
						if(block as IMyLargeTurretBase != null){
							
							targetBlocksList.Add(block);
							continue;
							
						}
						
					}
					
				}

			}catch(Exception exc){
				
				
				
			}

		}

		public static List<IMyPlayer> GetPlayersWithinDistance(Vector3D coords, double radius) {

			var playerList = new List<IMyPlayer>();
			MyAPIGateway.Players.GetPlayers(playerList, x => x.IsBot == false && Vector3D.Distance(coords, x.GetPosition()) < radius);
			return playerList;
			
		}
		//Overload didn't work for me somehow -cpt
		public static List<IMyPlayer> GetPlayersWithinRange(Vector3D coords, double minDistance, double maxDistance)
		{

			var playerList = new List<IMyPlayer>();

			MyAPIGateway.Players.GetPlayers(playerList, x => x.IsBot == false && Vector3D.Distance(coords, x.GetPosition()) < maxDistance && Vector3D.Distance(coords, x.GetPosition()) > minDistance);
			return playerList;

		}

		//IsGridPowered
		public static bool IsGridPowered(IMyCubeGrid cubeGrid) {

			if(cubeGrid == null) {

				return false;

			}

			var powerId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Electricity");
			var gts = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(cubeGrid);
			List<IMyTerminalBlock> blockList = new List<IMyTerminalBlock>();
			gts.GetBlocksOfType<IMyTerminalBlock>(blockList);

			foreach(var block in blockList) {

				var sink = block.Components.Get<MyResourceSinkComponent>();

				if(sink == null) {

					continue;

				}

				return sink.IsPowerAvailable(powerId, 0.001f);

			}

			return false;

		}

		//IsHumanControllingTarget
		public static bool IsHumanControllingTarget(IMyCubeGrid cubeGrid){

			if(cubeGrid == null) {

				return false;

			}

			var gts = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(cubeGrid);
			List<IMyShipController> blockList = new List<IMyShipController>();
			gts.GetBlocksOfType<IMyShipController>(blockList);
			
			foreach(var cockpit in blockList){
				
				if(cockpit.Pilot != null && cockpit.CanControlShip == true){
					
					return true;
					
				}
				
			}
			
			return false;
			
		}

		public static bool IsPositionInGravity(Vector3D position, MyPlanet planet) {

			if(planet == null) {

				return false;

			}

			var planetEntity = planet as IMyEntity;
			var gravityProvider = planetEntity.Components.Get<MyGravityProviderComponent>();

			if(gravityProvider == null) {

				return false;

			}

			return gravityProvider.IsPositionInRange(position);

		}

		//IsPositionInSafeZone
		public static bool IsPositionInSafeZone(Vector3D position){

			var zones = GetSafeZones();
			
			foreach(var safezone in zones){
				
				var zoneEntity = safezone as IMyEntity;
				
				if(zoneEntity == null){
					
					continue;
					
				}

				if(safezone.Enabled == false) {

					continue;

				}

				var checkPosition = position;
				bool inZone = false;
				
				if (safezone.Shape == MySafeZoneShape.Sphere){

					if(Vector3D.Distance(zoneEntity.PositionComp.WorldVolume.Center, position) < safezone.Radius) {

						inZone = true;

					}

				}else{
					
					MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(zoneEntity.PositionComp.LocalAABB, zoneEntity.PositionComp.WorldMatrix);
					inZone = myOrientedBoundingBoxD.Contains(ref checkPosition);
				
				}
				
				if(inZone == true){
					
					return true;
					
				}
				
			}
			
			return false;
			
		}

		//IsTargetBroadcasting
		public static bool IsTargetBroadcasting(IMyCubeGrid cubeGrid, IMyRemoteControl sourceBlock, bool checkAntennas, bool checkBeacons){
			
			var gts = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(cubeGrid);
			
			if(checkAntennas == true){
				
				List<IMyRadioAntenna> antennaList = new List<IMyRadioAntenna>();
				gts.GetBlocksOfType<IMyRadioAntenna>(antennaList);
				
				foreach(var antenna in antennaList){
					
					if(antenna.IsWorking == true && antenna.IsFunctional == true && antenna.IsBroadcasting == true){
						
						var distToNPC = (float)Vector3D.Distance(sourceBlock.GetPosition(), antenna.GetPosition());
						
						if(antenna.Radius >= distToNPC){
							
							return true;
							
						}
						
					}
					
				}
				
			}
			
			if(checkBeacons == true){
				
				List<IMyBeacon> beaconList = new List<IMyBeacon>();
				gts.GetBlocksOfType<IMyBeacon>(beaconList);
				
				foreach(var beacon in beaconList){
					
					if(beacon.IsWorking == true && beacon.IsFunctional == true){
						
						var distToNPC = (float)Vector3D.Distance(sourceBlock.GetPosition(), beacon.GetPosition());
						
						if(beacon.Radius >= distToNPC){
							
							return true;
							
						}
						
					}
					
				}
				
			}
			
			return false;
			
		}

		//IsTargetFaction
		public static bool IsTargetFaction(long myOwnerId, IMyEntity targetEntity) {

			var myFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(myOwnerId);

			if(myFaction == null) {

				return false;

			}

			long targetId = 0;

			if(targetEntity as IMyCubeGrid != null) {

				var cubeGrid = targetEntity as IMyCubeGrid;

				if(cubeGrid.BigOwners.Count > 0) {

					targetId = cubeGrid.BigOwners[0];

				}

			} else if(targetEntity as IMyCubeBlock != null) {

				var cubeBlock = targetEntity as IMyCubeBlock;
				targetId = cubeBlock.OwnerId;

			}

			if(targetId == 0) {

				return false;

			}

			var otherFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(targetId);

			if(otherFaction == null) {

				return false;

			}

			if(myFaction.FactionId == otherFaction.FactionId) {

				return true;

			}

			return false;

		}

		//IsTargetNPC
		public static bool IsTargetNPC(IMyEntity targetEntity) {

			long targetId = 0;

			if(targetEntity as IMyCubeGrid != null) {

				var cubeGrid = targetEntity as IMyCubeGrid;

				if(cubeGrid.BigOwners.Count > 0) {

					targetId = cubeGrid.BigOwners[0];

				}

			} else if(targetEntity as IMyCubeBlock != null) {

				var cubeBlock = targetEntity as IMyCubeBlock;
				targetId = cubeBlock.OwnerId;

			}

			if(targetId == 0) {

				return false;

			}

			var npcSteamId = MyAPIGateway.Players.TryGetSteamId(targetId);

			if(npcSteamId == 0) {

				return true;

			}

			return false;

		}

		//IsTargetNeutral
		public static bool IsTargetNeutral(long myOwnerId, IMyEntity targetEntity) {

			var myFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(myOwnerId);

			if(myFaction == null) {

				return false;

			}

			long targetId = 0;

			if(targetEntity as IMyCubeGrid != null) {

				var cubeGrid = targetEntity as IMyCubeGrid;

				if(cubeGrid.BigOwners.Count > 0) {

					targetId = cubeGrid.BigOwners[0];

				}

			} else if(targetEntity as IMyCubeBlock != null) {

				var cubeBlock = targetEntity as IMyCubeBlock;
				targetId = cubeBlock.OwnerId;

			}

			if(targetId == 0) {

				return false;

			}

			var otherFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(targetId);

			if(otherFaction == null) {

				return false;

			}

			if(MyAPIGateway.Session.Factions.AreFactionsEnemies(myFaction.FactionId, otherFaction.FactionId) == false) {

				return true;

			}

			return false;

		}

		//IsTargetOwned
		public static bool IsTargetOwned(long myOwnerId, IMyEntity targetEntity) {

			long targetId = 0;

			if(targetEntity as IMyCubeGrid != null) {

				var cubeGrid = targetEntity as IMyCubeGrid;

				if(cubeGrid.BigOwners.Count > 0) {

					targetId = cubeGrid.BigOwners[0];

				}

			} else if(targetEntity as IMyCubeBlock != null) {

				var cubeBlock = targetEntity as IMyCubeBlock;
				targetId = cubeBlock.OwnerId;

			}

			if(targetId == 0) {

				return false;

			}

			if(targetId == myOwnerId) {

				return true;

			}

			return false;

		}

		//IsTargetPlayer
		public static bool IsTargetPlayer(IMyEntity targetEntity) {

			long targetId = 0;

			if(targetEntity as IMyCubeGrid != null) {

				var cubeGrid = targetEntity as IMyCubeGrid;

				if(cubeGrid.BigOwners.Count > 0) {

					targetId = cubeGrid.BigOwners[0];

				}

			} else if(targetEntity as IMyCubeBlock != null) {

				var cubeBlock = targetEntity as IMyCubeBlock;
				targetId = cubeBlock.OwnerId;

			}

			if(targetId == 0) {

				return false;

			}

			var npcSteamId = MyAPIGateway.Players.TryGetSteamId(targetId);

			if(npcSteamId != 0) {

				return true;

			}

			return false;

		}

		//IsTargetUnowned
		public static bool IsTargetUnowned(IMyEntity targetEntity) {

			long targetId = 0;

			if(targetEntity as IMyCubeGrid != null) {

				var cubeGrid = targetEntity as IMyCubeGrid;

				if(cubeGrid.BigOwners.Count > 0) {

					targetId = cubeGrid.BigOwners[0];

				}

			} else if(targetEntity as IMyCubeBlock != null) {

				var cubeBlock = targetEntity as IMyCubeBlock;
				targetId = cubeBlock.OwnerId;

			}

			if(targetId == 0) {

				return true;

			}

			return false;

		}

		public static bool IsTargetUsingDefenseShield(IMyCubeGrid targetGrid){

			if(APIs.ShieldsApiLoaded){

				var api = APIs.Shields;
				
				if(api.GridHasShield(targetGrid) == true && api.GridShieldOnline(targetGrid) == true){
					
					return true;
					
				}
				
			}
			
			return false;
			
		}

		public static IMyPlayer MatchPlayerToEntity(IMyEntity entity) {

			if (entity == null)
				return null;

			return MyAPIGateway.Players.GetPlayerControllingEntity(entity);

		}
		
		
		
		
		
		
		
		public static Vector3D VoxelIntersectionCheck(Vector3D startScan, Vector3D scanDirection, double distance, out IMyEntity voxelEntity){

			voxelEntity = null;
			var voxelFrom = startScan;
			var voxelTo = scanDirection * distance + voxelFrom;
			var line = new LineD(voxelFrom, voxelTo);
			
			List<IMyVoxelBase> nearbyVoxels = new List<IMyVoxelBase>();
			MyAPIGateway.Session.VoxelMaps.GetInstances(nearbyVoxels);
			Vector3D closestDistance = Vector3D.Zero;
			
			foreach(var voxel in nearbyVoxels){
				
				if(Vector3D.Distance(voxel.GetPosition(), voxelFrom) > 120000){
					
					continue;
					
				}
				
				var voxelBase = voxel as MyVoxelBase;
				Vector3D? nearestHit = null;
				
				if(voxelBase.GetIntersectionWithLine(ref line, out nearestHit) == true){
					
					if(nearestHit.HasValue == true){
						
						if(closestDistance == Vector3D.Zero){

							voxelEntity = voxelBase;
							closestDistance = (Vector3D)nearestHit;
							continue;
							
						}
						
						if(Vector3D.Distance(voxelFrom, (Vector3D)nearestHit) < Vector3D.Distance(voxelFrom, closestDistance)){

							voxelEntity = voxelBase;
							closestDistance = (Vector3D)nearestHit;
							
						}
						
					}
					
				}

			}
			
			return closestDistance;
			
		}

	}
	
}
