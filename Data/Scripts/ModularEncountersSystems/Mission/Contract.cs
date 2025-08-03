﻿using ModularEncountersSystems.Configuration;
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
using ProtoBuf;

namespace ModularEncountersSystems.Missions
{
    //

    [ProtoContract]
    public class ActiveContract
    {
        [ProtoMember(1)]
        public long ContractId;

        [ProtoMember(2)]
        public long BlockId;

        [ProtoMember(3)]
        public int Reward;
        [ProtoMember(4)]
        public int ReputationReward;
        [ProtoMember(5)]
        public int FailReputationPrice;

        [ProtoMember(6)]
        public string FactionTag;

        [ProtoMember(7)]
        public string ActivateBooleanNameOnSucces;

        [ProtoMember(7)]
        public string MissionProfileSubtypeId;

        

        public ActiveContract()
        {

        }

        public ActiveContract(long ContractId,long BlockId, int Reward, int ReputationReward, int FailReputationPrice, string FactionTag, string ActivateBooleanNameOnSucces, string profileSubtypeId)
        {
            this.ContractId = ContractId;
            this.BlockId = BlockId;
 
            this.Reward = Reward;
            this.ReputationReward = ReputationReward;
            this.FailReputationPrice = FailReputationPrice;
            this.FactionTag = FactionTag;
            this.ActivateBooleanNameOnSucces = ActivateBooleanNameOnSucces;
            this.MissionProfileSubtypeId = profileSubtypeId;
        }
    }







    public class Contract
    {

        public long ContractId;
        public BlockEntity SourceBlock;
        public Mission MissionReference;

        public bool TryToActivateCustomContract(long playerIdentityId)
        {

            if (!MissionReference.RunPlayerCondition(playerIdentityId))
            {
                MyVisualScriptLogicProvider.SendChatMessageColored("You do not meet the requirements for this contract.", Color.Olive, "Contracts", playerIdentityId);
                return false;
            }
                
            Vector3D position = SourceBlock.GetPosition();

            var Players = new List<PlayerEntity>();

            var LeadPlayer = PlayerManager.GetPlayerWithIdentityId(playerIdentityId);
            Players.Add(LeadPlayer);

            var acceptingPlayerFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(playerIdentityId);

            if (!MissionReference.Profile.SoloMission && acceptingPlayerFaction != null)
            {

                List<long> localPlayerIdList;
                PlayerManager.PlayersNearby(position, 400, out localPlayerIdList);

                foreach (var playerId in localPlayerIdList)
                {
                    if (playerId == playerIdentityId)
                        continue;


                    var localFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(playerId);

                    if (localFaction == null)
                        continue;

                    if (localFaction.Equals(acceptingPlayerFaction))
                    {
                        var player = PlayerManager.GetPlayerWithIdentityId(playerId);

                        if (!MissionReference.RunPlayerCondition(playerId, false))
                        {
                            
                            MyVisualScriptLogicProvider.SendChatMessageColored($"{player?.Name() ?? "Someone in your faction"} does not meet the requirements for this contract", Color.Olive, "Contracts", playerIdentityId);
                            return false;
                        }

                        Players.Add(player);
                    }
                }

            }


            LeadPlayer.ProgressionData.Tags.Add($"LeadPlayer@{ContractId}");

            foreach (var player in Players)
            {
                player.ProgressionData.Tags.Add($"@{ContractId}");
            }

            // Remove all other contracts with this type contract blocks
            if (this.MissionReference.Profile.Exclusive)
            {
                InGameContractManager.PurgeContractsWithMissionSubtypeId(MissionReference.ProfileSubtypeId,ContractId);
            }

            var FactionTag = MissionReference.Faction.Tag;


            var _activeContract = new ActiveContract(ContractId,
                SourceBlock.Entity.EntityId, MissionReference.Reward,
                MissionReference.ReputationReward, MissionReference.FailReputationPrice,
                FactionTag,MissionReference.Profile.ActivateBooleanNameOnSucces,
                MissionReference.ProfileSubtypeId);

            InGameContractManager.ActiveContracts.Add(_activeContract);
            MissionReference.Start();


            return true;
        }



    }
}

             