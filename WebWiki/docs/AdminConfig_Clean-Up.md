#AdminConfig_Clean-Up.md

These settings can be found in each of the encounter types configuration files. The settings listed here control the circumstances for when grids of that type should be removed from the game by the clean-up process.

For the chat commands, replace `EncounterType` with any of the types of encounter from the other configurations (eg: `SpaceCargoShips`, `RandomEncounters`, etc)

The settings you can modify are listed below:

|Setting:|UseCleanupSettings|
|:----|:----|
|XML:|`<UseCleanupSettings>Value</UseCleanupSettings>`|
|Chat Command:|`/MES.Settings.EncounterType.UseCleanupSettings.Value`|
|Description:|This setting allows you to specify if the clean-up process should include encounters from this configuration file type. `Value` can be `true` or `false`|

|Setting:|OnlyCleanNpcsFromMes|
|:----|:----|
|XML:|`<OnlyCleanNpcsFromMes>Value</OnlyCleanNpcsFromMes>`|
|Chat Command:|`/MES.Settings.EncounterType.OnlyCleanNpcsFromMes.Value`|
|Description:|This setting allows you to specify if the clean-up process should only target grids that were spawned by Modular Encounters Systems. `Value` can be `true` or `false`|

|Setting:|CleanupUseDistance|
|:----|:----|
|XML:|`<CleanupUseDistance>Value</CleanupUseDistance>`|
|Chat Command:|`/MES.Settings.EncounterType.CleanupUseDistance.Value`|
|Description:|This setting allows you to specify if the clean-up process for this encounter type should remove grids that are too far away from players. `Value` can be `true` or `false`|

|Setting:|CleanupUseTimer|
|:----|:----|
|XML:|`<CleanupUseTimer>Value</CleanupUseTimer>`|
|Chat Command:|`/MES.Settings.EncounterType.CleanupUseTimer.Value`|
|Description:|This setting allows you to specify if the clean-up process for this encounter type should remove grids after a certain amount of time. `Value` can be `true` or `false`|

|Setting:|CleanupUseBlockLimit|
|:----|:----|
|XML:|`<CleanupUseBlockLimit>Value</CleanupUseBlockLimit>`|
|Chat Command:|`/MES.Settings.EncounterType.CleanupUseBlockLimit.Value`|
|Description:|This setting allows you to specify if the clean-up process for this encounter type should remove grids that exceed a certain block count. `Value` can be `true` or `false`|

|Setting:|CleanupDistanceStartsTimer|
|:----|:----|
|XML:|`<CleanupDistanceStartsTimer>Value</CleanupDistanceStartsTimer>`|
|Chat Command:|`/MES.Settings.EncounterType.CleanupDistanceStartsTimer.Value`|
|Description:|This setting allows you to specify if the clean-up process for this encounter type should start the clean-up timer for the grid after a player is outside the clean-up distance. `CleanupUseDistance` and `CleanupUseTimer` must both be set to `true` for this feature to work. `Value` can be `true` or `false`|

|Setting:|CleanupResetTimerWithinDistance|
|:----|:----|
|XML:|`<CleanupResetTimerWithinDistance>Value</CleanupResetTimerWithinDistance>`|
|Chat Command:|`/MES.Settings.EncounterType.CleanupResetTimerWithinDistance.Value`|
|Description:|This setting allows you to specify if `CleanupDistanceStartsTimer` is set to `true` and the player is within the clean-up distance limit, then the clean-up timer for that grid will reset to 0. `CleanupDistanceStartsTimer` must be set to `true` for this feature to work. `Value` can be `true` or `false`|

|Setting:|CleanupDistanceTrigger|
|:----|:----|
|XML:|`<CleanupDistanceTrigger>Value</CleanupDistanceTrigger>`|
|Chat Command:|`/MES.Settings.EncounterType.CleanupDistanceTrigger.Value`|
|Description:|This setting allows you to specify the maximum distance from the nearest player before the grid is removed if `CleanupUseDistance` is set to `true`. `Value` can be any number higher than `0`.|

|Setting:|CleanupTimerTrigger|
|:----|:----|
|XML:|`<CleanupTimerTrigger>Value</CleanupTimerTrigger>`|
|Chat Command:|`/MES.Settings.EncounterType.CleanupTimerTrigger.Value`|
|Description:|This setting allows you to specify the time limit (in seconds) before a grid is removed if `CleanupUseTimer` is set to true. `Value` can be set to any integer number higher than `0`|

