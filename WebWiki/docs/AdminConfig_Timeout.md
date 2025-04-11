#AdminConfig_Timeout.md

These settings can be found in each of the encounter types configuration files. The settings listed here control the spawning timeout system for the encounter type.

Timeouts occur if too many grids of a particular type (Space Cargo Ship, Random Encounter, etc) are spawned in an area within a short amount of time. If the limit is reached, then a cooldown timer is applied to the immediate area before any other spawns are allowed to occur. This helps mitigate larger groups of players working cooperatively being swamped with spawns if they are all in the same area.

For the chat commands, replace `EncounterType` with any of the types of encounter from the other configurations (eg: `SpaceCargoShips`, `RandomEncounters`, etc)

The settings you can modify are listed below:

|Setting:|UseTimeout|
|:----|:----|
|XML:|`<UseTimeout>Value</UseTimeout>`|
|Chat Command:|`/MES.Settings.EncounterType.UseTimeout.Value`|
|Description:|This setting allows you to specify if Timeout should be enabled for this encounter type. `Value` can be `true` or `false`|

|Setting:|TimeoutRadius|
|:----|:----|
|XML:|`<TimeoutRadius>Value</TimeoutRadius>`|
|Chat Command:|`/MES.Settings.EncounterType.TimeoutRadius.Value`|
|Description:|This setting allows you to specify the radius from a spawn that a timeout will cover. `Value` can be `0` or higher|

|Setting:|TimeoutSpawnLimit|
|:----|:----|
|XML:|`<TimeoutSpawnLimit>Value</TimeoutSpawnLimit>`|
|Chat Command:|`/MES.Settings.EncounterType.TimeoutSpawnLimit.Value`|
|Description:|This setting allows you to specify the amount of spawns that can occur in a timeout area before the cooldown is applied. `Value` can be `0` or higher|

|Setting:|TimeoutDuration|
|:----|:----|
|XML:|`<TimeoutDuration>Value</TimeoutDuration>`|
|Chat Command:|`/MES.Settings.EncounterType.TimeoutDuration.Value`|
|Description:|This setting allows you to specify the amount of time (in seconds) that spawns will be restricted in an area if the cooldown is triggered. `Value` can be `0` or higher|
