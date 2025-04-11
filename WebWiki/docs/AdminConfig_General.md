#AdminConfig_General.md

You can find the General Settings Configuration File in `MySaveWorldFolder\Storage\1521905890.sbm_ModularEncountersSpawner\Config-General.xml`. The settings you can modify are listed below:

|Setting:|EnableLegacySpaceCargoShipDetection|
|:----|:----|
|XML:|`<EnableLegacySpaceCargoShipDetection>Value</EnableLegacySpaceCargoShipDetection>`|
|Chat Command:|`/MES.Settings.General.EnableLegacySpaceCargoShipDetection.Value`|
|Description:|This setting allows you to specify if older Cargo Ship mods that do not contain Keens new `<IsCargoShip>` tag should still be considered for spawning by the Spawner. Default value is `true`. `Value` can be replaced with `true` or `false`|

|Setting:|UseModIdSelectionForSpawning|
|:----|:----|
|XML:|`<UseModIdSelectionForSpawning>Value</UseModIdSelectionForSpawning>`|
|Chat Command:|`/MES.Settings.General.UseModIdSelectionForSpawning.Value`|
|Description:|This setting allows you to specify if the spawner should sort all SpawnGroups by mod ID first, choose a random mod ID, and then spawn a random SpawnGroup from that mod ID. This is useful if you have mods loaded with lots of SpawnGroups that may outweigh spawning for other encounter mods. Default value is `true`. `Value` can be replaced with `true` or `false`|

|Setting:|UseWeightedModIdSelection|
|:----|:----|
|XML:|`<UseWeightedModIdSelection>Value</UseWeightedModIdSelection>`|
|Chat Command:|`/MES.Settings.General.UseWeightedModIdSelection.Value`|
|Description:|This setting allows you to specify if the encounter mods with more encounters should be weighed higher if `UseModIdSelectionForSpawning` is enabled. Default value is `true`. `Value` can be replaced with `true` or `false`|

|Setting:|LowWeightModIdSpawnGroups|
|:----|:----|
|XML:|`<LowWeightModIdSpawnGroups>Value</LowWeightModIdSpawnGroups>`|
|Chat Command:|`/MES.Settings.General.LowWeightModIdSpawnGroups.Value`|
|Description:|This setting allows you to specify how many SpawnGroups a mod must have (or less) to be considered `Low` weighted, if `UseWeightedModIdSelection` is enabled. `Value` can be replaced with any Integer number (eg: `1`, `2`, `3`)|

|Setting:|LowWeightModIdModifier|
|:----|:----|
|XML:|`<LowWeightModIdModifier>Value</LowWeightModIdModifier>`|
|Chat Command:|`/MES.Settings.General.LowWeightModIdModifier.Value`|
|Description:|This setting allows you to specify how many points a mod ID is worth if its considered `Low` weight, if `UseWeightedModIdSelection` is enabled. `Value` can be replaced with any Integer number (eg: `1`, `2`, `3`)|

|Setting:|MediumWeightModIdSpawnGroups|
|:----|:----|
|XML:|`<MediumWeightModIdSpawnGroups>Value</MediumWeightModIdSpawnGroups>`|
|Chat Command:|`/MES.Settings.General.MediumWeightModIdSpawnGroups.Value`|
|Description:|This setting allows you to specify how many SpawnGroups a mod must have (between LowWeightModIdSpawnGroups value and this value) to be considered `Medium` weighted, if `UseWeightedModIdSelection` is enabled. `Value` can be replaced with any Integer number (eg: `1`, `2`, `3`)|

|Setting:|MediumWeightModIdModifier|
|:----|:----|
|XML:|`<MediumWeightModIdModifier>Value</MediumWeightModIdModifier>`|
|Chat Command:|`/MES.Settings.General.MediumWeightModIdModifier.Value`|
|Description:|This setting allows you to specify how many points a mod ID is worth if its considered `Medium` weight, if `UseWeightedModIdSelection` is enabled. `Value` can be replaced with any Integer number (eg: `1`, `2`, `3`)|

