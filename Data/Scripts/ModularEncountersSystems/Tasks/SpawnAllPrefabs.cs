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
	public class SpawnAllPrefabs : TaskItem, ITaskItem {

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
		internal List<BoundingBoxD> _boxes;
		internal BoundingBoxD _combinedBox;
		internal MatrixD _matrix;

		internal bool _finalize;

		public SpawnAllPrefabs(string modID) {

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
			_boxes = new List<BoundingBoxD>();

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
				_nextGrid = true;
				return;

			} else if(_spawningGrid && _detectedGrid) {

				_spawningGridTicks += _tickTrigger;
				return;

			}
			   
		}

		public void Setup() {

			_setup = false;

			//Delete Grids
			ClearGrids();

			//Register Watcher
			MyAPIGateway.Entities.OnEntityAdd += OnEntityAdd;

			//Collect Prefabs
			var allPrefabs = MyDefinitionManager.Static.GetPrefabDefinitions();

			foreach (var prefab in allPrefabs.Keys) {

				if (prefab == null)
					continue;

				if (!string.IsNullOrWhiteSpace(allPrefabs[prefab]?.Context?.ModName) && allPrefabs[prefab].Context.ModName.Contains(_modId))
					_prefabs.Add(allPrefabs[prefab]);
			
			}

			_matrix = MyAPIGateway.Session.LocalHumanPlayer.Character.WorldMatrix;
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

			_nextGrid = false;
			_currentPrefabIndex++;

			if (_currentPrefabIndex >= _prefabs.Count) {

				_finalize = true;
				return;
			
			}

			_spawningGrid = true;
			_spawningGridTicks = 0;
			_grids.Clear();

			if (_boxes.Count > 0) {

				_combinedBox = _boxes[0];

				if(_boxes.Count > 1)
					for (int i = 1; i < _boxes.Count; i++) {

						var newBox = BoundingBoxD.CreateMerged(_combinedBox, _boxes[i]);
						_combinedBox = newBox;

					}

			}

			_boxes.Clear();
			PrefabSpawner.PrefabSpawnDebug(MyAPIGateway.Session.LocalHumanPlayer.IdentityId, _prefabs[_currentPrefabIndex].Id.SubtypeName, _combinedBox, _matrix);

		}

		public void OnEntityAdd(IMyEntity entity) {

			if (!_spawningGrid || entity as IMyCubeGrid == null)
				return;

			_boxes.Add(entity.WorldAABB);
			_detectedGrid = true;
			_spawningGridTicks = 0;

		}

		public void Finalize() {

			MyAPIGateway.Entities.OnEntityAdd -= OnEntityAdd;
			_isValid = false;

		}
	}

}
