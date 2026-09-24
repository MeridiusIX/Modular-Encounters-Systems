#Tags.md

MES's string-**Tag** broadcast system lets a field target a *label* instead of one specific profile: every Trigger or Event that declares a matching `[Tags:Value]` gets acted on at once, so many profiles can share one tag and one broadcast reaches all of them - no listing every SubtypeId by hand. There are two independent pools, backed by two separate C# classes - **Trigger tags** and **Event tags** - covered below. Several tag-consuming fields also support `{Token}` placeholders (`{Faction}`, `{SpawnGroupName}`, etc.) - see the **Tokens** page for those; this page only notes which fields are token-aware and in what context.

# Declaring Tags

<!--Tags (Trigger)  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Tags|
|:----|:----|
|Tag Format:|`[Tags:Value]`|
|Description:|Declared on a `[RivalAI Trigger]` (or `[MES AI Trigger]`) profile. Labels the Trigger so it can be found later by any of the tag-consuming tags below (`[ManuallyActivatedTriggerTags:]`, `[EnableTriggerTags:]`, etc). Many Triggers can share the same tag on purpose - a broadcast fires all of them at once.|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|
|Matching:|Exact, case-sensitive string comparison against every Trigger's declared Tags|

The same tag on a `[MES Event]` profile declares an **Event tag** instead - a separate pool that never matches a Trigger tag, even with identical spelling. Sharing a tag across profiles on purpose is normal here, so there's no "duplicate tag" warning, unlike SubtypeIds.

# Consuming Tags

These broadcast to every profile declaring a matching `[Tags:]`, unlike `[Triggers:]`/`[Actions:]`-style tags which target one specific SubtypeId. Each also has a SubtypeId-based sibling for targeting one profile directly (e.g. `[ManuallyActivatedTriggerNames:]`) - everything below applies equally to those.

**Requirement:** none of these reach a Trigger that isn't already attached to the grid's own Behavior - a tag broadcast only searches that grid's own, already-loaded Triggers, never a global lookup. Add the Trigger with `[Triggers:Value]`, or bundle several tagged variants into one `[TriggerGroups:Value]` if you're targeting many at once with a shared broadcast. See Scope and Overhead below.

