#Manipulation-Groups.md

Manipulation Group Profiles in Modular Encounters Systems are used to group together multiple Manipulation Profile names so they can easily be applied to SpawnGroups that use those manipulations often (reducing visual clutter and copy/paste activity). You can attach your Manipulation Group Profiles to any SpawnGroup by adding a `[ManipulationGroups:Value]` tag to the SpawnGroup and replace `Value` with the SubtypeId of your Manipulation Profile Group. Example:

`[ManipulationGroups:MES-ExampleManipulationGroupsProfile]`

Multiple Manipulation Group Profiles can be attached to a single SpawnGroup as well, just include additional `[ManipulationGroups:Value]` lines in your SpawnGroup Profile. It is important that you use a unique SubtypeId for each Manipulation Group Profile you create, otherwise they may not work correctly.

Here is an example of how a Manipulation Group Profile definition is setup:

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

      [MES Manipulation Group]
      [ManipulationProfiles:SomeManipulationProfileId]
      [ManipulationProfiles:AnotherManipulationProfileId]
      
      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

***

Below are the tags you are able to use in your Manipulation Group Profiles.  

<!--ManipulationProfiles-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManipulationProfiles|
|:----|:----|
|Tag Format:|`[ManipulationProfiles:Value]`|
|Description:|This tag specifies the name of Manipulation Profile this group should use.|
|Allowed Values:|Any Manipulation Profile SubtypeId|
|Multiple Tag Allowed:|Yes|

<!--  -->
