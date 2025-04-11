#Manipulation.md

Manipulation Profiles allow you to group together customizations that are applied after a SpawnGroup is selected, but before the grid is created in the game world.

SpawnGroups can have multiple Manipulation Profiles. After a SpawnGroup has been selected for spawn, all of the attached Manipulation Profiles in that SpawnGroup will be processed and applied to the Prefabs prior to spawn. This allows you to easily create sets of common customizations that you can apply to multiple SpawnGroups, and tweak them across all the encounters by editing the applicable profile.

Manipulation Profiles also have some limited conditional parameters as well to determine if a profile should be allowed to process. Examples include min/max threat levels, which Spawn Condition Profile was used, and more!

The tags in the Manipulation Profiles can be placed directly in the SpawnGroup tags. This is because these tags were once part of the regular SpawnGroup tags, and steps to ensure compatibility were considered.

Here is an example of how a Spawn Condition Profile is created:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>MES-Manipulation-PaintingExample</SubtypeId>
      </Id>
      <Description>

        [MES Manipulation]

        [SkinRandomBlocks:true]
        [MinPercentageSkinRandomBlocks:15]
        [MaxPercentageSkinRandomBlocks:40]
        [SkinRandomBlocksTextures:Rusty_Armor]
        [SkinRandomBlocksTextures:Battered_Armor]
        [SkinRandomBlocksTextures:Heavy_Rust_Armor]

        [ReduceBlockBuildStates:true]
        [MinimumBlocksPercent:5]
        [MaximumBlocksPercent:15]
        [MinimumBuildPercent:10]
        [MaximumBuildPercent:75]

      </Description>

    </EntityComponent>
    
  </EntityComponents>