<!--ManuallyActivatedTriggerTags  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ManuallyActivatedTriggerTags|
|:----|:----|
|Tag Format:|`[ManuallyActivatedTriggerTags:Value]`|
|Description:|Manually activates every Trigger Profile (in both the grid's normal Triggers and its Compromised Triggers) whose `[Tags:Value]` matches. Must be paired with `[ManuallyActivateTrigger:true]`.|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|
|Declared On:|`[RivalAI Action]` / `[MES AI Action]`|
|Token Support:|Yes - see the **Tokens** page|

<!--EnableTriggerTags  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|EnableTriggerTags|
|:----|:----|
|Tag Format:|`[EnableTriggerTags:Value]`|
|Description:|Sets `UseTrigger:true` on every Trigger Profile whose `[Tags:Value]` matches. Must be paired with `[EnableTriggers:true]`. Its SubtypeId-based sibling, `[EnableTriggerNames:]`, can also be written as `[EnableTriggerIds:]` - both spellings set the exact same list, use whichever you prefer.|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|
|Declared On:|`[RivalAI Action]` / `[MES AI Action]`|
|Token Support:|Yes - see the **Tokens** page|

<!--DisableTriggerTags  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|DisableTriggerTags|
|:----|:----|
|Tag Format:|`[DisableTriggerTags:Value]`|
|Description:|Sets `UseTrigger:false` on every Trigger Profile whose `[Tags:Value]` matches. Must be paired with `[DisableTriggers:true]`. Its SubtypeId-based sibling, `[DisableTriggerNames:]`, can also be written as `[DisableTriggerIds:]` - both spellings set the exact same list, use whichever you prefer.|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|
|Declared On:|`[RivalAI Action]` / `[MES AI Action]`|
|Token Support:|Yes - see the **Tokens** page|

<!--ResetTriggerCooldownTags  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ResetTriggerCooldownTags|
|:----|:----|
|Tag Format:|`[ResetTriggerCooldownTags:Value]`|
|Description:|Resets the cooldown timer on every Trigger Profile whose `[Tags:Value]` matches. Must be paired with `[ResetCooldownTimeOfTriggers:true]`.|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|
|Declared On:|`[RivalAI Action]` / `[MES AI Action]`|
|Token Support:|Yes - see the **Tokens** page|

<!--ActivateEventTags  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ActivateEventTags|
|:----|:----|
|Tag Format:|`[ActivateEventTags:Value]`|
|Description:|Cross-system bridge from RivalAI into MES Events - immediately runs the Actions of every valid MES Event whose `[Tags:Value]` matches. Must be paired with `[ActivateEvent:true]`. Has no MES Event Action equivalent.|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|
|Declared On:|`[RivalAI Action]` / `[MES AI Action]` only|
|Token Support:|Yes - see the **Tokens** page|

<!--ToggleEventTags  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ToggleEventTags|
|:----|:----|
|Tag Format:|`[ToggleEventTags:Value]`|
|Description:|Enables or disables every MES Event whose `[Tags:Value]` matches (direction set per-entry with the paired `[ToggleEventTagModes:Value]` boolean list). Must be paired with `[ToggleEvents:true]`. This tag exists on both `[RivalAI Action]`/`[MES AI Action]` and `[MES Event Action]` - see the Token Support row.|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|
|Declared On:|`[RivalAI Action]` / `[MES AI Action]` **or** `[MES Event Action]`|
|Token Support:|Only when declared on `[RivalAI Action]`/`[MES AI Action]` - does **not** resolve when declared on `[MES Event Action]`. See the gotcha below.|

<!--ResetEventCooldownTags  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|ResetEventCooldownTags|
|:----|:----|
|Tag Format:|`[ResetEventCooldownTags:Value]`|
|Description:|Resets the cooldown timer on every MES Event whose `[Tags:Value]` matches. Must be paired with `[ResetCooldownTimeOfEvents:true]`. This tag exists on both `[RivalAI Action]`/`[MES AI Action]` and `[MES Event Action]` - see the Token Support row.|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|
|Declared On:|`[RivalAI Action]` / `[MES AI Action]` **or** `[MES Event Action]`|
|Token Support:|Only when declared on `[RivalAI Action]`/`[MES AI Action]` - does **not** resolve when declared on `[MES Event Action]`. See the gotcha below.|

<!--IncreaseRunCountEventTags  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|IncreaseRunCountEventTags|
|:----|:----|
|Tag Format:|`[IncreaseRunCountEventTags:Value]`|
|Description:|Increases the run count on every MES Event whose `[Tags:Value]` matches, by the paired `[IncreaseRunCountEventTagAmount:Value]` integer list. Must be paired with `[IncreaseRunCountOfEvents:true]`. Has no RivalAI-side equivalent.|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|
|Declared On:|`[MES Event Action]` only|
|Token Support:|No|

**Gotcha:** `[ToggleEventTags:]`/`[ToggleEventIds:]` and `[ResetEventCooldownTags:]`/`[ResetEventCooldownIds:]` can be declared on **either** a `[RivalAI Action]`/`[MES AI Action]` profile (attached to a Trigger on a spawned grid) **or** a `[MES Event Action]` profile (a global MES Event). The tag name and behavior are the same either way, but which one you use changes whether tokens in the value will resolve - see the **Tokens** page.

# Example

A set of "point value" Triggers that only differ by encounter type and faction, activated by one reusable Action that doesn't need to know in advance which grid it's running on.

Tag each Trigger variant:

```
<EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
  <Id>
    <TypeId>Inventory</TypeId>
    <SubtypeId>MyMod-Trigger-BaseCompromised-AllianceBase-KHAANEPH</SubtypeId>
  </Id>
  <Description>
    [RivalAI Trigger]
    [UseTrigger:true]
    [Tags:MyMod-TriggerTags-BaseCompromised-AllianceBase-KHAANEPH]
    [Type:Manual]
    [Actions:MyMod-Action-BaseCompromised]
  </Description>
</EntityComponent>
```

Set the custom string the pattern needs, early, while NPC data is live (see **Tokens** for `[CustomStrings:]`):

```
[SetCustomStrings:true]
[CustomStrings:EncounterType,AllianceBase]
```

(`{Faction}` needs no setup - it always resolves to the NPC's own faction automatically.)

Broadcast from one reusable Action:

```
<EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
  <Id>
    <TypeId>Inventory</TypeId>
    <SubtypeId>MyMod-Action-BaseCompromised</SubtypeId>
  </Id>
  <Description>
    [ManuallyActivateTrigger:true]
    [ManuallyActivatedTriggerTags:MyMod-TriggerTags-BaseCompromised-{EncounterType}-{Faction}]
  </Description>
</EntityComponent>
```

`{EncounterType}` and `{Faction}` resolve before the broadcast, producing `MyMod-TriggerTags-BaseCompromised-AllianceBase-KHAANEPH` - matching the first Trigger's `[Tags:]` and firing it. Reuse the same Action line across every faction and encounter type.

# Scope and Overhead

**Every tag broadcast only searches the current grid's own, already-loaded Triggers - there is no global registry.** A Trigger never attached to this specific grid's Behavior (via `[Triggers:Value]` or `[TriggerGroups:Value]`) is simply not in scope to be found by tag - the first Common Pitfall below is exactly what happens if you forget this.

So the dynamic-per-faction/per-encounter-type behavior in the example above still needs every variant (`...-AllianceBase-KHAANEPH`, `...-AllianceBase-SOBAN`, `...-AllianceOutpost-KHAANEPH`, etc.) attached to any Behavior that might use it - almost always via one `[TriggerGroups:Value]` bundling them all, since `[TriggerGroups:]` saves you from repeating the list per Behavior but still loads every Trigger it contains onto every grid that uses it. The tag system saves you from writing a separate broadcast per variant, not from needing the variants to exist - that's just file/authoring bulk, not a performance concern.

# Common Pitfalls

**A tag that matches nothing fails completely silently.** There's no error, no warning, nothing in the logs - the broadcast's exact-match check just finds zero profiles and does nothing. If a `ManuallyActivateTrigger`/`EnableTriggers`/`DisableTriggers`/`ToggleEvents`/etc. action doesn't seem to be doing anything, double-check the tag spelling (including case) on both sides first.

***

**The same `[ToggleEventTags:]`/`[ToggleEventIds:]`/`[ResetEventCooldownTags:]`/`[ResetEventCooldownIds:]` tags behave differently depending on where you put them.** On a `[RivalAI Action]`/`[MES AI Action]` (attached to a grid's own Trigger), tokens resolve normally. On a `[MES Event Action]`, they don't. If you're moving logic between the two systems, this is an easy thing to miss.

***

**Trigger tags and Event tags are separate pools, even if you reuse the same string.** A Trigger declaring `[Tags:MyLabel]` will never be found by `[ActivateEventTags:MyLabel]` (which only searches Event tags), and an Event declaring `[Tags:MyLabel]` will never be found by `[ManuallyActivatedTriggerTags:MyLabel]` (which only searches Trigger tags).

***

**A tag-broadcast field can only find Triggers already attached to that grid's own Behavior** - there's no global trigger registry. See Scope and Overhead above.
