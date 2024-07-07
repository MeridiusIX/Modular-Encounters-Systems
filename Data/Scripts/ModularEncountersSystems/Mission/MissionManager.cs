using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using Sandbox.ModAPI;
using Sandbox.Game;
using System.Text;
using ModularEncountersSystems.Logging;
using VRageMath;
using VRage.Game.ModAPI;
using Sandbox.ModAPI.Contracts;
using VRage.Game;
using ModularEncountersSystems.Entities;

namespace ModularEncountersSystems.Missions {


    public static class MissionManager {
        public static List<Mission> ActiveMissionList = new List<Mission>();

        private static string _saveMissionsListName = "MES-Missions";



        // contract stuff
        public static List<Contract> GeneratedContracts = new List<Contract>();
        //public static List<long> ContractsForRemoval = new List<long>();




        public static void Setup()
        {

            //SpawnLogger.Write("Start", SpawnerDebugEnum.Dev, true);
            //Register Any Actions/Events
            MES_SessionCore.SaveActions += SaveData;
            MES_SessionCore.UnloadActions += UnloadData;
            //TaskProcessor.Tick30.Tasks += ProcessEvents;

            //SpawnLogger.Write("Existing Event Data", SpawnerDebugEnum.Dev, true);
            //Get Existing Event Data
            string missionsListString = "";

            /*
            if (MyAPIGateway.Utilities.GetVariable<string>(_saveMissionsListName, out missionsListString))
            {

                var missionsListSerialized = Convert.FromBase64String(missionsListString);
                MissionList = MyAPIGateway.Utilities.SerializeFromBinary<List<Mission>>(missionsListSerialized);

            }

            if (MissionList == null)
                MissionList = new List<Mission>();
            */
            SaveData();
        }

        public static void PlayersNearby(Vector3D position, int distance, out List<long> playerId)
        {
            playerId = new List<long>();

            foreach (var player in PlayerManager.Players)
            {
                if (player == null)
                    continue;

                if (Vector3D.Distance(player.GetPosition(), position) < distance)
                {
                    playerId.Add(player.Player.IdentityId);
                }
            }
        }

        public static void SaveData()
        {

            /*
            //Events
            var eventsListSerialized = MyAPIGateway.Utilities.SerializeToBinary<List<Mission>>(MissionList);
            var eventsListString = Convert.ToBase64String(eventsListSerialized);
            MyAPIGateway.Utilities.SetVariable<string>(_saveMissionsListName, eventsListString);
            */
        }

        public static void UnloadData()
        {

            PurgeAllActiveContracts();
            //Unregister Any Actions/Events That Were Registered in Setup()
            MES_SessionCore.SaveActions -= SaveData;
            MES_SessionCore.UnloadActions -= UnloadData;
            //TaskProcessor.Tick30.Tasks -= ProcessEvents;
        }




        private static void PurgeAllActiveContracts()
        {
            foreach (var contract in GeneratedContracts)
            {
                if (contract == null || contract.ContractId == 0) continue;

                MyAPIGateway.ContractSystem.RemoveContract(contract.ContractId);
            }

            GeneratedContracts.Clear();

        }

        public static void PurgeContract(long ContractId)
        {

            MyAPIGateway.Utilities.InvokeOnGameThread(() =>
            {
                MyAPIGateway.ContractSystem.RemoveContract(ContractId);
            });

        }



        public static void PurgeContractsWithMissionSubtypeId(string missionProfileSubtypeId)
        {
            foreach (var contract in GeneratedContracts)
            {
                if (contract.MissionReference.ProfileSubtypeId == missionProfileSubtypeId)
                {
                    MyAPIGateway.Utilities.InvokeOnGameThread(() =>
                    {
                        MyAPIGateway.ContractSystem.RemoveContract(contract.ContractId);
                    });
                }
            }
        }


    }
}
