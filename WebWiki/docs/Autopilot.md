#Autopilot.md

Autopilot Profiles in RivalAI allow you to specify a set of rules that are use for how the NPC ship moves to various waypoints and targets. You can attach your Autopilot Profiles to any Behavior Profile by adding a `[AutopilotData:Value]` tag to the Behavior and replace `Value` with the SubtypeId of your Autopilot Profile. Example:

`[AutopilotData:RAI-ExampleAutopilotProfile]`

It is important that you use a unique SubtypeId for each Autopilot Profile you create, otherwise they may not work correctly.

Here's an example of how a Autopilot Profile Definition is setup:  

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>RAI-ExampleAutopilotProfile</SubtypeId>
      </Id>
      <Description>

      [RivalAI Autopilot]
      
      [SlowDownOnWaypointApproach:true]
      [ExtraSlowDownDistance:150]
      [FlyLevelWithGravity:false]
      
      [AllowStrafing:true]
      [StrafeMinDurationMs:1000]
      [StrafeMaxDurationMs:1500]
      
      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

It's also important to note that all the tags below can also be used in the main `Behavior` profile as well. However, you must be careful when doing so. If you add individual tags before defining an entire `AutopilotData` tag, all those changes will be overwritten by the Autopilot Profile provided. If provided after the `AutopilotData` tag, that should be safe - and can be an easy way to make minor adjustments without having to provide an entirely new autopilot profile!

Below are the tags you are able to use in your Autopilot Profiles. They are divided into several categories based on what they control.

