using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using ModularEncountersSystems.Configuration.Editor;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using VRage.Game;

namespace ModularEncountersSystems.Configuration {

	//Creatures

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

	public class ConfigCreatures{

		public string ModVersion;
		public bool EnableSpawns;

		public bool OverrideVanillaCreatureSpawns;

		public int MinCreatureSpawnTime;
		public int MaxCreatureSpawnTime;

		public double MaxPlayerAltitudeForSpawn;

		public int CoordsAttemptsPerCreature;

		public string[] SpawnTypeBlacklist;
		public string[] SpawnTypePlanetBlacklist;

		public bool UseTimeout;
		public double TimeoutRadius;
		public int TimeoutSpawnLimit;
		public int TimeoutDuration;

		public bool EnableWaveSpawner;
		public string[] UseSpecificRandomGroups;
		public int MinWaveSpawnTime;
		public int MaxWaveSpawnTime;
		public int TotalSpawnEventsPerCluster;
		public int TimeBetweenWaveSpawns;
		public double PlayerClusterDistance;

		[XmlIgnore]
		public bool ConfigLoaded;

		[XmlIgnore]
		public Dictionary<string, Func<string, object, bool>> EditorReference;

		public ConfigCreatures(){
			
			ModVersion = MES_SessionCore.ModVersion;
			EnableSpawns = true;

			OverrideVanillaCreatureSpawns = false;

			MinCreatureSpawnTime = 900;
			MaxCreatureSpawnTime = 1200;

			MaxPlayerAltitudeForSpawn = 150;

			CoordsAttemptsPerCreature = 10;

			SpawnTypeBlacklist = new string[] { "SpawnGroupSubtypeIdHere", "SpawnGroupSubtypeIdHere" };
			SpawnTypePlanetBlacklist = new string[] { "PlanetSubtypeIdHere", "AnotherPlanetSubtypeId" };

			UseTimeout = true;
			TimeoutDuration = 900;
			TimeoutRadius = 5000;
			TimeoutSpawnLimit = 12;

			EnableWaveSpawner = false;
			UseSpecificRandomGroups = new string[] { "SomeSpawnGroupNameHere", "AnotherSpawnGroupNameHere", "EtcEtcEtc" };
			MinWaveSpawnTime = 1980;
			MaxWaveSpawnTime = 3600;
			TotalSpawnEventsPerCluster = 6;
			TimeBetweenWaveSpawns = 8;
			PlayerClusterDistance = 15000;

			EditorReference = new Dictionary<string, Func<string, object, bool>> {

				{"EnableSpawns", (s, o) => EditorTools.SetCommandValueBool(s, ref EnableSpawns) },
				{"OverrideVanillaCreatureSpawns", (s, o) => EditorTools.SetCommandValueBool(s, ref OverrideVanillaCreatureSpawns) },
				{"MinCreatureSpawnTime", (s, o) => EditorTools.SetCommandValueInt(s, ref MinCreatureSpawnTime) },
				{"MaxCreatureSpawnTime", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxCreatureSpawnTime) },
				{"MaxPlayerAltitudeForSpawn", (s, o) => EditorTools.SetCommandValueDouble(s, ref MaxPlayerAltitudeForSpawn) },
				{"CoordsAttemptsPerCreature", (s, o) => EditorTools.SetCommandValueInt(s, ref CoordsAttemptsPerCreature) },
				{"SpawnTypeBlacklist", (s, o) => EditorTools.SetCommandValueStringArray(s, ref SpawnTypeBlacklist) },
				{"SpawnTypePlanetBlacklist", (s, o) => EditorTools.SetCommandValueStringArray(s, ref SpawnTypePlanetBlacklist) },
				{"UseTimeout", (s, o) => EditorTools.SetCommandValueBool(s, ref UseTimeout) },
				{"TimeoutDuration", (s, o) => EditorTools.SetCommandValueInt(s, ref TimeoutDuration) },
				{"TimeoutRadius", (s, o) => EditorTools.SetCommandValueDouble(s, ref TimeoutRadius) },
				{"TimeoutSpawnLimit", (s, o) => EditorTools.SetCommandValueInt(s, ref TimeoutSpawnLimit) },
				{"EnableWaveSpawner", (s, o) => EditorTools.SetCommandValueBool(s, ref EnableWaveSpawner) },
				{"UseSpecificRandomGroups", (s, o) => EditorTools.SetCommandValueStringArray(s, ref UseSpecificRandomGroups) },
				{"MinWaveSpawnTime", (s, o) => EditorTools.SetCommandValueInt(s, ref MinWaveSpawnTime) },
				{"MaxWaveSpawnTime", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxWaveSpawnTime) },
				{"TotalSpawnEventsPerCluster", (s, o) => EditorTools.SetCommandValueInt(s, ref TotalSpawnEventsPerCluster) },
				{"TimeBetweenWaveSpawns", (s, o) => EditorTools.SetCommandValueInt(s, ref TimeBetweenWaveSpawns) },
				{"PlayerClusterDistance", (s, o) => EditorTools.SetCommandValueDouble(s, ref PlayerClusterDistance) },

			};


		}

		public ConfigCreatures LoadSettings(string phase) {
			
			if(MyAPIGateway.Utilities.FileExistsInWorldStorage("Config-Creatures.xml", typeof(ConfigCreatures)) == true){
				
				try{
					
					ConfigCreatures config = null;
					var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage("Config-Creatures.xml", typeof(ConfigCreatures));
					string configcontents = reader.ReadToEnd();
					config = MyAPIGateway.Utilities.SerializeFromXML<ConfigCreatures>(configcontents);
					config.ConfigLoaded = true;
					SpawnLogger.Write("Loaded Existing Settings From Config-Creatures.xml. Phase: " + phase, SpawnerDebugEnum.Startup, true);
					return config;
					
				}catch(Exception exc){
					
					SpawnLogger.Write("ERROR: Could Not Load Settings From Config-Creatures.xml. Using Default Configuration. Phase: " + phase, SpawnerDebugEnum.Error, true);
					var defaultSettings = new ConfigCreatures();
					return defaultSettings;
					
				}

			} else {

				SpawnLogger.Write("Config-Creatures.xml Doesn't Exist. Creating Default Configuration. Phase: " + phase, SpawnerDebugEnum.Startup, true);

			}

			var settings = new ConfigCreatures();
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-Creatures.xml", typeof(ConfigCreatures))){
				
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigCreatures>(settings));
				
				}
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Create Config-Creatures.xml. Default Settings Will Be Used. Phase: " + phase, SpawnerDebugEnum.Error, true);
				
			}
			
			return settings;
			
		}
		
		public string SaveSettings(){
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-Creatures.xml", typeof(ConfigCreatures))){
					
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigCreatures>(this));
				
				}
				
				SpawnLogger.Write("Settings In Config-Creatures.xml Updated Successfully!", SpawnerDebugEnum.Settings);
				return "Settings Updated Successfully.";
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Save To Config-Creatures.xml. Changes Will Be Lost On World Reload.", SpawnerDebugEnum.Settings);
				
			}
			
			return "Settings Changed, But Could Not Be Saved To XML. Changes May Be Lost On Session Reload.";
			
		}

		public string EditFields(string receivedCommand) {

			var commandSplit = receivedCommand.Split('.');

			if (commandSplit.Length < 5)
				return "Provided Command Missing Parameters.";

			Func<string, object, bool> referenceMethod = null;

			if (!EditorReference.TryGetValue(commandSplit[3], out referenceMethod))
				return "Provided Field [" + commandSplit[3] + "] Does Not Exist.";

			if (!referenceMethod?.Invoke(receivedCommand, null) ?? false)
				return "Provided Value For [" + commandSplit[3] + "] Could Not Be Parsed.";

			return SaveSettings();

		}

	}
	
}