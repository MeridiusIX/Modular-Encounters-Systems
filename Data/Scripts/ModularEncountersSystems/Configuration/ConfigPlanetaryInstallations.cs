using ModularEncountersSystems.Configuration.Editor;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ModularEncountersSystems.Configuration {

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

	public class ConfigPlanetaryInstallations : ConfigBase, IGridSpawn {
		
		public string ModVersion;
		public bool EnableSpawns;
		public int PlayerSpawnCooldown;
		public int SpawnTimerTrigger;
		public double PlayerDistanceSpawnTrigger;
		public int MaxShipsPerArea;
		public double AreaSize;
		public double PlayerMaximumDistanceFromSurface;
		public double MinimumSpawnDistanceFromPlayers;
		public double MaximumSpawnDistanceFromPlayers;
		public bool AggressivePathCheck;
		public double SearchPathIncrement;
		
		public double MinimumSpawnDistanceFromOtherGrids;
		public double MinimumTerrainVariance;
		public double MaximumTerrainVariance;
		public bool AggressiveTerrainCheck;
		public double TerrainCheckIncrementDistance;
		
		public double SmallTerrainCheckDistance;
		
		public int MediumSpawnChanceBaseValue;
		public int MediumSpawnChanceIncrement;
		public double MediumSpawnDistanceIncrement;
		public double MediumTerrainCheckDistance;
		
		public int LargeSpawnChanceBaseValue;
		public int LargeSpawnChanceIncrement;
		public double LargeSpawnDistanceIncrement;
		public double LargeTerrainCheckDistance;

		public bool RemoveVoxelsIfGridRemoved;

		public bool UseMaxSpawnGroupFrequency;
		public int MaxSpawnGroupFrequency;
		
		public double DespawnDistanceFromPlayer;

		[XmlIgnore]
		public Dictionary<string, Func<string, object, bool>> EditorReference;

		public ConfigPlanetaryInstallations(){
			
			ModVersion = MES_SessionCore.ModVersion;
			EnableSpawns = true;
			PlayerSpawnCooldown = 300;
			SpawnTimerTrigger = 60;
			PlayerDistanceSpawnTrigger = 6000;
			MaxShipsPerArea = 10;
			AreaSize = 15000;
			PlayerMaximumDistanceFromSurface = 6000;
			MinimumSpawnDistanceFromPlayers = 3000;
			MaximumSpawnDistanceFromPlayers = 6000;
			AggressivePathCheck = true;
			SearchPathIncrement = 150;
			
			MinimumSpawnDistanceFromOtherGrids = 2500;
			MinimumTerrainVariance = -2.5;
			MaximumTerrainVariance = 2.5;
			AggressiveTerrainCheck = true;
			TerrainCheckIncrementDistance = 10;
			
			SmallTerrainCheckDistance = 40;
			
			MediumSpawnChanceBaseValue = 15;
			MediumSpawnChanceIncrement = 15;
			MediumSpawnDistanceIncrement = 2000;
			MediumTerrainCheckDistance = 70;
			
			LargeSpawnChanceBaseValue = 5;
			LargeSpawnChanceIncrement = 15;
			LargeSpawnDistanceIncrement = 4000;
			LargeTerrainCheckDistance = 100;

			RemoveVoxelsIfGridRemoved = true;

			UseMaxSpawnGroupFrequency = false;
			MaxSpawnGroupFrequency = 5;
			
			DespawnDistanceFromPlayer = 1000;

			UseTimeout = true;
			TimeoutDuration = 900;
			TimeoutRadius = 25000;
			TimeoutSpawnLimit = 2;

			UseCleanupSettings = true;
			CleanupUseDistance = true;
			CleanupUseTimer = true;
			CleanupUseBlockLimit = false;
			CleanupDistanceStartsTimer = true;
			CleanupResetTimerWithinDistance = true;
			CleanupDistanceTrigger = 50000;
			CleanupTimerTrigger = 3600;
			CleanupBlockLimitTrigger = 0;
			CleanupIncludeUnowned = true;
			CleanupUnpoweredOverride = true;
			CleanupUnpoweredDistanceTrigger = 25000;
			CleanupUnpoweredTimerTrigger = 900;

			EditorReference = new Dictionary<string, Func<string, object, bool>> {

				{"EnableSpawns", (s, o) => EditorTools.SetCommandValueBool(s, ref EnableSpawns) },
				{"PlayerSpawnCooldown", (s, o) => EditorTools.SetCommandValueInt(s, ref PlayerSpawnCooldown) },
				{"SpawnTimerTrigger", (s, o) => EditorTools.SetCommandValueInt(s, ref SpawnTimerTrigger) },
				{"PlayerDistanceSpawnTrigger", (s, o) => EditorTools.SetCommandValueDouble(s, ref PlayerDistanceSpawnTrigger) },
				{"MaxShipsPerArea", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxShipsPerArea) },
				{"AreaSize", (s, o) => EditorTools.SetCommandValueDouble(s, ref AreaSize) },
				{"PlayerMaximumDistanceFromSurface", (s, o) => EditorTools.SetCommandValueDouble(s, ref PlayerMaximumDistanceFromSurface) },
				{"MinimumSpawnDistanceFromPlayers", (s, o) => EditorTools.SetCommandValueDouble(s, ref MinimumSpawnDistanceFromPlayers) },
				{"MaximumSpawnDistanceFromPlayers", (s, o) => EditorTools.SetCommandValueDouble(s, ref MaximumSpawnDistanceFromPlayers) },
				{"AggressivePathCheck", (s, o) => EditorTools.SetCommandValueBool(s, ref AggressivePathCheck) },
				{"SearchPathIncrement", (s, o) => EditorTools.SetCommandValueDouble(s, ref SearchPathIncrement) },
				{"MinimumSpawnDistanceFromOtherGrids", (s, o) => EditorTools.SetCommandValueDouble(s, ref MinimumSpawnDistanceFromOtherGrids) },
				{"MinimumTerrainVariance", (s, o) => EditorTools.SetCommandValueDouble(s, ref MinimumTerrainVariance) },
				{"MaximumTerrainVariance", (s, o) => EditorTools.SetCommandValueDouble(s, ref MaximumTerrainVariance) },
				{"AggressiveTerrainCheck", (s, o) => EditorTools.SetCommandValueBool(s, ref AggressiveTerrainCheck) },
				{"TerrainCheckIncrementDistance", (s, o) => EditorTools.SetCommandValueDouble(s, ref TerrainCheckIncrementDistance) },
				{"SmallTerrainCheckDistance", (s, o) => EditorTools.SetCommandValueDouble(s, ref SmallTerrainCheckDistance) },
				{"MediumSpawnChanceBaseValue", (s, o) => EditorTools.SetCommandValueInt(s, ref MediumSpawnChanceBaseValue) },
				{"MediumSpawnChanceIncrement", (s, o) => EditorTools.SetCommandValueInt(s, ref MediumSpawnChanceIncrement) },
				{"MediumSpawnDistanceIncrement", (s, o) => EditorTools.SetCommandValueDouble(s, ref MediumSpawnDistanceIncrement) },
				{"MediumTerrainCheckDistance", (s, o) => EditorTools.SetCommandValueDouble(s, ref MediumTerrainCheckDistance) },
				{"LargeSpawnChanceBaseValue", (s, o) => EditorTools.SetCommandValueInt(s, ref LargeSpawnChanceBaseValue) },
				{"LargeSpawnChanceIncrement", (s, o) => EditorTools.SetCommandValueInt(s, ref LargeSpawnChanceIncrement) },
				{"LargeSpawnDistanceIncrement", (s, o) => EditorTools.SetCommandValueDouble(s, ref LargeSpawnDistanceIncrement) },
				{"LargeTerrainCheckDistance", (s, o) => EditorTools.SetCommandValueDouble(s, ref LargeTerrainCheckDistance) },
				{"RemoveVoxelsIfGridRemoved", (s, o) => EditorTools.SetCommandValueBool(s, ref RemoveVoxelsIfGridRemoved) },
				{"UseMaxSpawnGroupFrequency", (s, o) => EditorTools.SetCommandValueBool(s, ref UseMaxSpawnGroupFrequency) },
				{"MaxSpawnGroupFrequency", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxSpawnGroupFrequency) },
				{"DespawnDistanceFromPlayer", (s, o) => EditorTools.SetCommandValueDouble(s, ref DespawnDistanceFromPlayer) },

			};

		}
		
		public ConfigPlanetaryInstallations LoadSettings(string phase) {
			
			if(MyAPIGateway.Utilities.FileExistsInWorldStorage("Config-PlanetaryInstallations.xml", typeof(ConfigPlanetaryInstallations)) == true){
				
				try{
					
					ConfigPlanetaryInstallations config = null;
					var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage("Config-PlanetaryInstallations.xml", typeof(ConfigPlanetaryInstallations));
					string configcontents = reader.ReadToEnd();
					config = MyAPIGateway.Utilities.SerializeFromXML<ConfigPlanetaryInstallations>(configcontents);
					config.ConfigLoaded = true;
					SpawnLogger.Write("Loaded Existing Settings From Config-PlanetaryInstallations.xml. Phase: " + phase, SpawnerDebugEnum.Startup, true);
					return config;
					
				}catch(Exception exc){
					
					SpawnLogger.Write("ERROR: Could Not Load Settings From Config-PlanetaryInstallations.xml. Using Default Configuration. Phase: " + phase, SpawnerDebugEnum.Error, true);
					var defaultSettings = new ConfigPlanetaryInstallations();
					return defaultSettings;
					
				}

			} else {

				SpawnLogger.Write("Config-PlanetaryInstallations.xml Doesn't Exist. Creating Default Configuration. Phase: " + phase, SpawnerDebugEnum.Startup, true);

			}

			var settings = new ConfigPlanetaryInstallations();
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-PlanetaryInstallations.xml", typeof(ConfigPlanetaryInstallations))){
				
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigPlanetaryInstallations>(settings));
				
				}
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Create Config-PlanetaryInstallations.xml. Default Settings Will Be Used. Phase: " + phase, SpawnerDebugEnum.Error, true);
				
			}
			
			return settings;
			
		}
		
		public string SaveThisSettings(ConfigPlanetaryInstallations settings){
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-PlanetaryInstallations.xml", typeof(ConfigPlanetaryInstallations))){
					
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigPlanetaryInstallations>(settings));
				
				}
				
				SpawnLogger.Write("Settings In Config-PlanetaryInstallations.xml Updated Successfully!", SpawnerDebugEnum.Settings);
				return "Settings Updated Successfully.";
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Save To Config-PlanetaryInstallations.xml. Changes Will Be Lost On World Reload.", SpawnerDebugEnum.Settings);
				
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