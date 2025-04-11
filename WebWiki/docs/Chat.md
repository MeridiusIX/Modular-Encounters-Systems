#Chat.md

Chat Profiles in Rival AI are used to display chat messages and screen notifications at certain intervals or when certain behavior events are triggered. You can attach your Chat Profiles to any **Action Profile** by linking the Chat Profile SubtypeId. It is important that you use a unique SubtypeId for each Chat Profile you create, otherwise they may not work correctly.

Here is an example of how a Chat Profile definition is setup:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>RAI-ExampleChatProfile</SubtypeId>
      </Id>
      <Description>

      [RivalAI Chat]
      
      [UseChat:true]
      [StartsReady:true]
      [Chance:50]
      [MaxChats:1]
      [BroadcastRandomly:true]
      
      [Author:Drone Fighter]
      [Color:Red]
      
      [ChatMessages:Hello {PlayerName}, we meet again!]
      [ChatAudio:GreetingSoundId-A]
      [BroadcastChatType:Chat]
      
      [ChatMessages:How many times do we gotta teach you this lesson old man!]
      [ChatAudio:GreetingSoundId-B]
      [BroadcastChatType:Chat]

      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

The above profile could be attached to an Action Profile that is attached to a **PlayerNear** Trigger Profile. It is configured to play one message from the messages provided randomly - and only has a 50% chance of broadcasting the message, then it will no longer broadcast from this Chat Profile. Using the settings included in this document, you can have Chat or Notifications that play at timed intervals, or when specific events are triggered in a Trigger Profile.

When specifying chat messages in the tags below, there are special tags you can include in your message to dynamically change the text.

 - `{PlayerName}` - will be replaced with the name of the player receiving the message.  
 - `{AntennaName}` - will be replaced with the name of the sending antenna (highest range antenna is used.) This also works for the Author Name as well.
 - `{Faction}` - will be replaced with the Faction Tag of the NPC.
 - `{GPS}` - will be replaced with the GPS coordinates of the grid broadcasting the message (should only use this with chat).
 - `{GridName}` - will be replaced with the name of the sending grid. This also works for the Author Name as well.

***

Below are the tags you are able to use in your Chat Profiles:


