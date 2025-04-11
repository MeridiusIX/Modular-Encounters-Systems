#Loot-Profile-Group.md

Loot Group Profiles in Modular Encounters Systems are used to group together multiple Loot Profile names so they can easily be applied to Manipulation Profiles that use those profiles often (reducing visual clutter and copy/paste activity). You can attach your Loot Group Profiles to any Manipulation Profile by adding a `[LootGroups:Value]` tag to the Manipulation Profile and replace `Value` with the SubtypeId of your Loot Profile Group. Example:

`[LootGroups:MES-ExampleLootGroupProfile]`

Multiple Loot Group Profiles can be attached to a single Manipulation Profile as well, just include additional `[LootGroups:Value]` lines in your Manipulation Profile. It is important that you use a unique SubtypeId for each Manipulation Group Profile you create, otherwise they may not work correctly.

Here is an example of how a Loot Group Profile definition is setup:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>MES-ExampleManipulationGroupsProfile</SubtypeId>
      </Id>
      <Description>

      [MES Loot Group]
      [LootProfiles:SomeLootProfileId]
      [LootProfiles:AnotherLootProfileId]
      
      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

***

Below are the tags you are able to use in your Loot Group Profiles.  

<!--LootProfiles-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LootProfiles|
|:----|:----|
|Tag Format:|`[LootProfiles:Value]`|
|Description:|This tag specifies the name of a Loot Profile this group should use.|
|Allowed Values:|Any Loot Profile SubtypeId|
|Multiple Tag Allowed:|Yes|

<!--  -->
