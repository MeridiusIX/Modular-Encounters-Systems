using ModularEncountersSystems.BlockLogic;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Spawning {
    public static class PlayerSpawnWatcher {

        public static int Timer = 0;
        public static List<WatchedPlayer> Players = new List<WatchedPlayer>();
        private static bool _firstRun = false;

        public static void Setup() {

            PlayerManager.NewPlayerDetected += AddNewPlayer;
            TaskProcessor.Tick60.Tasks += CheckPlayers;
            MES_SessionCore.UnloadActions += Unload;

        }

        public static void CheckPlayers() {

            Timer++;

            if (Timer < Settings.General.PlayerWatcherTimerTrigger)
                return;

            if (!_firstRun) {

                _firstRun = true;

                try {

                    var blockTest = new InhibitorLogic();
                    Action blockTestAction = blockTest.RunTick100;

                } catch (Exception) {

                    SpawnGroupManager.AddonDetected = true;

                }

            }

            Timer = 0;

            WaveManager.Run();

            for (int i = Players.Count - 1; i >= 0; i--) {

                Players[i].ProcessPlayerTimers();
            
            }
        
        }

        public static void AddNewPlayer(PlayerEntity player) {

            Players.Add(new WatchedPlayer(player));

        }

        public static WatchedPlayer GetWatchedPlayer(long identityId) {

            for (int i = Players.Count - 1; i >= 0; i--) {

                var id = Players[i]?.Player?.Player?.IdentityId ?? 0;
                if (id == identityId)
                    return Players[i];
            
            }

            return null;

        }

        public static void Unload() {

            PlayerManager.NewPlayerDetected -= AddNewPlayer;
            TaskProcessor.Tick60.Tasks -= CheckPlayers;
            Players.Clear();

        }

    }

}
