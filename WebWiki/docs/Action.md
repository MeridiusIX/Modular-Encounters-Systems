#Action.md

Action Profiles in Rival AI are used in conjunction with **Trigger Profiles**. These profile are what execute specified actions when the conditions in a Trigger Profile are satisfied. It is important that you use a unique SubtypeId for each Action Profile you create, otherwise they may not work correctly.

Here is an example of how an Action Profile definition is setup:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>RAI-ExampleActionProfile</SubtypeId>
      </Id>
      <Description>

      [RivalAI Action]
      
      [UseChatBroadcast:true]
      [ChatData:RAI-ExampleChatProfile]
	
      [CreateKnownPlayerArea:true]
      [KnownPlayerAreaRadius:15000]
      [KnownPlayerAreaTimer:30]

      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

Below are the tags you are able to use in your Action Profiles. They are divided into several categories based on what they affect.

**[Abilities](#Abilities)**  
**[Behavior](#Behavior)**  
**[Blocks](#Blocks)**  
**[Communication](#Communication)**  
**[Damage](#Damage)**   
**[Effects](#Effects)**  
**[General](#General)**  
**[Grid](#Grid)**  
**[Inventory](#Inventory)**  
**[PlayerTags](#PlayerTags)**  
**[Reputation](#Reputation)**  
**[Spawning](#Spawning)**  
**[Targeting](#Targeting)**  
**[Trigger](#Trigger)**  
**[Variables](#Variables)**  
**[Zone](#Zone)**  

***

# Abilities

This section contains actions that enable/disable special abilities for the behavior.

<!--UseJetpackInhibitorEffect-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseJetpackInhibitorEffect|
|:----|:----|
|Tag Format:|`[UseJetpackInhibitorEffect:Value]`|
|Description:|This tag specifies if the behavior should enable or disable an internalized Jetpack Inhibitor effect. Since this effect only lasts for the session, it's recommended to use it in combination with a Session Trigger when possible. `true` enables the effect, while `false` disables it.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--UseDrillInhibitorEffect-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseDrillInhibitorEffect|
|:----|:----|
|Tag Format:|`[UseDrillInhibitorEffect:Value]`|
|Description:|This tag specifies if the behavior should enable or disable an internalized Hand Drill Inhibitor effect. Since this effect only lasts for the session, it's recommended to use it in combination with a Session Trigger when possible. `true` enables the effect, while `false` disables it.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--UseNanobotInhibitorEffect-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseNanobotInhibitorEffect|
|:----|:----|
|Tag Format:|`[UseNanobotInhibitorEffect:Value]`|
|Description:|This tag specifies if the behavior should enable or disable an internalized Nanobot Inhibitor effect. Since this effect only lasts for the session, it's recommended to use it in combination with a Session Trigger when possible. `true` enables the effect, while `false` disables it.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--UseJumpInhibitorEffect-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseJumpInhibitorEffect|
|:----|:----|
|Tag Format:|`[UseJumpInhibitorEffect:Value]`|
|Description:|This tag specifies if the behavior should enable or disable an internalized Jump Drive Inhibitor effect. Since this effect only lasts for the session, it's recommended to use it in combination with a Session Trigger when possible. `true` enables the effect, while `false` disables it.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--UsePlayerInhibitorEffect-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UsePlayerInhibitorEffect|
|:----|:----|
|Tag Format:|`[UsePlayerInhibitorEffect:Value]`|
|Description:|This tag specifies if the behavior should enable or disable an internalized Player Inhibitor effect. Since this effect only lasts for the session, it's recommended to use it in combination with a Session Trigger when possible. `true` enables the effect, while `false` disables it.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!-- JumpToTarget -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|JumpToTarget|
|:----|:----|
|Tag Format:|`[JumpToTarget:Value]`|
|Description:|This tag allows you to activate the NPC Jump Drive (if present / ready) and jump to the current target.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- JumpToJumpedEntity -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|JumpToJumpedEntity|
|:----|:----|
|Tag Format:|`[JumpToJumpedEntity:Value]`|
|Description:|This tag allows you to activate the NPC Jump Drive (if present / ready) and jump to a grid that recently jumped away. This needs to be used with a `JumpCompleted` trigger|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- JumpedEntityMustBeTarget -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|JumpedEntityMustBeTarget|
|:----|:----|
|Tag Format:|`[JumpedEntityMustBeTarget:Value]`|
|Description:|This tag specifies whether or not an entity that jumps away (detected by `JumpCompleted` trigger) must also be the current target of the NPC in order to Jump / Follow it.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- JumpToWaypoint -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|JumpToWaypoint|
|:----|:----|
|Tag Format:|`[JumpToWaypoint:Value]`|
|Description:|Specifies if a ship should jump to a provided waypoint profile location.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- JumpWaypoint -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|JumpWaypoint|
|:----|:----|
|Tag Format:|`[JumpWaypoint:Value]`|
|Description:|Specifies the waypoint profile id that the ship will use to calculate jump location while using the `JumpToWaypoint` tag.|
|Allowed Value(s):|Any String Value|
|Multiple Tags Allowed:|No|


# Behavior

This section contains actions that relate to changes that can be made to the behavior and auto-pilot.

<!--ChangeBehaviorSubclass-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeBehaviorSubclass|
|:----|:----|
|Tag Format:|`[ChangeBehaviorSubclass:Value]`|
|Description:|This tag specifies if the Sub-Class of the behavior should be switched to another (eg: change from `Fighter` to `Strike`).|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--NewBehaviorSubclass-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|NewBehaviorSubclass|
|:----|:----|
|Tag Format:|`[NewBehaviorSubclass:Value]`|
|Description:|This tag specifies the new Behavior Sub-Class that is used if `ChangeBehaviorSubclass` is set to `true`.|
|Allowed Values:|Any Behavior Type<br>[Click Here For Eligible Types](https://github.com/MeridiusIX/Modular-Encounters-Systems/wiki/Core-Behavior)|
|Multiple Tag Allowed:|No|

<!--ChangeAutopilotSpeed  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeAutopilotSpeed|
|:----|:----|
|Tag Format:|`[ChangeAutopilotSpeed:Value]`|
|Description:|This tag specifies if the AutoPilot speed of the current NPC should be changed to a new value.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--NewAutopilotSpeed  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|NewAutopilotSpeed|
|:----|:----|
|Tag Format:|`[NewAutopilotSpeed:Value]`|
|Description:|Specifies the new AutoPilot speed if `ChangeAutopilotSpeed` was set to `true`.|
|Allowed Values:|Any number equal/higher than `0`|
|Multiple Tag Allowed:|No|

<!--ChangeInertiaDampeners  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeInertiaDampeners|
|:----|:----|
|Tag Format:|`[ChangeInertiaDampeners:Value]`|
|Description:|This tag specifies if the AutoPilot should enable or disable the Inertia Dampeners (may only work with some behaviors / modes).|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--InertiaDampenersEnable  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|InertiaDampenersEnable|
|:----|:----|
|Tag Format:|`[InertiaDampenersEnable:Value]`|
|Description:|Specifies the new Inertia Dampeners mode if `ChangeInertiaDampeners` was set to `true`.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--BarrelRoll  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BarrelRoll|
|:----|:----|
|Tag Format:|`[BarrelRoll:Value]`|
|Description:|This tag specifies if the NPC should perform a roll for a short time. Barrel Roll controls can be found in the Autopilot Profile tags section.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--Ramming  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Ramming|
|:----|:----|
|Tag Format:|`[Ramming:Value]`|
|Description:|This tag specifies if the NPC should attempt to ram a target for a short time. Ramming controls can be found in the Autopilot Profile tags section.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--Retreat  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Retreat|
|:----|:----|
|Tag Format:|`[Retreat:Value]`|
|Description:|This tag specifies if the Retreat function should be activated in the current behavior.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--RecalculateDespawnCoords-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RecalculateDespawnCoords|
|:----|:----|
|Tag Format:|`[RecalculateDespawnCoords:Value]`|
|Description:|This tag specifies if the NPC should recalculate its Despawn Coordinates if it is using coordinates retrieved from MES or generated within RivalAI. The newly generated coordinates will be created from data in the Active Autopilot Profile of the behavior.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ForceDespawn  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ForceDespawn|
|:----|:----|
|Tag Format:|`[ForceDespawn:Value]`|
|Description:|This tag specifies if the NPC grid should immediately be despawned.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--TerminateBehavior  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TerminateBehavior|
|:----|:----|
|Tag Format:|`[TerminateBehavior:Value]`|
|Description:|This tag specifies if the current NPC Behavior should be terminated (meaning no more AI processing from that behavior).|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ChangeRotationDirection  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeRotationDirection|
|:----|:----|
|Tag Format:|`[ChangeRotationDirection:Value]`|
|Description:|This tag specifies if the ship should use a different direction when rotating towards a target. Specify direction using the `RotationDirection` tag|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--RotationDirection  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RotationDirection|
|:----|:----|
|Tag Format:|`[RotationDirection:Value]`|
|Description:|This tag specifies the direction a ship should use when rotating towards a target if `ChangeRotationDirection` is `true`.|
|Allowed Values:|`Forward`<br>`Backward`<br>`Up`<br>`Down`<br>`Left`<br>`Right`|
|Multiple Tag Allowed:|No|

<!--ChangeAutopilotProfile-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeAutopilotProfile|
|:----|:----|
|Tag Format:|`[ChangeAutopilotProfile:Value]`|
|Description:|This tag specifies if the Autopilot Profile should be switched to another mode, using another attached Autopilot Profile. |
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--AutopilotProfile-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AutopilotProfile|
|:----|:----|
|Tag Format:|`[AutopilotProfile:Value]`|
|Description:|This tag specifies the Autopilot Profile Mode that should be switched to. |
|Allowed Values:|`Primary`<br>`Secondary`<br>`Tertiary`|
|Multiple Tag Allowed:|No|

<!--StopAllRotation-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StopAllRotation|
|:----|:----|
|Tag Format:|`[StopAllRotation:Value]`|
|Description:|This tag will set all Gyroscope Overrides to `0` on the NPC. This is useful when paired with a `Compromised` trigger to stop any gyros that may be stuck rotating when a RivalAI Remote Control is disabled.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--StopAllThrust-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StopAllThrust|
|:----|:----|
|Tag Format:|`[StopAllThrust:Value]`|
|Description:|This tag will set all Thruster Overrides to `0` on the NPC. This is useful when paired with a `Compromised` trigger to stop any thrusters that may be stuck in override when a RivalAI Remote Control is disabled.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--RandomGyroRotation-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RandomGyroRotation|
|:----|:----|
|Tag Format:|`[RandomGyroRotation:Value]`|
|Description:|This tag will set a random Gyroscopic rotation and apply it to the NPC grid. This is useful when paired with a `Compromised` trigger.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--RandomThrustDirection-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RandomThrustDirection|
|:----|:----|
|Tag Format:|`[RandomThrustDirection:Value]`|
|Description:|This tag will set a random Thruster Override Strength and Direction, and apply it to the NPC grid. This is useful when paired with a `Compromised` trigger.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--UseCurrentPositionAsPatrolReference-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseCurrentPositionAsPatrolReference|
|:----|:----|
|Tag Format:|`[UseCurrentPositionAsPatrolReference:Value]`|
|Description:|This tag sets the current position of the NPC as its Patrol Reference so it will calculate offsets starting from that position instead of using its initial spawn coordinates.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ClearCustomPatrolReference-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ClearCustomPatrolReference|
|:----|:----|
|Tag Format:|`[ClearCustomPatrolReference:Value]`|
|Description:|This tag removes any saved coordinates that may have been set using `UseCurrentPositionAsPatrolReference` so behaviors using patrol will revert to using their initial spawn coordinates to calculate patrol offsets.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!-- ClearAllWaypoints -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ClearAllWaypoints|
|:----|:----|
|Tag Format:|`[ClearAllWaypoints:Value]`|
|Description:|Specifies if all current waypoints should be erased when using the CargoShip behavior subclass. This will result in the behavior travelling to its generated despawn coordinates.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- AddWaypoints -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AddWaypoints|
|:----|:----|
|Tag Format:|`[AddWaypoints:Value]`|
|Description:|Specifies if one or more Waypoints should be created and added to a behavior if they are using the CargoShip subclass.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- WaypointsToAdd -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|WaypointsToAdd|
|:----|:----|
|Tag Format:|`[WaypointsToAdd:Value]`|
|Description:|Specifies the name(s) of the waypoint profiles you want to add to a CargoShip subclass behavior if using the `AddWaypoints` tag|
|Allowed Value(s):|Waypoint Profile SubtypeId|
|Multiple Tags Allowed:|Yes|

<!-- CancelWaitingAtWaypoint -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CancelWaitingAtWaypoint|
|:----|:----|
|Tag Format:|`[CancelWaitingAtWaypoint:Value]`|
|Description:|Specifies if a CargoShip subclass behavior should stop waiting at a waypoint it reached and begin travelling to the next eligible waypoint.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- SwitchToNextWaypoint -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SwitchToNextWaypoint|
|:----|:----|
|Tag Format:|`[SwitchToNextWaypoint:Value]`|
|Description:|Specifies if a CargoShip subclass behavior should have its current waypoint that is being travelled to cancelled so it can move onto the next eligible waypoint.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

***

# <a>Blocks</a>

This section contains actions that affect the state of blocks on the NPC grid.

<!--EnableBlocks  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EnableBlocks|
|:----|:----|
|Tag Format:|`[EnableBlocks:Value]`|
|Description:|This tag specifies if certain blocks should be turned On or Off.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--EnableBlockNames  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EnableBlockNames|
|:----|:----|
|Tag Format:|`[EnableBlockNames:Value]`|
|Description:|This tag specifies the name(s) of blocks that you want to toggle on or off.|
|Allowed Values:|Any Block Name|
|Multiple Tag Allowed:|Yes|

<!--EnableBlockStates  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EnableBlockStates|
|:----|:----|
|Tag Format:|`[EnableBlockStates:Value]`|
|Description:|This tag specifies the On/Off state of blocks that you want to toggle on or off.|
|Allowed Values:|`Off`<br>`On`<br>`Toggle`|
|Multiple Tag Allowed:|Yes|

<!--ChangeAntennaRanges  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeAntennaRanges|
|:----|:----|
|Tag Format:|`[ChangeAntennaRanges:Value]`|
|Description:|This tag specifies if attached antennas should have their range changed.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--AntennaNamesForRangeChange  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AntennaNamesForRangeChange|
|:----|:----|
|Tag Format:|`[AntennaNamesForRangeChange:Value]`|
|Description:|This tag specifies the name of the antenna(s) that should have their ranges changed. If this tag is not provided, then all antennas will be affected|
|Allowed Values:|Any Antenna Name|
|Multiple Tag Allowed:|Yes|

<!--AntennaRangeChangeType  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AntennaRangeChangeType|
|:----|:----|
|Tag Format:|`[AntennaRangeChangeType:Value]`|
|Description:|This tag specifies how the antenna range should be changed by using this value with the value of `AntennaRangeChangeAmount`.|
|Allowed Values:|`Set`<br />`Increase`<br />`Decrease`|
|Multiple Tag Allowed:|No|

<!--AntennaRangeChangeAmount  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AntennaRangeChangeAmount|
|:----|:----|
|Tag Format:|`[AntennaRangeChangeAmount:Value]`|
|Description:|This tag specifies how much the antenna range should be changed by.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--ChangeAntennaOwnership  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeAntennaOwnership|
|:----|:----|
|Tag Format:|`[ChangeAntennaOwnership:Value]`|
|Description:|This tag specifies if Antenna Blocks on the current NPC grid should have their ownership changed.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--AntennaFactionOwner  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AntennaFactionOwner|
|:----|:----|
|Tag Format:|`[AntennaFactionOwner:Value]`|
|Description:|Specifies the faction (by faction tag) that antenna blocks get changed to if `ChangeAntennaOwnership` is `true`.|
|Allowed Values:|Any Faction Tag|
|Multiple Tag Allowed:|No|

<!--ChangeBlockNames  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeBlockNames|
|:----|:----|
|Tag Format:|`[ChangeBlockNames:Value]`|
|Description:|This tag specifies if blocks on the NPC grid should have their names changed.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ChangeBlockNamesFrom  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeBlockNamesFrom|
|:----|:----|
|Tag Format:|`[ChangeBlockNamesFrom:Value]`|
|Description:|This tag specifies a name of a Block that will have its name changed. Ideally you should place this tag before the `ChangeBlockNamesTo` tag, keeping them in pairs in your Action Profile.|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--ChangeBlockNamesTo  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeBlockNamesTo|
|:----|:----|
|Tag Format:|`[ChangeBlockNamesTo:Value]`|
|Description:|This tag specifies the name that a Block specified in `ChangeBlockNamesFrom` will be changed to. Ideally you should place this tag after the `ChangeBlockNamesFrom` tag, keeping them in pairs in your Action Profile.|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--TriggerTimerBlocks  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TriggerTimerBlocks|
|:----|:----|
|Tag Format:|`[TriggerTimerBlocks:Value]`|
|Description:|This tag specifies if select Timer Blocks on the current NPC grid should be Triggered.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--TimerBlockNames  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TimerBlockNames|
|:----|:----|
|Tag Format:|`[TimerBlockNames:Value]`|
|Description:|This tag specifies a name of a Timer Block on the current NPC grid should be Triggered.|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--SelfDestruct  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SelfDestruct|
|:----|:----|
|Tag Format:|`[SelfDestruct:Value]`|
|Description:|This tag specifies if all Warhead Blocks on the current NPC grid should be detonated.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!-- StaggerWarheadDetonation -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StaggerWarheadDetonation|
|:----|:----|
|Tag Format:|`[StaggerWarheadDetonation:Value]`|
|Description:|This tag specifies if warheads should be detonated in 1 second intervals instead of all at once.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!-- SelfDestructTimerPadding -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SelfDestructTimerPadding|
|:----|:----|
|Tag Format:|`[SelfDestructTimerPadding:Value]`|
|Description:|This tag specifies the minimum time that all warheads will start at if countdown is initiated.|
|Allowed Values:|Any number greater or equal to `0`|
|Multiple Tag Allowed:|No|

<!-- SelfDestructTimeBetweenBlasts -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SelfDestructTimeBetweenBlasts|
|:----|:----|
|Tag Format:|`[SelfDestructTimeBetweenBlasts:Value]`|
|Description:|This tag specifies the time between detonations that is applied to warheads if `StaggerWarheadDetonation` is used.|
|Allowed Values:|Any number greater or equal to `0`|
|Multiple Tag Allowed:|No|

<!--ChangeBlockOwnership-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeBlockOwnership|
|:----|:----|
|Tag Format:|`[ChangeBlockOwnership:Value]`|
|Description:|This tag specifies if some blocks should have their ownership changed. Block names and Faction Tags should be provided in the `OwnershipBlockNames` and `OwnershipBlockFactions` tags together.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--OwnershipBlockNames-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|OwnershipBlockNames|
|:----|:----|
|Tag Format:|`[OwnershipBlockNames:Value]`|
|Description:|This tag specifies a name of a Block that gets it's ownership changed.|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--OwnershipBlockFactions-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|OwnershipBlockFactions|
|:----|:----|
|Tag Format:|`[OwnershipBlockFactions:Value]`|
|Description:|This tag specifies a name of a faction that a block ownership should be changed to.|
|Allowed Values:|Any Faction Tag|
|Multiple Tag Allowed:|Yes|

<!--RazeBlocksWithNames-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RazeBlocksWithNames|
|:----|:----|
|Tag Format:|`[RazeBlocksWithNames:Value]`|
|Description:|This tag specifies if some blocks should be destroyed on the NPC grid.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--RazeBlocksNames-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RazeBlocksNames|
|:----|:----|
|Tag Format:|`[RazeBlocksNames:Value]`|
|Description:|This tag specifies names of blocks that are destroyed when the action is executed.|
|Allowed Values:|Any Block Name|
|Multiple Tag Allowed:|Yes|

<!--RazeBlocksOfType-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RazeBlocksOfType|
|:----|:----|
|Tag Format:|`[RazeBlocksOfType:Value]`|
|Description:|This tag specifies if some block types should be destroyed on the NPC grid.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--RazeBlocksTypes-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RazeBlocksTypes|
|:----|:----|
|Tag Format:|`[RazeBlocksTypes:Value]`|
|Description:|This tag specifies types of blocks that are destroyed when the action is executed.|
|Allowed Values:|Any Block Type (eg `MyObjectBuilder_CubeBlock/SomeBlockSubtypeId`)|
|Multiple Tag Allowed:|Yes|

<!--ToggleBlocksOfType-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ToggleBlocksOfType|
|:----|:----|
|Tag Format:|`[ToggleBlocksOfType:Value]`|
|Description:|This tag specifies if blocks of a matching type should be set to on, off, or toggled from existing state.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--BlockTypesToToggle-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BlockTypesToToggle|
|:----|:----|
|Tag Format:|`[BlockTypesToToggle:Value]`|
|Description:|This tag specifies the types of block that will be toggled. This tag needs to be paired with a `BlockTypeToggles` tag.|
|Allowed Values:|Any Block DefinitionID<br>Eg: `MyObjectBuilder_Parachute/LgParachute`|
|Multiple Tag Allowed:|Yes|

<!--BlockTypeToggles-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BlockTypeToggles|
|:----|:----|
|Tag Format:|`[BlockTypeToggles:Value]`|
|Description:|This tag specifies the toggle action that will be applied to a block type. This tag needs to be paired with a `BlockTypesToToggle` tag.|
|Allowed Values:|`Off`<br>`On`<br>`Toggle`|
|Multiple Tag Allowed:|Yes|

<!--BuildProjectedBlocks-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BuildProjectedBlocks|
|:----|:----|
|Tag Format:|`[BuildProjectedBlocks:Value]`|
|Description:|This tag specifies if a number of blocks currently being projected from the NPC Grid should be built instantly. The projection must already be active at the time this Action is activated (enabling the projector and using this action in the same profile may not work since there is a minor delay when the projection is being created in game).|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--MaxProjectedBlocksToBuild -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxProjectedBlocksToBuild|
|:----|:----|
|Tag Format:|`[MaxProjectedBlocksToBuild:Value]`|
|Description:|Specifies the maximum number of blocks that are built if `BuildProjectedBlocks` is `true`. If this value is not defined, or the value is set to `-1`, then all currently eligible blocks will be built on the projection.|
|Allowed Values:|Any Integer higher than `-1`|
|Multiple Tag Allowed:|No|

<!--SetWeaponsToMinRange-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SetWeaponsToMinRange|
|:----|:----|
|Tag Format:|`[SetWeaponsToMinRange:Value]`|
|Description:|This tag specifies if all auto-firing weapons (turrets, weapon core homing weapons, etc) should be limited to 800m.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--SetWeaponsToMaxRange-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SetWeaponsToMaxRange|
|:----|:----|
|Tag Format:|`[SetWeaponsToMaxRange:Value]`|
|Description:|This tag specifies if all auto-firing weapons (turrets, weapon core homing weapons, etc) should be set to their maximum range.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--EnableHighestRangeAntennas-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EnableHighestRangeAntennas|
|:----|:----|
|Tag Format:|`[EnableHighestRangeAntennas:Value]`|
|Description:|This tag specifies if all antennas on the current grid that have the highest range should be turned on.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--DisableHighestRangeAntennas-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DisableHighestRangeAntennas|
|:----|:----|
|Tag Format:|`[DisableHighestRangeAntennas:Value]`|
|Description:|This tag specifies if all antennas on the current grid that have the highest range should be turned off.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!-- ApplyLcdChanges -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ApplyLcdChanges|
|:----|:----|
|Tag Format:|`[ApplyLcdChanges:Value]`|
|Description:|This tag allows you to specify if one or more blocks should have their LCD contents changed.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- LcdTextTemplateFile -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LcdTextTemplateFile|
|:----|:----|
|Tag Format:|`[LcdTextTemplateFile:Value]`|
|Description:|This tag allows you to specify the name of the TextTemplate file that is used for populating LCD content if using the `ApplyLcdChanges` tag|
|Allowed Value(s):|Any String Value|
|Multiple Tags Allowed:|No|

<!-- LcdBlockNames -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LcdBlockNames|
|:----|:----|
|Tag Format:|`[LcdBlockNames:Value]`|
|Description:|This tag allows you to specify one or more block name that will have LCD contents changed if using the `ApplyLcdChanges` tag. Each instance of this tag should be paired with the `LcdTemplateIndexes` tag as well.|
|Allowed Value(s):|Any String Value|
|Multiple Tags Allowed:|Yes|

<!-- LcdTemplateIndexes -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LcdTemplateIndexes|
|:----|:----|
|Tag Format:|`[LcdTemplateIndexes:Value]`|
|Description:|This tag allows you to specify one or more index that is used to select an Lcd Entry in the TextTemplate file if using the `ApplyLcdChanges` tag. Each instance of this tag should be paired with the `LcdBlockNames` tag as well.|
|Allowed Value(s):|Any Integer Greater/Equal To `0`|
|Multiple Tags Allowed:|Yes|



***

# Communication

This section contains actions for NPC to NPC communication.

<!--UseChatBroadcast  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseChatBroadcast|
|:----|:----|
|Tag Format:|`[UseChatBroadcast:Value]`|
|Description:|This tag specifies if an attached Chat Profile should be activated.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ChatData  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChatData|
|:----|:----|
|Tag Format:|`[ChatData:Value]`|
|Description:|This tag specifies which Chat Profile should be activated if `UseChatBroadcast` is `true`.|
|Allowed Values:|Any Chat Profile SubtypeId|
|Multiple Tag Allowed:|Yes|

<!--BroadcastCommandProfiles-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BroadcastCommandProfiles|
|:----|:----|
|Tag Format:|`[BroadcastCommandProfiles:Value]`|
|Description:|This tag specifies if the Behavior should broadcast one or more Command Profile to other nearby NPCs.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--CommandProfileIds-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CommandProfileIds|
|:----|:----|
|Tag Format:|`[CommandProfileIds:Value]`|
|Description:|This tag specifies the SubtypeId of a Command Profile that you want to use in your broadcast to other nearby NPCs.|
|Allowed Values:|Any Command Profile SubtypeId|
|Multiple Tag Allowed:|Yes|

<!--AddWaypointFromCommand-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AddWaypointFromCommand|
|:----|:----|
|Tag Format:|`[AddWaypointFromCommand:Value]`|
|Description:|This tag specifies if the Behavior should add a waypoint received by a command to its list of waypoints. This is mostly for use by the `CargoShip` behavior, but others may use it in the future.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--SwitchToReceivedTarget  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SwitchToReceivedTarget|
|:----|:----|
|Tag Format:|`[SwitchToReceivedTarget:Value]`|
|Description:|This tag specifies if the Behavior should switch to a new target provided by either the damage or broadcast system.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--InheritLastAttackerFromCommand-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|InheritLastAttackerFromCommand|
|:----|:----|
|Tag Format:|`[InheritLastAttackerFromCommand:Value]`|
|Description:|This tag specifies if the Behavior should consider the received target from a command as an entity that attacked it as well. Required for some behavior specific actions.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!-- AssignEscortFromCommand -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AssignEscortFromCommand|
|:----|:----|
|Tag Format:|`[AssignEscortFromCommand:Value]`|
|Description:|This tag allows you to assign any potential Escort Requests to an empty escort slot (if available) for this NPC. The Escort Requests are received by Command Profile related Triggers.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

***

# Damage

This section contains actions for damaging other entities.

<!--DamageToolAttacker  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DamageToolAttacker|
|:----|:----|
|Tag Format:|`[DamageToolAttacker:Value]`|
|Description:|This tag specifies if a player or block attacking the current NPC with a Grinder/Drill should receive damage. This tag only works if called from a Trigger Profile that uses `Damage` as a Trigger Type.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--DamageToolAttackerAmount  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DamageToolAttackerAmount|
|:----|:----|
|Tag Format:|`[DamageToolAttackerAmount:Value]`|
|Description:|Specifies the amount of damage a target receives if `DamageToolAttacker` is `true`.|
|Allowed Values:|Any Number higher than `0`|
|Multiple Tag Allowed:|No|

<!--DamageToolAttackerParticle  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DamageToolAttackerParticle|
|:----|:----|
|Tag Format:|`[DamageToolAttackerParticle:Value]`|
|Description:|Specifies a particle effect that will display over the attacker if `DamageToolAttacker` is `true`.|
|Allowed Values:|Any Particle Effect SubtypeId|
|Multiple Tag Allowed:|No|

<!--DamageToolAttackerSound  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DamageToolAttackerSound|
|:----|:----|
|Tag Format:|`[DamageToolAttackerSound:Value]`|
|Description:|Specifies a sound effect that will display over the attacker if `DamageToolAttacker` is `true`.|
|Allowed Values:|Any Audio SubtypeId|
|Multiple Tag Allowed:|No|

<!--GenerateExplosion  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|GenerateExplosion|
|:----|:----|
|Tag Format:|`[GenerateExplosion:Value]`|
|Description:|This tag specifies if an Explosion should be generated at the coordinates of the Remote Control.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ExplosionOffsetFromRemote  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ExplosionOffsetFromRemote|
|:----|:----|
|Tag Format:|`[ExplosionOffsetFromRemote:Value]`|
|Description:|This tag specifies the Offset from the Remote Control Position that the explosion should be created at if `GenerateExplosion` is `true`.|
|Allowed Values:|A Vector3D Value in the following format:<br />`{X:# Y:# Z:#}`<br />X: Right<br />Y: Up<br />Z: Forward<br />Replace `#` with values in meters.|
|Multiple Tag Allowed:|No|

<!--ExplosionRange  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ExplosionRange|
|:----|:----|
|Tag Format:|`[ExplosionRange:Value]`|
|Description:|Specifies the radius of the Explosion if `GenerateExplosion` is `true`. The particle effect used for the explosion changes depending on this value:<br />0-1: Tiny<br />2-14: Small<br />15-29: Medium<br />30+: Large|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--ExplosionDamage  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ExplosionDamage|
|:----|:----|
|Tag Format:|`[ExplosionDamage:Value]`|
|Description:|Specifies the damage of the Explosion if `GenerateExplosion` is `true`.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--ExplosionIgnoresVoxels  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ExplosionIgnoresVoxels|
|:----|:----|
|Tag Format:|`[ExplosionIgnoresVoxels:Value]`|
|Description:|This tag specifies if an Explosion should skip damage to voxels if `GenerateExplosion` is `true`.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--CreateRandomLightning  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CreateRandomLightning|
|:----|:----|
|Tag Format:|`[CreateRandomLightning:Value]`|
|Description:|This tag specifies if a bolt of lightning should be generated near the NPC. Only works on planets that have atmosphere.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--CreateLightningAtAttacker  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CreateLightningAtAttacker|
|:----|:----|
|Tag Format:|`[CreateLightningAtAttacker:Value]`|
|Description:|This tag specifies if a bolt of lightning should be generated at the position of the entity that caused damage to the NPC. Only works on planets that have atmosphere.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--CreateLightningAtTarget  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CreateLightningAtTarget|
|:----|:----|
|Tag Format:|`[CreateLightningAtTarget:Value]`|
|Description:|This tag specifies if a bolt of lightning should be generated at the position of current target. Only works on planets that have atmosphere.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--LightningDamage  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LightningDamage|
|:----|:----|
|Tag Format:|`[LightningDamage:Value]`|
|Description:|Specifies the damage of lightning bolts created by an Action.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--LightningExplosionRadius  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LightningExplosionRadius|
|:----|:----|
|Tag Format:|`[LightningExplosionRadius:Value]`|
|Description:|Specifies the explosion radius of lightning bolts created by an Action.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--LightningColor  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LightningColor|
|:----|:----|
|Tag Format:|`[LightningColor:Value]`|
|Description:|Specifies the color of lightning bolts created by an Action.|
|Allowed Values:|`{X:0 Y:0 Z:0}`<br>`X` 0 - 100 (Red)<br>`Y` 0 - 100 (Green)<br>`Z` 0 - 100 (Blue)|
|Multiple Tag Allowed:|No|

<!--LightningMinDistance  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LightningMinDistance|
|:----|:----|
|Tag Format:|`[LightningMinDistance:Value]`|
|Description:|Specifies the minimum distance from the NPC that lightning will be created at if `CreateRandomLightning` is `true`.|
|Allowed Values:|Any Number Greater Than `0`<br>Must be lower than `LightningMaxDistance`|
|Multiple Tag Allowed:|No|

<!--LightningMaxDistance  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LightningMaxDistance|
|:----|:----|
|Tag Format:|`[LightningMaxDistance:Value]`|
|Description:|Specifies the maximum distance from the NPC that lightning will be created at if `CreateRandomLightning` is `true`.|
|Allowed Values:|Any Number Greater Than `0`<br>Must be higher than `LightningMinDistance`|
|Multiple Tag Allowed:|No|

# Effects

This section contains actions for playing audio and visual effects.


<!-- PlaySoundAtPosition -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PlaySoundAtPosition|
|:----|:----|
|Tag Format:|`[PlaySoundAtPosition:Value]`|
|Description:|This tag specifies if a single sound effect should be played at the NPC current position. This does not require a sound block, or any additional grid configuration.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- SoundAtPosition -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SoundAtPosition|
|:----|:----|
|Tag Format:|`[SoundAtPosition:Value]`|
|Description:|This tag specifies the name of the sound effect you want to play if using `PlaySoundAtPosition`|
|Allowed Value(s):||
|Multiple Tags Allowed:|No|

# General

This section contains actions that don't quite fit the other sections.  

<!--Chance  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Chance|
|:----|:----|
|Tag Format:|`[Chance:Value]`|
|Description:|Specifies the Chance (out of 100) that this action will be run.|
|Allowed Values:|Any Number `0` to `100`|
|Multiple Tag Allowed:|No|

<!-- SetGridCleanupExempt -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SetGridCleanupExempt|
|:----|:----|
|Tag Format:|`[SetGridCleanupExempt:Value]`|
|Description:|This tag specifies if the NPC grid should be temporarily exempt from MES cleanup processes.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- GridCleanupExemptDuration -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|GridCleanupExemptDuration|
|:----|:----|
|Tag Format:|`[GridCleanupExemptDuration:Value]`|
|Description:|This tag specifies the length of time (in seconds) that the NPC will be exempt from cleanup if used with the `SetGridCleanupExempt` tag|
|Allowed Value(s):|Any Integer Greater/Equal To `0`|
|Multiple Tags Allowed:|No|


# Grid

This section contains actions that affect the entire grid.  

<!--RecolorGrid  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RecolorGrid|
|:----|:----|
|Tag Format:|`[RecolorGrid:Value]`|
|Description:|This tag specifies if selected blocks on the grid should be recolored and reskinned.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--RecolorSubGrids  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RecolorSubGrids|
|:----|:----|
|Tag Format:|`[RecolorSubGrids:Value]`|
|Description:|This tag specifies if subgrids should also be considered when using the `RecolorGrid` tag.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--OldBlockColors  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|OldBlockColors|
|:----|:----|
|Tag Format:|`[OldBlockColors:Value]`|
|Description:|This tag specifies the old colors that are targeted for replacement when using the `RecolorGrid` tag. You should also include `NewBlockColors` and `NewBlockSkins` tags together when using this tag.|
|Allowed Values:|A Vector3D Value in the following format:<br />`{X:# Y:# Z:#}`<br />X: Right<br />Y: Up<br />Z: Forward<br />Replace `#` with matching values from ColorMaskHSV|
|Multiple Tag Allowed:|Yes|

<!--NewBlockColors  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|NewBlockColors|
|:----|:----|
|Tag Format:|`[NewBlockColors:Value]`|
|Description:|This tag specifies the new colors that are used for replacement when using the `RecolorGrid` tag. Provide `{X:-10 Y:-10 Z:-10}` to skip replacing color if you only intend to replace skin.|
|Allowed Values:|A Vector3D Value in the following format:<br />`{X:# Y:# Z:#}`<br />X: Right<br />Y: Up<br />Z: Forward<br />Replace `#` with matching values from ColorMaskHSV|
|Multiple Tag Allowed:|Yes|

<!--NewBlockSkins  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|NewBlockSkins|
|:----|:----|
|Tag Format:|`[NewBlockSkins:Value]`|
|Description:|This tag specifies the new skins that are used for replacement when using the `RecolorGrid` tag. Provide a single blank space to skip replacing skin if you only intend to replace color.|
|Allowed Values:|Any Armor Skin SubtypeId|
|Multiple Tag Allowed:|Yes|

<!--GridEditable  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|GridEditable|
|:----|:----|
|Tag Format:|`[GridEditable:Value]`|
|Description:|This tag specifies if the NPC Grid is able to be edited (add/remove/weld/grind blocks).|
|Allowed Values:|`Yes`<br>`No`|
|Multiple Tag Allowed:|No|

<!--GridSubGridsEditableEditable  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SubGridsEditable|
|:----|:----|
|Tag Format:|`[SubGridsEditable:Value]`|
|Description:|This tag specifies if sub-grids editable state should also be changed if `GridEditable` tag is used.|
|Allowed Values:|`Yes`<br>`No`|
|Multiple Tag Allowed:|No|

<!--GridDestructible  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|GridDestructible|
|:----|:----|
|Tag Format:|`[GridDestructible:Value]`|
|Description:|This tag specifies if the NPC Grid should have destructible blocks (can receive damage).|
|Allowed Values:|`Yes`<br>`No`|
|Multiple Tag Allowed:|No|

<!--SubGridsDestructible  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SubGridsDestructible|
|:----|:----|
|Tag Format:|`[SubGridsDestructible:Value]`|
|Description:|This tag specifies if sub-grids destructible state should also be changed if `GridDestructible` tag is used.|
|Allowed Values:|`Yes`<br>`No`|
|Multiple Tag Allowed:|No|

# Inventory

<!--ApplyContainerTypeToInventoryBlock-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ApplyContainerTypeToInventoryBlock|
|:----|:----|
|Tag Format:|`[ApplyContainerTypeToInventoryBlock:Value]`|
|Description:|This tag specifies if one or more block should have their inventories filled using a specified ContainerType definition subtypeID.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ContainerTypeBlockNames-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ContainerTypeBlockNames|
|:----|:----|
|Tag Format:|`[ContainerTypeBlockNames:Value]`|
|Description:|This tag specifies one or more Block Names of blocks that will have inventory applied if using the `ApplyContainerTypeToInventoryBlock` tag. This tag should be paired with the `ContainerTypeSubtypeIds` tag.|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--ContainerTypeSubtypeIds-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ContainerTypeSubtypeIds|
|:----|:----|
|Tag Format:|`[ContainerTypeSubtypeIds:Value]`|
|Description:|This tag specifies one or more TextTemplate files that will be used to set block custom data if using the `ApplyContainerTypeToInventoryBlock` tag. This tag should be paired with the `ContainerTypeBlockNames` tag.|
|Allowed Values:|ContainerType definition SubtypeId|
|Multiple Tag Allowed:|Yes|

<!--AddDatapadsToSeats-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AddDatapadsToSeats|
|:----|:----|
|Tag Format:|`[AddDatapadsToSeats:Value]`|
|Description:|This tag specifies if one or more Datapads should be added to seats randomly on the grid.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--DatapadNamesToAdd-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DatapadNamesToAdd|
|:----|:----|
|Tag Format:|`[DatapadNamesToAdd:Value]`|
|Description:|Specifies one or more RivalAI Datapad Profiles to use when randomly adding Datapads to seats.|
|Allowed Values:|Any RivalAI Datapad Profile|
|Multiple Tag Allowed:|Yes|

<!--DatapadCountToAdd-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DatapadCountToAdd|
|:----|:----|
|Tag Format:|`[DatapadCountToAdd:Value]`|
|Description:|Specifies the number of random Datapads that get added to seats on the grid.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

# PlayerTags

<!--AddTagstoPlayers  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AddTagstoPlayers|
|:----|:----|
|Tag Format:|`[AddTagstoPlayers:Value]`|
|Description:|nan|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|no|
<!--AddTagsPlayerConditionIds  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AddTagsPlayerConditionIds|
|:----|:----|
|Tag Format:|`[AddTagsPlayerConditionIds:Value]`|
|Description:|nan|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|yes|
<!--AddTags  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AddTags|
|:----|:----|
|Tag Format:|`[AddTags:Value]`|
|Description:|nan|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|yes|
<!--AddTagsIncludeSavedPlayerIdentity  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AddTagsIncludeSavedPlayerIdentity|
|:----|:----|
|Tag Format:|`[AddTagsIncludeSavedPlayerIdentity:Value]`|
|Description:|nan|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|no|
<!--AddTagsOverridePositionInPlayerCondition  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AddTagsOverridePositionInPlayerCondition|
|:----|:----|
|Tag Format:|`[AddTagsOverridePositionInPlayerCondition:Value]`|
|Description:|nan|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|no|
<!--RemoveTagsFromPlayers  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RemoveTagsFromPlayers|
|:----|:----|
|Tag Format:|`[RemoveTagsFromPlayers:Value]`|
|Description:|nan|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|no|
<!--RemoveTagsPlayerConditionIds  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RemoveTagsPlayerConditionIds|
|:----|:----|
|Tag Format:|`[RemoveTagsPlayerConditionIds:Value]`|
|Description:|nan|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|yes|
<!--RemoveTags  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RemoveTags|
|:----|:----|
|Tag Format:|`[RemoveTags:Value]`|
|Description:|nan|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|yes|
<!--RemoveTagsIncludeSavedPlayerIdentity  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RemoveTagsIncludeSavedPlayerIdentity|
|:----|:----|
|Tag Format:|`[RemoveTagsIncludeSavedPlayerIdentity:Value]`|
|Description:|nan|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|no|
<!--RemoveTagsOverridePositioninPlayerCondition  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RemoveTagsOverridePositioninPlayerCondition|
|:----|:----|
|Tag Format:|`[RemoveTagsOverridePositioninPlayerCondition:Value]`|
|Description:|nan|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|no|

# Reputation

This section contains actions that affect the reputation of players interacting with NPC grids.

<!--ChangeReputationWithPlayers  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeReputationWithPlayers|
|:----|:----|
|Tag Format:|`[ChangeReputationWithPlayers:Value]`|
|Description:|This tag specifies if players within a radius near the current NPC should have their reputation changed with the NPC faction.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ReputationChangeRadius  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReputationChangeRadius|
|:----|:----|
|Tag Format:|`[ReputationChangeRadius:Value]`|
|Description:|Specifies the radius that players are detected for reputation change if `ChangeReputationWithPlayers` is `true`.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--ReputationChangeFactions  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReputationChangeFactions|
|:----|:----|
|Tag Format:|`[ReputationChangeFactions:Value]`|
|Description:|Specifies one or more Faction Tags that player reputation is changed with. If you provide the value `{Self}`, the NPCs faction will be used. This tag must be paired with a `ReputationChangeAmount` value|
|Allowed Values:|Any Faction Tag|
|Multiple Tag Allowed:|Yes|

<!--ReputationChangeAmount  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReputationChangeAmount|
|:----|:----|
|Tag Format:|`[ReputationChangeAmount:Value]`|
|Description:|Specifies the amount of reputation that is changed with a faction to eligible players if `ChangeReputationWithPlayers` is `true`. This tag must be paired with a `ReputationChangeFactions` value|
|Allowed Values:|Any Integer between -1500 and 1500|
|Multiple Tag Allowed:|Yes|

<!--ReputationChangesForAllRadiusPlayerFactionMembers  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReputationChangesForAllRadiusPlayerFactionMembers|
|:----|:----|
|Tag Format:|`[ReputationChangesForAllRadiusPlayerFactionMembers:Value]`|
|Description:|This tag specifies if reputation made to a player should affect all players in that player's faction.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ChangeAttackerReputation  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeAttackerReputation|
|:----|:----|
|Tag Format:|`[ChangeAttackerReputation:Value]`|
|Description:|This tag specifies if reputation should be changed if a player attacks the NPC.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ChangeAttackerReputationFaction  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeAttackerReputationFaction|
|:----|:----|
|Tag Format:|`[ChangeAttackerReputationFaction:Value]`|
|Description:|Specifies one or more Faction Tags that player reputation is changed with. If you provide the value `{Self}`, the NPCs faction will be used. This tag must be paired with a `ChangeAttackerReputationAmount` value|
|Allowed Values:|Any Faction Tag|
|Multiple Tag Allowed:|Yes|

<!--ChangeAttackerReputationAmount  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeAttackerReputationAmount|
|:----|:----|
|Tag Format:|`[ChangeAttackerReputationAmount:Value]`|
|Description:|Specifies the amount of reputation that is changed with a faction to attacking players if `ChangeReputationWithPlayers` is `true`. This tag must be paired with a `ChangeAttackerReputationFaction` value|
|Allowed Values:|Any Integer between -1500 and 1500|
|Multiple Tag Allowed:|Yes|

<!--ReputationChangesForAllAttackPlayerFactionMembers  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReputationChangesForAllAttackPlayerFactionMembers|
|:----|:----|
|Tag Format:|`[ReputationChangesForAllAttackPlayerFactionMembers:Value]`|
|Description:|This tag specifies if reputation made to a player should affect all players in that player's faction.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!-- ReputationMinCap -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReputationMinCap|
|:----|:----|
|Tag Format:|`[ReputationMinCap:Value]`|
|Description:|This tag allows you to specify the Minimum Reputation level that the action is able to decrease to. If the existing reputation is already lower than the cap, then no further decrease will occur.|
|Allowed Value(s):|Any Integer Greater/Equal To `0`|
|Multiple Tags Allowed:|No|

<!-- ReputationMaxCap -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReputationMaxCap|
|:----|:----|
|Tag Format:|`[ReputationMaxCap:Value]`|
|Description:|This tag allows you to specify the Maximum Reputation level that the action is able to increase to. If the existing reputation is already higher than the cap, then no further increase will occur.|
|Allowed Value(s):|Any Integer Greater/Equal To `0`|
|Multiple Tags Allowed:|No|

***

# Spawning

This section contains actions that allow NPCs to Spawn other NPCs, along with other actions that are triggered via the Modular Encounters Spawner API.

<!--SpawnEncounter  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SpawnEncounter|
|:----|:----|
|Tag Format:|`[SpawnEncounter:Value]`|
|Description:|This tag specifies if an attached Spawn Profile should be activated.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--Spawner  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Spawner|
|:----|:----|
|Tag Format:|`[Spawner:Value]`|
|Description:|This tag specifies which Spawn Profile should be activated if `SpawnEncounter` is `true`.|
|Allowed Values:|Any Spawn Profile SubtypeId|
|Multiple Tag Allowed:|Yes|

<!--CreateKnownPlayerArea  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CreateKnownPlayerArea|
|:----|:----|
|Tag Format:|`[CreateKnownPlayerArea:Value]`|
|Description:|This tag specifies if the area around the current NPC should be designated as a Known Player Area in the Modular Encounters Spawner mod (this tag is only functional if that mod is loaded in the game world).|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--KnownPlayerAreaRadius  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|KnownPlayerAreaRadius|
|:----|:----|
|Tag Format:|`[KnownPlayerAreaRadius:Value]`|
|Description:|Specifies the radius from the current NPC for the Known Player Area if `CreateKnownPlayerArea` is `true`.|
|Allowed Values:|Any Number higher than `0`|
|Multiple Tag Allowed:|No|

<!--KnownPlayerAreaTimer  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|KnownPlayerAreaTimer|
|:----|:----|
|Tag Format:|`[KnownPlayerAreaTimer:Value]`|
|Description:|Specifies the max area time-limit (in minutes) for the Known Player Area if `CreateKnownPlayerArea` is `true`.|
|Allowed Values:|Any Integer equal/higher than `0`|
|Multiple Tag Allowed:|No|

<!--KnownPlayerAreaMaxSpawns  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|KnownPlayerAreaMaxSpawns|
|:----|:----|
|Tag Format:|`[KnownPlayerAreaMaxSpawns:Value]`|
|Description:|Specifies the max amount of spawns for the Known Player Area if `CreateKnownPlayerArea` is `true`.|
|Allowed Values:|Any Integer equal/higher than `-1`|
|Multiple Tag Allowed:|No|

<!--RemoveKnownPlayerArea  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RemoveKnownPlayerArea|
|:----|:----|
|Tag Format:|`[RemoveKnownPlayerArea:Value]`|
|Description:|This tag specifies if a Known Player Location at the current NPC position should be removed. This will only remove locations that match the faction of the NPC or are not designated to an NPC faction.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--RemoveAllKnownPlayerAreas  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RemoveAllKnownPlayerAreas|
|:----|:----|
|Tag Format:|`[RemoveAllKnownPlayerAreas:Value]`|
|Description:|This tag specifies if all Known Player Locations at the current NPC position should be removed, regardless of faction ownership. `RemoveKnownPlayerArea` must also be `true` for this tag to activate.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--AddBotsToGrid-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AddBotsToGrid|
|:----|:----|
|Tag Format:|`[AddBotsToGrid:Value]`|
|Description:|This tag specifies if a number of AiEnabled (External Mod) Bots should be spawned onto the NPC Grid.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--BotCount-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BotCount|
|:----|:----|
|Tag Format:|`[BotCount:Value]`|
|Description:|This tag specifies the number of AiEnabled bots that should be spawned onto the grid.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--BotSpawnProfileNames-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BotSpawnProfileNames|
|:----|:----|
|Tag Format:|`[BotSpawnProfileNames:Value]`|
|Description:|This tag specifies the name(s) of the Spawn Bot Profiles that you want to use to spawn the bots. If multiple values are provided, then each spawn will select one at random.|
|Allowed Values:|Any `Spawn Bot Profile` SubtypeId|
|Multiple Tag Allowed:|Yes|

<!--OnlySpawnBotsInPressurizedRooms-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|OnlySpawnBotsInPressurizedRooms|
|:----|:----|
|Tag Format:|`[OnlySpawnBotsInPressurizedRooms:Value]`|
|Description:|This tag specifies if AiEnabled bots should only be spawned in pressurized spaces on the NPC grid. The location cannot be directly controlled from RivalAI, since the AiEnabled mod is what calculates the safe placement nodes. If you need them to appear in a specific room, you may want to arrange it so that room is the only air-tight space on the grid at the time of spawning.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!-- SpawnPlanet -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SpawnPlanet|
|:----|:----|
|Tag Format:|`[SpawnPlanet:Value]`|
|Description:|This tag specifies if the NPC should spawn an entire freakin planet.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- PlanetName -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PlanetName|
|:----|:----|
|Tag Format:|`[PlanetName:Value]`|
|Description:|This tag specifies the name of the planet being spawned if using `SpawnPlanet`|
|Allowed Value(s):|Any Planet SubtypeId|
|Multiple Tags Allowed:|No|

<!-- PlanetSize -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PlanetSize|
|:----|:----|
|Tag Format:|`[PlanetSize:Value]`|
|Description:|This tag specifies the size (diameter, in meters) of the planet that will be spawned if using `SpawnPlanet`|
|Allowed Value(s):|Any Number Greater/Equal To `100`|
|Multiple Tags Allowed:|No|

<!-- PlanetWaypointProfile -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PlanetWaypointProfile|
|:----|:----|
|Tag Format:|`[PlanetWaypointProfile:Value]`|
|Description:|This tag specifies the name of a Waypoint Profile that will be used to govern the spawning position of the planet if using `SpawnPlanet`|
|Allowed Value(s):|Any Waypoint Profile SubtypeId|
|Multiple Tags Allowed:|No|

<!-- TemporaryPlanet -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TemporaryPlanet|
|:----|:----|
|Tag Format:|`[TemporaryPlanet:Value]`|
|Description:|This tag specifies if the spawned planet should be deleted after a set amount of time if using `SpawnPlanet`|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- PlanetTimeLimit -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PlanetTimeLimit|
|:----|:----|
|Tag Format:|`[PlanetTimeLimit:Value]`|
|Description:|This tag specifies the time (in seconds) before the planet is despawned if using `SpawnPlanet` and `TemporaryPlanet`|
|Allowed Value(s):|Any Integer Greater/Equal To `1`|
|Multiple Tags Allowed:|No|

***

# Targeting

This section contains actions that change or affect targeting.

<!--RefreshTarget  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RefreshTarget|
|:----|:----|
|Tag Format:|`[RefreshTarget:Value]`|
|Description:|This tag specifies if the Behavior should refresh its current target using its own Target Profile rules.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ChangeTargetProfile  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeTargetProfile|
|:----|:----|
|Tag Format:|`[ChangeTargetProfile:Value]`|
|Description:|This tag specifies if the Current Target Profile should be replaced with another. The target evaluation is NOT immediately refreshed when this tag is used, so if you require the new targeting to be used right away, you should also use the `RefreshTarget` tag in your Action.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--NewTargetProfileId  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|NewTargetProfileId|
|:----|:----|
|Tag Format:|`[NewTargetProfileId:Value]`|
|Description:|This tag specifies the new Target Profile you want the behavior to use if `ChangeTargetProfile` is set to `true`.|
|Allowed Values:|Valid Target Profile SubtypeId|
|Multiple Tag Allowed:|No|

***

# Trigger 

This section contains actions that allow you to change properties of Triggers on the current NPC.

<!--ResetCooldownTimeOfTriggers  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ResetCooldownTimeOfTriggers|
|:----|:----|
|Tag Format:|`[ResetCooldownTimeOfTriggers:Value]`|
|Description:|This tag specifies if one or more Trigger Profiles should have their current cooldown timer reset.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ResetTriggerCooldownNames  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ResetTriggerCooldownNames|
|:----|:----|
|Tag Format:|`[ResetTriggerCooldownNames:Value]`|
|Description:|This tag specifies a name of a Trigger Profile that should have its cooldown timer reset.|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--EnableTriggers  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EnableTriggers|
|:----|:----|
|Tag Format:|`[EnableTriggers:Value]`|
|Description:|This tag specifies if one or more Trigger Profiles should be enabled (ie, UseTrigger set to true).|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--EnableTriggerNames  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EnableTriggerNames|
|:----|:----|
|Tag Format:|`[EnableTriggerNames:Value]`|
|Description:|This tag specifies a name of a Trigger Profile that should have be Enabled.|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--DisableTriggers  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DisableTriggers|
|:----|:----|
|Tag Format:|`[DisableTriggers:Value]`|
|Description:|This tag specifies if one or more Trigger Profiles should be disabled (ie, UseTrigger set to false).|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--DisableTriggerNames  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DisableTriggerNames|
|:----|:----|
|Tag Format:|`[DisableTriggerNames:Value]`|
|Description:|This tag specifies a name of a Trigger Profile that should have be Disabled.|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--ManuallyActivateTrigger  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManuallyActivateTrigger|
|:----|:----|
|Tag Format:|`[ManuallyActivateTrigger:Value]`|
|Description:|This tag specifies if one or more Trigger Profiles should be manually triggered.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ManuallyActivatedTriggerNames  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManuallyActivatedTriggerNames|
|:----|:----|
|Tag Format:|`[ManuallyActivatedTriggerNames:Value]`|
|Description:|This tag specifies a name of a Trigger Profile that should be Manually Triggered.|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--ForceManualTriggerActivation-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ForceManualTriggerActivation|
|:----|:----|
|Tag Format:|`[ForceManualTriggerActivation:Value]`|
|Description:|This tag specifies if manually activated triggers should be forcibly activated from the action, regardless of their conditions, cooldowns, etc.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

***

# Variables

This section contains actions that change local or global variables that can be used by the behavior or spawner.

<!--SetBooleansTrue  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SetBooleansTrue|
|:----|:----|
|Tag Format:|`[SetBooleansTrue:Value]`|
|Description:|This tag specifies a name of a Boolean Variable that you want to set to `true` within the Behavior Profile. If a provided name does not exist in the Behavior Profile, it will be created and saved with the `true` value|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--SetBooleansFalse  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SetBooleansFalse|
|:----|:----|
|Tag Format:|`[SetBooleansFalse:Value]`|
|Description:|This tag specifies a name of a Boolean Variable that you want to set to `false` within the Behavior Profile. If a provided name does not exist in the Behavior Profile, it will be created and saved with the `false` value|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--IncreaseCounters  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IncreaseCounters|
|:----|:----|
|Tag Format:|`[IncreaseCounters:Value]`|
|Description:|This tag specifies a name of an Integer Variable that you want to increase by `1` within the Behavior Profile. If a provided name does not exist in the Behavior Profile, it will be created and saved with a `1` value|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--DecreaseCounters  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DecreaseCounters|
|:----|:----|
|Tag Format:|`[DecreaseCounters:Value]`|
|Description:|This tag specifies a name of an Integer Variable that you want to decrease by `1` within the Behavior Profile. If a provided name does not exist in the Behavior Profile, it will be created and saved with a `-1` value|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--SetCounters  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SetCounters|
|:----|:----|
|Tag Format:|`[SetCounters:Value]`|
|Description:|This tag specifies a name of an Integer Variable that you want to set to a specific value within the behavior. If a provided name does not exist in the behavior, it will be created and saved with the provided value. Ensure you also provide the value you want to set the variable to using the `SetCountersValues` tag|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--SetCountersValues  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SetCountersValues|
|:----|:----|
|Tag Format:|`[SetCountersValues:Value]`|
|Description:|This tag specifies the value of an Integer Variable within the behavior. Ensure you also provide the name of the variable you want to set the value of using the `SetCounters` tag.|
|Allowed Values:|Any Integer Value|
|Multiple Tag Allowed:|Yes|

<!--ResetCounters  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ResetCounters|
|:----|:----|
|Tag Format:|`[ResetCounters:Value]`|
|Description:|This tag specifies a name of an Integer Variable that you want to set to a value of `0` within the Behavior Profile. If a provided name does not exist in the Behavior Profile, it will be created and saved with a `0` value|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--SetSandboxBooleansTrue  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SetSandboxBooleansTrue|
|:----|:----|
|Tag Format:|`[SetSandboxBooleansTrue:Value]`|
|Description:|This tag specifies a name of a Boolean Variable that you want to set to `true` within the Save File. If a provided name does not exist in the Save File, it will be created and saved with the `true` value|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--SetSandboxBooleansFalse  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SetSandboxBooleansFalse|
|:----|:----|
|Tag Format:|`[SetSandboxBooleansFalse:Value]`|
|Description:|This tag specifies a name of a Boolean Variable that you want to set to `false` within the Save File. If a provided name does not exist in the Save File, it will be created and saved with the `false` value|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--IncreaseSandboxCounters  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IncreaseSandboxCounters|
|:----|:----|
|Tag Format:|`[IncreaseSandboxCounters:Value]`|
|Description:|This tag specifies a name of an Integer Variable that you want to increase by `1` within the Save File. If a provided name does not exist in the Save File, it will be created and saved with a `1` value|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--IncreaseSandboxCountersAmount-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IncreaseSandboxCountersAmount|
|:----|:----|
|Tag Format:|`[IncreaseSandboxCountersAmount:Value]`|
|Description:|This tag specifies the amount that all sandbox counters will be increased by if also using `IncreaseSandboxCounters` tags.|
|Allowed Values:|Any Integer|
|Multiple Tag Allowed:|No|

<!--DecreaseSandboxCounters  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DecreaseSandboxCounters|
|:----|:----|
|Tag Format:|`[DecreaseSandboxCounters:Value]`|
|Description:|This tag specifies a name of an Integer Variable that you want to decrease by `1` within the Save File. If a provided name does not exist in the Save File, it will be created and saved with a `-1` value|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--DecreaseSandboxCountersAmount-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DecreaseSandboxCountersAmount|
|:----|:----|
|Tag Format:|`[DecreaseSandboxCountersAmount:Value]`|
|Description:|This tag specifies the amount that all sandbox counters will be decreased by if also using `DecreaseSandboxCounters` tags.|
|Allowed Values:|Any Integer|
|Multiple Tag Allowed:|No|

<!--SetSandboxCounters  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SetSandboxCounters|
|:----|:----|
|Tag Format:|`[SetSandboxCounters:Value]`|
|Description:|This tag specifies a name of an Integer Variable that you want to set to a specific value within the Save File. If a provided name does not exist in the Save File, it will be created and saved with the provided value. Ensure you also provide the value you want to set the variable to using the `SetSandboxCounters` tag|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--SetSandboxCountersValues  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SetSandboxCountersValues|
|:----|:----|
|Tag Format:|`[SetSandboxCountersValues:Value]`|
|Description:|This tag specifies the value of an Integer Variable within the Save File. Ensure you also provide the name of the variable you want to set the value of using the `SetSandboxCounters` tag.|
|Allowed Values:|Any Integer Value|
|Multiple Tag Allowed:|Yes|

<!--ResetSandboxCounters  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ResetSandboxCounters|
|:----|:----|
|Tag Format:|`[ResetSandboxCounters:Value]`|
|Description:|This tag specifies a name of an Integer Variable that you want to set to a value of `0` within the Save File. If a provided name does not exist in the Save File, it will be created and saved with a `0` value|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--AddCustomDataToBlocks-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AddCustomDataToBlocks|
|:----|:----|
|Tag Format:|`[AddCustomDataToBlocks:Value]`|
|Description:|This tag specifies if one or more block should have their CustomData set to a specified value using a TextTemplate xml file.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--CustomDataBlockNames-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomDataBlockNames|
|:----|:----|
|Tag Format:|`[CustomDataBlockNames:Value]`|
|Description:|This tag specifies one or more Block Names of blocks that will have their CustomData changed if using the `AddCustomDataToBlocks` tag. This tag should be paired with the `CustomDataFiles` tag.|
|Allowed Values:|Any text/string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--CustomDataFiles-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomDataFiles|
|:----|:----|
|Tag Format:|`[CustomDataFiles:Value]`|
|Description:|This tag specifies one or more TextTemplate files that will be used to set block custom data if using the `AddCustomDataToBlocks` tag. This tag should be paired with the `CustomDataBlockNames` tag.|
|Allowed Values:|TextTemplate file name<br />eg: `SomeTextTemplate.xml`|
|Multiple Tag Allowed:|Yes|

***

# Zone

This section contains actions that can change / manipulate Zones (formerly Territories) in the game world.

<!--ChangeZoneAtPosition-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ChangeZoneAtPosition|
|:----|:----|
|Tag Format:|`[ChangeZoneAtPosition:Value]`|
|Description:|This tag specifies if a zone at the NPCs current position should be manipulated.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ZoneName-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneName|
|:----|:----|
|Tag Format:|`[ZoneName:Value]`|
|Description:|This tag specifies the name of the specific zone that should be manipulated.|
|Allowed Values:|Any Zone PublicName value|
|Multiple Tag Allowed:|No|

<!--ZoneToggleActive-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneToggleActive|
|:----|:----|
|Tag Format:|`[ZoneToggleActive:Value]`|
|Description:|This tag specifies if the named zone in `ZoneName` should have its `Active` status changed. The NPC does not need to be inside the zone for this to be functional.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ZoneToggleActiveMode-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneToggleActiveMode|
|:----|:----|
|Tag Format:|`[ZoneToggleActive:Value]`|
|Description:|This tag specifies what the `Active` status of a zone should be set to if using `ZoneToggleActive`. The NPC does not need to be inside the zone for this to be functional.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ZoneToggleActiveAtPosition-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneToggleActiveAtPosition|
|:----|:----|
|Tag Format:|`[ZoneToggleActiveAtPosition:Value]`|
|Description:|This tag specifies if the named zone in `ZoneName` should have its `Active` status changed. The NPC needs to be inside the zone for this to be functional.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ZoneToggleActiveAtPositionMode-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneToggleActiveAtPositionMode|
|:----|:----|
|Tag Format:|`[ZoneToggleActiveAtPositionMode:Value]`|
|Description:|This tag specifies what the `Active` status of a zone should be set to if using `ZoneToggleActiveAtPosition`. The NPC needs to be inside the zone for this to be functional.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ZoneRadiusChangeType-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneRadiusChangeType|
|:----|:----|
|Tag Format:|`[ZoneRadiusChangeType:Value]`|
|Description:|This tag specifies if the zone that the NPC is inside (also matching `ZoneName`) should have its `Radius` changed. You can choose a modifier in this tag that will be used in combination with `ZoneRadiusChangeType`|
|Allowed Values:|`None`<br>`Set`<br>`Add`<br>`Subtract`<br>`Multiply`<br>`Divide`|
|Multiple Tag Allowed:|No|

<!--ZoneRadiusChangeAmount-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneRadiusChangeAmount|
|:----|:----|
|Tag Format:|`[ZoneRadiusChangeAmount:Value]`|
|Description:|This tag specifies how much the zone radius will be changed if using the `ZoneRadiusChangeType` tag.|
|Allowed Values:|Any Number|
|Multiple Tag Allowed:|No|

<!--ZoneCustomCounterChange-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneCustomCounterChange|
|:----|:----|
|Tag Format:|`[ZoneToggleActiveAtPositionMode:Value]`|
|Description:|This tag specifies if the Custom Counters of the zone (at NPC position and matching `ZoneName`) should be manipulated.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ZoneCustomCounterChangeType-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneCustomCounterChangeType|
|:----|:----|
|Tag Format:|`[ZoneCustomCounterChangeType:Value]`|
|Description:|This tag specifies how the value of a zone's Custom Counter will be changed.|
|Allowed Values:|`None`<br>`Set`<br>`Add`<br>`Subtract`<br>`Multiply`<br>`Divide`|
|Multiple Tag Allowed:|Yes|

<!--ZoneCustomCounterChangeName-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneCustomCounterChangeName|
|:----|:----|
|Tag Format:|`[ZoneCustomCounterChangeName:Value]`|
|Description:|This tag specifies the name of the zone's Custom Counter that will be changed.|
|Allowed Values:|Any Counter Name|
|Multiple Tag Allowed:|Yes|

<!--ZoneCustomCounterChangeAmount-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneCustomCounterChangeAmount|
|:----|:----|
|Tag Format:|`[ZoneCustomCounterChangeAmount:Value]`|
|Description:|This tag specifies the amount that the counter specified by `ZoneCustomCounterChangeName` will be changed.|
|Allowed Values:|Any Integer|
|Multiple Tag Allowed:|Yes|

<!--ZoneCustomBoolChange-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneCustomBoolChange|
|:----|:----|
|Tag Format:|`[ZoneCustomBoolChange:Value]`|
|Description:|This tag specifies if the Custom Bools of the zone (at NPC position and matching `ZoneName`) should be manipulated.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ZoneCustomBoolChangeName-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneCustomBoolChangeName|
|:----|:----|
|Tag Format:|`[ZoneCustomBoolChangeName:Value]`|
|Description:|This tag specifies the name of the zone's Custom Bool that will be changed.|
|Allowed Values:|Any Bool Name|
|Multiple Tag Allowed:|Yes|

<!--ZoneCustomBoolChangeValue-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneCustomBoolChangeValue|
|:----|:----|
|Tag Format:|`[ZoneCustomBoolChangeValue:Value]`|
|Description:|This tag specifies the value that the bool specified by `ZoneCustomBoolChangeName` will be changed.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|Yes|
