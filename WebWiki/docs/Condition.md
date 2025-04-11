#Condition.md

Condition Profiles in RivalAI allow you to define some extra conditions that must be met before a Trigger Profile can execute its Actions. It is important that you use a unique SubtypeId for each Condition Profile you create, otherwise they may not work correctly.

Here's an example of how a Condition Profile Definition is setup:

```
<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>RAI-ExampleConditionProfile</SubtypeId>
      </Id>
      <Description>

      [RivalAI Condition]
      
      [UseConditions:true]
      [MatchAnyCondition:true]
      
      </Description>
      
    </EntityComponent>

  </EntityComponents>
</Definitions>
```

<!---->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseConditions|
|:----|:----|
|Tag Format:|`[UseConditions:Value]`|
|Description:|This tag specifies if the Condition Profile should be active / used.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--MatchAnyCondition-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MatchAnyCondition|
|:----|:----|
|Tag Format:|`[MatchAnyCondition:Value]`|
|Description:|This tag allows you to require if all conditions in the profile must be met (`false`), or if any condition can be met (`true`).|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--CheckAllLoadedModIDs-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckAllLoadedModIDs|
|:----|:----|
|Tag Format:|`[CheckAllLoadedModIDs:Value]`|
|Description:|This tag allows you to check for mods currently loaded in your game world. For this condition to be satisfied, all mod IDs included in `AllModIDsToCheck` must be present in the world.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--AllModIDsToCheck-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AllModIDsToCheck|
|:----|:----|
|Tag Format:|`[AllModIDsToCheck:Value]`|
|Description:|Specifies a mod ID you want to have checked if `CheckAllLoadedModIDs` is `true`.|
|Allowed Values:|Any Mod ID|
|Multiple Tag Allowed:|Yes|

<!--CheckAnyLoadedModIDs-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckAnyLoadedModIDs|
|:----|:----|
|Tag Format:|`[CheckAnyLoadedModIDs:Value]`|
|Description:|This tag allows you to check for mods currently loaded in your game world. For this condition to be satisfied, any mod IDs included in `AnyModIDsToCheck` must be present in the world.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--AnyModIDsToCheck-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AnyModIDsToCheck|
|:----|:----|
|Tag Format:|`[AnyModIDsToCheck:Value]`|
|Description:|Specifies a mod ID you want to have checked if `CheckAnyLoadedModIDs` is `true`.|
|Allowed Values:|Any Mod ID|
|Multiple Tag Allowed:|Yes|

<!--CheckTrueBooleans-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckTrueBooleans|
|:----|:----|
|Tag Format:|`[CheckTrueBooleans:Value]`|
|Description:|This tag allows you to check for Boolean Variables stored in the Drone Behavior that are `true`. All provided variables must be `true` for this condition to be satisfied.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--TrueBooleans-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TrueBooleans|
|:----|:----|
|Tag Format:|`[TrueBooleans:Value]`|
|Description:|Specifies the name of a Boolean Variable you want to have checked if `CheckTrueBooleans` is `true`.|
|Allowed Values:|Any name string excluding characters `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--AllowAnyTrueBoolean-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AllowAnyTrueBoolean|
|:----|:----|
|Tag Format:|`[AllowAnyTrueBoolean:Value]`|
|Description:|This tag allows you to specify if the condition should pass if any of the provided boolean names evaluate as true.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--CheckCustomCounters-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckCustomCounters|
|:----|:----|
|Tag Format:|`[CheckCustomCounters:Value]`|
|Description:|This tag allows you to check for Integer Counter Variables stored in the Drone Behavior. All provided variables must be equal or higher for this condition to be satisfied.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--CustomCounters-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomCounters|
|:----|:----|
|Tag Format:|`[CustomCounters:Value]`|
|Description:|Specifies the name of an Integer Counter Variable you want to have checked if `CheckCustomCounters` is `true`. You must also provide a value to `CustomCountersTargets` as well for this tag to work.|
|Allowed Values:|Any name string excluding characters `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--CustomCountersTargets-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomCountersTargets|
|:----|:----|
|Tag Format:|`[CustomCountersTargets:Value]`|
|Description:|Specifies the target value of an Integer Counter Variable you want to have checked if `CheckCustomCounters` is `true`. You must also provide a value to `CustomCounters` as well for this tag to work.|
|Allowed Values:|Any interger equal or greater than `0`|
|Multiple Tag Allowed:|Yes|

