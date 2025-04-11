#AdminConfig_AiEnabled Bot Spawning.md

Bot Spawn Profiles in Modular Encounters Systems allow you to specify a set of rules for how a bot is configured before it is spawned by the AiEnabled mod.

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

<!--BotType-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BotType|
|:----|:----|
|Tag Format:|`[BotType:Value]`|
|Description:|This tag specifies the type of bot that will be spawned.|
|Allowed Values:|Any Character type (the `Name` field in the SBC, not the SubtypeId)|
|Multiple Tag Allowed:|No|

<!--BotBehavior-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BotBehavior|
|:----|:----|
|Tag Format:|`[BotBehavior:Value]`|
|Description:|This tag specifies the behavior that the spawned bot will use. If this tag is not defined, then it will use the default behavior that AiEnabled would use when spawning a bot of the selected type.|
|Allowed Values:|Any AiEnabled Behavior Role|
|Multiple Tag Allowed:|No|

<!--BotDisplayName-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BotDisplayName|
|:----|:----|
|Tag Format:|`[BotDisplayName:Value]`|
|Description:|This tag specifies the Display Name of the spawned bot.|
|Allowed Values:|Any Name|
|Multiple Tag Allowed:|No|

<!--Color-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Color|
|:----|:----|
|Tag Format:|`[Color:Value]`|
|Description:|Specifies the Color (RGB 0-255) of the spawned bot character.|
|Allowed Values:|Any Vector3 Value using this format<br />`{X:0 Y:0 Z:0}`|
|Multiple Tag Allowed:|No|

<!-- CanUseAirNodes -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CanUseAirNodes|
|:----|:----|
|Tag Format:|`[CanUseAirNodes:Value]`|
|Description:|This tag allows you to specify if the bot is able to use Air Nodes (ie Jetpack).|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- CanUseSpaceNodes -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CanUseSpaceNodes|
|:----|:----|
|Tag Format:|`[CanUseSpaceNodes:Value]`|
|Description:|This tag allows you to specify if the bot is able to use Space Nodes (ie Jetpack).|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- UseGroundNodesFirst -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseGroundNodesFirst|
|:----|:----|
|Tag Format:|`[UseGroundNodesFirst:Value]`|
|Description:|This tag allows you to specify if the bot should try to use ground nodes before using air / space nodes.|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- CanUseWaterNodes -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CanUseWaterNodes|
|:----|:----|
|Tag Format:|`[CanUseWaterNodes:Value]`|
|Description:|This tag allows you to specify if the bot is able to use water nodes (ie underwater via Water Mod)|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- WaterNodesOnly -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|WaterNodesOnly|
|:----|:----|
|Tag Format:|`[WaterNodesOnly:Value]`|
|Description:|This tag allows you to specify if the bot should only use water nodes (ie underwater only bot)|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- CanUseLadders -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CanUseLadders|
|:----|:----|
|Tag Format:|`[CanUseLadders:Value]`|
|Description:|This tag allows you to specify if the bot is able to climb ladders|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- CanUseSeats -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CanUseSeats|
|:----|:----|
|Tag Format:|`[CanUseSeats:Value]`|
|Description:|This tag allows you to specify if the bot is able to sit on seating blocks|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- DespawnTicks -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DespawnTicks|
|:----|:----|
|Tag Format:|`[DespawnTicks:Value]`|
|Description:|This tag allows you to specify if the bot should despawn after a set number of game ticks. If value is `0`, then no timer is used.|
|Allowed Value(s):|Any Integer Greater/Equal To `0`|
|Default Value(s):|`0`|
|Multiple Tags Allowed:|No|

<!-- DeathSound -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DeathSound|
|:----|:----|
|Tag Format:|`[DeathSound:Value]`|
|Description:|This tag allows you to specify a custom sound effect that is used when the bot dies.|
|Allowed Value(s):|Any Audio SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|No|

<!-- AttackSounds -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AttackSounds|
|:----|:----|
|Tag Format:|`[AttackSounds:Value]`|
|Description:|This tag allows you to specify one or more custom sound effects that are used when the bot attacks.|
|Allowed Value(s):|Any Audio SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|Yes|

