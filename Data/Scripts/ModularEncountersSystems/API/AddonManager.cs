using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;

namespace ModularEncountersSystems.API {
	public static class AddonManager {

		public static List<ulong> ModIdList = new List<ulong>();
		public static Dictionary<string, string> ModIdNameReferences = new Dictionary<string, string>();
		public static string ConfigInstance;

		//Modular Encounters Systems


		//NPC Weapons Upgrade
		public static bool NpcWeaponsUpgrade { get { return _npcWeaponsUpgrade || Settings.Grids.EnableGlobalNPCWeaponRandomizer; } }
		private static bool _npcWeaponsUpgrade = false;
		private static ulong _npcWeaponsUpgradeSteamId = 1555044803;
		private static ulong _npcWeaponsUpgradeModIoId = 42;

		//NPC Shield Provider
		public static bool NpcShieldProvider { get { return _npcShieldProvider || Settings.Grids.EnableGlobalNPCShieldProvider; } }
		private static bool _npcShieldProvider = false;
		private static ulong _npcShieldProviderSteamId = 2043339470;

		//Suppress Vanilla Cargo Ships
		public static bool SuppressVanillaCargoShips { get { return _suppressVanillaCargoShips; } } //TODO: Point To Settings
		private static bool _suppressVanillaCargoShips = false;
		private static ulong _suppressVanillaCargoShipsSteamId = 888457124;
		private static ulong _suppressVanillaCargoShipsModIoId = 42;

		//Suppress Vanilla Encounters
		public static bool SuppressVanillaEncounters { get { return _suppressVanillaEncounters; } } //TODO: Point To Settings
		private static bool _suppressVanillaEncounters = false;
		private static ulong _suppressVanillaEncountersSteamId = 888457381;
		private static ulong _suppressVanillaEncountersModIoId = 42;

		//Space Wave Spawner
		public static bool SpaceWaveSpawner { get { return _spaceWaveSpawner || Settings.SpaceCargoShips.EnableWaveSpawner; } }
		private static bool _spaceWaveSpawner = false;
		private static ulong _spaceWaveSpawnerSteamId = 1773965697;
		private static ulong _spaceWaveSpawnerModIoId = 42;

		//Planet Creature Spawner
		public static bool PlanetCreatureSpawner { get { return _planetCreatureSpawner || Settings.Creatures.OverrideVanillaCreatureSpawns; } }
		private static bool _planetCreatureSpawner = false;
		private static ulong _planetCreatureSpawnerSteamId = 2371761016;
		private static ulong _planetCreatureSpawnerModIoId = 42;

		//Water Mod
		public static bool WaterMod = false;
		private static ulong _waterModSteamId = 2200451495;
		private static ulong _waterModModIoId = 2148454;

		//Nebula Mod
		public static bool NebulaMod = false;
		private static ulong _nebulaModSteamId = 2200451495;

		//Defense Shields
		public static bool DefenseShields = false;
		private static ulong _defenseShieldsSteamId = 1365616918;
		private static ulong _defenseShieldsSteamIdNew = 3154379105;
		private static ulong _defenseShieldsSteamIdForkSC = 3149619582;
                private static ulong _defenseShieldsSteamIdForkFP = 3191211586;

		//WeaponCore
		public static bool WeaponCore = false;
		private static ulong _weaponCoreSteamId = 1918681825;
		private static ulong _coreSystemsSteamId = 2189703321;
		private static ulong _weaponCoreSteamIdNew = 3154371364;
		private static ulong _weaponCoreSteamIdForkSC = 3149625043;
 		private static ulong _weaponCoreSteamIdForkFP = 3191232165;

		//AiEnabled
		public static bool AiEnabled = false;
		private static ulong _aiEnabledDevSteamId = 2408831996;
		private static ulong _aiEnabledSteamId = 2596208372;
		//Energy Shields
		public static bool EnergyShields = false;
		private static ulong _energyShieldsSteamId = 484504816;

		//Nanobot Build and Repair
		public static bool NanobotBuildAndRepair = false;
		private static ulong _nanobotBuildAndRepairSteamId = 857053359;

