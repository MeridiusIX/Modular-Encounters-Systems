#Zone-Conditions-Profile.md

Zone Condition Profiles are additional controls that are attached to Spawn Condition Profiles that allow you to define spawning rules for when an encounter is inside of a Zone (formerly Territory).

Spawn Condition Profiles can have multiple Zone Condition Profiles. When evaluating the profiles for spawning eligibility, only a single Zone Condition Profile needs to be satisfied in order for the SpawnGroup to be considered eligible for spawning.

Here is an example of how a Zone Condition Profile is created:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>MES-ZoneConditionProfile-Example</SubtypeId>
      </Id>
      <Description>

        [MES Zone Conditions]

        [ZoneName:Pirate Space]
        [MaxDistanceFromZoneCenter:5000]
        [MinSpawnedZoneEncounters:5]

      </Description>

    </EntityComponent>
    
  </EntityComponents>
</Definitions>
```

To link a profile to your Spawn Conditions Profile, simply use the `ZoneConditions` tag and provide the SubtypeId of the Zone Condition Profile you created. Eg: `[ZoneConditions:MES-ZoneConditionProfile-Example]`. 

Below are the types of condition tags you can include in your Zone Condition Profile:

<!-- ZoneName-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneName|
|:----|:----|
|Tag Format:|`[ZoneName:Value]`|
|Description:|This tag specifies the name of the Zones that are allowed to Spawn this encounter.|
|Allowed Values:|Any Zone Profile SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|No|

<!-- MinDistanceFromZoneCenter-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinDistanceFromZoneCenter|
|:----|:----|
|Tag Format:|`[MinDistanceFromZoneCenter:Value]`|
|Description:|This tag specifies the Minimum Distance from the Zone Center that the encounter is able to spawn within. If this tag is not provided, then no minimum is used.|
|Allowed Values:|Any Number Greater Than `0`<br />Value should be lower than `MaxDistanceFromZoneCenter` if provided|
|Default Value(s):|`-1`|
|Multiple Tag Allowed:|No|

<!-- MaxDistanceFromZoneCenter-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxDistanceFromZoneCenter|
|:----|:----|
|Tag Format:|`[MaxDistanceFromZoneCenter:Value]`|
|Description:|This tag specifies the Maximum Distance from the Zone Center that the encounter is able to spawn within. If this tag is not provided, then no maximum is used.|
|Allowed Values:|Any Number Greater Than `0`<br />Value should be higher than `MinDistanceFromZoneCenter` if provided|
|Default Value(s):|`-1`|
|Multiple Tag Allowed:|No|

<!-- MinSpawnedZoneEncounters-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinSpawnedZoneEncounters|
|:----|:----|
|Tag Format:|`[MinSpawnedZoneEncounters:Value]`|
|Description:|This tag specifies the Minimum Spawned Encounters within the Zone that must have been spawned before the encounter is allowed to spawn. If this tag is not provided, then no minimum is used.|
|Allowed Values:|Any Number Greater Than `0`<br />Value should be lower than `MaxSpawnedZoneEncounters` if provided|
|Default Value(s):|`-1`|
|Multiple Tag Allowed:|No|

<!-- MaxSpawnedZoneEncounters-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxSpawnedZoneEncounters|
|:----|:----|
|Tag Format:|`[MaxSpawnedZoneEncounters:Value]`|
|Description:|This tag specifies the Maximum Spawned Encounters within the Zone that must have been spawned before the encounter is allowed to spawn. If this tag is not provided, then no maximum is used.|
|Allowed Values:|Any Number Greater Than `0`<br />Value should be higher than `MinSpawnedZoneEncounters` if provided|
|Default Value(s):|`-1`|
|Multiple Tag Allowed:|No|

<!-- CheckCustomZoneCounters-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckCustomZoneCounters|
|:----|:----|
|Tag Format:|`[CheckCustomZoneCounters:Value]`|
|Description:|This tag specifies if Zone Counters should be checked for specific values before the encounter spawns.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- CustomZoneCounterName-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomZoneCounterName|
|:----|:----|
|Tag Format:|`[CustomZoneCounterName:Value]`|
|Description:|This tag specifies the name of a Zone Counter that should be checked. It should be paired with a `CustomZoneCounterValue`|
|Allowed Values:|Any Counter Name|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!-- CustomZoneCounterValue-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomZoneCounterValue|
|:----|:----|
|Tag Format:|`[CustomZoneCounterValue:Value]`|
|Description:|This tag specifies the value of a Zone Counter that should be checked. It should be paired with a `CustomZoneCounterName`|
|Allowed Values:|Any Integer|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!-- CheckCustomZoneBools-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckCustomZoneBools|
|:----|:----|
|Tag Format:|`[CheckCustomZoneBools:Value]`|
|Description:|This tag specifies if Zone Booleans should be checked for specific values before the encounter spawns.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!-- CustomZoneBoolName-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomZoneBoolName|
|:----|:----|
|Tag Format:|`[CustomZoneBoolName:Value]`|
|Description:|This tag specifies the name of a Zone Boolean that should be checked. It should be paired with a `CustomZoneBoolValue`|
|Allowed Values:|Any Boolean Name|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!-- CustomZoneBoolValue-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomZoneBoolValue|
|:----|:----|
|Tag Format:|`[CustomZoneBoolValue:Value]`|
|Description:|This tag specifies the value of a Zone Boolean that should be checked. It should be paired with a `CustomZoneBoolName`|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|