**[Behavior](#Behavior)**  
**[Collision](#Collision)**  
**[General](#General)**  
**[Leading](#Leading)**  
**[Offset](#Offset)**  
**[Planet](#Planet)**  
**[Rotation](#Rotation)**  
**[Special](#Special)**  
**[Speed](#Speed)**  
**[Strafe](#Strafe)**  
**[Thrust](#Thrust)**  

***

# Behavior

This section contains tags that only work with specific behaviors.

<!--EngageDistanceSpace-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EngageDistanceSpace|
|:----|:----|
|Tag Format:|`[EngageDistanceSpace:Value]`|
|Description:|This tag specifies the max distance from a target waypoint (while in space) before an NPC will switch to its `EngageTarget` mode (which usually involves rotating to target and strafe).|
|Allowed Values:|Any Number Greater Than `0`<br>Value Must be `Equal or Lower` than `DisengageDistanceSpace`|
|Behaviors:|`Fighter`<br>`HorseFighter`<br>`Hunter`<br>`Nautical`|
|Multiple Tag Allowed:|No|

<!--DisengageDistanceSpace-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DisengageDistanceSpace|
|:----|:----|
|Tag Format:|`[DisengageDistanceSpace:Value]`|
|Description:|This tag specifies the minimum distance from a target waypoint (while in space) that an NPC will remain in `EngageTarget` mode (which usually involves rotating to target and strafe), otherwise will be switched back to a mode where it approach the target instead.|
|Allowed Values:|Any Number Greater Than `0`<br>Value Must be `Equal or Higher` than `EngageDistanceSpace`|
|Behaviors:|`Fighter`<br>`HorseFighter`<br>`Hunter`<br>`Nautical`|
|Multiple Tag Allowed:|No|

<!--EngageDistancePlanet-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EngageDistancePlanet|
|:----|:----|
|Tag Format:|`[EngageDistancePlanet:Value]`|
|Description:|This tag specifies the max distance from a target waypoint (while in gravity) before an NPC will switch to its `EngageTarget` mode (which usually involves rotating to target and strafe).|
|Allowed Values:|Any Number Greater Than `0`<br>Value Must be `Equal or Lower` than `DisengageDistanceSpace`|
|Behaviors:|`Fighter`<br>`HorseFighter`<br>`Hunter`<br>`Nautical`|
|Multiple Tag Allowed:|No|

<!--DisengageDistancePlanet-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DisengageDistancePlanet|
|:----|:----|
|Tag Format:|`[DisengageDistancePlanet:Value]`|
|Description:|This tag specifies the minimum distance from a target waypoint (while in gravity) that an NPC will remain in `EngageTarget` mode (which usually involves rotating to target and strafe), otherwise will be switched back to a mode where it approach the target instead.|
|Allowed Values:|Any Number Greater Than `0`<br>Value Must be `Equal or Higher` than `EngageDistancePlanet`|
|Behaviors:|`Fighter`<br>`HorseFighter`<br>`Hunter`<br>`Nautical`|
|Multiple Tag Allowed:|No|

<!--WaypointWaitTimeTrigger-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|WaypointWaitTimeTrigger|
|:----|:----|
|Tag Format:|`[WaypointWaitTimeTrigger:Value]`|
|Description:|This tag specifies the time that an NPC will wait at a waypoint after reaching it.|
|Allowed Values:|Any Number Greater Than `0`|
|Behaviors:|`CargoShip`<br>`HorseFighter`<br>`Horsefly`|
|Multiple Tag Allowed:|No|

<!--WaypointAbandonTimeTrigger-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|WaypointAbandonTimeTrigger|
|:----|:----|
|Tag Format:|`[WaypointAbandonTimeTrigger:Value]`|
|Description:|This tag specifies the time until an NPC will recalculate their current waypoint if they have not reached it yet.|
|Allowed Values:|Any Number Greater Than `0`|
|Behaviors:|`HorseFighter`<br>`Horsefly`|
|Multiple Tag Allowed:|No|

# Collision

This section contains tags that control collision detection.  

<!--UseVelocityCollisionEvasion-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseVelocityCollisionEvasion|
|:----|:----|
|Tag Format:|`[UseVelocityCollisionEvasion:Value]`|
|Description:|This tag specifies if the behavior should detect and attempt avoiding collisions in the direction the NPC is travelling in.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--CollisionEvasionWaypointDistance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CollisionEvasionWaypointDistance|
|:----|:----|
|Tag Format:|`[CollisionEvasionWaypointDistance:Value]`|
|Description:|This tag specifies how far from an NPCs current position it should try to move off its current path when avoiding a collision.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--CollisionFallEvasionWaypointDistance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CollisionFallEvasionWaypointDistance|
|:----|:----|
|Tag Format:|`[CollisionFallEvasionWaypointDistance:Value]`|
|Description:|This tag specifies the distance from an NPCs current position it should try to fly to if attempting to recover from a fall collision on a planet. Fall collisions are considered if your NPCs velocity direction is within 15 degrees of the planet center direction.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--CollisionEvasionResumeDistance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CollisionEvasionResumeDistance|
|:----|:----|
|Tag Format:|`[CollisionEvasionResumeDistance:Value]`|
|Description:|This tag specifies the distance from a collision evasion waypoint the NPC must reach before it resumes its regular path.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--CollisionEvasionResumeTime-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CollisionEvasionResumeTime|
|:----|:----|
|Tag Format:|`[CollisionEvasionResumeTime:Value]`|
|Description:|This tag specifies the maximum time an NPC can spend trying to reach a collision evasion waypoint before it will abandon the attempt and resume its regular path.|
|Allowed Values:|Any Integer Greater Than `0`|
|Multiple Tag Allowed:|No|


# General  

This section contains tags that don't quite fit in the other categories.  

<!--DisableInertiaDampeners-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DisableInertiaDampeners|
|:----|:----|
|Tag Format:|`[DisableInertiaDampeners:Value]`|
|Description:|This tag specifies if the ship Inertia Dampeners should be disabled. For most behaviors, this is not recommended, but the tag exists for cases if/when it might be necessary.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--WaypointTolerance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|WaypointTolerance|
|:----|:----|
|Tag Format:|`[WaypointTolerance:Value]`|
|Description:|This tag specifies the distance from a waypoint or target before some behaviors will perform other actions.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--UseVerticalRetreat-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseVerticalRetreat|
|:----|:----|
|Tag Format:|`[UseVerticalRetreat:Value]`|
|Description:|This tag specifies if the Despawn coordinates should be calculated in an upward direction when an NPC is retreating.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|


# Leading

This section contains tags that control if NPC should lead a target with weapons.  

<!--UseProjectileLeadPrediction-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseProjectileLeadPrediction|
|:----|:----|
|Tag Format:|`[UseProjectileLeadPrediction:Value]`|
|Description:|This tag specifies if a ship should try to aim ahead of a moving target so projectiles have a better chance to hit.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

# Offset

This section contains tags that control the offset distances some behaviors will use.  

<!--OffsetSpaceMinDistFromTarget-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|OffsetSpaceMinDistFromTarget|
|:----|:----|
|Tag Format:|`[OffsetSpaceMinDistFromTarget:Value]`|
|Description:|This tag specifies the minimum distance from a target (in space) that some behaviors will create a random offset position at.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--OffsetSpaceMaxDistFromTarget-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|OffsetSpaceMaxDistFromTarget|
|:----|:----|
|Tag Format:|`[OffsetSpaceMaxDistFromTarget:Value]`|
|Description:|This tag specifies the maximum distance from a target (in space) that some behaviors will create a random offset position at.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--OffsetPlanetMinDistFromTarget-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|OffsetPlanetMinDistFromTarget|
|:----|:----|
|Tag Format:|`[OffsetPlanetMinDistFromTarget:Value]`|
|Description:|This tag specifies the minimum distance from a target (on planet) that some behaviors will create a random offset position at.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--OffsetPlanetMaxDistFromTarget-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|OffsetPlanetMaxDistFromTarget|
|:----|:----|
|Tag Format:|`[OffsetPlanetMaxDistFromTarget:Value]`|
|Description:|This tag specifies the maximum distance from a target (on planet) that some behaviors will create a random offset position at.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--OffsetPlanetMinTargetAltitude-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|OffsetPlanetMinTargetAltitude|
|:----|:----|
|Tag Format:|`[OffsetPlanetMinTargetAltitude:Value]`|
|Description:|This tag specifies the minimum altitude from a target (on planet) that some behaviors will create a random offset position at.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--OffsetPlanetMaxTargetAltitude-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|OffsetPlanetMaxTargetAltitude|
|:----|:----|
|Tag Format:|`[OffsetPlanetMaxTargetAltitude:Value]`|
|Description:|This tag specifies the maximum altitude from a target (on planet) that some behaviors will create a random offset position at.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--ReverseOffsetDistAltAboveHeight-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReverseOffsetDistAltAboveHeight|
|:----|:----|
|Tag Format:|`[ReverseOffsetDistAltAboveHeight:Value]`|
|Description:|This tag specifies if the min and max values of the OffsetPlanet tags should be swapped (altitude becomes distance, distance becomes altitude) if the target is above a certain altitude. This is useful for behaviors that might want to stay high above targets while their near the ground, and fly further away when they're higher in the air.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ReverseOffsetHeight-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ReverseOffsetHeight|
|:----|:----|
|Tag Format:|`[ReverseOffsetHeight:Value]`|
|Description:|This tag specifies the maximum altitude a target can be from the ground before the altitude and distance offset distances are swapped. Only applies when `ReverseOffsetDistAltAboveHeight` is `true`|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--PadDistanceFromTarget-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PadDistanceFromTarget|
|:----|:----|
|Tag Format:|`[PadDistanceFromTarget:Value]`|
|Description:|This tag specifies extra distance from a target (in the direction of the NPC) that will be considered its target coordinates.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

# Planet

This section contains tags that control how behaviors react to planetary terrain.  

<!--FlyLevelWithGravity-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|FlyLevelWithGravity|
|:----|:----|
|Tag Format:|`[FlyLevelWithGravity:Value]`|
|Description:|This tag specifies if a ship should fly perpendicular to the planet gravity direction, keeping level with the ground similar to how vanilla autopilot does. This is more useful on larger ships, or ships that use turrets as a primary offensive choice.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--LevelWithGravityWhenIdle-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LevelWithGravityWhenIdle|
|:----|:----|
|Tag Format:|`[LevelWithGravityWhenIdle:Value]`|
|Description:|This tag specifies if a ship should rotate perpendicular to the planet gravity direction, keeping level with the ground similar to how vanilla autopilot does. This would happen when a ship arrives and is waiting at a waypoint.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--IdealPlanetAltitude-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IdealPlanetAltitude|
|:----|:----|
|Tag Format:|`[IdealPlanetAltitude:Value]`|
|Description:|This tag specifies the ideal altitude an NPC should travel at while travelling to a waypoint or target on a planet.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--MinimumPlanetAltitude-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinimumPlanetAltitude|
|:----|:----|
|Tag Format:|`[MinimumPlanetAltitude:Value]`|
|Description:|This tag specifies the minimum distance an NPC should travel at while travelling to a waypoint or target on a planet. If the NPC needs to adjust its altitude at any point, it will attempt to climb back to the `IdealPlanetAltitude` value|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--AltitudeTolerance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AltitudeTolerance|
|:----|:----|
|Tag Format:|`[AltitudeTolerance:Value]`|
|Description:|This tag specifies the distance from the `IdealPlanetAltitude` value that the NPC must be within to resume its regular path travel if it was correcting altitude.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--MinAngleForLeveledDescent-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinAngleForLeveledDescent|
|:----|:----|
|Tag Format:|`[MinAngleForLeveledDescent:Value]`|
|Description:|This tag specifies the minimum angle using the direction from the NPC to the waypoint and the direction from the NPC to UP before the NPC will begin to descend to the waypoint's altitude. This only works with `FlyLevelWithGravity` enabled. Default value is `0`|
|Allowed Values:|`0` to `180`|
|Multiple Tag Allowed:|No|

<!--MaxAngleForLeveledAscent-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxAngleForLeveledAscent|
|:----|:----|
|Tag Format:|`[MaxAngleForLeveledAscent:Value]`|
|Description:|This tag specifies the maximum angle using the direction from the NPC to the waypoint and the direction from the NPC to UP that the NPC is allowed to ascend to the waypoint's altitude. This only works with `FlyLevelWithGravity` enabled. Default value is `180`|
|Allowed Values:|`0` to `180`|
|Multiple Tag Allowed:|No|

<!--MaxVerticalSpeed-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxVerticalSpeed|
|:----|:----|
|Tag Format:|`[MaxVerticalSpeed:Value]`|
|Description:|This tag specifies the maximum speed that a ship is allow to move on the Y axis (up and down) while using the `FlyLevelWithGravity` tag.|
|Allowed Values:|Any Number Greater or Equal to `0`|
|Multiple Tag Allowed:|No|

<!--UseSurfaceHoverThrustMode-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseSurfaceHoverThrustMode|
|:----|:----|
|Tag Format:|`[UseSurfaceHoverThrustMode:Value]`|
|Description:|This tag specifies if a ship should try to maintain the `IdealPlanetAltitude` at all times, regardless of the actual target/waypoint altitude.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--HoverPathStepDistance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|HoverPathStepDistance|
|:----|:----|
|Tag Format:|`[HoverPathStepDistance:Value]`|
|Description:|This tag specifies the distance ahead of the ship that it will check the next terrain elevation at. It will also use this value multiplied by 4 when detecting whether or not it needs to ascend up a steep hill of cliff.|
|Allowed Values:|Any Number Greater or Equal to `0`|
|Multiple Tag Allowed:|No|

<!--UseWaterPatrolMode-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseWaterPatrolMode|
|:----|:----|
|Tag Format:|`[UseWaterPatrolMode:Value]`|
|Description:|This tag specifies if a ship should use water related navigation similar to what is found in the `Nautical` behavior. This only works with certain behavior subclasses.|
|Allowed Values:|`true`<br>`false`|
|Behaviors:|`Escort`<br>`Patrol`|
|Multiple Tag Allowed:|No|

# Rotation

This section contains tags that control rotation of the NPC grid.  

<!--RotationMultiplier-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RotationMultiplier|
|:----|:----|
|Tag Format:|`[RotationMultiplier:Value]`|
|Description:|This tag specifies a multiplier that is applied to gyro rotation calculations for some behaviors. `2` would be double, while `0.5` would be half. Keep in mind this does multiply the gyro force, only the strength or rotation in each direction.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--LimitRotationSpeed-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LimitRotationSpeed|
|:----|:----|
|Tag Format:|`[LimitRotationSpeed:Value]`|
|Description:|This tag specifies if the NPC ship should limit rotation magnitude on each of its rotation axis to the amount specified in `MaxRotationMagnitude`. This tag is useful for reducing 'targeting wobble' with grids that have stronger gyro rotation.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--MaxRotationMagnitude-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxRotationMagnitude|
|:----|:----|
|Tag Format:|`[MaxRotationMagnitude:Value]`|
|Description:|This tag specifies the maximum magnitude (in radians) that each rotation axis is allowed to rotate at if `LimitRotationSpeed` is true.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

# Special

This section contains tags for special autopilot modes such as Barrel Roll and Ramming.  

<!--BarrelRollMinDurationMs-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BarrelRollMinDurationMs|
|:----|:----|
|Tag Format:|`[BarrelRollMinDurationMs:Value]`|
|Description:|This tag specifies the minimum time (in ms) a ship will spend rolling if BarrelRoll is activated from an Action Profile.|
|Allowed Values:|Any Number Greater Than `0`<br>Value should be lower than `BarrelRollMaxDurationMs` if tag is provided.|
|Multiple Tag Allowed:|No|

<!--BarrelRollMaxDurationMs-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BarrelRollMaxDurationMs|
|:----|:----|
|Tag Format:|`[BarrelRollMaxDurationMs:Value]`|
|Description:|This tag specifies the maximum time (in ms) a ship will spend rolling if BarrelRoll is activated from an Action Profile.|
|Allowed Values:|Any Number Greater Than `0`<br>Value should be higher than `BarrelRollMinDurationMs` if tag is provided.|
|Multiple Tag Allowed:|No|

<!--RamMinDurationMs-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RamMinDurationMs|
|:----|:----|
|Tag Format:|`[RamMinDurationMs:Value]`|
|Description:|This tag specifies the minimum time (in ms) a ship will spend attempting to fly into a target if Ramming is activated from an Action Profile.|
|Allowed Values:|Any Number Greater Than `0`<br>Value should be lower than `RamMaxDurationMs` if tag is provided.|
|Multiple Tag Allowed:|No|

<!--RamMaxDurationMs-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RamMaxDurationMs|
|:----|:----|
|Tag Format:|`[RamMaxDurationMs:Value]`|
|Description:|This tag specifies the maximum time (in ms) a ship will spend attempting to fly into a target if Ramming is activated from an Action Profile.|
|Allowed Values:|Any Number Greater Than `0`<br>Value should be higher than `RamMinDurationMs` if tag is provided.|
|Multiple Tag Allowed:|No|

# Speed

This section contains tags that control speed of the NPC grid.  

<!--IdealMaxSpeed-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IdealMaxSpeed|
|:----|:----|
|Tag Format:|`[IdealMaxSpeed:Value]`|
|Description:|This tag specifies the max speed that an NPC should attempt to travel at.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--IdealMinSpeed-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IdealMinSpeed|
|:----|:----|
|Tag Format:|`[IdealMinSpeed:Value]`|
|Description:|This tag specifies the minimum speed that a ship will try to slow down to when the `SlowDownOnWaypointApproach` tag is used.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--MaxSpeedTolerance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxSpeedTolerance|
|:----|:----|
|Tag Format:|`[MaxSpeedTolerance:Value]`|
|Description:|This tag specifies the tolerance that is allowed for max speed. If speed is above or below `IdealMaxSpeed` by more than this value, speed correction will occur.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--SlowDownOnWaypointApproach-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SlowDownOnWaypointApproach|
|:----|:----|
|Tag Format:|`[SlowDownOnWaypointApproach:Value]`|
|Description:|This tag specifies if the NPC ship should attempt to slowdown upon approaching a waypoint. The slowdown distance is automatically calculated using the weight and braking force of the ship.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--ExtraSlowDownDistance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ExtraSlowDownDistance|
|:----|:----|
|Tag Format:|`[ExtraSlowDownDistance:Value]`|
|Description:|This tag specifies extra stopping distance that is added to the calculation when the `SlowDownOnWaypointApproach` tag is used.|
|Allowed Values:|Any Number Equal or Greater Than `0`|
|Multiple Tag Allowed:|No|

# Strafe

This section contains tags that control strafing manuevers for behaviors that are capable of using it. 

<!--AllowStrafing-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AllowStrafing|
|:----|:----|
|Tag Format:|`[AllowStrafing:Value]`|
|Description:|This tag specifies if Strafing should be enabled for behaviors that are able to use it.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--StrafeMinDurationMs-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StrafeMinDurationMs|
|:----|:----|
|Tag Format:|`[StrafeMinDurationMs:Value]`|
|Description:|This tag specifies the minimum time (in milliseconds) that a strafe manuever should last.|
|Allowed Values:|Any Integer Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--StrafeMaxDurationMs-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StrafeMaxDurationMs|
|:----|:----|
|Tag Format:|`[StrafeMaxDurationMs:Value]`|
|Description:|This tag specifies the maximum time (in milliseconds) that a strafe manuever should last.|
|Allowed Values:|Any Integer Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--StrafeMinCooldownMs-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StrafeMinCooldownMs|
|:----|:----|
|Tag Format:|`[StrafeMinCooldownMs:Value]`|
|Description:|This tag specifies the minimum time (in milliseconds) the behavior should wait between strafe manuevers.|
|Allowed Values:|Any Integer Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--StrafeMaxCooldownMs-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StrafeMaxCooldownMs|
|:----|:----|
|Tag Format:|`[StrafeMaxCooldownMs:Value]`|
|Description:|This tag specifies the maximum time (in milliseconds) the behavior should wait between strafe manuevers.|
|Allowed Values:|Any Integer Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--StrafeSpeedCutOff-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StrafeSpeedCutOff|
|:----|:----|
|Tag Format:|`[StrafeSpeedCutOff:Value]`|
|Description:|This tag specifies the speed an NPC must reach before a strafing manuever is terminated, regardless of time remaining for the strafe.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--StrafeDistanceCutOff-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StrafeDistanceCutOff|
|:----|:----|
|Tag Format:|`[StrafeDistanceCutOff:Value]`|
|Description:|This tag specifies the distance from where the strafe began that an NPC must reach before the strafing manuever is terminated, regardless of time remaining for the strafe.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--StrafeMinimumTargetDistance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StrafeMinimumTargetDistance|
|:----|:----|
|Tag Format:|`[StrafeMinimumTargetDistance:Value]`|
|Description:|This tag specifies the minimum distance from a target that the NPC must be at to be allowed to strafe in the direction of the target.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--StrafeMinimumSafeAngleFromTarget-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StrafeMinimumSafeAngleFromTarget|
|:----|:----|
|Tag Format:|`[StrafeMinimumSafeAngleFromTarget:Value]`|
|Description:|This tag specifies the minimum angle from a target for the strafe direction to be considered "in the direction of the target". Likely does not need to be changed from default value of `25` in most cases.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|


# Thrust

This section contains tags that control thrust properties. 

<!--AngleAllowedForForwardThrust-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AngleAllowedForForwardThrust|
|:----|:----|
|Tag Format:|`[AngleAllowedForForwardThrust:Value]`|
|Description:|This tag specifies the maximum angle from the forward direction of an NPC before it will engage thrusters to travel to it. Only applicable to behaviors that do not use Keen Autopilot|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--MaxVelocityAngleForSpeedControl-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxVelocityAngleForSpeedControl|
|:----|:----|
|Tag Format:|`[MaxVelocityAngleForSpeedControl:Value]`|
|Description:|This tag specifies the maximum angle from the ship current velocity direction before it will attempt to use speed control.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|