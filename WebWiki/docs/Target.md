#Target.md

Target Profiles in RivalAI allow you to specify a set of rules that are followed when combat-based behaviors are acquiring targets near-by. You can attach your Target Profiles to any Behavior Profile by adding a `[TargetData:Value]` tag to the Behavior and replace `Value` with the SubtypeId of your Target Profile. Example:

`[TargetData:RAI-ExampleTargetProfile]`

It is important that you use a unique SubtypeId for each Target Profile you create, otherwise they may not work correctly.

You can also provide a secondary Target Profile that is used when the NPC target is switched by a Trigger/Action event such as `Damage`, `Command`, or `TurretTarget`. This type of target is consider a **Target Override** in RivalAI, and once the target is lost or a refresh occurs, the target will switch back to the target defined by the `TargetData` tag. Override targets are not limited by entity type, so they can target Players, Grids, or Blocks. To specify an Override Target Profile, use the tag below:  

`[OverrideTargetData:RAI-ExampleOverrideTargetProfile]`  

Here's an example of how a Target Profile Definition is setup:  

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>RAI-ExampleTargetProfile-PirateHunter</SubtypeId>
      </Id>
      <Description>

      [RivalAI Target]
      
      [UseCustomTargeting:true]
      [Target:Block]
      [BlockTargets:All]
      
      [MaxDistance:10000]
      
      [MatchAllFilters:OutsideOfSafezone]
      [MatchAllFilters:Owner]
      [MatchAllFilters:Relation]
      [MatchAllFilters:Faction]
      [GetTargetBy:ClosestDistance]
      
      [Owners:Player]
      [Owners:NPC]
      [Relations:Enemy]
      [FactionTargets:SPRT]
      [PrioritizeSpecifiedFactions:true]

      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseCustomTargeting|
|:----|:----|
|Tag Format:|`[UseCustomTargeting:Value]`|
|Description:|This tag specifies if the behavior should use your Target Profile. If `false`, it will use default parameters based on the Behavior Type.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|