|Setting:|HighWeightModIdSpawnGroups|
|:----|:----|
|XML:|`<HighWeightModIdSpawnGroups>Value</HighWeightModIdSpawnGroups>`|
|Chat Command:|`/MES.Settings.General.HighWeightModIdSpawnGroups.Value`|
|Description:|This setting allows you to specify how many SpawnGroups a mod must have (or higher) to be considered `High` weighted, if `UseWeightedModIdSelection` is enabled. `Value` can be replaced with any Integer number (eg: `1`, `2`, `3`)|

|Setting:|HighWeightModIdModifier|
|:----|:----|
|XML:|`<HighWeightModIdModifier>Value</HighWeightModIdModifier>`|
|Chat Command:|`/MES.Settings.General.HighWeightModIdModifier.Value`|
|Description:|This setting allows you to specify how many points a mod ID is worth if its considered `High` weight, if `UseWeightedModIdSelection` is enabled. `Value` can be replaced with any Integer number (eg: `1`, `2`, `3`)|

|Setting:|UseMaxNpcGrids|
|:----|:----|
|XML:|`<UseMaxNpcGrids>Value</UseMaxNpcGrids>`|
|Chat Command:|`/MES.Settings.General.UseMaxNpcGrids.Value`|
|Description:|This setting allows you to specify whether or not a global limit on NPC grids in the world should be used. `Value` can be replaced with `true` or `false`|

|Setting:|UseGlobalEventsTimers|
|:----|:----|
|XML:|`<UseGlobalEventsTimers>Value</UseGlobalEventsTimers>`|
|Chat Command:|`/MES.Settings.General.UseGlobalEventsTimers.Value`|
|Description:|This setting allows you to specify whether or not the mod should use the spawner timings from Global Events Definitions that are found on start-up. `Value` can be replaced with `true` or `false`|

|Setting:|IgnorePlanetWhitelists|
|:----|:----|
|XML:|`<IgnorePlanetWhitelists>Value</IgnorePlanetWhitelists>`|
|Chat Command:|`/MES.Settings.General.IgnorePlanetWhitelists.Value`|
|Description:|This setting allows you to specify if the spawner should ignore Planet Whitelists found in spawngroups. `Value` can be replaced with `true` or `false`|

|Setting:|IgnorePlanetBlacklists|
|:----|:----|
|XML:|`<IgnorePlanetBlacklists>Value</IgnorePlanetBlacklists>`|
|Chat Command:|`/MES.Settings.General.IgnorePlanetBlacklists.Value`|
|Description:|This setting allows you to specify if the spawner should ignore Planet Blacklists found in spawngroups. `Value` can be replaced with `true` or `false`|

|Setting:|ThreatRefreshTimerMinimum|
|:----|:----|
|XML:|`<ThreatRefreshTimerMinimum>Value</ThreatRefreshTimerMinimum>`|
|Chat Command:|`/MES.Settings.General.ThreatRefreshTimerMinimum.Value`|
|Description:|This setting specifies the minimum time a that grids threat scores are refreshed. The refresh doesn't happen at this interval all the time if nothing has spawned and requested a threat score scan. `Value` can be replaced with any integer (Eg: `5`, `10`, `42`, etc)|

|Setting:|ThreatReductionHandicap|
|:----|:----|
|XML:|`<ThreatReductionHandicap>Value</ThreatReductionHandicap>`|
|Chat Command:|`/MES.Settings.General.ThreatReductionHandicap.Value`|
|Description:|This setting allows you to set a handicap that is subtracted from all threat scores. You can also provide a negative value for added challenge. `Value` can be replaced with any integer (Eg: `5`, `-10`, `42`, etc)|

