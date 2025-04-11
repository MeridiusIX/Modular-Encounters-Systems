#AdminConfig_Boss Encounters.md

You can find the Boss Encounters Settings Configuration File in `MySaveWorldFolder\Storage\1521905890.sbm_ModularEncountersSpawner\Config-BossEncounters.xml`. The settings you can modify are listed below:

|Setting:|EnableSpawns|
|:----|:----|
|XML:|`<EnableSpawns>Value</EnableSpawns>`|
|Chat Command:|`/MES.Settings.BossEncounters.EnableSpawns.Value`|
|Description:|This setting allows you to Enable or Disable All Encounters from this Spawn Type.|

|Setting:|PlayerSpawnCooldown|
|:----|:----|
|XML:|`<PlayerSpawnCooldown>Value</PlayerSpawnCooldown>`|
|Chat Command:|`/MES.Settings.BossEncounters.PlayerSpawnCooldown.Value`|
|Description:|This setting allows you to specify the cooldown time (in seconds) after a Boss Encounter appears before another one can appear for the player. `Value` can be any integer number (should not be `0` or lower)|

|Setting:|SpawnTimerTrigger|
|:----|:----|
|XML:|`<SpawnTimerTrigger>Value</SpawnTimerTrigger>`|
|Chat Command:|`/MES.Settings.BossEncounters.SpawnTimerTrigger.Value`|
|Description:|This setting allows you to specify how often the script should measure the player's distance to see if they've travelled far enough to trigger a Random Encounter spawn. `Value` can be any integer number (should not be `0` or lower)|

|Setting:|SignalActiveTimer|
|:----|:----|
|XML:|`<SignalActiveTimer>Value</SignalActiveTimer>`|
|Chat Command:|`/MES.Settings.BossEncounters.SignalActiveTimer.Value`|
|Description:|This setting allows you to specify the amount of time (in seconds) that a Boss Encounter GPS Signal will stay active. `Value` can be any integer number (should not be lower than `0`)|

|Setting:|MaxShipsPerArea|
|:----|:----|
|XML:|`<MaxShipsPerArea>Value</MaxShipsPerArea>`|
|Chat Command:|`/MES.Settings.BossEncounters.MaxShipsPerArea.Value`|
|Description:|This setting allows you to specify if there can should be a maximum amount of Boss Encounter grids near the player area. The `AreaSize` property below specifies the radius to check. `Value` can be `true` or `false`|

|Setting:|AreaSize|
|:----|:----|
|XML:|`<AreaSize>Value</AreaSize>`|
|Chat Command:|`/MES.Settings.BossEncounters.AreaSize.Value`|
|Description:|This setting allows you to specify the area to check for existing Boss Encounters if `MaxShipsPerArea` is set to `true`. `Value` can be any number (should not be `0` or lower)|

|Setting:|TriggerDistance|
|:----|:----|
|XML:|`<TriggerDistance>Value</TriggerDistance>`|
|Chat Command:|`/MES.Settings.BossEncounters.TriggerDistance.Value`|
|Description:|This setting allows you to specify how far from the Boss Encounter GPS the player much be before it spawns the Encounter grid(s). `Value` can be any number higher than `0`|

|Setting:|PathCalculationAttempts|
|:----|:----|
|XML:|`<PathCalculationAttempts>Value</PathCalculationAttempts>`|
|Chat Command:|`/MES.Settings.BossEncounters.PathCalculationAttempts.Value`|
|Description:|This setting allows you to specify how many times the mod should attempt to spawn the encounter grid(s) if a valid path cannot be found. `Value` can be any integer number higher than `0`|

|Setting:|MinCoordsDistanceSpace|
|:----|:----|
|XML:|`<MinCoordsDistanceSpace>Value</MinCoordsDistanceSpace>`|
|Chat Command:|`/MES.Settings.BossEncounters.MinCoordsDistanceSpace.Value`|
|Description:|This setting allows you to specify the minimum distance from the player the Boss Encounter GPS will be created while in Space. `Value` can be any number (should be higher than `0` and lower than `MaxCoordsDistanceSpace`)|

|Setting:|MaxCoordsDistanceSpace|
|:----|:----|
|XML:|`<MaxCoordsDistanceSpace>Value</MaxCoordsDistanceSpace>`|
|Chat Command:|`/MES.Settings.BossEncounters.MaxCoordsDistanceSpace.Value`|
|Description:|This setting allows you to specify the maximum distance from the player the Boss Encounter GPS will be created while in Space. `Value` can be any number (should be higher than `0` and higher than `MinCoordsDistanceSpace`)|

|Setting:|MinCoordsDistancePlanet|
|:----|:----|
|XML:|`<MinCoordsDistancePlanet>Value</MinCoordsDistancePlanet>`|
|Chat Command:|`/MES.Settings.BossEncounters.MinCoordsDistancePlanet.Value`|
|Description:|This setting allows you to specify the minimum distance from the player the Boss Encounter GPS will be created while on a Planet. `Value` can be any number (should be higher than `0` and lower than `MaxCoordsDistancePlanet`)|

|Setting:|MaxCoordsDistancePlanet|
|:----|:----|
|XML:|`<MaxCoordsDistancePlanet>Value</MaxCoordsDistancePlanet>`|
|Chat Command:|`/MES.Settings.BossEncounters.MaxCoordsDistancePlanet.Value`|
|Description:|This setting allows you to specify the maximum distance from the player the Boss Encounter GPS will be created while on a Planet. `Value` can be any number (should be higher than `0` and higher than `MinCoordsDistancePlanet`)|

