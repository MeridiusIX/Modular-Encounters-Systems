#Dereliction.md

Dereliction Profiles allow you to apply block degradation to selected blocks to achieve a 'derelict' appearance on grids. Dereliction profiles are attached to `Manipulation` profiles.

Here is an example of how a Dereliction Profile is setup:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>MES-ExampleDereliction</SubtypeId>
      </Id>
      <Description>

      [MES Dereliction]

      [Blocks:MyObjectBuilder_CubeBlock:SomeBlock]
      [Blocks:MyObjectBuilder_Thruster:SomeThruster]
      [MatchOnlyTypeId:true]
      [Chance:50]
      
      [MinPercentage:5]
      [MaxPercentage:50]
      
      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

These profiles attach to Manipulation Profiles using the `[DerelictionProfiles:YourDerelictionProfileIdHere]` tag.

Below you can find all the tags that can be used in your Dereliction Profile:

<!-- Blocks -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Blocks|
|:----|:----|
|Tag Format:|`[Blocks:Value]`|
|Description:|This tag allows you to specify the `MyDefinitionId` of a block that will be used by the Dereliction process.|
|Allowed Value(s):|Any String Value|
|Default Value(s):|N/A|
|Multiple Tags Allowed:|Yes|

<!-- MatchOnlyTypeId -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MatchOnlyTypeId|
|:----|:----|
|Tag Format:|`[MatchOnlyTypeId:Value]`|
|Description:|This tag allows you to specify if only the TypeId from the values provided in the `Blocks` tags should be used (eg `MyObjectBuilder_CubeBlock`).|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- Chance -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Chance|
|:----|:----|
|Tag Format:|`[Chance:Value]`|
|Description:|This tag allows you to specify the chance (per block) that this dereliction profile has for running.|
|Allowed Value(s):|Any Integer Greater/Equal To `0`|
|Multiple Tags Allowed:|Yes|

<!-- MinPercentage -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinPercentage|
|:----|:----|
|Tag Format:|`[MinPercentage:Value]`|
|Description:|This tag allows you to specify the Minimum Integrity / Build Level Percentage that the affected blocks will be randomly set to.|
|Allowed Value(s):|Any Integer Greater/Equal To `0`<br />`Value` must be Less Than or Equal to `MaxPercentage` if provided.|
|Multiple Tags Allowed:|No|

<!-- MaxPercentage -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxPercentage|
|:----|:----|
|Tag Format:|`[MaxPercentage:Value]`|
|Description:|This tag allows you to specify the Maximum Integrity / Build Level Percentage that the affected blocks will be randomly set to.|
|Allowed Value(s):|Any Integer Greater/Equal To `0`<br />`Value` must be Greater Than or Equal to `MinPercentage` if provided.|
|Multiple Tags Allowed:|No|

<!-- UseSeparatePercentages -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseSeparatePercentages|
|:----|:----|
|Tag Format:|`[UseSeparatePercentages:Value]`|
|Description:|This tag allows you to specify if different percentages should be applied to Integrity and Build Levels. Use the tags below to specify those ranges|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- MinIntegrityPercentage -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinIntegrityPercentage|
|:----|:----|
|Tag Format:|`[MinIntegrityPercentage:Value]`|
|Description:|This tag allows you to specify the Minimum Integrity Percentage that the affected blocks will be randomly set to.|
|Allowed Value(s):|Any Integer Greater/Equal To `0`<br />`Value` must be Less Than or Equal to `MaxIntegrityPercentage`|
|Multiple Tags Allowed:|No|

<!-- MaxIntegrityPercentage -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxIntegrityPercentage|
|:----|:----|
|Tag Format:|`[MaxIntegrityPercentage:Value]`|
|Description:|This tag allows you to specify the Maximum Integrity Percentage that the affected blocks will be randomly set to.|
|Allowed Value(s):|Any Integer Greater/Equal To `0`<br />`Value` must be Greater Than or Equal to `MinIntegrityPercentage`|
|Multiple Tags Allowed:|No|

<!-- MinBuildPercentage -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinBuildPercentage|
|:----|:----|
|Tag Format:|`[MinBuildPercentage:Value]`|
|Description:|This tag allows you to specify the Minimum Build Level Percentage that the affected blocks will be randomly set to.|
|Allowed Value(s):|Any Integer Greater/Equal To `0`<br />`Value` must be Less Than or Equal to `MaxBuildPercentage`|
|Multiple Tags Allowed:|No|

<!-- MaxBuildPercentage -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxBuildPercentage|
|:----|:----|
|Tag Format:|`[MaxBuildPercentage:Value]`|
|Description:|This tag allows you to specify the Maximum Build Level Percentage that the affected blocks will be randomly set to.|
|Allowed Value(s):|Any Integer Greater/Equal To `0`<br />`Value` must be Greater Than or Equal to `MinBuildPercentage`|
|Multiple Tags Allowed:|No|

