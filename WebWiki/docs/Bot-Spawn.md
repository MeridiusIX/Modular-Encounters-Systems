#Bot-Spawn.md

Bot Spawn Profiles in Modular Encounters Systems allow you to specify a set of rules for how a bot is configured before it is spawned by the base game or the AiEnabled mod.

Here's an example of how a Bot Spawn Profile Definition is setup:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>MES-ExampleBotSpawnProfile</SubtypeId>
      </Id>
      <Description>
        [MES Bot Spawn]
        
        [BotType:Police_Bot]
        [BotDisplayName:Combat Bot]      
      </Description>      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

Below are all of the eligible tags you can use in your Bot Spawn Profiles:

**[Shared](#shared)**  
**[Vanilla](#vanilla)**  
**[AiEnabled](#aienabled)**  

# Shared
This section contains tags that are relevant for both Vanilla and AiEnabled bots.

<!--BotType-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BotType|
|:----|:----|
|Tag Format:|`[BotType:Value]`|
|Description:|This tag specifies the type of bot that will be spawned.|
|Allowed Values:|Vanilla: Any valid `Type/Subtype` combination (include `MyObjectBuilder_` for types!)<br>AiEnabled: Any Character type (the `Name` field in the SBC, not the SubtypeId)|
|Multiple Tag Allowed:|No|

<!--BotDisplayName-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BotDisplayName|
|:----|:----|
|Tag Format:|`[BotDisplayName:Value]`|
|Description:|This tag specifies the Display Name of the spawned bot.|
|Allowed Values:|Any Name|
|Multiple Tag Allowed:|No|

<!--BotFactionTag-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BotFactionTag|
|:----|:----|
|Tag Format:|`[BotFactionTag:Value]`|
|Description:|This tag specifies faction of the bot.|
|Allowed Values:|Any faction tag.|
|Multiple Tag Allowed:|No|

<!--Color-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Color|
|:----|:----|
|Tag Format:|`[Color:Value]`|
|Description:|Specifies the Color (RGB 0-255) of the spawned bot character.|
|Allowed Values:|Any Vector3 Value using this format<br />`{X:0 Y:0 Z:0}`|
|Multiple Tag Allowed:|No|

***

# Vanilla

This section contains tags that only affect vanilla bot spawning.

<!--Tags-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Tags|
|:----|:----|
|Tag Format:|`[Tags:Value]`|
|Description:|This tag specifies the tags to associate with the vanilla bots that are spawned with this Bot Spawner Profile. They allow you select which vanilla bots are targeted by actions affecting bots, such as `[RemoveBotsFromGrid:true]` or `[EjectBotsFromSeats:true]`. Note that actions can specify additional tags for bots to spawn with, using `[BotTags:Value]`.|
|Allowed Values:|Any `string` excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--WeaponsContainerId-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|WeaponsContainerId|
|:----|:----|
|Tag Format:|`[WeaponsContainerId:Value]`|
|Description:|This tag specifies the SubtypeId of the ContainerType used to assign weapons to the bot. It corresponds to `NPCWeaponsContainerType` in the vanilla SpawnGroup SBC.|
|Allowed Values:|Any ContainerType SubtypeId|
|Default Value(s):|The `<StartingItem>` loadout in the bot definition is used.|
|Multiple Tag Allowed:|No|

<!--LootContainerId-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LootContainerId|
|:----|:----|
|Tag Format:|`[LootContainerId:Value]`|
|Description:|This tag specifies the SubtypeId of the ContainerType used to assign loot to the bot's backpack.|
|Allowed Values:|Any ContainerType SubtypeId|
|Multiple Tag Allowed:|No|

<!--ChatMessageSetId-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChatMessageSetId|
|:----|:----|
|Tag Format:|`[ChatMessageSetId:Value]`|
|Description:|This tag specifies the SubtypeId of the NPCChatMessageSet used to assign weapons to the bot. It corresponds to `NPCChatMessageSet` in the vanilla SpawnGroup SBC.|
|Allowed Values:|Any ContainerType SubtypeId|
|Multiple Tag Allowed:|No|

<!--SpawnBlocks-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SpawnBlocks|
|:----|:----|
|Tag Format:|`[SpawnBlocks:Value]`|
|Description:|SpawnBlocks are the blocks in which vanilla blocks are spawned into. As such, an armor block would not be suitable, but a Cockpit or a Passage-type block would work. If bots should spawn seated in a cockpit listed here, be sure to also set `[SpawnSeated:true]`. Bots will pick a random block from this list to spawn in - provided it is not already occupied. Note that an actions' `[BotSpawnBlockOverrides:Value]` will replace any SpawnBlocks defined in this profile.|
|Allowed Values:|Any valid `Type/Subtype` combination (include `MyObjectBuilder_` for types!)|
|Multiple Tag Allowed:|Yes|

<!-- SpawnSeated -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SpawnSeated|
|:----|:----|
|Tag Format:|`[SpawnSeated:Value]`|
|Description:|This tag determines if bots with this profile should be spawned seated on the defined `[SpawnBlocks:Value]` If no or not enough such SpawnBlocks have been defined in either profile or action (via `[BotSpawnBlockOverrides:Value]`), or if they are already occupied, no further bots will be spawned.|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!--AssetOverrideSets-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AssetOverrideSets|
|:----|:----|
|Tag Format:|`[AssetOverrideSets:Value]`|
|Description:|This tag specifies a list of suit skins, which should be picked from randomly and applied to bots when spawned.|
|Allowed Values:|`FactionDefault`<br/>`Assault`<br/>`Aviator`<br/>`Badger`<br/>`Carpet`<br/>`Chef`<br/>`Cow`<br/>`Crocodile`<br/>`DigitalCamouflage`<br/>`Disco`<br/>`EmissiveStripes`<br/>`Fiber`<br/>`Frozen`<br/>`Ghillie`<br/>`Glamour`<br/>`Golden`<br/>`Graffiti`<br/>`Hazmat`<br/>`Halloween`<br/>`Hexagon`<br/>`KeenPajamas`<br/>`KidsPajamas`<br/>`Lava`<br/>`Leopard`<br/>`Medieval`<br/>`Metallic`<br/>`Miner`<br/>`Mummy`<br/>`Newspaper`<br/>`NextGen`<br/>`PartyTux`<br/>`Pharaoh`<br/>`Policeman`<br/>`Prisoner`<br/>`Proper`<br/>`Racing`<br/>`Rainbow`<br/>`Retro`<br/>`Robot`<br/>`Scavenger`<br/>`Shark`<br/>`Skeleton`<br/>`Slime`<br/>`Terracotta`<br/>`Soldier`<br/>`Tuxedo`<br/>`Veteran`<br/>`VeteranMarkII`<br/>`Wood`<br/>`Zebra`<br/>`Zombie`|
|Multiple Tag Allowed:|Yes|

***

# AiEnabled

This section contains tags that are only relevant for AiEnabled bot spawning.

<!-- UseAiEnabled -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseAiEnabled|
|:----|:----|
|Tag Format:|`[UseAiEnabled:Value]`|
|Description:|This tag determines if bots with this profile should be spawned with AiEnabled or not.|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!--BotBehavior-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BotBehavior|
|:----|:----|
|Tag Format:|`[BotBehavior:Value]`|
|Description:|This tag specifies the behavior that a spawned AiEnabled bot will use, it is not relevant for vanilla bots. If this tag is not defined, then it will use the default behavior that AiEnabled would use when spawning a bot of the selected type.|
|Allowed Values:|Any AiEnabled Behavior Role|
|Multiple Tag Allowed:|No|

<!-- CanUseAirNodes -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CanUseAirNodes|
|:----|:----|
|Tag Format:|`[CanUseAirNodes:Value]`|
|Description:|This tag allows you to specify if the AiEnabled bot is able to use Air Nodes (ie Jetpack). Not relevant for vanilla bots.|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- CanUseSpaceNodes -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CanUseSpaceNodes|
|:----|:----|
|Tag Format:|`[CanUseSpaceNodes:Value]`|
|Description:|This tag allows you to specify if the AiEnabled bot is able to use Space Nodes (ie Jetpack). Not relevant for vanilla bots.|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- UseGroundNodesFirst -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseGroundNodesFirst|
|:----|:----|
|Tag Format:|`[UseGroundNodesFirst:Value]`|
|Description:|This tag allows you to specify if the AiEnabled bot should try to use ground nodes before using air / space nodes. Not relevant for vanilla bots.|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- CanUseWaterNodes -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CanUseWaterNodes|
|:----|:----|
|Tag Format:|`[CanUseWaterNodes:Value]`|
|Description:|This tag allows you to specify if the AiEnabled bot is able to use water nodes (ie underwater via Water Mod). Not relevant for vanilla bots.|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- WaterNodesOnly -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|WaterNodesOnly|
|:----|:----|
|Tag Format:|`[WaterNodesOnly:Value]`|
|Description:|This tag allows you to specify if the AiEnabled bot should only use water nodes (ie underwater only bot). Not relevant for vanilla bots.|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- CanUseLadders -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CanUseLadders|
|:----|:----|
|Tag Format:|`[CanUseLadders:Value]`|
|Description:|This tag allows you to specify if the AiEnabled bot is able to climb ladders. Not relevant for vanilla bots.|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- CanUseSeats -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CanUseSeats|
|:----|:----|
|Tag Format:|`[CanUseSeats:Value]`|
|Description:|This tag allows you to specify if the AiEnabeld bot is able to sit on seating blocks. Not relevant for vanilla bots.|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- DespawnTicks -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DespawnTicks|
|:----|:----|
|Tag Format:|`[DespawnTicks:Value]`|
|Description:|This tag allows you to specify if the AiEnabled bot should despawn after a set number of game ticks. If value is `0`, then no timer is used. Not relevant for vanilla bots.|
|Allowed Value(s):|Any Integer Greater/Equal To `0`|
|Default Value(s):|`0`|
|Multiple Tags Allowed:|No|

<!-- DeathSound -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DeathSound|
|:----|:----|
|Tag Format:|`[DeathSound:Value]`|
|Description:|This tag allows you to specify a custom sound effect that is used when the AiEnabled bot dies. Not relevant for vanilla bots.|
|Allowed Value(s):|Any Audio SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|No|

<!-- AttackSounds -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AttackSounds|
|:----|:----|
|Tag Format:|`[AttackSounds:Value]`|
|Description:|This tag allows you to specify one or more custom sound effects that are used when the AiEnabled bot attacks. Not relevant for vanilla bots.|
|Allowed Value(s):|Any Audio SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|Yes|

<!-- PainSounds -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PainSounds|
|:----|:----|
|Tag Format:|`[PainSounds:Value]`|
|Description:|This tag allows you to specify one or more custom sound effects that are used when the AiEnabled bot takes damage. Not relevant for vanilla bots.|
|Allowed Value(s):|Any Audio SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|Yes|

<!-- IdleSounds -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IdleSounds|
|:----|:----|
|Tag Format:|`[IdleSounds:Value]`|
|Description:|This tag allows you to specify one or more custom sound effects that are used when the AiEnabled bot is idle. Not relevant for vanilla bots.|
|Allowed Value(s):|Any Audio SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|Yes|

<!-- TauntSounds -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TauntSounds|
|:----|:----|
|Tag Format:|`[TauntSounds:Value]`|
|Description:|This tag allows you to specify one or more custom sound effects that are used when the AiEnabled bot is chasing a target. Not relevant for vanilla bots.|
|Allowed Value(s):|Any Audio SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|Yes|

<!-- EmoteActions -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EmoteActions|
|:----|:----|
|Tag Format:|`[EmoteActions:Value]`|
|Description:|This tag allows you to specify one or more emote that are used by nomad AiEnabled bots randomly. Not relevant for vanilla bots.|
|Allowed Value(s):|Any Emote SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|Yes|

<!-- ShotDeviationAngle -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ShotDeviationAngle|
|:----|:----|
|Tag Format:|`[ShotDeviationAngle:Value]`|
|Description:|This tag allows you to specify the angle of deviation that is applied to gunfire from this AiEnabled bot. Not relevant for vanilla bots.|
|Allowed Value(s):|Any Number Greater/Equal To `0'|
|Default Value(s):|`1.5`|
|Multiple Tags Allowed:|No|

<!-- LeadTargets -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LeadTargets|
|:----|:----|
|Tag Format:|`[LeadTargets:Value]`|
|Description:|This tag allows you to specify if AiEnabled bots should try to lead their target while shooting at them. Not relevant for vanilla bots.|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- ToolSubtypeId -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ToolSubtypeId|
|:----|:----|
|Tag Format:|`[ToolSubtypeId:Value]`|
|Description:|This tag allows you to specify a custom tool/weapon for the AiEnabled bot to use. Not relevant for vanilla bots.|
|Allowed Value(s):|Any Vanilla Tool SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|No|

<!-- CanDamageGrids -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CanDamageGrids|
|:----|:----|
|Tag Format:|`[CanDamageGrids:Value]`|
|Description:|This tag allows you to specify if the AiEnabled bot is able to damage grids (ie target blocks). Not relevant for vanilla bots.|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- TargetPriorities -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TargetPriorities|
|:----|:----|
|Tag Format:|`[TargetPriorities:Value]`|
|Description:|This tag allows you to specify one or more target priorities that the AiEnabled bot will use when attacking. Add them in priority order. Remove entries to have the bot ignore them completely (requires at least one entry for this to take effect). Not relevant for vanilla bots.|
|Allowed Value(s):|IMyCharacter<br />IMyUserControllableGun<br />IMyShipController<br />IMyPowerProducer<br />IMyThrust<br />IMyGyro<br />IMyProductionBlock<br />IMyDoor<br />IMyProgrammableBlock<br />IMyProjector<br />IMyConveyor<br />IMyCargoContainer<br />IMyFunctionalBlock<br />IMyTerminalBlock<br />IMyCubeBlock<br />IMySlimBlock|
|Default Value(s):|All allowed values|
|Multiple Tags Allowed:|Yes|

<!-- RepairPriorities -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RepairPriorities|
|:----|:----|
|Tag Format:|`[RepairPriorities:Value]`|
|Description:|This tag allows you to specify one or more repair priorities that the AiEnabled bot will use when repairing. Add them in priority order. Remove entries to have the bot ignore them completely (requires at least one entry for this to take effect). Not relevant for vanilla bots.|
|Allowed Value(s):|IMyUserControllableGun<br />IMyShipController<br />IMyPowerProducer<br />IMyThrust<br />IMyGyro<br />IMyProductionBlock<br />IMyDoor<br />IMyProgrammableBlock<br />IMyProjector<br />IMyConveyor<br />IMyCargoContainer<br />IMyFunctionalBlock<br />IMyTerminalBlock<br />IMyCubeBlock<br />IMySlimBlock|
|Default Value(s):|All allowed values|
|Multiple Tags Allowed:|Yes|