|Setting:|MaxGlobalNpcGrids|
|:----|:----|
|XML:|`<MaxGlobalNpcGrids>Value</MaxGlobalNpcGrids>`|
|Chat Command:|`/MES.Settings.General.MaxGlobalNpcGrids.Value`|
|Description:|If `UseMaxNpcGrids` is set to `true`, then this setting specifies how many NPCs are allow to exist in the world at once. `Value` can be replaced with any integer (Eg: `5`, `10`, `42`, etc)|

|Setting:|PlayerWatcherTimerTrigger|
|:----|:----|
|XML:|`<PlayerWatcherTimerTrigger>Value</PlayerWatcherTimerTrigger>`|
|Chat Command:|`/MES.Settings.General.PlayerWatcherTimerTrigger.Value`|
|Description:|This setting specifies how often (in seconds) the spawner will check to see if it's time to spawn a new encounter near a player (the actual spawning times for encounters can be found in their respective configuration files). `Value` can be replaced with any integer (Eg: `5`, `10`, `42`, etc)|

|Setting:|NpcDistanceCheckTimerTrigger|
|:----|:----|
|XML:|`<NpcDistanceCheckTimerTrigger>Value</NpcDistanceCheckTimerTrigger>`|
|Chat Command:|`/MES.Settings.General.NpcDistanceCheckTimerTrigger.Value`|
|Description:|This setting specifies how often (in seconds) the mod will check an NPCs distance to the end of its travel path (for Space/Lunar/Planetary Cargo Ship Encounters). `Value` can be replaced with any integer (Eg: `5`, `10`, `42`, etc)|

|Setting:|NpcOwnershipCheckTimerTrigger|
|:----|:----|
|XML:|`<NpcOwnershipCheckTimerTrigger>Value</NpcOwnershipCheckTimerTrigger>`|
|Chat Command:|`/MES.Settings.General.NpcOwnershipCheckTimerTrigger.Value`|
|Description:|`Value` can be replaced with any integer (Eg: `5`, `10`, `42`, etc)|

|Setting:|NpcCleanupCheckTimerTrigger|
|:----|:----|
|XML:|`<NpcCleanupCheckTimerTrigger>Value</NpcCleanupCheckTimerTrigger>`|
|Chat Command:|`/MES.Settings.General.NpcCleanupCheckTimerTrigger.Value`|
|Description:|This setting specifies how often (in seconds) the mod will check an NPCs ownership to ensure players have not taken ownership of blocks. `Value` can be replaced with any integer (Eg: `5`, `10`, `42`, etc)|

|Setting:|NpcBlacklistCheckTimerTrigger|
|:----|:----|
|XML:|`<NpcBlacklistCheckTimerTrigger>Value</NpcBlacklistCheckTimerTrigger>`|
|Chat Command:|`/MES.Settings.General.NpcBlacklistCheckTimerTrigger.Value`|
|Description:|This setting specifies how often (in seconds) the mod will check each NPC against clean-up settings for its particular encounter type. `Value` can be replaced with any integer (Eg: `5`, `10`, `42`, etc)|

|Setting:|SpawnedVoxelCheckTimerTrigger|
|:----|:----|
|XML:|`<SpawnedVoxelCheckTimerTrigger>Value</SpawnedVoxelCheckTimerTrigger>`|
|Chat Command:|`/MES.Settings.General.SpawnedVoxelCheckTimerTrigger.Value`|
|Description:|This setting specifies how often (in seconds) the mod will check to see if voxels spawned with the mod have grids nearby (Voxels are deleted if no grids are found nearby at the distance specified in `SpawnedVoxelMinimumGridDistance`). `Value` can be replaced with any integer (Eg: `5`, `10`, `42`, etc)|

