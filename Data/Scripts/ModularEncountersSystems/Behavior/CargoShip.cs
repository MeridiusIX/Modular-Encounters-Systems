using Sandbox.ModAPI;
using VRageMath;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using System.Collections.Generic;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.World;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Configuration;
using System.Text;

namespace ModularEncountersSystems.Behavior {

	public class CargoShip : IBehaviorSubClass{

		public List<Vector3D> CustomWaypoints;
		private bool _waypointIsDespawn = true;

		private Vector3D _lastCoords = Vector3D.Zero;
		private IBehavior _behavior;

		public bool GetSpeedFromSpawnGroup;
		public bool UsePauseAutopilotFromSpawnGroup;

		private bool _playerInRange;
		private bool _stoppedForPlayer;
		private double _stoppingRange;
		public BehaviorSubclass SubClass { get { return _subClass; } set { _subClass = value; } }
		private BehaviorSubclass _subClass;

		public string DefaultWeaponProfile { get { return _defaultWeaponProfile; } }
		private string _defaultWeaponProfile;

		private EncounterWaypoint _cargoShipWaypoint { 
			
			get {

				if (_behavior.AutoPilot.State.CargoShipWaypoints.Count > 0) {

					if (_waypointIsDespawn) {

						BehaviorLogger.Write("CargoShip Switching To A Non-Despawn Waypoint", BehaviorDebugEnum.BehaviorSpecific);
						_waypointIsDespawn = false;
						_behavior.BehaviorTriggerC = true;

					}

					return _behavior.AutoPilot.State.CargoShipWaypoints[0];

				}

				if (!_waypointIsDespawn) {

					BehaviorLogger.Write("CargoShip Switching To A Despawn Waypoint", BehaviorDebugEnum.BehaviorSpecific);
					_waypointIsDespawn = true;
					_behavior.BehaviorTriggerD = true;


				}

				return _behavior.AutoPilot.State.CargoShipDespawn;

			} 
		
		}

		public CargoShip(IBehavior behavior){

			_subClass = BehaviorSubclass.CargoShip;
			_behavior = behavior;
			_waypointIsDespawn = false;

			_defaultWeaponProfile = "MES-Weapons-GenericPassive";

			CustomWaypoints = new List<Vector3D>();

			GetSpeedFromSpawnGroup = false;
			UsePauseAutopilotFromSpawnGroup = false;

		}