|Setting:|PlayersWithinDistance|
|:----|:----|
|XML:|`<PlayersWithinDistance>Value</PlayersWithinDistance>`|
|Chat Command:|`/MES.Settings.BossEncounters.PlayersWithinDistance.Value`|
|Description:|This setting allows you to specify the distance from where the Boss Encounter GPS is created that players will receive the GPS signal. `Value` can be any number higher than `0`|

|Setting:|MinPlanetAltitude|
|:----|:----|
|XML:|`<MinPlanetAltitude>Value</MinPlanetAltitude>`|
|Chat Command:|`/MES.Settings.BossEncounters.MinPlanetAltitude.Value`|
|Description:|This setting allows you to specify the minimum altitude that Boss Encounter GPS markers and grid(s) will appear from the planet surface (if encounter is Planetary). `Value` can be any number higher than `0`.|

|Setting:|MinSignalDistFromOtherEntities|
|:----|:----|
|XML:|`<MinSignalDistFromOtherEntities>Value</MinSignalDistFromOtherEntities>`|
|Chat Command:|`/MES.Settings.BossEncounters.MinSignalDistFromOtherEntities.Value`|
|Description:|This setting allows you to specify the minimum distance the Boss Encounter GPS signal will appear from other grid/character entities. `Value` can be set to any number higher than `0`|

|Setting:|MinSpawnDistFromCoords|
|:----|:----|
|XML:|`<MinSpawnDistFromCoords>Value</MinSpawnDistFromCoords>`|
|Chat Command:|`/MES.Settings.BossEncounters.MinSpawnDistFromCoords.Value`|
|Description:|This setting allows you to specify the minimum. distance from the Boss Encounter GPS signal that the Encounter grid(s) will appear. `Value` can be any number higher than `0` and lower than `MaxSpawnDistFromCoords`|

|Setting:|MaxSpawnDistFromCoords|
|:----|:----|
|XML:|`<MaxSpawnDistFromCoords>Value</MaxSpawnDistFromCoords>`|
|Chat Command:|`/MES.Settings.BossEncounters.MaxSpawnDistFromCoords.Value`|
|Description:|This setting allows you to specify the maximum. distance from the Boss Encounter GPS signal that the Encounter grid(s) will appear. `Value` can be any number higher than `0` and higher than `MinSpawnDistFromCoords`|

|Setting:|MinAirDensity|
|:----|:----|
|XML:|`<MinAirDensity>Value</MinAirDensity>`|
|Chat Command:|`/MES.Settings.BossEncounters.MinAirDensity.Value`|
|Description:|This setting allows you to specify the minimum air density that is required for a Boss Encounter Signal to appear (for planetary encounters). `Value` can be any number between `0` and `1`.|

|Setting:|UseMaxSpawnGroupFrequency|
|:----|:----|
|XML:|`<UseMaxSpawnGroupFrequency>Value</UseMaxSpawnGroupFrequency>`|
|Chat Command:|`/MES.Settings.BossEncounters.UseMaxSpawnGroupFrequency.Value`|
|Description:|This setting allows you to specify if the spawn group frequency should be set no higher than a provided value. The `MaxSpawnGroupFrequency` value would be the frequency used if the spawn group frequency is above that value. `Value` can be set to `true` or `false`|

|Setting:|MaxSpawnGroupFrequency|
|:----|:----|
|XML:|`<MaxSpawnGroupFrequency>Value</MaxSpawnGroupFrequency>`|
|Chat Command:|`/MES.Settings.BossEncounters.MaxSpawnGroupFrequency.Value`|
|Description:|This setting allows you to specify the spawning frequency of Boss Encounters if `UseMaxSpawnGroupFrequency` is set to `true`. `Value` can be replaced with any number (no lower than `0`).|

|Setting:|DespawnDistanceFromPlayer|
|:----|:----|
|XML:|`<DespawnDistanceFromPlayer>Value</DespawnDistanceFromPlayer>`|
|Chat Command:|`/MES.Settings.BossEncounters.DespawnDistanceFromPlayer.Value`|
|Description:|This setting allows you to specify the minimum distance players must be from the encounter before despawn is triggered. `Value` can be replaced with any number (no lower than `0`).|

|Setting:|SpawnTypeBlacklist|
|:----|:----|
|XML:|`<SpawnTypeBlacklist>Value</SpawnTypeBlacklist>`|
|Chat Command (Add):   |`/MES.Settings.Creatures.SpawnTypeBlacklist.Add.Value`|
|Chat Command (Remove):|`/MES.Settings.Creatures.SpawnTypeBlacklist.Remove.Value`|
|Description:|This setting allows you to blacklist certain spawngroups from appearing when this encounter type is used for spawning.|

|Setting:|SpawnTypePlanetBlacklist|
|:----|:----|
|XML:|`<SpawnTypePlanetBlacklist>Value</SpawnTypePlanetBlacklist>`|
|Chat Command (Add):   |`/MES.Settings.Creatures.SpawnTypePlanetBlacklist.Add.Value`|
|Chat Command (Remove):|`/MES.Settings.Creatures.SpawnTypePlanetBlacklist.Remove.Value`|
|Description:|This setting allows you to blacklist certain planets from being able to spawn this type of encounter.|
