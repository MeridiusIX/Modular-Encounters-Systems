#Datapad.md

Datapad Profiles in Rival AI are used to dynamically add Datapads to NPC Grids without having to manually add them to the grid. In these profiles, you can define the name/title of the Datapad using the `DisplayName` tag, and the body is defined using the `Description` tag.

To add Datapads to your grid, use the Trigger/Action system. Search for the Action tag `AddDatapadsToSeats` to get started.

Because the `Description` is used for the body, these profiles are read a little differently from regular RivalAI Profiles. In order to define these profiles, you MUST include `RivalAI-Datapad` at the beginning of your Profile SubtypeId.

Here is an example of how a Datapad Profile definition is setup:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>RivalAI-Datapad-ExampleDataPad</SubtypeId>
      </Id>

      <DisplayName>Datapad Title Goes Here</DisplayName>
      <Description>

      Datapad Body Text Goes Here

      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```
