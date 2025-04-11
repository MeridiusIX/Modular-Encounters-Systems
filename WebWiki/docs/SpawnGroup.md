#SpawnGroup.md

SpawnGroups are definitions that provide rules about an NPC encounter to the Modular Encounters Systems Spawning Mechanism.

The SpawnGroups that the Modular Encounters Systems uses utilize the `<Description>` tags to define many custom rules and modifications to a spawned entity. This includes conditions for when the encounter is allowed to spawn, and manipulations that are applied to the prefabs (grids) before they are spawned into the world.



[**Conditions**](#Conditions)  
[**Economy**](#Economy)   
[**Manipulations**](#Manipulations  )  
[**Misc**](#Misc)  
[**Replenishment**](#Replenishment)  

# Conditions

<!-- SpawnConditionsProfiles-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SpawnConditionsProfiles|
|:----|:----|
|Tag Format:|`[SpawnConditionsProfiles:Value]`|
|Description:|This tag allows you to provide one or more sets of Spawn Conditions to the SpawnGroup. If a single Spawn Conditions Profile is satisfied, then the spawn will be considered eligible.|
|Allowed Values:|Any Spawn Condition Profile SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!-- SpawnConditionGroups-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SpawnConditionGroups|
|:----|:----|
|Tag Format:|`[SpawnConditionGroups:Value]`|
|Description:|This tag allows you to provide one or more sets of Spawn Condition Groups to the SpawnGroup.|
|Allowed Values:|Any Spawn Condition Group Profile SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!-- PersistentConditions-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PersistentConditions|
|:----|:----|
|Tag Format:|`[PersistentConditions:Value]`|
|Description:|This tag allows you to provide a single Spawn Condition Profile that is used as a persistent set of conditions IN ADDITION to the existing spawn conditions defined in `SpawnConditionsProfiles`. If the conditions in this profile are not satisfied, the spawn will not be eligible even if other Spawn Conditions are. Some tags may not work properly if used in this profile (eg: Spawn Type related tags, Faction, etc), so try to avoid using those.|
|Allowed Values:|Any Spawn Condition Profile SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|No|

<!-- UseFirstConditionsAsPersistent-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseFirstConditionsAsPersistent|
|:----|:----|
|Tag Format:|`[UseFirstConditionsAsPersistent:Value]`|
|Description:|This tag allows you to specify if Spawn Conditions tags that are placed directly in the spawngroup should be used as the 'Persistent' Spawn Conditions. If you use this, then the profile in `PersistentConditions` is no longer used. This is useful if you have some properties you want to apply only to a single spawngroup, but don't want to create a separate spawn conditions profile. `MaxGravity` is a good example of a tag that works well here.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|


# Economy

<!-- InitializeStoreBlocks  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|InitializeStoreBlocks|
|:----|:----|
|Tag Format:|`[InitializeStoreBlocks:Value]`|
|Description:|If set to `true`, any Store Blocks on this grid will be populated with all items in inventory the Store Block is connected to.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- ContainerTypesForStoreOrders  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ContainerTypesForStoreOrders|
|:----|:----|
|Tag Format:|`[ContainerTypesForStoreOrders:Value1,Value2,Value3,etc]`|
|Description:|This tag allows you to specify the names of ContainerType definitions that are randomly selected to populate the Store Block Sell Screen on your NPC Grid.|
|Allowed Values:|Any ContainerType SubtypeId<br>If providing multiple values, use comma with no spaces as shown in `Tag Format`|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|No|


# Manipulations  

<!-- ManipulationProfiles-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManipulationProfiles|
|:----|:----|
|Tag Format:|`[ManipulationProfiles:Value]`|
|Description:|This tag allows you to provide one or more Manipulation Profiles to the SpawnGroup. If multiple profiles are provided, then all of the Manipulations within each profile are processed on the Grids before they spawn..|
|Allowed Values:|Any Manipulation Profile SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!-- WeaponRandomizationOverrideProfile-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|WeaponRandomizationOverrideProfile|
|:----|:----|
|Tag Format:|`[WeaponRandomizationOverrideProfile:Value]`|
|Description:|This tag allows you to provide a Manipulation Profile that is used exclusively for defining rules related to Weapon Randomization. If you use this profile, then all other manipulation profiles are not used for anything Weapon Randomization related. Any tags included in this profile that are not related to Weapon Randomization are also not used.|
|Allowed Values:|Any Manipulation Profile SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|No|


# Misc

<!-- FactionOverride-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|FactionOverride|
|:----|:----|
|Tag Format:|`[FactionOverride:Value]`|
|Description:|This tag specifies if a SpawnGroup should use a specific Faction Tag for spawning/ownership, regardless of the tags being used in the SpawnConditions profiles.|
|Allowed Values:|Any NPC faction tag|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|No|

<!-- IgnoreCleanupRules  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IgnoreCleanupRules|
|:----|:----|
|Tag Format:|`[IgnoreCleanupRules:Value]`|
|Description:|This tag specifies if a SpawnGroup should be ignored by the mod Clean-Up rules (this does not include path despawn for Cargo Ship Type Spawns).|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- PauseAutopilotAtPlayerDistance  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PauseAutopilotAtPlayerDistance|
|:----|:----|
|Tag Format:|`[PauseAutopilotAtPlayerDistance:Value]`|
|Description:|If player is within this distance from a Cargo Ship using Auto-Pilot, the ship will come to a stop. It will resume travel once the player is outside of this distance.|
|Allowed Values:|Any Number Greater Than `0`|
|Default Value(s):|`-1`|
|Multiple Tag Allowed:|No|


# Replenishment

<!-- ReplenishSystems  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReplenishSystems|
|:----|:----|
|Tag Format:|`[ReplenishSystems:Value]`|
|Description:|This tag specifies if the prefabs in a SpawnGroup should have their weapons, parachutes, and reactors restocked with ammo and fuel on spawn.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- ReplenishProfiles-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReplenishProfiles|
|:----|:----|
|Tag Format:|`[ReplenishProfiles:Value]`|
|Description:|This tag specifies one or more Replenishment Profile IDs that are used to determine what kinds of replenishment limits the grid will receive.|
|Allowed Values:|Any Replenishment Profile SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!-- IgnoreGlobalReplenishProfiles-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IgnoreGlobalReplenishProfiles|
|:----|:----|
|Tag Format:|`[IgnoreGlobalReplenishProfiles:Value]`|
|Description:|This tag specifies if grid replenishment should ignore any Global Replenishment Profiles that are present in the game world.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|
