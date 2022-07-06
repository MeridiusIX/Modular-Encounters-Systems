using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Tasks {
	public class PrefabDiagnosticsTask : TaskItem, ITaskItem {

		internal int _spawnTicks; //Amount of ticks to wait after spawning
		internal int _testTicks; //Amount of ticks to wait for thrust test to complete

		internal string _modId;
		internal List<MyPrefabDefinition> _prefabs;
		internal int _currentPrefabIndex;
		internal StringBuilder _text;

		internal bool _setup;

		internal bool _nextGrid;

		internal bool _spawningGrid;
		internal bool _detectedGrid;
		internal int _spawningGridTicks;

		internal bool _checkingSystems;
		internal int _thrusterTestTicks;

		internal List<PrefabDiagnostics> _grids;
		internal List<IMyCubeGrid> _dummyList;

		internal bool _finalize;

		public PrefabDiagnosticsTask(string modID) {

			_isValid = true;
			_tickTrigger = 5;

			_spawnTicks = 60;
			_testTicks = 150;

			_setup = true;
			_modId = modID;
			_prefabs = new List<MyPrefabDefinition>();
			_currentPrefabIndex = -1;
			_text = new StringBuilder();

			_grids = new List<PrefabDiagnostics>();

			_finalize = false;

		}

		public override void Run() {

			if (_finalize) {

				Finalize();
				return;
			
			}

			if (_setup)
				Setup();

			if (_nextGrid) {

				NextGrid();
				return;

			}

			if (_spawningGridTicks >= _spawnTicks) {

				_detectedGrid = false;
				_spawningGrid = false;
				_checkingSystems = true;
				_spawningGridTicks = 0;
				_thrusterTestTicks = 0;

				foreach (var grid in _grids) {

					try {

						grid.ProcessGrid();

					} catch (Exception) {

						grid.ProcessedCorrectly = false;
					
					}
			
				}
					
				return;

			} else if(_spawningGrid && _detectedGrid) {

				_spawningGridTicks += _tickTrigger;
				return;

			}

			if (_thrusterTestTicks >= _testTicks) {

				_checkingSystems = false;
				_thrusterTestTicks = 0;
				ClearGrids();
				_nextGrid = true;
			
			} else if(_checkingSystems) {

				_thrusterTestTicks += _tickTrigger;
				return;

			}
			   
		}

		public void Setup() {

			_setup = false;

			//Delete Grids
			ClearGrids();

			//Register Watcher
			DamageHelper.DamageRelay += DamageHandler;
			MyAPIGateway.Entities.OnEntityAdd += OnEntityAdd;

			//Collect Prefabs
			var allPrefabs = MyDefinitionManager.Static.GetPrefabDefinitions();

			foreach (var prefab in allPrefabs.Keys) {

				if (prefab == null)
					continue;

				if (!string.IsNullOrWhiteSpace(allPrefabs[prefab]?.Context?.ModName) && allPrefabs[prefab].Context.ModName.Contains(_modId))
					_prefabs.Add(allPrefabs[prefab]);
			
			}

			//Prefill StringBuilder
			_text.Append("PrefabName").Append(",");
			_text.Append("GridName").Append(",");
			_text.Append("PhysicalGrid").Append(",");
			_text.Append("ProcessedCorrectly").Append(",");

			_text.Append("BlockCount").Append(",");

			_text.Append("AttachedToOtherGrids").Append(",");

			_text.Append("ControllerCount").Append(",");
			_text.Append("HasMainCockpit").Append(",");
			_text.Append("HasRemoteControl").Append(",");
			_text.Append("RemoteControlGridPivotCorrect").Append(",");

			_text.Append("ThrustCount").Append(",");
			_text.Append("HydroThrust").Append(",");
			_text.Append("IonThrust").Append(",");
			_text.Append("AtmoThrust").Append(",");
			_text.Append("ThrustInAllDirections").Append(",");
			_text.Append("DownwardThrust").Append(",");
			_text.Append("ThrustDamageSameGrid").Append(",");
			_text.Append("ThrustDamageAttachedGrid").Append(",");
			_text.Append("ThrustDamageOtherGrid").Append(",");
			_text.Append("MaxGravityAtmo").Append(",");
			_text.Append("MaxGravityVacuum").Append(",");

			_text.Append("GyroCount").Append(",");

			_text.Append("GravityGenerators").Append(",");

			_text.Append("PowerState").Append(",");
			_text.Append("Reactors").Append(",");
			_text.Append("Batteries").Append(",");
			_text.Append("Generators").Append(",");
			_text.Append("Renewables").Append(",");

			_text.Append("Wheels").Append(",");

			_text.Append("LightArmor").Append(",");
			_text.Append("HeavyArmor").Append(",");

			_text.Append("ProgrammableBlocksWithScripts").Append(",");
			_text.Append("Sensors").Append(",");
			_text.Append("Timers").Append(",");
			_text.Append("Lcds").Append(",");

			_text.Append("ContainersWithCargo").Append(",");

			_text.Append("Antennas").Append(",");
			_text.Append("Beacons").Append(",");
			_text.Append("ActiveSignals").Append(",");
			_text.Append("HighestRangeActiveAntenna").Append(",");
			_text.Append("HighestRangeActiveBeacon").Append(",");

			_text.Append("Turrets").Append(",");
			_text.Append("Guns").Append(",");
			_text.Append("Warheads").Append(",");

			_text.Append("-----").Append(",");

			_text.Append("DamageThrusters").Append(",");

			_text.AppendLine();

			//Start Process
			_nextGrid = true;

		}

		public void ClearGrids() {

			HashSet<IMyEntity> entities = new HashSet<IMyEntity>();
			MyAPIGateway.Entities.GetEntities(entities);

			foreach (var entity in entities)
				if (entity as IMyCubeGrid != null)
					entity.Close();

		}

		public void NextGrid() {

			foreach (var grid in _grids)
				_text.Append(grid.GetCSV()).AppendLine();

			_nextGrid = false;
			_currentPrefabIndex++;

			if (_currentPrefabIndex >= _prefabs.Count) {

				_finalize = true;
				return;
			
			}

			_spawningGrid = true;
			_spawningGridTicks = 0;
			_grids.Clear();
			PrefabSpawner.PrefabSpawnDebug(MyAPIGateway.Session.LocalHumanPlayer.IdentityId, _prefabs[_currentPrefabIndex].Id.SubtypeName);

		}

		public void OnEntityAdd(IMyEntity entity) {

			if (!_spawningGrid || entity as IMyCubeGrid == null)
				return;

			var diagnostics = new PrefabDiagnostics();
			diagnostics.PrefabName = _prefabs[_currentPrefabIndex].Id.SubtypeName;
			diagnostics.CubeGrid = entity as IMyCubeGrid;
			diagnostics.GridName = diagnostics.CubeGrid?.CustomName ?? "Null";
			_grids.Add(diagnostics);
			_detectedGrid = true;
			_spawningGridTicks = 0;

		}

		public void Finalize() {

			MyAPIGateway.Entities.OnEntityAdd -= OnEntityAdd;
			DamageHelper.DamageRelay -= DamageHandler;

			VRage.Utils.MyClipboardHelper.SetClipboard(_text.ToString());
			MyVisualScriptLogicProvider.ShowNotificationToAll("Prefab Processing Complete On Prefab Count: " + _prefabs.Count, 4000, "Green");
			MyVisualScriptLogicProvider.ShowNotificationToAll("CSV Data Saved To Clipboard", 4000, "Green");

			_isValid = false;

		}

		public void DamageHandler(object damagedObject, MyDamageInformation info) {

			IMyEntity entity = null;

			if (!MyAPIGateway.Entities.TryGetEntityById(info.AttackerId, out entity))
				return;

			if (entity as IMyThrust == null || damagedObject as IMySlimBlock == null)
				return;

			var thrust = entity as IMyThrust;
			var hit = damagedObject as IMySlimBlock;

			for (int i = 0; i < _grids.Count; i++) {

				if (_grids[i].CubeGrid == thrust.SlimBlock.CubeGrid && !_grids[i].DamageThrusts.Contains(thrust)) {

					_grids[i].DamageThrusts.Add(thrust);
					break;

				}

			}

			if (hit.CubeGrid == thrust.SlimBlock.CubeGrid) {

				for (int i = 0; i < _grids.Count; i++) {

					if (_grids[i].CubeGrid == thrust.SlimBlock.CubeGrid) {

						_grids[i].ThrustDamageSameGrid = true;
						return;

					}
				
				}

				return;
			
			}

			if (MyAPIGateway.GridGroups.HasConnection(hit.CubeGrid, thrust.SlimBlock.CubeGrid, GridLinkTypeEnum.Physical) || MyAPIGateway.GridGroups.HasConnection(hit.CubeGrid, thrust.SlimBlock.CubeGrid, GridLinkTypeEnum.NoContactDamage)) {

				for (int i = 0; i < _grids.Count; i++) {

					if (_grids[i].CubeGrid == thrust.SlimBlock.CubeGrid) {

						_grids[i].ThrustDamageAttachedGrid = true;
						return;

					}

				}

				return;

			}

			for (int i = 0; i < _grids.Count; i++) {

				if (_grids[i].CubeGrid == thrust.SlimBlock.CubeGrid) {

					_grids[i].ThrustDamageOtherGrid = true;
					return;

				}

			}

		}

	}

	public class PrefabThrustDirections {

		public bool HasThrust;

		public bool Left;
		public bool Right;
		public bool Up;
		public bool Down;
		public bool Forward;
		public bool Backward;

		public PrefabThrustDirections() {
		
			
		
		}

		public override string ToString() {

			string dirs = "";

			if (Left)
				dirs += "L-";

			if (Right)
				dirs += "R-";

			if (Up)
				dirs += "U-";

			if (Down)
				dirs += "D-";

			if (Forward)
				dirs += "F-";

			if (Backward)
				dirs += "B-";

			return string.IsNullOrWhiteSpace(dirs) ? HasThrust.ToString() : dirs.Remove(dirs.Length - 1, 1);

		}

	}

	public class PrefabDiagnostics {

		public IMyCubeGrid CubeGrid;

		public string PrefabName;
		public string GridName;
		public bool Physics;
		public bool ProcessedCorrectly;
		public bool GotGridSize;
		public MyCubeSize GridSize;

		public int BlockCount;

		public bool AttachedToOtherGrids;

		public int ControllerCount;
		public bool HasMainCockpit;
		public bool HasRemoteControl;
		public bool RemoteControlGridPivotCorrect;

		public int ThrustCount;
		public PrefabThrustDirections IonThrust;
		public PrefabThrustDirections AtmoThrust;
		public PrefabThrustDirections HydroThrust;
		public bool ThrustInAllDirections;
		public bool ThrustDamageSameGrid;
		public bool ThrustDamageAttachedGrid;
		public bool ThrustDamageOtherGrid;
		public float MaxGravityAtmo;
		public float MaxGravityVacuum;

		public int GyroCount;

		public int GravityGenerators;

		public MyResourceStateEnum PowerState;
		public int Reactors;
		public int Batteries;
		public int Generators;
		public int Renewables;

		public int Wheels;

		public int LightArmor;
		public int HeavyArmor;

		public int ProgrammableBlocksWithScripts;
		public int Sensors;
		public int Timers;
		public int Lcds;

		public int ContainersWithCargo;

		public int Antennas;
		public int Beacons;
		public bool ActiveSignals;
		public float HighestRangeActiveAntenna;
		public float HighestRangeActiveBeacon;

		public int Turrets;
		public int Guns;
		public int Warheads;

		public List<IMyThrust> DamageThrusts;

		private List<IMyThrust> _downwardThrust;
		private IMyShipController _mainController;

		public PrefabDiagnostics(bool dummy = false) {

			CubeGrid = null;

			PrefabName = "";
			GridName = "";
			Physics = false;
			ProcessedCorrectly = true;
			GotGridSize = false;
			GridSize = MyCubeSize.Large;

			BlockCount = 0;

			AttachedToOtherGrids = false;

			ControllerCount = 0;
			HasMainCockpit = false;
			HasRemoteControl = false;
			RemoteControlGridPivotCorrect = false;

			ThrustCount = 0;
			IonThrust = new PrefabThrustDirections();
			AtmoThrust = new PrefabThrustDirections();
			HydroThrust = new PrefabThrustDirections();
			ThrustInAllDirections = false;
			ThrustDamageSameGrid = false;
			ThrustDamageAttachedGrid = false;
			ThrustDamageOtherGrid = false;
			MaxGravityAtmo = 0f;
			MaxGravityVacuum = 0f;

			GyroCount = 0;

			Wheels = 0;

			GravityGenerators = 0;

			PowerState = MyResourceStateEnum.Disconnected;
			Reactors = 0;
			Batteries = 0;
			Generators = 0;
			Renewables = 0;

			LightArmor = 0;
			HeavyArmor = 0;

			ProgrammableBlocksWithScripts = 0;
			Sensors = 0;
			Lcds = 0;
			Timers = 0;

			ContainersWithCargo = 0;

			Antennas = 0;
			Beacons = 0;
			ActiveSignals = false;
			HighestRangeActiveAntenna = 0;
			HighestRangeActiveBeacon = 0;

			Turrets = 0;
			Guns = 0;
			Warheads = 0;

			DamageThrusts = new List<IMyThrust>();

			_downwardThrust = new List<IMyThrust>();

		}

		public void ProcessGrid() {

			if (CubeGrid == null || CubeGrid.Closed || CubeGrid.MarkedForClose)
				return;

			Physics = CubeGrid.Physics != null;

			var blocks = new List<IMySlimBlock>();
			CubeGrid.GetBlocks(blocks);

			BlockCount = blocks.Count;
			var thrustDir = new List<Vector3D>();

			var othergrids = new List<IMyCubeGrid>();
			MyAPIGateway.GridGroups.GetGroup(CubeGrid, GridLinkTypeEnum.Physical, othergrids);

			if (othergrids.Count > 1) {

				AttachedToOtherGrids = true;

			} else {

				othergrids.Clear();
				MyAPIGateway.GridGroups.GetGroup(CubeGrid, GridLinkTypeEnum.NoContactDamage, othergrids);
				if(othergrids.Count > 1)
					AttachedToOtherGrids = true;

			}
			
			//First Run of Blocks
			foreach (var block in blocks) {

				//Controller
				if (block.FatBlock as IMyShipController != null) {

					var controller = block.FatBlock as IMyShipController;

					if (controller.CanControlShip) {

						ControllerCount++;
						if (_mainController == null)
							_mainController = controller;

					}
						
					if (controller.IsMainCockpit) {

						HasMainCockpit = true;

						if (!_mainController.IsMainCockpit)
							_mainController = controller;

					}
						
					if (controller as IMyRemoteControl != null) {

						HasRemoteControl = true;

						if (controller.Orientation.Forward == Base6Directions.Direction.Forward && controller.Orientation.Up == Base6Directions.Direction.Up)
							RemoteControlGridPivotCorrect = true;
					
					}

					continue;
				
				}

			}

			//2nd Run of Blocks
			foreach (var block in blocks) {

				//Armor
				if (block.FatBlock == null) {

					if (block.BlockDefinition.Id.TypeId == typeof(MyObjectBuilder_CubeBlock) && block.BlockDefinition.Id.SubtypeName.Contains("Armor")) {

						if (block.BlockDefinition.Id.SubtypeName.Contains("Heavy"))
							HeavyArmor++;
						else
							LightArmor++;

					}

					continue;

				}

				//Thrust
				if (block.FatBlock as IMyThrust != null) {

					ThrustCount++;
					var thrust = block.FatBlock as IMyThrust;

					var thrustDef = block.BlockDefinition as MyThrustDefinition;

					if (thrustDef == null) {

						continue;

					}

					var myThrust = thrust as MyThrust;

					if (thrustDef.ThrusterType.ToString() == "Atmospheric")
						ThrustDirection(thrust, _mainController, AtmoThrust);

					if (thrustDef.ThrusterType.ToString() == "Hydrogen")
						ThrustDirection(thrust, _mainController, HydroThrust);

					if (thrustDef.ThrusterType.ToString() == "Ion")
						ThrustDirection(thrust, _mainController, IonThrust);

					if (!thrustDir.Contains(thrust.WorldMatrix.Forward)) {

						if (thrust.IsFunctional && myThrust.IsPowered && thrust.Enabled)
							thrustDir.Add(thrust.WorldMatrix.Forward);

					}

					if (_mainController != null && thrust.WorldMatrix.Forward == _mainController.WorldMatrix.Down)
						if (thrust.IsFunctional && myThrust.IsPowered && thrust.Enabled)
							_downwardThrust.Add(thrust);

					thrust.ThrustOverridePercentage = 1;

					continue;

				}

				if (block.FatBlock as IMyGyro != null) {

					GyroCount++;
					continue;

				}

				if (block.FatBlock as IMyGravityGeneratorBase != null) {

					GravityGenerators++;
					continue;

				}

				if (block.FatBlock as IMyPowerProducer != null) {

					if (block.FatBlock as IMyReactor != null) {

						Reactors++;
						continue;

					}

					if (block.FatBlock as IMySolarPanel != null) {

						Renewables++;
						continue;

					}

					if (block.FatBlock as IMyBatteryBlock != null) {

						Batteries++;
						continue;

					}

					if (block.BlockDefinition as MyHydrogenEngineDefinition != null) {

						Generators++;
						continue;

					}

					//Wind
					Renewables++;
					continue;

				}

				if (block.FatBlock as IMyMotorSuspension != null) {

					Wheels++;
					continue;

				}

				if (block.FatBlock as IMyProgrammableBlock != null) {

					var pb = block.FatBlock as IMyProgrammableBlock;

					if (!string.IsNullOrWhiteSpace(pb.ProgramData)) {

						ProgrammableBlocksWithScripts++;
						continue;

					}

				}

				if (block.FatBlock as IMySensorBlock != null) {

					Sensors++;
					continue;

				}

				if (block.FatBlock as IMyTimerBlock != null) {

					Timers++;
					continue;

				}

				if (block.FatBlock as IMyTextPanel != null) {

					Lcds++;
					continue;

				}

				if (block.FatBlock as IMyCargoContainer != null && !block.FatBlock.GetInventory().Empty()) {

					ContainersWithCargo++;
					continue;

				}

				if (block.FatBlock as IMyRadioAntenna != null) {

					Antennas++;

					var antenna = block.FatBlock as IMyRadioAntenna;

					if (antenna.IsWorking && antenna.IsFunctional && antenna.Enabled && antenna.IsBroadcasting) {

						ActiveSignals = true;

						if (antenna.Radius > HighestRangeActiveAntenna)
							HighestRangeActiveAntenna = antenna.Radius;

					}

					continue;

				}

				if (block.FatBlock as IMyBeacon != null) {

					Beacons++;

					var antenna = block.FatBlock as IMyBeacon;

					if (antenna.IsWorking && antenna.IsFunctional && antenna.Enabled) {

						ActiveSignals = true;

						if (antenna.Radius > HighestRangeActiveBeacon)
							HighestRangeActiveBeacon = antenna.Radius;

					}

					continue;

				}

				if (block.FatBlock as IMyUserControllableGun != null) {

					if (block.FatBlock as IMyLargeTurretBase != null)
						Turrets++;
					else
						Guns++;

					continue;

				}

				if (block.FatBlock as IMyWarhead != null) {

					Warheads++;
					continue;

				}

			}

			PowerState = CubeGrid.ResourceDistributor.ResourceState;

			if (thrustDir.Count >= 6)
				ThrustInAllDirections = true;

			MaxGravityAtmo = (float)Math.Round(CalculatePlanetaryThrust(), 2);
			MaxGravityVacuum = (float)Math.Round(CalculatePlanetaryThrust(0), 2);

		}

		internal void ThrustDirection(IMyThrust thrust, IMyShipController controller, PrefabThrustDirections directions) {

			if (thrust == null)
				return;

			directions.HasThrust = true;

			if (controller == null)
				return;

			if (thrust.WorldMatrix.Forward == controller.WorldMatrix.Right) {

				directions.Left = true;
				return;
			
			}

			if (thrust.WorldMatrix.Forward == controller.WorldMatrix.Left) {

				directions.Right = true;
				return;

			}

			if (thrust.WorldMatrix.Forward == controller.WorldMatrix.Down) {

				directions.Up = true;
				return;

			}

			if (thrust.WorldMatrix.Forward == controller.WorldMatrix.Up) {

				directions.Down = true;
				return;

			}

			if (thrust.WorldMatrix.Forward == controller.WorldMatrix.Backward) {

				directions.Forward = true;
				return;

			}

			if (thrust.WorldMatrix.Forward == controller.WorldMatrix.Forward) {

				directions.Backward = true;
				return;

			}

		}

		internal float CalculatePlanetaryThrust(float airDensity = 0.7f) {

			if (_mainController == null || _downwardThrust.Count == 0)
				return 0;

			float gravityMultiplier = 0;

			while (gravityMultiplier < 20) {

				gravityMultiplier += 0.1f;
				double totalForceAvailable = 0;

				foreach (var thrust in _downwardThrust) {

					var thrustDef = thrust.SlimBlock.BlockDefinition as MyThrustDefinition;

					if (thrustDef == null)
						continue;

					/*
					if (thrustDef.ThrusterType.ToString() == "Hydrogen") {

						totalForceAvailable += thrustDef.ForceMagnitude; //TODO: see why this doesnt work
						continue;a

					}
					*/

					totalForceAvailable += thrustDef.ForceMagnitude * thrustForce(thrustDef, airDensity);

				}

				var liftingAccel = totalForceAvailable / _mainController.CalculateShipMass().TotalMass;
				var gravityAccel = gravityMultiplier * 9.81;

				if ((liftingAccel / gravityAccel) < 1.25) {

					gravityMultiplier -= 0.1f;
					break;

				}

			}

			return gravityMultiplier;

		}

		internal double thrustForce(MyThrustDefinition def, float airDensity = 0.7f) {

			double minPlanetInfluence = def.MinPlanetaryInfluence;
			double maxPlanetInfluence = def.MaxPlanetaryInfluence;
			double effectiveAtMin = def.EffectivenessAtMinInfluence;
			double effectiveAtMax = def.EffectivenessAtMaxInfluence;

			var InvDiffMinMaxPlanetaryInfluence = 1f / (maxPlanetInfluence - minPlanetInfluence);

			double value = (airDensity - minPlanetInfluence) * InvDiffMinMaxPlanetaryInfluence;
			var result = MathHelper.Lerp(effectiveAtMin, effectiveAtMax, MathHelper.Clamp(value, 0f, 1f));
			return result;

		}

		public string GetCSV() {

			var sb = new StringBuilder();
			sb.Append(PrefabName).Append(",");
			sb.Append(GridName).Append(",");
			sb.Append(Physics).Append(",");
			sb.Append(ProcessedCorrectly).Append(",");

			sb.Append(BlockCount).Append(",");

			sb.Append(AttachedToOtherGrids).Append(",");

			sb.Append(ControllerCount).Append(",");
			sb.Append(HasMainCockpit).Append(",");
			sb.Append(HasRemoteControl).Append(",");
			sb.Append(RemoteControlGridPivotCorrect).Append(",");

			sb.Append(ThrustCount).Append(",");
			sb.Append(HydroThrust.ToString()).Append(",");
			sb.Append(IonThrust.ToString()).Append(",");
			sb.Append(AtmoThrust.ToString()).Append(",");
			sb.Append(ThrustInAllDirections).Append(",");
			sb.Append(_downwardThrust.Count).Append(",");
			sb.Append(ThrustDamageSameGrid).Append(",");
			sb.Append(ThrustDamageAttachedGrid).Append(",");
			sb.Append(ThrustDamageOtherGrid).Append(",");
			sb.Append(MaxGravityAtmo).Append(",");
			sb.Append(MaxGravityVacuum).Append(",");

			sb.Append(GyroCount).Append(",");

			sb.Append(GravityGenerators).Append(",");

			sb.Append(PowerState.ToString()).Append(",");
			sb.Append(Reactors).Append(",");
			sb.Append(Batteries).Append(",");
			sb.Append(Generators).Append(",");
			sb.Append(Renewables).Append(",");

			sb.Append(Wheels).Append(",");

			sb.Append(LightArmor).Append(",");
			sb.Append(HeavyArmor).Append(",");

			sb.Append(ProgrammableBlocksWithScripts).Append(",");
			sb.Append(Sensors).Append(",");
			sb.Append(Timers).Append(",");
			sb.Append(Lcds).Append(",");

			sb.Append(ContainersWithCargo).Append(",");

			sb.Append(Antennas).Append(",");
			sb.Append(Beacons).Append(",");
			sb.Append(ActiveSignals).Append(",");
			sb.Append(HighestRangeActiveAntenna).Append(",");
			sb.Append(HighestRangeActiveBeacon).Append(",");

			sb.Append(Turrets).Append(",");
			sb.Append(Guns).Append(",");
			sb.Append(Warheads).Append(",");

			sb.Append("-----").Append(",");

			foreach (var thrust in DamageThrusts)
				sb.Append(thrust.CustomName).Append(" - ").Append(thrust.SlimBlock.Min.ToString().Replace(',', ' ')).Append(",");

			return sb.ToString();
		
		}

	}

}
