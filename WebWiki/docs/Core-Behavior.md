#Core-Behavior.md

Behavior Profiles in Rival AI are how you give your NPC grids special behaviors. After specifying a `BehaviorName` tag that gives your behavior its basic movement patterns, you can then attach all sorts of other tags to the behavior that define its Autopilot rules, Targeting rules, and Triggers that execute when certain conditions are met!

Here is an example of how a Behavior Profile definition is setup:

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

      [RivalAI Behavior]
      
      [BehaviorName:Horsefly]
	
      [AutopilotData:RAI-ExampleAutopilotProfile]
      [TargetData:RAI-ExampleTargetProfile]
      [Triggers:RAI-ExampleTriggerA]
      [Triggers:RAI-ExampleTriggerB]

      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

Below are the tags you are able to use in your Behavior Profiles. They are divided into several categories based on what they control.

**[Core](#Core)**  
**[Damage](#Damage)**  
**[Despawn](#Despawn)**  
**[Damage](#Damage)**   
**[EscortSystem](#EscortSystem)**  
**[Weapons](#Weapons)**  

**[CargoShip](#CargoShip)**  
**[Escort](#Escort)**  
**[Fighter](#Fighter)**  
**[HorseFighter](#HorseFighter)**  
**[Horsefly](#Horsefly)**  
**[Hunter](#Hunter)**  
**[Nautical](#Nautical)**  
**[Passive](#Passive)**  
**[Patrol](#Patrol)**  
**[Scout](#Scout)**  
**[Sniper](#Sniper)**  
**[Strike](#Strike)**  
 
 
# Core

These tags are the main building blocks of your behavior. You can specify the type of Behavior your encounter will use, along with other parameters/profiles such as Autopilot, Targeting, Triggers, etc.

<!--BehaviorName-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BehaviorName|
|:----|:----|
|Tag Format:|`[BehaviorName:Value]`|
|Description:|This tag specifies the type of preset behavior that you NPC should use. Depending on what type you choose, additional tags become available for use (see Behavior Specific Tags section for more details)|
|Allowed Values:|`CargoShip`<br />`Escort`<br />`Fighter`<br>`HorseFighter`<br>`Horsefly`<br>`Hunter`<br />`Nautical`<br />`Passive`<br>`Patrol`<br />`Strike`|
|Multiple Tag Allowed:|No|

<!--AutopilotData-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AutopilotData|
|:----|:----|
|Tag Format:|`[AutopilotData:Value]`|
|Description:|This tag specifies the Autopilot Profile that you want your behavior to use.|
|Allowed Values:|Any Autopilot Profile SubtypeId|
|Multiple Tag Allowed:|No|

<!--SecondaryAutopilotData-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SecondaryAutopilotData|
|:----|:----|
|Tag Format:|`[SecondaryAutopilotData:Value]`|
|Description:|This tag specifies a second Autopilot Profile that you want your behavior to use. Some behaviors may require this, otherwise it is optional.|
|Allowed Values:|Any Autopilot Profile SubtypeId|
|Multiple Tag Allowed:|No|

<!--TertiaryAutopilotData-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TertiaryAutopilotData|
|:----|:----|
|Tag Format:|`[TertiaryAutopilotData:Value]`|
|Description:|This tag specifies a third Autopilot Profile that you want your behavior to use. Some behaviors may require this, otherwise it is optional.|
|Allowed Values:|Any Autopilot Profile SubtypeId|
|Multiple Tag Allowed:|No|

<!--TargetData-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TargetData|
|:----|:----|
|Tag Format:|`[TargetData:Value]`|
|Description:|This tag specifies the Target Profile that you want your behavior to use when acquiring targets.|
|Allowed Values:|Any Target Profile SubtypeId|
|Multiple Tag Allowed:|No|

<!--OverrideTargetData-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|OverrideTargetData|
|:----|:----|
|Tag Format:|`[OverrideTargetData:Value]`|
|Description:|This tag specifies the Target Profile that you want your behavior to use when acquiring targets from Actions (eg Damage Attacker, Received From Command, Received From Turret, etc).|
|Allowed Values:|Any Target Profile SubtypeId|
|Multiple Tag Allowed:|No|

<!--Triggers-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Triggers|
|:----|:----|
|Tag Format:|`[Triggers:Value]`|
|Description:|This tag specifies Trigger Profiles that you want your behavior to use.|
|Allowed Values:|Any Trigger Profile SubtypeId|
|Multiple Tag Allowed:|Yes|

<!--TriggerGroups-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TriggerGroups|
|:----|:----|
|Tag Format:|`[TriggerGroups:Value]`|
|Description:|This tag specifies TriggerGroup Profiles that you want your behavior to use.|
|Allowed Values:|Any TriggerGroup Profile SubtypeId|
|Multiple Tag Allowed:|Yes|

<!--RemoteControlCode-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RemoteControlCode|
|:----|:----|
|Tag Format:|`[RemoteControlCode:Value]`|
|Description:|This tag specifies a code that is attached to the Remote Control and then registered with the Modular Encounters Spawner. This allows you to setup other SpawnGroups to spawn inside/outside a certain distance of this Remote Control while it is active.|
|Allowed Values:|Any String Value|
|Multiple Tag Allowed:|No|

<!--WeaponSystem-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|WeaponSystem|
|:----|:----|
|Tag Format:|`[WeaponSystem:Value]`|
|Description:|This tag specifies a Weapon System Profile that you want your behavior to use.|
|Allowed Values:|Any Weapon System Profile SubtypeId|
|Multiple Tag Allowed:|No|

# Damage

The Damage System in Rival AI is used to monitor when damage is taken and how to respond to it.

<!--UseDamageDetection-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseDamageDetection|
|:----|:----|
|Tag Format:|`[UseDamageDetection:Value]`|
|Description:|This tag specifies if an NPC grid should monitor for Damage. When damage is detected, it can be used to with the Trigger System to activate Action Profiles.<br /><br />This tag only need to be assigned to `true` if you plan to use the Accumulated Damage Conditions in a `Condition Profile` without having Trigger Profiles that watch for Damage events. If you include any Trigger Profile that watches Damage, this value will default to `true`|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|


# Despawn

The Despawn System in Modular Encounters Systems is used to control when an NPC ship should either Retreat and/or Despawn.

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UsePlayerDistanceTimer|
|:----|:----|
|Tag Format:|`[UsePlayerDistanceTimer:Value]`|
|Description:|This tag specifies if a despawn timer should be started if there are no players in the area near the NPC.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PlayerDistanceTimerTrigger|
|:----|:----|
|Tag Format:|`[PlayerDistanceTimerTrigger:Value]`|
|Description:|Specifies the time that a player must be outside of the `PlayerDistanceTrigger` distance from the NPC before the despawn occurs.|
|Allowed Values:|Any Number Higher Than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PlayerDistanceTrigger|
|:----|:----|
|Tag Format:|`[PlayerDistanceTrigger:Value]`|
|Description:|Specifies the max distance a player can be from the NPC before the Player Distance Timer starts.|
|Allowed Values:|Any Number Higher Than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseNoTargetTimer|
|:----|:----|
|Tag Format:|`[UseNoTargetTimer:Value]`|
|Description:|This tag specifies if a retreat timer should be started if there are no valid targets in the area near the NPC.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|NoTargetTimerTrigger|
|:----|:----|
|Tag Format:|`[NoTargetTimerTrigger:Value]`|
|Description:|Specifies the time that must pass without finding a valid target before the NPC retreats.|
|Allowed Values:|Any Number Higher Than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseRetreatTimer|
|:----|:----|
|Tag Format:|`[UseRetreatTimer:Value]`|
|Description:|This tag specifies if a retreat timer should be started immediately after the NPC spawns. Once the timer reaches 0, the NPC would abandon anything it was currently doing and retreat/despawn.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RetreatTimerTrigger|
|:----|:----|
|Tag Format:|`[RetreatTimerTrigger:Value]`|
|Description:|Specifies the time from spawn that it will take for the Behavior to trigger the Retreat function.|
|Allowed Values:|Any Number Higher Than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RetreatDespawnDistance|
|:----|:----|
|Tag Format:|`[RetreatDespawnDistance:Value]`|
|Description:|Specifies the max distance a player can be from the NPC after it begins retreating before the NPC depsawns.|
|Allowed Values:|Any Number Higher Than `0`|
|Multiple Tag Allowed:|No|


# EscortSystem

The Escort System in Modular Encounters Systems is used to assist other ships following an NPC in a formation.

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EscortOffsets|
|:----|:----|
|Tag Format:|`[EscortOffsets:Value]`|
|Description:|This tag specifies an offset location from the NPC Remote Control block that will be assigned to other NPCs requesting to escort this NPC.|
|Allowed Values:|Any Vector3D String<br />`eg {X:100 Y:0 Z:0}`|
|Multiple Tag Allowed:|Yes|


# CargoShip

**Behavior Description:** The Cargo Ship behavior is designed to be an alternative from using the default Cargo Ship movement types (no-dampener drifting in space or Keen autopilot in gravity). The behavior will use RivalAI Autopilot to drive the ship to the Despawn Coordinates that are generated by the Modular Encounters Spawner. If no Despawn Coordinates were generated by the Spawner, then it will generate its own.

In addition to this, the behavior is also able to travel to other waypoints before traveling to its Despawn coordinates. Using Command Profiles with the Trigger / Action system can allow this behavior to receive waypoints generated by other encounters, allowing for events where a ship may fly close to another encounter or station (simulating supply drop off, etc).

It is recommended to use this behavior with an Autopilot Profile that uses the `FlyLevelWithGravity` tag.
**Core System Tag Defaults:**

|System:|Tag:|Value:|
|:----|:----|:----|
|AutoPilot|Default Primary Profile|[Click Here](https://github.com/MeridiusIX/RivalAI/blob/master/Data/GenericProfiles/Autopilot/CargoShip.sbc)|
|Despawn|UseNoTargetTimer|`false`|

***

The CargoShip behavior type also has some custom Trigger Types as well. Below they are described:

|Type:|Description:|
|:----|:----|
|`BehaviorTriggerA`|This trigger is activated when the CargoShip arrives at a non-despawn waypoint.|
|`BehaviorTriggerB`|This trigger is activated when the CargoShip leaves a waypoint.|
|`BehaviorTriggerC`|This trigger is activated when the CargoShip switches from a despawn waypoint to a non-despawn waypoint.|
|`BehaviorTriggerD`|This trigger is activated when the CargoShip switches from a non-despawn waypoint to a despawn waypoint.|


# Escort

**Behavior Description:** This behavior is used in conjunction with the Trigger/Action/Command system to allow a ship to request escorting another ship (eg: small drone escorting a cargo ship). Once the behavior sends the request, nearby potential 'parent' ships will receive the request. If the parent ship has a trigger configured to receive the command and process the escort request, it will attempt to designate an available escort offset to the requestor (offsets need to be defined in the parent behavior). If an escort offset is successfully designated, then the requestor will travel to the offset and continue to travel to it as long as the ship exists. If an escort cannot acquire a parent grid to follow, then it will trigger a despawn based on time set in `WaypointWaitTimeTrigger` of the current Autopilot Profile (this only happens while `UseNoTargetTimer` is true).

***

**Core System Tag Defaults:**

|System:|Tag:|Value:|
|:----|:----|:----|
|AutoPilot|Default Primary Profile|[Click Here](https://github.com/MeridiusIX/RivalAI/blob/master/Data/GenericProfiles/Autopilot/Escort.sbc)|
|Despawn|UseNoTargetTimer|`true`|
|Target Profile|TargetType|`Player`|
|Target Profile|TargetRelation |`Enemy`|
|Target Profile|TargetOwner|`Player`|
|Weapons|UseStaticGuns|`true`|


# Fighter

**Behavior Description:** The fighter behavior is designed primarily for small grid drones or fighter craft. It utilizes Forward Facing Static Weapons to engage targets. The behavior will approach the target, and then once it reaches a specified distance, it will switch to another mode where it will rotate towards the target to engage with weapons, while occasionally strafing.

***

**Core System Tag Defaults:**

|System:|Tag:|Value:|
|:----|:----|:----|
|AutoPilot|Default Primary Profile|[Click Here](https://github.com/MeridiusIX/RivalAI/blob/master/Data/GenericProfiles/Autopilot/Fighter.sbc)|
|Despawn|UseNoTargetTimer|`true`|
|Target Profile|TargetType|`Player`|
|Target Profile|TargetRelation |`Enemy`|
|Target Profile|TargetOwner|`Player`|
|Weapons|UseStaticGuns|`true`|

***

The Fighter behavior type also has some custom Trigger Types as well. Below they are described:

|Type:|Description:|
|:----|:----|
|`BehaviorTriggerA`|This trigger is activated when the Fighter is within Strafe / Rotate to Target distance (ie Engage).|
|`BehaviorTriggerB`|This trigger is activated when the Fighter is outside Strafe / Rotate to Target distance (ie Approach).|


# Horsefly

**Behavior Description:** The Horsefly behavior is designed for ships of any size that use Turrets as their primary means of attack. The behavior flies to the area of a target. Once within range, the NPC will fly to random positions around the target while the turrets engage. If the NPC cannot reach one of the waypoints after a certain time, it will recalculate a new waypoint. This behavior does not use Static Weapons.

***

**Core System Tag Defaults:**

|System:|Tag:|Value:|
|:----|:----|:----|
|AutoPilot|Default Primary Profile|[Click Here](https://github.com/MeridiusIX/RivalAI/blob/master/Data/GenericProfiles/Autopilot/Horsefly.sbc)|


# HorseFighter

**Behavior Description:** One day, the fighter behavior went out for a few drinks. It ended up going home with the Horsefly behavior for some consensual... well, you can use your imagination (or don't, that's probably the sane thing to do).

HorseFighter is a combination between the Horsefly and Fighter behaviors. Its main behavior mostly resembles the Horsefly movements where it will fly around a target in randomized patters and engage targets with Turrets. 

Internally, there is a timer that toggles a switch between Fighter and Horsefly. If the switch is activated and the NPC grid is within range of the target, the behavior will switch to the Rotate and Strafe behavior of the Fighter, engaging with Static Weapons. 

After the timer triggers the switch to turn off again, the behavior will resume Horsefly-like flight patterns until the timer once again activates the switch.

***

**Core System Tag Defaults:**

|System:|Tag:|Value:|
|:----|:----|:----|
|AutoPilot|Default Primary Profile|[Click Here](https://github.com/MeridiusIX/RivalAI/blob/master/Data/GenericProfiles/Autopilot/HorseFighter.sbc)|

***

Below are the custom tags you can use for the Horsefly Behavior:

<!--HorseFighterTimeApproaching-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|HorseFighterTimeApproaching|
|:----|:----|
|Tag Format:|`[HorseFighterTimeApproaching:Value]`|
|Description:|This tag specifies the amount of time before the NPC will spend in Horsefly mode before switching to Fighter mode (if in range of the target based on the EngageDistance tags).|
|Allowed Values:|Any integer `0` or greater<br />|
|Multiple Tag Allowed:|No|

<!--HorseFighterTimeEngaging-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|HorseFighterTimeEngaging|
|:----|:----|
|Tag Format:|`[HorseFighterTimeEngaging:Value]`|
|Description:|This tag specifies the amount of time before the NPC will spend in Fighter mode before switching to Horsefly mode.|
|Allowed Values:|Any integer `0` or greater<br />|
|Multiple Tag Allowed:|No|


# Hunter

**Behavior Description:** The hunter behavior is an advanced combat behavior that requires careful setup of its Targeting and Autopilot Profiles to function properly. When the behavior does not have a valid target, it will approach its despawn coordinates using the **Primary Autopilot Profile**. 

If the behavior acquires a target, it will begin to approach the target using the **Secondary Autopilot Profile** if it is still valid when it does it's own timed check. While approaching the target, it requires one of 3 methods to confirm the target as valid. To confirm a valid target, it can use Turret Target Detection, Camera Raycast, or Line of Sight (if the target profile includes that filter). 

If a target is validated, then the behavior will engage it with `Fighter` like maneuvering. If a target is lost and no other target can be found, the behavior will still travel to the target's last known location for a short time before resuming its course to despawn coordinates. 

If another target is later acquired, the process above is repeated.


***

**Core System Tag Defaults:**

|System:|Tag:|Value:|
|:----|:----|:----|
|AutoPilot|Default Profiles|[Click Here](https://github.com/MeridiusIX/RivalAI/blob/master/Data/GenericProfiles/Autopilot/Hunter.sbc)|
|Despawn|UseNoTargetTimer|`true`|
|Target Profile|TargetType|`Player`|
|Target Profile|TargetRelation |`Enemy`|
|Target Profile|TargetOwner|`Player`|
|Weapons|UseStaticGuns|`true`|

***

The Hunter behavior type also has some custom Trigger Types as well. Below they are described:

|Type:|Description:|
|:----|:----|
|`BehaviorTriggerA`|This trigger is activated when the Hunter detects a new target while approaching Despawn.|
|`BehaviorTriggerB`|This trigger is activated when the Hunter loses its current target, but is still approaching the targets last known coordinates.|
|`BehaviorTriggerC`|This trigger is activated when the Hunter loses its current target and resumes travel to Despawn.|
|`BehaviorTriggerD`|This trigger is activated when the Hunter confirms a target and begins to Engage.|
|`BehaviorTriggerE`|This trigger is activated when the Hunter enters Strafe/Rotate range of a target.|
|`BehaviorTriggerF`|This trigger is activated when the Hunter leaves Strafe/Rotate range of a target.|

***

Below are the custom tags you can use for the Hunter Behavior:

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TimeBetweenNewTargetChecks|
|:----|:----|
|Tag Format:|`[TimeBetweenNewTargetChecks:Value]`|
|Description:|This tag specifies the time it takes to check if the behavior has a valid target to begin approaching.|
|Allowed Values:|Any number greater than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|LostTargetTimerTrigger|
|:----|:----|
|Tag Format:|`[LostTargetTimerTrigger:Value]`|
|Description:|This tag specifies the time the behavior will spend approaching the last known coordinates of a lost target before resuming travel to its despawn coordinates.|
|Allowed Values:|Any number greater than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EngageOnCameraDetection|
|:----|:----|
|Tag Format:|`[EngageOnCameraDetection:Value]`|
|Description:|This tag specifies if the behavior should use Camera Blocks to confirm potential targets that it is approaching. Please be mindful of how many cameras your grid uses, since these checks cannot be done in a parallel thread - meaning they can become a performance concern if spammed.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EngageOnWeaponActivation|
|:----|:----|
|Tag Format:|`[EngageOnWeaponActivation:Value]`|
|Description:|This tag specifies if the behavior should use Turret Targeting to confirm if an approached target is valid.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EngageOnTargetLineOfSight|
|:----|:----|
|Tag Format:|`[EngageOnTargetLineOfSight:Value]`|
|Description:|This tag specifies if the behavior should use the `LineOfSight` filter in the Target Profile to confirm if the approached target is valid.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CameraDetectionMaxRange|
|:----|:----|
|Tag Format:|`[CameraDetectionMaxRange:Value]`|
|Description:|This tag specifies the max distance that Camera Blocks will attempt to raycast a target if using the `EngageOnCameraDetection` tag.|
|Allowed Values:|Any number greater than `0`|
|Multiple Tag Allowed:|No|


# Nautical

**Behavior Description:** The Nautical behavior is a specialized behavior that is designed to work with [Jakaria's Water Mod](https://steamcommunity.com/sharedfiles/filedetails/?id=2200451495). This behavior allows NPCs to navigate to a target while floating in water. It uses pathfinding to navigate around terrain obstacles, allowing it reach targets that may be hiding among complex terrain and water. NPCs using this behavior must be spawned in water, otherwise they will not be able to move.

***

**Core System Tag Defaults:**

|System:|Tag:|Value:|
|:----|:----|:----|
|AutoPilot|Default Primary Profile|[Click Here](https://github.com/MeridiusIX/RivalAI/blob/master/Data/GenericProfiles/Autopilot/Nautical.sbc)|

***

The Nautical behavior type also has some custom Trigger Types as well. Below they are described:

|Type:|Description:|
|:----|:----|
|`BehaviorTriggerA`|This trigger is activated when the Nautical is within Strafe / Rotate to Target distance (ie Engage).|
|`BehaviorTriggerB`|This trigger is activated when the Nautical is outside Strafe / Rotate to Target distance (ie Approach).|

***

**Behavior Tags:**

See the AutoPilot Profile guide for compatible tags with this behavior.


# Passive

**Behavior Description:** The passive behavior is designed to be used with Cargo Ship and Station encounters. It does not have any special features on its own, but instead relies on provided configuration in the rest of the behavior settings (Triggers, Actions, etc).

***

**Core System Tag Defaults:**

|System:|Tag:|Value:|
|:----|:----|:----|
|Despawn|UsePlayerDistanceTimer|`false`|
***

The Passive Behavior Type has not behavior exclusive tags at this time.


# Patrol

**Behavior Description:** This behavior allows the ship using it to patrol a random area from where the ship first spawned. The distances for this are governed by the same Offset tags that Horsefly and other behaviors use.


# Scout

# Sniper

# Strike

**Behavior Description:** The Strike behavior is designed primarily for small grid drones or fighter craft. It utilizes Forward Facing Static Weapons to engage targets. The behavior will fly in the general direction of a target, and once it gets within a certain range it will charge directly at the target and engage with static weapons. It will then break from the charge, retreating a short distance, and then attempt another charge. If the target is on a planet near the surface, the charge maneuver is reminiscent of a dive-bomb maneuver. This behavior does not use Keen Vanilla AutoPilot at all.

***

**Core System Tag Defaults:**

|System:|Tag:|Value:|
|:----|:----|:----|
|AutoPilot|Default Primary Profile|[Click Here](https://github.com/MeridiusIX/RivalAI/blob/master/Data/GenericProfiles/Autopilot/Strike.sbc)|

***

Below are the custom tags you can use for the Strike Behavior:

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StrikeBeginSpaceAttackRunDistance|
|:----|:----|
|Tag Format:|`[StrikeBeginSpaceAttackRunDistance:Value]`|
|Description:|This tag specifies the distance (while in space) from the offset coordinates the NPC initially travels to near the target before it switches direction and begins to attack the player.|
|Allowed Values:|Any number greater than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StrikeBeginPlanetAttackRunDistance|
|:----|:----|
|Tag Format:|`[StrikeBeginPlanetAttackRunDistance:Value]`|
|Description:|This tag specifies the distance (while in natural gravity) from the offset coordinates the NPC initially travels to near the target before it switches direction and begins to attack the player.|
|Allowed Values:|Any number greater than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StrikeBreakawayDistance|
|:----|:----|
|Tag Format:|`[StrikeBreakawayDistance:Value]`|
|Description:|This tag specifies the distance from a target before the NPC will attempt its breakaway from charging the target.|
|Allowed Values:|Any number greater than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StrikeOffsetRecalculationTime|
|:----|:----|
|Tag Format:|`[StrikeOffsetRecalculationTime:Value]`|
|Description:|This tag specifies the maximum time (in seconds) that the NPC will spend trying to reach an offset waypoint before recalculating another one (this is useful if the NPC cannot reach a waypoint for some reason).|
|Allowed Values:|Any number greater than `0`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StrikeEngageUseSafePlanetPathing|
|:----|:----|
|Tag Format:|`[StrikeEngageUseSafePlanetPathing:Value]`|
|Description:|This tag specifies if the NPC should still use safe planetary pathing while engaging targets on a planet. Default value is `true`. Disabling this may cause an NPC to crash into terrain when engaging a target in an attack run. |
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StrikeEngageUseCollisionEvasion|
|:----|:----|
|Tag Format:|`[StrikeEngageUseCollisionEvasion:Value]`|
|Description:|This tag specifies if the NPC should still use collision evasion while engaging targets. Default value is `true`. Disabling this may cause an NPC to crash into objects when engaging a target in an attack run. |
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|