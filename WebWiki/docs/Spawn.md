#Spawn.md

Spawn Profiles in RivalAI allow you to specify a set of parameters that get passed to the Modular Encounters Spawner that will spawn an encounter when triggered. These are essentially RivalAI's alternative to Keen's Antenna Spawns. Spawn Profiles are attached to **Action Profiles**. It is important that you use a unique SubtypeId for each Spawn Profile you create, otherwise they may not work correctly.

Here's an example of how a Spawn Profile Definition is setup:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>RAI-ExampleSpawnProfile</SubtypeId>
      </Id>
      <Description>

      [RivalAI Spawn]
      
      [UseSpawn:true]
      
      [StartsReady:true]
      [SpawnMinCooldown:30]
      [SpawnMaxCooldown:60]
      [MaxSpawns:3]
      
      [SpawnGroups:DefenseDrone]
      [SpawnGroups:AssaultDrone]
      
      [UseRelativeSpawnPosition:false]
      [MinDistance:1000]
      [MaxDistance:1500]
      [MinAltitude:500]
      [MaxAltitude:1000]
      [IgnoreSafetyChecks:false]
      
      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```
<!--UseSpawn-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseSpawn|
|:----|:----|
|Tag Format:|`[UseSpawn:Value]`|
|Description:|This tag specifies if the behavior should use your Spawn Profile.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--SpawningType-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SpawningType|
|:----|:----|
|Tag Format:|`[SpawningType:Value]`|
|Description:|This tag specifies the type of spawn that this spawn profile will request from the Modular Encounters Spawner. Please note that all position tags are not used for tags other than `CustomSpawn`. Also, if using `CustomSpawn`, your SpawnGroup/Conditions must include the `[RivalAiSpawn:true]` tag|
|Allowed Values:|`CustomSpawn`<br>`SpaceCargoShip`<br>`RandomEncounter`<br>`PlanetaryCargoShip`<br>`PlanetaryInstallation`<br>`BossEncounter`|
|Multiple Tag Allowed:|No|

<!--StartsReady-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StartsReady|
|:----|:----|
|Tag Format:|`[StartsReady:Value]`|
|Description:|This tag specifies if the Spawn Profile should be ready to spawn as soon as it is triggered from the Action Profile.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--SpawnMinCooldown-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SpawnMinCooldown|
|:----|:----|
|Tag Format:|`[SpawnMinCooldown:Value]`|
|Description:|Specifies the minimum time (in seconds) after a spawn event that the profile needs to wait before it can attempt another.|
|Allowed Values:|Any integer greater than `0`<br />Must be lower than `SpawnMaxCooldown`|
|Multiple Tag Allowed:|No|

<!--SpawnMaxCooldown-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SpawnMaxCooldown|
|:----|:----|
|Tag Format:|`[SpawnMaxCooldown:Value]`|
|Description:|Specifies the maximum time (in seconds) after a spawn event that the profile needs to wait before it can attempt another.|
|Allowed Values:|Any integer greater than `0`<br />Must be higher than `SpawnMinCooldown`|
|Multiple Tag Allowed:|No|

<!--MaxSpawns-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxSpawns|
|:----|:----|
|Tag Format:|`[MaxSpawns:Value]`|
|Description:|Specifies the maximum amount of spawns that can be triggered from this profile.|
|Allowed Values:|Any integer greater than `0`<br />`-1` is used for No Limit|
|Multiple Tag Allowed:|No|

<!--SpawnGroups-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SpawnGroups|
|:----|:----|
|Tag Format:|`[SpawnGroups:Value]`|
|Description:|Specifies the SubtypeId of a SpawnGroup you want the profile to spawn. If multiple instances of this tag are provided, the Modular Encounter Spawner will select one at random based on the SpawnGroup `Frequency` rules. When using any other SpawningType than `CustomSpawn`, not providing a SpawnGroup here will request a random spawn from any eligible spawngroup for that area.|
|Allowed Values:|Any SpawnGroup SubtypeId string excluding characters `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--UseRelativeSpawnPosition-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseRelativeSpawnPosition|
|:----|:----|
|Tag Format:|`[UseRelativeSpawnPosition:Value]`|
|Description:|This tag specifies if the Spawn position should be at a relative position to the NPC Remote Control block. Otherwise the spawn will be in a random location within a min/max distance from the grid.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--MinDistance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinDistance|
|:----|:----|
|Tag Format:|`[MinDistance:Value]`|
|Description:|Specifies the minimum spawn distance from the NPC grid if `UseRelativeSpawnPosition` is `false`.|
|Allowed Values:|Any number greater than `0`<br />Must be lower than `MaxDistance`|
|Multiple Tag Allowed:|No|

<!--MaxDistance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxDistance|
|:----|:----|
|Tag Format:|`[MaxDistance:Value]`|
|Description:|Specifies the maximum spawn distance from the NPC grid if `UseRelativeSpawnPosition` is `false`.|
|Allowed Values:|Any number greater than `0`<br />Must be higher than `MinDistance`|
|Multiple Tag Allowed:|No|

<!--MinAltitude-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinAltitude|
|:----|:----|
|Tag Format:|`[MinAltitude:Value]`|
|Description:|Specifies the minimum spawn altitude from the NPC grid if `UseRelativeSpawnPosition` is `false`. This is only used if the spawn occurs in natural gravity.|
|Allowed Values:|Any number greater than `0`<br />Must be lower than `MaxAltitude`|
|Multiple Tag Allowed:|No|

<!--MaxAltitude-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxAltitude|
|:----|:----|
|Tag Format:|`[MaxAltitude:Value]`|
|Description:|Specifies the maximum spawn altitude from the NPC grid if `UseRelativeSpawnPosition` is `false`. This is only used if the spawn occurs in natural gravity.|
|Allowed Values:|Any number greater than `0`<br />Must be higher than `MinAltitude`|
|Multiple Tag Allowed:|No|

<!--InheritNpcAltitude-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|InheritNpcAltitude|
|:----|:----|
|Tag Format:|`[InheritNpcAltitude:Value]`|
|Description:|Specifies if the current altitude of the NPC should be added to the random result of `MinAltitude` and `MinAltitude`.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--RelativeSpawnOffset-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RelativeSpawnOffset|
|:----|:----|
|Tag Format:|`[RelativeSpawnOffset:Value]`|
|Description:|Specifies the position offset from the Remote Control that a spawn will be located at if `UseRelativeSpawnPosition` is `true`.|
|Allowed Values:|A Vector3D Value in the following format:<br />`{X:# Y:# Z:#}`<br />X: Right<br />Y: Up<br />Z: Forward<br />Replace `#` with values in meters.|
|Multiple Tag Allowed:|No|

<!--RelativeSpawnVelocity-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RelativeSpawnVelocity|
|:----|:----|
|Tag Format:|`[RelativeSpawnVelocity:Value]`|
|Description:|Specifies the velocity vector from the Remote Control that a spawn will start with if `UseRelativeSpawnPosition` is `true`.|
|Allowed Values:|A Vector3D Value in the following format:<br />`{X:# Y:# Z:#}`<br />X: Right<br />Y: Up<br />Z: Forward<br />Replace `#` with values in m/s.|
|Multiple Tag Allowed:|No|

<!--IgnoreSafetyChecks-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IgnoreSafetyChecks|
|:----|:----|
|Tag Format:|`[IgnoreSafetyChecks:Value]`|
|Description:|This tag specifies if the safety checks that look for obstructions before spawn should be ignored.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|
