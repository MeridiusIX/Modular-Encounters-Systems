#Prefab-Data.md

Prefab Data profiles allow you to setup rules that are applied directly to specified prefabs, regardless of what SpawnGroups they're attached to. The benefit of this is that you can setup consistent modifications to prefabs that you may use in multiple SpawnGroups. Because these profiles are not attached to specific SpawnGroups, you do not need to attach them to a SpawnGroup, Spawn Condition, etc.

Below is an example of what a Prefab Data Profile looks like:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>MES-PrefabData-Example</SubtypeId>
      </Id>
      <Description>

      [MES Prefab Data]
      
      [Prefabs:ExampleDroneA]
      [Prefabs:ExampleDroneB]
      [ManipulationProfiles:ExampleDroneManipulationProfile]     
      
      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

Below you can find all the tags that can be used in your Prefab Data Profile:


<!-- Prefabs -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Prefabs|
|:----|:----|
|Tag Format:|`[Prefabs:Value]`|
|Description:|This tag allows you to specify one or more Prefab SubtypeIds that will be processed by the rules in this profile during spawning.|
|Allowed Value(s):|Any Prefab SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|Yes|

<!-- CustomTags -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomTags|
|:----|:----|
|Tag Format:|`[CustomTags:Value]`|
|Description:|This tag allows you to specify one or more Custom Tags that will be added to the prefab. These are string values you can apply to your prefabs so other manipulation profiles can be allowed / restricted. Example: Add a `Miner` tag to a prefab and a manipulation profile that adds Ore related inventory can check for that tag and add it if found.|
|Allowed Value(s):|Any String Value|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|Yes|

<!-- ManipulationProfiles -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManipulationProfiles|
|:----|:----|
|Tag Format:|`[ManipulationProfiles:Value]`|
|Description:|This tag allows you to specify one or more Manipulation Profiles that are applied to each prefab in this profile.|
|Allowed Value(s):|Any Manipulation Profile SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|Yes|

<!-- ManipulationGroups -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManipulationGroups|
|:----|:----|
|Tag Format:|`[ManipulationGroups:Value]`|
|Description:|This tag allows you to specify one or more Manipulation Profile Groups that are applied to each prefab in this profile.|
|Allowed Value(s):|Any Manipulation Profile Group SubtypeId|
|Default Value(s):|`N/A`|
|Multiple Tags Allowed:|Yes|