<!--CounterCompareTypes-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CounterCompareTypes|
|:----|:----|
|Tag Format:|`[CounterCompareTypes:Value]`|
|Description:|Specifies the logic used to determine if the behavior counter check passes or not for each provided counter. If no value is provided for a counter name, then `GreaterOrEqual` is used by default.|
|Allowed Values:|`GreaterOrEqual`<br>`Greater`<br>`Equal`<br>`NotEqual`<br>`Less`<br>`LessOrEqual`|
|Multiple Tag Allowed:|Yes|

<!--AllowAnyValidCounter-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AllowAnyValidCounter|
|:----|:----|
|Tag Format:|`[AllowAnyValidCounter:Value]`|
|Description:|This tag allows you to specify if the condition should pass if any of the provided counters pass their check.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--CheckTrueSandboxBooleans-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckTrueSandboxBooleans|
|:----|:----|
|Tag Format:|`[CheckTrueSandboxBooleans:Value]`|
|Description:|This tag allows you to check for Boolean Variables stored in the Save File that are `true`. All provided variables must be `true` for this condition to be satisfied.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--TrueSandboxBooleans-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TrueSandboxBooleans|
|:----|:----|
|Tag Format:|`[TrueSandboxBooleans:Value]`|
|Description:|Specifies the name of a Sandbox Boolean Variable you want to have checked if `CheckTrueSandboxBooleans` is `true`.|
|Allowed Values:|Any name string excluding characters `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--AllowAnyTrueSandboxBoolean-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AllowAnyTrueSandboxBoolean|
|:----|:----|
|Tag Format:|`[AllowAnyTrueSandboxBoolean:Value]`|
|Description:|This tag allows you to specify if the condition should pass if any of the provided boolean names evaluate as true.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--CheckCustomSandboxCounters-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckCustomSandboxCounters|
|:----|:----|
|Tag Format:|`[CheckCustomSandboxCounters:Value]`|
|Description:|This tag allows you to check for Integer Counter Variables stored in the Save File. All provided variables must be equal or higher for this condition to be satisfied.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--CustomSandboxCounters-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomSandboxCounters|
|:----|:----|
|Tag Format:|`[CustomSandboxCounters:Value]`|
|Description:|Specifies the name of an Integer Counter Variable you want to have checked if `CheckCustomSandboxCounters` is `true`. You must also provide a value to `CustomSandboxCountersTargets` as well for this tag to work.|
|Allowed Values:|Any name string excluding characters `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--CustomSandboxCountersTargets-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CustomSandboxCountersTargets|
|:----|:----|
|Tag Format:|`[CustomSandboxCountersTargets:Value]`|
|Description:|Specifies the target value of an Integer Counter Variable you want to have checked if `CheckCustomSandboxCounters` is `true`. You must also provide a value to `CustomSandboxCounters` as well for this tag to work.|
|Allowed Values:|Any interger equal or greater than `0`|
|Multiple Tag Allowed:|Yes|

<!--SandboxCounterCompareTypes-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SandboxCounterCompareTypes|
|:----|:----|
|Tag Format:|`[SandboxCounterCompareTypes:Value]`|
|Description:|Specifies the logic used to determine if the sandbox counter check passes or not for each provided counter. If no value is provided for a counter name, then `GreaterOrEqual` is used by default.|
|Allowed Values:|`GreaterOrEqual`<br>`Greater`<br>`Equal`<br>`NotEqual`<br>`Less`<br>`LessOrEqual`|
|Multiple Tag Allowed:|Yes|