|Setting:|CleanupBlockLimitTrigger|
|:----|:----|
|XML:|`<CleanupBlockLimitTrigger>Value</CleanupBlockLimitTrigger>`|
|Chat Command:|`/MES.Settings.EncounterType.CleanupBlockLimitTrigger.Value`|
|Description:|This setting allows you to specify the maximum amount of blocks a grid is allowed to have. Grids with a higher block count will be removed if `CleanupUseBlockLimit` is set to `true`. `Value` can be set to any integer number higher than `0`|

|Setting:|CleanupIncludeUnowned|
|:----|:----|
|XML:|`<CleanupIncludeUnowned>Value</CleanupIncludeUnowned>`|
|Chat Command:|`/MES.Settings.EncounterType.CleanupIncludeUnowned.Value`|
|Description:|This setting allows you to specify if unowned grids spawned by the spawner should also be included in the clean-up process. `Value` can be set to `true` or `false`.|

|Setting:|CleanupUnpoweredOverride|
|:----|:----|
|XML:|`<CleanupUnpoweredOverride>Value</CleanupUnpoweredOverride>`|
|Chat Command:|`/MES.Settings.EncounterType.CleanupUnpoweredOverride.Value`|
|Description:|This setting allows you to specify if different Distance and Timer values should be used if the target grid is unpowered. `Value` can be set to `true` or `false`.|

|Setting:|CleanupUnpoweredDistanceTrigger|
|:----|:----|
|XML:|`<CleanupUnpoweredDistanceTrigger>Value</CleanupUnpoweredDistanceTrigger>`|
|Chat Command:|`/MES.Settings.EncounterType.CleanupUnpoweredDistanceTrigger.Value`|
|Description:|This setting allows you to specify the maximum distance from the nearest player before the unpowered grid is removed if `CleanupUseDistance` and `CleanupUnpoweredOverride` is set to `true`. `Value` can be any number higher than `0`.|

|Setting:|CleanupUnpoweredTimerTrigger|
|:----|:----|
|XML:|`<CleanupUnpoweredTimerTrigger>Value</CleanupUnpoweredTimerTrigger>`|
|Chat Command:|`/MES.Settings.EncounterType.CleanupUnpoweredTimerTrigger.Value`|
|Description:|This setting allows you to specify the time limit (in seconds) before the unpowered grid is removed if `CleanupUseTimer` and `CleanupUnpoweredOverride` is set to true. `Value` can be set to any integer number higher than `0`|

|Setting:|UseBlockDisable|
|:----|:----|
|XML:|`<UseBlockDisable>Value</UseBlockDisable>`|
|Chat Command:|`/MES.Settings.EncounterType.UseBlockDisable.Value`|
|Description:|This setting allows you to specify if certain blocks on the NPC grid should be disabled after spawning. `Value` can be set to `true` or `false`|

|Setting:|DisableBlocksByType|
|:----|:----|
|XML:|`<DisableBlocksByType>`<br />   `<string>Value1</string>`<br />   `<string>Value2</string>`<br />`</DisableBlocksByType>`|
|Chat Command (Add):|`/MES.Settings.General.DisableBlocksByType.Add.Value`|
|Chat Command (Remove):|`/MES.Settings.General.DisableBlocksByType.Remove.Value`|
|Description:|This setting allows you to specify one or more Block Types (using their TypeId) that will be disabled on NPC grids when they spawn. To add more types to the list, simply create a new line between the `<DisableBlocksByType>` and `</DisableBlocksByType>` tags and enter the following `<string>Value</string>` - Replace `Value` with the exact name of the Block TypeId you want to Disable.|

|Setting:|DisableBlocksByDefinitionId|
|:----|:----|
|XML:|`<DisableBlocksByDefinitionId>`<br />   `<string>Value1</string>`<br />   `<string>Value2</string>`<br />`</DisableBlocksByDefinitionId>`|
|Chat Command (Add):|`/MES.Settings.General.DisableBlocksByDefinitionId.Add.Value`|
|Chat Command (Remove):|`/MES.Settings.General.DisableBlocksByDefinitionId.Remove.Value`|
|Description:|This setting allows you to specify one or more Block Types (using their MyDefinitionId string) that will be disabled on NPC grids when they spawn. To add more types to the list, simply create a new line between the `<DisableBlocksByDefinitionId>` and `</DisableBlocksByDefinitionId>` tags and enter the following `<string>Value</string>` - Replace `Value` with the exact name of the Block MyDefinitionId you want to Disable.|
