using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Events;
using ModularEncountersSystems.Events.Condition;
using ModularEncountersSystems.Files;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning.Profiles;
using ProtoBuf;
using Sandbox.Game;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Missions
{



    public class Mission
    {
        public string ProfileSubtypeId;
        public string SpawnGroupName;
        public IMyFaction Faction;

        public string Title;
        public string Description;
        public int Reward;
        public int Collateral;
        public int ReputationReward;
        public int FailReputationPrice;
        public int Duration;

        public List<string> ReplaceKeys;
        public List<string> ReplaceValues;

        public List<EventCondition> Conditions;
        public List<EventCondition> PersistantConditions;
        public List<PlayerCondition> PlayerConditions;
        public List<PlayerCondition> LeadPlayerConditions;

        public BlockEntity SourceContractBlock;
        public long InstanceId;

        private string StoreProfileId;
        private StoreProfile _StoreProfile;
        public string InstanceEventGroupId;

        StoreProfile StoreProfile
        {
            get
            {
                if(_StoreProfile == null)
                {
                    StoreProfile _profile = null;
                    if (ProfileManager.StoreProfiles.TryGetValue(StoreProfileId, out _profile))
                    {
                        _StoreProfile = _profile;
                        return _StoreProfile;
                    }


                    return null;
                }
                else
                {
                    return _StoreProfile;
                }

            }
        }



        public MissionProfile Profile
        {

            get
            {

                if (_profile == null)
                {

                    if (!ProfileManager.MissionProfiles.TryGetValue(ProfileSubtypeId, out _profile))
                    {

                        //ErrorOnSetup = true;
                        //ErrorSetupMsg = "Could Not Find EventProfile With Name [" + ProfileSubtypeId ?? null + "]";
                        return null;

                    }

                }

                return _profile;

            }

        }



        private MissionProfile _profile;



        
        public Mission(string profileSubtypeId, string storeProfileId, string spawnGroupName)
        {
            ProfileSubtypeId = profileSubtypeId;

            if (string.IsNullOrEmpty(storeProfileId))
            {
                StoreProfileId = "MES-StoreProfile-Example";
            }
            else
            {
                StoreProfileId = storeProfileId;
            }
            SpawnGroupName = spawnGroupName;
        }



        public bool Init(BlockEntity sourceContractBlock)
        {
            ReplaceKeys = new List<string>();
            ReplaceValues = new List<string>();

            ReplaceKeys.AddList(Profile.ReplaceKeys);
            ReplaceValues.AddList(Profile.ReplaceValues);

            SourceContractBlock = sourceContractBlock;



            if (!string.IsNullOrEmpty(Profile.OverrideFaction))
            {
                Faction = MyAPIGateway.Session.Factions.TryGetFactionByTag(Profile.OverrideFaction);

                //Override faction tag not found
                if (Faction == null)
                    return false;
            }
            else
            {
                Faction = sourceContractBlock.Faction();
            }

            if (Faction == null)
                return false;   


            var coords = sourceContractBlock.GetPosition();

            ReplaceKeys.Add("{ContractBlockLocation}");
            ReplaceValues.Add($"{{X:{coords.X} Y:{coords.Y} Z:{coords.Z}}}");

            ReplaceKeys.Add("{ContractBlockFaction}");
            ReplaceValues.Add($"{sourceContractBlock.Faction()?.Tag}");

            ReplaceKeys.Add("{Faction}");
            ReplaceValues.Add($"{Faction.Tag}");


            if (Profile.CustomApiMapping.Count > 0)
            {

                foreach (var methodName in Profile.CustomApiMapping)
                {
                    Func<string, string, List<string>, Vector3D, Dictionary<string, string>> func;

                    if (!LocalApi.MissionCustomMappings.TryGetValue(methodName, out func))
                    {
                        return false;
                    }
                    if (func != null)
                    {
                        var dict = func.Invoke(ProfileSubtypeId, SpawnGroupName, Profile.Tags, sourceContractBlock.GetPosition());

                        if (dict == null)
                            return false;

                        ReplaceKeys.AddList(new List<string>(dict.Keys));
                        ReplaceValues.AddList(new List<string>(dict.Values));
                    }
                }
            }

            InstanceEventGroupId = IdsReplacer.ReplaceText(Profile.InstanceEventGroupId, ReplaceKeys, ReplaceValues);

            Title = IdsReplacer.ReplaceText(Profile.Title, ReplaceKeys, ReplaceValues);
            Description = IdsReplacer.ReplaceText(Profile.Description, ReplaceKeys, ReplaceValues);

            var RewardString = IdsReplacer.ReplaceText(Profile.Reward, ReplaceKeys, ReplaceValues);
            var CollateralString = IdsReplacer.ReplaceText(Profile.Collateral, ReplaceKeys, ReplaceValues);
            var ReputationRewardString = IdsReplacer.ReplaceText(Profile.ReputationReward, ReplaceKeys, ReplaceValues);
            var FailReputationPriceString = IdsReplacer.ReplaceText(Profile.FailReputationPrice, ReplaceKeys, ReplaceValues);
            var DurationString = IdsReplacer.ReplaceText(Profile.Duration, ReplaceKeys, ReplaceValues);

            // Initialize default values
            Reward = 0;
            Collateral = 0;
            ReputationReward = 0;
            FailReputationPrice = 0;
            Duration = 0;

            // Convert strings to integers
            if (!int.TryParse(RewardString, out Reward))
            {
                Reward = 0; 
            }

            if (!int.TryParse(CollateralString, out Collateral))
            {
                Collateral = 0; 
            }

            if (!int.TryParse(ReputationRewardString, out ReputationReward))
            {

                ReputationReward = 0; 
            }

            if (!int.TryParse(FailReputationPriceString, out FailReputationPrice))
            {
                FailReputationPrice = 0; 
            }

            if (!int.TryParse(DurationString, out Duration))
            {
                Duration = 0;
            }


            LeadPlayerConditions = new List<PlayerCondition>();
            PlayerConditions = new List<PlayerCondition>();

            PersistantConditions = new List<EventCondition>();
            Conditions = new List<EventCondition>();



            //Get the profiles:
            foreach (var id in Profile.LeadPlayerConditionIds)
            {
                PlayerCondition conditionProfile = null;

                if (ProfileManager.PlayerConditions.TryGetValue(id, out conditionProfile))
                {
                    LeadPlayerConditions.Add(conditionProfile);
                }
            }

            foreach (var id in Profile.PlayerConditionIds)
            {
                PlayerCondition conditionProfile = null;

                if (ProfileManager.PlayerConditions.TryGetValue(id, out conditionProfile))
                {
                    PlayerConditions.Add(conditionProfile);
                }
            }


            foreach (var id in Profile.PersistantEventConditionIds)
            {
                EventCondition conditionProfile = null;

                if (ProfileManager.EventConditions.TryGetValue(id, out conditionProfile))
                {
                    PersistantConditions.Add(conditionProfile);
                }
            }

            foreach (var id in Profile.PlayerConditionIds)
            {
                EventCondition conditionProfile = null;

                if (ProfileManager.EventConditions.TryGetValue(id, out conditionProfile))
                {
                    Conditions.Add(conditionProfile);
                }
            }


            if (!RunEventConditions())
                return false;


            return AddMissionToBlock();
        }


        public bool RunEventConditions()
        {

            if (!EventCondition.AreConditionsMet(false, this.PersistantConditions))
            {
                MyVisualScriptLogicProvider.ShowNotificationToAll("Conditions not Met", 20000, "Red");
                return false;
            }



            if (!EventCondition.AreConditionsMet(Profile.UseAnyPassingEventCondition, this.Conditions))
            {
                MyVisualScriptLogicProvider.ShowNotificationToAll("Conditions not Met", 20000, "Red");
                return false;
            }

            return true;

        }

        public bool RunPlayerCondition(long PlayerId, bool IsLeadPlayer = true)
        {
            if (IsLeadPlayer)
            {
                if (!PlayerCondition.ArePlayerConditionsMet(this.LeadPlayerConditions, PlayerId))
                {
                    return false;
                }
            }

            if (!PlayerCondition.ArePlayerConditionsMet(this.PlayerConditions, PlayerId))
            {
                return false;
            }

            return true;
        }






        public void Start()
        {


            TemplateEventGroup tja = null;

            if (!ProfileManager.TemplateEventGroup.TryGetValue(InstanceEventGroupId, out tja))
            {
                return;
            }

            tja.AddEventsAsInsertible(ReplaceKeys, ReplaceValues, InstanceId);

        }

        public bool AddMissionToBlock()
        {

            if (SourceContractBlock == null)
                return false;




            MyAddContractResultWrapper result;
            IMyPlayerCollection playerCollection;

            switch (Profile.MissionType)
            {
                case MissionType.Custom:
                    MyDefinitionId definitionId;
                    bool parsed = MyDefinitionId.TryParse("MyObjectBuilder_ContractTypeDefinition/MESContract", out definitionId);

                    if (!parsed) return false;

                    playerCollection = MyAPIGateway.Players;
                    playerCollection.RequestChangeBalance(SourceContractBlock.Block.OwnerId, Reward);

                    MyContractCustom newContract = new MyContractCustom(definitionId: definitionId,
                        startBlockId: SourceContractBlock.Entity.EntityId,
                        moneyReward: Reward,
                        collateral: Collateral,
                        duration: Duration,
                        name: Title,
                        description: Description,
                        reputationReward: ReputationReward,
                        failReputationPrice: FailReputationPrice,
                        endBlockId: null);

                    Contract contractObject = new Contract();

                    contractObject.MissionReference = this;
                    contractObject.SourceBlock = SourceContractBlock;


                    result = MyAPIGateway.ContractSystem.AddContract(newContract);

                    if (result.Success)
                    {
                        // set contract values
                        contractObject.ContractId = result.ContractId;
                        InGameContractManager.GeneratedContracts.Add(contractObject);
                        InGameContractManager.ContractsForRemoval.Add(result.ContractId);

                        InstanceId = result.ContractId;

                        ReplaceKeys.Add("{InstanceId}");
                        ReplaceValues.Add($"{InstanceId}");


                        //playerCollection.RequestChangeBalance(SourceContractBlock.Block.OwnerId, -Reward); Keen already does this?
                        return true;
                    }

                    break;

                case MissionType.Acquisition:

                    if (StoreProfile == null)
                        return false;


                    int index = MathTools.RandomBetween(0, StoreProfile.Orders.Count); ;


                    if (index >= StoreProfile.OrderItems.Count)
                    {
                        // Handle case where index is out of range
                        MyVisualScriptLogicProvider.ShowNotificationToAll($"Acquisition failed {StoreProfile.ProfileSubtypeId}", 5000);
                        MyVisualScriptLogicProvider.ShowNotificationToAll($"Index: {index}, Count: {StoreProfile.OrderItems.Count}", 5000);
                        return false;
                    }
                    var storeItem = StoreProfile.OrderItems[index];
                    MyVisualScriptLogicProvider.ShowNotificationToAll($"{index}: {storeItem.ItemSubtypeId}", 5000);

                    int amount = storeItem.GetAmount(false);
                    int reward = (int)((int)storeItem.GetPrice(105, false) * amount);


                    playerCollection = MyAPIGateway.Players;
                    playerCollection.RequestChangeBalance(SourceContractBlock.Block.OwnerId, reward);



                    MyContractAcquisition newAcquisition = new MyContractAcquisition(startBlockId: SourceContractBlock.Entity.EntityId,
                        moneyReward: reward, //(int)storeItem.GetPrice(200,false)
                        collateral: 0,
                        duration: Duration,
                        endBlockId: SourceContractBlock.Entity.EntityId,
                        itemTypeId: storeItem.GetItemId(),
                        itemAmount: amount
                        );

                    Contract contractAcquisitionObject = new Contract();
                    contractAcquisitionObject.MissionReference = this;
                    contractAcquisitionObject.SourceBlock = SourceContractBlock;
                    result = MyAPIGateway.ContractSystem.AddContract(newAcquisition);

                    if (result.Success)
                    {
                        // set contract values
                        contractAcquisitionObject.ContractId = result.ContractId;
                        InGameContractManager.ContractsForRemoval.Add(result.ContractId);
                        InGameContractManager.GeneratedContracts.Add(contractAcquisitionObject);

                        InstanceId = result.ContractId;


                        //playerCollection.RequestChangeBalance(SourceContractBlock.Block.OwnerId, -reward); Not needed because keen already does this?
                        return true;
                    }
                    else
                    {
                        MyVisualScriptLogicProvider.ShowNotificationToAll($"{result.Success}? factions have too little money for contracts", 5000);
                    }

                    break;

                default:

                    break;
            }
            

            return false;

        }

        public bool ReAddContractToBlock(long previousId)
        {
            if(InstanceId == previousId)
            {

                ReplaceKeys.Remove("{InstanceId}");
                ReplaceValues.Remove($"{InstanceId}");
                InstanceId = 0;

                return AddMissionToBlock();
            }
            else
            {
                return false;
            }

        }




}

}



             