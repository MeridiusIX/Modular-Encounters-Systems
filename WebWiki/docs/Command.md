#Command.md

Command Profiles in RivalAI allow you to specify a group of data that is broadcast from one encounter and then received by other encounters within antenna range (or outside, if you choose).

It is important that you use a unique SubtypeId for each Command Profile you create, otherwise they may not work correctly.

Here's an example of how a Command Profile Definition is setup:  

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>RAI-ExampleCommandProfile</SubtypeId>
      </Id>
      <Description>

      [RivalAI Command]
      
      [CommandCode:TestCode]
      
      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

Below are the tags you are able to use in your Command Profiles.

***

<!--CommandCode-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CommandCode|
|:----|:----|
|Tag Format:|`[CommandCode:Value]`|
|Description:|This tag specifies the code that is broadcasted with the command. Other NPCs must have Trigger Profiles matching this code to be able to receive and process it.|
|Allowed Values:|Any String Value|
|Multiple Tag Allowed:|No|


<!--CommandDelayTicks-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CommandDelayTicks|
|:----|:----|
|Tag Format:|`[CommandDelayTicks:Value]`|
|Description:|This tag specifies if there should be a delay before the command code is sent out from the originating NPC. Delay value is in game ticks (60 ticks = 1 second)|
|Allowed Values:|Any Integer Value|
|Multiple Tag Allowed:|No|

<!--SingleRecipient-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SingleRecipient|
|:----|:----|
|Tag Format:|`[SingleRecipient:Value]`|
|Description:|This tag specifies if the command should only process on the first receiver that is able to successfully process Actions after receiving it.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--MatchSenderReceiverOwners-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MatchSenderReceiverOwners|
|:----|:----|
|Tag Format:|`[MatchSenderReceiverOwners:Value]`|
|Description:|This tag specifies if the command should only be processed on grids that match the owner of the command sender.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--IgnoreAntennaRequirement-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IgnoreAntennaRequirement|
|:----|:----|
|Tag Format:|`[IgnoreAntennaRequirement:Value]`|
|Description:|This tag specifies if the command should ignore the antenna requirement to send to other encounters. Because no antenna is used, you must also provide a value to the `Radius` tag as well.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--IgnoreReceiverAntennaRequirement-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IgnoreReceiverAntennaRequirement|
|:----|:----|
|Tag Format:|`[IgnoreReceiverAntennaRequirement:Value]`|
|Description:|This tag specifies if the command should be able to reach other grids that may not have an active antenna to receive the code with.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--Radius-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Radius|
|:----|:----|
|Tag Format:|`[Radius:Value]`|
|Description:|This tag specifies the Radius from the sender that other encounters will receive the command if `IgnoreAntennaRequirement` is true.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--MaxRadius-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxRadius|
|:----|:----|
|Tag Format:|`[MaxRadius:Value]`|
|Description:|This tag specifies the Maximum Radius from the sender that other encounters will receive the command, even if they are within antenna range of the sender.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--SendTargetEntityId-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SendTargetEntityId|
|:----|:----|
|Tag Format:|`[SendTargetEntityId:Value]`|
|Description:|This tag specifies if the ID of the sender's current target should be sent with the command.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--SendDamagerEntityId-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SendDamagerEntityId|
|:----|:----|
|Tag Format:|`[SendDamagerEntityId:Value]`|
|Description:|This tag specifies if the ID of the last entity that damaged the sender should be sent with the command.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--SendWaypoint-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SendWaypoint|
|:----|:----|
|Tag Format:|`[SendWaypoint:Value]`|
|Description:|This tag specifies if a waypoint profile should be generated and sent along with this command.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--Waypoint-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Waypoint|
|:----|:----|
|Tag Format:|`[Waypoint:Value]`|
|Description:|This tag specifies the SubtypeId of a Waypoint Profile that you want to generate and include with your command when using `SendWaypoint`.|
|Allowed Values:|Any Waypoint Profile SubtypeId|
|Multiple Tag Allowed:|No|

<!--SendGridValue-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SendGridValue|
|:----|:----|
|Tag Format:|`[SendGridValue:Value]`|
|Description:|This tag specifies if the Grid Value (aka Threat Score) of the broadcasting NPC should be sent with the command.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|
