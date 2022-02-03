using ModularEncountersSystems.Configuration.Editor;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ModularEncountersSystems.Configuration {

	//General

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

	public class ConfigGeneral{
		
		public string ModVersion;

		public bool SpawnerDebug;
		public bool BehaviorDebug;

		public bool EnableLegacySpaceCargoShipDetection;
		
		public bool UseModIdSelectionForSpawning;
		public bool UseWeightedModIdSelection;
		public int LowWeightModIdSpawnGroups;
		public int LowWeightModIdModifier;
		public int MediumWeightModIdSpawnGroups;
		public int MediumWeightModIdModifier;
		public int HighWeightModIdSpawnGroups;
		public int HighWeightModIdModifier;
		
		public bool UseMaxNpcGrids;
		public bool UseGlobalEventsTimers;
		
		public bool IgnorePlanetWhitelists;
		public bool IgnorePlanetBlacklists;
		
		public int ThreatRefreshTimerMinimum;
		public int ThreatReductionHandicap;
		
		public int MaxGlobalNpcGrids;
		public int PlayerWatcherTimerTrigger;
		public int NpcDistanceCheckTimerTrigger;
		public int NpcOwnershipCheckTimerTrigger;
		public int NpcCleanupCheckTimerTrigger;
		public int NpcBlacklistCheckTimerTrigger;
		public int SpawnedVoxelCheckTimerTrigger;
		public double SpawnedVoxelMinimumGridDistance;
		public string[] PlanetSpawnsDisableList;
		public string[] NpcGridNameBlacklist;
		public string[] NpcSpawnGroupBlacklist;

		public bool UseEconomyBuyingReputationIncrease;
		public long EconomyBuyingReputationCostAmount;

		public int Difficulty;

		[XmlIgnore]
		public bool ConfigLoaded;

		[XmlIgnore]
		public Dictionary<string, Func<string, object, bool>> EditorReference;

		public ConfigGeneral(){
			
			ModVersion = MES_SessionCore.ModVersion;
			SpawnerDebug = false;
			BehaviorDebug = false;
			EnableLegacySpaceCargoShipDetection = true;
			UseModIdSelectionForSpawning = true;
			UseWeightedModIdSelection = true;
			LowWeightModIdSpawnGroups = 10;
			LowWeightModIdModifier = 1;
			MediumWeightModIdSpawnGroups = 19;
			MediumWeightModIdModifier = 2;
			HighWeightModIdSpawnGroups = 20;
			HighWeightModIdModifier = 3;
			UseMaxNpcGrids = false;
			UseGlobalEventsTimers = false;
			IgnorePlanetWhitelists = false;
			IgnorePlanetBlacklists = false;
			ThreatRefreshTimerMinimum = 20;
			ThreatReductionHandicap = 0;
			MaxGlobalNpcGrids = 50;
			PlayerWatcherTimerTrigger = 10;
			NpcDistanceCheckTimerTrigger = 1;
			NpcOwnershipCheckTimerTrigger = 10;
			NpcCleanupCheckTimerTrigger = 60;
			NpcBlacklistCheckTimerTrigger = 5;
			SpawnedVoxelCheckTimerTrigger = 900;
			SpawnedVoxelMinimumGridDistance = 1000;
			PlanetSpawnsDisableList = new string[]{"Planet_SubtypeId_Here", "Planet_SubtypeId_Here"};
			NpcGridNameBlacklist = new string[]{"BlackList_Grid_Name_Here", "BlackList_Grid_Name_Here"};
			NpcSpawnGroupBlacklist = new string[]{"BlackList_SpawnGroup_Here", "BlackList_SpawnGroup_Here"};
			UseEconomyBuyingReputationIncrease = true;
			EconomyBuyingReputationCostAmount = 500000;
			Difficulty = 1;

			EditorReference = new Dictionary<string, Func<string, object, bool>> {

				{"SpawnerDebug", (s, o) => EditorTools.SetCommandValueBool(s, ref SpawnerDebug) },
				{"BehaviorDebug", (s, o) => EditorTools.SetCommandValueBool(s, ref BehaviorDebug) },
				{"EnableLegacySpaceCargoShipDetection", (s, o) => EditorTools.SetCommandValueBool(s, ref EnableLegacySpaceCargoShipDetection) },
				{"UseModIdSelectionForSpawning", (s, o) => EditorTools.SetCommandValueBool(s, ref UseModIdSelectionForSpawning) },
				{"UseWeightedModIdSelection", (s, o) => EditorTools.SetCommandValueBool(s, ref UseWeightedModIdSelection) },
				{"LowWeightModIdSpawnGroups", (s, o) => EditorTools.SetCommandValueInt(s, ref LowWeightModIdSpawnGroups) },
				{"LowWeightModIdModifier", (s, o) => EditorTools.SetCommandValueInt(s, ref LowWeightModIdModifier) },
				{"MediumWeightModIdSpawnGroups", (s, o) => EditorTools.SetCommandValueInt(s, ref MediumWeightModIdSpawnGroups) },
				{"MediumWeightModIdModifier", (s, o) => EditorTools.SetCommandValueInt(s, ref MediumWeightModIdModifier) },
				{"HighWeightModIdSpawnGroups", (s, o) => EditorTools.SetCommandValueInt(s, ref HighWeightModIdSpawnGroups) },
				{"HighWeightModIdModifier", (s, o) => EditorTools.SetCommandValueInt(s, ref HighWeightModIdModifier) },
				{"UseMaxNpcGrids", (s, o) => EditorTools.SetCommandValueBool(s, ref UseMaxNpcGrids) },
				{"UseGlobalEventsTimers", (s, o) => EditorTools.SetCommandValueBool(s, ref UseGlobalEventsTimers) },
				{"IgnorePlanetWhitelists", (s, o) => EditorTools.SetCommandValueBool(s, ref IgnorePlanetWhitelists) },
				{"IgnorePlanetBlacklists", (s, o) => EditorTools.SetCommandValueBool(s, ref IgnorePlanetBlacklists) },
				{"ThreatRefreshTimerMinimum", (s, o) => EditorTools.SetCommandValueInt(s, ref ThreatRefreshTimerMinimum) },
				{"ThreatReductionHandicap", (s, o) => EditorTools.SetCommandValueInt(s, ref ThreatReductionHandicap) },
				{"MaxGlobalNpcGrids", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxGlobalNpcGrids) },
				{"PlayerWatcherTimerTrigger", (s, o) => EditorTools.SetCommandValueInt(s, ref PlayerWatcherTimerTrigger) },
				{"NpcDistanceCheckTimerTrigger", (s, o) => EditorTools.SetCommandValueInt(s, ref NpcDistanceCheckTimerTrigger) },
				{"NpcOwnershipCheckTimerTrigger", (s, o) => EditorTools.SetCommandValueInt(s, ref NpcOwnershipCheckTimerTrigger) },
				{"NpcCleanupCheckTimerTrigger", (s, o) => EditorTools.SetCommandValueInt(s, ref NpcCleanupCheckTimerTrigger) },
				{"NpcBlacklistCheckTimerTrigger", (s, o) => EditorTools.SetCommandValueInt(s, ref NpcBlacklistCheckTimerTrigger) },
				{"SpawnedVoxelCheckTimerTrigger", (s, o) => EditorTools.SetCommandValueInt(s, ref SpawnedVoxelCheckTimerTrigger) },
				{"SpawnedVoxelMinimumGridDistance", (s, o) => EditorTools.SetCommandValueDouble(s, ref SpawnedVoxelMinimumGridDistance) },
				{"PlanetSpawnsDisableList", (s, o) => EditorTools.SetCommandValueStringArray(s, ref PlanetSpawnsDisableList) },
				{"NpcGridNameBlacklist", (s, o) => EditorTools.SetCommandValueStringArray(s, ref NpcGridNameBlacklist) },
				{"NpcSpawnGroupBlacklist", (s, o) => EditorTools.SetCommandValueStringArray(s, ref NpcSpawnGroupBlacklist) },
				{"UseEconomyBuyingReputationIncrease", (s, o) => EditorTools.SetCommandValueBool(s, ref UseEconomyBuyingReputationIncrease) },
				{"EconomyBuyingReputationCostAmount", (s, o) => EditorTools.SetCommandValueLong(s, ref EconomyBuyingReputationCostAmount) },
				{"Difficulty", (s, o) => EditorTools.SetCommandValueInt(s, ref Difficulty) },

			};

		}
		
		public ConfigGeneral LoadSettings(string phase) {
			
			if(MyAPIGateway.Utilities.FileExistsInWorldStorage("Config-General.xml", typeof(ConfigGeneral)) == true){
				
				try{
					
					ConfigGeneral config = null;
					var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage("Config-General.xml", typeof(ConfigGeneral));
					string configcontents = reader.ReadToEnd();
					config = MyAPIGateway.Utilities.SerializeFromXML<ConfigGeneral>(configcontents);
					config.ConfigLoaded = true;
					SpawnLogger.Write("Loaded Existing Settings From Config-General.xml. Phase: " + phase, SpawnerDebugEnum.Startup, true);
					return config;
					
				}catch(Exception exc){
					
					SpawnLogger.Write("ERROR: Could Not Load Settings From Config-General.xml. Using Default Configuration. Phase: " + phase, SpawnerDebugEnum.Error, true);
					var defaultSettings = new ConfigGeneral();
					return defaultSettings;
					
				}

			} else {

				SpawnLogger.Write("Config-General.xml Doesn't Exist. Creating Default Configuration. Phase: " + phase, SpawnerDebugEnum.Startup, true);

			}

			var settings = new ConfigGeneral();
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-General.xml", typeof(ConfigGeneral))){
				
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigGeneral>(settings));
				
				}
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Create Config-General.xml. Default Settings Will Be Used. Phase: " + phase, SpawnerDebugEnum.Error, true);
				
			}
			
			return settings;
			
		}
		
		public string SaveSettings(){
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-General.xml", typeof(ConfigGeneral))){
					
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigGeneral>(this));
				
				}
				
				SpawnLogger.Write("Settings In Config-General.xml Updated Successfully!", SpawnerDebugEnum.Settings);
				return "Settings Updated Successfully.";
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Save To Config-General.xml. Changes Will Be Lost On World Reload.", SpawnerDebugEnum.Settings);
				
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