		//Aerodynamic Drag
		public static bool AerodynamicDrag = false;
		public static ulong _aerodynamicDragSteamId = 571920453;

		//TextHUDAPI
		public static bool TextHudApi = false;
		private static ulong _textHudApiSteamId = 758597413;

		//Stealth Mod
		public static bool StealthMod = false;
		private static ulong _stealthModSteamId = 2805859069;

		//ChaoticSpawningSettings
		public static bool ChaoticSpawningSettingsMod = false;
		public static ulong _chaoticSpawningSettingsSteamId = 3023485481;

		public static void DetectAddons() {

			ConfigInstance = MyAPIGateway.Utilities.GamePaths.ModScopeName;

			foreach (var mod in MyAPIGateway.Session.Mods) {

				var id = mod.PublishedFileId;

				if (!ModIdList.Contains(id))
					ModIdList.Add(id);

				if (id != 0) {

					if (!ModIdNameReferences.ContainsKey(id.ToString()))
						ModIdNameReferences.Add(id.ToString(), mod.FriendlyName);

				} else {

					if (!ModIdNameReferences.ContainsKey(mod.Name))
						ModIdNameReferences.Add(mod.Name, mod.FriendlyName);

				}

				

				//NPC Weapons Upgrade
				if (id == _npcWeaponsUpgradeSteamId || id == _npcWeaponsUpgradeModIoId) {

					SpawnLogger.Write("NPC Weapons Upgrade Mod Detected", SpawnerDebugEnum.Startup);
					_npcWeaponsUpgrade = true;
					continue;

				}

				//NPC Shield Provider
				if (id == _npcShieldProviderSteamId) {

					SpawnLogger.Write("NPC Shield Provider Mod Detected", SpawnerDebugEnum.Startup);
					_npcShieldProvider = true;
					continue;

				}

				//Suppress Vanilla Cargo Ships
				if (id == _suppressVanillaCargoShipsSteamId || id == _suppressVanillaCargoShipsModIoId) {

					SpawnLogger.Write("Suppress Vanilla Cargo Ships Mod Detected", SpawnerDebugEnum.Startup);
					_suppressVanillaCargoShips = true;
					continue;

				}

				//Suppress Vanilla Encounters
				if (id == _suppressVanillaEncountersSteamId || id == _suppressVanillaEncountersModIoId) {

					SpawnLogger.Write("Suppress Vanilla Encounters Mod Detected", SpawnerDebugEnum.Startup);
					_suppressVanillaEncounters = true;
					continue;

				}

				//Space Wave Spawner
				if (id == _spaceWaveSpawnerSteamId || id == _spaceWaveSpawnerModIoId) {

					SpawnLogger.Write("Wave Spawner Mod Detected", SpawnerDebugEnum.Startup);
					_spaceWaveSpawner = true;
					continue;

				}

				//Planet Creature Spawner
				if (id == _planetCreatureSpawnerSteamId || id == _planetCreatureSpawnerModIoId) {

					SpawnLogger.Write("Planet Creature Spawner Mod Detected", SpawnerDebugEnum.Startup);
					_planetCreatureSpawner = true;
					continue;

				}

				//Water Mod
				if (id == _waterModSteamId || id == _waterModModIoId) {

					SpawnLogger.Write("Water Mod Detected", SpawnerDebugEnum.Startup);
					WaterMod = true;
					continue;

				}

				//Nebula Mod
				if (id == _nebulaModSteamId) {

					SpawnLogger.Write("Nebula Mod Detected", SpawnerDebugEnum.Startup);
					NebulaMod = true;
					continue;

				}

				//Defense Shields
				if (id == _defenseShieldsSteamId || id == _defenseShieldsSteamIdNew || id == _defenseShieldsSteamIdForkSC || id == _defenseShieldsSteamIdForkFP) {

					SpawnLogger.Write("Defense Shields Mod Detected", SpawnerDebugEnum.Startup);
					DefenseShields = true;
					continue;

				}

				//WeaponCore
				if (id == _weaponCoreSteamId || id == _coreSystemsSteamId || id == _weaponCoreSteamIdNew || id == _weaponCoreSteamIdForkSC || id == _weaponCoreSteamIdForkFP || mod.GetPath().Contains("AppData\\Roaming\\SpaceEngineers\\Mods\\CoreSystems")) {

					SpawnLogger.Write("WeaponCore Mod Detected", SpawnerDebugEnum.Startup);
					WeaponCore = true;
					continue;

				}

				//AiEnabled
				if (id == _aiEnabledSteamId || id == _aiEnabledDevSteamId) {

					SpawnLogger.Write("AiEnabled Mod Detected", SpawnerDebugEnum.Startup);
					AiEnabled = true;
					continue;
				
				}

				//EnergyShields
				if (id == _energyShieldsSteamId) {

					SpawnLogger.Write("Energy Shields Mod Detected", SpawnerDebugEnum.Startup);
					EnergyShields = true;
					continue;

				}

				//NanobotBuildAndRepair
				if (id == _nanobotBuildAndRepairSteamId) {

					SpawnLogger.Write("Nanobots Build and Repair Mod Detected", SpawnerDebugEnum.Startup);
					NanobotBuildAndRepair = true;
					continue;

				}

				//Aerodynamic Drag
				if (id == _aerodynamicDragSteamId) {

					SpawnLogger.Write("Aerodynamic Drag Mod Detected", SpawnerDebugEnum.Startup);
					AerodynamicDrag = true;
					continue;

				}

				//TextHudAPI
				if (id == _textHudApiSteamId) {

					SpawnLogger.Write("TextHudAPI Mod Detected", SpawnerDebugEnum.Startup);
					TextHudApi = true;
					continue;

				}

				if (id == _stealthModSteamId) {

					SpawnLogger.Write("Stealth Drive Mod Detected", SpawnerDebugEnum.Startup);
					StealthMod = true;
					continue;

				}

				if (id == _chaoticSpawningSettingsSteamId) {

					SpawnLogger.Write("Chaotic Spawning Settings Mod Detected", SpawnerDebugEnum.Startup);
					ChaoticSpawningSettingsMod = true;
					continue;

				}

			}

			

		}

