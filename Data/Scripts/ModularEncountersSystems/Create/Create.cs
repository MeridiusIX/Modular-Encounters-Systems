using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Sync;
using ModularEncountersSystems.World;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;

namespace ModularEncountersSystems.Create
{

    public class CreateMaker
    {

        public static string CreateEventArea(ChatMessage msg, string args)
        {
            // StringBuilder
            var sb = new StringBuilder();
            var array = args.Trim().Split('#');


            string VectorCoords = "{" + msg.PlayerPosition.ToString()+ "}";

            string Name = "DefaultName";
            string distance = "DefaultDistance";
            bool unique = false;
            // Check if array has expected length and assign values accordingly
            if (array.Length >= 2)
            {
                Name = array[0];
                distance = array[1];
                MyVisualScriptLogicProvider.AddGPSObjectiveForAll($"{Name} {distance}m", $"You can remove this GPS. It is just to help you.", msg.PlayerPosition, VRageMath.Color.Beige);
            }
            else
            {
                return sb.Append("Missing Arguments").ToString();
            }

           // Your XML string with placeholders replaced
            sb.Append($@"
    <!-- Persistant-->
    <EntityComponent xsi:type=""MyObjectBuilder_InventoryComponentDefinition"">
        <Id>
            <TypeId>Inventory</TypeId>
            <SubtypeId>PlayerCondition-Area-{Name}</SubtypeId>
        </Id>
        <Description>
            [MES Player Condition]

            [CheckPlayerNear:true]    
            [PlayerNearCoords:{VectorCoords}]
            [PlayerNearDistanceFromCoords:{distance}]

            [CheckPlayerTags:true]
            [ExcludedPlayerTag:Player_Triggered-Area-{Name}]

            [CheckPlayerReputation:false]
            [CheckReputationwithFaction:SPRT]
            [MinPlayerReputation:500]
            [MaxPlayerReputation:1500]
        </Description>
    </EntityComponent>

    <EntityComponent xsi:type=""MyObjectBuilder_InventoryComponentDefinition"">
        <Id>
            <TypeId>Inventory</TypeId>
            <SubtypeId>PlayerCondition-Area-{Name}-ConsiderTriggered</SubtypeId>
        </Id>
        <Description>
            [MES Player Condition]
            [CheckPlayerNear:true]    
            [PlayerNearCoords:{VectorCoords}]
            [PlayerNearDistanceFromCoords:4500]
        </Description>
    </EntityComponent>

    <EntityComponent xsi:type=""MyObjectBuilder_InventoryComponentDefinition"">
        <Id>
            <TypeId>Inventory</TypeId>
            <SubtypeId>MES-EventPersistantCondition-Area-{Name}</SubtypeId>
        </Id>
        <Description>
            [MES Event Condition]
            [CheckPlayerCondition:true]
            [PlayerConditionIds:PlayerCondition-Area-{Name}]     
        </Description>   
    </EntityComponent>


    <EntityComponent xsi:type=""MyObjectBuilder_InventoryComponentDefinition"">
        <Id>
            <TypeId>Inventory</TypeId>
            <SubtypeId>MES-Event-Area-{Name}</SubtypeId>
        </Id>
        <Description>
            [MES Event]
            [UseEvent:true]
            [Tags:EventArea{Name}]
            [UniqueEvent:false]    
            [MinCooldownMs:600000]
            [MaxCooldownMs:600001]
            [PersistantConditionIds:MES-EventPersistantCondition-Area-{Name}]


            [UseAnyPassingCondition:true]
            [ActionExecution:Condition]

            [ConditionIds:MES-EventCondition-Area-{Name}-A]
            [ActionIds:MES-EventAction-Area-{Name}-A]



        </Description>
    </EntityComponent>

    <!-- Option A -->

    <EntityComponent xsi:type=""MyObjectBuilder_InventoryComponentDefinition"">
        <Id>
            <TypeId>Inventory</TypeId>
            <SubtypeId>MES-EventCondition-Area-{Name}-A</SubtypeId>
        </Id>
        <Description>
            [MES Event Condition]
   
            [CheckPlayerCondition:true]
            [PlayerConditionIds:PlayerCondition-Area-{Name}-A]    
        </Description>   
    </EntityComponent>

    <EntityComponent xsi:type=""MyObjectBuilder_InventoryComponentDefinition"">
        <Id>
            <TypeId>Inventory</TypeId>
            <SubtypeId>PlayerCondition-Area-{Name}-A</SubtypeId>
        </Id>
        <Description>
            [MES Player Condition]

            [CheckPlayerReputation:false]
            [CheckReputationwithFaction:SPRT]
            [MinPlayerReputation:500]
            [MaxPlayerReputation:1500]

            [CheckPlayerTags:false]
            [IncludedPlayerTag:]
            [ExcludedPlayerTag:]
        </Description>
    </EntityComponent>


    <!-- Option A -->
    <EntityComponent xsi:type=""MyObjectBuilder_InventoryComponentDefinition"">
        <Id>
            <TypeId>Inventory</TypeId>
            <SubtypeId>MES-EventAction-Area-{Name}-A</SubtypeId>
        </Id>
        <Description>
            [MES Event Action]

            [AddTagstoPlayers:true]
            [AddTagsPlayerConditionIds:PlayerCondition-Area-{Name}-ConsiderTriggered]
            [AddTags:Player_Triggered-Area-{Name}]

            [UseChatBroadcast:true]
            [ChatData:MES-EventChat-Area-{Name}-A]

        </Description>
    </EntityComponent>

    <EntityComponent xsi:type=""MyObjectBuilder_InventoryComponentDefinition"">
        <Id>
            <TypeId>Inventory</TypeId>
            <SubtypeId>MES-EventChat-Area-{Name}-A</SubtypeId>
        </Id> 
        <Description>
            [RivalAI Chat]
            [UseChat:true]
            [StartsReady:true]
            [Chance:100]
            [MaxChats:-1]
            [BroadcastRandomly:true]
            [IgnoreAntennaRequirement:True]
            [IgnoredAntennaRangeOverride:1]
            [SendToAllOnlinePlayers:false]
            [SendToSpecificPlayers:true]
            [PlayerConditionIds:PlayerCondition-Area-{Name}-ConsiderTriggered]
            [Color:Green]
            [Author:MES]
            [ChatMessages:{Name} A triggered]
            [BroadcastChatType:Chat]
            [ChatAudio:]  
        </Description>
    </EntityComponent>");
            


        return sb.ToString();
        }
    }
}
