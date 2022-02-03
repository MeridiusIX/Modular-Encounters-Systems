using ModularEncountersSystems.Configuration.Editor;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ModularEncountersSystems.Configuration {

	//PlanetaryCargoShips

	/*
	  
	  Hello Stranger!
	 
	  If you are in here because you want to change settings
	  for how this mod behaves, you are in the wrong place.

	  All the settings in this file, along with the other
	  configuration files, are created as XML files in the
	  \Storage\1521905890.sbm_ModularEncountersSpawner folder
	  of your Save File. This means you do not need to edit
	  the mod files here to tune the settings to your liking.

	  The workshop page for this mod also has a link to a
	  guide that explains what all the configuration options
	  do, along with how to activate them in-game via chat
	  commands if desired.
	  
	  If you plan to edit the values here anyway, I ask that
	  you do not reupload this mod to the Steam Workshop. If
	  this is not respected and I find out about it, I'll
	  exercise my rights as the creator and file a DMCA
	  takedown on any infringing copies. This warning can be
	  found on the workshop page for this mod as well.

	  Thank you.
		 
	*/

	public class ConfigPlanetaryCargoShips : ConfigBase, IGridSpawn {
		
		public string ModVersion;
		public bool EnableSpawns;
		public int FirstSpawnTime; //Time Until Spawn When World Starts
		public int MinSpawnTime; //Min Time Until Next Spawn
		public int MaxSpawnTime; //Max Time Until Next Spawn
		public int MaxShipsPerArea;
		public double AreaSize;
		public int MaxSpawnAttempts; //Number Of Attempts To Spawn Ship(s)
		public double PlayerSurfaceAltitude; //Player Must Be Less Than This Altitude From Surface For Spawn Attempt
		public double MinPathDistanceFromPlayer;
		public double MaxPathDistanceFromPlayer;
		public double MinSpawnDistFromEntities;
		public float MinAirDensity; //Acts As A Dynamic Max Altitude For Spawning
		public double MinSpawningAltitude; //Minimum Distance From The Surface For Spawning
		public double MaxSpawningAltitude;
		public double MinPathAltitude; //Minimum Path Altitude From Start to End
		public double MinPathDistance; //Minimum Path Distance Of Cargo Ship
		public double MaxPathDistance; //Maximum Path Distance Of Cargo Ship
		public double PathStepCheckDistance; //Distance Between Altitude Checks Of Path (Used To Ensure Path Isn't Obstructed By Terrain)
		public double DespawnDistanceFromEndPath; // Ship Will Despawn If Within This Distance Of Path End Coordinates
		public double DespawnDistanceFromPlayer;
		public double DespawnAltitude;
		public bool UseMinimumSpeed;
		public float MinimumSpeed;
		public bool UseSpeedOverride; //If True, The Cargo Ship Will Use Override Speed Instead Of Prefab Speed
		public float SpeedOverride; //Override Speed Value For Cargo Ship (If Used)
		
		public bool UseMaxSpawnGroupFrequency;
		public int MaxSpawnGroupFrequency;

		public bool EnableWaveSpawner;
		public string[] UseSpecificRandomGroups;
		public int MinWaveSpawnTime;
		public int MaxWaveSpawnTime;
		public int TotalSpawnEventsPerCluster;
		public int TimeBetweenWaveSpawns;
		public double PlayerClusterDistance;

		[XmlIgnore]
		public Dictionary<string, Func<string, object, bool>> EditorReference;

		public ConfigPlanetaryCargoShips(){
			
			ModVersion = MES_SessionCore.ModVersion;
			EnableSpawns = true;
			FirstSpawnTime = 300;
			MinSpawnTime = 780;
			MaxSpawnTime = 1020;
			MaxShipsPerArea = 2;
			AreaSize = 20000;
			MaxSpawnAttempts = 25;
			PlayerSurfaceAltitude = 6000;
			MinPathDistanceFromPlayer = 3000;
			MaxPathDistanceFromPlayer = 5000;
			MinSpawnDistFromEntities = 1200;
			MinAirDensity = 0.70f;
			MinSpawningAltitude = 1700;
			MaxSpawningAltitude = 2500;
			MinPathAltitude = 900;
			MinPathDistance = 10000;
			MaxPathDistance = 13000;
			PathStepCheckDistance = 100;
			DespawnDistanceFromEndPath = 750;
			DespawnDistanceFromPlayer = 1000;
			DespawnAltitude = 5000;
			UseMinimumSpeed = false;
			MinimumSpeed = 10;
			UseSpeedOverride = false;
			SpeedOverride = 20;
			
			UseMaxSpawnGroupFrequency = false;
			MaxSpawnGroupFrequency = 5;

			UseTimeout = true;
			TimeoutDuration = 900;
			TimeoutRadius = 10000;
			TimeoutSpawnLimit = 2;

			UseTypeDisownTimer = true;
			TypeDisownTimer = 1800;

			EnableWaveSpawner = false;
			UseSpecificRandomGroups = new string[] { "SomeSpawnGroupNameHere", "AnotherSpawnGroupNameHere", "EtcEtcEtc" };
			MinWaveSpawnTime = 1980;
			MaxWaveSpawnTime = 3600;
			TotalSpawnEventsPerCluster = 6;
			TimeBetweenWaveSpawns = 8;
			PlayerClusterDistance = 15000;

			UseCleanupSettings = true;
			CleanupUseDistance = true;
			CleanupUseTimer = false;
			CleanupUseBlockLimit = false;
			CleanupDistanceStartsTimer = false;
			CleanupResetTimerWithinDistance = false;
			CleanupDistanceTrigger = 25000;
			CleanupTimerTrigger = 1800;
			CleanupBlockLimitTrigger = 0;
			CleanupIncludeUnowned = true;
			CleanupUnpoweredOverride = true;
			CleanupUnpoweredDistanceTrigger = 25000;
			CleanupUnpoweredTimerTrigger = 900;

			EditorReference = new Dictionary<string, Func<string, object, bool>> {

				{"EnableSpawns", (s, o) => EditorTools.SetCommandValueBool(s, ref EnableSpawns) },
				{"FirstSpawnTime", (s, o) => EditorTools.SetCommandValueInt(s, ref FirstSpawnTime) },
				{"MinSpawnTime", (s, o) => EditorTools.SetCommandValueInt(s, ref MinSpawnTime) },
				{"MaxSpawnTime", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxSpawnTime) },
				{"MaxShipsPerArea", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxShipsPerArea) },
				{"AreaSize", (s, o) => EditorTools.SetCommandValueDouble(s, ref AreaSize) },
				{"MaxSpawnAttempts", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxSpawnAttempts) },
				{"PlayerSurfaceAltitude", (s, o) => EditorTools.SetCommandValueDouble(s, ref PlayerSurfaceAltitude) },
				{"MinPathDistanceFromPlayer", (s, o) => EditorTools.SetCommandValueDouble(s, ref MinPathDistanceFromPlayer) },
				{"MaxPathDistanceFromPlayer", (s, o) => EditorTools.SetCommandValueDouble(s, ref MaxPathDistanceFromPlayer) },
				{"MinSpawnDistFromEntities", (s, o) => EditorTools.SetCommandValueDouble(s, ref MinSpawnDistFromEntities) },
				{"MinAirDensity", (s, o) => EditorTools.SetCommandValueFloat(s, ref MinAirDensity) },
				{"MinSpawningAltitude", (s, o) => EditorTools.SetCommandValueDouble(s, ref MinSpawningAltitude) },
				{"MaxSpawningAltitude", (s, o) => EditorTools.SetCommandValueDouble(s, ref MaxSpawningAltitude) },
				{"MinPathAltitude", (s, o) => EditorTools.SetCommandValueDouble(s, ref MinPathAltitude) },
				{"MinPathDistance", (s, o) => EditorTools.SetCommandValueDouble(s, ref MinPathDistance) },
				{"MaxPathDistance", (s, o) => EditorTools.SetCommandValueDouble(s, ref MaxPathDistance) },
				{"PathStepCheckDistance", (s, o) => EditorTools.SetCommandValueDouble(s, ref PathStepCheckDistance) },
				{"DespawnDistanceFromEndPath", (s, o) => EditorTools.SetCommandValueDouble(s, ref DespawnDistanceFromEndPath) },
				{"DespawnDistanceFromPlayer", (s, o) => EditorTools.SetCommandValueDouble(s, ref DespawnDistanceFromPlayer) },
				{"DespawnAltitude", (s, o) => EditorTools.SetCommandValueDouble(s, ref DespawnAltitude) },
				{"UseMinimumSpeed", (s, o) => EditorTools.SetCommandValueBool(s, ref UseMinimumSpeed) },
				{"MinimumSpeed", (s, o) => EditorTools.SetCommandValueFloat(s, ref MinimumSpeed) },
				{"UseSpeedOverride", (s, o) => EditorTools.SetCommandValueBool(s, ref UseSpeedOverride) },
				{"SpeedOverride", (s, o) => EditorTools.SetCommandValueFloat(s, ref SpeedOverride) },
				{"MaxSpawnGroupFrequency", (s, o) => EditorTools.SetCommandValueBool(s, ref UseMaxSpawnGroupFrequency) },
				{"EnableWaveSpawner", (s, o) => EditorTools.SetCommandValueBool(s, ref EnableWaveSpawner) },
				{"UseSpecificRandomGroups", (s, o) => EditorTools.SetCommandValueStringArray(s, ref UseSpecificRandomGroups) },
				{"MinWaveSpawnTime", (s, o) => EditorTools.SetCommandValueInt(s, ref MinWaveSpawnTime) },
				{"MaxWaveSpawnTime", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxWaveSpawnTime) },
				{"TotalSpawnEventsPerCluster", (s, o) => EditorTools.SetCommandValueInt(s, ref TotalSpawnEventsPerCluster) },
				{"TimeBetweenWaveSpawns", (s, o) => EditorTools.SetCommandValueInt(s, ref TimeBetweenWaveSpawns) },
				{"PlayerClusterDistance", (s, o) => EditorTools.SetCommandValueDouble(s, ref PlayerClusterDistance) },

			};

		}
		
		public ConfigPlanetaryCargoShips LoadSettings(string phase) {
			
			if(MyAPIGateway.Utilities.FileExistsInWorldStorage("Config-PlanetaryCargoShips.xml", typeof(ConfigPlanetaryCargoShips)) == true){
				
				try{
					
					ConfigPlanetaryCargoShips config = null;
					var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage("Config-PlanetaryCargoShips.xml", typeof(ConfigPlanetaryCargoShips));
					string configcontents = reader.ReadToEnd();
					config = MyAPIGateway.Utilities.SerializeFromXML<ConfigPlanetaryCargoShips>(configcontents);
					config.ConfigLoaded = true;
					SpawnLogger.Write("Loaded Existing Settings From Config-PlanetaryCargoShips.xml. Phase: " + phase, SpawnerDebugEnum.Startup, true);
					return config;
					
				}catch(Exception exc){
					
					SpawnLogger.Write("ERROR: Could Not Load Settings From Config-PlanetaryCargoShips.xml. Using Default Configuration. Phase: " + phase, SpawnerDebugEnum.Error, true);
					var defaultSettings = new ConfigPlanetaryCargoShips();
					return defaultSettings;
					
				}

			} else {

				SpawnLogger.Write("Config-PlanetaryCargoShips.xml Doesn't Exist. Creating Default Configuration. Phase: " + phase, SpawnerDebugEnum.Startup, true);

			}

			var settings = new ConfigPlanetaryCargoShips();
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-PlanetaryCargoShips.xml", typeof(ConfigPlanetaryCargoShips))){
				
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigPlanetaryCargoShips>(settings));
				
				}
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Create Config-PlanetaryCargoShips.xml. Default Settings Will Be Used. Phase: " + phase, SpawnerDebugEnum.Error, true);
				
			}
			
			return settings;
			
		}
		
		public string SaveThisSettings(ConfigPlanetaryCargoShips settings){
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-PlanetaryCargoShips.xml", typeof(ConfigPlanetaryCargoShips))){
					
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigPlanetaryCargoShips>(settings));
				
				}
				
				SpawnLogger.Write("Settings In Config-PlanetaryCargoShips.xml Updated Successfully!", SpawnerDebugEnum.Settings);
				return "Settings Updated Successfully.";
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Save To Config-PlanetaryCargoShips.xml. Changes Will Be Lost On World Reload.", SpawnerDebugEnum.Settings);
				
			}
			
			return "Settings Changed, But Could Not Be Saved To XML. Changes May Be Lost On Session Reload.";
			
		}

		public string EditFields(string receivedCommand) {

			var commandSplit = receivedCommand.Split('.');

			if (commandSplit.Length < 5)
				return "Provided Command Missing Parameters.";

			Func<string, object, bool> referenceMethod = null;

			if (!EditorReference.TryGetValue(commandSplit[3], out referenceMethod))
				if (!EditorBaseReference.TryGetValue(commandSplit[3], out referenceMethod))
					return "Provided Field [" + commandSplit[3] + "] Does Not Exist.";

			if (!referenceMethod?.Invoke(receivedCommand, null) ?? false)
				return "Provided Value For [" + commandSplit[3] + "] Could Not Be Parsed.";

			InitDefinitionDisableList();
			return SaveThisSettings(this);

		}

	}
	
}