<!--AllowAnyValidSandboxCounter-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AllowAnyValidSandboxCounter|
|:----|:----|
|Tag Format:|`[AllowAnyValidSandboxCounter:Value]`|
|Description:|This tag allows you to specify if the condition should pass if any of the provided counters pass their check.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--CheckGridSpeed-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckGridSpeed|
|:----|:----|
|Tag Format:|`[CheckGridSpeed:Value]`|
|Description:|This tag allows you to check the current NPC grid speed. For this condition to be satisfied, the grid speed must be between `MinGridSpeed` and `MaxGridSpeed`.|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--MinGridSpeed-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinGridSpeed|
|:----|:----|
|Tag Format:|`[MinGridSpeed:Value]`|
|Description:|The minimum grid speed that must be met if `CheckGridSpeed` is `true`.|
|Allowed Values:|Any number equal or greater than `0`<br />Must be lower than `MaxGridSpeed`|
|Multiple Tag Allowed:|No|

<!--MaxGridSpeed-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxGridSpeed|
|:----|:----|
|Tag Format:|`[MaxGridSpeed:Value]`|
|Description:|The maximum grid speed that must be met if `CheckGridSpeed` is `true`.|
|Allowed Values:|Any number equal or greater than `0`<br />Must be higher than `MinGridSpeed`|
|Multiple Tag Allowed:|No|

<!--CheckMESBlacklistedSpawnGroups-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckMESBlacklistedSpawnGroups|
|:----|:----|
|Tag Format:|`[CheckMESBlacklistedSpawnGroups:Value]`|
|Description:|This tag allows you to check the Modular Encounters Spawner NPC SpawnGroup Blacklist for entries. For this condition to be satisfied..|
|Allowed Values:|`true`<br>`false`|
|Multiple Tag Allowed:|No|

