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

        //Defense Shields
        public static bool DefenseShields = false;
        private static ulong _defenseShieldsSteamId = 1365616918;

        //WeaponCore
        public static bool WeaponCore = false;
        private static ulong _weaponCoreSteamId = 1918681825;

        //AiEnabled
        public static bool AiEnabled = false;
        private static ulong _aiEnabledSteamId = 2408831996;

        //Energy Shields
        public static bool EnergyShields = false;
        private static ulong _energyShieldsSteamId = 484504816;

        //Nanobot Build and Repair
        public static bool NanobotBuildAndRepair = false;
        private static ulong _nanobotBuildAndRepairSteamId = 857053359;

        public static void DetectAddons() {

            ConfigInstance = MyAPIGateway.Utilities.GamePaths.ModScopeName;

            foreach (var mod in MyAPIGateway.Session.Mods) {

                var id = mod.PublishedFileId;

                if (!ModIdList.Contains(id))
                    ModIdList.Add(id);

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
                if (id == _waterModSteamId) {

                    SpawnLogger.Write("Water Mod Detected", SpawnerDebugEnum.Startup);
                    WaterMod = true;
                    continue;

                }

                //Defense Shields
                if (id == _defenseShieldsSteamId) {

                    SpawnLogger.Write("Defense Shields Mod Detected", SpawnerDebugEnum.Startup);
                    DefenseShields = true;
                    continue;

                }

                //WeaponCore
                if (id == _weaponCoreSteamId) {

                    SpawnLogger.Write("WeaponCore Mod Detected", SpawnerDebugEnum.Startup);
                    WeaponCore = true;
                    continue;

                }

                //AiEnabled
                if (id == _aiEnabledSteamId) {

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

            }

        }

        public static void WeaponCoreCallback() {

            if (!APIs.WeaponCore.IsReady)
                return;

            APIs.WeaponCoreApiLoaded = true;
            APIs.WeaponCore.GetAllCoreWeapons(BlockManager.AllWeaponCoreBlocks);
            APIs.WeaponCore.GetAllCoreTurrets(BlockManager.AllWeaponCoreTurrets);
            APIs.WeaponCore.GetAllCoreStaticLaunchers(BlockManager.AllWeaponCoreGuns);

        }

    }

}