		public void ProcessBehavior() {

			if(MES_SessionCore.IsServer == false) {

				return;

			}

			BehaviorLogger.Write(_behavior.Mode.ToString(), BehaviorDebugEnum.General);
			
			if(_behavior.Mode != BehaviorMode.Retreat && _behavior.BehaviorSettings.DoRetreat == true){

				_behavior.ChangeCoreBehaviorMode(BehaviorMode.Retreat);
				_behavior.AutoPilot.ActivateAutoPilot(_cargoShipWaypoint.GetCoords(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing, CheckEnum.Yes, CheckEnum.No);

			}

			bool _firstRun = false;

			//Init
			if (_behavior.Mode == BehaviorMode.Init) {

				_behavior.AutoPilot.State.CargoShipWaypoints.Clear();

				foreach (var waypoint in CustomWaypoints) {

					_behavior.AutoPilot.State.CargoShipWaypoints.Add(new EncounterWaypoint(waypoint));
				
				}

				SelectNextWaypoint();
				_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitAtWaypoint);

				if (GetSpeedFromSpawnGroup && _behavior.CurrentGrid.Npc != null && _behavior.CurrentGrid.Npc.Attributes.IsCargoShip) {

					_behavior.BehaviorSettings.State.MaxSpeedOverride = _behavior.CurrentGrid.Npc.PrefabSpeed;

				}

				if (UsePauseAutopilotFromSpawnGroup && _behavior.CurrentGrid.Npc.SpawnGroup != null) {

					_behavior.BehaviorSettings.State.MaxSpeedOverride = _behavior.CurrentGrid.Npc.PrefabSpeed;
					_stoppingRange = _behavior.CurrentGrid.Npc.SpawnGroup.PauseAutopilotAtPlayerDistance;
				}

				_firstRun = true;

			}

			//WaitAtWaypoint
			if (_behavior.Mode == BehaviorMode.WaitAtWaypoint) {

				var timeSpan = MyAPIGateway.Session.GameDateTime - _behavior.AutoPilot.State.WaypointWaitTime;

				if (timeSpan.TotalSeconds >= _behavior.AutoPilot.Data.WaypointWaitTimeTrigger) {

					SelectNextWaypoint();
					_behavior.AutoPilot.ActivateAutoPilot(_cargoShipWaypoint.GetCoords(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing, CheckEnum.Yes, CheckEnum.No);
					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);

					if (!_firstRun)
						_behavior.BehaviorTriggerB = true;
					else
						_firstRun = false;

				}

			}

			//Approach
			if (_behavior.Mode == BehaviorMode.ApproachTarget) {

				if (UsePauseAutopilotFromSpawnGroup && _stoppingRange > 0) {

					var player = PlayerManager.GetNearestPlayer(_behavior.RemoteControl.GetPosition());
					_playerInRange = player != null && player.Distance(_behavior.RemoteControl.GetPosition()) < _stoppingRange;

					if (_playerInRange && !_stoppedForPlayer) {

						_stoppedForPlayer = true;
						_behavior.AutoPilot.ActivateAutoPilot(_cargoShipWaypoint.GetCoords(), NewAutoPilotMode.PlanetaryPathing, CheckEnum.Yes, CheckEnum.No);

					} else if (!_playerInRange && _stoppedForPlayer) {

						_stoppedForPlayer = false;
						_behavior.AutoPilot.ActivateAutoPilot(_cargoShipWaypoint.GetCoords(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.PlanetaryPathing, CheckEnum.Yes, CheckEnum.No);

					}
				
				}

				if (_cargoShipWaypoint == null) {

					_behavior.AutoPilot.ActivateAutoPilot(_cargoShipWaypoint.GetCoords(), NewAutoPilotMode.None, CheckEnum.No, CheckEnum.Yes);
					_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitAtWaypoint);
					return;

				}

				var coords = _cargoShipWaypoint.GetCoords();

				if (!_cargoShipWaypoint.Valid || _cargoShipWaypoint.ReachedWaypoint) {

					_behavior.AutoPilot.ActivateAutoPilot(_cargoShipWaypoint.GetCoords(), NewAutoPilotMode.None, CheckEnum.No, CheckEnum.Yes);
					_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitAtWaypoint);
					return;

				}

				if ((!_waypointIsDespawn && _lastCoords != coords) || (coords != Vector3D.Zero && _behavior.AutoPilot.State.InitialWaypoint == Vector3D.Zero)) {

					_behavior.AutoPilot.SetInitialWaypoint(coords);
					_lastCoords = coords;

				}

				if (ArrivedAtWaypoint()) {

					_cargoShipWaypoint.ReachedWaypoint = true;
					_cargoShipWaypoint.ReachedWaypointTime = MyAPIGateway.Session.GameDateTime;
					_behavior.AutoPilot.State.WaypointWaitTime = MyAPIGateway.Session.GameDateTime;

					if (_waypointIsDespawn) {

						if (_behavior.Despawn.NearestPlayer == null || _behavior.Despawn.PlayerDistance > 1200) {

							_behavior.BehaviorSettings.DoDespawn = true;
						
						}

						_behavior.ChangeCoreBehaviorMode(BehaviorMode.Retreat);
						_behavior.BehaviorSettings.DoRetreat = true;

					} else {

						_behavior.AutoPilot.ActivateAutoPilot(_cargoShipWaypoint.GetCoords(), NewAutoPilotMode.None, CheckEnum.No, CheckEnum.Yes);
						_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitAtWaypoint);
						_behavior.BehaviorTriggerA = true;

					}
				
				}

			}

