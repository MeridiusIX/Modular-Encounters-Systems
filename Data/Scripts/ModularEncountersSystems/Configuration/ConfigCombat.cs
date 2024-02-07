using ModularEncountersSystems.Configuration.Editor;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using VRage.Game;

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

	public class ConfigCombat{
		
		public string ModVersion {get; set;}

		public bool EnableCombatPhaseSystem;

		public int MinCombatPhaseSeconds;
		public int MaxCombatPhaseSeconds;

		public int MinPeacePhaseSeconds;
		public int MaxPeacePhaseSeconds;

		public string[] CombatPhaseModIdOverride;
		public string[] AllPhaseModIdOverride;

		public string[] CombatPhaseSpawnGroupOverride;
		public string[] AllPhaseSpawnGroupOverride;

		public bool UseCombatPhaseSpawnTimerMultiplier;
		public float CombatPhaseSpawnTimerMultiplier;

		public bool AnnouncePhaseChanges;

		[XmlIgnore]
		public bool ConfigLoaded;

		[XmlIgnore]
		public Dictionary<string, Func<string, object, bool>> EditorReference;

		public ConfigCombat(){
			
			ModVersion = MES_SessionCore.ModVersion;

			EnableCombatPhaseSystem = false;

			MinCombatPhaseSeconds = 900;
			MaxCombatPhaseSeconds = 1800;

			MinPeacePhaseSeconds = 1800;
			MaxPeacePhaseSeconds = 3600;

			CombatPhaseModIdOverride = new string[] { };
			AllPhaseModIdOverride = new string[] { };

			CombatPhaseSpawnGroupOverride = new string[] { };
			AllPhaseSpawnGroupOverride = new string[] { };

			UseCombatPhaseSpawnTimerMultiplier = false;
			CombatPhaseSpawnTimerMultiplier = 1.5f;

			AnnouncePhaseChanges = false;

			EditorReference = new Dictionary<string, Func<string, object, bool>> {

				{"EnableCombatPhaseSystem", (s, o) => EditorTools.SetCommandValueBool(s, ref EnableCombatPhaseSystem) },
				{"MinCombatPhaseSeconds", (s, o) => EditorTools.SetCommandValueInt(s, ref MinCombatPhaseSeconds) },
				{"MaxCombatPhaseSeconds", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxCombatPhaseSeconds) },
				{"MinPeacePhaseSeconds", (s, o) => EditorTools.SetCommandValueInt(s, ref MinPeacePhaseSeconds) },
				{"MaxPeacePhaseSeconds", (s, o) => EditorTools.SetCommandValueInt(s, ref MaxPeacePhaseSeconds) },
				{"CombatPhaseModIdOverride", (s, o) => EditorTools.SetCommandValueStringArray(s, ref CombatPhaseModIdOverride) },
				{"AllPhaseModIdOverride", (s, o) => EditorTools.SetCommandValueStringArray(s, ref AllPhaseModIdOverride) },
				{"CombatPhaseSpawnGroupOverride", (s, o) => EditorTools.SetCommandValueStringArray(s, ref CombatPhaseSpawnGroupOverride) },
				{"AllPhaseSpawnGroupOverride", (s, o) => EditorTools.SetCommandValueStringArray(s, ref AllPhaseSpawnGroupOverride) },
				{"UseCombatPhaseSpawnTimerMultiplier", (s, o) => EditorTools.SetCommandValueBool(s, ref UseCombatPhaseSpawnTimerMultiplier) },
				{"CombatPhaseSpawnTimerMultiplier", (s, o) => EditorTools.SetCommandValueFloat(s, ref CombatPhaseSpawnTimerMultiplier) },
				{"AnnouncePhaseChanges", (s, o) => EditorTools.SetCommandValueBool(s, ref AnnouncePhaseChanges) },

			};

		}
		
		public ConfigCombat LoadSettings(string phase) {
			
			if(MyAPIGateway.Utilities.FileExistsInWorldStorage("Config-Combat.xml", typeof(ConfigCombat)) == true){
				
				try{
					
					ConfigCombat config = null;
					var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage("Config-Combat.xml", typeof(ConfigCombat));
					string configcontents = reader.ReadToEnd();
					config = MyAPIGateway.Utilities.SerializeFromXML<ConfigCombat>(configcontents);
					config.ConfigLoaded = true;
					SpawnLogger.Write("Loaded Existing Settings From Config-Combat.xml. Phase: " + phase, SpawnerDebugEnum.Startup, true);
					return config;
					
				}catch(Exception exc){
					
					SpawnLogger.Write("ERROR: Could Not Load Settings From Config-Combat.xml. Using Default Configuration. Phase: " + phase, SpawnerDebugEnum.Error, true);
					var defaultSettings = new ConfigCombat();
					return defaultSettings;
					
				}

			} else {

				SpawnLogger.Write("Config-Combat.xml Doesn't Exist. Creating Default Configuration. Phase: " + phase, SpawnerDebugEnum.Startup, true);

			}

			var settings = new ConfigCombat();
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-Combat.xml", typeof(ConfigCombat))){
				
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigCombat>(settings));
				
				}
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Create Config-Combat.xml. Default Settings Will Be Used. Phase: " + phase, SpawnerDebugEnum.Error, true);
				
			}
			
			return settings;
			
		}
		
		public string SaveSettings(){
			
			try{

				//return "Combat Phases Feature Not Fully Implemented. Config Only Saved For This Session.";

				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-Combat.xml", typeof(ConfigCombat))){
					
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigCombat>(this));
				
				}
				
				SpawnLogger.Write("Settings In Config-Combat.xml Updated Successfully!", SpawnerDebugEnum.Settings);
				return "Settings Updated Successfully.";
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Save To Config-Combat.xml. Changes Will Be Lost On World Reload.", SpawnerDebugEnum.Settings);
				
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