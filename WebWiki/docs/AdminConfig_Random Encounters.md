#AdminConfig_Random Encounters.md

You can find the Random Encounters Settings Configuration File in `MySaveWorldFolder\Storage\1521905890.sbm_ModularEncountersSpawner\Config-RandomEncounters.xml`. The settings you can modify are listed below:

|Setting:|EnableSpawns|
|:----|:----|
|XML:|`<EnableSpawns>Value</EnableSpawns>`|
|Chat Command:|`/MES.Settings.RandomEncounters.EnableSpawns.Value`|
|Description:|This setting allows you to Enable or Disable All Encounters from this Spawn Type.|

|Setting:|PlayerSpawnCooldown|
|:----|:----|
|XML:|`<PlayerSpawnCooldown>Value</PlayerSpawnCooldown>`|
|Chat Command:|`/MES.Settings.RandomEncounters.PlayerSpawnCooldown.Value`|
|Description:|This setting allows you to specify the cooldown time (in seconds) after a Random Encounter appears before another one can appear for the player. `Value` can be any integer number (should not be `0` or lower)|

|Setting:|SpawnTimerTrigger|
|:----|:----|
|XML:|`<SpawnTimerTrigger>Value</SpawnTimerTrigger>`|
|Chat Command:|`/MES.Settings.RandomEncounters.SpawnTimerTrigger.Value`|
|Description:|This setting allows you to specify how often the script should measure the player's distance to see if they've travelled far enough to trigger a Random Encounter spawn. `Value` can be any integer number (should not be `0` or lower)|

|Setting:|PlayerTravelDistance|
|:----|:----|
|XML:|`<PlayerTravelDistance>Value</PlayerTravelDistance>`|
|Chat Command:|`/MES.Settings.RandomEncounters.PlayerTravelDistance.Value`|
|Description:|This settings allows you to specify the distance a player must travel in space before a Random Encounter will spawn. `Value` can be any number (should not be `0` or lower)|

|Setting:|MaxShipsPerArea|
|:----|:----|
|XML:|`<MaxShipsPerArea>Value</MaxShipsPerArea>`|
|Chat Command:|`/MES.Settings.RandomEncounters.MaxShipsPerArea.Value`|
|Description:|This setting allows you to specify if there can should be a maximum amount of Random Encounter grids near the player area. The `AreaSize` property below specifies the radius to check. `Value` can be `true` or `false`|

|Setting:|AreaSize|
|:----|:----|
|XML:|`<AreaSize>Value</AreaSize>`|
|Chat Command:|`/MES.Settings.RandomEncounters.AreaSize.Value`|
|Description:|This setting allows you to specify the area to check for existing Random Encounters if `MaxShipsPerArea` is set to `true`. `Value` can be any number (should not be `0` or lower)|

|Setting:|MinSpawnDistanceFromPlayer|
|:----|:----|
|XML:|`<MinSpawnDistanceFromPlayer>Value</MinSpawnDistanceFromPlayer>`|
|Chat Command:|`/MES.Settings.RandomEncounters.MinSpawnDistanceFromPlayer.Value`|
|Description:|This setting allows you to specify the minimum distance a Random Encounter will appear from a player position. The distance is calculated as a random value between `MinSpawnDistanceFromPlayer` and `MaxSpawnDistanceFromPlayer`. `Value` can be any number (should not be `0` or lower and should be lower than `MaxSpawnDistanceFromPlayer`)|

|Setting:|MaxSpawnDistanceFromPlayer|
|:----|:----|
|XML:|`<MaxSpawnDistanceFromPlayer>Value</MaxSpawnDistanceFromPlayer>`|
|Chat Command:|`/MES.Settings.RandomEncounters.MaxSpawnDistanceFromPlayer.Value`|
|Description:|This setting allows you to specify the minimum distance a Random Encounter will appear from a player position. The distance is calculated as a random value between `MinSpawnDistanceFromPlayer` and `MaxSpawnDistanceFromPlayer`. `Value` can be any number (should not be `0` or lower and should be higher than `MinSpawnDistanceFromPlayer`)|

|Setting:|MinDistanceFromOtherEntities|
|:----|:----|
|XML:|`<MinDistanceFromOtherEntities>Value</MinDistanceFromOtherEntities>`|
|Chat Command:|`/MES.Settings.RandomEncounters.MinDistanceFromOtherEntities.Value`|
|Description:|This setting allows you to specify the minimum distance from other entities (grids, players, etc). `Value` can be any number higher than `0`|

|Setting:|SpawnAttempts|
|:----|:----|
|XML:|`<SpawnAttempts>Value</SpawnAttempts>`|
|Chat Command:|`/MES.Settings.RandomEncounters.SpawnAttempts.Value`|
|Description:|This setting allows you to specify how many attempts the mod should make to find a suitable spot to spawn the Random Encounter. `Value` can take any integer number higher than `0`|

|Setting:|UseMaxSpawnGroupFrequency|
|:----|:----|
|XML:|`<UseMaxSpawnGroupFrequency>Value</UseMaxSpawnGroupFrequency>`|
|Chat Command:|`/MES.Settings.RandomEncounters.UseMaxSpawnGroupFrequency.Value`|
|Description:|This setting allows you to specify if the spawn group frequency should be set no higher than a provided value. The `MaxSpawnGroupFrequency` value would be the frequency used if the spawn group frequency is above that value. `Value` can be set to `true` or `false`|

|Setting:|MaxSpawnGroupFrequency|
|:----|:----|
|XML:|`<MaxSpawnGroupFrequency>Value</MaxSpawnGroupFrequency>`|
|Chat Command:|`/MES.Settings.RandomEncounters.MaxSpawnGroupFrequency.Value`|
|Description:|This setting allows you to specify the spawning frequency of Random Encounters if `UseMaxSpawnGroupFrequency` is set to `true`. `Value` can be replaced with any number (no lower than `0`).|

|Setting:|DespawnDistanceFromPlayer|
|:----|:----|
|XML:|`<DespawnDistanceFromPlayer>Value</DespawnDistanceFromPlayer>`|
|Chat Command:|`/MES.Settings.RandomEncounters.DespawnDistanceFromPlayer.Value`|
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
