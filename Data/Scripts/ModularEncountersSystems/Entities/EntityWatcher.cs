using ModularEncountersSystems.Core;
using ModularEncountersSystems.Progression;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.Zones;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Entities {
	public static class EntityWatcher {

		
		public static bool NewPlayerConnected = false;
		public static bool EntityWatcherRegistered = false;

		private static bool _setup = false;

		public static List<IMyCubeGrid> GridsOnLoad = new List<IMyCubeGrid>();

		public static Action<GridEntity> GridAdded;

		public static Action UnloadEntities;

		public static void RegisterWatcher() {

			var entityList = new HashSet<IMyEntity>();
			MyAPIGateway.Entities.GetEntities(entityList);

			_setup = true;

			foreach (var entity in entityList) {

				if(entity as IMyCubeGrid != null)
					GridsOnLoad.Add(entity as IMyCubeGrid);

				NewEntityDetected(entity);

			}

			_setup = false;

			MyVisualScriptLogicProvider.PlayerConnected += PlayerManager.PlayerConnectEvent;
			MyVisualScriptLogicProvider.PlayerSpawned += PlayerManager.PlayerConnectEvent;
			MyVisualScriptLogicProvider.PlayerRespawnRequest += PlayerManager.PlayerConnectEvent;
			MyAPIGateway.Players.ItemConsumed += PlayerManager.ItemConsumedEvent;
			PlayerManager.RefreshAllPlayers(true);

			UnloadEntities += GridManager.UnloadEntities;
			UnloadEntities += PlayerManager.UnloadEntities;

			MyAPIGateway.Entities.OnEntityAdd += NewEntityDetected;
			EntityWatcherRegistered = true;

			GridManager.LoadData();
			UnloadEntities += GridManager.UnloadData;

			MES_SessionCore.UnloadActions += UnregisterWatcher;

			if (MyAPIGateway.Multiplayer.IsServer) {

				TaskProcessor.Tasks.Add(new TimedAction(9, SafeZoneManager.MonitorEntitySafezones, true));
			
			}

		}

		public static void NewEntityDetected(IMyEntity entity) {

			var cubeGrid = entity as IMyCubeGrid;

			if (cubeGrid != null) {

				lock (GridManager.Grids) {

					var gridEntity = new GridEntity(entity);
					UnloadEntities += gridEntity.Unload;
					GridManager.Grids.Add(gridEntity);
					GridAdded?.Invoke(gridEntity);

				}
				
				return;
			
			}

			var planet = entity as MyPlanet;

			if (planet != null) {

				var planetEntity = new PlanetEntity(entity);
				UnloadEntities += planetEntity.Unload;
				PlanetManager.Planets.Add(planetEntity);

				if(!_setup)
					ZoneManager.AddNewZones();

				PlanetManager.CalculateLanes();
				return;

			}

			var safezone = entity as MySafeZone;

			if (safezone != null) {

				var safezoneEntity = new SafeZoneEntity(entity);
				UnloadEntities += safezoneEntity.Unload;
				SafeZoneManager.SafeZones.Add(safezoneEntity);
				return;

			}

		}

		public static void UnregisterWatcher() {

			MyAPIGateway.Entities.OnEntityAdd -= NewEntityDetected;
			MyAPIGateway.Players.ItemConsumed -= PlayerManager.ItemConsumedEvent;
			MyVisualScriptLogicProvider.PlayerConnected -= PlayerManager.PlayerConnectEvent;
			MyVisualScriptLogicProvider.PlayerSpawned -= PlayerManager.PlayerConnectEvent;
			MyVisualScriptLogicProvider.PlayerRespawnRequest -= PlayerManager.PlayerConnectEvent;
			MES_SessionCore.SaveActions -= ProgressionManager.SaveProgression;

			UnloadEntities?.Invoke();

			UnloadEntities = null;
			GridManager.Grids.Clear();
			PlayerManager.Players.Clear();
			PlanetManager.Planets.Clear();
			SafeZoneManager.SafeZones.Clear();
			

		}

	}

}