|Setting:|SpawnedVoxelMinimumGridDistance|
|:----|:----|
|XML:|`<SpawnedVoxelMinimumGridDistance>Value</SpawnedVoxelMinimumGridDistance>`|
|Chat Command:|`/MES.Settings.General.SpawnedVoxelMinimumGridDistance.Value`|
|Description:|This setting specifies the minimum distance that any grid must be from a mod-spawned voxel, otherwise it is deleted in clean-up. `Value` can be replaced with any number (Eg: `1000`, `5000`, `42000`, etc). Value should not be lower than `0`|

|Setting:|PlanetSpawnsDisableList|
|:----|:----|
|XML:|`<PlanetSpawnsDisableList>`<br />   `<string>Value1</string>`<br />   `<string>Value2</string>`<br />`</PlanetSpawnsDisableList>`|
|Chat Command (Add):|`/MES.Settings.General.PlanetSpawnsDisableList.Add.Value`|
|Chat Command (Remove):|`/MES.Settings.General.PlanetSpawnsDisableList.Remove.Value`|
|Description:|This setting allows you to specify one or more planet names that spawns of any kind should not take place on. To add more names to the list, simply create a new line between the `<PlanetSpawnsDisableList>` and `</PlanetSpawnsDisableList>` tags and enter the following `<string>Value</string>` - Replace `Value` with the exact name of the planet subtypeid you want to restrict.|

|Setting:|NpcGridNameBlacklist|
|:----|:----|
|XML:|`<NpcGridNameBlacklist>`<br />   `<string>Value1</string>`<br />   `<string>Value2</string>`<br />`</NpcGridNameBlacklist>`|
|Chat Command (Add):|`/MES.Settings.General.NpcGridNameBlacklist.Add.Value`|
|Chat Command (Remove):|`/MES.Settings.General.NpcGridNameBlacklist.Remove.Value`|
|Description:|This setting allows you to specify one or more grid names that will be automatically deleted if spawned with NPC ownership. To add more names to the list, simply create a new line between the `<NpcGridNameBlacklist>` and `</NpcGridNameBlacklist>` tags and enter the following `<string>Value</string>` - Replace `Value` with the exact name of the grid you want to BlackList.|

|Setting:|NpcSpawnGroupBlacklist|
|:----|:----|
|XML:|`<NpcSpawnGroupBlacklist>`<br />   `<string>Value1</string>`<br />   `<string>Value2</string>`<br />`</NpcSpawnGroupBlacklist>`|
|Chat Command (Add):|`/MES.Settings.General.NpcSpawnGroupBlacklist.Add.Value`|
|Chat Command (Remove):|`/MES.Settings.General.NpcSpawnGroupBlacklist.Remove.Value`|
|Description:|This setting allows you to specify one or more SpawnGroups (using their SubtypeIds) that will be removed from the mod spawning pool. To add more groups to the list, simply create a new line between the `<NpcSpawnGroupBlacklist>` and `</NpcSpawnGroupBlacklist>` tags and enter the following `<string>Value</string>` - Replace `Value` with the exact name of the SpawnGroup SubtypeId you want to BlackList.|

<!--
|Setting:|UseEconomyBuyingReputationIncrease|
|:----|:----|
|XML:|`<UseEconomyBuyingReputationIncrease>Value</UseEconomyBuyingReputationIncrease>`|
|Chat Command:|`/MES.Settings.General.UseEconomyBuyingReputationIncrease.Value`|
|Description:|This setting allows you to gain reputation with an NPC faction when buying from their store blocks. This only works on NPC grids spawned via MES (Keen Economy Stations with SafeZones do not get this feature). `Value` can be replaced with `true` or `false`|

|Setting:|EconomyBuyingReputationCostAmount|
|:----|:----|
|XML:|`<EconomyBuyingReputationCostAmount>Value</EconomyBuyingReputationCostAmount>`|
|Chat Command:|`/MES.Settings.General.EconomyBuyingReputationCostAmount.Value`|
|Description:|This setting allows you to specify how much you must spend at an NPC store before you receive 1 point of reputation increase. `Value` can be replaced with any number greater than `0`|
-->