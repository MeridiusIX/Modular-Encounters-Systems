using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Profiles {

	public enum PrefabSpawnMode {
	
		None,
		All,
		Random,
		SelectedIndexes,
		RandomSelectedIndexes,
		RandomFixedCount,
	
	}

	public class SpawnConditionsProfile {

		public string ProfileSubtypeId;

		public bool SpaceCargoShip;
		public bool LunarCargoShip;
		public bool AtmosphericCargoShip;
		public bool GravityCargoShip;

		public bool SkipAirDensityCheck;
		public bool CargoShipTerrainPath;
		public double CustomPathStartAltitude;
		public double CustomPathEndAltitude;

		public bool SpaceRandomEncounter;
		public bool AlignVoxelsToSpawnMatrix;
		public bool UseOptimizedVoxelSpawning;
		public List<string> CustomVoxelMaterial;

		public bool PlanetaryInstallation;
		public string PlanetaryInstallationType;
		public bool SkipTerrainCheck;
		public List<Vector3D> RotateInstallations;
		public List<bool> ReverseForwardDirections;
		public bool InstallationTerrainValidation;
		public bool InstallationSpawnsOnDryLand;
		public bool InstallationSpawnsUnderwater;
		public bool InstallationSpawnsOnWaterSurface;

		public bool CutVoxelsAtAirtightCells;
		public double CutVoxelSize;

		public bool BossEncounterSpace;
		public bool BossEncounterAtmo;
		public bool BossEncounterAny;

		public bool RivalAiSpawn;
		public bool RivalAiSpaceSpawn;
		public bool RivalAiAtmosphericSpawn;
		public bool RivalAiAnySpawn;

		public bool DroneEncounter;
		public int MinimumPlayerTime;
		public int MaximumPlayerTime;
		public bool FailedDroneSpawnResetsPlayerTime;
		public double MinDroneDistance;
		public double MaxDroneDistance;
		public double MinDroneAltitude;
		public double MaxDroneAltitude;
		public bool DroneInheritsSourceAltitude;

		public bool SkipVoxelSpawnChecks;
		public bool SkipGridSpawnChecks;

		public bool CreatureSpawn;
		public List<string> CreatureIds;
		public List<BotSpawnProfile> BotProfiles;
		public bool AiEnabledReady;
		public bool AiEnabledModBots;
		public string AiEnabledRole;
		public int MinCreatureCount;
		public int MaxCreatureCount;
		public int MinCreatureDistance;
		public int MaxCreatureDistance;
		public bool RegisterCreatureToPlanetGenerators;
		public int MinDistFromOtherCreaturesInGroup;

		public bool CanSpawnUnderwater;
		public bool MustSpawnUnderwater;
		public double MinWaterDepth;

		public bool StaticEncounter;
		public bool UniqueEncounter;

		public Vector3D TriggerCoords;
		public double TriggerRadius;

		public Vector3D StaticEncounterCoords;
		public Vector3D StaticEncounterForward;
		public Vector3D StaticEncounterUp;

		public bool StaticEncounterUsePlanetDirectionAndAltitude;
		public string StaticEncounterPlanet;
		public Vector3D StaticEncounterPlanetDirection;
		public double StaticEncounterPlanetAltitude;

		public bool ForceStaticGrid;
		public bool DoNotForceStaticGrid;
		public bool ForceExactPositionAndOrientation;
		public bool AdminSpawnOnly;

		public bool MustSpawnInPlanetaryLane;
		public string PlanetaryLanePlanetNameA;
		public string PlanetaryLanePlanetNameB;

		public string FactionOwner;
		public bool UseRandomMinerFaction;
		public bool UseRandomBuilderFaction;
		public bool UseRandomTraderFaction;

		public int RandomNumberRoll;
		public int ChanceCeiling;
		public int SpaceCargoShipChance;
		public int LunarCargoShipChance;
		public int AtmosphericCargoShipChance;
		public int GravityCargoShipChance;
		public int RandomEncounterChance;
		public int PlanetaryInstallationChance;
		public int BossEncounterChance;
		public int CreatureChance;
		public int DroneEncounterChance; //Doc

		public bool UseSettingsCheck; //Doc
		public BoolEnum SettingsAutoHeal; //Doc
		public BoolEnum SettingsAutoRespawn; //Doc
		public BoolEnum SettingsBountyContracts; //Doc
		public BoolEnum SettingsDestructibleBlocks; //Doc
		public BoolEnum SettingsCopyPaste; //Doc
		public BoolEnum SettingsContainerDrops; //Doc
		public BoolEnum SettingsEconomy; //Doc
		public BoolEnum SettingsEnableDrones; //Doc
		public BoolEnum SettingsIngameScripts; //Doc
		public BoolEnum SettingsJetpack; //Doc
		public BoolEnum SettingsOxygen; //Doc
		public BoolEnum SettingsResearch; //Doc
		public BoolEnum SettingsSpawnWithTools; //Doc
		public BoolEnum SettingsSpiders; //Doc
		public BoolEnum SettingsSubgridDamage; //Doc
		public BoolEnum SettingsSunRotation; //Doc
		public BoolEnum SettingsSupergridding; //Doc
		public BoolEnum SettingsThrusterDamage; //Doc
		public BoolEnum SettingsVoxelDestruction; //Doc
		public BoolEnum SettingsWeaponsEnabled; //Doc
		public BoolEnum SettingsWeather; //Doc
		public BoolEnum SettingsWolves; //Doc

		public bool UsesAerodynamicModAdvLift;

		public bool UseDateTimeYearRange;
		public int MinDateTimeYear;
		public int MaxDateTimeYear;

		public bool UseDateTimeMonthRange;
		public int MinDateTimeMonth;
		public int MaxDateTimeMonth;

		public bool UseDateTimeDayRange;
		public int MinDateTimeDay;
		public int MaxDateTimeDay;

		public bool UseDateTimeHourRange;
		public int MinDateTimeHour;
		public int MaxDateTimeHour;

		public bool UseDateTimeMinuteRange;
		public int MinDateTimeMinute;
		public int MaxDateTimeMinute;

		public bool UseDateTimeDaysOfWeek;
		//public List<DayOfWeek> DateTimeDaysOfWeek;

		public double MinSpawnFromWorldCenter;
		public double MaxSpawnFromWorldCenter;
		public Vector3D CustomWorldCenter;
		public List<Vector3D> DirectionFromWorldCenter;
		public double MinAngleFromDirection;
		public double MaxAngleFromDirection;

		public List<Vector3D> DirectionFromPlanetCenter;
		public double MinAngleFromPlanetCenterDirection;
		public double MaxAngleFromPlanetCenterDirection;

		public double MinSpawnFromPlanetSurface;
		public double MaxSpawnFromPlanetSurface;

		public bool UseDayOrNightOnly;
		public bool SpawnOnlyAtNight;

		public bool UseWeatherSpawning;
		public List<string> AllowedWeatherSystems;

		public bool UseTerrainTypeValidation;
		public List<string> AllowedTerrainTypes;

		public double MinAirDensity;
		public double MaxAirDensity;
		public double MinGravity;
		public double MaxGravity;

		public bool CheckPrefabGravityProfiles;

		public List<string> PlanetBlacklist;
		public List<string> PlanetWhitelist;
		public bool PlanetRequiresVacuum;
		public bool PlanetRequiresAtmo;
		public bool PlanetRequiresOxygen;
		public double PlanetMinimumSize;
		public double PlanetMaximumSize;

		public bool UsePlayerCountCheck;
		public double PlayerCountCheckRadius;
		public int MinimumPlayers;
		public int MaximumPlayers;

		public bool UseThreatLevelCheck;
		public double ThreatLevelCheckRange;
		public bool ThreatIncludeOtherNpcOwners;
		public int ThreatScoreMinimum;
		public int ThreatScoreMaximum;
		public int ThreatScorePlanetaryHandicap;
		public Entities.GridConfigurationEnum ThreatScoreGridConfiguration;

		public bool UsePCUCheck;
		public double PCUCheckRadius;
		public int PCUMinimum;
		public int PCUMaximum;

		public bool UseDifficulty;
		public int MinDifficulty;
		public int MaxDifficulty;

		public bool UseCombatPhase;
		public bool IgnoreCombatPhase;
		public bool CombatPhaseChecksInPersistentCondition;

		public bool UsePlayerCredits;
		public bool IncludeAllPlayersInRadius;
		public bool IncludeFactionBalance;
		public double PlayerCreditsCheckRadius;
		public int MinimumPlayerCredits;
		public int MaximumPlayerCredits;

		public bool UsePlayerFactionReputation;
		public double PlayerReputationCheckRadius;
		public string CheckReputationAgainstOtherNPCFaction;
		public int MinimumReputation;
		public int MaximumReputation;

		public bool UsePlayerCondition;
		public double PlayerConditionCheckRadius;
		public List<string> PlayerConditionIds;


		public bool UseSignalRequirement;
		public double MinSignalRadius;
		public double MaxSignalRadius;
		public bool AllowNpcSignals;
		public bool UseOnlySelfOwnedSignals;
		public string MatchSignalName; //Implement //Doc

		public bool ChargeNpcFactionForSpawn;
		public long ChargeForSpawning;

		public bool UseSandboxCounterCosts;
		public List<string> SandboxCounterCostNames;
		public List<int> SandboxCounterCostAmounts;

		public List<string> SandboxVariables;
		public List<string> FalseSandboxVariables;

		public bool CheckCustomSandboxCounters;
		public List<string> CustomSandboxCounters;
		public List<int> CustomSandboxCountersTargets;
		public List<CounterCompareEnum> SandboxCounterCompareTypes;



		public bool UseRemoteControlCodeRestrictions;
		public string RemoteControlCode;
		public double RemoteControlCodeMinDistance;
		public double RemoteControlCodeMaxDistance;

		public List<ulong> RequireAllMods;
		public List<ulong> RequireAnyMods;
		public List<ulong> ExcludeAllMods;
		public List<ulong> ExcludeAnyMods;

		public List<string> ModBlockExists;

		public List<ulong> RequiredPlayersOnline;
		public List<ulong> RequiredAnyPlayersOnline;

		public bool UseKnownPlayerLocations;
		public bool KnownPlayerLocationMustBeInside;
		public bool KnownPlayerLocationMustMatchFaction;
		public int KnownPlayerLocationMinSpawnedEncounters;
		public int KnownPlayerLocationMaxSpawnedEncounters;

		public List<ZoneConditionsProfile> ZoneConditions;

		public List<string> CustomApiConditions;

		public bool BossCustomAnnounceEnable;
		public string BossCustomAnnounceAuthor;
		public string BossCustomAnnounceMessage;
		public string BossCustomGPSLabel;
		public Vector3D BossCustomGPSColor;
		public string BossMusicId; //Implement //Doc

		public bool PlaySoundAtSpawnTriggerPosition;
		public string SpawnTriggerPositionSoundId;

		public bool RotateFirstCockpitToForward;
		public bool PositionAtFirstCockpit;
		public bool SpawnRandomCargo;
		public bool DisableDampeners;
		public bool ReactorsOn;
		public bool UseBoundingBoxCheck;
		public bool RemoveVoxelsIfGridRemoved;
		public bool UseGridOrigin;

		public PrefabSpawnMode PrefabSpawningMode;
		public bool AllowPrefabIndexReuse;
		public List<int> PrefabIndexes;
		public Dictionary<string, List<int>> PrefabIndexGroups;
		public List<string> PrefabIndexGroupNames;
		public List<int> PrefabIndexGroupValues;
		public int PrefabFixedCount;
		public List<Vector3D> PrefabOffsetOverrides;
		public bool UseSpawnGroupPrefabSpawningMode;

		public bool UseEventController;
		public List<string> EventControllerId;


		public bool CheckRequiredBlocks;
		public List<string> RequiredBlockSubtypeIds;
		public bool RequiredBlockAnySubtypeId;
		public double RequiredBlockCheckRange;
		public bool RequiredBlockIncludeNPCGrids;


		public Dictionary<string, Action<string, object>> EditorReference;


		public SpawnConditionsProfile() {

			ProfileSubtypeId = "";

			SpaceCargoShip = false;
			LunarCargoShip = false;
			AtmosphericCargoShip = false;
			GravityCargoShip = false;

			SkipAirDensityCheck = false;
			CargoShipTerrainPath = false;
			CustomPathStartAltitude = -1;
			CustomPathEndAltitude = -1;

			SpaceRandomEncounter = false;
			AlignVoxelsToSpawnMatrix = false;
			UseOptimizedVoxelSpawning = false;
			CustomVoxelMaterial = new List<string>();

			PlanetaryInstallation = false;
			PlanetaryInstallationType = "Small";
			SkipTerrainCheck = false;
			RotateInstallations = new List<Vector3D>();
			ReverseForwardDirections = new List<bool>();
			InstallationTerrainValidation = false;
			InstallationSpawnsOnDryLand = true;
			InstallationSpawnsUnderwater = false;
			InstallationSpawnsOnWaterSurface = false;

			CutVoxelsAtAirtightCells = false;
			CutVoxelSize = 2.7;

			BossEncounterSpace = false;
			BossEncounterAtmo = false;
			BossEncounterAny = false;

			RivalAiSpawn = false;
			RivalAiSpaceSpawn = false;
			RivalAiAtmosphericSpawn = false;
			RivalAiAnySpawn = false;

			DroneEncounter = false;
			MinimumPlayerTime = -1;
			MaximumPlayerTime = 0;
			FailedDroneSpawnResetsPlayerTime = false;
			MinDroneDistance = 1000;
			MaxDroneDistance = 1000;
			MinDroneAltitude = 1000;
			MaxDroneAltitude = 1000;
			DroneInheritsSourceAltitude = false;

			SkipVoxelSpawnChecks = false;
			SkipGridSpawnChecks = false;

			CreatureSpawn = false;
			CreatureIds = new List<string>();
			BotProfiles = new List<BotSpawnProfile>();
			AiEnabledModBots = false;
			AiEnabledReady = false;
			AiEnabledRole = "";
			MinCreatureCount = 1;
			MaxCreatureCount = 1;
			MinCreatureDistance = 100;
			MaxCreatureDistance = 150;
			RegisterCreatureToPlanetGenerators = false;
			MinDistFromOtherCreaturesInGroup = 5;

			CanSpawnUnderwater = false;
			MustSpawnUnderwater = false;
			MinWaterDepth = 0;

			StaticEncounter = false;
			UniqueEncounter = false;

			TriggerCoords = Vector3D.Zero;
			TriggerRadius = 0;

			StaticEncounterCoords = Vector3D.Zero;
			StaticEncounterForward = Vector3D.Zero;
			StaticEncounterUp = Vector3D.Zero;

			StaticEncounterUsePlanetDirectionAndAltitude = false;
			StaticEncounterPlanet = "";
			StaticEncounterPlanetDirection = Vector3D.Zero;
			StaticEncounterPlanetAltitude = 0;

			ForceStaticGrid = false;
			DoNotForceStaticGrid = false;
			ForceExactPositionAndOrientation = false;
			AdminSpawnOnly = false;

			MustSpawnInPlanetaryLane = false;
			PlanetaryLanePlanetNameA = "";
			PlanetaryLanePlanetNameB = "";

			FactionOwner = "SPRT";
			UseRandomMinerFaction = false;
			UseRandomBuilderFaction = false;
			UseRandomTraderFaction = false;

			RandomNumberRoll = 1;
			ChanceCeiling = 100;
			SpaceCargoShipChance = 100;
			LunarCargoShipChance = 100;
			AtmosphericCargoShipChance = 100;
			GravityCargoShipChance = 100;
			RandomEncounterChance = 100;
			PlanetaryInstallationChance = 100;
			BossEncounterChance = 100;
			CreatureChance = 100;
			DroneEncounterChance = 100;

			UseSettingsCheck = false;
			SettingsAutoHeal = BoolEnum.None;
			SettingsAutoRespawn = BoolEnum.None;
			SettingsBountyContracts = BoolEnum.None;
			SettingsDestructibleBlocks = BoolEnum.None;
			SettingsCopyPaste = BoolEnum.None;
			SettingsContainerDrops = BoolEnum.None;
			SettingsEconomy = BoolEnum.None;
			SettingsEnableDrones = BoolEnum.None;
			SettingsIngameScripts = BoolEnum.None;
			SettingsJetpack = BoolEnum.None;
			SettingsOxygen = BoolEnum.None;
			SettingsResearch = BoolEnum.None;
			SettingsSpawnWithTools = BoolEnum.None;
			SettingsSpiders = BoolEnum.None;
			SettingsSubgridDamage = BoolEnum.None;
			SettingsSunRotation = BoolEnum.None;
			SettingsSupergridding = BoolEnum.None;
			SettingsThrusterDamage = BoolEnum.None;
			SettingsVoxelDestruction = BoolEnum.None;
			SettingsWeaponsEnabled = BoolEnum.None;
			SettingsWeather = BoolEnum.None;
			SettingsWolves = BoolEnum.None;

			UsesAerodynamicModAdvLift = false;

			UseDateTimeYearRange = false;
			MinDateTimeYear = -1;
			MaxDateTimeYear = -1;

			UseDateTimeMonthRange = false;
			MinDateTimeMonth = -1;
			MaxDateTimeMonth = -1;

			UseDateTimeDayRange = false;
			MinDateTimeDay = -1;
			MaxDateTimeDay = -1;

			UseDateTimeHourRange = false;
			MinDateTimeHour = -1;
			MaxDateTimeHour = -1;

			UseDateTimeMinuteRange = false;
			MinDateTimeMinute = -1;
			MaxDateTimeMinute = -1;

			UseDateTimeDaysOfWeek = false;
			//DateTimeDaysOfWeek = new List<DayOfWeek>();

			CheckCustomSandboxCounters= false;
			CustomSandboxCounters = new List<string>();
			CustomSandboxCountersTargets = new List<int>();
			SandboxCounterCompareTypes = new List<CounterCompareEnum>();

			SandboxVariables = new List<string>();
			FalseSandboxVariables = new List<string>();

			MinSpawnFromWorldCenter = -1;
			MaxSpawnFromWorldCenter = -1;
			CustomWorldCenter = Vector3D.Zero;
			DirectionFromWorldCenter = new List<Vector3D>();
			MinAngleFromDirection = -1;
			MaxAngleFromDirection = -1;

			DirectionFromPlanetCenter = new List<Vector3D>();
			MinAngleFromPlanetCenterDirection = -1;
			MaxAngleFromPlanetCenterDirection = -1;

			MinSpawnFromPlanetSurface = -1;
			MaxSpawnFromPlanetSurface = -1;

			UseDayOrNightOnly = false;
			SpawnOnlyAtNight = false;

			UseWeatherSpawning = false;
			AllowedWeatherSystems = new List<string>();

			UseTerrainTypeValidation = false;
			AllowedTerrainTypes = new List<string>();

			MinAirDensity = -1;
			MaxAirDensity = -1;
			MinGravity = -1;
			MaxGravity = -1;

			CheckPrefabGravityProfiles = false;

			PlanetBlacklist = new List<string>();
			PlanetWhitelist = new List<string>();
			PlanetRequiresVacuum = false;
			PlanetRequiresAtmo = false;
			PlanetRequiresOxygen = false;
			PlanetMinimumSize = -1;
			PlanetMaximumSize = -1;

			UsePlayerCountCheck = false;
			PlayerCountCheckRadius = -1;
			MinimumPlayers = -1;
			MaximumPlayers = -1;

			UseThreatLevelCheck = false;
			ThreatLevelCheckRange = 5000;
			ThreatIncludeOtherNpcOwners = false;
			ThreatScoreMinimum = -1;
			ThreatScoreMaximum = -1;
			ThreatScorePlanetaryHandicap = 0;
			ThreatScoreGridConfiguration = Entities.GridConfigurationEnum.All;

			UsePCUCheck = false;
			PCUCheckRadius = 5000;
			PCUMinimum = -1;
			PCUMaximum = -1;

			UseDifficulty = false;
			MinDifficulty = -1;
			MaxDifficulty = -1;

			UseCombatPhase = false;
			IgnoreCombatPhase = false;
			CombatPhaseChecksInPersistentCondition = false;

			UsePlayerCredits = false;
			IncludeAllPlayersInRadius = false;
			IncludeFactionBalance = false;
			PlayerCreditsCheckRadius = 15000;
			MinimumPlayerCredits = -1;
			MaximumPlayerCredits = -1;

			UsePlayerFactionReputation = false;
			PlayerReputationCheckRadius = 15000;
			CheckReputationAgainstOtherNPCFaction = "";
			MinimumReputation = -1501;
			MaximumReputation = 1501;

			UsePlayerCondition = false;
			PlayerConditionCheckRadius = 10000;
			PlayerConditionIds = new List<string>();

			UseSignalRequirement = false;
			MinSignalRadius = -1;
			MaxSignalRadius = -1;
			AllowNpcSignals = false;
			UseOnlySelfOwnedSignals = false;
			MatchSignalName = null;

			ChargeNpcFactionForSpawn = false;
			ChargeForSpawning = 0;

			UseSandboxCounterCosts = false;
			SandboxCounterCostNames = new List<string>();
			SandboxCounterCostAmounts = new List<int>();

			UseRemoteControlCodeRestrictions = false;
			RemoteControlCode = "";
			RemoteControlCodeMinDistance = -1;
			RemoteControlCodeMaxDistance = -1;

			RequireAllMods = new List<ulong>();
			RequireAnyMods = new List<ulong>();
			ExcludeAllMods = new List<ulong>();
			ExcludeAnyMods = new List<ulong>();

			ModBlockExists = new List<string>();

			RequiredPlayersOnline = new List<ulong>();
			RequiredAnyPlayersOnline = new List<ulong>();

			UseKnownPlayerLocations = false;
			KnownPlayerLocationMustBeInside = true;
			KnownPlayerLocationMustMatchFaction = false;
			KnownPlayerLocationMinSpawnedEncounters = -1;
			KnownPlayerLocationMaxSpawnedEncounters = -1;

			ZoneConditions = new List<ZoneConditionsProfile>();
			ZoneConditions.Add(new ZoneConditionsProfile());

			CustomApiConditions = new List<string>();

			BossCustomAnnounceEnable = false;
			BossCustomAnnounceAuthor = "";
			BossCustomAnnounceMessage = "";
			BossCustomGPSLabel = "Dangerous Encounter";
			BossCustomGPSColor = new Vector3D(255, 0, 255);
			BossMusicId = "";

			PlaySoundAtSpawnTriggerPosition = false;
			SpawnTriggerPositionSoundId = "";

			RotateFirstCockpitToForward = true;
			PositionAtFirstCockpit = false;
			SpawnRandomCargo = true;
			DisableDampeners = false;
			ReactorsOn = true;
			UseBoundingBoxCheck = false;
			RemoveVoxelsIfGridRemoved = true;
			UseGridOrigin = false;

			PrefabSpawningMode = PrefabSpawnMode.All;
			AllowPrefabIndexReuse = false;
			PrefabIndexes = new List<int>();
			PrefabIndexGroups = new Dictionary<string, List<int>>();
			PrefabIndexGroupNames = new List<string>();
			PrefabIndexGroupValues = new List<int>();
			PrefabFixedCount = 1;
			PrefabOffsetOverrides = new List<Vector3D>();
			UseSpawnGroupPrefabSpawningMode = false;
			UseEventController = false;
			EventControllerId = new List<string>();

			CheckRequiredBlocks = false;
			RequiredBlockSubtypeIds = new List<string>();
			RequiredBlockAnySubtypeId = false;
			RequiredBlockCheckRange = 5000;
			RequiredBlockIncludeNPCGrids = false;

			EditorReference = new Dictionary<string, Action<string, object>> {

				{"SpaceCargoShip", (s, o) => TagParse.TagBoolCheck(s, ref SpaceCargoShip) },
				{"LunarCargoShip", (s, o) => TagParse.TagBoolCheck(s, ref LunarCargoShip) },
				{"AtmosphericCargoShip", (s, o) => TagParse.TagBoolCheck(s, ref AtmosphericCargoShip) },
				{"PlanetaryCargoShip", (s, o) => TagParse.TagBoolCheck(s, ref AtmosphericCargoShip) },
				{"GravityCargoShip", (s, o) => TagParse.TagBoolCheck(s, ref GravityCargoShip) },
				{"SkipAirDensityCheck", (s, o) => TagParse.TagBoolCheck(s, ref SkipAirDensityCheck) },
				{"CargoShipTerrainPath", (s, o) => TagParse.TagBoolCheck(s, ref CargoShipTerrainPath) },
				{"CustomPathStartAltitude", (s, o) => TagParse.TagDoubleCheck(s, ref CustomPathStartAltitude) },
				{"CustomPathEndAltitude", (s, o) => TagParse.TagDoubleCheck(s, ref CustomPathEndAltitude) },
				{"SpaceRandomEncounter", (s, o) => TagParse.TagBoolCheck(s, ref SpaceRandomEncounter) },
				{"AlignVoxelsToSpawnMatrix", (s, o) => TagParse.TagBoolCheck(s, ref AlignVoxelsToSpawnMatrix) },
				{"UseOptimizedVoxelSpawning", (s, o) => TagParse.TagBoolCheck(s, ref UseOptimizedVoxelSpawning) },
				{"CustomVoxelMaterial", (s, o) => TagParse.TagStringListCheck(s, ref CustomVoxelMaterial) },
				{"PlanetaryInstallation", (s, o) => TagParse.TagBoolCheck(s, ref PlanetaryInstallation) },
				{"PlanetaryInstallationType", (s, o) => TagParse.TagStringCheck(s, ref PlanetaryInstallationType) },
				{"SkipTerrainCheck", (s, o) => TagParse.TagBoolCheck(s, ref SkipTerrainCheck) },
				{"RotateInstallations", (s, o) => TagParse.TagVector3DListCheck(s, ref RotateInstallations) },
				{"ReverseForwardDirections", (s, o) => TagParse.TagBoolListCheck(s, ref ReverseForwardDirections) },
				{"InstallationTerrainValidation", (s, o) => TagParse.TagBoolCheck(s, ref InstallationTerrainValidation) },
				{"InstallationSpawnsOnDryLand", (s, o) => TagParse.TagBoolCheck(s, ref InstallationSpawnsOnDryLand) },
				{"InstallationSpawnsUnderwater", (s, o) => TagParse.TagBoolCheck(s, ref InstallationSpawnsUnderwater) },
				{"InstallationSpawnsOnWaterSurface", (s, o) => TagParse.TagBoolCheck(s, ref InstallationSpawnsOnWaterSurface) },
				{"CutVoxelsAtAirtightCells", (s, o) => TagParse.TagBoolCheck(s, ref CutVoxelsAtAirtightCells) },
				{"CutVoxelSize", (s, o) => TagParse.TagDoubleCheck(s, ref CutVoxelSize) },
				{"BossEncounterSpace", (s, o) => TagParse.TagBoolCheck(s, ref BossEncounterSpace) },
				{"BossEncounterAtmo", (s, o) => TagParse.TagBoolCheck(s, ref BossEncounterAtmo) },
				{"BossEncounterAny", (s, o) => TagParse.TagBoolCheck(s, ref BossEncounterAny) },
				{"RivalAiSpawn", (s, o) => TagParse.TagBoolCheck(s, ref RivalAiSpawn) },
				{"BehaviorSpawn", (s, o) => TagParse.TagBoolCheck(s, ref RivalAiSpawn) },
				{"RivalAiSpaceSpawn", (s, o) => TagParse.TagBoolCheck(s, ref RivalAiSpaceSpawn) },
				{"RivalAiAtmosphericSpawn", (s, o) => TagParse.TagBoolCheck(s, ref RivalAiAtmosphericSpawn) },
				{"RivalAiAnySpawn", (s, o) => TagParse.TagBoolCheck(s, ref RivalAiAnySpawn) },
				{"DroneEncounter", (s, o) => TagParse.TagBoolCheck(s, ref DroneEncounter) },
				{"MinimumPlayerTime", (s, o) => TagParse.TagIntCheck(s, ref MinimumPlayerTime) },
				{"MaximumPlayerTime", (s, o) => TagParse.TagIntCheck(s, ref MaximumPlayerTime) },
				{"FailedDroneSpawnResetsPlayerTime", (s, o) => TagParse.TagBoolCheck(s, ref FailedDroneSpawnResetsPlayerTime) },
				{"MinDroneDistance", (s, o) => TagParse.TagDoubleCheck(s, ref MinDroneDistance) },
				{"MaxDroneDistance", (s, o) => TagParse.TagDoubleCheck(s, ref MaxDroneDistance) },
				{"MinDroneAltitude", (s, o) => TagParse.TagDoubleCheck(s, ref MinDroneAltitude) },
				{"MaxDroneAltitude", (s, o) => TagParse.TagDoubleCheck(s, ref MaxDroneAltitude) },
				{"DroneInheritsSourceAltitude", (s, o) => TagParse.TagBoolCheck(s, ref DroneInheritsSourceAltitude) },
				{"SkipVoxelSpawnChecks", (s, o) => TagParse.TagBoolCheck(s, ref SkipVoxelSpawnChecks) },
				{"SkipGridSpawnChecks", (s, o) => TagParse.TagBoolCheck(s, ref SkipGridSpawnChecks) },
				{"CreatureSpawn", (s, o) => TagParse.TagBoolCheck(s, ref CreatureSpawn) },
				{"CreatureIds", (s, o) => TagParse.TagStringListCheck(s, ref CreatureIds) },
				{"BotProfiles", (s, o) => TagParse.TagBotProfileListCheck(s, ref BotProfiles) },
				{"AiEnabledReady", (s, o) => TagParse.TagBoolCheck(s, ref AiEnabledReady) },
				{"AiEnabledModBots", (s, o) => TagParse.TagBoolCheck(s, ref AiEnabledModBots) },
				{"AiEnabledRole", (s, o) => TagParse.TagStringCheck(s, ref AiEnabledRole) },
				{"MinCreatureCount", (s, o) => TagParse.TagIntCheck(s, ref MinCreatureCount) },
				{"MaxCreatureCount", (s, o) => TagParse.TagIntCheck(s, ref MaxCreatureCount) },
				{"MinCreatureDistance", (s, o) => TagParse.TagIntCheck(s, ref MinCreatureDistance) },
				{"MaxCreatureDistance", (s, o) => TagParse.TagIntCheck(s, ref MaxCreatureDistance) },
				{"RegisterCreatureToPlanetGenerators", (s, o) => TagParse.TagBoolCheck(s, ref RegisterCreatureToPlanetGenerators) },
				{"MinDistFromOtherCreaturesInGroup", (s, o) => TagParse.TagIntCheck(s, ref MinDistFromOtherCreaturesInGroup) },
				{"CanSpawnUnderwater", (s, o) => TagParse.TagBoolCheck(s, ref CanSpawnUnderwater) },
				{"MustSpawnUnderwater", (s, o) => TagParse.TagBoolCheck(s, ref MustSpawnUnderwater) },
				{"MinWaterDepth", (s, o) => TagParse.TagDoubleCheck(s, ref MinWaterDepth) },
				{"StaticEncounter", (s, o) => TagParse.TagBoolCheck(s, ref StaticEncounter) },
				{"UniqueEncounter", (s, o) => TagParse.TagBoolCheck(s, ref UniqueEncounter) },
				{"TriggerCoords", (s, o) => TagParse.TagVector3DCheck(s, ref TriggerCoords) },
				{"TriggerRadius", (s, o) => TagParse.TagDoubleCheck(s, ref TriggerRadius) },
				{"StaticEncounterCoords", (s, o) => TagParse.TagVector3DCheck(s, ref StaticEncounterCoords) },
				{"StaticEncounterForward", (s, o) => TagParse.TagVector3DCheck(s, ref StaticEncounterForward) },
				{"StaticEncounterUp", (s, o) => TagParse.TagVector3DCheck(s, ref StaticEncounterUp) },
				{"StaticEncounterUsePlanetDirectionAndAltitude", (s, o) => TagParse.TagBoolCheck(s, ref StaticEncounterUsePlanetDirectionAndAltitude) },
				{"StaticEncounterPlanet", (s, o) => TagParse.TagStringCheck(s, ref StaticEncounterPlanet) },
				{"StaticEncounterPlanetDirection", (s, o) => TagParse.TagVector3DCheck(s, ref StaticEncounterPlanetDirection) },
				{"StaticEncounterPlanetAltitude", (s, o) => TagParse.TagDoubleCheck(s, ref StaticEncounterPlanetAltitude) },
				{"ForceStaticGrid", (s, o) => TagParse.TagBoolCheck(s, ref ForceStaticGrid) },
				{"DoNotForceStaticGrid", (s, o) => TagParse.TagBoolCheck(s, ref DoNotForceStaticGrid) },

				{"ForceExactPositionAndOrientation", (s, o) => TagParse.TagBoolCheck(s, ref ForceExactPositionAndOrientation) },
				{"AdminSpawnOnly", (s, o) => TagParse.TagBoolCheck(s, ref AdminSpawnOnly) },
				{"MustSpawnInPlanetaryLane", (s, o) => TagParse.TagBoolCheck(s, ref MustSpawnInPlanetaryLane) },
				{"PlanetaryLanePlanetNameA", (s, o) => TagParse.TagStringCheck(s, ref PlanetaryLanePlanetNameA) },
				{"PlanetaryLanePlanetNameB", (s, o) => TagParse.TagStringCheck(s, ref PlanetaryLanePlanetNameB) },
				{"FactionOwner", (s, o) => TagParse.TagStringCheck(s, ref FactionOwner) },
				{"UseRandomMinerFaction", (s, o) => TagParse.TagBoolCheck(s, ref UseRandomMinerFaction) },
				{"UseRandomBuilderFaction", (s, o) => TagParse.TagBoolCheck(s, ref UseRandomBuilderFaction) },
				{"UseRandomTraderFaction", (s, o) => TagParse.TagBoolCheck(s, ref UseRandomTraderFaction) },
				{"RandomNumberRoll", (s, o) => TagParse.TagIntCheck(s, ref RandomNumberRoll) },
				{"ChanceCeiling", (s, o) => TagParse.TagIntCheck(s, ref ChanceCeiling) },
				{"SpaceCargoShipChance", (s, o) => TagParse.TagIntCheck(s, ref SpaceCargoShipChance) },
				{"LunarCargoShipChance", (s, o) => TagParse.TagIntCheck(s, ref LunarCargoShipChance) },
				{"AtmosphericCargoShipChance", (s, o) => TagParse.TagIntCheck(s, ref AtmosphericCargoShipChance) },
				{"GravityCargoShipChance", (s, o) => TagParse.TagIntCheck(s, ref GravityCargoShipChance) },
				{"RandomEncounterChance", (s, o) => TagParse.TagIntCheck(s, ref RandomEncounterChance) },
				{"PlanetaryInstallationChance", (s, o) => TagParse.TagIntCheck(s, ref PlanetaryInstallationChance) },
				{"BossEncounterChance", (s, o) => TagParse.TagIntCheck(s, ref BossEncounterChance) },
				{"CreatureChance", (s, o) => TagParse.TagIntCheck(s, ref CreatureChance) },
				{"DroneEncounterChance", (s, o) => TagParse.TagIntCheck(s, ref DroneEncounterChance) },
				{"UseSettingsCheck", (s, o) => TagParse.TagBoolCheck(s, ref UseSettingsCheck) },
				{"SettingsAutoHeal", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsAutoHeal) },
				{"SettingsAutoRespawn", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsAutoRespawn) },
				{"SettingsBountyContracts", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsBountyContracts) },
				{"SettingsDestructibleBlocks", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsDestructibleBlocks) },
				{"SettingsCopyPaste", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsCopyPaste) },
				{"SettingsContainerDrops", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsContainerDrops) },
				{"SettingsEconomy", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsEconomy) },
				{"SettingsEnableDrones", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsEnableDrones) },
				{"SettingsIngameScripts", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsIngameScripts) },
				{"SettingsJetpack", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsJetpack) },
				{"SettingsOxygen", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsOxygen) },
				{"SettingsResearch", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsResearch) },
				{"SettingsSpawnWithTools", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsSpawnWithTools) },
				{"SettingsSpiders", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsSpiders) },
				{"SettingsSubgridDamage", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsSubgridDamage) },
				{"SettingsSunRotation", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsSunRotation) },
				{"SettingsSupergridding", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsSupergridding) },
				{"SettingsThrusterDamage", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsThrusterDamage) },
				{"SettingsVoxelDestruction", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsVoxelDestruction) },
				{"SettingsWeaponsEnabled", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsWeaponsEnabled) },
				{"SettingsWeather", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsWeather) },
				{"SettingsWolves", (s, o) => TagParse.TagBoolEnumCheck(s, ref SettingsWolves) },
				{"UsesAerodynamicModAdvLift", (s, o) => TagParse.TagBoolCheck(s, ref UsesAerodynamicModAdvLift) },
				{"UseDateTimeYearRange", (s, o) => TagParse.TagBoolCheck(s, ref UseDateTimeYearRange) },
				{"MinDateTimeYear", (s, o) => TagParse.TagIntCheck(s, ref MinDateTimeYear) },
				{"MaxDateTimeYear", (s, o) => TagParse.TagIntCheck(s, ref MaxDateTimeYear) },
				{"UseDateTimeMonthRange", (s, o) => TagParse.TagBoolCheck(s, ref UseDateTimeMonthRange) },
				{"MinDateTimeMonth", (s, o) => TagParse.TagIntCheck(s, ref MinDateTimeMonth) },
				{"MaxDateTimeMonth", (s, o) => TagParse.TagIntCheck(s, ref MaxDateTimeMonth) },
				{"UseDateTimeDayRange", (s, o) => TagParse.TagBoolCheck(s, ref UseDateTimeDayRange) },
				{"MinDateTimeDay", (s, o) => TagParse.TagIntCheck(s, ref MinDateTimeDay) },
				{"MaxDateTimeDay", (s, o) => TagParse.TagIntCheck(s, ref MaxDateTimeDay) },
				{"UseDateTimeHourRange", (s, o) => TagParse.TagBoolCheck(s, ref UseDateTimeHourRange) },
				{"MinDateTimeHour", (s, o) => TagParse.TagIntCheck(s, ref MinDateTimeHour) },
				{"MaxDateTimeHour", (s, o) => TagParse.TagIntCheck(s, ref MaxDateTimeHour) },
				{"UseDateTimeMinuteRange", (s, o) => TagParse.TagBoolCheck(s, ref UseDateTimeMinuteRange) },
				{"MinDateTimeMinute", (s, o) => TagParse.TagIntCheck(s, ref MinDateTimeMinute) },
				{"MaxDateTimeMinute", (s, o) => TagParse.TagIntCheck(s, ref MaxDateTimeMinute) },
				{"UseDateTimeDaysOfWeek", (s, o) => TagParse.TagBoolCheck(s, ref UseDateTimeDaysOfWeek) },
				//{"DateTimeDaysOfWeek", (s, o) => TagParse.TagDayOfWeekEnumCheck(s, ref DateTimeDaysOfWeek) },
				{"MinSpawnFromWorldCenter", (s, o) => TagParse.TagDoubleCheck(s, ref MinSpawnFromWorldCenter) },
				{"MaxSpawnFromWorldCenter", (s, o) => TagParse.TagDoubleCheck(s, ref MaxSpawnFromWorldCenter) },
				{"CustomWorldCenter", (s, o) => TagParse.TagVector3DCheck(s, ref CustomWorldCenter) },
				{"DirectionFromWorldCenter", (s, o) => TagParse.TagVector3DListCheck(s, ref DirectionFromWorldCenter) },
				{"MinAngleFromDirection", (s, o) => TagParse.TagDoubleCheck(s, ref MinAngleFromDirection) },
				{"MaxAngleFromDirection", (s, o) => TagParse.TagDoubleCheck(s, ref MaxAngleFromDirection) },
				{"DirectionFromPlanetCenter", (s, o) => TagParse.TagVector3DListCheck(s, ref DirectionFromPlanetCenter) },
				{"MinAngleFromPlanetCenterDirection", (s, o) => TagParse.TagDoubleCheck(s, ref MinAngleFromPlanetCenterDirection) },
				{"MaxAngleFromPlanetCenterDirection", (s, o) => TagParse.TagDoubleCheck(s, ref MaxAngleFromPlanetCenterDirection) },
				{"MinSpawnFromPlanetSurface", (s, o) => TagParse.TagDoubleCheck(s, ref MinSpawnFromPlanetSurface) },
				{"MaxSpawnFromPlanetSurface", (s, o) => TagParse.TagDoubleCheck(s, ref MaxSpawnFromPlanetSurface) },
				{"UseDayOrNightOnly", (s, o) => TagParse.TagBoolCheck(s, ref UseDayOrNightOnly) },
				{"SpawnOnlyAtNight", (s, o) => TagParse.TagBoolCheck(s, ref SpawnOnlyAtNight) },
				{"UseWeatherSpawning", (s, o) => TagParse.TagBoolCheck(s, ref UseWeatherSpawning) },
				{"AllowedWeatherSystems", (s, o) => TagParse.TagStringListCheck(s, ref AllowedWeatherSystems) },
				{"UseTerrainTypeValidation", (s, o) => TagParse.TagBoolCheck(s, ref UseTerrainTypeValidation) },
				{"AllowedTerrainTypes", (s, o) => TagParse.TagStringListCheck(s, ref AllowedTerrainTypes) },
				{"MinAirDensity", (s, o) => TagParse.TagDoubleCheck(s, ref MinAirDensity) },
				{"MaxAirDensity", (s, o) => TagParse.TagDoubleCheck(s, ref MaxAirDensity) },
				{"MinGravity", (s, o) => TagParse.TagDoubleCheck(s, ref MinGravity) },
				{"MaxGravity", (s, o) => TagParse.TagDoubleCheck(s, ref MaxGravity) },
				{"CheckPrefabGravityProfiles", (s, o) => TagParse.TagBoolCheck(s, ref CheckPrefabGravityProfiles) },
				{"PlanetBlacklist", (s, o) => TagParse.TagStringListCheck(s, ref PlanetBlacklist) },
				{"PlanetWhitelist", (s, o) => TagParse.TagStringListCheck(s, ref PlanetWhitelist) },
				{"PlanetRequiresVacuum", (s, o) => TagParse.TagBoolCheck(s, ref PlanetRequiresVacuum) },
				{"PlanetRequiresAtmo", (s, o) => TagParse.TagBoolCheck(s, ref PlanetRequiresAtmo) },
				{"PlanetRequiresOxygen", (s, o) => TagParse.TagBoolCheck(s, ref PlanetRequiresOxygen) },
				{"PlanetMinimumSize", (s, o) => TagParse.TagDoubleCheck(s, ref PlanetMinimumSize) },
				{"PlanetMaximumSize", (s, o) => TagParse.TagDoubleCheck(s, ref PlanetMaximumSize) },
				{"UsePlayerCountCheck", (s, o) => TagParse.TagBoolCheck(s, ref UsePlayerCountCheck) },
				{"PlayerCountCheckRadius", (s, o) => TagParse.TagDoubleCheck(s, ref PlayerCountCheckRadius) },
				{"MinimumPlayers", (s, o) => TagParse.TagIntCheck(s, ref MinimumPlayers) },
				{"MaximumPlayers", (s, o) => TagParse.TagIntCheck(s, ref MaximumPlayers) },
				{"UsePlayerCondition", (s, o) => TagParse.TagBoolCheck(s, ref UsePlayerCondition) },
				{"PlayerConditionCheckRadius", (s, o) => TagParse.TagDoubleCheck(s, ref PlayerConditionCheckRadius) },
				{"PlayerConditionIds", (s, o) => TagParse.TagStringListCheck(s, ref PlayerConditionIds) },
				{ "UseThreatLevelCheck", (s, o) => TagParse.TagBoolCheck(s, ref UseThreatLevelCheck) },
				{"ThreatLevelCheckRange", (s, o) => TagParse.TagDoubleCheck(s, ref ThreatLevelCheckRange) },
				{"ThreatIncludeOtherNpcOwners", (s, o) => TagParse.TagBoolCheck(s, ref ThreatIncludeOtherNpcOwners) },
				{"ThreatScoreMinimum", (s, o) => TagParse.TagIntCheck(s, ref ThreatScoreMinimum) },
				{"ThreatScoreMaximum", (s, o) => TagParse.TagIntCheck(s, ref ThreatScoreMaximum) },
				{"ThreatScorePlanetaryHandicap", (s, o) => TagParse.TagIntCheck(s, ref ThreatScorePlanetaryHandicap) },
				{"ThreatScoreGridConfiguration", (s, o) => TagParse.TagGridConfigurationCheck(s, ref ThreatScoreGridConfiguration) },

				{"UsePCUCheck", (s, o) => TagParse.TagBoolCheck(s, ref UsePCUCheck) },
				{"PCUCheckRadius", (s, o) => TagParse.TagDoubleCheck(s, ref PCUCheckRadius) },
				{"PCUMinimum", (s, o) => TagParse.TagIntCheck(s, ref PCUMinimum) },
				{"PCUMaximum", (s, o) => TagParse.TagIntCheck(s, ref PCUMaximum) },
				{"UseDifficulty", (s, o) => TagParse.TagBoolCheck(s, ref UseDifficulty) },
				{"MinDifficulty", (s, o) => TagParse.TagIntCheck(s, ref MinDifficulty) },
				{"MaxDifficulty", (s, o) => TagParse.TagIntCheck(s, ref MaxDifficulty) },
				{"UseCombatPhase", (s, o) => TagParse.TagBoolCheck(s, ref UseCombatPhase) },
				{"IgnoreCombatPhase", (s, o) => TagParse.TagBoolCheck(s, ref IgnoreCombatPhase) },
				{"CombatPhaseChecksInPersistentCondition", (s, o) => TagParse.TagBoolCheck(s, ref CombatPhaseChecksInPersistentCondition) },
				{"UsePlayerCredits", (s, o) => TagParse.TagBoolCheck(s, ref UsePlayerCredits) },
				{"IncludeAllPlayersInRadius", (s, o) => TagParse.TagBoolCheck(s, ref IncludeAllPlayersInRadius) },
				{"IncludeFactionBalance", (s, o) => TagParse.TagBoolCheck(s, ref IncludeFactionBalance) },
				{"PlayerCreditsCheckRadius", (s, o) => TagParse.TagDoubleCheck(s, ref PlayerCreditsCheckRadius) },
				{"MinimumPlayerCredits", (s, o) => TagParse.TagIntCheck(s, ref MinimumPlayerCredits) },
				{"MaximumPlayerCredits", (s, o) => TagParse.TagIntCheck(s, ref MaximumPlayerCredits) },
				{"UsePlayerFactionReputation", (s, o) => TagParse.TagBoolCheck(s, ref UsePlayerFactionReputation) },
				{"PlayerReputationCheckRadius", (s, o) => TagParse.TagDoubleCheck(s, ref PlayerReputationCheckRadius) },
				{"CheckReputationAgainstOtherNPCFaction", (s, o) => TagParse.TagStringCheck(s, ref CheckReputationAgainstOtherNPCFaction) },
				{"MinimumReputation", (s, o) => TagParse.TagIntCheck(s, ref MinimumReputation) },
				{"MaximumReputation", (s, o) => TagParse.TagIntCheck(s, ref MaximumReputation) },
				{"UseSignalRequirement", (s, o) => TagParse.TagBoolCheck(s, ref UseSignalRequirement) },
				{"MinSignalRadius", (s, o) => TagParse.TagDoubleCheck(s, ref MinSignalRadius) },
				{"MaxSignalRadius", (s, o) => TagParse.TagDoubleCheck(s, ref MaxSignalRadius) },
				{"AllowNpcSignals", (s, o) => TagParse.TagBoolCheck(s, ref AllowNpcSignals) },
				{"UseOnlySelfOwnedSignals", (s, o) => TagParse.TagBoolCheck(s, ref UseOnlySelfOwnedSignals) },
				{"MatchSignalName", (s, o) => TagParse.TagStringCheck(s, ref MatchSignalName) },
				{"ChargeNpcFactionForSpawn", (s, o) => TagParse.TagBoolCheck(s, ref ChargeNpcFactionForSpawn) },
				{"ChargeForSpawning", (s, o) => TagParse.TagLongCheck(s, ref ChargeForSpawning) },
				{"UseSandboxCounterCosts", (s, o) => TagParse.TagBoolCheck(s, ref UseSandboxCounterCosts) },
				{"SandboxCounterCostNames", (s, o) => TagParse.TagStringListCheck(s, ref SandboxCounterCostNames) },
				{"SandboxCounterCostAmounts", (s, o) => TagParse.TagIntListCheck(s, ref SandboxCounterCostAmounts) },
				{"SandboxVariables", (s, o) => TagParse.TagStringListCheck(s, ref SandboxVariables) },
				{"FalseSandboxVariables", (s, o) => TagParse.TagStringListCheck(s, ref FalseSandboxVariables) },
				{"UseRemoteControlCodeRestrictions", (s, o) => TagParse.TagBoolCheck(s, ref UseRemoteControlCodeRestrictions) },
				{"RemoteControlCode", (s, o) => TagParse.TagStringCheck(s, ref RemoteControlCode) },
				{"RemoteControlCodeMinDistance", (s, o) => TagParse.TagDoubleCheck(s, ref RemoteControlCodeMinDistance) },
				{"RemoteControlCodeMaxDistance", (s, o) => TagParse.TagDoubleCheck(s, ref RemoteControlCodeMaxDistance) },
				{"RequireAllMods", (s, o) => TagParse.TagUlongListCheck(s, ref RequireAllMods) },
				{"RequireAnyMods", (s, o) => TagParse.TagUlongListCheck(s, ref RequireAnyMods) },
				{"ExcludeAllMods", (s, o) => TagParse.TagUlongListCheck(s, ref ExcludeAllMods) },
				{"ExcludeAnyMods", (s, o) => TagParse.TagUlongListCheck(s, ref ExcludeAnyMods) },
				{"ModBlockExists", (s, o) => TagParse.TagStringListCheck(s, ref ModBlockExists) },
				{"RequiredPlayersOnline", (s, o) => TagParse.TagUlongListCheck(s, ref RequiredPlayersOnline) },
				{"RequiredAnyPlayersOnline", (s, o) => TagParse.TagUlongListCheck(s, ref RequiredAnyPlayersOnline) },
				{"UseKnownPlayerLocations", (s, o) => TagParse.TagBoolCheck(s, ref UseKnownPlayerLocations) },
				{"KnownPlayerLocationMustBeInside", (s, o) => TagParse.TagBoolCheck(s, ref KnownPlayerLocationMustBeInside) },
				{"KnownPlayerLocationMustMatchFaction", (s, o) => TagParse.TagBoolCheck(s, ref KnownPlayerLocationMustMatchFaction) },
				{"KnownPlayerLocationMinSpawnedEncounters", (s, o) => TagParse.TagIntCheck(s, ref KnownPlayerLocationMinSpawnedEncounters) },
				{"KnownPlayerLocationMaxSpawnedEncounters", (s, o) => TagParse.TagIntCheck(s, ref KnownPlayerLocationMaxSpawnedEncounters) },
				{"ZoneConditions", (s, o) => TagParse.TagZoneConditionsProfileCheck(s, ref ZoneConditions) },
				{"CustomApiConditions", (s, o) => TagParse.TagStringListCheck(s, ref CustomApiConditions) },
				{"BossCustomAnnounceEnable", (s, o) => TagParse.TagBoolCheck(s, ref BossCustomAnnounceEnable) },
				{"BossCustomAnnounceAuthor", (s, o) => TagParse.TagStringCheck(s, ref BossCustomAnnounceAuthor) },
				{"BossCustomAnnounceMessage", (s, o) => TagParse.TagStringCheck(s, ref BossCustomAnnounceMessage) },
				{"BossCustomGPSLabel", (s, o) => TagParse.TagStringCheck(s, ref BossCustomGPSLabel) },
				{"BossCustomGPSColor", (s, o) => TagParse.TagVector3DCheck(s, ref BossCustomGPSColor) },
				{"BossMusicId", (s, o) => TagParse.TagStringCheck(s, ref BossMusicId) },
				{"PlaySoundAtSpawnTriggerPosition", (s, o) => TagParse.TagBoolCheck(s, ref PlaySoundAtSpawnTriggerPosition) },
				{"SpawnTriggerPositionSoundId", (s, o) => TagParse.TagStringCheck(s, ref SpawnTriggerPositionSoundId) },
				{"RotateFirstCockpitToForward", (s, o) => TagParse.TagBoolCheck(s, ref RotateFirstCockpitToForward) },
				{"PositionAtFirstCockpit", (s, o) => TagParse.TagBoolCheck(s, ref PositionAtFirstCockpit) },
				{"SpawnRandomCargo", (s, o) => TagParse.TagBoolCheck(s, ref SpawnRandomCargo) },
				{"DisableDampeners", (s, o) => TagParse.TagBoolCheck(s, ref DisableDampeners) },
				{"ReactorsOn", (s, o) => TagParse.TagBoolCheck(s, ref ReactorsOn) },
				{"UseBoundingBoxCheck", (s, o) => TagParse.TagBoolCheck(s, ref UseBoundingBoxCheck) },
				{"RemoveVoxelsIfGridRemoved", (s, o) => TagParse.TagBoolCheck(s, ref RemoveVoxelsIfGridRemoved) },
				{"UseGridOrigin", (s, o) => TagParse.TagBoolCheck(s, ref UseGridOrigin) },
				{"PrefabSpawningMode", (s, o) => TagParse.TagPrefabSpawnModeEnumCheck(s, ref PrefabSpawningMode) },
				{"AllowPrefabIndexReuse", (s, o) => TagParse.TagBoolCheck(s, ref AllowPrefabIndexReuse) },
				{"PrefabIndexes", (s, o) => TagParse.TagIntListCheck(s, true, ref PrefabIndexes) },
				{"PrefabIndexGroupNames", (s, o) => TagParse.TagStringListCheck(s, ref PrefabIndexGroupNames) },
				{"PrefabIndexGroupValues", (s, o) => TagParse.TagIntListCheck(s, true, ref PrefabIndexGroupValues) },
				{"PrefabFixedCount", (s, o) => TagParse.TagIntCheck(s, ref PrefabFixedCount) },
				{"PrefabOffsetOverrides", (s, o) => TagParse.TagVector3DListCheck(s, ref PrefabOffsetOverrides) },
				{"UseSpawnGroupPrefabSpawningMode", (s, o) => TagParse.TagBoolCheck(s, ref UseSpawnGroupPrefabSpawningMode) },
				{"UseEventController", (s, o) => TagParse.TagBoolCheck(s, ref UseEventController) },
				{"EventControllerId", (s, o) => TagParse.TagStringListCheck(s, ref EventControllerId) },
				{"CheckCustomSandboxCounters", (s, o) => TagParse.TagBoolCheck(s, ref CheckCustomSandboxCounters) },
				{"CustomSandboxCounters", (s, o) => TagParse.TagStringListCheck(s, ref CustomSandboxCounters) },
				{"CustomSandboxCountersTargets", (s, o) => TagParse.TagIntListCheck(s, ref CustomSandboxCountersTargets) },
				{"SandboxCounterCompareTypes", (s, o) => TagParse.TagCounterCompareEnumCheck(s, ref SandboxCounterCompareTypes) },
				{"CheckRequiredBlocks", (s, o) => TagParse.TagBoolCheck(s, ref CheckRequiredBlocks) },
				{"RequiredBlockSubtypeIds", (s, o) => TagParse.TagStringListCheck(s, ref RequiredBlockSubtypeIds) },
				{"RequiredBlockAnySubtypeId", (s, o) => TagParse.TagBoolCheck(s, ref RequiredBlockAnySubtypeId) },
				{"RequiredBlockCheckRange", (s, o) => TagParse.TagDoubleCheck(s, ref RequiredBlockCheckRange) },
				{"RequiredBlockIncludeNPCGrids", (s, o) => TagParse.TagBoolCheck(s, ref RequiredBlockIncludeNPCGrids) },

				};

		}



		public void EditValue(string receivedValue) {

			var processedTag = TagParse.ProcessTag(receivedValue);

			if (processedTag.Length < 2)
				return;

			Action<string, object> referenceMethod = null;

			if (!EditorReference.TryGetValue(processedTag[0], out referenceMethod))
				//TODO: Notes About Value Not Found
				return;

			referenceMethod?.Invoke(receivedValue, null);

		}

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				bool setDampeners = false;
				bool setAtmoRequired = false;
				bool setForceStatic = false;

				ZoneConditions[0].ProfileSubtypeId = ProfileSubtypeId;

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					EditValue(tag);

				}


				if (this.SpaceCargoShip == true && setDampeners == false) {

					this.DisableDampeners = true;

				}

				if (this.AtmosphericCargoShip == true && setAtmoRequired == false) {

					if (!this.SkipAirDensityCheck)
						this.PlanetRequiresAtmo = true;

				}

				if (this.PlanetaryInstallation == true && !this.InstallationSpawnsOnWaterSurface && setForceStatic == false) {


					
					this.ForceStaticGrid = true;

					if (this.DoNotForceStaticGrid)
						this.ForceStaticGrid = false;

				}

				for (int i = 0; i < this.PrefabIndexGroupNames.Count; i++) {

					if (i >= this.PrefabIndexGroupValues.Count)
						break;

					if (PrefabIndexGroups.ContainsKey(this.PrefabIndexGroupNames[i])) {

						if (!PrefabIndexGroups[this.PrefabIndexGroupNames[i]].Contains(this.PrefabIndexGroupValues[i])) {

							PrefabIndexGroups[this.PrefabIndexGroupNames[i]].Add(this.PrefabIndexGroupValues[i]);

						}

					} else {

						PrefabIndexGroups.Add(this.PrefabIndexGroupNames[i], new List<int>());
						PrefabIndexGroups[this.PrefabIndexGroupNames[i]].Add(this.PrefabIndexGroupValues[i]);

					}

				}

			}

		}

	}

}
