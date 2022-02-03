using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
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

namespace ModularEncountersSystems.Configuration{

	//CustomBlocks

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

	public class ConfigCustomBlocks{

		public string ModVersion;

		public float ProprietaryReactorFuelAmount;

		public bool UseDisposableBeaconPlayerDistance;
		public bool UseDisposableBeaconInactivity;
		public double DisposableBeaconPlayerDistanceTrigger;
		public float DisposableBeaconRemovalTimerMinutes;

		[XmlIgnore]
		public bool ConfigLoaded;

		[XmlIgnore]
		public Dictionary<string, Func<string, object, bool>> EditorReference;


		public ConfigCustomBlocks() {

			ModVersion = MES_SessionCore.ModVersion;

			ProprietaryReactorFuelAmount = 100;

			UseDisposableBeaconPlayerDistance = true;
			UseDisposableBeaconInactivity = true;
			DisposableBeaconPlayerDistanceTrigger = 7500;
			DisposableBeaconRemovalTimerMinutes = 60;

			EditorReference = new Dictionary<string, Func<string, object, bool>> {

				{"ProprietaryReactorFuelAmount", (s, o) => EditorTools.SetCommandValueFloat(s, ref ProprietaryReactorFuelAmount) },
				{"UseDisposableBeaconPlayerDistance", (s, o) => EditorTools.SetCommandValueBool(s, ref UseDisposableBeaconPlayerDistance) },
				{"UseDisposableBeaconInactivity", (s, o) => EditorTools.SetCommandValueBool(s, ref UseDisposableBeaconInactivity) },
				{"DisposableBeaconPlayerDistanceTrigger", (s, o) => EditorTools.SetCommandValueDouble(s, ref DisposableBeaconPlayerDistanceTrigger) },
				{"DisposableBeaconRemovalTimerMinutes", (s, o) => EditorTools.SetCommandValueFloat(s, ref DisposableBeaconRemovalTimerMinutes) },

			};

		}

		public ConfigCustomBlocks LoadSettings(string phase) {
			
			if(MyAPIGateway.Utilities.FileExistsInWorldStorage("Config-CustomBlocks.xml", typeof(ConfigCustomBlocks)) == true){
				
				try{
					
					ConfigCustomBlocks config = null;
					var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage("Config-CustomBlocks.xml", typeof(ConfigCustomBlocks));
					string configcontents = reader.ReadToEnd();
					config = MyAPIGateway.Utilities.SerializeFromXML<ConfigCustomBlocks>(configcontents);
					config.ConfigLoaded = true;
					SpawnLogger.Write("Loaded Existing Settings From Config-CustomBlocks.xml. Phase: " + phase, SpawnerDebugEnum.Startup, true);
					return config;
					
				}catch(Exception exc){
					
					SpawnLogger.Write("ERROR: Could Not Load Settings From Config-CustomBlocks.xml. Using Default Configuration. Phase: " + phase, SpawnerDebugEnum.Error, true);
					var defaultSettings = new ConfigCustomBlocks();
					return defaultSettings;
					
				}

			} else {

				SpawnLogger.Write("Config-CustomBlocks.xml Doesn't Exist. Creating Default Configuration. Phase: " + phase, SpawnerDebugEnum.Startup, true);

			}

			var settings = new ConfigCustomBlocks();
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-CustomBlocks.xml", typeof(ConfigCustomBlocks))){
				
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigCustomBlocks>(settings));
				
				}
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Create Config-CustomBlocks.xml. Default Settings Will Be Used. Phase: " + phase, SpawnerDebugEnum.Error, true);
				
			}
			
			return settings;
			
		}
		
		public string SaveSettings(){
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-CustomBlocks.xml", typeof(ConfigCustomBlocks))){
					
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigCustomBlocks>(this));
				
				}
				
				SpawnLogger.Write("Settings In Config-CustomBlocks.xml Updated Successfully!", SpawnerDebugEnum.Settings);
				return "Settings Updated Successfully.";
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Save To Config-CustomBlocks.xml. Changes Will Be Lost On World Reload.", SpawnerDebugEnum.Settings);
				
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