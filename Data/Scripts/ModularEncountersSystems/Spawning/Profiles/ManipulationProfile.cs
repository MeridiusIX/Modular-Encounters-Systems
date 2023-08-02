using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class ManipulationProfile {

		public string ProfileSubtypeId;

		public int ManipulationChance;
		public List<SpawningType> RequiredManipulationSpawnType;
		public List<string> RequiredManipulationSpawnConditions;
		public BlockSizeEnum ManipulationBlockSizeCheck;
		public float ManipulationThreatMinimum;
		public float ManipulationThreatMaximum;
		public int ManipulationMinBlockCount;
		public int ManipulationMaxBlockCount;
		public List<string> ManipulationRequiredCustomTags;
		public List<string> ManipulationAllowedPrefabNames;
		public List<string> ManipulationRestrictedPrefabNames;
		public List<int> ManipulationAllowedPrefabIndexes;
		public List<int> ManipulationRestrictedPrefabIndexes;
		public int ManipulationMinDifficulty;
		public int ManipulationMaxDifficulty;
		public List<ulong> ManipulationRequireAllMods;
		public List<ulong> ManipulationRequireAnyMods;
		public List<ulong> ManipulationExcludeAllMods;
		public List<ulong> ManipulationExcludeAnyMods;

		public bool RandomizeWeapons;
		public bool IgnoreWeaponRandomizerMod;

		public bool IgnoreWeaponRandomizerTargetGlobalBlacklist;
		public bool IgnoreWeaponRandomizerTargetGlobalWhitelist;
		public bool IgnoreWeaponRandomizerGlobalBlacklist;
		public bool IgnoreWeaponRandomizerGlobalWhitelist;

		public List<string> WeaponRandomizerTargetBlacklist;
		public List<string> WeaponRandomizerTargetWhitelist;
		public List<string> WeaponRandomizerBlacklist;
		public List<string> WeaponRandomizerWhitelist;

		public int RandomWeaponChance;
		public int RandomWeaponSizeVariance;

		public List<string> NonRandomWeaponNames;
		public List<MyDefinitionId> NonRandomWeaponIds;
		public Dictionary<string, MyDefinitionId> NonRandomWeaponReference;
		public bool NonRandomWeaponReplacingOnly;

		public bool IgnoreGlobalWeaponMassLimits; //Imp //Doc
		public bool IgnoreGlobalAmmoMassLimits; //Imp //Doc
		public bool UseWeaponMassLimits; //Imp //Doc
		public float WeaponMassMultiplierForGrid; //Imp //Doc

		public bool AddDefenseShieldBlocks = false;
		public bool IgnoreShieldProviderMod = false;
		public int ShieldProviderChance;

		public bool ReplaceArmorBlocksWithModules;
		public List<MyDefinitionId> ModulesForArmorReplacement;

		public bool UseBlockReplacer;
		public Dictionary<MyDefinitionId, MyDefinitionId> ReplaceBlockReference;
		public List<MyDefinitionId> ReplaceBlockOld;
		public List<MyDefinitionId> ReplaceBlockNew;

		public bool UseBlockReplacerProfile;
		public List<string> BlockReplacerProfileNames;

		public bool RelaxReplacedBlocksSize;
		public bool AlwaysRemoveBlock;

		public bool ConfigureSpecialNpcThrusters;

		public bool RestrictNpcIonThrust;
		public float NpcIonThrustForceMultiply;
		public float NpcIonThrustPowerMultiply;

		public bool RestrictNpcAtmoThrust;
		public float NpcAtmoThrustForceMultiply;
		public float NpcAtmoThrustPowerMultiply;

		public bool RestrictNpcHydroThrust;
		public float NpcHydroThrustForceMultiply;
		public float NpcHydroThrustPowerMultiply;

		public bool SetNpcGyroscopeMultiplier;
		public float NpcGyroscopeMultiplier;

		public bool IgnoreGlobalBlockReplacer;

		public bool ConvertToHeavyArmor;

		public bool UseRandomNameGenerator;
		public string RandomGridNamePrefix;
		public List<string> RandomGridNamePattern;
		public string ReplaceAntennaNameWithRandomizedName;
		public bool ProcessBlocksForCustomGridName;

		public string CustomChatAuthorName;
		public bool ProcessCustomChatAuthorAsRandom;

		public bool UseBlockNameReplacer;
		public Dictionary<string, string> BlockNameReplacerReference;
		public List<string> ReplaceBlockNameOld;
		public List<string> ReplaceBlockNameNew;

		public List<string> AssignContainerTypesToAllCargo;

		public bool UseContainerTypeAssignment;
		public Dictionary<string, string> ContainerTypeAssignmentReference;
		public List<string> ContainerTypeAssignBlockName;
		public List<string> ContainerTypeAssignSubtypeId;

		public bool OverrideBlockDamageModifier;
		public double BlockDamageModifier;

		public bool GridsAreEditable;
		public bool GridsAreDestructable;

		public bool ShiftBlockColorsHue;
		public bool RandomHueShift;
		public double ShiftBlockColorAmount;

		public List<string> AssignGridSkin;

		public bool RecolorGrid;
		public Dictionary<Vector3, Vector3> ColorReferencePairs;
		public List<Vector3> RecolorOld;
		public List<Vector3> RecolorNew;
		public Dictionary<Vector3, string> ColorSkinReferencePairs;
		public List<Vector3> ReskinTarget;
		public List<string> ReskinTexture;

		public bool SkinRandomBlocks;
		public List<string> SkinRandomBlocksTextures;
		public int MinPercentageSkinRandomBlocks;
		public int MaxPercentageSkinRandomBlocks;

		public bool ReduceBlockBuildStates;
		public bool AffectNonFunctionalBlock;
		public bool AffectFunctionalBlock;
		public int MinimumBlocksPercent;
		public int MaximumBlocksPercent;
		public int MinimumBuildPercent;
		public int MaximumBuildPercent;

		public bool UseGridDereliction;
		public List<string> DerelictionProfiles;

		public bool UseRivalAi;
		public bool RivalAiReplaceRemoteControl;
		public string ApplyBehaviorToNamedBlock;
		public bool ConvertAllRemoteControlBlocks;

		public bool ClearGridInventories;
		public bool EraseIngameScripts;
		public bool DisableTimerBlocks;
		public bool DisableSensorBlocks;
		public bool DisableWarheads;
		public bool DisableThrustOverride;
		public bool DisableGyroOverride;
		public bool EraseLCDs;
		public List<string> UseTextureLCD;

		public List<string> EnableBlocksWithName;
		public List<string> DisableBlocksWithName;
		public bool AllowPartialNames;

		public bool ChangeTurretSettings;
		public double TurretRange;
		public bool TurretIdleRotation;
		public bool TurretTargetMeteors;
		public bool TurretTargetMissiles;
		public bool TurretTargetCharacters;
		public bool TurretTargetSmallGrids;
		public bool TurretTargetLargeGrids;
		public bool TurretTargetStations;
		public bool TurretTargetNeutrals;

		public bool ShipyardSetup;
		public List<string> ShipyardConsoleBlockNames;
		public List<string> ShipyardProfileNames;

		public bool SuitUpgradeSetup;
		public List<string> SuitUpgradeBlockNames;
		public List<string> SuitUpgradeProfileNames;

		public bool UseResearchPointButtons;

		public bool ClearAuthorship;

		public bool AttachModStorageComponentToGrid;
		public Guid StorageKey;
		public string StorageValue;

		public bool UseLootProfiles;
		public List<LootProfile> LootProfiles;
		public List<string> LootGroups;
		public bool ClearExistingContainerTypes;
		public bool OverrideLootChance;
		public int LootChanceOverride;

		public bool SetDoorsAnyoneCanUse;
		public bool SetStoresAnyoneCanUse;
		public bool SetConnectorsTradeMode;

		public ManipulationProfile(string data = null) {

			ProfileSubtypeId = "";

			ManipulationChance = 100;
			RequiredManipulationSpawnType = new List<SpawningType>();
			RequiredManipulationSpawnConditions = new List<string>();
			ManipulationBlockSizeCheck = BlockSizeEnum.None;
			ManipulationThreatMinimum = -1;
			ManipulationThreatMaximum = -1;
			ManipulationMinBlockCount = -1;
			ManipulationMaxBlockCount = -1;
			ManipulationRequiredCustomTags = new List<string>();
			ManipulationAllowedPrefabNames = new List<string>();
			ManipulationRestrictedPrefabNames = new List<string>();
			ManipulationAllowedPrefabIndexes = new List<int>();
			ManipulationRestrictedPrefabIndexes = new List<int>();
			ManipulationMinDifficulty = -1;
			ManipulationMaxDifficulty = -1;
			ManipulationRequireAllMods = new List<ulong>();
			ManipulationRequireAnyMods = new List<ulong>();
			ManipulationExcludeAllMods = new List<ulong>();
			ManipulationExcludeAnyMods = new List<ulong>();

			RandomizeWeapons = false;
			IgnoreWeaponRandomizerMod = false;
			IgnoreWeaponRandomizerTargetGlobalBlacklist = false;
			IgnoreWeaponRandomizerTargetGlobalWhitelist = false;
			IgnoreWeaponRandomizerGlobalBlacklist = false;
			IgnoreWeaponRandomizerGlobalWhitelist = false;
			WeaponRandomizerTargetBlacklist = new List<string>();
			WeaponRandomizerTargetWhitelist = new List<string>();
			WeaponRandomizerBlacklist = new List<string>();
			WeaponRandomizerWhitelist = new List<string>();
			RandomWeaponChance = 100;
			RandomWeaponSizeVariance = -1;
			NonRandomWeaponNames = new List<string>();
			NonRandomWeaponIds = new List<MyDefinitionId>();
			NonRandomWeaponReference = new Dictionary<string, MyDefinitionId>();
			NonRandomWeaponReplacingOnly = false;

			AddDefenseShieldBlocks = false;
			IgnoreShieldProviderMod = false;
			ShieldProviderChance = 100;

			ReplaceArmorBlocksWithModules = false;
			ModulesForArmorReplacement = new List<MyDefinitionId>();

			UseBlockReplacer = false;
			ReplaceBlockReference = new Dictionary<MyDefinitionId, MyDefinitionId>();
			ReplaceBlockOld = new List<MyDefinitionId>();
			ReplaceBlockNew = new List<MyDefinitionId>();

			UseBlockReplacerProfile = false;
			BlockReplacerProfileNames = new List<string>();

			RelaxReplacedBlocksSize = false;
			AlwaysRemoveBlock = false;

			ConfigureSpecialNpcThrusters = false;

			RestrictNpcIonThrust = false;
			NpcIonThrustForceMultiply = 1;
			NpcIonThrustPowerMultiply = 1;

			RestrictNpcAtmoThrust = false;
			NpcAtmoThrustForceMultiply = 1;
			NpcAtmoThrustPowerMultiply = 1;

			RestrictNpcHydroThrust = false;
			NpcHydroThrustForceMultiply = 1;
			NpcHydroThrustPowerMultiply = 1;

			SetNpcGyroscopeMultiplier = false;
			NpcGyroscopeMultiplier = 1;

			IgnoreGlobalBlockReplacer = false;

			ConvertToHeavyArmor = false;

			UseRandomNameGenerator = false;
			RandomGridNamePrefix = "";
			RandomGridNamePattern = new List<string>();
			ReplaceAntennaNameWithRandomizedName = "";
			ProcessBlocksForCustomGridName = false;

			CustomChatAuthorName = null;
			ProcessCustomChatAuthorAsRandom = false;

			UseBlockNameReplacer = false;
			BlockNameReplacerReference = new Dictionary<string, string>();
			ReplaceBlockNameOld = new List<string>();
			ReplaceBlockNameNew = new List<string>();

			AssignContainerTypesToAllCargo = new List<string>();

			UseContainerTypeAssignment = false;
			ContainerTypeAssignmentReference = new Dictionary<string, string>();
			ContainerTypeAssignBlockName = new List<string>();
			ContainerTypeAssignSubtypeId = new List<string>();

			OverrideBlockDamageModifier = false;
			BlockDamageModifier = 1;

			GridsAreEditable = true;
			GridsAreDestructable = true;

			ShiftBlockColorsHue = false;
			RandomHueShift = false;
			ShiftBlockColorAmount = 0;

			AssignGridSkin = new List<string>();

			RecolorGrid = false;
			ColorReferencePairs = new Dictionary<Vector3, Vector3>();
			RecolorOld = new List<Vector3>();
			RecolorNew = new List<Vector3>();
			ColorSkinReferencePairs = new Dictionary<Vector3, string>();
			ReskinTarget = new List<Vector3>();
			ReskinTexture = new List<string>();

			SkinRandomBlocks = false;
			SkinRandomBlocksTextures = new List<string>();
			MinPercentageSkinRandomBlocks = 10;
			MaxPercentageSkinRandomBlocks = 40;

			ReduceBlockBuildStates = false;
			AffectNonFunctionalBlock = true;
			AffectFunctionalBlock = false;
			MinimumBlocksPercent = 10;
			MaximumBlocksPercent = 40;
			MinimumBuildPercent = 10;
			MaximumBuildPercent = 75;

			UseGridDereliction = false;
			DerelictionProfiles = new List<string>();

			UseRivalAi = false;
			RivalAiReplaceRemoteControl = false;
			ApplyBehaviorToNamedBlock = "";
			ConvertAllRemoteControlBlocks = false;

			ClearGridInventories = false;
			EraseIngameScripts = false;
			DisableTimerBlocks = false;
			DisableSensorBlocks = false;
			DisableWarheads = false;
			DisableThrustOverride = false;
			DisableGyroOverride = false;
			EraseLCDs = false;
			UseTextureLCD = new List<string>();

			EnableBlocksWithName = new List<string>();
			DisableBlocksWithName = new List<string>();
			AllowPartialNames = false;

			ChangeTurretSettings = false;
			TurretRange = 800;
			TurretIdleRotation = false;
			TurretTargetMeteors = true;
			TurretTargetMissiles = true;
			TurretTargetCharacters = true;
			TurretTargetSmallGrids = true;
			TurretTargetLargeGrids = true;
			TurretTargetStations = true;
			TurretTargetNeutrals = true;

			ShipyardSetup = false;
			ShipyardConsoleBlockNames = new List<string>();
			ShipyardProfileNames = new List<string>();

			SuitUpgradeSetup = false;
			SuitUpgradeBlockNames = new List<string>();
			SuitUpgradeProfileNames = new List<string>();

			UseResearchPointButtons = false;

			ClearAuthorship = false;

			AttachModStorageComponentToGrid = false;
			StorageKey = new Guid("00000000-0000-0000-0000-000000000000");
			StorageValue = "";

			UseLootProfiles = false;
			LootProfiles = new List<LootProfile>();
			LootGroups = new List<string>();
			ClearExistingContainerTypes = false;
			OverrideLootChance = false;
			LootChanceOverride = 100;

			SetDoorsAnyoneCanUse = false;
			SetStoresAnyoneCanUse = false;
			SetConnectorsTradeMode = false;

			InitTags(data);

		}

		public void InitTags(string data = null) {

			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			foreach (var tagRaw in descSplit) {

				var tag = tagRaw.Trim();

				//ManipulationChance
				if (tag.StartsWith("[ManipulationChance:") == true) {

					TagParse.TagIntCheck(tag, ref this.ManipulationChance);

				}

				//RequiredManipulationSpawnType
				if (tag.StartsWith("[RequiredManipulationSpawnType:") == true) {

					TagParse.TagSpawningTypeEnumCheck(tag, ref this.RequiredManipulationSpawnType);

				}

				//RequiredManipulationSpawnConditions
				if (tag.StartsWith("[RequiredManipulationSpawnConditions:") == true) {

					TagParse.TagStringListCheck(tag, ref this.RequiredManipulationSpawnConditions);

				}

				//ManipulationThreatMinimum
				if (tag.StartsWith("[ManipulationThreatMinimum:") == true) {

					TagParse.TagFloatCheck(tag, ref this.ManipulationThreatMinimum);

				}

				//ManipulationBlockSizeCheck
				if (tag.StartsWith("[ManipulationBlockSizeCheck:") == true) {

					TagParse.TagBlockSizeEnumCheck(tag, ref this.ManipulationBlockSizeCheck);

				}

				//ManipulationThreatMaximum
				if (tag.StartsWith("[ManipulationThreatMaximum:") == true) {

					TagParse.TagFloatCheck(tag, ref this.ManipulationThreatMaximum);

				}

				//ManipulationMinBlockCount
				if (tag.StartsWith("[ManipulationMinBlockCount:") == true) {

					TagParse.TagIntCheck(tag, ref this.ManipulationMinBlockCount);

				}

				//ManipulationMaxBlockCount
				if (tag.StartsWith("[ManipulationMaxBlockCount:") == true) {

					TagParse.TagIntCheck(tag, ref this.ManipulationMaxBlockCount);

				}

				//ManipulationRequiredCustomTags
				if (tag.StartsWith("[ManipulationRequiredCustomTags:") == true) {

					TagParse.TagStringListCheck(tag, ref this.ManipulationRequiredCustomTags);

				}

				//ManipulationAllowedPrefabNames
				if (tag.StartsWith("[ManipulationAllowedPrefabNames:") == true) {

					TagParse.TagStringListCheck(tag, ref this.ManipulationAllowedPrefabNames);

				}

				//ManipulationRestrictedPrefabNames
				if (tag.StartsWith("[ManipulationRestrictedPrefabNames:") == true) {

					TagParse.TagStringListCheck(tag, ref this.ManipulationRestrictedPrefabNames);

				}

				//ManipulationAllowedPrefabIndexes
				if (tag.StartsWith("[ManipulationAllowedPrefabIndexes:") == true) {

					TagParse.TagIntListCheck(tag, true, ref this.ManipulationAllowedPrefabIndexes);

				}

				//ManipulationRestrictedPrefabIndexes
				if (tag.StartsWith("[ManipulationRestrictedPrefabIndexes:") == true) {

					TagParse.TagIntListCheck(tag, true, ref this.ManipulationRestrictedPrefabIndexes);

				}

				//ManipulationMinDifficulty
				if (tag.StartsWith("[ManipulationMinDifficulty:") == true) {

					TagParse.TagIntCheck(tag, ref this.ManipulationMinDifficulty);

				}

				//ManipulationMaxDifficulty
				if (tag.StartsWith("[ManipulationMaxDifficulty:") == true) {

					TagParse.TagIntCheck(tag, ref this.ManipulationMaxDifficulty);

				}

				//ManipulationRequireAllMods
				if (tag.StartsWith("[ManipulationRequireAllMods:") == true) {

					TagParse.TagUlongListCheck(tag, ref this.ManipulationRequireAllMods);

				}

				//ManipulationRequireAnyMods
				if (tag.StartsWith("[ManipulationRequireAnyMods:") == true) {

					TagParse.TagUlongListCheck(tag, ref this.ManipulationRequireAnyMods);

				}

				//ManipulationExcludeAllMods
				if (tag.StartsWith("[ManipulationExcludeAllMods:") == true) {

					TagParse.TagUlongListCheck(tag, ref this.ManipulationExcludeAllMods);

				}

				//ManipulationExcludeAnyMods
				if (tag.StartsWith("[ManipulationExcludeAnyMods:") == true) {

					TagParse.TagUlongListCheck(tag, ref this.ManipulationExcludeAnyMods);

				}

				//RandomizeWeapons
				if (tag.StartsWith("[RandomizeWeapons:") == true) {

					TagParse.TagBoolCheck(tag, ref this.RandomizeWeapons);

				}

				//IgnoreWeaponRandomizerMod
				if (tag.StartsWith("[IgnoreWeaponRandomizerMod:") == true) {

					TagParse.TagBoolCheck(tag, ref this.IgnoreWeaponRandomizerMod);

				}

				//IgnoreWeaponRandomizerTargetGlobalBlacklist
				if (tag.StartsWith("[IgnoreWeaponRandomizerTargetGlobalBlacklist:") == true) {

					TagParse.TagBoolCheck(tag, ref this.IgnoreWeaponRandomizerTargetGlobalBlacklist);

				}

				//IgnoreWeaponRandomizerTargetGlobalWhitelist
				if (tag.StartsWith("[IgnoreWeaponRandomizerTargetGlobalWhitelist:") == true) {

					TagParse.TagBoolCheck(tag, ref this.IgnoreWeaponRandomizerTargetGlobalWhitelist);

				}

				//IgnoreWeaponRandomizerGlobalBlacklist
				if (tag.StartsWith("[IgnoreWeaponRandomizerGlobalBlacklist:") == true) {

					TagParse.TagBoolCheck(tag, ref this.IgnoreWeaponRandomizerGlobalBlacklist);

				}

				//IgnoreWeaponRandomizerGlobalWhitelist
				if (tag.StartsWith("[IgnoreWeaponRandomizerGlobalWhitelist:") == true) {

					TagParse.TagBoolCheck(tag, ref this.IgnoreWeaponRandomizerGlobalWhitelist);

				}

				//WeaponRandomizerTargetBlacklist
				if (tag.StartsWith("[WeaponRandomizerTargetBlacklist:") == true) {

					TagParse.TagStringListCheck(tag, ref this.WeaponRandomizerTargetBlacklist);

				}

				//WeaponRandomizerTargetWhitelist
				if (tag.StartsWith("[WeaponRandomizerTargetWhitelist:") == true) {

					TagParse.TagStringListCheck(tag, ref this.WeaponRandomizerTargetWhitelist);

				}

				//WeaponRandomizerBlacklist
				if (tag.StartsWith("[WeaponRandomizerBlacklist:") == true) {

					TagParse.TagStringListCheck(tag, ref this.WeaponRandomizerBlacklist);

				}

				//WeaponRandomizerWhitelist
				if (tag.StartsWith("[WeaponRandomizerWhitelist:") == true) {

					TagParse.TagStringListCheck(tag, ref this.WeaponRandomizerWhitelist);

				}

				//RandomWeaponChance
				if (tag.StartsWith("[RandomWeaponChance:") == true) {

					TagParse.TagIntCheck(tag, ref this.RandomWeaponChance);

				}

				//RandomWeaponSizeVariance
				if (tag.StartsWith("[RandomWeaponSizeVariance:") == true) {

					TagParse.TagIntCheck(tag, ref this.RandomWeaponSizeVariance);

				}

				//NonRandomWeaponNames
				if (tag.StartsWith("[NonRandomWeaponNames:") == true) {

					TagParse.TagStringListCheck(tag, ref this.NonRandomWeaponNames);

				}

				//NonRandomWeaponIds
				if (tag.StartsWith("[NonRandomWeaponIds:") == true) {

					TagParse.TagMyDefIdCheck(tag, ref this.NonRandomWeaponIds);

				}

				//NonRandomWeaponReplacingOnly
				if (tag.StartsWith("[NonRandomWeaponReplacingOnly:") == true) {

					TagParse.TagBoolCheck(tag, ref this.NonRandomWeaponReplacingOnly);

				}

				//AddDefenseShieldBlocks
				if (tag.StartsWith("[AddDefenseShieldBlocks:") == true) {

					TagParse.TagBoolCheck(tag, ref this.AddDefenseShieldBlocks);

				}

				//IgnoreShieldProviderMod
				if (tag.StartsWith("[IgnoreShieldProviderMod:") == true) {

					TagParse.TagBoolCheck(tag, ref this.IgnoreShieldProviderMod);

				}

				//ShieldProviderChance
				if (tag.StartsWith("[ShieldProviderChance:") == true) {

					TagParse.TagIntCheck(tag, ref this.ShieldProviderChance);

				}

				//ReplaceArmorBlocksWithModules
				if (tag.StartsWith("[ReplaceArmorBlocksWithModules:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ReplaceArmorBlocksWithModules);

				}

				//ModulesForArmorReplacement
				if (tag.StartsWith("[ModulesForArmorReplacement:") == true) {

					TagParse.TagMyDefIdCheck(tag, ref this.ModulesForArmorReplacement);

				}

				//UseBlockReplacerProfile
				if (tag.StartsWith("[UseBlockReplacerProfile:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseBlockReplacerProfile);

				}

				//BlockReplacerProfileNames
				if (tag.StartsWith("[BlockReplacerProfileNames:") == true) {

					TagParse.TagStringListCheck(tag, ref this.BlockReplacerProfileNames);

				}

				//UseBlockReplacer
				if (tag.StartsWith("[UseBlockReplacer:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseBlockReplacer);

				}

				//RelaxReplacedBlocksSize
				if (tag.StartsWith("[RelaxReplacedBlocksSize:") == true) {

					TagParse.TagBoolCheck(tag, ref this.RelaxReplacedBlocksSize);

				}

				//AlwaysRemoveBlock
				if (tag.StartsWith("[AlwaysRemoveBlock:") == true) {

					TagParse.TagBoolCheck(tag, ref this.AlwaysRemoveBlock);

				}

				//ConfigureSpecialNpcThrusters
				if (tag.StartsWith("[ConfigureSpecialNpcThrusters:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ConfigureSpecialNpcThrusters);

				}

				//RestrictNpcIonThrust
				if (tag.StartsWith("[RestrictNpcIonThrust:") == true) {

					TagParse.TagBoolCheck(tag, ref this.RestrictNpcIonThrust);

				}

				//NpcIonThrustForceMultiply
				if (tag.StartsWith("[NpcIonThrustForceMultiply:") == true) {

					TagParse.TagFloatCheck(tag, ref this.NpcIonThrustForceMultiply);

				}

				//NpcIonThrustPowerMultiply
				if (tag.StartsWith("[NpcIonThrustPowerMultiply:") == true) {

					TagParse.TagFloatCheck(tag, ref this.NpcIonThrustPowerMultiply);

				}

				//RestrictNpcAtmoThrust
				if (tag.StartsWith("[RestrictNpcAtmoThrust:") == true) {

					TagParse.TagBoolCheck(tag, ref this.RestrictNpcAtmoThrust);

				}

				//NpcAtmoThrustForceMultiply
				if (tag.StartsWith("[NpcAtmoThrustForceMultiply:") == true) {

					TagParse.TagFloatCheck(tag, ref this.NpcAtmoThrustForceMultiply);

				}

				//NpcAtmoThrustPowerMultiply
				if (tag.StartsWith("[NpcAtmoThrustPowerMultiply:") == true) {

					TagParse.TagFloatCheck(tag, ref this.NpcAtmoThrustPowerMultiply);

				}

				//RestrictNpcHydroThrust
				if (tag.StartsWith("[RestrictNpcHydroThrust:") == true) {

					TagParse.TagBoolCheck(tag, ref this.RestrictNpcHydroThrust);

				}

				//NpcHydroThrustForceMultiply
				if (tag.StartsWith("[NpcHydroThrustForceMultiply:") == true) {

					TagParse.TagFloatCheck(tag, ref this.NpcHydroThrustForceMultiply);

				}

				//NpcHydroThrustPowerMultiply
				if (tag.StartsWith("[NpcHydroThrustPowerMultiply:") == true) {

					TagParse.TagFloatCheck(tag, ref this.NpcHydroThrustPowerMultiply);

				}

				//SetNpcGyroscopeMultiplier
				if (tag.StartsWith("[SetNpcGyroscopeMultiplier:") == true) {

					TagParse.TagBoolCheck(tag, ref this.SetNpcGyroscopeMultiplier);

				}

				//NpcGyroscopeMultiplier
				if (tag.StartsWith("[NpcGyroscopeMultiplier:") == true) {

					TagParse.TagFloatCheck(tag, ref this.NpcGyroscopeMultiplier);

				}

				//IgnoreGlobalBlockReplacer
				if (tag.StartsWith("[IgnoreGlobalBlockReplacer:") == true) {

					TagParse.TagBoolCheck(tag, ref this.IgnoreGlobalBlockReplacer);

				}

				//ReplaceBlockReference
				if (tag.StartsWith("[ReplaceBlockReference:") == true) {

					TagParse.TagMDIDictionaryCheck(tag, ref this.ReplaceBlockReference);

				}

				//ReplaceBlockOld
				if (tag.StartsWith("[ReplaceBlockOld:") == true) {

					TagParse.TagMyDefIdCheck(tag, ref this.ReplaceBlockOld);

				}

				//ReplaceBlockNew
				if (tag.StartsWith("[ReplaceBlockNew:") == true) {

					TagParse.TagMyDefIdCheck(tag, ref this.ReplaceBlockNew);

				}

				//ConvertToHeavyArmor
				if (tag.StartsWith("[ConvertToHeavyArmor:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ConvertToHeavyArmor);

					if (this.ConvertToHeavyArmor && !this.BlockReplacerProfileNames.Contains("MES-Armor-LightToHeavy"))
						this.BlockReplacerProfileNames.Add("MES-Armor-LightToHeavy");

				}

				//UseRandomNameGenerator
				if (tag.StartsWith("[UseRandomNameGenerator:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseRandomNameGenerator);

				}

				//RandomGridNamePrefix
				if (tag.StartsWith("[RandomGridNamePrefix:") == true) {

					TagParse.TagStringCheck(tag, ref this.RandomGridNamePrefix);

				}

				//RandomGridNamePattern
				if (tag.StartsWith("[RandomGridNamePattern:") == true) {

					TagParse.TagStringListCheck(tag, ref this.RandomGridNamePattern);

				}

				//ReplaceAntennaNameWithRandomizedName
				if (tag.StartsWith("[ReplaceAntennaNameWithRandomizedName:") == true) {

					TagParse.TagStringCheck(tag, ref this.ReplaceAntennaNameWithRandomizedName);

				}

				//CustomChatAuthorName
				if (tag.StartsWith("[CustomChatAuthorName:") == true) {

					TagParse.TagStringCheck(tag, ref this.CustomChatAuthorName);

				}

				//ProcessCustomChatAuthorAsRandom
				if (tag.StartsWith("[ProcessCustomChatAuthorAsRandom:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ProcessCustomChatAuthorAsRandom);

				}

				//ProcessBlocksForCustomGridName
				if (tag.StartsWith("[ProcessBlocksForCustomGridName:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ProcessBlocksForCustomGridName);

				}

				//UseBlockNameReplacer
				if (tag.StartsWith("[UseBlockNameReplacer:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseBlockNameReplacer);

				}

				//BlockNameReplacerReference
				if (tag.StartsWith("[BlockNameReplacerReference:") == true) {

					TagParse.TagStringDictionaryCheck(tag, ref this.BlockNameReplacerReference);

				}

				//ReplaceBlockNameOld
				if (tag.StartsWith("[ReplaceBlockNameOld:") == true) {

					TagParse.TagStringListCheck(tag, ref this.ReplaceBlockNameOld);

				}

				//ReplaceBlockNameNew
				if (tag.StartsWith("[ReplaceBlockNameNew:") == true) {

					TagParse.TagStringListCheck(tag, ref this.ReplaceBlockNameNew);

				}

				//AssignContainerTypesToAllCargo
				if (tag.StartsWith("[AssignContainerTypesToAllCargo:") == true) {

					TagParse.TagStringListCheck(tag, ref this.AssignContainerTypesToAllCargo);

				}

				//UseContainerTypeAssignment
				if (tag.StartsWith("[UseContainerTypeAssignment:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseContainerTypeAssignment);

				}

				//ContainerTypeAssignmentReference
				if (tag.StartsWith("[ContainerTypeAssignmentReference") == true) {

					TagParse.TagStringDictionaryCheck(tag, ref this.ContainerTypeAssignmentReference);

				}

				//ContainerTypeAssignBlockName
				if (tag.StartsWith("[ContainerTypeAssignBlockName:") == true) {

					TagParse.TagStringListCheck(tag, ref this.ContainerTypeAssignBlockName);

				}

				//ContainerTypeAssignSubtypeId
				if (tag.StartsWith("[ContainerTypeAssignSubtypeId:") == true) {

					TagParse.TagStringListCheck(tag, ref this.ContainerTypeAssignSubtypeId);

				}

				//OverrideBlockDamageModifier
				if (tag.StartsWith("[OverrideBlockDamageModifier:") == true) {

					TagParse.TagBoolCheck(tag, ref this.OverrideBlockDamageModifier);

				}

				//BlockDamageModifier
				if (tag.StartsWith("[BlockDamageModifier:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.BlockDamageModifier);

				}

				//GridsAreEditable
				if (tag.StartsWith("[GridsAreEditable:") == true) {

					TagParse.TagBoolCheck(tag, ref this.GridsAreEditable);

				}

				//GridsAreDestructable
				if (tag.StartsWith("[GridsAreDestructable:") == true) {

					TagParse.TagBoolCheck(tag, ref this.GridsAreDestructable);

				}

				//ShiftBlockColorsHue
				if (tag.StartsWith("[ShiftBlockColorsHue:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ShiftBlockColorsHue);

				}

				//RandomHueShift
				if (tag.StartsWith("[RandomHueShift:") == true) {

					TagParse.TagBoolCheck(tag, ref this.RandomHueShift);

				}

				//ShiftBlockColorAmount
				if (tag.StartsWith("[ShiftBlockColorAmount:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.ShiftBlockColorAmount);

				}

				//AssignGridSkin
				if (tag.StartsWith("[AssignGridSkin:") == true) {

					TagParse.TagStringListCheck(tag, ref this.AssignGridSkin);

				}

				//RecolorGrid
				if (tag.StartsWith("[RecolorGrid:") == true) {

					TagParse.TagBoolCheck(tag, ref this.RecolorGrid);

				}

				//ColorReferencePairs
				if (tag.StartsWith("[ColorReferencePairs:") == true) {

					TagParse.TagVector3DictionaryCheck(tag, ref this.ColorReferencePairs);
					//Logger.Write(this.ColorReferencePairs.Keys.Count.ToString() + " Color Reference Pairs");

				}

				//RecolorOld
				if (tag.StartsWith("[RecolorOld:") == true) {

					TagParse.TagVector3Check(tag, ref this.RecolorOld);

				}

				//RecolorNew
				if (tag.StartsWith("[RecolorNew:") == true) {

					TagParse.TagVector3Check(tag, ref this.RecolorNew);

				}

				//ColorSkinReferencePairs
				if (tag.StartsWith("[ColorSkinReferencePairs:") == true) {

					TagParse.TagVector3StringDictionaryCheck(tag, ref this.ColorSkinReferencePairs);
					//Logger.Write(this.ColorReferencePairs.Keys.Count.ToString() + " Color-Skin Reference Pairs");

				}

				//ReskinTarget
				if (tag.StartsWith("[ReskinTarget:") == true) {

					TagParse.TagVector3Check(tag, ref this.ReskinTarget);

				}

				//ReskinTexture
				if (tag.StartsWith("[ReskinTexture:") == true) {

					TagParse.TagStringListCheck(tag, ref this.ReskinTexture);

				}

				//SkinRandomBlocks
				if (tag.StartsWith("[SkinRandomBlocks:") == true) {

					TagParse.TagBoolCheck(tag, ref this.SkinRandomBlocks);

				}

				//SkinRandomBlocksTextures
				if (tag.StartsWith("[SkinRandomBlocksTextures:") == true) {

					TagParse.TagStringListCheck(tag, ref this.SkinRandomBlocksTextures);

				}

				//MinPercentageSkinRandomBlocks
				if (tag.StartsWith("[MinPercentageSkinRandomBlocks:") == true) {

					TagParse.TagIntCheck(tag, ref this.MinPercentageSkinRandomBlocks);

				}

				//MaxPercentageSkinRandomBlocks
				if (tag.StartsWith("[MaxPercentageSkinRandomBlocks:") == true) {

					TagParse.TagIntCheck(tag, ref this.MaxPercentageSkinRandomBlocks);

				}

				//ReduceBlockBuildStates
				if (tag.StartsWith("[ReduceBlockBuildStates:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ReduceBlockBuildStates);

				}

				//MinimumBlocksPercent
				if (tag.StartsWith("[MinimumBlocksPercent:") == true) {

					TagParse.TagIntCheck(tag, ref this.MinimumBlocksPercent);

				}

				//MaximumBlocksPercent
				if (tag.StartsWith("[MaximumBlocksPercent:") == true) {

					TagParse.TagIntCheck(tag, ref this.MaximumBlocksPercent);

				}

				//MinimumBuildPercent
				if (tag.StartsWith("[MinimumBuildPercent:") == true) {

					TagParse.TagIntCheck(tag, ref this.MinimumBuildPercent);

				}

				//MaximumBuildPercent
				if (tag.StartsWith("[MaximumBuildPercent:") == true) {

					TagParse.TagIntCheck(tag, ref this.MaximumBuildPercent);

				}

				//UseGridDereliction
				if (tag.StartsWith("[UseGridDereliction:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseGridDereliction);

				}

				//DerelictionProfiles
				if (tag.StartsWith("[DerelictionProfiles:") == true) {

					TagParse.TagStringListCheck(tag, ref this.DerelictionProfiles);

				}

				//UseRivalAi
				if (tag.StartsWith("[UseRivalAi:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseRivalAi);

				}

				//RivalAiReplaceRemoteControl
				if (tag.StartsWith("[RivalAiReplaceRemoteControl:") == true) {

					TagParse.TagBoolCheck(tag, ref this.RivalAiReplaceRemoteControl);

				}

				//ApplyBehaviorToNamedBlock
				if (tag.StartsWith("[ApplyBehaviorToNamedBlock:") == true) {

					TagParse.TagStringCheck(tag, ref this.ApplyBehaviorToNamedBlock);

				}

				//ConvertAllRemoteControlBlocks
				if (tag.StartsWith("[ConvertAllRemoteControlBlocks:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ConvertAllRemoteControlBlocks);

				}

				//ClearGridInventories
				if (tag.StartsWith("[ClearGridInventories:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ClearGridInventories);

				}

				//EraseIngameScripts
				if (tag.StartsWith("[EraseIngameScripts:") == true) {

					TagParse.TagBoolCheck(tag, ref this.EraseIngameScripts);

				}

				//DisableTimerBlocks
				if (tag.StartsWith("[DisableTimerBlocks:") == true) {

					TagParse.TagBoolCheck(tag, ref this.DisableTimerBlocks);

				}

				//DisableSensorBlocks
				if (tag.StartsWith("[DisableSensorBlocks:") == true) {

					TagParse.TagBoolCheck(tag, ref this.DisableSensorBlocks);

				}

				//DisableWarheads
				if (tag.StartsWith("[DisableWarheads:") == true) {

					TagParse.TagBoolCheck(tag, ref this.DisableWarheads);

				}

				//DisableThrustOverride
				if (tag.StartsWith("[DisableThrustOverride:") == true) {

					TagParse.TagBoolCheck(tag, ref this.DisableThrustOverride);

				}

				//DisableGyroOverride
				if (tag.StartsWith("[DisableGyroOverride:") == true) {

					TagParse.TagBoolCheck(tag, ref this.DisableGyroOverride);

				}

				//EraseLCDs
				if (tag.StartsWith("[EraseLCDs:") == true) {

					TagParse.TagBoolCheck(tag, ref this.EraseLCDs);

				}

				//UseTextureLCD
				if (tag.StartsWith("[UseTextureLCD:") == true) {

					TagParse.TagStringListCheck(tag, ref this.UseTextureLCD);

				}

				//EnableBlocksWithName
				if (tag.StartsWith("[EnableBlocksWithName:") == true) {

					TagParse.TagStringListCheck(tag, ref this.EnableBlocksWithName);

				}

				//DisableBlocksWithName
				if (tag.StartsWith("[DisableBlocksWithName:") == true) {

					TagParse.TagStringListCheck(tag, ref this.DisableBlocksWithName);

				}

				//AllowPartialNames
				if (tag.StartsWith("[AllowPartialNames:") == true) {

					TagParse.TagBoolCheck(tag, ref this.AllowPartialNames);

				}

				//ChangeTurretSettings
				if (tag.StartsWith("[ChangeTurretSettings:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ChangeTurretSettings);

				}

				//TurretRange
				if (tag.StartsWith("[TurretRange:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.TurretRange);

				}

				//TurretIdleRotation
				if (tag.StartsWith("[TurretIdleRotation:") == true) {

					TagParse.TagBoolCheck(tag, ref this.TurretIdleRotation);

				}

				//TurretTargetMeteors
				if (tag.StartsWith("[TurretTargetMeteors:") == true) {

					TagParse.TagBoolCheck(tag, ref this.TurretTargetMeteors);

				}

				//TurretTargetMissiles
				if (tag.StartsWith("[TurretTargetMissiles:") == true) {

					TagParse.TagBoolCheck(tag, ref this.TurretTargetMissiles);

				}

				//TurretTargetCharacters
				if (tag.StartsWith("[TurretTargetCharacters:") == true) {

					TagParse.TagBoolCheck(tag, ref this.TurretTargetCharacters);

				}

				//TurretTargetSmallGrids
				if (tag.StartsWith("[TurretTargetSmallGrids:") == true) {

					TagParse.TagBoolCheck(tag, ref this.TurretTargetSmallGrids);

				}

				//TurretTargetLargeGrids
				if (tag.StartsWith("[TurretTargetLargeGrids:") == true) {

					TagParse.TagBoolCheck(tag, ref this.TurretTargetLargeGrids);

				}

				//TurretTargetStations
				if (tag.StartsWith("[TurretTargetStations:") == true) {

					TagParse.TagBoolCheck(tag, ref this.TurretTargetStations);

				}

				//TurretTargetNeutrals
				if (tag.StartsWith("[TurretTargetNeutrals:") == true) {

					TagParse.TagBoolCheck(tag, ref this.TurretTargetNeutrals);

				}

				//ShipyardSetup
				if (tag.StartsWith("[ShipyardSetup:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ShipyardSetup);

				}

				//ShipyardConsoleBlockNames
				if (tag.StartsWith("[ShipyardConsoleBlockNames:") == true) {

					TagParse.TagStringListCheck(tag, ref this.ShipyardConsoleBlockNames);

				}

				//ShipyardProfileNames
				if (tag.StartsWith("[ShipyardProfileNames:") == true) {

					TagParse.TagStringListCheck(tag, ref this.ShipyardProfileNames);

				}

				//SuitUpgradeSetup
				if (tag.StartsWith("[SuitUpgradeSetup:") == true) {

					TagParse.TagBoolCheck(tag, ref this.SuitUpgradeSetup);

				}

				//SuitUpgradeBlockNames
				if (tag.StartsWith("[SuitUpgradeBlockNames:") == true) {

					TagParse.TagStringListCheck(tag, ref this.SuitUpgradeBlockNames);

				}

				//SuitUpgradeProfileNames
				if (tag.StartsWith("[SuitUpgradeProfileNames:") == true) {

					TagParse.TagStringListCheck(tag, ref this.SuitUpgradeProfileNames);

				}

				//UseResearchPointButtons
				if (tag.StartsWith("[UseResearchPointButtons:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseResearchPointButtons);

				}

				//ClearAuthorship
				if (tag.StartsWith("[ClearAuthorship:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ClearAuthorship);

				}

				//AttachModStorageComponentToGrid
				if (tag.StartsWith("[AttachModStorageComponentToGrid:") == true) {

					TagParse.TagBoolCheck(tag, ref this.AttachModStorageComponentToGrid);

				}

				//StorageKey
				if (tag.StartsWith("[StorageKey:") == true) {

					TagParse.TagGuidCheck(tag, ref this.StorageKey);

				}

				//StorageValue
				if (tag.StartsWith("[StorageValue:") == true) {

					TagParse.TagStringCheck(tag, ref this.StorageValue);

				}

				//UseLootProfiles
				if (tag.StartsWith("[UseLootProfiles:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseLootProfiles);

				}

				//LootProfiles
				if (tag.StartsWith("[LootProfiles:") == true) {

					TagParse.TagLootProfileCheck(tag, ref this.LootProfiles);

				}

				//LootGroups
				if (tag.StartsWith("[LootGroups:") == true) {

					TagParse.TagStringListCheck(tag, ref this.LootGroups);

				}

				//ClearExistingContainerTypes
				if (tag.StartsWith("[ClearExistingContainerTypes:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ClearExistingContainerTypes);

				}

				//OverrideLootChance
				if (tag.StartsWith("[OverrideLootChance:") == true) {

					TagParse.TagBoolCheck(tag, ref this.OverrideLootChance);

				}

				//LootChanceOverride
				if (tag.StartsWith("[LootChanceOverride:") == true) {

					TagParse.TagIntCheck(tag, ref this.LootChanceOverride);

				}

				//SetDoorsAnyoneCanUse
				if (tag.StartsWith("[SetDoorsAnyoneCanUse:") == true) {

					TagParse.TagBoolCheck(tag, ref this.SetDoorsAnyoneCanUse);

				}

				//SetStoresAnyoneCanUse
				if (tag.StartsWith("[SetStoresAnyoneCanUse:") == true) {

					TagParse.TagBoolCheck(tag, ref this.SetStoresAnyoneCanUse);

				}

				//SetConnectorsTradeMode
				if (tag.StartsWith("[SetConnectorsTradeMode:") == true) {

					TagParse.TagBoolCheck(tag, ref this.SetConnectorsTradeMode);

				}

			}

			//Build Dictionaries
			if (this.NonRandomWeaponNames.Count > 0 && this.NonRandomWeaponNames.Count == this.NonRandomWeaponIds.Count) {

				for (int i = 0; i < this.NonRandomWeaponNames.Count; i++) {

					if (!this.NonRandomWeaponReference.ContainsKey(this.NonRandomWeaponNames[i]))
						this.NonRandomWeaponReference.Add(this.NonRandomWeaponNames[i], this.NonRandomWeaponIds[i]);

				}

			}

			if (this.ReplaceBlockOld.Count > 0 && this.ReplaceBlockOld.Count == this.ReplaceBlockNew.Count) {

				for (int i = 0; i < this.ReplaceBlockOld.Count; i++) {

					if (!this.ReplaceBlockReference.ContainsKey(this.ReplaceBlockOld[i]))
						this.ReplaceBlockReference.Add(this.ReplaceBlockOld[i], this.ReplaceBlockNew[i]);

				}

			}

			if (this.ReplaceBlockNameOld.Count > 0 && this.ReplaceBlockNameOld.Count == this.ReplaceBlockNameNew.Count) {

				for (int i = 0; i < this.ReplaceBlockNameOld.Count; i++) {

					if (!this.BlockNameReplacerReference.ContainsKey(this.ReplaceBlockNameOld[i]))
						this.BlockNameReplacerReference.Add(this.ReplaceBlockNameOld[i], this.ReplaceBlockNameNew[i]);

				}

			}

			if (this.ContainerTypeAssignBlockName.Count > 0 && this.ContainerTypeAssignBlockName.Count == this.ContainerTypeAssignSubtypeId.Count) {

				for (int i = 0; i < this.ContainerTypeAssignBlockName.Count; i++) {

					if (!this.ContainerTypeAssignmentReference.ContainsKey(this.ContainerTypeAssignBlockName[i]))
						this.ContainerTypeAssignmentReference.Add(this.ContainerTypeAssignBlockName[i], this.ContainerTypeAssignSubtypeId[i]);

				}

			}

			if (this.RecolorOld.Count > 0 && this.RecolorOld.Count == this.RecolorNew.Count) {

				for (int i = 0; i < this.RecolorOld.Count; i++) {

					if (!this.ColorReferencePairs.ContainsKey(this.RecolorOld[i]))
						this.ColorReferencePairs.Add(this.RecolorOld[i], this.RecolorNew[i]);

				}

			}

			if (this.ReskinTarget.Count > 0 && this.ReskinTarget.Count == this.ReskinTexture.Count) {

				for (int i = 0; i < this.ReskinTarget.Count; i++) {

					if (!this.ColorSkinReferencePairs.ContainsKey(this.ReskinTarget[i]))
						this.ColorSkinReferencePairs.Add(this.ReskinTarget[i], this.ReskinTexture[i]);

				}

			}

			//Loot Groups
			foreach (var group in LootGroups) {

				if (string.IsNullOrWhiteSpace(group))
					continue;

				LootGroup lootGroup = null;

				if (ProfileManager.LootGroups.TryGetValue(group, out lootGroup)) {

					foreach (var profile in lootGroup.LootProfiles) {

						if (!LootProfiles.Contains(profile))
							LootProfiles.Add(profile);

					}

				}

			}

		}

	}
}
