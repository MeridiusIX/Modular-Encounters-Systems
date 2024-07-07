using ModularEncountersSystems.API;
using ModularEncountersSystems.Entities;
using ProtoBuf;
using Sandbox.Game;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Contracts;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;

namespace ModularEncountersSystems.Missions
{

    [ProtoContract]
    public class Mission
    {
        //SAVE
        [ProtoMember(1)] public string ProfileSubtypeId;
        [ProtoMember(2)] public List<long> PlayerIds = new List<long>();
        [ProtoMember(3)] public StringBuilder CurrentObjective;
        [ProtoMember(4)] public IMyFaction Faction;
        [ProtoMember(5)] public string Title;
        [ProtoMember(6)] public string Description;
        [ProtoMember(7)] public bool Unique;
        [ProtoMember(8)] public bool Exclusive;

        [ProtoMember(9)] public bool Active;


        [ProtoIgnore] public bool ErrorOnSetup;
        [ProtoIgnore] public string ErrorSetupMsg;
        [ProtoIgnore] public bool Canceled;
        [ProtoIgnore] private string tja;

        [ProtoIgnore] private List<string> ReplaceKeys;
        [ProtoIgnore] private List<string> ReplaceValues;

        [ProtoIgnore] private string InstanceEventGroupId;



        [ProtoIgnore]
        public bool Valid
        {

            get
            {

                if (ErrorOnSetup)
                    return false;

                return true;

            }

        }



        public Mission()
        {



        }

        public Mission(string profileSubtypeId)
        {


        }

        public Mission(MissionProfile profile)
        {
            ProfileSubtypeId = profile.ProfileSubtypeId;
            Title = profile.Title;
            Description = profile.Description;

            ReplaceKeys = profile.ReplaceKeys;
            ReplaceValues = profile.ReplaceValues;
            InstanceEventGroupId = profile.InstanceEventGroupId;
            Unique = profile.Unique;
            Exclusive = profile.Exclusive;
        }

        public void Start()
        {
            Active = true;
            MissionManager.ActiveMissionList.Add(this);
            MyVisualScriptLogicProvider.ShowNotificationToAll("Mission Start",2000,"Green");
            LocalApi.AddInstanceEventGroup(InstanceEventGroupId, ReplaceKeys, ReplaceValues);
        }

        public bool AddMissionToBlock(BlockEntity sourceContractBlock)
        {
            if (sourceContractBlock == null)
                return false;

            IMyFaction contractBlockFaction =
                MyAPIGateway.Session.Factions.TryGetFactionByTag(sourceContractBlock.FactionOwner());
            if (contractBlockFaction == null)
                return false;

            MyDefinitionId definitionId;
            bool parsed = MyDefinitionId.TryParse("MyObjectBuilder_ContractTypeDefinition/MESContract", out definitionId);

            if (!parsed) return false;

            MyVisualScriptLogicProvider.ShowNotificationToAll(sourceContractBlock.Entity.EntityId.ToString(), 5000, "White");
            MyContractCustom newContract = new MyContractCustom(definitionId: definitionId,
                startBlockId: sourceContractBlock.Entity.EntityId,
                moneyReward: 0,
                collateral: 0,
                duration: 1200,
                name: Title,
                description: Description,
                reputationReward: 0,
                failReputationPrice: 0,
                endBlockId: sourceContractBlock.Entity.EntityId);

            Contract contractObject = new Contract();

            newContract.OnContractAcquired += contractObject.OnContractAcquired;
            newContract.OnContractFailed += contractObject.OnContractEnded;
            newContract.OnContractSucceeded += contractObject.OnContractEnded;
            contractObject.MissionReference = this;
            contractObject.SourceBlock = sourceContractBlock;

            //  This adds a contract to the contract system.
            //  This makes it available in the contract block for the player to see and accept.
            MyAddContractResultWrapper result = MyAPIGateway.ContractSystem.AddContract(newContract);


            if (result.Success)
            {
                // set contract values
                contractObject.ContractId = result.ContractId;
                MissionManager.GeneratedContracts.Add(contractObject);
                return true;
            }
            MyVisualScriptLogicProvider.ShowNotificationToAll("Fail to add contract to a block", 2000, "Red");
            return false;

        }



    }

}



             