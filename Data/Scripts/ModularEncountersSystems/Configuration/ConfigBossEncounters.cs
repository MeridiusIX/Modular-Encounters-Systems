using ModularEncountersSystems.Configuration.Editor;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ModularEncountersSystems.Configuration {

	//BossEncounters

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

	public class ConfigBossEncounters : ConfigBase, IGridSpawn {

		public bool EnableSpawns;
		public int PlayerSpawnCooldown;
		public int SpawnTimerTrigger;
		public int SignalActiveTimer;
		public int MaxShipsPerArea;
		public double AreaSize;
		public double TriggerDistance;
		public int PathCalculationAttempts;
		public double MinCoordsDistanceSpace;
		public double MaxCoordsDistanceSpace;
		public double MinCoordsDistancePlanet;
		public double MaxCoordsDistancePlanet;
		public double PlayersWithinDistance;
		public double MinPlanetAltitude;
		public double MinSignalDistFromOtherEntities;
		public double MinSpawnDistFromCoords;
		public double MaxSpawnDistFromCoords;
		public float MinAirDensity;
		
		public bool UseMaxSpawnGroupFrequency;
		public int MaxSpawnGroupFrequency;
		
		public double DespawnDistanceFromPlayer;

		[XmlIgnore]
		public Dictionary<string, Func<string, object, bool>> EditorReference;



		public ConfigBossEncounters(){

			EnableSpawns = true;
			PlayerSpawnCooldown = 600;
			SpawnTimerTrigger = 1200;
			SignalActiveTimer = 1200;
			MaxShipsPerArea = 6;
			AreaSize = 25000;
			TriggerDistance = 300;
			PathCalculationAttempts = 25;
			MinCoordsDistanceSpace = 6000;
			MaxCoordsDistanceSpace = 8000;
			MinCoordsDistancePlanet = 6000;
			MaxCoordsDistancePlanet = 8000;
			PlayersWithinDistance = 25000;
			MinPlanetAltitude = 1500;
			MinSignalDistFromOtherEntities = 2000;
			MinSpawnDistFromCoords = 2500;
			MaxSpawnDistFromCoords = 4000;
			MinAirDensity = 0.65f;
			
			UseMaxSpawnGroupFrequency = false;
			MaxSpawnGroupFrequency = 5;
			
			DespawnDistanceFromPlayer = 1000;

			UseTimeout = false;
			TimeoutDuration = 900;
			TimeoutRadius = 10000;
			TimeoutSpawnLimit = 4;

			UseCleanupSettings = true;
			CleanupUseDistance = true;
			CleanupUseTimer = false;
			CleanupUseBlockLimit = false;
			CleanupDistanceStartsTimer = false;
			CleanupResetTimerWithinDistance = false;
			CleanupDistanceTrigger = 50000;
			CleanupTimerTrigger = 1800;
			CleanupBlockLimitTrigger = 0;
			CleanupIncludeUnowned = true;
			CleanupUnpoweredOverride = true;
			CleanupUnpoweredDistanceTrigger = 25000;
			CleanupUnpoweredTimerTrigger = 900;

			EditorReference = new Dictionary<string, Func<string, object, bool>> {

				{"EnableSpawns", (s, o) => EditorTools.SetCommandValueBool(s, ref EnableSpawns) },
				{"PlayerSpawnCooldown", (s, o) => EditorTools.SetCommandValueInt(s, ref PlayerSpawnCooldown) },
				{"SpawnTimerTrigger", (s, o) => EditorTools.SetCommandValueInt(s, ref SpawnTimerTrigger) },
				{"SignalActiveTimer", (s, o) => EditorTools.SetCommandValueInt(s, ref SignalActiveTimer) },
				{"MaxShipsPerArea", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxShipsPerArea) },
				{"AreaSize", (s, o) => EditorTools.SetCommandValueDouble(s, ref AreaSize) },
				{"TriggerDistance", (s, o) => EditorTools.SetCommandValueDouble(s, ref TriggerDistance) },
				{"PathCalculationAttempts", (s, o) => EditorTools.SetCommandValueInt(s, ref PathCalculationAttempts) },
				{"MinCoordsDistanceSpace", (s, o) => EditorTools.SetCommandValueDouble(s, ref MinCoordsDistanceSpace) },
				{"MaxCoordsDistanceSpace", (s, o) => EditorTools.SetCommandValueDouble(s, ref MaxCoordsDistanceSpace) },
				{"MinCoordsDistancePlanet", (s, o) => EditorTools.SetCommandValueDouble(s, ref MinCoordsDistancePlanet) },
				{"MaxCoordsDistancePlanet", (s, o) => EditorTools.SetCommandValueDouble(s, ref MaxCoordsDistancePlanet) },
				{"PlayersWithinDistance", (s, o) => EditorTools.SetCommandValueDouble(s, ref PlayersWithinDistance) },
				{"MinPlanetAltitude", (s, o) => EditorTools.SetCommandValueDouble(s, ref MinPlanetAltitude) },
				{"MinSignalDistFromOtherEntities", (s, o) => EditorTools.SetCommandValueDouble(s, ref MinSignalDistFromOtherEntities) },
				{"MinSpawnDistFromCoords", (s, o) => EditorTools.SetCommandValueDouble(s, ref MinSpawnDistFromCoords) },
				{"MaxSpawnDistFromCoords", (s, o) => EditorTools.SetCommandValueDouble(s, ref MaxSpawnDistFromCoords) },
				{"MinAirDensity", (s, o) => EditorTools.SetCommandValueFloat(s, ref MinAirDensity) },
				{"UseMaxSpawnGroupFrequency", (s, o) => EditorTools.SetCommandValueBool(s, ref UseMaxSpawnGroupFrequency) },
				{"MaxSpawnGroupFrequency", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxSpawnGroupFrequency) },
				{"DespawnDistanceFromPlayer", (s, o) => EditorTools.SetCommandValueDouble(s, ref DespawnDistanceFromPlayer) }

			};

		}
		
		public ConfigBossEncounters LoadSettings(string phase){

			if (MyAPIGateway.Utilities.FileExistsInWorldStorage("Config-BossEncounters.xml", typeof(ConfigBossEncounters)) == true) {

				try {

					ConfigBossEncounters config = null;
					var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage("Config-BossEncounters.xml", typeof(ConfigBossEncounters));
					string configcontents = reader.ReadToEnd();
					config = MyAPIGateway.Utilities.SerializeFromXML<ConfigBossEncounters>(configcontents);
					config.ConfigLoaded = true;
					SpawnLogger.Write("Loaded Existing Settings From Config-BossEncounters.xml. Phase: " + phase, SpawnerDebugEnum.Startup, true);
					return config;

				} catch (Exception exc) {

					SpawnLogger.Write("ERROR: Could Not Load Settings From Config-BossEncounters.xml. Using Default Configuration. Phase: " + phase, SpawnerDebugEnum.Error, true);
					var defaultSettings = new ConfigBossEncounters();
					return defaultSettings;

				}

			} else {

				SpawnLogger.Write("Config-BossEncounters.xml Doesn't Exist. Creating Default Configuration. Phase: " + phase, SpawnerDebugEnum.Startup, true);

			}
			
			var settings = new ConfigBossEncounters();
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-BossEncounters.xml", typeof(ConfigBossEncounters))){
				
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigBossEncounters>(settings));
				
				}
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Create Config-BossEncounters.xml. Default Settings Will Be Used. Phase: " + phase, SpawnerDebugEnum.Error, true);
				
			}
			
			return settings;
			
		}
		
		public override string SaveSettings(){
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-BossEncounters.xml", typeof(ConfigBossEncounters))){
					
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigBossEncounters>(this));
				
				}
				
				SpawnLogger.Write("Settings In Config-BossEncounters.xml Updated Successfully!", SpawnerDebugEnum.Settings);

				return "Settings Updated Successfully.";
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Save To Config-BossEncounters.xml. Changes Will Be Lost On World Reload.", SpawnerDebugEnum.Settings);
				
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
			return SaveSettings();

		}

	}
		
}