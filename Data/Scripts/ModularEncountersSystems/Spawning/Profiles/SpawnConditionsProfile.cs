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
		public bool ForceExactPositionAndOrientation;
		public bool AdminSpawnOnly;

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

		public bool SettingsAutoHeal;
		public bool SettingsAutoRespawn;
		public bool SettingsBountyContracts;
		public bool SettingsDestructibleBlocks;
		public bool SettingsCopyPaste;
		public bool SettingsContainerDrops;
		public bool SettingsEconomy;
		public bool SettingsEnableDrones;
		public bool SettingsIngameScripts;
		public bool SettingsJetpack;
		public bool SettingsOxygen;
		public bool SettingsResearch;
		public bool SettingsSpawnWithTools;
		public bool SettingsSpiders;
		public bool SettingsSubgridDamage;
		public bool SettingsSunRotation;
		public bool SettingsSupergridding;
		public bool SettingsThrusterDamage;
		public bool SettingsVoxelDestruction;
		public bool SettingsWeaponsEnabled;
		public bool SettingsWeather;
		public bool SettingsWolves;

		public double MinSpawnFromWorldCenter;
		public double MaxSpawnFromWorldCenter;
		public Vector3D CustomWorldCenter; //Doc
		public List<Vector3D> DirectionFromWorldCenter; //Doc
		public double MinAngleFromDirection; //Doc
		public double MaxAngleFromDirection; //Doc

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

		public bool UsePCUCheck;
		public double PCUCheckRadius;
		public int PCUMinimum;
		public int PCUMaximum;

		public bool UseDifficulty; //Doc
		public int MinDifficulty; //Doc
		public int MaxDifficulty; //Doc

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

		public bool UseSignalRequirement; //Doc
		public double MinSignalRadius; //Doc
		public double MaxSignalRadius; //Doc
		public bool AllowNpcSignals; //Doc
		public bool UseOnlySelfOwnedSignals; //Doc
		public string MatchSignalName; //Doc

		public bool ChargeNpcFactionForSpawn; //Doc
		public long ChargeForSpawning; //Doc

		public bool UseSandboxCounterCosts;
		public List<string> SandboxCounterCostNames;
		public List<int> SandboxCounterCostAmounts;

		public List<string> SandboxVariables;
		public List<string> FalseSandboxVariables;

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
		public bool KnownPlayerLocationMustMatchFaction;
		public int KnownPlayerLocationMinSpawnedEncounters;
		public int KnownPlayerLocationMaxSpawnedEncounters;

		public List<ZoneConditionsProfile> ZoneConditions; //Doc

		public List<string> CustomApiConditions; //Doc

		public bool BossCustomAnnounceEnable;
		public string BossCustomAnnounceAuthor;
		public string BossCustomAnnounceMessage;
		public string BossCustomGPSLabel;
		public Vector3D BossCustomGPSColor; //Doc
		public string BossMusicId; //Doc

		public bool PlaySoundAtSpawnTriggerPosition; //Doc
		public string SpawnTriggerPositionSoundId; //Doc

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
		public List<Vector3D> PrefabOffsetOverrides;

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
			ForceExactPositionAndOrientation = false;
			AdminSpawnOnly = false;

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

			UsePCUCheck = false;
			PCUCheckRadius = 5000;
			PCUMinimum = -1;
			PCUMaximum = -1;

			UseDifficulty = false;
			MinDifficulty = -1;
			MaxDifficulty = -1;

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
			PrefabOffsetOverrides = new List<Vector3D>();

		}

		public void InitTags(string data = null) {

			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			bool setDampeners = false;
			bool setAtmoRequired = false;
			bool setForceStatic = false;

			ZoneConditions[0].ProfileSubtypeId = ProfileSubtypeId;

			foreach (var tagRaw in descSplit) {

				var tag = tagRaw.Trim();

				//SpaceCargoShip
				if (tag.StartsWith("[SpaceCargoShip:") == true) {

					TagParse.TagBoolCheck(tag, ref this.SpaceCargoShip);

				}

				//LunarCargoShip
				if (tag.StartsWith("[LunarCargoShip:") == true) {

					TagParse.TagBoolCheck(tag, ref this.LunarCargoShip);

				}

				//AtmosphericCargoShip
				if (tag.StartsWith("[AtmosphericCargoShip:") == true) {

					TagParse.TagBoolCheck(tag, ref this.AtmosphericCargoShip);

				}

				//GravityCargoShip
				if (tag.StartsWith("[GravityCargoShip:") == true) {

					TagParse.TagBoolCheck(tag, ref this.GravityCargoShip);

				}

				//SkipAirDensityCheck
				if (tag.StartsWith("[SkipAirDensityCheck:") == true) {

					TagParse.TagBoolCheck(tag, ref this.SkipAirDensityCheck);

				}

				//CargoShipTerrainPath
				if (tag.StartsWith("[CargoShipTerrainPath:") == true) {

					TagParse.TagBoolCheck(tag, ref this.CargoShipTerrainPath);

				}

				//CustomPathStartAltitude
				if (tag.StartsWith("[CustomPathStartAltitude:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.CustomPathStartAltitude);

				}

				//CustomPathEndAltitude
				if (tag.StartsWith("[CustomPathEndAltitude:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.CustomPathEndAltitude);

				}

				//SpaceRandomEncounter
				if (tag.StartsWith("[SpaceRandomEncounter:") == true) {

					TagParse.TagBoolCheck(tag, ref this.SpaceRandomEncounter);

				}

				//AlignVoxelsToSpawnMatrix
				if (tag.StartsWith("[AlignVoxelsToSpawnMatrix:") == true) {

					TagParse.TagBoolCheck(tag, ref this.AlignVoxelsToSpawnMatrix);

				}

				//UseOptimizedVoxelSpawning
				if (tag.StartsWith("[UseOptimizedVoxelSpawning:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseOptimizedVoxelSpawning);

				}

				//CustomVoxelMaterial
				if (tag.StartsWith("[CustomVoxelMaterial:") == true) {

					TagParse.TagStringListCheck(tag, ref this.CustomVoxelMaterial);

				}

				//PlanetaryInstallation
				if (tag.StartsWith("[PlanetaryInstallation:") == true) {

					TagParse.TagBoolCheck(tag, ref this.PlanetaryInstallation);

				}

				//PlanetaryInstallationType
				if (tag.StartsWith("[PlanetaryInstallationType:") == true) {

					TagParse.TagStringCheck(tag, ref this.PlanetaryInstallationType);

					if (this.PlanetaryInstallationType == "") {

						this.PlanetaryInstallationType = "Small";

					}

				}

				//SkipTerrainCheck
				if (tag.StartsWith("[SkipTerrainCheck:") == true) {

					TagParse.TagBoolCheck(tag, ref this.SkipTerrainCheck);

				}

				//RotateInstallations
				if (tag.StartsWith("[RotateInstallations:") == true) {

					TagParse.TagVector3DListCheck(tag, ref this.RotateInstallations);

				}

				//InstallationTerrainValidation
				if (tag.StartsWith("[InstallationTerrainValidation:") == true) {

					TagParse.TagBoolCheck(tag, ref this.InstallationTerrainValidation);

				}

				//InstallationSpawnsOnDryLand
				if (tag.StartsWith("[InstallationSpawnsOnDryLand:") == true) {

					TagParse.TagBoolCheck(tag, ref this.InstallationSpawnsOnDryLand);

				}

				//InstallationSpawnsUnderwater
				if (tag.StartsWith("[InstallationSpawnsUnderwater:") == true) {

					TagParse.TagBoolCheck(tag, ref this.InstallationSpawnsUnderwater);

				}

				//InstallationSpawnsOnWaterSurface
				if (tag.StartsWith("[InstallationSpawnsOnWaterSurface:") == true) {

					TagParse.TagBoolCheck(tag, ref this.InstallationSpawnsOnWaterSurface);

				}

				//ReverseForwardDirections
				if (tag.StartsWith("[ReverseForwardDirections:") == true) {

					TagParse.TagBoolListCheck(tag, ref this.ReverseForwardDirections);

				}

				//CutVoxelsAtAirtightCells
				if (tag.StartsWith("[CutVoxelsAtAirtightCells:") == true) {

					TagParse.TagBoolCheck(tag, ref this.CutVoxelsAtAirtightCells);

				}

				//CutVoxelSize
				if (tag.StartsWith("[CutVoxelSize:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.CutVoxelSize);

				}

				//BossEncounterSpace
				if (tag.StartsWith("[BossEncounterSpace:") == true) {

					TagParse.TagBoolCheck(tag, ref this.BossEncounterSpace);

				}

				//BossEncounterAtmo
				if (tag.StartsWith("[BossEncounterAtmo:") == true) {

					TagParse.TagBoolCheck(tag, ref this.BossEncounterAtmo);

				}

				//BossEncounterAny
				if (tag.StartsWith("[BossEncounterAny:") == true) {

					TagParse.TagBoolCheck(tag, ref this.BossEncounterAny);

				}

				//RivalAiSpawn
				if (tag.StartsWith("[RivalAiSpawn:") == true) {

					TagParse.TagBoolCheck(tag, ref this.RivalAiAnySpawn);

				}

				//RivalAiSpaceSpawn
				if (tag.StartsWith("[RivalAiSpaceSpawn:") == true) {

					TagParse.TagBoolCheck(tag, ref this.RivalAiSpaceSpawn);

				}

				//RivalAiAtmosphericSpawn
				if (tag.StartsWith("[RivalAiAtmosphericSpawn:") == true) {

					TagParse.TagBoolCheck(tag, ref this.RivalAiAtmosphericSpawn);

				}

				//RivalAiAnySpawn
				if (tag.StartsWith("[RivalAiAnySpawn:") == true) {

					TagParse.TagBoolCheck(tag, ref this.RivalAiAnySpawn);

				}

				//DroneEncounter
				if (tag.StartsWith("[DroneEncounter:") == true) {

					TagParse.TagBoolCheck(tag, ref this.DroneEncounter);

				}

				//MinimumPlayerTime
				if (tag.StartsWith("[MinimumPlayerTime:") == true) {

					TagParse.TagIntCheck(tag, ref this.MinimumPlayerTime);

				}

				//MaximumPlayerTime
				if (tag.StartsWith("[MaximumPlayerTime:") == true) {

					TagParse.TagIntCheck(tag, ref this.MaximumPlayerTime);

				}

				//FailedDroneSpawnResetsPlayerTime
				if (tag.StartsWith("[FailedDroneSpawnResetsPlayerTime:") == true) {

					TagParse.TagBoolCheck(tag, ref this.FailedDroneSpawnResetsPlayerTime);

				}

				//MinDroneDistance
				if (tag.StartsWith("[MinDroneDistance:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MinDroneDistance);

				}

				//MaxDroneDistance
				if (tag.StartsWith("[MaxDroneDistance:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MaxDroneDistance);

				}

				//MinDroneAltitude
				if (tag.StartsWith("[MinDroneAltitude:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MinDroneAltitude);

				}

				//MaxDroneAltitude
				if (tag.StartsWith("[MaxDroneAltitude:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MaxDroneAltitude);

				}

				//DroneInheritsSourceAltitude
				if (tag.StartsWith("[DroneInheritsSourceAltitude:") == true) {

					TagParse.TagBoolCheck(tag, ref this.DroneInheritsSourceAltitude);

				}

				//SkipVoxelSpawnChecks
				if (tag.StartsWith("[SkipVoxelSpawnChecks:") == true) {

					TagParse.TagBoolCheck(tag, ref this.SkipVoxelSpawnChecks);

				}

				//SkipGridSpawnChecks
				if (tag.StartsWith("[SkipGridSpawnChecks:") == true) {

					TagParse.TagBoolCheck(tag, ref this.SkipGridSpawnChecks);

				}

				//CreatureSpawn
				if (tag.StartsWith("[CreatureSpawn:") == true) {

					TagParse.TagBoolCheck(tag, ref this.CreatureSpawn);

				}

				//CreatureIds
				if (tag.StartsWith("[CreatureIds:") == true) {

					TagParse.TagStringListCheck(tag, ref this.CreatureIds);

				}

				//BotProfiles
				if (tag.StartsWith("[BotProfiles:") == true) {

					TagParse.TagBotProfileListCheck(tag, ref this.BotProfiles);

				}

				//AiEnabledModBots
				if (tag.StartsWith("[AiEnabledModBots:") == true) {

					TagParse.TagBoolCheck(tag, ref this.AiEnabledModBots);

				}

				//AiEnabledReady
				if (tag.StartsWith("[AiEnabledReady:") == true) {

					TagParse.TagBoolCheck(tag, ref this.AiEnabledReady);

				}

				//AiEnabledRole
				if (tag.StartsWith("[AiEnabledRole:") == true) {

					TagParse.TagStringCheck(tag, ref this.AiEnabledRole);

				}

				//MinCreatureCount
				if (tag.StartsWith("[MinCreatureCount:") == true) {

					TagParse.TagIntCheck(tag, ref this.MinCreatureCount);

				}

				//MaxCreatureCount
				if (tag.StartsWith("[MaxCreatureCount:") == true) {

					TagParse.TagIntCheck(tag, ref this.MaxCreatureCount);

				}

				//MinCreatureDistance
				if (tag.StartsWith("[MinCreatureDistance:") == true) {

					TagParse.TagIntCheck(tag, ref this.MinCreatureDistance);

				}

				//MaxCreatureDistance
				if (tag.StartsWith("[MaxCreatureDistance:") == true) {

					TagParse.TagIntCheck(tag, ref this.MaxCreatureDistance);

				}

				//MinDistFromOtherCreaturesInGroup
				if (tag.StartsWith("[MaxCreatureDistance:") == true) {

					TagParse.TagIntCheck(tag, ref this.MaxCreatureDistance);

				}

				//CanSpawnUnderwater
				if (tag.StartsWith("[CanSpawnUnderwater:") == true) {

					TagParse.TagBoolCheck(tag, ref this.CanSpawnUnderwater);

				}

				//MinWaterDepth
				if (tag.StartsWith("[MinWaterDepth:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MinWaterDepth);

				}

				//StaticEncounter
				if (tag.StartsWith("[StaticEncounter:") == true) {

					TagParse.TagBoolCheck(tag, ref this.StaticEncounter);

				}

				//UniqueEncounter
				if (tag.StartsWith("[UniqueEncounter:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UniqueEncounter);

				}

				//TriggerCoords
				if (tag.StartsWith("[TriggerCoords:") == true) {

					TagParse.TagVector3DCheck(tag, ref this.TriggerCoords);

				}

				//TriggerRadius
				if (tag.StartsWith("[TriggerRadius:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.TriggerRadius);

				}

				//StaticEncounterCoords
				if (tag.StartsWith("[StaticEncounterCoords:") == true) {

					TagParse.TagVector3DCheck(tag, ref this.StaticEncounterCoords);

				}

				//StaticEncounterForward
				if (tag.StartsWith("[StaticEncounterForward:") == true) {

					TagParse.TagVector3DCheck(tag, ref this.StaticEncounterForward);

				}

				//StaticEncounterUp
				if (tag.StartsWith("[StaticEncounterUp:") == true) {

					TagParse.TagVector3DCheck(tag, ref this.StaticEncounterUp);

				}

				//StaticEncounterUsePlanetDirectionAndAltitude
				if (tag.StartsWith("[StaticEncounterUsePlanetDirectionAndAltitude:") == true) {

					TagParse.TagBoolCheck(tag, ref this.StaticEncounterUsePlanetDirectionAndAltitude);

				}

				//StaticEncounterPlanet
				if (tag.StartsWith("[StaticEncounterPlanet:") == true) {

					TagParse.TagStringCheck(tag, ref this.StaticEncounterPlanet);

				}

				//StaticEncounterPlanetDirection
				if (tag.StartsWith("[StaticEncounterPlanetDirection:") == true) {

					TagParse.TagVector3DCheck(tag, ref this.StaticEncounterPlanetDirection);

				}

				//StaticEncounterPlanetAltitude
				if (tag.StartsWith("[StaticEncounterPlanetAltitude:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.StaticEncounterPlanetAltitude);

				}

				//ForceStaticGrid
				if (tag.StartsWith("[ForceStaticGrid:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ForceStaticGrid);
					setForceStatic = true;

				}

				//ForceExactPositionAndOrientation
				if (tag.StartsWith("[ForceExactPositionAndOrientation:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ForceExactPositionAndOrientation);

				}

				//AdminSpawnOnly
				if (tag.StartsWith("[AdminSpawnOnly:") == true) {

					TagParse.TagBoolCheck(tag, ref this.AdminSpawnOnly);

				}


				//SandboxVariables
				if (tag.StartsWith("[SandboxVariables:") == true) {

					TagParse.TagStringListCheck(tag, ref this.SandboxVariables);

				}

				//FalseSandboxVariables
				if (tag.StartsWith("[FalseSandboxVariables:") == true) {

					TagParse.TagStringListCheck(tag, ref this.FalseSandboxVariables);

				}

				//RandomNumberRoll
				if (tag.StartsWith("[RandomNumberRoll:") == true) {

					TagParse.TagIntCheck(tag, ref this.RandomNumberRoll);

				}

				//FactionOwner
				if (tag.StartsWith("[FactionOwner:") == true) {

					TagParse.TagStringCheck(tag, ref this.FactionOwner);

					if (this.FactionOwner == "") {

						this.FactionOwner = "SPRT";

					}

				}

				//UseRandomMinerFaction
				if (tag.StartsWith("[UseRandomMinerFaction:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseRandomMinerFaction);

				}

				//UseRandomBuilderFaction
				if (tag.StartsWith("[UseRandomBuilderFaction:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseRandomBuilderFaction);

				}

				//UseRandomTraderFaction
				if (tag.StartsWith("[UseRandomTraderFaction:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseRandomTraderFaction);

				}

				//ChanceCeiling
				if (tag.StartsWith("[ChanceCeiling:") == true) {

					TagParse.TagIntCheck(tag, ref this.ChanceCeiling);

				}

				//SpaceCargoShipChance
				if (tag.StartsWith("[SpaceCargoShipChance:") == true) {

					TagParse.TagIntCheck(tag, ref this.SpaceCargoShipChance);

				}

				//LunarCargoShipChance
				if (tag.StartsWith("[LunarCargoShipChance:") == true) {

					TagParse.TagIntCheck(tag, ref this.LunarCargoShipChance);

				}

				//AtmosphericCargoShipChance
				if (tag.StartsWith("[AtmosphericCargoShipChance:") == true) {

					TagParse.TagIntCheck(tag, ref this.AtmosphericCargoShipChance);

				}

				//GravityCargoShipChance
				if (tag.StartsWith("[GravityCargoShipChance:") == true) {

					TagParse.TagIntCheck(tag, ref this.GravityCargoShipChance);

				}

				//RandomEncounterChance
				if (tag.StartsWith("[RandomEncounterChance:") == true) {

					TagParse.TagIntCheck(tag, ref this.RandomEncounterChance);

				}

				//PlanetaryInstallationChance
				if (tag.StartsWith("[PlanetaryInstallationChance:") == true) {

					TagParse.TagIntCheck(tag, ref this.PlanetaryInstallationChance);

				}

				//BossEncounterChance
				if (tag.StartsWith("[BossEncounterChance:") == true) {

					TagParse.TagIntCheck(tag, ref this.BossEncounterChance);

				}

				//CreatureChance
				if (tag.StartsWith("[CreatureChance:") == true) {

					TagParse.TagIntCheck(tag, ref this.CreatureChance);

				}

				//DroneEncounterChance
				if (tag.StartsWith("[DroneEncounterChance:") == true) {

					TagParse.TagIntCheck(tag, ref this.DroneEncounterChance);

				}

				//MinSpawnFromWorldCenter
				if (tag.StartsWith("[MinSpawnFromWorldCenter:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MinSpawnFromWorldCenter);

				}

				//MaxSpawnFromWorldCenter
				if (tag.StartsWith("[MaxSpawnFromWorldCenter:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MaxSpawnFromWorldCenter);

				}

				//CustomWorldCenter
				if (tag.StartsWith("[CustomWorldCenter:") == true) {

					TagParse.TagVector3DCheck(tag, ref this.CustomWorldCenter);

				}

				//DirectionFromWorldCenter
				if (tag.StartsWith("[DirectionFromWorldCenter:") == true) {

					TagParse.TagVector3DListCheck(tag, ref this.DirectionFromWorldCenter);

				}

				//MinAngleFromDirection
				if (tag.StartsWith("[MinAngleFromDirection:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MinAngleFromDirection);

				}

				//MaxAngleFromDirection
				if (tag.StartsWith("[MaxAngleFromDirection:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MaxAngleFromDirection);

				}

				//DirectionFromPlanetCenter
				if (tag.StartsWith("[DirectionFromPlanetCenter:") == true) {

					TagParse.TagVector3DListCheck(tag, ref this.DirectionFromPlanetCenter);

				}

				//MinAngleFromPlanetCenterDirection
				if (tag.StartsWith("[MinAngleFromPlanetCenterDirection:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MinAngleFromPlanetCenterDirection);

				}

				//MaxAngleFromPlanetCenterDirection
				if (tag.StartsWith("[MaxAngleFromPlanetCenterDirection:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MaxAngleFromPlanetCenterDirection);

				}

				//MinSpawnFromPlanetSurface
				if (tag.StartsWith("[MinSpawnFromPlanetSurface:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MinSpawnFromPlanetSurface);

				}

				//MaxSpawnFromPlanetSurface
				if (tag.StartsWith("[MaxSpawnFromPlanetSurface:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MaxSpawnFromPlanetSurface);

				}

				//UseDayOrNightOnly
				if (tag.StartsWith("[UseDayOrNightOnly:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseDayOrNightOnly);

				}

				//SpawnOnlyAtNight
				if (tag.StartsWith("[SpawnOnlyAtNight:") == true) {

					TagParse.TagBoolCheck(tag, ref this.SpawnOnlyAtNight);

				}

				//UseWeatherSpawning
				if (tag.StartsWith("[UseWeatherSpawning:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseWeatherSpawning);

				}

				//AllowedWeatherSystems
				if (tag.StartsWith("[AllowedWeatherSystems:") == true) {

					TagParse.TagStringListCheck(tag, ref this.AllowedWeatherSystems);

				}

				//UseTerrainTypeValidation
				if (tag.StartsWith("[UseTerrainTypeValidation:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseTerrainTypeValidation);

				}

				//AllowedTerrainTypes
				if (tag.StartsWith("[AllowedTerrainTypes:") == true) {

					TagParse.TagStringListCheck(tag, ref this.AllowedTerrainTypes);

				}

				//MinAirDensity
				if (tag.StartsWith("[MinAirDensity:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MinAirDensity);

				}

				//MaxAirDensity
				if (tag.StartsWith("[MaxAirDensity:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MaxAirDensity);

				}

				//MinGravity
				if (tag.StartsWith("[MinGravity:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MinGravity);

				}

				//MaxGravity
				if (tag.StartsWith("[MaxGravity:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MaxGravity);

				}

				//PlanetBlacklist
				if (tag.StartsWith("[PlanetBlacklist:") == true) {

					TagParse.TagStringListCheck(tag, ref this.PlanetBlacklist);

				}

				//PlanetWhitelist
				if (tag.StartsWith("[PlanetWhitelist:") == true) {

					TagParse.TagStringListCheck(tag, ref this.PlanetWhitelist);

				}

				//PlanetRequiresVacuum
				if (tag.StartsWith("[PlanetRequiresVacuum:") == true) {

					TagParse.TagBoolCheck(tag, ref this.PlanetRequiresVacuum);

				}

				//PlanetRequiresAtmo
				if (tag.StartsWith("[PlanetRequiresAtmo:") == true) {

					TagParse.TagBoolCheck(tag, ref this.PlanetRequiresAtmo);
					setAtmoRequired = true;

				}

				//PlanetRequiresOxygen
				if (tag.StartsWith("[PlanetRequiresOxygen:") == true) {

					TagParse.TagBoolCheck(tag, ref this.PlanetRequiresOxygen);

				}

				//PlanetMinimumSize
				if (tag.StartsWith("[PlanetMinimumSize:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.PlanetMinimumSize);

				}

				//PlanetMaximumSize
				if (tag.StartsWith("[PlanetMaximumSize:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.PlanetMaximumSize);

				}

				//UsePlayerCountCheck
				if (tag.StartsWith("[UsePlayerCountCheck:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UsePlayerCountCheck);

				}

				//PlayerCountCheckRadius
				if (tag.StartsWith("[PlayerCountCheckRadius:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.PlayerCountCheckRadius);

				}

				//MinimumPlayers
				if (tag.StartsWith("[MinimumPlayers:") == true) {

					TagParse.TagIntCheck(tag, ref this.MinimumPlayers);

				}

				//MaximumPlayers
				if (tag.StartsWith("[MaximumPlayers:") == true) {

					TagParse.TagIntCheck(tag, ref this.MaximumPlayers);

				}

				//UseThreatLevelCheck
				if (tag.StartsWith("[UseThreatLevelCheck:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseThreatLevelCheck);

				}

				//ThreatLevelCheckRange
				if (tag.StartsWith("[ThreatLevelCheckRange:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.ThreatLevelCheckRange);

				}

				//ThreatIncludeOtherNpcOwners
				if (tag.StartsWith("[ThreatIncludeOtherNpcOwners:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ThreatIncludeOtherNpcOwners);

				}

				//ThreatScoreMinimum
				if (tag.StartsWith("[ThreatScoreMinimum:") == true) {

					TagParse.TagIntCheck(tag, ref this.ThreatScoreMinimum);

				}

				//ThreatScoreMaximum
				if (tag.StartsWith("[ThreatScoreMaximum:") == true) {

					TagParse.TagIntCheck(tag, ref this.ThreatScoreMaximum);

				}

				//UsePCUCheck
				if (tag.StartsWith("[UsePCUCheck:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UsePCUCheck);

				}

				//PCUCheckRadius
				if (tag.StartsWith("[PCUCheckRadius:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.PCUCheckRadius);

				}

				//PCUMinimum
				if (tag.StartsWith("[PCUMinimum:") == true) {

					TagParse.TagIntCheck(tag, ref this.PCUMinimum);

				}

				//PCUMaximum
				if (tag.StartsWith("[PCUMaximum:") == true) {

					TagParse.TagIntCheck(tag, ref this.PCUMaximum);

				}

				//UseDifficulty
				if (tag.StartsWith("[UseDifficulty:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseDifficulty);

				}

				//MinDifficulty
				if (tag.StartsWith("[MinDifficulty:") == true) {

					TagParse.TagIntCheck(tag, ref this.MinDifficulty);

				}

				//MaxDifficulty
				if (tag.StartsWith("[MaxDifficulty:") == true) {

					TagParse.TagIntCheck(tag, ref this.MaxDifficulty);

				}

				//UsePlayerCredits
				if (tag.StartsWith("[UsePlayerCredits:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UsePlayerCredits);

				}

				//IncludeAllPlayersInRadius
				if (tag.StartsWith("[IncludeAllPlayersInRadius:") == true) {

					TagParse.TagBoolCheck(tag, ref this.IncludeAllPlayersInRadius);

				}

				//IncludeFactionBalance
				if (tag.StartsWith("[IncludeFactionBalance:") == true) {

					TagParse.TagBoolCheck(tag, ref this.IncludeFactionBalance);

				}

				//PlayerCreditsCheckRadius
				if (tag.StartsWith("[PlayerCreditsCheckRadius:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.PlayerCreditsCheckRadius);

				}

				//MinimumPlayerCredits
				if (tag.StartsWith("[MinimumPlayerCredits:") == true) {

					TagParse.TagIntCheck(tag, ref this.MinimumPlayerCredits);

				}

				//MaximumPlayerCredits
				if (tag.StartsWith("[MaximumPlayerCredits:") == true) {

					TagParse.TagIntCheck(tag, ref this.MaximumPlayerCredits);

				}

				//UsePlayerFactionReputation
				if (tag.StartsWith("[UsePlayerFactionReputation:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UsePlayerFactionReputation);

				}

				//PlayerReputationCheckRadius
				if (tag.StartsWith("[PlayerReputationCheckRadius:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.PlayerReputationCheckRadius);

				}

				//CheckReputationAgainstOtherNPCFaction
				if (tag.StartsWith("[CheckReputationAgainstOtherNPCFaction:") == true) {

					TagParse.TagStringCheck(tag, ref this.CheckReputationAgainstOtherNPCFaction);

				}

				//MinimumReputation
				if (tag.StartsWith("[MinimumReputation:") == true) {

					TagParse.TagIntCheck(tag, ref this.MinimumReputation);

				}

				//MaximumReputation
				if (tag.StartsWith("[MaximumReputation:") == true) {

					TagParse.TagIntCheck(tag, ref this.MaximumReputation);

				}

				//UseSignalRequirement 
				if (tag.StartsWith("[UseSignalRequirement:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseSignalRequirement);

				}

				//MinSignalRadius 
				if (tag.StartsWith("[MinSignalRadius:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MinSignalRadius);

				}

				//MaxSignalRadius 
				if (tag.StartsWith("[MaxSignalRadius:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.MaxSignalRadius);

				}

				//AllowNpcSignals  
				if (tag.StartsWith("[AllowNpcSignals:") == true) {

					TagParse.TagBoolCheck(tag, ref this.AllowNpcSignals);

				}

				//UseOnlySelfOwnedSignals  
				if (tag.StartsWith("[UseOnlySelfOwnedSignals:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseOnlySelfOwnedSignals);

				}

				//MatchSignalName  
				if (tag.StartsWith("[MatchSignalName:") == true) {

					TagParse.TagStringCheck(tag, ref this.MatchSignalName);

				}

				//ChargeNpcFactionForSpawn
				if (tag.StartsWith("[ChargeNpcFactionForSpawn:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ChargeNpcFactionForSpawn);

				}

				//ChargeForSpawning
				if (tag.StartsWith("[ChargeForSpawning:") == true) {

					TagParse.TagLongCheck(tag, ref this.ChargeForSpawning);

				}

				//UseSandboxCounterCosts
				if (tag.StartsWith("[UseSandboxCounterCosts:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseSandboxCounterCosts);

				}

				//SandboxCounterCostNames
				if (tag.StartsWith("[SandboxCounterCostNames:") == true) {

					TagParse.TagStringListCheck(tag, ref this.SandboxCounterCostNames);

				}

				//SandboxCounterCostAmounts
				if (tag.StartsWith("[SandboxCounterCostAmounts:") == true) {

					TagParse.TagIntListCheck(tag, ref this.SandboxCounterCostAmounts);

				}

				//UseRemoteControlCodeRestrictions
				if (tag.StartsWith("[UseRemoteControlCodeRestrictions:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseRemoteControlCodeRestrictions);

				}

				//RemoteControlCode
				if (tag.StartsWith("[RemoteControlCode:") == true) {

					TagParse.TagStringCheck(tag, ref this.RemoteControlCode);

				}

				//RemoteControlCodeMinDistance
				if (tag.StartsWith("[RemoteControlCodeMinDistance:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.RemoteControlCodeMinDistance);

				}

				//RemoteControlCodeMaxDistance
				if (tag.StartsWith("[RemoteControlCodeMaxDistance:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.RemoteControlCodeMaxDistance);

				}

				//RequireAllMods
				if (tag.StartsWith("[RequiredMods:") == true || tag.StartsWith("[RequireAllMods") == true) {

					TagParse.TagUlongListCheck(tag, ref this.RequireAllMods);

				}

				//ExcludeAnyMods
				if (tag.StartsWith("[ExcludedMods:") == true || tag.StartsWith("[ExcludeAnyMods") == true) {

					TagParse.TagUlongListCheck(tag, ref this.ExcludeAnyMods);

				}

				//RequireAnyMods
				if (tag.StartsWith("[RequireAnyMods:") == true) {

					TagParse.TagUlongListCheck(tag, ref this.RequireAnyMods);

				}

				//ExcludeAllMods
				if (tag.StartsWith("[ExcludeAllMods:") == true) {

					TagParse.TagUlongListCheck(tag, ref this.ExcludeAllMods);

				}

				//RequiredPlayersOnline
				if (tag.StartsWith("[RequiredPlayersOnline:") == true) {

					TagParse.TagUlongListCheck(tag, ref this.RequiredPlayersOnline);

				}

				//RequiredAnyPlayersOnline
				if (tag.StartsWith("[RequiredAnyPlayersOnline:") == true) {

					TagParse.TagUlongListCheck(tag, ref this.RequiredAnyPlayersOnline);

				}

				//UseKnownPlayerLocations
				if (tag.StartsWith("[UseKnownPlayerLocations:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseKnownPlayerLocations);

				}

				//KnownPlayerLocationMustMatchFaction
				if (tag.StartsWith("[KnownPlayerLocationMustMatchFaction:") == true) {

					TagParse.TagBoolCheck(tag, ref this.KnownPlayerLocationMustMatchFaction);

				}

				//KnownPlayerLocationMinSpawnedEncounters
				if (tag.StartsWith("[KnownPlayerLocationMinSpawnedEncounters:") == true) {

					TagParse.TagIntCheck(tag, ref this.KnownPlayerLocationMinSpawnedEncounters);

				}

				//KnownPlayerLocationMaxSpawnedEncounters
				if (tag.StartsWith("[KnownPlayerLocationMaxSpawnedEncounters:") == true) {

					TagParse.TagIntCheck(tag, ref this.KnownPlayerLocationMaxSpawnedEncounters);

				}

				//ZoneConditions
				if (tag.StartsWith("[ZoneConditions:") == true) {

					TagParse.TagZoneConditionsProfileCheck(tag, ref this.ZoneConditions);

				}

				//CustomApiConditions
				if (tag.StartsWith("[CustomApiConditions:") == true) {

					TagParse.TagStringListCheck(tag, ref this.CustomApiConditions);

				}

				//Territory
				if (tag.StartsWith("[Territory:") == true) {

					TagParse.TagStringCheck(tag, ref this.ZoneConditions[0].ZoneName);

				}

				//MinDistanceFromTerritoryCenter
				if (tag.StartsWith("[MinDistanceFromTerritoryCenter:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.ZoneConditions[0].MinDistanceFromZoneCenter);

				}

				//MaxDistanceFromTerritoryCenter
				if (tag.StartsWith("[MaxDistanceFromTerritoryCenter:") == true) {

					TagParse.TagDoubleCheck(tag, ref this.ZoneConditions[0].MaxDistanceFromZoneCenter);

				}


				//BossCustomAnnounceEnable
				if (tag.StartsWith("[BossCustomAnnounceEnable:") == true) {

					TagParse.TagBoolCheck(tag, ref this.BossCustomAnnounceEnable);

				}

				//BossCustomAnnounceAuthor
				if (tag.StartsWith("[BossCustomAnnounceAuthor:") == true) {

					TagParse.TagStringCheck(tag, ref this.BossCustomAnnounceAuthor);

				}

				//BossCustomAnnounceMessage
				if (tag.StartsWith("[BossCustomAnnounceMessage:") == true) {

					TagParse.TagStringCheck(tag, ref this.BossCustomAnnounceMessage);

				}

				//BossCustomGPSLabel
				if (tag.StartsWith("[BossCustomGPSLabel:") == true) {

					TagParse.TagStringCheck(tag, ref this.BossCustomGPSLabel);

				}

				//BossCustomGPSColor
				if (tag.StartsWith("[BossCustomGPSColor:") == true) {

					TagParse.TagVector3DCheck(tag, ref this.BossCustomGPSColor);

				}

				//BossMusicId
				if (tag.StartsWith("[BossMusicId:") == true) {

					TagParse.TagStringCheck(tag, ref this.BossMusicId);

				}

				//PlaySoundAtSpawnTriggerPosition
				if (tag.StartsWith("[PlaySoundAtSpawnTriggerPosition:") == true) {

					TagParse.TagBoolCheck(tag, ref this.PlaySoundAtSpawnTriggerPosition);

				}

				//SpawnTriggerPositionSoundId
				if (tag.StartsWith("[SpawnTriggerPositionSoundId:") == true) {

					TagParse.TagStringCheck(tag, ref this.SpawnTriggerPositionSoundId);

				}

				//DisableDampeners
				if (tag.StartsWith("[DisableDampeners:") == true) {

					TagParse.TagBoolCheck(tag, ref this.DisableDampeners);
					setDampeners = true;

				}


				//RotateFirstCockpitToForward
				if (tag.StartsWith("[RotateFirstCockpitToForward:") == true) {

					TagParse.TagBoolCheck(tag, ref this.RotateFirstCockpitToForward);

				}

				//PositionAtFirstCockpit
				if (tag.StartsWith("[PositionAtFirstCockpit:") == true) {

					TagParse.TagBoolCheck(tag, ref this.PositionAtFirstCockpit);

				}

				//SpawnRandomCargo
				if (tag.StartsWith("[SpawnRandomCargo:") == true) {

					TagParse.TagBoolCheck(tag, ref this.SpawnRandomCargo);

				}

				//ReactorsOn
				if (tag.StartsWith("[ReactorsOn:") == true) {

					TagParse.TagBoolCheck(tag, ref this.ReactorsOn);

				}

				//RemoveVoxelsIfGridRemoved
				if (tag.StartsWith("[RemoveVoxelsIfGridRemoved:") == true) {

					TagParse.TagBoolCheck(tag, ref this.RemoveVoxelsIfGridRemoved);

				}

				//UseGridOrigin
				if (tag.StartsWith("[UseGridOrigin:") == true) {

					TagParse.TagBoolCheck(tag, ref this.UseGridOrigin);

				}

				//PrefabSpawningMode
				if (tag.StartsWith("[PrefabSpawningMode:") == true) {

					TagParse.TagPrefabSpawnModeEnumCheck(tag, ref this.PrefabSpawningMode);

				}

				//AllowPrefabIndexReuse
				if (tag.StartsWith("[AllowPrefabIndexReuse:") == true) {

					TagParse.TagBoolCheck(tag, ref this.AllowPrefabIndexReuse);

				}

				//PrefabIndexes
				if (tag.StartsWith("[PrefabIndexes:") == true) {

					TagParse.TagIntListCheck(tag, true, ref this.PrefabIndexes);

				}

				//PrefabIndexGroupNames
				if (tag.StartsWith("[PrefabIndexGroupNames:") == true) {

					TagParse.TagStringListCheck(tag, ref this.PrefabIndexGroupNames);

				}

				//PrefabIndexGroupValues
				if (tag.StartsWith("[PrefabIndexGroupValues:") == true) {

					TagParse.TagIntListCheck(tag, true, ref this.PrefabIndexGroupValues);

				}

				//PrefabOffsetOverrides
				if (tag.StartsWith("[PrefabOffsetOverrides:") == true) {

					TagParse.TagVector3DListCheck(tag, ref this.PrefabOffsetOverrides);

				}

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
