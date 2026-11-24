#Tokens.md

Several tag values across MES support `{Token}`-style placeholders that get replaced with real data immediately before the value is used - for example `{Faction}` becomes the NPC's actual faction tag, so `[ChatData:Hello {Faction}!]` might become `[ChatData:Hello KHAANEPH!]` at runtime. This substitution is **not universal** - it only happens for a specific set of tag fields, and only when there is a real NPC grid backing the action. Internally, most of this substitution runs through a single class (`IdsReplacer`), though a couple of the fields noted below use their own separate, more limited replace logic instead.

# Scope

Token substitution needs an actual spawned NPC's data (its faction, its spawn group name, its custom strings, etc.) to pull values from. That data only exists when an action is running as part of a **RivalAI Behavior** on a spawned grid (i.e. triggered by that grid's own Trigger/Condition/Action chain). MES's global **Event** system has no such concept - an Event isn't tied to any one grid, so there's no NPC data to substitute from.

This has two consequences that trip people up constantly:

1. **A token in a plain profile-name reference never resolves, in any context.** Tags like `[Triggers:]`, `[TriggerGroups:]`, `[Conditions:]`, `[Actions:]`, `[ManipulationProfiles:]`, `[LootProfiles:]`, `[ContainerTypes:]`, etc. are always a direct, static lookup of a profile's SubtypeId - there is no substitution step involved at all. Writing `[Triggers:MyTrigger-{Faction}]` will never find your `MyTrigger-KHAANEPH` profile; it will always look for a profile literally named `MyTrigger-{Faction}`, fail, and silently do nothing.
2. **A token only resolves when the field carrying it is actually being processed from a live RivalAI Behavior.** A handful of fields exist on both a RivalAI-side profile and an MES Event profile (see Other Token-Aware Fields below, and the **Tags** page for the tag-broadcast fields) - the same field name behaves differently depending on which one you put it on.

# Available Tokens

Everything below except `{<SandboxVariableName>}` requires live NPC data (a RivalAI Behavior context) - none of them resolve from inside an MES Event. Sandbox variables are the one exception that works everywhere, because they're global session state rather than per-NPC data.

<!--Faction (Token)  -->
|Token:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|`{Faction}`|
|:----|:----|
|Replaced With:|The NPC's own faction tag|
|Requires:|Live NPC data|

<!--SpawnGroupName (Token)  -->
|Token:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|`{SpawnGroupName}`|
|:----|:----|
|Replaced With:|The full name of the SpawnGroup that spawned this NPC|
|Requires:|Live NPC data|

<!--SpawnGroupNameTruncated (Token)  -->
|Token:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|`{SpawnGroupNameTruncated}`|
|:----|:----|
|Replaced With:|Same as `{SpawnGroupName}`, with a trailing `_SpawnGroup` suffix stripped|
|Requires:|Live NPC data|

<!--EventInstance (Token)  -->
|Token:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|`{EventInstance}`|
|:----|:----|
|Replaced With:|The unique instance ID of the spawning event, if any|
|Requires:|Live NPC data|

<!--CustomVariablesName (Token)  -->
|Token:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|`{CustomVariablesName}`|
|:----|:----|
|Replaced With:|`npcData.CustomVariablesName`, which is copied from the SpawnGroup's own `CustomVariablesName` field at spawn time.|
|Requires:|Live NPC data - but as of this writing, no SBC tag anywhere sets the SpawnGroup's `CustomVariablesName`, so this token currently always resolves to an empty string. Listed here for completeness in case that changes in a future MES version; there's no way to populate it today.|

<!--Position (Token)  -->
|Token:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|`{Position}`|
|:----|:----|
|Replaced With:|The Remote Control's position, formatted as `{X:.. Y:.. Z:..}`|
|Requires:|Live NPC data, and the calling code must actually pass a position into the replace call - a few call sites don't, and `{Position}` silently resolves to `{X:0 Y:0 Z:0}` there instead of failing loudly|

<!--CustomStrings Key (Token)  -->
|Token:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|`{<CustomStringsKey>}`|
|:----|:----|
|Replaced With:|The value of a `[CustomStrings:]` key. The token IS the key name - if you set `[SetCustomStrings:true]` + `[CustomStrings:EncounterType,AllianceBase]` on an Action, the token `{EncounterType}` (not a literal `{CustomStringsKey}`) resolves to `AllianceBase` anywhere after that point.|
|Requires:|Live NPC data, and the key must have actually been set earlier in the same NPC's chain before this point runs|

<!--CustomCounters Key (Token)  -->
|Token:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|`{<CustomCountersKey>}`|
|:----|:----|
|Replaced With:|The value of a `[CustomCountersVariables:]` key, set the same way as `[CustomStrings:]` above via `[SetCustomCountersVariables:true]` + `[CustomCountersVariables:Key,Value]` on an Action. Same rule as `{<CustomStringsKey>}` above - the token IS the key name, not a literal `{CustomCountersKey}`.|
|Requires:|Live NPC data, and the key must have actually been set earlier in the same NPC's chain before this point runs|

<!--Sandbox Variable (Token)  -->
|Token:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|`{<SandboxVariableName>}`|
|:----|:----|
|Replaced With:|The value of any Space Engineers sandbox variable (`MyAPIUtilities.Static.Variables`) - not MES-specific, this is the engine's own session-wide key/value store, so it also picks up variables set by Programmable Block scripts or other mods. The token IS the variable's name - e.g. after `[SetSandboxBooleansTrue:BaseAlerted]`, `{BaseAlerted}` resolves to `True`.|
|Requires:|Works in **both** RivalAI Behavior and MES Events, since sandbox variables are global session state rather than per-NPC data|

