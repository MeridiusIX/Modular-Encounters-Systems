#Waypoint.md

Waypoint Profiles in RivalAI allow you to specify rules for how a Waypoint or other sets of coordinates are generated.

It is important that you use a unique SubtypeId for each Waypoint Profile you create, otherwise they may not work correctly.

Here's an example of how a Waypoint Profile Definition is setup:  

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>RAI-ExampleWaypointProfile</SubtypeId>
      </Id>
      <Description>

      [RivalAI Waypoint]
      
      [Waypoint:RelativeRandom]
      [RelativeEntity:Self]

      [MinDistance:100]
      [MaxDistance:200]
      [MinAltitude:100]
      [MaxAltitude:200]

      [InheritRelativeAltitude:true]
      
      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

Below are the tags you are able to use in your Waypoint Profiles.

***

<!--Waypoint-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Waypoint|
|:----|:----|
|Tag Format:|`[Waypoint:Value]`|
|Description:|This tag specifies the type of Waypoint that will be generated. `Static` types are hard-coded to the world. `Relative` types use an entity in the world as reference at creation. `Entity` types also use an entity in the world as reference, but also updates with that entity if the entity position changes.|
|Allowed Values:|`Static`<br>`StaticRandom`<br>`RelativeOffset`<br>`RelativeRandom`<br>`EntityOffset`<br>`EntityRandom`|
|Multiple Tag Allowed:|No|

<!--RelativeEntity-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RelativeEntity|
|:----|:----|
|Tag Format:|`[RelativeEntity:Value]`|
|Description:|This tag specifies waht entity to use as reference if you've used a `Relative` or `Entity` type of waypoint.|
|Allowed Values:|`Self`<br>`Target`<br>`Damager`|
|Multiple Tag Allowed:|No|

<!--Coordinates-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Coordinates|
|:----|:----|
|Tag Format:|`[Coordinates:Value]`|
|Description:|This tag specifies the coordinates in the game world you want to use if you've chosen a `Static` waypoint type.|
|Allowed Values:|Vector3D Value. Eg:<br>`{X:100 Y:100 Z:100}`|
|Multiple Tag Allowed:|No|

<!--Offset-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Offset|
|:----|:----|
|Tag Format:|`[Offset:Value]`|
|Description:|This tag specifies the an offset vector that is applied to an entity position if using a `Relative` or `Entity` waypoint type. `X` shifts to the right, `Y` shifts upward, and `Z` shifts forward.|
|Allowed Values:|Vector3D Value. Eg:<br>`{X:100 Y:100 Z:100}`|
|Multiple Tag Allowed:|No|

<!--MinDistance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinDistance|
|:----|:----|
|Tag Format:|`[MinDistance:Value]`|
|Description:|This tag specifies the Minimum Distance from the initial coordinates that the waypoint will be created at if using any `Random` waypoint type.|
|Allowed Values:|Any Number Greater Than `0`<br>Less than `MaxDistance`|
|Multiple Tag Allowed:|No|

<!--MaxDistance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxDistance|
|:----|:----|
|Tag Format:|`[MaxDistance:Value]`|
|Description:|This tag specifies the Maximum Distance from the initial coordinates that the waypoint will be created at if using any `Random` waypoint type.|
|Allowed Values:|Any Number Greater Than `0`<br>Greater than `MinDistance`|
|Multiple Tag Allowed:|No|

<!--MinAltitude-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinAltitude|
|:----|:----|
|Tag Format:|`[MinAltitude:Value]`|
|Description:|This tag specifies the Minimum Altitude from the initial coordinates that the waypoint will be created at if using any `Random` waypoint type. This is only used if the coordinates are being created in natural gravity.|
|Allowed Values:|Any Number Greater Than `0`<br>Less than `MaxAltitude`|
|Multiple Tag Allowed:|No|

<!--MaxAltitude-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxAltitude|
|:----|:----|
|Tag Format:|`[MaxAltitude:Value]`|
|Description:|This tag specifies the Maximum Altitude from the initial coordinates that the waypoint will be created at if using any `Random` waypoint type. This is only used if the coordinates are being created in natural gravity.|
|Allowed Values:|Any Number Greater Than `0`<br>Greater than `MinAltitude`|
|Multiple Tag Allowed:|No|

<!--InheritRelativeAltitude-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|InheritRelativeAltitude|
|:----|:----|
|Tag Format:|`[InheritRelativeAltitude:Value]`|
|Description:|This tag specifies if the waypoint should inherit the altitude of the reference coordinates when using a `Random` type waypoint.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|
