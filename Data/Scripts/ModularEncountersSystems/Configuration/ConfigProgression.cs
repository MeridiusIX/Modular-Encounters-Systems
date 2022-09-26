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

	public class ConfigProgression{
		
		public string ModVersion {get; set;}

		public bool AllowAntiJetpackInhibitorSuitUpgrade;
		public bool AllowAntiHandDrillInhibitorSuitUpgrade;
		public bool AllowAntiPersonnelInhibitorSuitUpgrade;
		public bool AllowAntiEnergyInhibitorSuitUpgrade;
		public bool AllowSolarChargingSuitUpgrade;
		public bool AllowDamageReductionSuitUpgrade;


		[XmlIgnore]
		public bool ConfigLoaded;

		[XmlIgnore]
		public Dictionary<string, Func<string, object, bool>> EditorReference;

		public ConfigProgression(){
			
			ModVersion = MES_SessionCore.ModVersion;

			AllowAntiJetpackInhibitorSuitUpgrade = true;
			AllowAntiHandDrillInhibitorSuitUpgrade = true;
			AllowAntiPersonnelInhibitorSuitUpgrade = true;
			AllowAntiEnergyInhibitorSuitUpgrade = true;
			AllowSolarChargingSuitUpgrade = true;
			AllowDamageReductionSuitUpgrade = true;

			EditorReference = new Dictionary<string, Func<string, object, bool>> {

				{"AllowAntiJetpackInhibitorSuitUpgrade", (s, o) => EditorTools.SetCommandValueBool(s, ref AllowAntiJetpackInhibitorSuitUpgrade) },
				{"AllowAntiHandDrillInhibitorSuitUpgrade", (s, o) => EditorTools.SetCommandValueBool(s, ref AllowAntiHandDrillInhibitorSuitUpgrade) },
				{"AllowAntiPersonnelInhibitorSuitUpgrade", (s, o) => EditorTools.SetCommandValueBool(s, ref AllowAntiPersonnelInhibitorSuitUpgrade) },
				{"AllowAntiEnergyInhibitorSuitUpgrade", (s, o) => EditorTools.SetCommandValueBool(s, ref AllowAntiEnergyInhibitorSuitUpgrade) },
				{"AllowSolarChargingInhibitorSuitUpgrade", (s, o) => EditorTools.SetCommandValueBool(s, ref AllowSolarChargingSuitUpgrade) },
				{"AllowDamageReductionInhibitorSuitUpgrade", (s, o) => EditorTools.SetCommandValueBool(s, ref AllowDamageReductionSuitUpgrade) },

			};

		}
		
		public ConfigProgression LoadSettings(string phase) {
			
			if(MyAPIGateway.Utilities.FileExistsInWorldStorage("Config-Progression.xml", typeof(ConfigProgression)) == true){
				
				try{
					
					ConfigProgression config = null;
					var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage("Config-Progression.xml", typeof(ConfigProgression));
					string configcontents = reader.ReadToEnd();
					config = MyAPIGateway.Utilities.SerializeFromXML<ConfigProgression>(configcontents);
					config.ConfigLoaded = true;
					SpawnLogger.Write("Loaded Existing Settings From Config-Progression.xml. Phase: " + phase, SpawnerDebugEnum.Startup, true);
					return config;
					
				}catch(Exception exc){
					
					SpawnLogger.Write("ERROR: Could Not Load Settings From Config-Progression.xml. Using Default Configuration. Phase: " + phase, SpawnerDebugEnum.Error, true);
					var defaultSettings = new ConfigProgression();
					return defaultSettings;
					
				}

			} else {

				SpawnLogger.Write("Config-Progression.xml Doesn't Exist. Creating Default Configuration. Phase: " + phase, SpawnerDebugEnum.Startup, true);

			}

			var settings = new ConfigProgression();
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-Progression.xml", typeof(ConfigProgression))){
				
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigProgression>(settings));
				
				}
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Create Config-Progression.xml. Default Settings Will Be Used. Phase: " + phase, SpawnerDebugEnum.Error, true);
				
			}
			
			return settings;
			
		}
		
		public string SaveSettings(){
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-Progression.xml", typeof(ConfigProgression))){
					
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigProgression>(this));
				
				}
				
				SpawnLogger.Write("Settings In Config-Progression.xml Updated Successfully!", SpawnerDebugEnum.Settings);
				return "Settings Updated Successfully.";
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Save To Config-Progression.xml. Changes Will Be Lost On World Reload.", SpawnerDebugEnum.Settings);
				
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