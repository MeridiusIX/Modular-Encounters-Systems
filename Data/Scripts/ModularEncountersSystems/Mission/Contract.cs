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

namespace ModularEncountersSystems.Missions
{

    public class Contract
    {
        public long ContractId;
        public BlockEntity SourceBlock;
        public Mission MissionReference;

        public void OnContractAcquired(long identityId)
        {
            AcquireContractInternal(identityId, SourceBlock.GetPosition());
        }


        private bool PlayerHasMission(long playerId)
        {
            foreach (var mission in MissionManager.ActiveMissionList)
            {
                if (mission.PlayerIds.Contains(playerId))
                {
                    return true;
                }
            }
            return false;
        }


        public void AcquireContractInternal(long identityId, Vector3D position)
        {
            // Function to check if player already has a mission


            // Check if player already has a mission or not
            if (!PlayerHasMission(identityId))
            {
                var acceptingPlayerFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(identityId);

                MissionReference.PlayerIds.Add(identityId);

                List<long> locPlayerIdList;
                MissionManager.PlayersNearby(position, 100, out locPlayerIdList);
                foreach (var playerId in locPlayerIdList)
                {
                    var locFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(playerId);
                    if (locFaction == acceptingPlayerFaction && playerId != identityId)
                    {
                        // Add all same faction players who don't have a mission open
                        if (!PlayerHasMission(playerId))
                        {
                            MissionReference.PlayerIds.Add(playerId);
                        }
                    }
                }

                 /*
                var validRep = true;


                foreach (var player in MissionReference.PlayerId)
                {
                    var reputation = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(player, MissionReference.Faction.FactionId);
                    if (reputation < -500)
                    {
                        validRep = false;
                        break;
                    }
                }

                if (validRep)
                {
                    // Code to execute if the reputation is valid
                }
                else
                {
                    MyVisualScriptLogicProvider.ShowNotificationToAll("Accept missions with neutral or friendly factions", 5000, "Red");
                    MissionReference.Canceled = true;
                }
                */
            }
            else
            {
                MyVisualScriptLogicProvider.ShowNotificationToAll("You have already accepted one mission", 5000, "Red");
                MissionReference.Canceled = true;
            }




            // Remove mission from all contract blocks

            if (this.MissionReference.Exclusive)
            {
                MissionManager.PurgeContractsWithMissionSubtypeId(MissionReference.ProfileSubtypeId);
            }
            else
            {
                MissionManager.PurgeContract(this.ContractId);
            }






            MissionReference.Start();

            MissionManager.GeneratedContracts.Remove(this);



        }


        public void OnContractEnded()
        {
            // nothing
            MissionManager.PurgeContractsWithMissionSubtypeId(MissionReference.ProfileSubtypeId);
        }
    }
}

             