			//Retreat
			if (_behavior.Mode == BehaviorMode.Retreat) {

				if (_behavior.Despawn.NearestPlayer?.Player?.Controller?.ControlledEntity?.Entity != null) {

					//BehaviorLogger.AddMsg("DespawnCoordsCreated", true);
					_behavior.AutoPilot.SetInitialWaypoint(_behavior.Despawn.GetRetreatCoords());

				}

			}

		}

		private bool ArrivedAtWaypoint() {

			var dist = GetDistanceToWaypoint();
			var waypointTolerance = MathTools.Hypotenuse(_behavior.AutoPilot.Data.WaypointTolerance, _behavior.AutoPilot.Data.WaypointTolerance);

			if (_waypointIsDespawn) {

				bool gotType = false;

				if (_behavior.CurrentGrid?.Npc != null) {

					var spawnType = SpawnRequest.GetPrimarySpawningType(_behavior.CurrentGrid.Npc.SpawnType);

					if (spawnType == SpawningType.SpaceCargoShip) {

						waypointTolerance = Settings.SpaceCargoShips.DespawnDistanceFromEndPath;
						gotType = true;

					}

					if (spawnType == SpawningType.PlanetaryCargoShip) {

						waypointTolerance = Settings.PlanetaryCargoShips.DespawnDistanceFromEndPath;
						gotType = true;

					}

				}

				if (!gotType) {

					if (_behavior.AutoPilot.InGravity()) {

						waypointTolerance = Settings.PlanetaryCargoShips.DespawnDistanceFromEndPath;

					} else {

						waypointTolerance = Settings.SpaceCargoShips.DespawnDistanceFromEndPath;

					}
				
				}
			
			}

			return dist < waypointTolerance;

		}

		private double GetDistanceToWaypoint() {

			if (_behavior.AutoPilot.CurrentPlanet?.Planet != null && _behavior.AutoPilot.UpDirectionFromPlanet != Vector3D.Zero && _waypointIsDespawn) {

				var despawnUp = Vector3D.Normalize(_behavior.AutoPilot.State.InitialWaypoint - _behavior.AutoPilot.CurrentPlanet.Center());
				var mySeaLevel = _behavior.AutoPilot.UpDirectionFromPlanet * _behavior.AutoPilot.CurrentPlanet.Planet.AverageRadius + _behavior.AutoPilot.CurrentPlanet.Center();
				var despawnSeaLevel = despawnUp * _behavior.AutoPilot.CurrentPlanet.Planet.AverageRadius + _behavior.AutoPilot.CurrentPlanet.Center();

				return Vector3D.Distance(mySeaLevel, despawnSeaLevel);

			} 

			return Vector3D.Distance(_behavior.RemoteControl.GetPosition(), _behavior.AutoPilot.State.InitialWaypoint); 

		}

		private void SelectNextWaypoint() {

			if (!_behavior.AutoPilot.State.CargoShipDespawn.Valid) {

				var despawnCoords = Vector3D.Zero;

				BehaviorLogger.Write("Setting Initial CargoShip Despawn Waypoint", BehaviorDebugEnum.BehaviorSpecific);
				//BehaviorLogger.Write("Behavior Null: " + (_behavior == null), BehaviorDebugEnum.Dev);
				//BehaviorLogger.Write("Current Grid Null: " + (_behavior.CurrentGrid == null), BehaviorDebugEnum.Dev);
				//BehaviorLogger.Write("NPC Data Null: " + (_behavior.CurrentGrid.Npc == null), BehaviorDebugEnum.Dev);

				if (_behavior.CurrentGrid.Npc != null && _behavior.CurrentGrid.Npc.EndCoords != Vector3D.Zero && _behavior.CurrentGrid.Npc.StartCoords != _behavior.CurrentGrid.Npc.EndCoords)
					despawnCoords = _behavior.CurrentGrid.Npc.EndCoords;

				if (despawnCoords == Vector3D.Zero) {

					BehaviorLogger.Write("Could Not Get From MES, or Start/End are the Same. Creating Manual Despawn Waypoint", BehaviorDebugEnum.BehaviorSpecific);
					despawnCoords = _behavior.AutoPilot.CalculateDespawnCoords(_behavior.RemoteControl.GetPosition());

				}

				BehaviorLogger.Write("Setting Autopilot State", BehaviorDebugEnum.Dev);
				_behavior.AutoPilot.State.CargoShipDespawn = new EncounterWaypoint(despawnCoords);

			}

			while (true) {

				if (_behavior.AutoPilot.State.CargoShipWaypoints.Count == 0)
					break;

				var waypoint = _behavior.AutoPilot.State.CargoShipWaypoints[0];
				waypoint.GetCoords();

				if (waypoint == null || !waypoint.Valid || waypoint.ReachedWaypoint) {

					BehaviorLogger.Write("Invalid or Reached Waypoint Has Been Removed", BehaviorDebugEnum.BehaviorSpecific);
					_behavior.AutoPilot.State.CargoShipWaypoints.RemoveAt(0);
					continue;

				}

				break;

			}

		}

		public void SetDefaultTags() {

			_behavior.AutoPilot.Data = ProfileManager.GetAutopilotProfile("RAI-Generic-Autopilot-CargoShip");
			_behavior.Despawn.UseNoTargetTimer = false;
			_behavior.AutoPilot.Data.DisableInertiaDampeners = false;

			if (string.IsNullOrWhiteSpace(_behavior.BehaviorSettings.WeaponsSystemProfile)) {

				_behavior.BehaviorSettings.WeaponsSystemProfile = _defaultWeaponProfile;

			}

		}

		public void InitTags() {

			if(string.IsNullOrWhiteSpace(_behavior.RemoteControl?.CustomData) == false) {

				var descSplit = _behavior.RemoteControl.CustomData.Split('\n');

				foreach(var tag in descSplit) {

					//CustomWaypoints
					if (tag.Contains("[CustomWaypoints:") == true) {

						TagParse.TagVector3DListCheck(tag, ref CustomWaypoints);

					}

					//UsePauseAutopilotFromSpawnGroup
					if (tag.Contains("[UsePauseAutopilotFromSpawnGroup:") == true) {

						TagParse.TagBoolCheck(tag, ref UsePauseAutopilotFromSpawnGroup);

					}

					//GetSpeedFromSpawnGroup
					if (tag.Contains("[GetSpeedFromSpawnGroup:") == true) {

						TagParse.TagBoolCheck(tag, ref GetSpeedFromSpawnGroup);

					}


				}

			}

		}

		public override string ToString() {

			var sb = new StringBuilder();
			sb.Append("::: Cargo Ship Behavior :::").AppendLine();
			sb.Append(" - Current Waypoint Is Despawn: ").Append(_waypointIsDespawn).AppendLine();
			sb.Append(" - Total Waypoints:             ").Append(_behavior.AutoPilot.State.CargoShipWaypoints.Count).AppendLine();
			sb.Append(" - Custom Waypoints:            ").Append(CustomWaypoints.Count).AppendLine();

			if (_cargoShipWaypoint.Valid) {

				sb.Append(" - Waypoint:                    ").Append(_cargoShipWaypoint.GetCoords()).AppendLine();

			}

			sb.Append(" - Distance To Waypoint:        ").Append(GetDistanceToWaypoint()).AppendLine();
			sb.Append(" - Arrived At Waypoint:         ").Append(ArrivedAtWaypoint()).AppendLine();
			
			if (_behavior.AutoPilot.CurrentPlanet != null) {

				var despawnUp = Vector3D.Normalize(_behavior.AutoPilot.State.InitialWaypoint - _behavior.AutoPilot.CurrentPlanet.Center());
				var mySeaLevel = _behavior.AutoPilot.UpDirectionFromPlanet * _behavior.AutoPilot.CurrentPlanet.Planet.AverageRadius + _behavior.AutoPilot.CurrentPlanet.Center();
				var despawnSeaLevel = despawnUp * _behavior.AutoPilot.CurrentPlanet.Planet.AverageRadius + _behavior.AutoPilot.CurrentPlanet.Center();
				sb.Append(" - Planet AvgRadius Distance:   ").Append(_behavior.AutoPilot.CurrentPlanet.Planet.AverageRadius).AppendLine();
				sb.Append(" - Position At AvgRadius:       ").Append(mySeaLevel).AppendLine();
				sb.Append(" - Despawn Coords At AvgRadius: ").Append(despawnSeaLevel).AppendLine();

			}

			sb.AppendLine();
			return sb.ToString();

		}

	}

}
	