`[Chat:]`/`[ChatData:]` message text has its own, partially overlapping token set (`{PlayerName}`, `{AntennaName}`, `{GridName}`, `{GPS}`, `{PlayerRelation}`, plus everything above) - see the **Chat** page for details on those.

# Other Token-Aware Fields

A few fields resolve tokens the same way. `[SpawnGroups:Value]` on a Spawner is already documented on the **Spawn** page - it resolves the full token set when declared on a `[RivalAI Action]`. The tag-broadcast fields (`[ManuallyActivatedTriggerTags:]`, `[EnableTriggerTags:]`, `[ToggleEventTags:]`, and the rest) are documented on the **Tags** page, since which of them are token-aware ties directly into how the Tag system itself works.

<!--SpawnData (Token Support)  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|SpawnData|
|:----|:----|
|Tag Format:|`[SpawnData:Value]`|
|Description:|The MES Event equivalent of `[Spawner:]` on a RivalAI Action - the top-level tag that attaches a spawn configuration. (`[SpawnGroups:Value]` is a separate, inner field inside that spawn configuration, present the same way on both sides.) Only `{Faction}` resolves here, via a separate hardcoded replace fed from the paired, index-aligned `[SpawnFactionTags:Value]` list - see the **Event Action** page for the full Spawner setup.|
|Declared On:|`[MES Event Action]`|
|Token Support:|`{Faction}` only|

<!--StoreProfiles  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|StoreProfiles|
|:----|:----|
|Tag Format:|`[StoreProfiles:Value]`|
|Description:|Specifies one or more Store Profile SubtypeIds to apply to this Action's grid.|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|
|Declared On:|`[RivalAI Action]` / `[MES AI Action]`|
|Token Support:|Yes|

<!--PlanetWaypointProfile  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|PlanetWaypointProfile|
|:----|:----|
|Tag Format:|`[PlanetWaypointProfile:Value]`|
|Description:|Specifies the Waypoint Profile SubtypeId this Action should send the grid to.|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|No|
|Declared On:|`[RivalAI Action]` / `[MES AI Action]` only|
|Token Support:|Yes|

<!--Waypoint (Spawner)  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Waypoint (Spawner)|
|:----|:----|
|Tag Format:|`[Waypoint:Value]` (paired with `[UseWaypoint:true]`)|
|Description:|Specifies the Waypoint Profile SubtypeId a Spawner's spawn position should be calculated from. Declared on the shared `SpawnProfile` class, so it's usable from a Spawner on either a `[RivalAI Action]`/`[MES AI Action]` or a `[MES Event Action]` - both route through the same `BehaviorSpawnHelper` spawn code.|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|No|
|Declared On:|`[RivalAI Action]` / `[MES AI Action]` **or** `[MES Event Action]`|
|Token Support:|Only when declared on `[RivalAI Action]`/`[MES AI Action]`. On a `[MES Event Action]`, the Spawner has no parent grid/Behavior to pull NPC data from, so only sandbox-variable tokens resolve - same dual-context gotcha as `[ToggleEventTags:]`/`[ResetEventCooldownTags:]` on the **Tags** page.|

<!--TrueSandboxBooleans  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|TrueSandboxBooleans|
|:----|:----|
|Tag Format:|`[TrueSandboxBooleans:Value]`|
|Description:|On a Condition Profile, checks the named sandbox variable(s) resolve to `true`.|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|
|Declared On:|`[RivalAI Condition]` / `[MES AI Condition]`|
|Token Support:|`{Faction}`, `{SpawnGroupName}` only - sourced directly from the Behavior via its own hand-rolled replace, not the general token list above|

<!--FalseSandboxBooleans  -->
|Tag:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|FalseSandboxBooleans|
|:----|:----|
|Tag Format:|`[FalseSandboxBooleans:Value]`|
|Description:|On a Condition Profile, checks the named sandbox variable(s) resolve to `false`.|
|Allowed Values:|Any name string excluding `:`, `[`, `]`|
|Multiple Tag Allowed:|Yes|
|Declared On:|`[RivalAI Condition]` / `[MES AI Condition]`|
|Token Support:|None. Despite sitting right next to `TrueSandboxBooleans` above, this one has no `{Faction}`/`{SpawnGroupName}` replace at all - the value goes straight to the variable lookup literally. Don't assume the two are symmetric.|

# Common Pitfalls

**Tokens in `[Triggers:]`, `[Actions:]`, `[Conditions:]`, `[TriggerGroups:]`, `[ManipulationProfiles:]`, `[LootProfiles:]`, `[ContainerTypes:]`, and similar SubtypeId references never resolve, in any context.** There's no substitution mechanism for these tags at all - a token in one of them is dead on arrival regardless of where it's declared. If you need runtime-variable behavior on one of these, use a tag-based broadcast (see the **Tags** page) that points at multiple statically-named profiles instead of trying to build one dynamic profile name.

***

**A few fields exist on both a RivalAI-side profile and an MES Event profile, with the same name but different token behavior.** `[ToggleEventTags:]`/`[ToggleEventIds:]`/`[ResetEventCooldownTags:]`/`[ResetEventCooldownIds:]` are the clearest example - see the **Tags** page for the full rundown. If a token isn't resolving where you expect it to, check which profile type the field is actually declared on.