<!-- PainSounds -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PainSounds|
|:----|:----|
|Tag Format:|`[PainSounds:Value]`|
|Description:|This tag allows you to specify one or more custom sound effects that are used when the bot takes damage.|
|Allowed Value(s):|Any Audio SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|Yes|

<!-- IdleSounds -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IdleSounds|
|:----|:----|
|Tag Format:|`[IdleSounds:Value]`|
|Description:|This tag allows you to specify one or more custom sound effects that are used when the bot is idle.|
|Allowed Value(s):|Any Audio SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|Yes|

<!-- TauntSounds -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TauntSounds|
|:----|:----|
|Tag Format:|`[TauntSounds:Value]`|
|Description:|This tag allows you to specify one or more custom sound effects that are used when the bot is chasing a target.|
|Allowed Value(s):|Any Audio SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|Yes|

<!-- EmoteActions -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EmoteActions|
|:----|:----|
|Tag Format:|`[EmoteActions:Value]`|
|Description:|This tag allows you to specify one or more emote that are used by nomad bots randomly.|
|Allowed Value(s):|Any Emote SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|Yes|

<!-- ShotDeviationAngle -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ShotDeviationAngle|
|:----|:----|
|Tag Format:|`[ShotDeviationAngle:Value]`|
|Description:|This tag allows you to specify the angle of deviation that is applied to gunfire from this bot.|
|Allowed Value(s):|Any Number Greater/Equal To `0'|
|Default Value(s):|`1.5`|
|Multiple Tags Allowed:|No|

<!-- LeadTargets -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LeadTargets|
|:----|:----|
|Tag Format:|`[LeadTargets:Value]`|
|Description:|This tag allows you to specify if bots should try to lead their target while shooting at them.|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- ToolSubtypeId -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ToolSubtypeId|
|:----|:----|
|Tag Format:|`[ToolSubtypeId:Value]`|
|Description:|This tag allows you to specify a custom tool/weapon for the bot to use.|
|Allowed Value(s):|Any Vanilla Tool SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|No|

<!-- CanDamageGrids -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CanDamageGrids|
|:----|:----|
|Tag Format:|`[CanDamageGrids:Value]`|
|Description:|This tag allows you to specify if the bot is able to damage grids (ie target blocks).|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|

<!-- TargetPriorities -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TargetPriorities|
|:----|:----|
|Tag Format:|`[TargetPriorities:Value]`|
|Description:|This tag allows you to specify one or more target priorities that the bot will use when attacking. Add them in priority order. Remove entries to have the bot ignore them completely (requires at least one entry for this to take effect).|
|Allowed Value(s):|IMyCharacter<br />IMyUserControllableGun<br />IMyShipController<br />IMyPowerProducer<br />IMyThrust<br />IMyGyro<br />IMyProductionBlock<br />IMyDoor<br />IMyProgrammableBlock<br />IMyProjector<br />IMyConveyor<br />IMyCargoContainer<br />IMyFunctionalBlock<br />IMyTerminalBlock<br />IMyCubeBlock<br />IMySlimBlock|
|Default Value(s):|All allowed values|
|Multiple Tags Allowed:|Yes|

<!-- RepairPriorities -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RepairPriorities|
|:----|:----|
|Tag Format:|`[RepairPriorities:Value]`|
|Description:|This tag allows you to specify one or more repair priorities that the bot will use when repairing. Add them in priority order. Remove entries to have the bot ignore them completely (requires at least one entry for this to take effect).|
|Allowed Value(s):|IMyUserControllableGun<br />IMyShipController<br />IMyPowerProducer<br />IMyThrust<br />IMyGyro<br />IMyProductionBlock<br />IMyDoor<br />IMyProgrammableBlock<br />IMyProjector<br />IMyConveyor<br />IMyCargoContainer<br />IMyFunctionalBlock<br />IMyTerminalBlock<br />IMyCubeBlock<br />IMySlimBlock|
|Default Value(s):|All allowed values|
|Multiple Tags Allowed:|Yes|