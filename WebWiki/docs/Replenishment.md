#Replenishment.md

Replenishment profiles in the Modular Encounter Systems mod allow you to define rules for how grids inventories will be treated while using the `[ReplenishSystems:true]` tag in your SpawnGroups. With these profiles, you can define limits for certain item types to prevent overfilling of resources like fuel or ammo.

Here is an example of how a Replenishment Profile is setup:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>MES-ExampleReplenishment</SubtypeId>
      </Id>
      <Description>

        [MES Replenishment]

        [RestrictedItems:MyObjectBuilder_AmmoMagazine/NATO_5p56x45mm]

        [MaxItemId:MyObjectBuilder_AmmoMagazine/RapidFireAutomaticRifleGun_Mag_50rd]
        [MaxItemAmount:50]
      
      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

The above would restrict the old ammo magazine interior turrets used and limit the newer magazines to only 50 magazines per turret.

To add a Replenishment profile to your SpawnGroup, include the tag: `[ReplenishProfiles:MES-ExampleReplenishment]` in the SpawnGroup itself. You can also include multiple instances of Replenishment profiles as well.

Below you can find all the tags that can be used in your Replenishment Profile:

<!-- RestrictedItems-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RestrictedItems|
|:----|:----|
|Tag Format:|`[RestrictedItems:Value]`|
|Description:|This tag specifies DefinitionIds of items that will not be used to replenish any inventories.|
|Allowed Values:|Any item MyDefinitionId<br />eg: `MyObjectBuilder_AmmoMagazine/NATO_5p56x45mm`|
|Multiple Tag Allowed:|Yes|

<!-- MaxItemId-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxItemId|
|:----|:----|
|Tag Format:|`[MaxItemId:Value]`|
|Description:|This tag specifies DefinitionIds of items that will have a maximum amount added to inventories (this is not a minimum). Each instance of this tag should be paired with a `MaxItemAmount` tag.|
|Allowed Values:|Any item MyDefinitionId<br />eg: `MyObjectBuilder_AmmoMagazine/NATO_5p56x45mm`|
|Multiple Tag Allowed:|Yes|

<!-- MaxItemAmount-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxItemAmount|
|:----|:----|
|Tag Format:|`[MaxItemAmount:Value]`|
|Description:|This tag specifies the maximum amount of an item that can be added to an inventory. Each instance of this tag should be paired with a `MaxItemId` tag.|
|Allowed Values:|Any Integer Greater or Equal to `0`|
|Multiple Tag Allowed:|Yes|
