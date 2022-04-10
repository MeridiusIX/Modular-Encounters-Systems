using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using ModularEncountersSystems;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Configuration.Editor;
using System.Xml.Serialization;

namespace ModularEncountersSystems.Configuration{

	//OtherNPCs

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

	public class ConfigOtherNPCs : ConfigBase {
		
		public string ModVersion;
		public double DespawnDistanceFromPlayer;

		[XmlIgnore]
		public Dictionary<string, Func<string, object, bool>> EditorReference;

		public ConfigOtherNPCs(){
			
			ModVersion = MES_SessionCore.ModVersion;
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

				{"DespawnDistanceFromPlayer", (s, o) => EditorTools.SetCommandValueDouble(s, ref DespawnDistanceFromPlayer) }

			};

		}
		
		public ConfigOtherNPCs LoadSettings(string phase) {
			
			if(MyAPIGateway.Utilities.FileExistsInWorldStorage("Config-OtherNPCs.xml", typeof(ConfigOtherNPCs)) == true){
				
				try{
					
					ConfigOtherNPCs config = null;
					var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage("Config-OtherNPCs.xml", typeof(ConfigOtherNPCs));
					string configcontents = reader.ReadToEnd();
					config = MyAPIGateway.Utilities.SerializeFromXML<ConfigOtherNPCs>(configcontents);
					config.ConfigLoaded = true;
					SpawnLogger.Write("Loaded Existing Settings From Config-OtherNPCs.xml. Phase: " + phase, SpawnerDebugEnum.Startup, true);
					return config;
					
				}catch(Exception exc){
					
					SpawnLogger.Write("ERROR: Could Not Load Settings From Config-OtherNPCs.xml. Using Default Configuration. Phase: " + phase, SpawnerDebugEnum.Error, true);
					var defaultSettings = new ConfigOtherNPCs();
					return defaultSettings;
					
				}

			} else {

				SpawnLogger.Write("Config-OtherNPCs.xml Doesn't Exist. Creating Default Configuration. Phase: " + phase, SpawnerDebugEnum.Startup, true);

			}

			var settings = new ConfigOtherNPCs();
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-OtherNPCs.xml", typeof(ConfigOtherNPCs))){
				
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigOtherNPCs>(settings));
				
				}
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Create Config-OtherNPCs.xml. Default Settings Will Be Used. Phase: " + phase, SpawnerDebugEnum.Error, true);
				
			}
			
			return settings;
			
		}
		
		public override string SaveSettings(){
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-OtherNPCs.xml", typeof(ConfigOtherNPCs))){
					
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigOtherNPCs>(this));
				
				}
				
				SpawnLogger.Write("Settings In Config-OtherNPCs.xml Updated Successfully!", SpawnerDebugEnum.Settings);
				return "Settings Updated Successfully.";
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Save To Config-OtherNPCs.xml. Changes Will Be Lost On World Reload.", SpawnerDebugEnum.Settings);
				
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