#Spawn-Conditions-Groups.md

Spawn Conditions Group Profiles in Modular Encounters Systems are used to group together multiple Spawn Conditions names so they can easily be applied to SpawnGroups that use those conditions often (reducing visual clutter and copy/paste activity). You can attach your Spawn Conditions Group Profiles to any SpawnGroup by adding a `[SpawnConditionGroups:Value]` tag to the SpawnGroup and replace `Value` with the SubtypeId of your Spawn Conditions Group Profile. Example:

`[SpawnConditionGroups:MES-ExampleSpawnConditionGroupsProfile]`

Multiple Spawn Conditions Group Profiles can be attached to a single SpawnGroup as well, just include additional `[SpawnConditionGroups:Value]` lines in your SpawnGroup Profile. It is important that you use a unique SubtypeId for each Spawn Conditions Group Profile you create, otherwise they may not work correctly.

Here is an example of how a Spawn Conditions Group Profile definition is setup:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>MES-ExampleSpawnConditionGroupsProfile</SubtypeId>
      </Id>
      <Description>

      [MES Spawn Conditions Group]
      [SpawnConditionProfiles:SomeSpawnConditionProfileId]
      [SpawnConditionProfiles:AnotherSpawnConditionProfileId]
      
      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

***

Below are the tags you are able to use in your Spawn Conditions Group Profiles.  

<!--SpawnConditionProfiles-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SpawnConditionProfiles|
|:----|:----|
|Tag Format:|`[SpawnConditionProfiles:Value]`|
|Description:|This tag specifies the name of Spawn Conditions profile this group should use.|
|Allowed Values:|Any Spawn Conditions Profile SubtypeId|
|Multiple Tag Allowed:|Yes|

<!--  -->
