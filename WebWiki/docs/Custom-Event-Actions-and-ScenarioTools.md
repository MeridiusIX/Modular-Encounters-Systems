#Custom-Event-Actions-and-ScenarioTools.md

# EventActions continuation

### Tags 
<!--ActivateCustomAction  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ActivateCustomAction|
|:----|:----|
|Tag Format:|`[ActivateCustomAction:Value]`|
|Description:|nan|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|no|
<!--CustomActionName  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomActionName|
|:----|:----|
|Tag Format:|`[CustomActionName:Value]`|
|Description:|nan|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|no|
<!--CustomActionArgumentsString  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomActionArgumentsString|
|:----|:----|
|Tag Format:|`[CustomActionArgumentsString:Value]`|
|Description:|nan|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|yes|
<!--CustomActionArgumentsBool  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomActionArgumentsBool|
|:----|:----|
|Tag Format:|`[CustomActionArgumentsBool:Value]`|
|Description:|nan|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|yes|
<!--CustomActionArgumentsInt  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomActionArgumentsInt|
|:----|:----|
|Tag Format:|`[CustomActionArgumentsInt:Value]`|
|Description:|nan|
|Allowed Values:|Any interger equal or greater than 0|
|Multiple Tag Allowed:|yes|
<!--CustomActionArgumentsFloat  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomActionArgumentsFloat|
|:----|:----|
|Tag Format:|`[CustomActionArgumentsFloat:Value]`|
|Description:|nan|
|Allowed Values:|Any float equal or greater than 0|
|Multiple Tag Allowed:|yes|
<!--CustomActionArgumentsLong  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomActionArgumentsLong|
|:----|:----|
|Tag Format:|`[CustomActionArgumentsLong:Value]`|
|Description:|nan|
|Allowed Values:|Any long equal or greater than 0|
|Multiple Tag Allowed:|yes|
<!--CustomActionArgumentsDouble  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomActionArgumentsDouble|
|:----|:----|
|Tag Format:|`[CustomActionArgumentsDouble:Value]`|
|Description:|nan|
|Allowed Values:|Any double equal or greater than 0|
|Multiple Tag Allowed:|yes|
<!--CustomActionArgumentsVector3D  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomActionArgumentsVector3D|
|:----|:----|
|Tag Format:|`[CustomActionArgumentsVector3D:Value]`|
|Description:|nan|
|Allowed Values:|Any Vector3D equal or greater than 0|
|Multiple Tag Allowed:|yes|


# Scenario Tools

[Mod link](https://steamcommunity.com/sharedfiles/filedetails/?id=2998575759)

Scenario Tools is an extension of MES. 



**Scenario Tools Custom Actions:**


**Overview:**

**[ScT-CreateGPS](#ScT-CreateGPS)**  
**[ScT-RemoveGPS](#ScT-RemoveGPS)**  
**[ScT-AddNews](#ScT-AddNews)**  
**[ScT-SpawnPlanetaryInstallation](#ScT-SpawnPlanetaryInstallation)**  
**[ScT-SpawnPlanetaryBlockade](#ScT-SpawnPlanetaryBlockade)**    

## ScT-CreateGPS

This custom action creates a new GPS marker with specified parameters.

Parameters:

`string` name: The name of the GPS marker.

`string `desc: The description of the GPS marker.

`int` time: The time (in minutes) the GPS marker will be active.

`Vector3D`coord: The coordinates of the GPS marker.


```
<EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
  <Id>
	  <TypeId>Inventory</TypeId>
	  <SubtypeId>MOD-EventAction-Test</SubtypeId>
  </Id>
  <Description>
	[MES Event Action]
	[ActivateCustomAction:true]
	[CustomActionName:ScT-CreateGPS]
	[CustomActionArgumentsString:Story Event]
	[CustomActionArgumentsString:Oh the humanity!]
	[CustomActionArgumentsInt:120]		
	[CustomActionArgumentsVector3D:{X:-1725718.78 Y:1493440.69 Z:-698321.45}]
  </Description>
</EntityComponent>
```
![](https://steamuserimages-a.akamaihd.net/ugc/2029485208071521476/F9E1FFFAD880D21B73D67CE0583E367BD9A3AB47/)

## ScT-RemoveGPS

Parameters:

`string` name:Name of the GPS, so that was created by Create GPS

The name of the GPS.
Usage:
```
<EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
  <Id>
	  <TypeId>Inventory</TypeId>
	  <SubtypeId>MOD-EventAction-Test</SubtypeId>
  </Id>
  <Description>
	[MES Event Action]
	[ActivateCustomAction:true]
	[CustomActionName:ScT-RemoveGPS]
	[CustomActionArgumentsString:Story Event]
  </Description>
</EntityComponent>
```


## ScT-AddNews

This custom action adds a news item to a LCD script.

Parameters:

`string` text: The text of the news item.

```
<EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
  <Id>
	  <TypeId>Inventory</TypeId>
	  <SubtypeId>MOD-EventAction-Test</SubtypeId>
  </Id>
  <Description>
	[MES Event Action]
	[ActivateCustomAction:true]
	[CustomActionName:ScT-AddNews]
	[CustomActionArgumentsString:FAF Captured Carcosa]
  </Description>
</EntityComponent>
```

![](https://steamuserimages-a.akamaihd.net/ugc/2029485208071488065/6E482174D5D82826F80A853D68F3BB5F262B723A/)

## ScT-SpawnPlanetaryInstallation (DON"T USE)

Description: This custom action spawns a planetary installation at a specified location.

Parameters:

`string` spawnGroup: The group name for the planetary installation.

`Vector3D` coord: The coordinates of the area where the installation should be spawned .

```
<EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
  <Id>
	  <TypeId>Inventory</TypeId>
	  <SubtypeId>MOD-EventAction-Test</SubtypeId>
  </Id>
  <Description>
	[MES Event Action]
	[ActivateCustomAction:true]
	[CustomActionName:ScT-SpawnPlanetaryInstallation]
	[CustomActionArgumentsString:FAFCarcosa]	
	[CustomActionArgumentsVector3D:{X:-1169412.7 Y:97934.39 Z:1325510.96}]	
  </Description>
</EntityComponent>

```




### ScT-SpawnPlanetaryBlockade (DON"T USE)

Description: This custom action spawns a planetary blockade near players within a specified radius.

This needs to be connected to a Event Condition that uses playernear

Parameters:

`string` spawnGroup: The group name for the spawned blockade.

`int` MinRadius: The minimum radius from players for spawning.

`int` MaxRadius: The maximum radius from players for spawning.

`int` SpawnDistance: The distance from players where the blockade should be spawned.

`Vector3D` PlanetCentercoord: The coordinates of the center of the planet.

```
	<EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>Mod-EventAction-Test</SubtypeId>
      </Id>
      <Description>
	[MES Event Action]
	[ActivateCustomAction:true]
	[CustomActionName:ScT-SpawnPlanetaryBlockade]
	[CustomActionArgumentsString:GC-SpawnGroup-HeavyGarrison]
	[CustomActionArgumentsInt:70000]	
	[CustomActionArgumentsInt:78000]
	[CustomActionArgumentsInt:4500]
	[CustomActionArgumentsVector3D:{X:1449429.5 Y:-622819.5 Z:-2854387.5}]
      </Description>
    </EntityComponent>	
```

