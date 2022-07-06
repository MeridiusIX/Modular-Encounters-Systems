using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class GlobalConditionProfile {

		public bool UseSettingsCheck;
		public BoolEnum SettingsAutoHeal;
		public BoolEnum SettingsAutoRespawn;
		public BoolEnum SettingsBountyContracts;
		public BoolEnum SettingsDestructibleBlocks;
		public BoolEnum SettingsCopyPaste;
		public BoolEnum SettingsContainerDrops;
		public BoolEnum SettingsEconomy;
		public BoolEnum SettingsEnableDrones;
		public BoolEnum SettingsIngameScripts; 
		public BoolEnum SettingsJetpack;
		public BoolEnum SettingsOxygen;
		public BoolEnum SettingsResearch;
		public BoolEnum SettingsSpawnWithTools;
		public BoolEnum SettingsSpiders; 
		public BoolEnum SettingsSubgridDamage; 
		public BoolEnum SettingsSunRotation; 
		public BoolEnum SettingsSupergridding; 
		public BoolEnum SettingsThrusterDamage;
		public BoolEnum SettingsVoxelDestruction;
		public BoolEnum SettingsWeaponsEnabled; 
		public BoolEnum SettingsWeather; 
		public BoolEnum SettingsWolves;

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

		public bool UsePlayerCountCheck;
		public int MinimumPlayers;
		public int MaximumPlayers;

		public bool UseDifficulty;
		public int MinDifficulty;
		public int MaxDifficulty;

		public List<ulong> RequireAllMods;
		public List<ulong> RequireAnyMods;
		public List<ulong> ExcludeAllMods;
		public List<ulong> ExcludeAnyMods;

		public List<string> ModBlockExists;

		public List<ulong> RequiredPlayersOnline;
		public List<ulong> RequiredAnyPlayersOnline;

		public List<string> SandboxVariables;
		public List<string> FalseSandboxVariables;

		public GlobalConditionProfile() {

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

		}


	}
}
