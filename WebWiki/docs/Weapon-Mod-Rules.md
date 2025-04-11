#Weapon-Mod-Rules.md

Weapon Mod Rules Profiles in Modular Encounters Systems are used by mod authors to restrict their weapons from appearing on grids during weapon randomization. Some reasons they may want to do this is because a weapon is not suitable to use for combat (designator turret), or a weapon doesn't fit well with the rest of the grid when randomly placed (turrets with passage/crew areas beneath them).

These profiles are not attached to SpawnGroups, SpawnGroup Conditions, or Manipulation Profiles. If they are detected in the world, then they will apply to all grids spawned with randomized weapons.

Here is an example of how a Weapon Mod Rules Profile definition is setup:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>MES-ExampleWeaponRules</SubtypeId>
      </Id>
      <Description>

      [MES Weapon Mod Rules]
      [WeaponBlock:MyObjectBuilder_LargeInteriorTurret/SomeModdedWeapon]
      [AllowInRandomization:false]
      
      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

***

Below are the tags you are able to use in your Weapon Mod Rules Profiles.  

<!--WeaponBlock-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|WeaponBlock|
|:----|:----|
|Tag Format:|`[WeaponBlock:Value]`|
|Description:|This tag specifies the weapon that you want to apply the profile rules to.|
|Allowed Values:|Any Weapon Block MyDefinitionId<br />eg: `MyObjectBuilder_InteriorTurret/SomeModdedWeapon`|
|Multiple Tag Allowed:|No|

<!--AllowInRandomization-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AllowInRandomization|
|:----|:----|
|Tag Format:|`[AllowInRandomization:Value]`|
|Description:|This tag specifies if the weapon is allowed to be used in Weapon Randomization at all.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--AllowIfNonPublic-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AllowIfNonPublic|
|:----|:----|
|Tag Format:|`[AllowIfNonPublic:Value]`|
|Description:|This tag specifies if the weapon is allowed to be used in Weapon Randomization even if it is not listed as a public block (ie: doesn't appear in G-Menu).|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--AllowOnlyIfExactSize-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AllowOnlyIfExactSize|
|:----|:----|
|Tag Format:|`[AllowOnlyIfExactSize:Value]`|
|Description:|This tag specifies if the weapon being chosen for weapon randomization must be an exact size match.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--AllowedTargetBlocks-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AllowedTargetBlocks|
|:----|:----|
|Tag Format:|`[AllowedTargetBlocks:Value]`|
|Description:|This tag specifies one or more types of blocks that the specified weapon is only allowed to replace.|
|Allowed Values:|Any Weapon Block MyDefinitionId<br />eg: `MyObjectBuilder_InteriorTurret/LargeInteriorTurret`|
|Multiple Tag Allowed:|Yes|

<!--RestrictedTargetBlocks-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RestrictedTargetBlocks|
|:----|:----|
|Tag Format:|`[RestrictedTargetBlocks:Value]`|
|Description:|This tag specifies one or more types of blocks that the specified weapon are never allowed to replace.|
|Allowed Values:|Any Weapon Block MyDefinitionId<br />eg: `MyObjectBuilder_InteriorTurret/LargeInteriorTurret`|
|Multiple Tag Allowed:|Yes|

<!--  -->
