using System;
using Sandbox.ModAPI;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Configuration.Editor;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ModularEncountersSystems.Configuration {

	//ConfigDroneEncounters

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

	public class ConfigDroneEncounters : ConfigBase, IGridSpawn {
		
		public string ModVersion;
		public bool EnableSpawns;

		public int PlayerSpawnCooldown;
		public int SpawnTimerTrigger;
		public int MaxSpawnAttempts;
		public double MinDistanceFromOtherEntities;

		[XmlIgnore]
		public Dictionary<string, Func<string, object, bool>> EditorReference;

		public ConfigDroneEncounters(){
			
			ModVersion = MES_SessionCore.ModVersion;
			EnableSpawns = true;

			PlayerSpawnCooldown = 0;
			SpawnTimerTrigger = 30;
			MaxSpawnAttempts = 5;
			MinDistanceFromOtherEntities = 800;

			UseTimeout = true;
			TimeoutDuration = 900;
			TimeoutRadius = 10000;
			TimeoutSpawnLimit = 2;

			UseCleanupSettings = true;
			CleanupUseDistance = true;
			CleanupUseTimer = false;
			CleanupUseBlockLimit = false;
			CleanupDistanceStartsTimer = false;
			CleanupResetTimerWithinDistance = false;
			CleanupDistanceTrigger = 30000;
			CleanupTimerTrigger = 1800;
			CleanupBlockLimitTrigger = 0;
			CleanupIncludeUnowned = true;
			CleanupUnpoweredOverride = true;
			CleanupUnpoweredDistanceTrigger = 20000;
			CleanupUnpoweredTimerTrigger = 900;

			EditorReference = new Dictionary<string, Func<string, object, bool>> {

				{"EnableSpawns", (s, o) => EditorTools.SetCommandValueBool(s, ref EnableSpawns) },
				{"PlayerSpawnCooldown", (s, o) => EditorTools.SetCommandValueInt(s, ref PlayerSpawnCooldown) },
				{"SpawnTimerTrigger", (s, o) => EditorTools.SetCommandValueInt(s, ref SpawnTimerTrigger) },
				{"MaxSpawnAttempts", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxSpawnAttempts) },
				{"MinDistanceFromOtherEntities", (s, o) => EditorTools.SetCommandValueDouble(s, ref MinDistanceFromOtherEntities) },

			};

		}
		
		public ConfigDroneEncounters LoadSettings(string phase) {
			
			if(MyAPIGateway.Utilities.FileExistsInWorldStorage("Config-ConfigDroneEncounters.xml", typeof(ConfigDroneEncounters)) == true){
				
				try{
					
					ConfigDroneEncounters config = null;
					var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage("Config-ConfigDroneEncounters.xml", typeof(ConfigDroneEncounters));
					string configcontents = reader.ReadToEnd();
					config = MyAPIGateway.Utilities.SerializeFromXML<ConfigDroneEncounters>(configcontents);
					config.ConfigLoaded = true;
					SpawnLogger.Write("Loaded Existing Settings From Config-ConfigDroneEncounters.xml. Phase: " + phase, SpawnerDebugEnum.Startup, true);
					return config;
					
				}catch(Exception exc){
					
					SpawnLogger.Write("ERROR: Could Not Load Settings From Config-ConfigDroneEncounters.xml. Using Default Configuration. Phase: " + phase, SpawnerDebugEnum.Error, true);
					var defaultSettings = new ConfigDroneEncounters();
					return defaultSettings;
					
				}

			} else {

				SpawnLogger.Write("Config-ConfigDroneEncounters.xml Doesn't Exist. Creating Default Configuration. Phase: " + phase, SpawnerDebugEnum.Startup, true);

			}

			var settings = new ConfigDroneEncounters();
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-ConfigDroneEncounters.xml", typeof(ConfigDroneEncounters))){
				
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigDroneEncounters>(settings));
				
				}
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Create Config-ConfigDroneEncounters.xml. Default Settings Will Be Used. Phase: " + phase, SpawnerDebugEnum.Error, true);
				
			}
			
			return settings;
			
		}

		public override string SaveSettings() {

			try {

				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-ConfigDroneEncounters.xml", typeof(ConfigDroneEncounters))) {

					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigDroneEncounters>(this));

				}

				SpawnLogger.Write("Settings In Config-ConfigDroneEncounters.xml Updated Successfully!", SpawnerDebugEnum.Settings);
				return "Settings Updated Successfully.";

			} catch (Exception exc) {

				SpawnLogger.Write("ERROR: Could Not Save To Config-ConfigDroneEncounters.xml. Changes Will Be Lost On World Reload.", SpawnerDebugEnum.Settings);

			}

			return "Settings Changed, But Could Not Be Saved To XML. Changes May Be Lost On Session Reload.";

		}

		public string EditFields(string receivedCommand) {

			var commandSplit = receivedCommand.Split('.');

			if (commandSplit.Length < 5)
				return "Provided Command Missing Parameters.";

			Func<string, object, bool> referenceMethod = null;

			if (!EditorReference.TryGetValue(commandSplit[3], out referenceMethod))
				if(!EditorBaseReference.TryGetValue(commandSplit[3], out referenceMethod))
					return "Provided Field [" + commandSplit[3] + "] Does Not Exist.";

			if (!referenceMethod?.Invoke(receivedCommand, null) ?? false)
				return "Provided Value For [" + commandSplit[3] + "] Could Not Be Parsed.";

			InitDefinitionDisableList();
			return SaveSettings();

		}

	}
	
}