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

	public class ConfigGrids{
		
		public string ModVersion {get; set;}

		public bool EnableGlobalNPCWeaponRandomizer;
		public bool EnableGlobalNPCShieldProvider;

		public string[] WeaponReplacerBlacklist;
		public string[] WeaponReplacerWhitelist;
		public string[] WeaponReplacerTargetBlacklist;
		public string[] WeaponReplacerTargetWhitelist;
		public int RandomWeaponChance;
		public int RandomWeaponSizeVariance;
		public bool WeaponReplacerMassLimits;
		public bool WeaponReplacerUseTotalGridMassMultiplier;
		public float WeaponReplacerTotalGridMassMultiplier;
		public bool RandomizedWeaponsUseFullRange;

		public int ShieldProviderChance;

		public bool UseGlobalBlockReplacer;
		public string[] GlobalBlockReplacerReference;
		public string[] GlobalBlockReplacerProfiles;

		public bool UseNonPhysicalAmmoForNPCs;
		public bool RemoveContainerInventoryFromNPCs;

		public bool UseMaxAmmoInventoryWeight;
		public float MaxAmmoInventoryWeight;

		public string[] GlobalReplenishmentProfiles;

		public bool AerodynamicsModAdvLiftOverride;

		public bool StopCompromisedAiMovement;

		[XmlIgnore]
		public bool ConfigLoaded;

		[XmlIgnore]
		public Dictionary<string, Func<string, object, bool>> EditorReference;

		public ConfigGrids(){
			
			ModVersion = MES_SessionCore.ModVersion;

			EnableGlobalNPCWeaponRandomizer = false;
			EnableGlobalNPCShieldProvider = false;

			WeaponReplacerBlacklist = new string[]{"1380830774", "Large_SC_LaserDrill_HiddenStatic", "Large_SC_LaserDrill_HiddenTurret", "Large_SC_LaserDrill", "Large_SC_LaserDrillTurret", "Spotlight_Turret_Large", "Spotlight_Turret_Light_Large", "Spotlight_Turret_Small", "SmallSpotlight_Turret_Small", "ShieldChargerBase_Large", "LDualPulseLaserBase_Large", "AegisLargeBeamBase_Large", "AegisMediumeamBase_Large", "XLGigaBeamGTFBase_Large", "XLDualPulseLaserBase_Large", "1817300677", "LargeSearchlight", "SmallSearchlight" };
			WeaponReplacerWhitelist = new string[]{};
			WeaponReplacerTargetBlacklist = new string[]{};
			WeaponReplacerTargetWhitelist = new string[]{};
			RandomWeaponChance = 100;
			RandomWeaponSizeVariance = -1;
			WeaponReplacerUseTotalGridMassMultiplier = false;
			WeaponReplacerTotalGridMassMultiplier = 1.20f;
			RandomizedWeaponsUseFullRange = false;

			ShieldProviderChance = 100;

			UseGlobalBlockReplacer = true;
			GlobalBlockReplacerReference = new string[]{};
			GlobalBlockReplacerProfiles = new string[]{ "MES-Turret-InteriorToNpcInterior" };

			UseNonPhysicalAmmoForNPCs = false;
			RemoveContainerInventoryFromNPCs = false;

			UseMaxAmmoInventoryWeight = true;
			MaxAmmoInventoryWeight = 1500;

			AerodynamicsModAdvLiftOverride = false;

			GlobalReplenishmentProfiles = new string[] { "MES-Replenishment-BaseRules" };

			StopCompromisedAiMovement = true;

			EditorReference = new Dictionary<string, Func<string, object, bool>> {

				{"EnableGlobalNPCWeaponRandomizer", (s, o) => EditorTools.SetCommandValueBool(s, ref EnableGlobalNPCWeaponRandomizer) },
				{"EnableGlobalNPCShieldProvider", (s, o) => EditorTools.SetCommandValueBool(s, ref EnableGlobalNPCShieldProvider) },
				{"WeaponReplacerBlacklist", (s, o) => EditorTools.SetCommandValueStringArray(s, ref WeaponReplacerBlacklist) },
				{"WeaponReplacerWhitelist", (s, o) => EditorTools.SetCommandValueStringArray(s, ref WeaponReplacerWhitelist) },
				{"WeaponReplacerTargetBlacklist", (s, o) => EditorTools.SetCommandValueStringArray(s, ref WeaponReplacerTargetBlacklist) },
				{"WeaponReplacerTargetWhitelist", (s, o) => EditorTools.SetCommandValueStringArray(s, ref WeaponReplacerTargetWhitelist) },
				{"RandomWeaponChance", (s, o) => EditorTools.SetCommandValueInt(s, ref RandomWeaponChance) },
				{"RandomWeaponSizeVariance", (s, o) => EditorTools.SetCommandValueInt(s, ref RandomWeaponSizeVariance) },
				{"ShieldProviderChance", (s, o) => EditorTools.SetCommandValueInt(s, ref ShieldProviderChance) },
				{"UseGlobalBlockReplacer", (s, o) => EditorTools.SetCommandValueBool(s, ref UseGlobalBlockReplacer) },
				{"GlobalBlockReplacerReference", (s, o) => EditorTools.SetCommandValueStringArray(s, ref GlobalBlockReplacerReference) },
				{"GlobalBlockReplacerProfiles", (s, o) => EditorTools.SetCommandValueStringArray(s, ref GlobalBlockReplacerProfiles) },
				{"UseNonPhysicalAmmoForNPCs", (s, o) => EditorTools.SetCommandValueBool(s, ref UseNonPhysicalAmmoForNPCs) },
				{"RemoveContainerInventoryFromNPCs", (s, o) => EditorTools.SetCommandValueBool(s, ref RemoveContainerInventoryFromNPCs) },
				{"UseMaxAmmoInventoryWeight", (s, o) => EditorTools.SetCommandValueBool(s, ref UseMaxAmmoInventoryWeight) },
				{"MaxAmmoInventoryWeight", (s, o) => EditorTools.SetCommandValueFloat(s, ref MaxAmmoInventoryWeight) },
				{"WeaponReplacerUseTotalGridMassMultiplier", (s, o) => EditorTools.SetCommandValueBool(s, ref WeaponReplacerUseTotalGridMassMultiplier) },
				{"WeaponReplacerTotalGridMassMultiplier", (s, o) => EditorTools.SetCommandValueFloat(s, ref WeaponReplacerTotalGridMassMultiplier) },
				{"RandomizedWeaponsUseFullRange", (s, o) => EditorTools.SetCommandValueBool(s, ref RandomizedWeaponsUseFullRange) },
				{"GlobalReplenishmentProfiles", (s, o) => EditorTools.SetCommandValueStringArray(s, ref GlobalReplenishmentProfiles) },
				{"AerodynamicsModAdvLiftOverride", (s, o) => EditorTools.SetCommandValueBool(s, ref AerodynamicsModAdvLiftOverride) },
				{"StopCompromisedAiMovement", (s, o) => EditorTools.SetCommandValueBool(s, ref StopCompromisedAiMovement) },

			};

		}
		
		public ConfigGrids LoadSettings(string phase) {
			
			if(MyAPIGateway.Utilities.FileExistsInWorldStorage("Config-Grids.xml", typeof(ConfigGrids)) == true){
				
				try{
					
					ConfigGrids config = null;
					var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage("Config-Grids.xml", typeof(ConfigGrids));
					string configcontents = reader.ReadToEnd();
					config = MyAPIGateway.Utilities.SerializeFromXML<ConfigGrids>(configcontents);
					config.ConfigLoaded = true;
					SpawnLogger.Write("Loaded Existing Settings From Config-Grids.xml. Phase: " + phase, SpawnerDebugEnum.Startup, true);
					return config;
					
				}catch(Exception exc){
					
					SpawnLogger.Write("ERROR: Could Not Load Settings From Config-Grids.xml. Using Default Configuration. Phase: " + phase, SpawnerDebugEnum.Error, true);
					var defaultSettings = new ConfigGrids();
					return defaultSettings;
					
				}

			} else {

				SpawnLogger.Write("Config-Grids.xml Doesn't Exist. Creating Default Configuration. Phase: " + phase, SpawnerDebugEnum.Startup, true);

			}

			var settings = new ConfigGrids();
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-Grids.xml", typeof(ConfigGrids))){
				
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigGrids>(settings));
				
				}
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Create Config-Grids.xml. Default Settings Will Be Used. Phase: " + phase, SpawnerDebugEnum.Error, true);
				
			}
			
			return settings;
			
		}
		
		public string SaveSettings(){
			
			try{
				
				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage("Config-Grids.xml", typeof(ConfigGrids))){
					
					writer.Write(MyAPIGateway.Utilities.SerializeToXML<ConfigGrids>(this));
				
				}
				
				SpawnLogger.Write("Settings In Config-Grids.xml Updated Successfully!", SpawnerDebugEnum.Settings);
				return "Settings Updated Successfully.";
				
			}catch(Exception exc){
				
				SpawnLogger.Write("ERROR: Could Not Save To Config-Grids.xml. Changes Will Be Lost On World Reload.", SpawnerDebugEnum.Settings);
				
			}
			
			return "Settings Changed, But Could Not Be Saved To XML. Changes May Be Lost On Session Reload.";
			
		}

		public Dictionary<MyDefinitionId, MyDefinitionId> GetReplacementReferencePairs() {

			var result = new Dictionary<MyDefinitionId, MyDefinitionId>();

			if(this.GlobalBlockReplacerReference.Length == 0) {

				SpawnLogger.Write("Global Block Replacement References 0", SpawnerDebugEnum.Settings);
				return result;

			}

			foreach(var pair in this.GlobalBlockReplacerReference) {

				var split = pair.Split('|');

				if(split.Length != 2) {

					SpawnLogger.Write("Global Replace Bad Split: " + pair, SpawnerDebugEnum.Settings);
					continue;

				}

				var idA = new MyDefinitionId();
				var idB = new MyDefinitionId();

				if(MyDefinitionId.TryParse(split[0], out idA) == false) {

					SpawnLogger.Write("Could Not Parse: " + split[0], SpawnerDebugEnum.Settings);
					continue;

				}

				if(MyDefinitionId.TryParse(split[1], out idB) == false) {

					SpawnLogger.Write("Could Not Parse: " + split[1], SpawnerDebugEnum.Settings);
					continue;

				}

				if(result.ContainsKey(idA) == true) {

					SpawnLogger.Write("MyDefinitionId already present: " + split[0], SpawnerDebugEnum.Settings);
					continue;

				}

				result.Add(idA, idB);

			}

			return result;

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