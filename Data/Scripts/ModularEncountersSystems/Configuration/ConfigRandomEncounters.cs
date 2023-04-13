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

	public class ConfigRandomEncounters : ConfigBase, IGridSpawn {
		
		public string ModVersion;
		public bool EnableSpawns;
		public int PlayerSpawnCooldown;
		public int SpawnTimerTrigger;
		public double PlayerTravelDistance;
		public int MaxShipsPerArea;
		public double AreaSize;
		public double MinSpawnDistanceFromPlayer;
		public double MaxSpawnDistanceFromPlayer;
		public double MinDistanceFromOtherEntities;
		public bool RemoveVoxelsIfGridRemoved;
		public int SpawnAttempts;
		
		public bool UseMaxSpawnGroupFrequency;
		public int MaxSpawnGroupFrequency;
		
		public double DespawnDistanceFromPlayer;

		[XmlIgnore]
		public Dictionary<string, Func<string, object, bool>> EditorReference;

		public ConfigRandomEncounters(){
			
			ModVersion = MES_SessionCore.ModVersion;
			EnableSpawns = true;
			PlayerSpawnCooldown = 300;
			SpawnTimerTrigger = 60;
			PlayerTravelDistance = 15000;
			MaxShipsPerArea = 10;
			AreaSize = 25000;
			MinSpawnDistanceFromPlayer = 8000;
			MaxSpawnDistanceFromPlayer = 12000;
			MinDistanceFromOtherEntities = 3000;
			RemoveVoxelsIfGridRemoved = true;
			SpawnAttempts = 10;
			
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
			CleanupResetTimerWithinDistance = false;
			CleanupDistanceTrigger = 50000;
			CleanupTimerTrigger = 3600;
			CleanupBlockLimitTrigger = 0;
			CleanupIncludeUnowned = true;
			CleanupUnpoweredOverride = false;
			CleanupUnpoweredDistanceTrigger = 25000;
			CleanupUnpoweredTimerTrigger = 900;

			EditorReference = new Dictionary<string, Func<string, object, bool>> {

				{"EnableSpawns", (s, o) => EditorTools.SetCommandValueBool(s, ref EnableSpawns) },
				{"PlayerSpawnCooldown", (s, o) => EditorTools.SetCommandValueInt(s, ref PlayerSpawnCooldown) },
				{"SpawnTimerTrigger", (s, o) => EditorTools.SetCommandValueInt(s, ref SpawnTimerTrigger) },
				{"PlayerTravelDistance", (s, o) => EditorTools.SetCommandValueDouble(s, ref PlayerTravelDistance) },
				{"MaxShipsPerArea", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxShipsPerArea) },
				{"AreaSize", (s, o) => EditorTools.SetCommandValueDouble(s, ref AreaSize) },
				{"MinSpawnDistanceFromPlayer", (s, o) => EditorTools.SetCommandValueDouble(s, ref MinSpawnDistanceFromPlayer) },
				{"MaxSpawnDistanceFromPlayer", (s, o) => EditorTools.SetCommandValueDouble(s, ref MaxSpawnDistanceFromPlayer) },
				{"MinDistanceFromOtherEntities", (s, o) => EditorTools.SetCommandValueDouble(s, ref MinDistanceFromOtherEntities) },
				{"RemoveVoxelsIfGridRemoved", (s, o) => EditorTools.SetCommandValueBool(s, ref RemoveVoxelsIfGridRemoved) },
				{"SpawnAttempts", (s, o) => EditorTools.SetCommandValueInt(s, ref SpawnAttempts) },
				{"UseMaxSpawnGroupFrequency", (s, o) => EditorTools.SetCommandValueBool(s, ref UseMaxSpawnGroupFrequency) },
				{"MaxSpawnGroupFrequency", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxSpawnGroupFrequency) },
				{"DespawnDistanceFromPlayer", (s, o) => EditorTools.SetCommandValueDouble(s, ref DespawnDistanceFromPlayer) },

			};

		}
		
		public ConfigRandomEncounters LoadSettings(string phase) {
			
			if(MyAPIGateway.Utilities.FileExistsInWorldStorage("Config-RandomEncounters.xml", typeof(ConfigRandomEncounters)) == true){
				
				try{
					
					ConfigRandomEncounters config = null;
					var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage("Config-RandomEncounters.xml", typeof(ConfigRandomEncounters));
					string configcontents = reader.ReadToEnd();
					config = MyAPIGateway.Utilities.SerializeFromXML<ConfigRandomEncounters>(configcontents);
					config.ConfigLoaded = true;
					SpawnLogger.Write("Loaded Existing Settings From Config-RandomEncounters.xml. Phase: " + phase, SpawnerDebugEnum.Startup, true);
					return config;
					
				}catch(Exception exc){
					
					SpawnLogger.Write("ERROR: Could Not Load Settings From Config-RandomEncounters.xml. Using Default Configuration. Phase: " + phase, SpawnerDebugEnum.Error, true);
					var defaultSettings = new ConfigRandomEncounters();
					return defaultSettings;
					
				}

			} else {

				SpawnLogger.Write("Config-RandomEncounters.xml Doesn't Exist. Creating Default Configuration. Phase: " + phase, SpawnerDebugEnum.Startup, true);

			}

			var settings = new ConfigRandomEncounters();
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-RandomEncounters.xml", typeof(ConfigRandomEncounters))){
				
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigRandomEncounters>(settings));
				
				}
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Create Config-RandomEncounters.xml. Default Settings Will Be Used. Phase: " + phase, SpawnerDebugEnum.Error, true);
				
			}
			
			return settings;
			
		}
		
		public override string SaveSettings(){
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-RandomEncounters.xml", typeof(ConfigRandomEncounters))){
					
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigRandomEncounters>(this));
				
				}
				
				SpawnLogger.Write("Settings In Config-RandomEncounters.xml Updated Successfully!", SpawnerDebugEnum.Settings);
				return "Settings Updated Successfully.";
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Save To Config-RandomEncounters.xml. Changes Will Be Lost On World Reload.", SpawnerDebugEnum.Settings);
				
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