<!--SpawnGroupBlacklistContainsAll-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SpawnGroupBlacklistContainsAll|
|:----|:----|
|Tag Format:|`[SpawnGroupBlacklistContainsAll:Value]`|
|Description:|Specifies the name of a SpawnGroup SubtypeID you want checked for if `CheckMESBlacklistedSpawnGroups` is `true`. If this tag contains values, then **All** values must be present in the BlackList for the condition to be satisfied.|
|Allowed Values:|Any name string excluding characters `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--SpawnGroupBlacklistContainsAny-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SpawnGroupBlacklistContainsAny|
|:----|:----|
|Tag Format:|`[SpawnGroupBlacklistContainsAny:Value]`|
|Description:|Specifies the name of a SpawnGroup SubtypeID you want checked for if `CheckMESBlacklistedSpawnGroups` is `true`. If this tag contains values, then **Any** of values must be present in the BlackList for the condition to be satisfied.|
|Allowed Values:|Any name string excluding characters `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--UseRequiredFunctionalBlocks-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseRequiredFunctionalBlocks|
|:----|:----|
|Tag Format:|`[UseRequiredFunctionalBlocks:Value]`|
|Description:|Specifies if the condition should check for certain blocks that exist in a functional status on the grid.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--RequiredAllFunctionalBlockNames-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RequiredAllFunctionalBlockNames|
|:----|:----|
|Tag Format:|`[RequiredAllFunctionalBlockNames:Value]`|
|Description:|Specifies the name of a Block you want checked for if `UseRequiredFunctionalBlocks` is `true`. If this tag contains values, then the grid must have **All** blocks (in a working/functional state) with names specified for the condition to be satisfied.|
|Allowed Values:|Any name string excluding characters `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--RequiredAnyFunctionalBlockNames-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RequiredAnyFunctionalBlockNames|
|:----|:----|
|Tag Format:|`[RequiredAnyFunctionalBlockNames:Value]`|
|Description:|Specifies the name of a Block you want checked for if `UseRequiredFunctionalBlocks` is `true`. If this tag contains values, then the grid must have **Any** blocks (in a working/functional state) with names specified for the condition to be satisfied.|
|Allowed Values:|Any name string excluding characters `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--RequiredNoneFunctionalBlockNames-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RequiredNoneFunctionalBlockNames|
|:----|:----|
|Tag Format:|`[RequiredNoneFunctionalBlockNames:Value]`|
|Description:|Specifies the name of a Block you want checked for if `UseRequiredFunctionalBlocks` is `true`. If this tag contains values, then the grid must have **No** blocks (in a working/functional state) with names specified for the condition to be satisfied.|
|Allowed Values:|Any name string excluding characters `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|

<!--CheckTargetAltitudeDifference-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckTargetAltitudeDifference|
|:----|:----|
|Tag Format:|`[CheckTargetAltitudeDifference:Value]`|
|Description:|Specifies if the condition should check the altitude difference between itself and its current target. Only works if NPC has a valid target and the NPC is in planetary gravity.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--MinTargetAltitudeDifference-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinTargetAltitudeDifference|
|:----|:----|
|Tag Format:|`[MinTargetAltitudeDifference:Value]`|
|Description:|The minimum target altitude difference that must be met if `CheckTargetAltitudeDifference` is `true`.|
|Allowed Values:|Any number equal or greater than `0`<br />Must be lower than `MaxTargetAltitudeDifference`|
|Multiple Tag Allowed:|No|

<!--MaxTargetAltitudeDifference-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxTargetAltitudeDifference|
|:----|:----|
|Tag Format:|`[MaxTargetAltitudeDifference:Value]`|
|Description:|The maximum target altitude difference that must be met if `CheckTargetAltitudeDifference` is `true`.|
|Allowed Values:|Any number equal or greater than `0`<br />Must be higher than `MinTargetAltitudeDifference`|
|Multiple Tag Allowed:|No|

<!--CheckTargetDistance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckTargetDistance|
|:----|:----|
|Tag Format:|`[CheckTargetDistance:Value]`|
|Description:|Specifies if the condition should check the distance difference between itself and its current target. Only works if NPC has a valid target.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--MinTargetDistance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinTargetDistance|
|:----|:----|
|Tag Format:|`[MinTargetDistance:Value]`|
|Description:|The minimum target distance difference that must be met if `CheckTargetDistance` is `true`.|
|Allowed Values:|Any number equal or greater than `0`<br />Must be lower than `MaxTargetDistance`|
|Multiple Tag Allowed:|No|

<!--MaxTargetDistance-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxTargetDistance|
|:----|:----|
|Tag Format:|`[MaxTargetDistance:Value]`|
|Description:|The maximum target distance difference that must be met if `CheckTargetDistance` is `true`.|
|Allowed Values:|Any number equal or greater than `0`<br />Must be higher than `MinTargetDistance`|
|Multiple Tag Allowed:|No|

<!--CheckTargetAngleFromForward-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckTargetAngleFromForward|
|:----|:----|
|Tag Format:|`[CheckTargetAngleFromForward:Value]`|
|Description:|Specifies if the condition should check the angle difference between itself (forward direction) and its current target. Only works if NPC has a valid target.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--MinTargetAngle-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinTargetAngle|
|:----|:----|
|Tag Format:|`[MinTargetAngle:Value]`|
|Description:|The minimum target angle difference that must be met if `CheckTargetAngleFromForward` is `true`.|
|Allowed Values:|Any number equal or greater than `0`<br />Must be lower than `MaxTargetAngle`|
|Multiple Tag Allowed:|No|

<!--MaxTargetAngle-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxTargetAngle|
|:----|:----|
|Tag Format:|`[MaxTargetAngle:Value]`|
|Description:|The maximum target angle difference that must be met if `CheckTargetAngleFromForward` is `true`.|
|Allowed Values:|Any number equal or greater than `0`<br />Must be higher than `MinTargetAngle`|
|Multiple Tag Allowed:|No|

<!--CheckIfTargetIsChasing-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckIfTargetIsChasing|
|:----|:----|
|Tag Format:|`[CheckIfTargetIsChasing:Value]`|
|Description:|Specifies if the condition should check the velocity direction angle from the current target to the NPC. Only works if NPC has a valid target.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--MinTargetChaseAngle-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinTargetChaseAngle|
|:----|:----|
|Tag Format:|`[MinTargetChaseAngle:Value]`|
|Description:|The minimum target velocity angle difference that must be met if `CheckIfTargetIsChasing` is `true`.|
|Allowed Values:|Any number equal or greater than `0`<br />Must be lower than `MaxTargetChaseAngle`|
|Multiple Tag Allowed:|No|

<!--MaxTargetChaseAngle-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxTargetChaseAngle|
|:----|:----|
|Tag Format:|`[MaxTargetChaseAngle:Value]`|
|Description:|The maximum target velocity angle difference that must be met if `CheckIfTargetIsChasing` is `true`.|
|Allowed Values:|Any number equal or greater than `0`<br />Must be higher than `MinTargetChaseAngle`|
|Multiple Tag Allowed:|No|

<!--CheckIfGridNameMatches-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckIfGridNameMatches|
|:----|:----|
|Tag Format:|`[CheckIfGridNameMatches:Value]`|
|Description:|Specifies if the condition should check the name of the NPC CubeGrid matches a provided value.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--AllowPartialGridNameMatches-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AllowPartialGridNameMatches|
|:----|:----|
|Tag Format:|`[AllowPartialGridNameMatches:Value]`|
|Description:|Specifies if the NPC CubeGrid name can be a partial match instead of exact.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--GridNamesToCheck-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|GridNamesToCheck|
|:----|:----|
|Tag Format:|`[GridNamesToCheck:Value]`|
|Description:|Specifies the name(s) to check against the NPC CubeGrid name.|
|Allowed Values:|Any Grid Name|
|Multiple Tag Allowed:|Yes|

<!--AltitudeCheck-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|AltitudeCheck|
|:----|:----|
|Tag Format:|`[AltitudeCheck:Value]`|
|Description:|Specifies if the Altitude of the NPC should be checked for min/max values.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--MinAltitude-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinAltitude|
|:----|:----|
|Tag Format:|`[MinAltitude:Value]`|
|Description:|The minimum NPC altitude that must be met if `AltitudeCheck` is `true`.|
|Allowed Values:|Any number equal or greater than `0`<br />Must be lower than `MaxAltitude`|
|Multiple Tag Allowed:|No|

<!--MaxAltitude-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxAltitude|
|:----|:----|
|Tag Format:|`[MaxAltitude:Value]`|
|Description:|The maximum NPC altitude that must be met if `AltitudeCheck` is `true`.|
|Allowed Values:|Any number equal or greater than `0`<br />Must be higher than `MinAltitude`|
|Multiple Tag Allowed:|No|

<!--CheckIfDamagerIsPlayer-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckIfDamagerIsPlayer|
|:----|:----|
|Tag Format:|`[CheckIfDamagerIsPlayer:Value]`|
|Description:|If trigger was activated by a Damage event, this tag specifies if the Damager should be a Player Identity.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--CheckIfDamagerIsNpc-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckIfDamagerIsNpc|
|:----|:----|
|Tag Format:|`[CheckIfDamagerIsNpc:Value]`|
|Description:|If trigger was activated by a Damage event, this tag specifies if the Damager should be an NPC Identity.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--CheckIfTargetIsPlayerOwned-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckIfTargetIsPlayerOwned|
|:----|:----|
|Tag Format:|`[CheckIfTargetIsPlayerOwned:Value]`|
|Description:|Specifies if the current NPC targer should be a Player Owned entity.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--CheckIfTargetIsNpcOwned-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckIfTargetIsNpcOwned|
|:----|:----|
|Tag Format:|`[CheckIfTargetIsNpcOwned:Value]`|
|Description:|Specifies if the current NPC targer should be an NPC Owned entity.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--CheckCommandGridValue-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckCommandGridValue|
|:----|:----|
|Tag Format:|`[CheckCommandGridValue:Value]`|
|Description:|If trigger was activated by a Command event, this tag specifies if a check should be performed on the Grid Value (aka Threat Score) of the NPC that sent the command.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--CommandGridValue-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CommandGridValue|
|:----|:----|
|Tag Format:|`[CommandGridValue:Value]`|
|Description:|If using `CheckCommandGridValue`, this tag specifies the Grid Value that the received Grid Value from the command should be compared against.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!--CheckCommandGridValueCompare-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckCommandGridValueCompare|
|:----|:----|
|Tag Format:|`[CheckCommandGridValueCompare:Value]`|
|Description:|If using `CheckCommandGridValue`, this tag specifies how the Received Grid Value is compared against the value provided in `CommandGridValue`.|
|Allowed Values:|`Equal`<br />`NotEqual`<br />`Greater`<br />`GreaterOrEqual`<br />`Less`<br />`LessOrEqual`|
|Multiple Tag Allowed:|No|

<!--CompareCommandGridValue-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CompareCommandGridValue|
|:----|:----|
|Tag Format:|`[CompareCommandGridValue:Value]`|
|Description:|If trigger was activated by a Command event, this tag specifies if a check should be performed on the Grid Value (aka Threat Score) of the NPC that sent the command. This check compares the received Grid Value directly against the value of the NPC that received the command / activated the trigger.|
|Allowed Values:|`true`<br />`false`|
|Multiple Tag Allowed:|No|

<!--CompareCommandGridValueMode-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CompareCommandGridValueMode|
|:----|:----|
|Tag Format:|`[CompareCommandGridValueMode:Value]`|
|Description:|If using `CompareCommandGridValue`, this tag specifies how the Received Grid Value is compared against the Grid Value of the NPC.|
|Allowed Values:|`Equal`<br />`NotEqual`<br />`Greater`<br />`GreaterOrEqual`<br />`Less`<br />`LessOrEqual`|
|Multiple Tag Allowed:|No|

<!--CompareCommandGridValueSelfMultiplier-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CompareCommandGridValueSelfMultiplier|
|:----|:----|
|Tag Format:|`[CompareCommandGridValueSelfMultiplier:Value]`|
|Description:|If using `CompareCommandGridValue`, this tag specifies a multiplier that is applied to the Grid Value of this NPC before the comparing of values is completed.|
|Allowed Values:|Any Number Greater Than `0`|
|Multiple Tag Allowed:|No|

<!-- CommandGravityCheck -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CommandGravityCheck|
|:----|:----|
|Tag Format:|`[CommandGravityCheck:Value]`|
|Description:|This tag allows you to specify if both NPCs involved with transmission of a command are inside the same gravity field.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- CommandGravityMatches -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CommandGravityMatches|
|:----|:----|
|Tag Format:|`[CommandGravityMatches:Value]`|
|Description:|This tag allows you to specify if the gravity of both NPCs involved with the transmission of a Command should match.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- UseFailCondition -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|UseFailCondition|
|:----|:----|
|Tag Format:|`[UseFailCondition:Value]`|
|Description:|This tag specifies if the Condition Profile should pass if the conditions fail. Should all conditions pass, then the profile would evalate as failed.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- CheckForBlocksOfType -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckForBlocksOfType|
|:----|:----|
|Tag Format:|`[CheckForBlocksOfType:Value]`|
|Description:|This tag specifies if certain working/functional block types should be searched for on the grid. If all provided types are found (at least one block of each type), then this condition passes.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- BlocksOfType -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|BlocksOfType|
|:----|:----|
|Tag Format:|`[BlocksOfType:Value]`|
|Description:|This tag specifies the types of blocks that are searched for on the grid.|
|Allowed Value(s):|Any Block TypeId<br />eg: `MyObjectBuilder_BatteryBlock`|
|Multiple Tags Allowed:|Yes|

<!-- CheckHorizonAngle -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckHorizonAngle|
|:----|:----|
|Tag Format:|`[CheckHorizonAngle:Value]`|
|Description:|This tag allows you to check the NPC current forward direction against the angle of the horizon (90 degree from 'up' on a planet)|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- MinHorizonAngle -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MinHorizonAngle|
|:----|:----|
|Tag Format:|`[MinHorizonAngle:Value]`|
|Description:|This tag allows you to specify the minimum angle that is checked against the horizon if using `CheckHorizonAngle`|
|Allowed Value(s):|Any Number Greater/Equal To `0`<br />`Value` must be Less Than or Equal to `MaxHorizonAngle` if provided.|
|Multiple Tags Allowed:|No|

<!-- MaxHorizonAngle -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|MaxHorizonAngle|
|:----|:----|
|Tag Format:|`[MaxHorizonAngle:Value]`|
|Description:|This tag allows you to specify the maximum angle that is checked against the horizon if using `CheckHorizonAngle`|
|Allowed Value(s):|Any Number Greater/Equal To `0`<br />`Value` must be Less Than or Equal to `MinHorizonAngle` if provided.|
|Multiple Tags Allowed:|No|

<!-- CheckForSpawnConditions -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckForSpawnConditions|
|:----|:----|
|Tag Format:|`[CheckForSpawnConditions:Value]`|
|Description:|This tag allows you to check if the encounter was spawned using a specific SpawnCondition Profile.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- RequiredSpawnConditions -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|RequiredSpawnConditions|
|:----|:----|
|Tag Format:|`[RequiredSpawnConditions:Value]`|
|Description:|This tag allows you to specify one or more SpawnCondition Profile SubtypeIds that will be checked against if using `CheckForSpawnConditions`.|
|Allowed Value(s):|Any String Value|
|Multiple Tags Allowed:|Yes|

<!-- CheckForPlanetaryLane-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|CheckForPlanetaryLane|
|:----|:----|
|Tag Format:|`[CheckForPlanetaryLane:Value]`|
|Description:|This tag allows you to check if the encounter is currently inside or outside of a Planetary Lane.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- PlanetaryLanePassValue-->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PlanetaryLanePassValue|
|:----|:----|
|Tag Format:|`[PlanetaryLanePassValue:Value]`|
|Description:|This tag specifies whether the grid must be in a planetary lane in order for the `CheckForPlanetaryLane` check to pass.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- IsAttackerHostile -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IsAttackerHostile|
|:----|:----|
|Tag Format:|`[IsAttackerHostile:Value]`|
|Description:|This tag allows you to specify if the condition should check if the attacking entity has a hostile relation with the NPC. This tag should only be used on triggers using the `Damage` type.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- IsAttackerNeutral -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IsAttackerNeutral|
|:----|:----|
|Tag Format:|`[IsAttackerNeutral:Value]`|
|Description:|This tag allows you to specify if the condition should check if the attacking entity has a neutral relation with the NPC. This tag should only be used on triggers using the `Damage` type.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|

<!-- IsAttackerFriendly -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IsAttackerFriendly|
|:----|:----|
|Tag Format:|`[IsAttackerFriendly:Value]`|
|Description:|This tag allows you to specify if the condition should check if the attacking entity has a friendly relation with the NPC. This tag should only be used on triggers using the `Damage` type.|
|Allowed Value(s):|`true`<br />`false`|
|Multiple Tags Allowed:|No|
