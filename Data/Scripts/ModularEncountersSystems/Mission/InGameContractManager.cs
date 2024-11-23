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


    public static class InGameContractManager {

        public static List<long> ContractsForRemoval = new List<long>();
        private static string _saveContractsForRemovalName = "MES-ContractsForRemoval";

        // contract stuff
        public static List<Contract> GeneratedContracts = new List<Contract>();


        public static List<ActiveContract> ActiveContracts = new List<ActiveContract>();
        private static string _saveActiveContractsName = "MES-ActiveContracts";



        public static void Setup()
        {
            MES_SessionCore.SaveActions += SaveData;
            MES_SessionCore.UnloadActions += UnloadData;
            TaskProcessor.Tick100.Tasks += ProcessIngameContract;

            MyVisualScriptLogicProvider.ContractAbandoned += ContractAbandoned;
            MyVisualScriptLogicProvider.ContractFailed += ContractFailed;
            MyVisualScriptLogicProvider.ContractFinished += ContractFinished;
            MyVisualScriptLogicProvider.ContractAccepted += ContractAccepted;



            string ContractsForRemovalNameString = "";
            if (MyAPIGateway.Utilities.GetVariable<string>(_saveContractsForRemovalName, out ContractsForRemovalNameString))
            {
                var ContractsForRemovalSerialized = Convert.FromBase64String(ContractsForRemovalNameString);
                ContractsForRemoval = MyAPIGateway.Utilities.SerializeFromBinary<List<long>>(ContractsForRemovalSerialized);
            }

            string ActiveContractsString = "";
            if (MyAPIGateway.Utilities.GetVariable<string>(_saveActiveContractsName, out ActiveContractsString))
            {
                var ActiveContractsSerialized = Convert.FromBase64String(ActiveContractsString);
                ActiveContracts = MyAPIGateway.Utilities.SerializeFromBinary<List<ActiveContract>>(ActiveContractsSerialized);
            }



            if (ContractsForRemoval == null)
                ContractsForRemoval = new List<long>();

            for (int i = ContractsForRemoval.Count - 1; i >= 0; i--)
            {
                MyAPIGateway.ContractSystem.RemoveContract(ContractsForRemoval[i]);
                ContractsForRemoval.RemoveAt(i);
            }


            if (ActiveContracts == null)
                ActiveContracts = new List<ActiveContract>();

            SaveData();
        }


        
        public static void ContractAccepted(long contractId, MyDefinitionId contractDefinitionId, long acceptingPlayerId, bool isPlayerMade, long startingBlockId, long startingFactionId, long startingStationId)
        {

            if (isPlayerMade)
            {
                if(contractDefinitionId.SubtypeId.ToString() == "ObtainAndDeliver")
                {
                    return;
                }

                for (int i = GeneratedContracts.Count - 1; i >= 0; i--)
                {
                    var contract = GeneratedContracts[i];

                    if (contract.ContractId == contractId)
                    {

                        if (!contract.MissionReference.RunEventConditions())
                        {
                            MyVisualScriptLogicProvider.SendChatMessageColored("Contract no longer available. We apologize for the inconvenience", Color.Olive, "Contracts", acceptingPlayerId);

                            //Eventconditions not satisfied remove contract;
                            PurgeContractsWithMissionSubtypeId(contract.MissionReference.ProfileSubtypeId);

                            var player = PlayerManager.GetPlayerWithIdentityId(acceptingPlayerId);
                            if(player != null)
                                player.Player.RequestChangeBalance(contract.MissionReference.Collateral);


                            GeneratedContracts.RemoveAt(i);
                            ContractsForRemoval.Remove(contract.ContractId);
                            return;
                        }


                        if (contract.TryToActivateCustomContract(acceptingPlayerId))
                        {
                            //Suc
                            GeneratedContracts.RemoveAt(i);
                            ContractsForRemoval.Remove(contract.ContractId);
                        }
                        else
                        {
                            //PlayerConditions not satisfied, so reAdd the contract to the block
                            MyAPIGateway.Utilities.InvokeOnGameThread(() =>
                            {
                                MyAPIGateway.ContractSystem.RemoveContract(contractId);
                            });

                            var player = PlayerManager.GetPlayerWithIdentityId(acceptingPlayerId);

                            if (player != null)
                                player.Player.RequestChangeBalance(contract.MissionReference.Collateral);


                            GeneratedContracts.RemoveAt(i);
                            ContractsForRemoval.Remove(contract.ContractId);
                            contract.MissionReference.ReAddContractToBlock(contract.ContractId);


                        }


                        return;
                    }

                }


            }
        }

        public static void ContractAbandoned(long contractId, MyDefinitionId contractDefinitionId, long acceptingPlayerId, bool isPlayerMade, long startingBlockId, long startingFactionId, long startingStationId)
        {
            MyVisualScriptLogicProvider.ShowNotificationToAll("ContractAbandoned", 15000, "Red");
            if (isPlayerMade)
            {
                var contract = GetActiveContract(contractId);

                if (contract == null)
                {
                    return;
                }

                for (int i = 0; i < Events.EventManager.EventsList.Count; i++)
                {
                    var thisEvent = Events.EventManager.EventsList[i];

                    if (!thisEvent.Valid)
                    {
                        continue;
                    }

                    var thisEventTag = $"Abandoned@{contractId}";
                    if (thisEvent.Profile.Tags.Contains(thisEventTag))
                    {
                        thisEvent.ActivateEventActions();
                        thisEvent.RunCount++;
                    }
                }

                var PlayerList = new List<long>();

                foreach (var player in PlayerManager.Players)
                {
                    if (player.ProgressionData.Tags.Contains($"LeadPlayer@{contractId}"))
                    {
                        player.ProgressionData.Tags.Remove($"LeadPlayer@{contractId}");
                    }

                    if (player.ProgressionData.Tags.Contains($"@{contractId}"))
                    {
                        PlayerList.Add(player.GetEntityId());

                        player.ProgressionData.Tags.Remove($"@{contractId}");
                    }

                }

                FactionHelper.ChangePlayerReputationWithFactions(null, -contract.FailReputationPrice, PlayerList, contract.FactionTag, false, -1501, 1501);

                RemoveActiveContractInternal(contractId);
            }


        }


        public static void ContractFailed(long contractId, MyDefinitionId contractDefinitionId, long acceptingPlayerId, bool isPlayerMade, long startingBlockId, long startingFactionId, long startingStationId, bool IsAbandon)
        {
            MyVisualScriptLogicProvider.ShowNotificationToAll("ContractFailed", 15000, "Red");
            if (isPlayerMade)
            {
                var contract = GetActiveContract(contractId);

                if (contract == null)
                {
                    return;
                }

                for (int i = 0; i < Events.EventManager.EventsList.Count; i++)
                {
                    var thisEvent = Events.EventManager.EventsList[i];

                    if (!thisEvent.Valid)
                    {
                        continue;
                    }

                    var thisEventTag = $"Failed@{contractId}";
                    if (thisEvent.Profile.Tags.Contains(thisEventTag))
                    {
                        thisEvent.ActivateEventActions();
                        thisEvent.RunCount++;
                    }
                }

                foreach (var player in PlayerManager.Players)
                {
                    if (player.ProgressionData.Tags.Contains($"LeadPlayer@{contractId}"))
                    {
                        player.ProgressionData.Tags.Remove($"LeadPlayer@{contractId}");
                    }

                    if (player.ProgressionData.Tags.Contains($"@{contractId}"))
                    {
                        player.ProgressionData.Tags.Remove($"@{contractId}");
                    }

                }

                RemoveActiveContractInternal(contractId);
            }

        }

        public static void ContractFinished(long contractId, MyDefinitionId contractDefinitionId, long acceptingPlayerId, bool isPlayerMade, long startingBlockId, long startingFactionId, long startingStationId)
        {
            if (isPlayerMade)
            {
                var contract = GetActiveContract(contractId);

                if(contract == null)
                {
                    return;
                }


                var PlayerList = new List<long>();

                foreach (var player in PlayerManager.Players)
                {
                    if (player.ProgressionData.Tags.Contains($"LeadPlayer@{contractId}"))
                    {
                        player.Player.RequestChangeBalance(contract.Reward);
                        player.ProgressionData.Tags.Remove($"LeadPlayer@{contractId}");
                    }

                    if (player.ProgressionData.Tags.Contains($"@{contractId}"))
                    {
                        PlayerList.Add(player.GetEntityId());

                        player.ProgressionData.Tags.Remove($"@{contractId}");
                    }

                }

                FactionHelper.ChangePlayerReputationWithFactions(null, contract.ReputationReward, PlayerList, contract.FactionTag, false, -1501, 1501);




                RemoveActiveContractInternal(contractId);

                
            }
        }


        public static void ProcessIngameContract()
        {

        }

        public static void SaveData()
        {
            var contractListSerialized = MyAPIGateway.Utilities.SerializeToBinary<List<long>>(ContractsForRemoval);
            var contractListString = Convert.ToBase64String(contractListSerialized);
            MyAPIGateway.Utilities.SetVariable<string>(_saveContractsForRemovalName, contractListString);

            var ActiveContractsSerialized = MyAPIGateway.Utilities.SerializeToBinary<List<ActiveContract>>(ActiveContracts);
            var ActiveContractsString = Convert.ToBase64String(ActiveContractsSerialized);
            MyAPIGateway.Utilities.SetVariable<string>(_saveActiveContractsName, ActiveContractsString);
        }

        public static void UnloadData()
        {
            //Unregister Any Actions/Events That Were Registered in Setup()
            MES_SessionCore.SaveActions -= SaveData;
            MES_SessionCore.UnloadActions -= UnloadData;
            TaskProcessor.Tick100.Tasks -= ProcessIngameContract;

            MyVisualScriptLogicProvider.ContractAbandoned -= ContractAbandoned;
            MyVisualScriptLogicProvider.ContractFailed -= ContractFailed;
            MyVisualScriptLogicProvider.ContractFinished -= ContractFinished;



            MyVisualScriptLogicProvider.ContractAccepted -= ContractAccepted;

        }



        public static void PurgeContract(long ContractId)
        {

            MyAPIGateway.Utilities.InvokeOnGameThread(() =>
            {
                MyAPIGateway.ContractSystem.RemoveContract(ContractId);
            });

        }

        public static void PurgeContractsWithMissionSubtypeId(string missionProfileSubtypeId, long ExcludeContractId = -1)
        {
            foreach (var contract in GeneratedContracts)
            {
                if (contract.ContractId == ExcludeContractId)
                    continue;

                if (contract.MissionReference.ProfileSubtypeId == missionProfileSubtypeId)
                {

                    MyAPIGateway.Utilities.InvokeOnGameThread(() =>
                    {
                        MyAPIGateway.ContractSystem.RemoveContract(contract.ContractId);
                    });
                }
            }
        }


        public static ActiveContract GetActiveContract(long contractId)
        {
            for (int i = ActiveContracts.Count - 1; i >= 0; i--)
            {
                var ActiveContract = ActiveContracts[i];

                if (ActiveContract.ContractId == contractId)
                {
                    return ActiveContract;

                }
            }

            return null; 

        }

        public static void RemoveActiveContractInternal(long contractId)
        {
            for (int i = ActiveContracts.Count - 1; i >= 0; i--)
            {
                var ActiveContract = ActiveContracts[i];

                if(ActiveContract.ContractId == contractId)
                {
                    ActiveContracts.RemoveAt(i);
                    return;
                }
            }
        }




        public static void ClearBlockContracts(long blockId)
        {
            for (int i = GeneratedContracts.Count - 1; i >= 0; i--)
            {
                var generatedContracts = GeneratedContracts[i];

                if (generatedContracts.SourceBlock.Entity.EntityId == blockId)
                {
                    PurgeContract(generatedContracts.ContractId);
                    GeneratedContracts.RemoveAt(i);
                    
                }
            }


        }


        




        public static bool HasContractBlockActiveContract(long blockId)
        {
            for (int i = ActiveContracts.Count - 1; i >= 0; i--)
            {
                var ActiveContract = ActiveContracts[i];

                if (ActiveContract.BlockId == blockId)
                {
                    return true;
                }
            }

            return false;
        }









    }
}
