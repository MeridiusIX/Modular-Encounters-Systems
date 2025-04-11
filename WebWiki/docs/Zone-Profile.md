#Zone-Profile.md

Zones (formerly Territories) are pre-defined locations in the game world that can be used to control what encounters are allowed to spawn and when. Zone Profiles do not attach to SpawnGroups or any other profile.

Here is an example of how a Zone is defined:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>MES-Zone-PirateZone</SubtypeId>
      </Id>
      <Description>

        [MES Zone]

        [PublicName:Pirate Space]

        [Active:true]
        [Persistent:true]
        [Strict:true]

        [Coordinates:{X:1 Y:1 Z:1}]
        [Radius:25000]

      </Description>

    </EntityComponent>
    
  </EntityComponents>
</Definitions>
```

Below are the types of tags you can include in your Zone Profile:

<!--Active-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Active|
|:----|:----|
|Tag Format:|`[Active:Value]`|
|Description:|This tag specifies if the Zone should be active in the game world or not.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!--Persistent-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Persistent|
|:----|:----|
|Tag Format:|`[Persistent:Value]`|
|Description:|This tag determines if a Zone should persist in the game world between saves, reloads, etc.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!--Strict-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Strict|
|:----|:----|
|Tag Format:|`[Strict:Value]`|
|Description:|This tag determines if encounters must be able to spawn in this zone in order to spawn at all.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!--NoSpawnZone-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|NoSpawnZone|
|:----|:----|
|Tag Format:|`[NoSpawnZone:Value]`|
|Description:|This tag determines if the Zone should be considered a No Spawn Zone, preventing any type of encounter from spawning.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!--PublicName-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PublicName|
|:----|:----|
|Tag Format:|`[PublicName:Value]`|
|Description:|This tag determines the name the Zone will use with the Spawner. You'll use this to link Zones to your Zone Conditions Profiles.|
|Allowed Values:|Any Name|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|No|

<!--UseLimitedFactions-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseLimitedFactions|
|:----|:----|
|Tag Format:|`[UseLimitedFactions:Value]`|
|Description:|This tag determines if the Zone should only allow encounters from certain factions to spawn within it.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!--Factions-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Factions|
|:----|:----|
|Tag Format:|`[Factions:Value]`|
|Description:|This tag determines the Factions that are allowed to spawn within the Zone if using the `UseLimitedFactions` tag.|
|Allowed Values:|Any Faction Tag|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!--Coordinates-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Coordinates|
|:----|:----|
|Tag Format:|`[Coordinates:Value]`|
|Description:|This tag determines the exact location of the center of this Zone.|
|Allowed Values:|Vector3D Coordinates<br />eg: `{X:0 Y:0 Z:0}`|
|Default Value(s):|`{X:0 Y:0 Z:0}`|
|Multiple Tag Allowed:|No|

<!--Radius-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Radius|
|:----|:----|
|Tag Format:|`[Radius:Value]`|
|Description:|This tag determines the radius distance from the Zone center coordinates.|
|Allowed Values:|Any Number Greater Than `0`|
|Default Value(s):|`0`|
|Multiple Tag Allowed:|No|

<!--PlanetaryZone-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PlanetaryZone|
|:----|:----|
|Tag Format:|`[PlanetaryZone:Value]`|
|Description:|This tag determines if a zone should be dynamically calculated based on provided planetary details.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!--PlanetName-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PlanetName|
|:----|:----|
|Tag Format:|`[PlanetName:Value]`|
|Description:|This tag determines the name of the planet that the Zone will be created on.|
|Allowed Values:|Any Planet Name|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|No|

<!--Direction-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Direction|
|:----|:----|
|Tag Format:|`[Direction:Value]`|
|Description:|This tag determines the direction from the center of the planet that the Zone is created at. If a direction is provided, then the Zone center will be at the surface location nearest to the direction provided. If no direction is provided, then the center of the planet is instead used for the Zone center.|
|Allowed Values:|Vector3D Coordinates<br />eg: `{X:0 Y:0 Z:0}`|
|Default Value(s):|`{X:0 Y:0 Z:0}`|
|Multiple Tag Allowed:|No|

<!--HeightOffset-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|HeightOffset|
|:----|:----|
|Tag Format:|`[HeightOffset:Value]`|
|Description:|This tag determines the height offset from the planet surface if a `Direction` tag is provided.|
|Allowed Values:|Any Number|
|Default Value(s):|`0`|
|Multiple Tag Allowed:|No|

<!--ScaleZoneRadiusWithPlanet NOT YET-->


<!--IntendedPlanetSize NOT YET-->

<!--UseAllowedSpawnGroups-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseAllowedSpawnGroups|
|:----|:----|
|Tag Format:|`[UseAllowedSpawnGroups:Value]`|
|Description:|This tag determines if only a list of specific SpawnGroups should be allowed to spawn in this Zone.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!--AllowedSpawnGroups-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AllowedSpawnGroups|
|:----|:----|
|Tag Format:|`[AllowedSpawnGroups:Value]`|
|Description:|This tag determines which SpawnGroups are allowed to spawn inside this Zone.|
|Allowed Values:|Any SpawnGroup SubtypeID|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!--UseRestrictedSpawnGroups-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseRestrictedSpawnGroups|
|:----|:----|
|Tag Format:|`[UseRestrictedSpawnGroups:Value]`|
|Description:|This tag determines if certain SpawnGroups should not be allowed to spawn while in this Zone.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!--RestrictedSpawnGroups-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RestrictedSpawnGroups|
|:----|:----|
|Tag Format:|`[RestrictedSpawnGroups:Value]`|
|Description:|This tag determines which SpawnGroups are not allowed to spawn inside this Zone.|
|Allowed Values:|Any SpawnGroup SubtypeID|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!--UseAllowedModIDs-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseAllowedModIDs|
|:----|:----|
|Tag Format:|`[UseAllowedModIDs:Value]`|
|Description:|This tag determines if only SpawnGroups belonging to certain Mod IDs are allowed to spawn in this Zone.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!--AllowedModIDs-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AllowedModIDs|
|:----|:----|
|Tag Format:|`[AllowedModIDs:Value]`|
|Description:|This tag determines which SpawnGroup Mod IDs are allowed to spawn inside this Zone.|
|Allowed Values:|Any SpawnGroup SubtypeID|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!--UseRestrictedModIDs-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseRestrictedModIDs|
|:----|:----|
|Tag Format:|`[UseRestrictedModIDs:Value]`|
|Description:|This tag determines if SpawnGroups belonging to certain Mod IDs should not be allowed to spawn in this Zone.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!--RestrictedModIDs-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RestrictedModIDs|
|:----|:----|
|Tag Format:|`[RestrictedModIDs:Value]`|
|Description:|This tag determines which SpawnGroup Mod IDs are not allowed to spawn inside this Zone.|
|Allowed Values:|Any SpawnGroup SubtypeID|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|Yes|

<!--UseZoneAnnounce-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseZoneAnnounce|
|:----|:----|
|Tag Format:|`[UseZoneAnnounce:Value]`|
|Description:|This tag determines if players should receive an alert if they enter or leave this Zone.|
|Allowed Values:|`true`<br>`false`|
|Default Value(s):|`false`|
|Multiple Tag Allowed:|No|

<!--ZoneEnterAnnounce-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneEnterAnnounce|
|:----|:----|
|Tag Format:|`[ZoneEnterAnnounce:Value]`|
|Description:|This tag determines the message players will receive when they enter this Zone.|
|Allowed Values:|Any Message|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|No|

<!--ZoneLeaveAnnounce-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ZoneLeaveAnnounce|
|:----|:----|
|Tag Format:|`[ZoneLeaveAnnounce:Value]`|
|Description:|This tag determines the message players will receive when they leave this Zone.|
|Allowed Values:|Any Message|
|Default Value(s):|`N/A`|
|Multiple Tag Allowed:|No|

<!-- RequiredSandboxBool -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RequiredSandboxBool|
|:----|:----|
|Tag Format:|`[RequiredSandboxBool:Value]`|
|Description:|This tag allows you to specify the name of a Sandbox Boolean that must be `true` in order for this zone to be Active.|
|Allowed Value(s):|Any Sandbox Boolean Name|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|No|

<!-- RequiredFalseSandboxBool -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RequiredFalseSandboxBool|
|:----|:----|
|Tag Format:|`[RequiredFalseSandboxBool:Value]`|
|Description:|This tag allows you to specify the name of a Sandbox Boolean that must be `false` in order for this zone to be Active.|
|Allowed Value(s):|Any Sandbox Boolean Name|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|No|

<!--FlashZoneRadius NOT YET-->


<!--XXX-->


<!--XXX-->


<!--XXX-->


<!--XXX-->


<!--XXX-->


<!--XXX-->


<!--XXX-->


<!--XXX-->


<!--XXX-->


<!--XXX-->


<!--XXX-->


<!--XXX-->


<!--XXX-->


<!--XXX-->


<!--XXX-->


<!--XXX-->

