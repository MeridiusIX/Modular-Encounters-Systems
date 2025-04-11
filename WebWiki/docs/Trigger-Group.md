#Trigger-Group.md

Trigger Group Profiles in Rival AI are used to group together multiple Trigger Profile names so they can easily be applied to behaviors that use those triggers often (reducing visual clutter and copy/paste activity). You can attach your Trigger Group Profiles to any Behavior Profile by adding a `[TriggerGroups:Value]` tag to the Behavior and replace `Value` with the SubtypeId of your Trigger Group Profile. Example:

`[TriggerGroups:RAI-ExampleTriggerGroupProfile]`

Multiple Trigger Group Profiles can be attached to a single behavior as well, just include additional `[TriggerGroups:Value]` lines in your Behavior Profile. It is important that you use a unique SubtypeId for each Trigger Group Profile you create, otherwise they may not work correctly.

Here is an example of how a Trigger Group Profile definition is setup:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>RAI-ExampleTriggerGroupProfile</SubtypeId>
      </Id>
      <Description>

      [RivalAI TriggerGroup]
      
      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

***

Below are the tags you are able to use in your Trigger Group Profiles.  

<!--Triggers  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Triggers|
|:----|:----|
|Tag Format:|`[Triggers:Value]`|
|Description:|This tag specifies the name of Trigger profile this group should use.|
|Allowed Values:|Any Trigger Profile SubtypeId|
|Multiple Tag Allowed:|Yes|

<!--  -->
