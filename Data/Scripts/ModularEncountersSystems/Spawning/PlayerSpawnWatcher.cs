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

        public static void Setup() {

            PlayerManager.NewPlayerDetected += AddNewPlayer;
            TaskProcessor.Tick60.Tasks += CheckPlayers;
            MES_SessionCore.UnloadActions += Unload;

        }

        public static void CheckPlayers() {

            Timer++;

            if (Timer < Settings.General.PlayerWatcherTimerTrigger)
                return;

            Timer = 0;

            for (int i = Players.Count - 1; i >= 0; i--) {

                Players[i].ProcessPlayerTimers();
            
            }
        
        }

        public static void AddNewPlayer(PlayerEntity player) {

            Players.Add(new WatchedPlayer(player));

        }

        public static void Unload() {

            PlayerManager.NewPlayerDetected -= AddNewPlayer;
            TaskProcessor.Tick60.Tasks -= CheckPlayers;
            Players.Clear();

        }

    }

}