</Definitions>
```

To link a profile to your SpawnGroup, simply use the `ManipulationProfiles` tag and provide the SubtypeId of the Manipulation Profile you created. Eg: `[ManipulationProfiles:MES-Manipulation-CargoShipExample]`. 

Below are several types of tags you can include in your Manipulation Profile:

[Conditions](#Conditions)

[General](#General)  
[Armor-Modules](#Armor-Modules)  
[Block-Replacement](#Block-Replacement)  
[Block-Settings](#Block-Settings)  
[Build-States](#Build-States)  
[Damage](#Damage)  
[Inventory](#Inventory)  
[Paint-and-Skins](#Paint-and-Skins)  
[Profiles](#Profiles)  
[Renaming](#Renaming)  
[RivalAI](#RivalAI)
[Shields](#Shields)  
[Propulsion](#Propulsion)  
[Turrets](#Turrets)  
[Weapon-Randomization](#Weapon-Randomization)  


# Conditions

<!-- ManipulationChance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManipulationChance|
|:----|:----|
|Tag Format:|`[ManipulationChance:Value]`|
|Description:|This tag allows you to specify a number between `0` and `100` that is used to determine the chance the Manipulation Profile has to be applied to the pending spawn.|
|Allowed Values:|Integer Between `0` and `100`|
|Default Value(s):|`100`|
|Multiple Tag Allowed:|No|

<!-- RequiredManipulationSpawnType-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RequiredManipulationSpawnType|
|:----|:----|
|Tag Format:|`[RequiredManipulationSpawnType:Value]`|
|Description:|This tag allows you to specify one or more Spawn Types. If any of the provided Spawn Types match the Type of the encounter that is being spawned, then this condition is considered satisfied.|
|Allowed Values:|`SpaceCargoShip`<br />`LunarCargoShip`<br />`RandomEncounter`<br />`PlanetaryCargoShip`<br />`GravityCargoShip`<br />`PlanetaryInstallation`<br />`WaterSurfaceStation`<br />`UnderWaterStation`<br />`BossSpace`<br />`BossAtmo`<br />`BossGravity`<br />`Creature`<br />`OtherNPC`<br />`DroneEncounter`<br />`StaticEncounter`<br />`DryLandInstallation`|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!-- RequiredManipulationSpawnConditions-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RequiredManipulationSpawnConditions|
|:----|:----|
|Tag Format:|`[RequiredManipulationSpawnConditions:Value]`|
|Description:|This tag allows you to specify one or more Spawn Condition Profile SubtypeIds. If any Spawn Condition Profile with a matching ID is used to spawn the encounter, then this condition is considered satisfied.|
|Allowed Values:|Any Spawn Condition Profile SubtypeID|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!-- ManipulationThreatMinimum-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManipulationThreatMinimum|
|:----|:----|
|Tag Format:|`[ManipulationThreatMinimum:Value]`|
|Description:|This tag allows you to specify a minimum threat score that must be met for this Manipulation Profile to be applied to the pending spawn. If this tag is not provided, then no minimum is used.|
|Allowed Values:|Any Number|
|Default Value(s):|`-1`|
|Multiple Tag Allowed:|No|

<!-- ManipulationThreatMaximum-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManipulationThreatMaximum|
|:----|:----|
|Tag Format:|`[ManipulationThreatMaximum:Value]`|
|Description:|This tag allows you to specify a maximum threat score that must be met for this Manipulation Profile to be applied to the pending spawn. If this tag is not provided, then no maximum is used.|
|Allowed Values:|Any Number|
|Default Value(s):|`-1`|
|Multiple Tag Allowed:|No|

<!-- ManipulationMinDifficulty-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManipulationMinDifficulty|
|:----|:----|
|Tag Format:|`[ManipulationMinDifficulty:Value]`|
|Description:|This tag allows you to specify a minimum difficulty (which is set in the General Config of the world) that must be met for this Manipulation Profile to be applied to the pending spawn. If this tag is not provided, then no minimum is used.|
|Allowed Values:|Any Integer|
|Default Value(s):|`-1`|
|Multiple Tag Allowed:|No|

<!-- ManipulationMaxDifficulty-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManipulationMaxDifficulty|
|:----|:----|
|Tag Format:|`[ManipulationMaxDifficulty:Value]`|
|Description:|This tag allows you to specify a maximum difficulty (which is set in the General Config of the world) that must be met for this Manipulation Profile to be applied to the pending spawn. If this tag is not provided, then no maximum is used.|
|Allowed Values:|Any Integer|
|Default Value(s):|`-1`|
|Multiple Tag Allowed:|No|

<!-- ManipulationMinBlockCount-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManipulationMinBlockCount|
|:----|:----|
|Tag Format:|`[ManipulationMinBlockCount:Value]`|
|Description:|This tag allows you to specify a minimum prefab block count for this Manipulation Profile to be applied to the pending spawn. If this tag is not provided, then no minimum is used.|
|Allowed Values:|Any Number|
|Default Value(s):|`-1`|
|Multiple Tag Allowed:|No|

<!-- ManipulationMaxBlockCount-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManipulationMaxBlockCount|
|:----|:----|
|Tag Format:|`[ManipulationMaxBlockCount:Value]`|
|Description:|This tag allows you to specify a maximum prefab block count for this Manipulation Profile to be applied to the pending spawn. If this tag is not provided, then no maximum is used.|
|Allowed Values:|Any Number|
|Default Value(s):|`-1`|
|Multiple Tag Allowed:|No|

<!-- ManipulationAllowedPrefabNames-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManipulationAllowedPrefabNames|
|:----|:----|
|Tag Format:|`[ManipulationAllowedPrefabNames:Value]`|
|Description:|This tag allows you to specify one or more Prefab Names that must match the current Prefab that is being manipulated in order to run the Manipulation Profile.|
|Allowed Values:|Any Prefab SubtypeID|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!-- ManipulationRestrictedPrefabNames-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManipulationRestrictedPrefabNames|
|:----|:----|
|Tag Format:|`[ManipulationRestrictedPrefabNames:Value]`|
|Description:|This tag allows you to specify one or more Prefab Names that cannot match the current Prefab that is being manipulated in order to run the Manipulation Profile.|
|Allowed Values:|Any Prefab SubtypeID|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!-- ManipulationAllowedPrefabIndexes-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManipulationAllowedPrefabIndexes|
|:----|:----|
|Tag Format:|`[ManipulationAllowedPrefabIndexes:Value]`|
|Description:|This tag allows you to specify one or more Prefab Indexes (eg: 0, 1, 2, etc) that must match the current Prefab that is being manipulated in order to run the Manipulation Profile.|
|Allowed Values:|Any Integer Matching a Prefab Index|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!-- ManipulationRestrictedPrefabIndexes-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManipulationRestrictedPrefabIndexes|
|:----|:----|
|Tag Format:|`[ManipulationRestrictedPrefabIndexes:Value]`|
|Description:|This tag allows you to specify one or more Prefab Indexes (eg: 0, 1, 2, etc) that cannot match the current Prefab that is being manipulated in order to run the Manipulation Profile.|
|Allowed Values:|Any Integer Matching a Prefab Index|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!-- ManipulationRequiredCustomTags-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManipulationRequiredCustomTags|
|:----|:----|
|Tag Format:|`[ManipulationRequiredCustomTags:Value]`|
|Description:|This tag allows you to specify one or more CustomTags that must be present in the prefab in order to run the Manipulation Profile.|
|Allowed Values:|Any String Value|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|


# General

<!-- ClearAuthorship  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ClearAuthorship|
|:----|:----|
|Tag Format:|`[ClearAuthorship:Value]`|
|Description:|This tag allows you to specify all blocks should have their authorship (BuiltBy) removed.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|


# Armor-Modules

<!-- ReplaceArmorBlocksWithModules-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReplaceArmorBlocksWithModules|
|:----|:----|
|Tag Format:|`[ReplaceArmorBlocksWithModules:Value]`|
|Description:|This tag allows you to specify if some armor blocks (cubes) on the grid should be replaced with specialized modules that add additional abilities and effects to the NPC Prefabs.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- ModulesForArmorReplacement-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ModulesForArmorReplacement|
|:----|:----|
|Tag Format:|`[ModulesForArmorReplacement:Value]`|
|Description:|This tag lets you determine the types of blocks that will replace an armor cube. For every value provided in this tag, one armor cube is replaced with the provided block type.|
|Allowed Values:|MyDefinitionId of Block|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|


# Block-Replacement

<!-- UseBlockReplacer  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseBlockReplacer|
|:----|:----|
|Tag Format:|`[UseBlockReplacer:Value]`|
|Description:|This tag let's you determine if a SpawnGroup should replace certain block types using the provided template in `ReplaceBlockReference`.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- ReplaceBlockOld  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReplaceBlockOld|
|:----|:----|
|Tag Format:|`[ReplaceBlockOld:Value]`|
|Description:|This tag lets you determine a block that gets replaced if `UseBlockReplacer` is enabled. If this tag is used, you must also provide a `ReplaceBlockNew` tag for the block that is replacing the old block.|
|Allowed Values:|MyDefinitionId of Block|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!-- ReplaceBlockNew  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReplaceBlockNew|
|:----|:----|
|Tag Format:|`[ReplaceBlockNew:Value]`|
|Description:|This tag lets you determine a block that replaces an other block if `UseBlockReplacer` is enabled. If this tag is used, you must also provide a `ReplaceBlockOld` tag for the block that is being replaced.|
|Allowed Values:|MyDefinitionId of Block|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!-- UseBlockReplacerProfile  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseBlockReplacerProfile|
|:----|:----|
|Tag Format:|`[UseBlockReplacerProfile:Value]`|
|Description:|This tag let's you assign one or more Block Replacement Profile names to easily replace multiple blocks in your NPC Grid. [Click Here](https://gist.github.com/MeridiusIX/415b45b53174c608c6486ce06bb58e2c) for a list of existing Block Replacement Profiles.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- BlockReplacerProfileNames  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BlockReplacerProfileNames|
|:----|:----|
|Tag Format:|`[BlockReplacerProfileNames:Value1,Value2,Value3,etc]`|
|Description:|This tag allows you to specify profile names of Blocks to replace if `UseBlockReplacerProfile` is set to `true`.|
|Allowed Values:|Any Block Replacer Profile Name<br>If providing multiple values, use comma with no spaces as shown in `Tag Format`|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|Yes|

<!-- RelaxReplacedBlocksSize  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RelaxReplacedBlocksSize|
|:----|:----|
|Tag Format:|`[RelaxReplacedBlocksSize:Value]`|
|Description:|This tag allows you to specify if blocks being replaced while using either the `UseBlockReplacer` or `UseBlockReplacerProfile` tags can be a mis-matched size. This is not always safe to use since it does not check mounting points and orientations, so ensure that it works properly with the SpawnGroup before going live with it.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- AlwaysRemoveBlock  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AlwaysRemoveBlock|
|:----|:----|
|Tag Format:|`[AlwaysRemoveBlock:Value]`|
|Description:|This tag allows you to specify if blocks being replaced while using either the `UseBlockReplacer` or `UseBlockReplacerProfile` should be removed, regardless if a valid replacement can be found.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- IgnoreGlobalBlockReplacer  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IgnoreGlobalBlockReplacer|
|:----|:----|
|Tag Format:|`[IgnoreGlobalBlockReplacer:Value]`|
|Description:|This tag allows you to specify if any block replacements specified in the Global Block Replacer (found in General Configuration) should be ignored.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- ConvertToHeavyArmor  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ConvertToHeavyArmor|
|:----|:----|
|Tag Format:|`[ConvertToHeavyArmor:Value]`|
|Description:|This tag allows you to specify if a grid should have all vanilla light armor blocks replaced with their respective heavy armor variant. `UseBlockReplacerProfile` must be set to true for this to function.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|


# Block-Settings

<!-- EraseIngameScripts  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EraseIngameScripts|
|:----|:----|
|Tag Format:|`[EraseIngameScripts:Value]`|
|Description:|This tag allows you to specify if all programmable blocks on a grid should have their scripts erased.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- DisableTimerBlocks  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DisableTimerBlocks|
|:----|:----|
|Tag Format:|`[DisableTimerBlocks:Value]`|
|Description:|This tag allows you to specify if all timer blocks on a grid should be disabled.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- DisableSensorBlocks  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DisableSensorBlocks|
|:----|:----|
|Tag Format:|`[DisableSensorBlocks:Value]`|
|Description:|This tag allows you to specify if all sensor blocks on a grid should be disabled.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- DisableWarheads  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DisableWarheads|
|:----|:----|
|Tag Format:|`[DisableWarheads:Value]`|
|Description:|This tag allows you to specify if all warheads on a grid should be disarmed and have their countdown stopped.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- DisableThrustOverride  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DisableThrustOverride|
|:----|:----|
|Tag Format:|`[DisableThrustOverride:Value]`|
|Description:|This tag allows you to specify if all thrusters should have disabled thruster override.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- DisableGyroOverride  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DisableGyroOverride|
|:----|:----|
|Tag Format:|`[DisableGyroOverride:Value]`|
|Description:|This tag allows you to specify if all gyros should have their override disabled.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- EnableBlocksWithName  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EnableBlocksWithName|
|:----|:----|
|Tag Format:|`[EnableBlocksWithName:Value1,Value2,etc]`|
|Description:|This tag allows you to specify one or more blocks by name that will be turned on / enabled at spawn.|
|Allowed Values:|Any Block Name<br>If providing multiple values, use comma with no spaces as shown in `Tag Format`|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|No|

<!-- DisableBlocksWithName  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DisableBlocksWithName|
|:----|:----|
|Tag Format:|`[DisableBlocksWithName:Value1,Value2,etc]`|
|Description:|This tag allows you to specify one or more blocks by name that will be turned off / disabled at spawn.|
|Allowed Values:|Any Block Name<br>If providing multiple values, use comma with no spaces as shown in `Tag Format`|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|No|

<!-- AllowPartialNames  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AllowPartialNames|
|:----|:----|
|Tag Format:|`[AllowPartialNames:Value]`|
|Description:|This tag allows you to specify if names specified in `EnableBlocksWithName` and `DisableBlocksWithName` can be partial matches (eg: match `Thruster` in `Hydrogen Thruster`).|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|


# Build-States

<!-- ReduceBlockBuildStates  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReduceBlockBuildStates|
|:----|:----|
|Tag Format:|`[ReduceBlockBuildStates:Value]`|
|Description:|This tag allows you to specify if the blocks on a grid should be partially ground down. This property only affect non-essential blocks (eg: armor, glass, etc).|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- MinimumBlocksPercent  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinimumBlocksPercent|
|:----|:----|
|Tag Format:|`[MinimumBlocksPercent:Value]`|
|Description:|This tag allows you to specify the minimum amount of blocks that should be affected by percentage if `ReduceBlockBuildStates` is enabled.|
|Allowed Values:|Any Number Equal or Greater Than `0`<br>Value should be Lower than `MaximumBlocksPercent` value|
|Default Value(s):|`10`|
|Multiple Tag Allowed:|No|

<!-- MaximumBlocksPercent  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaximumBlocksPercent|
|:----|:----|
|Tag Format:|`[MaximumBlocksPercent:Value]`|
|Description:|This tag allows you to specify the minimum amount of blocks that should be affected by percentage if `ReduceBlockBuildStates` is enabled.|
|Allowed Values:|Any Number Equal or Greater Than `0`<br>Value should be Higher than `MinimumBlocksPercent` value|
|Default Value(s):|`40`|
|Multiple Tag Allowed:|No|

<!-- MinimumBuildPercent  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinimumBuildPercent|
|:----|:----|
|Tag Format:|`[MinimumBuildPercent:Value]`|
|Description:|This tag allows you to specify the minimum build state of affected blocks by percentage if `ReduceBlockBuildStates` is enabled.|
|Allowed Values:|Any Number Equal or Greater Than `0`<br>Value should be Lower than `MaximumBuildPercent` value|
|Default Value(s):|`10`|
|Multiple Tag Allowed:|No|

<!-- MaximumBuildPercent  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaximumBuildPercent|
|:----|:----|
|Tag Format:|`[MaximumBuildPercent:Value]`|
|Description:|This tag allows you to specify the maximum build state of affected blocks by percentage if `ReduceBlockBuildStates` is enabled.|
|Allowed Values:|Any Number Equal or Greater Than `0`<br>Value should be Higher than `MinimumBuildPercent` value|
|Default Value(s):|`75`|
|Multiple Tag Allowed:|No|


# Damage

<!-- OverrideBlockDamageModifier  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|OverrideBlockDamageModifier|
|:----|:----|
|Tag Format:|`[OverrideBlockDamageModifier:Value]`|
|Description:|This tag allows you to specify if the blocks on a grid should have a custom damage multiplier applied. Keep in mind this is applied at the block level and not at the grid level.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- BlockDamageModifier  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BlockDamageModifier|
|:----|:----|
|Tag Format:|`[BlockDamageModifier:Value]`|
|Description:|This tag allows you to specify the percentage of damage blocks take if OverrideBlockDamageModifier is enabled.|
|Allowed Values:|Any Number Equal or Greater Than `0`|
|Default Value(s):|`100`|
|Multiple Tag Allowed:|No|

<!-- GridsAreEditable  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|GridsAreEditable|
|:----|:----|
|Tag Format:|`[GridsAreEditable:Value]`|
|Description:|This tag allows you to specify if the grids in your SpawnGroups are editable. If this tag is set to false, the grid(s) will not be able to be welded, grinded, or have blocks added/removed.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`true`|
|Multiple Tag Allowed:|No|

<!-- GridsAreDestructable  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|GridsAreDestructable|
|:----|:----|
|Tag Format:|`[GridsAreDestructable:Value]`|
|Description:|This tag allows you to specify if the grids in your SpawnGroups are destructable. If this tag is set to false, the grid(s) will not be able to take damage from weapons or collisions.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`true`|
|Multiple Tag Allowed:|No|


# Inventory

<!-- AssignContainerTypesToAllCargo  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AssignContainerTypesToAllCargo|
|:----|:----|
|Tag Format:|`[AssignContainerTypesToAllCargo:Value1,Value2,etc]`|
|Description:|This tag allows you to specify one or more ContainerType SubtypeIds that will be applied to all Cargo Containers on the grid. This allows you to easily setup randomized loot for your SpawnGroups. If multiple values are provided, then each cargo container will choose a ContainerType Id at random.|
|Allowed Values:|ContainerType SubtypeId<br>If providing multiple values, use comma with no spaces as shown in `Tag Format`|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|No|

<!-- UseContainerTypeAssignment  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseContainerTypeAssignment|
|:----|:----|
|Tag Format:|`[UseContainerTypeAssignment:Value]`|
|Description:|This tag allows you to enable replacing ContainerType assignments on blocks matching the criteria specified in the `ContainerTypeAssignmentReference` tag.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|No|

<!-- ContainerTypeAssignBlockName  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ContainerTypeAssignBlockName|
|:----|:----|
|Tag Format:|`[ContainerTypeAssignBlockName:Value]`|
|Description:|This tag allows you to specify the name of container block(s) that will be assigned a ContainerType SubtypeId if `UseContainerTypeAssignment` is enabled. If using this tag, you must also provide a `ContainerTypeAssignSubtypeId` tag that specifies the SubtypeId that is assigned to the block(s)|
|Allowed Values:|Any Container Block Name|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|Yes|

<!-- ContainerTypeAssignSubtypeId  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ContainerTypeAssignSubtypeId|
|:----|:----|
|Tag Format:|`[ContainerTypeAssignSubtypeId:Value]`|
|Description:|This tag allows you to specify the SubtypeId of a ContainerType that is assigned to container block(s) if `UseContainerTypeAssignment` is enabled. If using this tag, you must also provide a `ContainerTypeAssignBlockName` tag that specifies the name of container block(s) that will be assigned the SubtypeId.|
|Allowed Values:|Any ContainerType SubtypeId|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|Yes|

<!-- UseLootProfiles:true-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseLootProfiles:true|
|:----|:----|
|Tag Format:|`[UseLootProfiles:true:Value]`|
|Description:|This tag allows provided Loot Profiles and Loot Profile Groups to be run while using this manipulation.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- ClearGridInventories-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ClearGridInventories|
|:----|:----|
|Tag Format:|`[ClearGridInventories:Value]`|
|Description:|This tag allows you to completely erase any pre-exisiting inventory items that were stored in SpawnGroup prefabs when they were exported.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- LootProfiles-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LootProfiles|
|:----|:----|
|Tag Format:|`[LootProfiles:Value]`|
|Description:|This tag allows you to specify one or more Loot Profiles that will be applied to the grid.|
|Allowed Values:|Any Loot Profile SubtypeId|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|Yes|

<!-- LootGroups-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LootGroups|
|:----|:----|
|Tag Format:|`[LootGroups:Value]`|
|Description:|This tag allows you to specify one or more Loot Profile Groups that will be applied to the grid.|
|Allowed Values:|Any Loot Profile Group SubtypeId|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|Yes|


# Paint-and-Skins

<!-- ShiftBlockColorsHue  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ShiftBlockColorsHue|
|:----|:----|
|Tag Format:|`[ShiftBlockColorsHue:Value]`|
|Description:|This tag allows you to specify if the blocks on a grid should have their block color Hue shifted, to change its color. Can be set to random or an assigned value below.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- RandomHueShift  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RandomHueShift|
|:----|:----|
|Tag Format:|`[RandomHueShift:Value]`|
|Description:|This tag allows you to specify if a random Hue value should be used when `ShiftBlockColorsHue` is enabled. If this property is set to `true`, `ShiftBlockColorAmount` will be ignored.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- ShiftBlockColorAmount  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ShiftBlockColorAmount|
|:----|:----|
|Tag Format:|`[ShiftBlockColorAmount:Value]`|
|Description:|This tag allows you to specify the amount of Hue that block colors are shifted by.|
|Allowed Values:|Any Number Between `-360` and `360`|
|Default Value(s):|`0`|
|Multiple Tag Allowed:|No|

<!-- AssignGridSkin  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AssignGridSkin|
|:----|:----|
|Tag Format:|`[AssignGridSkin:Value1,Value2,Value3,etc]`|
|Description:|This tag allows you to specify one or more Armor Skin IDs to be used to reskin the entire grid. If more than 1 is provided, then a random skin from the provided values is used.|
|Allowed Values:|Armor Skin SubtypeId<br>If providing multiple values, use comma with no spaces as shown in `Tag Format`|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|No|

<!-- RecolorGrid  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RecolorGrid|
|:----|:----|
|Tag Format:|`[RecolorGrid:Value]`|
|Description:|This tag allows you to specify if the grid should be recolored and/or reskinned based on the values you provide in the next two tags..|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- RecolorOld  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RecolorOld|
|:----|:----|
|Tag Format:|`[RecolorOld:Value]`|
|Description:|This tag allows you to specify a block color on a grid that will be replaced with another color if `RecolorGrid` is enabled. If using this tag, you must also provide a `RecolorNew` tag that specifies the color that replaces the color in this tag.|
|Allowed Values:|ColorMaskHSV in Below Format:<br>`{X:0 Y:0 Z:0}`|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|Yes|

<!-- RecolorNew  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RecolorNew|
|:----|:----|
|Tag Format:|`[RecolorNew:Value]`|
|Description:|This tag allows you to specify a block color that will replace another specified color `RecolorGrid` is enabled. If using this tag, you must also provide a `RecolorOld` tag that specifies the color that will be replaced.|
|Allowed Values:|ColorMaskHSV in Below Format:<br>`{X:0 Y:0 Z:0}`|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|Yes|

<!-- ReskinTarget  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReskinTarget|
|:----|:----|
|Tag Format:|`[ReskinTarget:Value]`|
|Description:|This tag allows you to specify a block color on a grid that will be re-skinned if `RecolorGrid` is enabled. If using this tag, you must also provide a `ReskinTexture` tag that specifies the skin name that is applied to blocks with the color in this tag.|
|Allowed Values:|ColorMaskHSV in Below Format:<br>`{X:0 Y:0 Z:0}`|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|Yes|

<!-- ReskinTexture  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReskinTexture|
|:----|:----|
|Tag Format:|`[ReskinTexture:Value]`|
|Description:|This tag allows you to specify a skin name that will be applied to blocks of a specified color if `RecolorGrid` is enabled. If using this tag, you must also provide a `ReskinTarget` tag that specifies the color of blocks that will be re-skinned.|
|Allowed Values:|Any Block Skin Name|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|Yes|

<!-- SkinRandomBlocks-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SkinRandomBlocks|
|:----|:----|
|Tag Format:|`[SkinRandomBlocks:Value]`|
|Description:|This tag allows you to specify if the grid should have a random selection of blocks reskinned with a selection of Armor Skins defined in the `SkinRandomBlocksTextures` tags.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- SkinRandomBlocksTextures-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SkinRandomBlocksTextures|
|:----|:----|
|Tag Format:|`[SkinRandomBlocksTextures:Value]`|
|Description:|This tag allows you to specify one or more Armor Skin SubtypeIds that will be used to skin a random selection of blocks. The skins provided to this tag are selected at random.|
|Allowed Values:|Valid Armor Skin SubtypeID|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!-- MinPercentageSkinRandomBlocks-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinPercentageSkinRandomBlocks|
|:----|:----|
|Tag Format:|`[MinPercentageSkinRandomBlocks:Value]`|
|Description:|This tag allows you to specify the minimum amount of blocks that should be affected by percentage if `SkinRandomBlocks` is enabled.|
|Allowed Values:|Any Number Equal or Greater Than `0`<br>Value should be Lower than `MaxPercentageSkinRandomBlocks` value|
|Default Value(s):|`10`|
|Multiple Tag Allowed:|No|

<!-- MaxPercentageSkinRandomBlocks-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxPercentageSkinRandomBlocks|
|:----|:----|
|Tag Format:|`[MaxPercentageSkinRandomBlocks:Value]`|
|Description:|This tag allows you to specify the minimum amount of blocks that should be affected by percentage if `SkinRandomBlocks` is enabled.|
|Allowed Values:|Any Number Equal or Greater Than `0`<br>Value should be Higher than `MinPercentageSkinRandomBlocks` value|
|Default Value(s):|`40`|
|Multiple Tag Allowed:|No|


# Profiles

<!-- UseGridDereliction-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseGridDereliction|
|:----|:----|
|Tag Format:|`[UseGridDereliction:Value]`|
|Description:|This tag allows you to specify whether or not Grid Dereliction Profiles should be applied to a prefab.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- DerelictionProfiles-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DerelictionProfiles|
|:----|:----|
|Tag Format:|`[DerelictionProfiles:Value]`|
|Description:|This tag allows you to specify one or more Dereliction Profile SubtypeIds that will be used to adjust the build states of various blocks on the prefabs in the SpawnGroup.|
|Allowed Values:|Valid Dereliction Profile SubtypeID|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|


# Propulsion

<!-- ConfigureSpecialNpcThrusters  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ConfigureSpecialNpcThrusters|
|:----|:----|
|Tag Format:|`[ConfigureSpecialNpcThrusters:Value]`|
|Description:|This tag allows you to specify if special NPC thrusters (added to prefab via the spawner built-in block replacement profiles) should be configured with extra parameters. Please note you must ensure that the NPC Thrusters are added to your grid either by Block Replacement or manually, since this tag does not add them automatically.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- RestrictNpcIonThrust  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RestrictNpcIonThrust|
|:----|:----|
|Tag Format:|`[RestrictNpcIonThrust:Value]`|
|Description:|This tag allows you to specify if NPC Ion Thrusters (added via built-in block replacement) should be disabled if the grid is owned by a player identity if the `ConfigureSpecialNpcThrusters` tag is used.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- NpcIonThrustForceMultiply  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|NpcIonThrustForceMultiply|
|:----|:----|
|Tag Format:|`[NpcIonThrustForceMultiply:Value]`|
|Description:|This tag allows you to specify the Force Multiplier of NPC Ion Thrusters (added via built-in block replacement) if the `ConfigureSpecialNpcThrusters` tag is used. Eg: '2' would double force, while `0.5` would cut it in half.|
|Allowed Values:|Any Number Equal or Greater Than `0`|
|Default Value(s):|`1`|
|Multiple Tag Allowed:|No|

<!-- NpcIonThrustPowerMultiply  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|NpcIonThrustPowerMultiply|
|:----|:----|
|Tag Format:|`[NpcIonThrustPowerMultiply:Value]`|
|Description:|This tag allows you to specify the Power Multiplier of NPC Ion Thrusters (added via built-in block replacement) if the `ConfigureSpecialNpcThrusters` tag is used. Eg: '2' would double required power, while `0.5` would cut it in half.|
|Allowed Values:|Any Number Equal or Greater Than `0`|
|Default Value(s):|`1`|
|Multiple Tag Allowed:|No|

<!-- RestrictNpcAtmoThrust  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RestrictNpcAtmoThrust|
|:----|:----|
|Tag Format:|`[RestrictNpcAtmoThrust:Value]`|
|Description:|This tag allows you to specify if NPC Atmospheric Thrusters (added via built-in block replacement) should be disabled if the grid is owned by a player identity if the `ConfigureSpecialNpcThrusters` tag is used.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- NpcAtmoThrustForceMultiply  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|NpcAtmoThrustForceMultiply|
|:----|:----|
|Tag Format:|`[NpcAtmoThrustForceMultiply:Value]`|
|Description:|This tag allows you to specify the Force Multiplier of NPC Atmospheric Thrusters (added via built-in block replacement) if the `ConfigureSpecialNpcThrusters` tag is used. Eg: '2' would double force, while `0.5` would cut it in half.|
|Allowed Values:|Any Number Equal or Greater Than `0`|
|Default Value(s):|`1`|
|Multiple Tag Allowed:|No|

<!-- NpcAtmoThrustPowerMultiply  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|NpcAtmoThrustPowerMultiply|
|:----|:----|
|Tag Format:|`[NpcAtmoThrustPowerMultiply:Value]`|
|Description:|This tag allows you to specify the Power Multiplier of NPC Atmospheric Thrusters (added via built-in block replacement) if the `ConfigureSpecialNpcThrusters` tag is used. Eg: '2' would double required power, while `0.5` would cut it in half.|
|Allowed Values:|Any Number Equal or Greater Than `0`|
|Default Value(s):|`1`|
|Multiple Tag Allowed:|No|

<!-- RestrictNpcHydroThrust  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RestrictNpcHydroThrust|
|:----|:----|
|Tag Format:|`[RestrictNpcHydroThrust:Value]`|
|Description:|This tag allows you to specify if NPC Hydrogen Thrusters (added via built-in block replacement) should be disabled if the grid is owned by a player identity if the `ConfigureSpecialNpcThrusters` tag is used.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- NpcHydroThrustForceMultiply  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|NpcHydroThrustForceMultiply|
|:----|:----|
|Tag Format:|`[NpcHydroThrustForceMultiply:Value]`|
|Description:|This tag allows you to specify the Force Multiplier of NPC Hydrogen Thrusters (added via built-in block replacement) if the `ConfigureSpecialNpcThrusters` tag is used. Eg: '2' would double force, while `0.5` would cut it in half.|
|Allowed Values:|Any Number Equal or Greater Than `0`|
|Default Value(s):|`1`|
|Multiple Tag Allowed:|No|

<!-- NpcHydroThrustPowerMultiply  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|NpcHydroThrustPowerMultiply|
|:----|:----|
|Tag Format:|`[NpcHydroThrustPowerMultiply:Value]`|
|Description:|This tag allows you to specify the Power Multiplier of NPC Hydrogen Thrusters (added via built-in block replacement) if the `ConfigureSpecialNpcThrusters` tag is used. Eg: '2' would double required power, while `0.5` would cut it in half.|
|Allowed Values:|Any Number Equal or Greater Than `0`|
|Default Value(s):|`1`|
|Multiple Tag Allowed:|No|

<!-- SetNpcGyroscopeMultiplier-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SetNpcGyroscopeMultiplier|
|:----|:----|
|Tag Format:|`[SetNpcGyroscopeMultiplier:Value]`|
|Description:|This tag allows you to specify if Gyroscopes on the NPC Grid should have a custom force multiplier applied to them.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- NpcGyroscopeMultiplier-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|NpcGyroscopeMultiplier|
|:----|:----|
|Tag Format:|`[NpcGyroscopeMultiplier:Value]`|
|Description:|This tag allows you to specify the force multipler that is applied to Gyroscopes if using the `SetNpcGyroscopeMultiplier` tag.|
|Allowed Values:|Any Number Greater Than `0`|
|Default Value(s):|`1`|
|Multiple Tag Allowed:|No|


# Renaming

<!-- UseRandomNameGenerator  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseRandomNameGenerator|
|:----|:----|
|Tag Format:|`[UseRandomNameGenerator:Value]`|
|Description:|This tag will allow you to utilize the Random Name Generator built into the Spawner for your SpawnGroup. Keep in mind that this will affect all prefabs in your SpawnGroup, and they may all have different naming as a result.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- RandomGridNamePrefix  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RandomGridNamePrefix|
|:----|:----|
|Tag Format:|`[RandomGridNamePrefix:Value]`|
|Description:|This tag allows you to specify a prefix that is added to the beginning of the grid name, mostly to identify your grid after it's been renamed (eg, `(NPC-CPC)` , `etc`).|
|Allowed Values:|Any String Value|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|No|

<!-- RandomGridNamePattern  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RandomGridNamePattern|
|:----|:----|
|Tag Format:|`[RandomGridNamePattern:Value]`|
|Description:|This tag allows you to specify a pattern that the Random Name Generator should follow. [Please click here for a guide to these patterns](https://gist.github.com/MeridiusIX/8888bbc06a623cac90f8362dd948033c).|
|Allowed Values:|Any Grid Name Pattern|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|No|

<!-- ReplaceAntennaNameWithRandomizedName  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReplaceAntennaNameWithRandomizedName|
|:----|:----|
|Tag Format:|`[ReplaceAntennaNameWithRandomizedName:Value]`|
|Description:|This tag gives you the option to replace the Block Name of an antenna with the newly generated Random Name (without the Prefix, if specified). Value is replaced with the name of the Antenna Block you want to change.|
|Allowed Values:|Existing Antenna Block Name|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|No|

<!-- UseBlockNameReplacer  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseBlockNameReplacer|
|:----|:----|
|Tag Format:|`[UseBlockNameReplacer:Value]`|
|Description:|This tag allows you to enable whether or not block names should be replaced with the reference specified in the `BlockNameReplacerReference` tag.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|No|

<!-- ReplaceBlockNameOld  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReplaceBlockNameOld|
|:----|:----|
|Tag Format:|`[ReplaceBlockNameOld:Value]`|
|Description:|This tag allows you to specify the name of block(s) that will be renamed to something else if `UseBlockNameReplacer` is enabled. If using this tag, you must also provide a `ReplaceBlockNameNew` tag that specifies the new name the block(s) will use|
|Allowed Values:|Any Block Name|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|Yes|

<!-- ReplaceBlockNameNew  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReplaceBlockNameNew|
|:----|:----|
|Tag Format:|`[ReplaceBlockNameNew:Value]`|
|Description:|This tag allows you to specify the name that block(s) will be renamed to if `UseBlockNameReplacer` is enabled. If using this tag, you must also provide a `ReplaceBlockNameOld` tag that specifies the name of block(s) that will be renamed.|
|Allowed Values:|Any Block Name|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|Yes|

<!-- ProcessBlocksForCustomGridName -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ProcessBlocksForCustomGridName|
|:----|:----|
|Tag Format:|`[ProcessBlocksForCustomGridName:Value]`|
|Description:|This tag allows you to specify if the blocks on the grid should be scanned for `{GridName}` in their name and have it replaced with the randomized name generated if using `UseRandomNameGenerator`.|
|Allowed Value(s):|`true`<br />`false`|
|Default Value(s):|`false`|
|Multiple Tags Allowed:|No|


#RivalAI

<!-- UseRivalAi  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseRivalAi|
|:----|:----|
|Tag Format:|`[UseRivalAi:Value]`|
|Description:|This tag allows you to specify if a RivalAI Behavior Profile should be used in the prefabs included in your SpawnGroup. With this tag being set to `true`, the spawner will look for a RivalAI Behavior with a SubtypeId matching what was provided in the Prefab `<Behaviour>` tags, and will automatically add it to a valid RivalAI Remote Control block.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- RivalAiReplaceRemoteControl  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RivalAiReplaceRemoteControl|
|:----|:----|
|Tag Format:|`[RivalAiReplaceRemoteControl:Value]`|
|Description:|This tag allows you to specify if the first valid Vanilla Remote Control block in each Prefab should be replaced with a RivalAI Remote Control.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- ApplyBehaviorToNamedBlock  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ApplyBehaviorToNamedBlock|
|:----|:----|
|Tag Format:|`[ApplyBehaviorToNamedBlock:Value]`|
|Description:|This tag allows you to specify the specific block name of a Remote Control block that you want to have used/replaced with a RivalAI control block.|
|Allowed Values:|Any Remote Control Block Name|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|No|

<!-- ConvertAllRemoteControlBlocks  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ConvertAllRemoteControlBlocks|
|:----|:----|
|Tag Format:|`[ConvertAllRemoteControlBlocks:Value]`|
|Description:|This tag allows you to specify if all remote control blocks should be converted to RivalAI control blocks. Only the first or primary (or named, if you use the option above) block will receive the behavior specified in your prefab, other blocks will need to use RivalAI Trigger/Action to assign behaviors to them.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|


# Shields

<!-- AddDefenseShieldBlocks  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AddDefenseShieldBlocks|
|:----|:----|
|Tag Format:|`[AddDefenseShieldBlocks:Value]`|
|Description:|This tag let's you add Defense Shield Blocks to the prefabs if the Defense Shields mod is loaded.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- IgnoreShieldProviderMod  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IgnoreShieldProviderMod|
|:----|:----|
|Tag Format:|`[IgnoreShieldProviderMod:Value]`|
|Description:|This tag let's you lets you specify if the grid should not receive Defense Shields if the `NPC Defense Shields Provider` Mod or the `EnableGlobalNPCShieldProvider` config option is enabled.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|


# Turrets

<!-- ChangeTurretSettings  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeTurretSettings|
|:----|:----|
|Tag Format:|`[ChangeTurretSettings:Value]`|
|Description:|This tag allows you to specify if turret settings should be set by the following SpawnGroup settings. Please note if you decide to use this setting, you should specify each setting listed below in your tags so default spawner values don't interfere with your intended settings.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- TurretRange  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TurretRange|
|:----|:----|
|Tag Format:|`[TurretRange:Value]`|
|Description:|This tag allows you to specify the range of all turrets. If value is higher than allowed range, it will use the max range of that turret.|
|Allowed Values:|Any Number Greater Than `0`|
|Default Value(s):|`800`|
|Multiple Tag Allowed:|No|

<!-- TurretIdleRotation  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TurretIdleRotation|
|:----|:----|
|Tag Format:|`[TurretIdleRotation:Value]`|
|Description:|This tag allows you to specify if turret idle rotation should be enabled.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- TurretTargetMeteors  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TurretTargetMeteors|
|:----|:----|
|Tag Format:|`[TurretTargetMeteors:Value]`|
|Description:|This tag allows you to specify if turrets targeting meteors should be enabled.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`true`|
|Multiple Tag Allowed:|No|

<!-- TurretTargetMissiles  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TurretTargetMissiles|
|:----|:----|
|Tag Format:|`[TurretTargetMissiles:Value]`|
|Description:|This tag allows you to specify if turrets targeting missiles should be enabled.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`true`|
|Multiple Tag Allowed:|No|

<!-- TurretTargetCharacters  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TurretTargetCharacters|
|:----|:----|
|Tag Format:|`[TurretTargetCharacters:Value]`|
|Description:|This tag allows you to specify if turrets targeting characters should be enabled.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`true`|
|Multiple Tag Allowed:|No|

<!-- TurretTargetSmallGrids  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TurretTargetSmallGrids|
|:----|:----|
|Tag Format:|`[TurretTargetSmallGrids:Value]`|
|Description:|This tag allows you to specify if turrets targeting small grids should be enabled.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`true`|
|Multiple Tag Allowed:|No|

<!-- TurretTargetLargeGrids  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TurretTargetLargeGrids|
|:----|:----|
|Tag Format:|`[TurretTargetLargeGrids:Value]`|
|Description:|This tag allows you to specify if turrets targeting large grids should be enabled.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`true`|
|Multiple Tag Allowed:|No|

<!-- TurretTargetStations  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TurretTargetStations|
|:----|:----|
|Tag Format:|`[TurretTargetStations:Value]`|
|Description:|This tag allows you to specify if turrets targeting static grids should be enabled.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`true`|
|Multiple Tag Allowed:|No|

<!-- TurretTargetNeutrals  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TurretTargetNeutrals|
|:----|:----|
|Tag Format:|`[TurretTargetNeutrals:Value]`|
|Description:|This tag allows you to specify if turrets targeting neutrals should be enabled.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`true`|
|Multiple Tag Allowed:|No|


# Weapon-Randomization

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RandomizeWeapons|
|:----|:----|
|Tag Format:|`[RandomizeWeapons:Value]`|
|Description:|This tag specifies if the prefabs in a SpawnGroup should have their weapons randomized (similar behavior to the NPC Weapon Upgrades mod). If set to `true`, then the behavior provided by the `ReplenishSystems` tag will be applied as well.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- IgnoreWeaponRandomizerMod  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IgnoreWeaponRandomizerMod|
|:----|:----|
|Tag Format:|`[IgnoreWeaponRandomizerMod:Value]`|
|Description:|This tag allows you to control whether or not a SpawnGroup will have randomized weapons if the `NPC Weapons Upgrade` mod is enabled. If set to `true`, your SpawnGroup will not have its weapons randomized (unless you've also enabled the `RandomizeWeapons` option).|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- WeaponRandomizerBlacklist  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|WeaponRandomizerBlacklist|
|:----|:----|
|Tag Format:|`[WeaponRandomizerBlacklist:Value1,Value2,Value3,etc]`|
|Description:|This tag allows you to specify weapons that will not be used in Weapon Randomization for your SpawnGroup. Weapons are identified by their MyDefintionId or Steam Workshop Mod ID . A MyDefinitionId uses the following `TypeId/SubtypeId` format - eg: `MyObjectBuilder_SmallMissileLauncher/LargeMissileLauncher`.|
|Allowed Values:|MyDefinitionId or Steam Workshop ID<br>If providing multiple values, use comma with no spaces as shown in `Tag Format`|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|No|

<!-- WeaponRandomizerWhitelist  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|WeaponRandomizerWhitelist|
|:----|:----|
|Tag Format:|`[WeaponRandomizerWhitelist:Value1,Value2,Value3,etc]`|
|Description:|This tag allows you to specify weapons that will only be used in Weapon Randomization for your SpawnGroup. Weapons are identified by their MyDefintionId or Steam Workshop Mod ID . A MyDefinitionId uses the following `TypeId/SubtypeId` format - eg: `MyObjectBuilder_SmallMissileLauncher/LargeMissileLauncher`.|
|Allowed Values:|MyDefinitionId or Steam Workshop ID<br>If providing multiple values, use comma with no spaces as shown in `Tag Format`|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|No|

<!-- IgnoreWeaponRandomizerTargetGlobalBlacklist  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IgnoreWeaponRandomizerTargetGlobalBlacklist|
|:----|:----|
|Tag Format:|`[IgnoreWeaponRandomizerTargetGlobalBlacklist:Value]`|
|Description:|If set to `true`, this will allow your SpawnGroup to ignore the Global Weapon Randomizer Target Blacklist (ie, which weapons are not allowed to be replaced) when Weapon Randomization is enabled.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- IgnoreWeaponRandomizerTargetGlobalWhitelist  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IgnoreWeaponRandomizerTargetGlobalWhitelist|
|:----|:----|
|Tag Format:|`[IgnoreWeaponRandomizerTargetGlobalWhitelist:Value]`|
|Description:|If set to `true`, this will allow your SpawnGroup to ignore the Global Weapon Randomizer Target Whitelist (ie, which weapons are only allowed to be replaced) when Weapon Randomization is enabled.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- WeaponRandomizerTargetBlacklist  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|WeaponRandomizerTargetBlacklist|
|:----|:----|
|Tag Format:|`[WeaponRandomizerTargetBlacklist:Value1,Value2,Value3,etc]`|
|Description:|This tag allows you to specify weapons that will not be replaced with Weapon Randomization for your SpawnGroup. Weapons are identified by their MyDefintionId or Steam Workshop Mod ID . A MyDefinitionId uses the following `TypeId/SubtypeId` format - eg: `MyObjectBuilder_SmallMissileLauncher/LargeMissileLauncher`.|
|Allowed Values:|MyDefinitionId or Steam Workshop ID<br>If providing multiple values, use comma with no spaces as shown in `Tag Format`|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|No|

<!-- WeaponRandomizerTargetWhitelist  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|WeaponRandomizerTargetWhitelist|
|:----|:----|
|Tag Format:|`[WeaponRandomizerTargetWhitelist:Value1,Value2,Value3,etc]`|
|Description:|This tag allows you to specify weapons that will only be replaced with Weapon Randomization for your SpawnGroup. Weapons are identified by their MyDefintionId or Steam Workshop Mod ID . A MyDefinitionId uses the following `TypeId/SubtypeId` format - eg: `MyObjectBuilder_SmallMissileLauncher/LargeMissileLauncher`.|
|Allowed Values:|MyDefinitionId or Steam Workshop ID<br>If providing multiple values, use comma with no spaces as shown in `Tag Format`|
|Default Value(s):|`none`|
|Multiple Tag Allowed:|No|