|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Target|
|:----|:----|
|Tag Format:|`[Target:Value]`|
|Description:|This tag allows you to specify what sort of entity target the behavior should go after.|
|Allowed Values:|`Player`<br>`Block`<br>`Grid`<br>`PlayerAndBlock`<br>`PlayerAndGrid`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BlockTargets|
|:----|:----|
|Tag Format:|`[BlockTargets:Value]`|
|Description:|This tag allows you to specify one or more types of Block (if `Target` is set to Block) that will be targeted specifically.|
|Allowed Values:|`All`<br>`Antennas`<br>`Beacons`<br>`Containers`<br>`Controllers`<br>`Guns`<br>`JumpDrives`<br>`Mechanical`<br>`Medical`<br>`NanoBots`<br>`Power`<br>`Production`<br>`RivalAi`<br>`Shields`<br>`Thrusters`<br>`Tools`<br>`Turrets`|
|Multiple Tag Allowed:|Yes|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TimeUntilTargetAcquisition|
|:----|:----|
|Tag Format:|`[TimeUntilTargetAcquisition:Value]`|
|Description:|This tag specifies the time (in seconds) that it takes for the behavior to acquire a target if the behavior has no current target.|
|Allowed Values:|Any Integer Greater than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseTargetRefresh|
|:----|:----|
|Tag Format:|`[UseTargetRefresh:Value]`|
|Description:|This tag specifies if the behavior should attempt to acquire a new target after some time, even if the behavior has a valid target already.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TimeUntilNextRefresh|
|:----|:----|
|Tag Format:|`[TimeUntilNextRefresh:Value]`|
|Description:|This tag specifies the time (in seconds) that it takes for the behavior to acquire a new target if `UseTargetRefresh` is set to `true`.|
|Allowed Values:|Any Integer Greater than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TimeUntilNextEvaluation|
|:----|:----|
|Tag Format:|`[TimeUntilNextEvaluation:Value]`|
|Description:|This tag specifies the time (in seconds) that it takes for the behavior to check the current target to ensure it still meets the requirements of the settings and filters in this Target Profile.|
|Allowed Values:|Any Integer Greater than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxDistance|
|:----|:----|
|Tag Format:|`[MaxDistance:Value]`|
|Description:|This tag allows you to specify the distance from the behavior owner that is checked for targets.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxExistingTargetDistance|
|:----|:----|
|Tag Format:|`[MaxExistingTargetDistance:Value]`|
|Description:|This tag allows you to specify the distance that an existing valid target must be outside before a new target is selected. If this value is not provided, then it uses the same value as `MaxDistance`|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MatchAllFilters|
|:----|:----|
|Tag Format:|`[MatchAllFilters:Value]`|
|Description:|This tag allows you to specify one or more extra conditions that must be met when acquiring or evaluating a target. If any of the specified conditions under this tag are not met, the target is not considered valid.|
|Allowed Values:|`Altitude`<br>`Broadcasting`<br>`Faction`<br>`Gravity`<br>`IgnoreStealthDrive`<br />`LineOfSight`<br>`MovementScore`<br>`Name`<br>`OutsideOfSafezone`<br>`Owner`<br>`PlayerControlled`<br>`PlayerKnownLocation`<br>`Powered`<br>`Relation`<br>`Shielded`<br>`Speed`<br>`Static`<br>`TargetValue`|
|Multiple Tag Allowed:|Yes|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MatchAnyFilters|
|:----|:----|
|Tag Format:|`[MatchAnyFilters:Value]`|
|Description:|This tag allows you to specify one or more extra conditions that must be met when acquiring or evaluating a target. If any of the specified conditions under this tag are met, the target is considered valid.|
|Allowed Values:|`Altitude`<br>`Broadcasting`<br>`Faction`<br>`Gravity`<br>`IgnoreStealthDrive`<br />`LineOfSight`<br>`MovementScore`<br>`Name`<br>`OutsideOfSafezone`<br>`Owner`<br>`PlayerControlled`<br>`PlayerKnownLocation`<br>`Powered`<br>`Relation`<br>`Shielded`<br>`Speed`<br>`Static`<br>`TargetValue`|
|Multiple Tag Allowed:|Yes|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MatchNoneFilters|
|:----|:----|
|Tag Format:|`[MatchNoneFilters:Value]`|
|Description:|This tag allows you to specify one or more extra conditions that must be met when acquiring or evaluating a target. If any of the specified conditions under this tag are met, the target is not considered valid.|
|Allowed Values:|`Altitude`<br>`Broadcasting`<br>`Faction`<br>`Gravity`<br>`IgnoreStealthDrive`<br />`LineOfSight`<br>`MovementScore`<br>`Name`<br>`OutsideOfSafezone`<br>`Owner`<br>`PlayerControlled`<br>`PlayerKnownLocation`<br>`Powered`<br>`Relation`<br>`Shielded`<br>`Speed`<br>`Static`<br>`TargetValue`|
|Multiple Tag Allowed:|Yes|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|GetTargetBy|
|:----|:----|
|Tag Format:|`[GetTargetBy:Value]`|
|Description:|This tag allows you to specify which target should be used after all eligible targets have been identified. `HighestTargetValue` and `LowestTargetValue` are similar to the threat score calculations that are done in the Modular Encounters Spawner.|
|Allowed Values:|`Random`<br>`ClosestDistance`<br>`FurthestDistance`<br>`HighestTargetValue`<br>`LowestTargetValue`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Owners|
|:----|:----|
|Tag Format:|`[Owners:Value]`|
|Description:|This tag allows you to specify one or more ownership types that the target must have to be considered.|
|Filter Required:|`Owner`|
|Allowed Values:|`Unowned`<br>`Player`<br>`NPC`|
|Multiple Tag Allowed:|Yes|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Relations|
|:----|:----|
|Tag Format:|`[Relations:Value]`|
|Description:|This tag allows you to narrow down targets based on their relation to the behavior owner.|
|Filter Required:|`Relation`|
|Allowed Values:|`Faction`<br>`Neutral`<br>`Enemy`<br>`Friends`|
|Multiple Tag Allowed:|Yes|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|FactionTargets|
|:----|:----|
|Tag Format:|`[FactionTargets:Value]`|
|Description:|This tag allows you to narrow down targets based on their Faction Owner.|
|Filter Required:|`Faction`|
|Allowed Values:|Any Faction Tag|
|Multiple Tag Allowed:|Yes|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|OnlyGetFromEntityOwner|
|:----|:----|
|Tag Format:|`[OnlyGetFromEntityOwner:Value]`|
|Description:|This tag specifies how the Owner and Relation is considered for targets. When `false`, ownership of subgrids is considered. While `true`, only the immediate grid or block is considered.|
|Filter Required:|`Owner` and/or `Relation`|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|GetFromMinorityGridOwners|
|:----|:----|
|Tag Format:|`[GetFromMinorityGridOwners:Value]`|
|Description:|This tag specifies how the Owner and Relation is considered for targets. When `false`, only the majority ownership of a grid is considered. While `true`, minority ownership of a grid is also considered (eg: a player owning a single block on an NPC grid would flag the grid as NPC and Player owned).|
|Filter Required:|`Owner` and/or `Relation`|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PrioritizeSpecifiedFactions|
|:----|:----|
|Tag Format:|`[PrioritizeSpecifiedFactions:Value]`|
|Description:|This tag specifies how Faction ownership should be considered for targets when using a `Faction` filter. If `false`, then the target MUST match the faction(s) provided. If `true`, then the evaluation will try to find the specified factions first, but will consider other valid targets if no targets from the specified factions can be found.|
|Filter Required:|`Faction`|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PrioritizePlayerControlled|
|:----|:----|
|Tag Format:|`[PrioritizePlayerControlled:Value]`|
|Description:|This tag specifies how targets should be considered when using a `PlayerControlled` filter. If `false`, then the target MUST match be player controlled. If `true`, then the evaluation will try to find the player controlled targets first, but will consider other valid targets if no player controlled targets can be found.|
|Filter Required:|`PlayerControlled`|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IsStatic|
|:----|:----|
|Tag Format:|`[IsStatic:Value]`|
|Description:|This tag specifies if a target should be a Static Grid.|
|Filter Required:|`Static`|
|Allowed Values:|`Yes`<br>`No`<br>`Ignore`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinAltitude|
|:----|:----|
|Tag Format:|`[MinAltitude:Value]`|
|Description:|This tag specifies the minimum altitude a target must be at to be considered valid. If a target is not on a planet, then this tag is not considered. Value must not be `higher` than `MaxAltitude`|
|Filter Required:|`Altitude`|
|Allowed Values:|Any Number (Negative or Positive)|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxAltitude|
|:----|:----|
|Tag Format:|`[MaxAltitude:Value]`|
|Description:|This tag specifies the maximum altitude a target must be at to be considered valid. If a target is not on a planet, then this tag is not considered. Value must not be `lower` than `MinAltitude`|
|Filter Required:|`Altitude`|
|Allowed Values:|Any Number (Negative or Positive)|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|NonBroadcastVisualRange|
|:----|:----|
|Tag Format:|`[NonBroadcastVisualRange:Value]`|
|Description:|This tag specifies if a non broadcasting target should be considered if the target is within this specified distance.|
|Filter Required:|`Broadcasting`|
|Allowed Values:|Any Number Higher Than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinGravity|
|:----|:----|
|Tag Format:|`[MinGravity:Value]`|
|Description:|This tag specifies the minimum gravity a target must be at to be considered valid. Value must not be `higher` than `MaxGravity`|
|Filter Required:|`Gravity`|
|Allowed Values:|Any Number Higher Than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxGravity|
|:----|:----|
|Tag Format:|`[MaxGravity:Value]`|
|Description:|This tag specifies the maximum gravity a target must be at to be considered valid. Value must not be `lower` than `MinGravity`|
|Filter Required:|`Gravity`|
|Allowed Values:|Any Number Higher Than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinSpeed|
|:----|:----|
|Tag Format:|`[MinSpeed:Value]`|
|Description:|This tag specifies the minimum speed a target must be at to be considered valid. Value must not be `higher` than `MaxSpeed`|
|Filter Required:|`Speed`|
|Allowed Values:|Any Number Higher Than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxSpeed|
|:----|:----|
|Tag Format:|`[MaxSpeed:Value]`|
|Description:|This tag specifies the maximum speed a target must be at to be considered valid. Value must not be `lower` than `MinSpeed`|
|Filter Required:|`Speed`|
|Allowed Values:|Any Number Higher Than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinTargetValue|
|:----|:----|
|Tag Format:|`[MinTargetValue:Value]`|
|Description:|This tag specifies the minimum value a target must be at to be considered valid. Value must not be `higher` than `MaxTargetValue`|
|Filter Required:|`TargetValue`|
|Allowed Values:|Any Number Higher Than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxTargetValue|
|:----|:----|
|Tag Format:|`[MaxTargetValue:Value]`|
|Description:|This tag specifies the maximum value a target must be at to be considered valid. Value must not be `lower` than `MinTargetValue`|
|Filter Required:|`TargetValue`|
|Allowed Values:|Any Number Higher Than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Names|
|:----|:----|
|Tag Format:|`[Names:Value]`|
|Description:|This tag allows you to narrow down targets based on their Name (Block/Grid CustomName and Player DisplayName)|
|Filter Required:|`Name`|
|Allowed Values:|Any Name|
|Multiple Tag Allowed:|Yes|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UsePartialNameMatching|
|:----|:----|
|Tag Format:|`[UsePartialNameMatching:Value]`|
|Description:|This tag specifies if names used in provided `Names` tag are allowed to be a partial match. If `false`, then a target name must match exactly with one of the names provided.|
|Filter Required:|`Name`|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinMovementScore|
|:----|:----|
|Tag Format:|`[MinMovementScore:Value]`|
|Description:|This tag specifies the minimum movement score (bounding-box size multiplied by current speed) a target must have to be considered valid|
|Filter Required:|`MovementScore`|
|Allowed Values:|Any Number Higher Than `0`<br>Value should be lower than `MaxMovementScore` if tag is provided.|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxMovementScore|
|:----|:----|
|Tag Format:|`[MaxMovementScore:Value]`|
|Description:|This tag specifies the maximum movement score (bounding-box size multiplied by current speed) a target must have to be considered valid.|
|Filter Required:|`MovementScore`|
|Allowed Values:|Any Number Higher Than `0`<br>Value should be lower than `MaxMovementScore` if tag is provided.|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxMovementDetectableDistance|
|:----|:----|
|Tag Format:|`[MaxMovementDetectableDistance:Value]`|
|Description:|This tag specifies the maximum distance from the NPC that Movement Score is eligible to be calculated at.|
|Filter Required:|`MovementScore`|
|Allowed Values:|Any Number Higher Than `0`.|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxLineOfSight|
|:----|:----|
|Tag Format:|`[MaxLineOfSight:Value]`|
|Description:|This tag specifies the maximum distance that is used when performing a Line of Sight check|
|Filter Required:|`LineOfSight`|
|Allowed Values:|Any Number Higher Than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PlayerKnownLocationFactionOverride|
|:----|:----|
|Tag Format:|`[PlayerKnownLocationFactionOverride:Value]`|
|Description:|This tag allows you to override the Player Known Location Faction Check to use a faction that is different from the faction that owns the NPC Remote Control|
|Filter Required:|`PlayerKnownLocation`|
|Allowed Values:|Any NPC Faction Tag|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StealthDriveMinDistance|
|:----|:----|
|Tag Format:|`[StealthDriveMinDistance:Value]`|
|Description:|This tag specifies the minimum distance a target must maintain in order to remain undetectable while using a Stealth Drive.|
|Filter Required:|`LineOfSight`|
|Allowed Values:|Any Number Higher Than `0`|
|Multiple Tag Allowed:|No|
