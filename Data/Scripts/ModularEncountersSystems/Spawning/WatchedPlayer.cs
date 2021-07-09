using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Spawning {
    public class WatchedPlayer {

        public PlayerEntity Player;

        public int SpaceCargoShipTimer;

        public int AtmoCargoShipTimer;

        public int RandomEncounterCheckTimer;
        public int RandomEncounterCoolDownTimer;
        public Vector3D RandomEncounterDistanceCoordCheck;

        public int PlanetaryInstallationCheckTimer;
        public int PlanetaryInstallationCooldownTimer;
        public Vector3D InstallationDistanceCoordCheck;

        public int BossEncounterCheckTimer;
        public int BossEncounterCooldownTimer;
        public bool BossEncounterActive;

        public int CreatureCheckTimer;

        public WatchedPlayer(PlayerEntity player) {

            Player = player;

            SpaceCargoShipTimer = Settings.SpaceCargoShips.FirstSpawnTime;
            AtmoCargoShipTimer = Settings.PlanetaryCargoShips.FirstSpawnTime;

            RandomEncounterCheckTimer = Settings.RandomEncounters.SpawnTimerTrigger;
            RandomEncounterCoolDownTimer = 0;

            PlanetaryInstallationCheckTimer = Settings.PlanetaryInstallations.SpawnTimerTrigger;
            PlanetaryInstallationCooldownTimer = 0;

            BossEncounterCheckTimer = Settings.BossEncounters.SpawnTimerTrigger;
            BossEncounterCooldownTimer = 0;
            BossEncounterActive = false;

            CreatureCheckTimer = MathTools.RandomBetween(Settings.Creatures.MinCreatureSpawnTime, Settings.Creatures.MaxCreatureSpawnTime);

            RandomEncounterDistanceCoordCheck = player.GetPosition();
            InstallationDistanceCoordCheck = player.GetPosition();

        }

        public void ProcessPlayerTimers() {

            if (!Player.Online)
                return;

            if (Player.Player?.Character == null || Player.Player.Character.IsDead)
                return;

            //Space/Lunar Cargo Ships
            if (Settings.SpaceCargoShips.EnableSpawns) {

                //TODO: Stop Here If Wave Spawner Active
                ApplyDecrement(ref SpaceCargoShipTimer);

                if (SpawnRequest.PlayerSpawnEligiblity(SpawningType.SpaceCargoShip, this)) {
                
                    
                
                }
                
            }

            //Planetary/Gravity Cargo Ships
            if (Settings.PlanetaryCargoShips.EnableSpawns) {

                ApplyDecrement(ref AtmoCargoShipTimer);

                if (SpawnRequest.PlayerSpawnEligiblity(SpawningType.PlanetaryCargoShip, this)) {



                }

            }

            //Random Encounters
            if (Settings.RandomEncounters.EnableSpawns) {

                if(RandomEncounterCoolDownTimer > 0)
                    ApplyDecrement(ref RandomEncounterCoolDownTimer);
                else
                    ApplyDecrement(ref RandomEncounterCheckTimer);

                if (SpawnRequest.PlayerSpawnEligiblity(SpawningType.RandomEncounter, this)) {



                }

            }

            //Planetary Installations
            if (Settings.PlanetaryInstallations.EnableSpawns) {

                if (PlanetaryInstallationCooldownTimer > 0)
                    ApplyDecrement(ref PlanetaryInstallationCooldownTimer);
                else
                    ApplyDecrement(ref PlanetaryInstallationCheckTimer);

                if (SpawnRequest.PlayerSpawnEligiblity(SpawningType.PlanetaryInstallation, this)) {



                }

            }

            //Boss Encounters
            if (Settings.BossEncounters.EnableSpawns) {

                if (BossEncounterCooldownTimer > 0)
                    ApplyDecrement(ref BossEncounterCooldownTimer);
                else
                    ApplyDecrement(ref BossEncounterCheckTimer);

                if (SpawnRequest.PlayerSpawnEligiblity(SpawningType.BossEncounter, this)) {



                }

            }

            //Creatures
            if (Settings.Creatures.EnableSpawns) {

                ApplyDecrement(ref CreatureCheckTimer);

                if (SpawnRequest.PlayerSpawnEligiblity(SpawningType.Creature, this)) {



                }

            }

        }

        public bool CheckTimer(SpawningType spawnType) {

            if (spawnType == SpawningType.SpaceCargoShip && SpaceCargoShipTimer <= 0)
                return true;

            if (spawnType == SpawningType.RandomEncounter && RandomEncounterCheckTimer <= 0)
                return true;

            if (spawnType == SpawningType.PlanetaryCargoShip && AtmoCargoShipTimer <= 0)
                return true;

            if (spawnType == SpawningType.PlanetaryInstallation && PlanetaryInstallationCheckTimer <= 0)
                return true;

            if (spawnType == SpawningType.BossEncounter && BossEncounterCheckTimer <= 0)
                return true;

            if (spawnType == SpawningType.Creature && CreatureCheckTimer <= 0)
                return true;

            return false;
        
        }

        public void ResetTimer(SpawningType spawnType) {

            if (spawnType == SpawningType.SpaceCargoShip && SpaceCargoShipTimer <= 0)
                SpaceCargoShipTimer = MathTools.RandomBetween(Settings.SpaceCargoShips.MinSpawnTime, Settings.SpaceCargoShips.MaxSpawnTime);

            if (spawnType == SpawningType.RandomEncounter && RandomEncounterCheckTimer <= 0)
                RandomEncounterCheckTimer = Settings.RandomEncounters.SpawnTimerTrigger;

            if (spawnType == SpawningType.PlanetaryCargoShip && AtmoCargoShipTimer <= 0)
                AtmoCargoShipTimer = MathTools.RandomBetween(Settings.PlanetaryCargoShips.MinSpawnTime, Settings.PlanetaryCargoShips.MaxSpawnTime);

            if (spawnType == SpawningType.PlanetaryInstallation && PlanetaryInstallationCheckTimer <= 0)
                PlanetaryInstallationCheckTimer = Settings.PlanetaryInstallations.SpawnTimerTrigger;

            if (spawnType == SpawningType.BossEncounter && BossEncounterCheckTimer <= 0)
                BossEncounterCheckTimer = Settings.BossEncounters.SpawnTimerTrigger;

            if (spawnType == SpawningType.Creature && CreatureCheckTimer <= 0)
                CreatureCheckTimer = MathTools.RandomBetween(Settings.Creatures.MinCreatureSpawnTime, Settings.Creatures.MaxCreatureSpawnTime);

        }

        private void ApplyDecrement(ref int setting) {

            setting -= Settings.General.PlayerWatcherTimerTrigger;

        }

    }

}