|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseChat|
|:----|:----|
|Tag Format:|`[UseChat:Value]`|
|Description:|This tag specifies if the Chat Profile should be enabled.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinTime|
|:----|:----|
|Tag Format:|`[MinTime:Value]`|
|Description:|This tag specifies the minimum time (in seconds) before the next Chat message is broadcasted|
|Allowed Values:|Any Integer Greater Than `0`<br>`Value` Must Be `Less` Than `MaxTime`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxTime|
|:----|:----|
|Tag Format:|`[MaxTime:Value]`|
|Description:|This tag specifies the maximum time (in seconds) before the next Chat message is broadcasted|
|Allowed Values:|Any Integer Greater Than `0`<br>`Value` Must Be `Greater` Than `MinTime`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StartsReady|
|:----|:----|
|Tag Format:|`[StartsReady:Value]`|
|Description:|This tag specifies if the Chat timer should be ready to broadcast when the Chat Profile is triggered.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Chance|
|:----|:----|
|Tag Format:|`[Chance:Value]`|
|Description:|This tag specifies the chance (`100` being always, and `0` being never) that the Chat will broadcast when the Chat Profile is triggered.|
|Allowed Values:|Any Integer From `0` to `100`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxChats|
|:----|:----|
|Tag Format:|`[MaxChats:Value]`|
|Description:|This tag specifies the number of times the Chat Profile is allowed to broadcast.|
|Allowed Values:|Any Integer From `0` to `100`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BroadcastRandomly|
|:----|:----|
|Tag Format:|`[BroadcastRandomly:Value]`|
|Description:|This tag specifies if messages provided in your Chat Profile should be broadcasted in randomly. If `false`, they will be broadcasted from first to last.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IgnoreAntennaRequirement|
|:----|:----|
|Tag Format:|`[IgnoreAntennaRequirement:Value]`|
|Description:|This tag specifies if the Active Antenna Block requirement for Chat/Notifications should be ignored.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IgnoredAntennaRangeOverride|
|:----|:----|
|Tag Format:|`[IgnoredAntennaRangeOverride:Value]`|
|Description:|Specifies the range in meters from the Remote Control block that players will receive Chat/Notifications if the `IgnoreAntennaRequirement` is set to `true`.|
|Allowed Values:|Any number higher than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Author|
|:----|:----|
|Tag Format:|`[Author:Value]`|
|Description:|This tag specifies the author displayed when the chat message appears (does not apply for notifications)|
|Allowed Values:|Any combination of words<br>Do not use characters `[`, `]`, `:`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Color|
|:----|:----|
|Tag Format:|`[Color:Value]`|
|Description:|This tag specifies the color of the author name when the chat message appears. For notifications, this changes the entire message color.|
|Allowed Values:|`Red`<br>`Green`<br>`Blue`<br>`White`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChatMessages|
|:----|:----|
|Tag Format:|`[ChatMessages:Value]`|
|Description:|This tag allows you to specify the message(s) that gets displayed when the Chat is triggered. If multiple instances of this tag are provided, then they will play in order each time the action is triggered (or randomly if `BroadcastRandomly` is `true`).|
|Allowed Values:|Any combination of words<br>Do not use characters `[`, `]`, `:`|
|Multiple Tag Allowed:|Yes|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChatAudio|
|:----|:----|
|Tag Format:|`[ChatAudio:Value]`|
|Description:|This tag specifies the sound file you want to play with your chat message. If providing multiple instances of this tag, the values must be provided in the same order you provided the `ChatMessages`, otherwise they may play in the wrong order. If a chat message should not play audio, provide the value `None`|
|Allowed Values:|SubtypeId of Audio you want to play.|
|Multiple Tag Allowed:|Yes|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BroadcastChatType|
|:----|:----|
|Tag Format:|`[BroadcastChatType:Value]`|
|Description:|This tag specifies if the Chat should broadcast as a Chat, Notification, or Both. If providing multiple instances of this tag, the values must be provided in the same order you provided the `ChatMessages`, otherwise they may trigger in the wrong order.|
|Allowed Values:|`Chat`<br />`Notify`<br />`Both`|
|Multiple Tag Allowed:|Yes|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChatVolumeMultiplier|
|:----|:----|
|Tag Format:|`[ChatVolumeMultiplier:Value]`|
|Description:|This tag specifies the volume multiplier of audio provided with a particular chat message. To reduce volume by half, you would provide a floating value of `0.5`, etc. If providing multiple instances of this tag, the values must be provided in the same order you provided the `ChatMessages` and `ChatAudio` tags, otherwise they may trigger in the wrong order.|
|Allowed Values:|Any Value Between `0` and `1`|
|Multiple Tag Allowed:|Yes|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SendToAllOnlinePlayers|
|:----|:----|
|Tag Format:|`[SendToAllOnlinePlayers:Value]`|
|Description:|This tag specifies if chat message should be sent to all players currently online, regardless of their distance.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|GPSLabel|
|:----|:----|
|Tag Format:|`[GPSLabel:Value]`|
|Description:|This tag specifies the name of the GPS coordinates that are created if you use `{GPS}` in your chat message.|
|Allowed Values:|Any String Excluding Character `:`.|
|Multiple Tag Allowed:|No|


<!--GPSOffset   -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|GPSOffset |
|:----|:----|
|Tag Format:|`[GPSOffset :Value]`|
|Description:|nan|
|Allowed Values:|A Vector3D Value in the following format:<br />`{X:# Y:# Z:#}`<br />.|
|Multiple Tag Allowed:|no|

<!-- AllowDuplicatedMessages -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AllowDuplicatedMessages|
|:----|:----|
|Tag Format:|`[AllowDuplicatedMessages:Value]`|
|Description:|This tag allows you to specify if a chat/notification can be broadcast again if the previous message was the same.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

### SendToSpecificPlayers   
<!--SendToSpecificPlayers   -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SendToSpecificPlayers |
|:----|:----|
|Tag Format:|`[SendToSpecificPlayers :Value]`|
|Description:|nan|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|no|
<!--PlayerConditionIds  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PlayerConditionIds|
|:----|:----|
|Tag Format:|`[PlayerConditionIds:Value]`|
|Description:|nan|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|yes|