		public static void ProcessMesAddons() {

			if (ChaoticSpawningSettingsMod) {

				MyAPIGateway.Utilities.SetVariable<bool>("MES-ChaoticSpawningSettings-UsedInWorld", true);
				PresetConfigs.DelimitSpawns();
				PresetConfigs.FrequentSpawns();
				PresetConfigs.Suffering();
				Settings.SaveAll();
			}

		}

		public static void WeaponCoreCallback() {

			if (!APIs.WeaponCore.IsReady)
				return;

			SpawnLogger.Write("WeaponCore API Loaded", SpawnerDebugEnum.Startup);
			APIs.WeaponCoreApiLoaded = true;
			APIs.WeaponCore.GetAllCoreWeapons(BlockManager.AllWeaponCoreBlocks);
			APIs.WeaponCore.GetAllCoreTurrets(BlockManager.AllWeaponCoreTurrets);
			APIs.WeaponCore.GetAllCoreStaticLaunchers(BlockManager.AllWeaponCoreGuns);

			if (BlockManager.AllWeaponCoreBlocks.Count > 0) {

				if (BlockManager.AllWeaponCoreTurrets.Count == 0 || BlockManager.AllWeaponCoreGuns.Count == 0) {

					foreach (var weapon in BlockManager.AllWeaponCoreBlocks) {

						foreach (var definition in APIs.WeaponCore.WeaponDefinitions) {

							var list = BlockManager.AllWeaponCoreTurrets;

							if (!definition.HardPoint.Ai.TurretAttached)
								list = BlockManager.AllWeaponCoreGuns;

							if (definition.Assignments.MountPoints.Length > 0) {

								if (definition.Assignments.MountPoints[0].SubtypeId != weapon.SubtypeName)
									continue;

								if(!list.Contains(weapon))
									list.Add(weapon);

								break;

							}
						
						}

					}
				
				}

			}

		}

	}

}
