#AdminConfig_Planetary Installations.md

You can find the Planetary Installations Settings Configuration File in `MySaveWorldFolder\Storage\1521905890.sbm_ModularEncountersSpawner\Config-PlanetaryInstallations.xml`. The settings you can modify are listed below:

|Setting:|EnableSpawns|
|:----|:----|
|XML:|`<EnableSpawns>Value</EnableSpawns>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.EnableSpawns.Value`|
|Description:|This setting allows you to Enable or Disable All Encounters from this Spawn Type.|

|Setting:|PlayerSpawnCooldown|
|:----|:----|
|XML:|`<PlayerSpawnCooldown>Value</PlayerSpawnCooldown>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.PlayerSpawnCooldown.Value`|
|Description:|This setting allows you to specify the cooldown time (in seconds) after a Planetary Installation appears before another one can appear for the player. `Value` can be any integer number (should not be `0` or lower)|

|Setting:|SpawnTimerTrigger|
|:----|:----|
|XML:|`<SpawnTimerTrigger>Value</SpawnTimerTrigger>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.SpawnTimerTrigger.Value`|
|Description:|This setting allows you to specify how often the script should measure the player's distance to see if they've travelled far enough to trigger a Planetary Installation spawn. `Value` can be any integer number (should not be `0` or lower)|

|Setting:|PlayerDistanceSpawnTrigger|
|:----|:----|
|XML:|`<PlayerDistanceSpawnTrigger>Value</PlayerDistanceSpawnTrigger>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.PlayerDistanceSpawnTrigger.Value`|
|Description:|This settings allows you to specify the distance a player must travel in space before a Planetary Installation will spawn. `Value` can be any number (should not be `0` or lower)|

|Setting:|MaxShipsPerArea|
|:----|:----|
|XML:|`<MaxShipsPerArea>Value</MaxShipsPerArea>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.MaxShipsPerArea.Value`|
|Description:|This setting allows you to specify if there can should be a maximum amount of Planetary Installation grids near the player area. The `AreaSize` property below specifies the radius to check. `Value` can be replaced with any integer number (no lower than `0`).|

|Setting:|AreaSize|
|:----|:----|
|XML:|`<AreaSize>Value</AreaSize>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.AreaSize.Value`|
|Description:|This setting allows you to specify the area to check for existing Planetary Installation if `MaxShipsPerArea` is set to `true`. `Value` can be any number (should not be `0` or lower)|

|Setting:|PlayerMaximumDistanceFromSurface|
|:----|:----|
|XML:|`<PlayerMaximumDistanceFromSurface>Value</PlayerMaximumDistanceFromSurface>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.PlayerMaximumDistanceFromSurface.Value`|
|Description:|This setting allows you to specify the maximum altitude from the surface the player must be for a Planetary Installation spawn event to trigger. `Value` can be any number higher than `0`|

|Setting:|MinimumSpawnDistanceFromPlayers|
|:----|:----|
|XML:|`<MinimumSpawnDistanceFromPlayers>Value</MinimumSpawnDistanceFromPlayers>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.MinimumSpawnDistanceFromPlayers.Value`|
|Description:|This setting allows you to specify the minimum distance from the player that the station(s) will appear. `Value` can be any number higher than `0` and lower than `MaximumSpawnDistanceFromPlayers`|

|Setting:|MaximumSpawnDistanceFromPlayers|
|:----|:----|
|XML:|`<MaximumSpawnDistanceFromPlayers>Value</MaximumSpawnDistanceFromPlayers>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.MaximumSpawnDistanceFromPlayers.Value`|
|Description:|This setting allows you to specify the maximum distance from the player that the station(s) will appear. `Value` can be any number higher than `0` and higher than `MinimumSpawnDistanceFromPlayers`|

|Setting:|AggressivePathCheck|
|:----|:----|
|XML:|`<AggressivePathCheck>Value</AggressivePathCheck>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.AggressivePathCheck.Value`|
|Description:|This setting allows you to specify if the mod should perform terrain checks in 8 directions from the player. If set to `false`, the mod will only perform checks in 4 directions, which may result in a lower chance of finding a suitable spot to spawn the station(s). `Value` can be `true` or `false`|

|Setting:|SearchPathIncrement|
|:----|:----|
|XML:|`<SearchPathIncrement>Value</SearchPathIncrement>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.SearchPathIncrement.Value`|
|Description:|This setting allows you to specify the distance to the next terrain surface check area if a previous check fails. `Value` can be any number higher than `0`.|

|Setting:|MinimumSpawnDistanceFromOtherGrids|
|:----|:----|
|XML:|`<MinimumSpawnDistanceFromOtherGrids>Value</MinimumSpawnDistanceFromOtherGrids>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.MinimumSpawnDistanceFromOtherGrids.Value`|
|Description:|This setting allows you to specify the minimum distance from other grids and players the station spawn coordinates must be. `Value` can be any number higher than `0`.|

|Setting:|MinimumTerrainVariance|
|:----|:----|
|XML:|`<MinimumTerrainVariance>Value</MinimumTerrainVariance>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.MinimumTerrainVariance.Value`|
|Description:|This setting allows you to specify the minimum terrain variance that is allowed when checking a potential area for spawn coordinates. `Value` can be any number lower than `0`.|

|Setting:|MaximumTerrainVariance|
|:----|:----|
|XML:|`<MaximumTerrainVariance>Value</MaximumTerrainVariance>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.MaximumTerrainVariance.Value`|
|Description:|This setting allows you to specify the maximum terrain variance that is allowed when checking a potential area for spawn coordinates. `Value` can be any number higher than `0`.|

|Setting:|AggressiveTerrainCheck|
|:----|:----|
|XML:|`<AggressiveTerrainCheck>Value</AggressiveTerrainCheck>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.AggressiveTerrainCheck.Value`|
|Description:|This setting allows you to specify if the mod should check a potenial spawning area in 8 directions to ensure the terrain is level (using `MinimumTerrainVariance` and `MaximumTerrainVariance`). If set to `false`, only 4 directions will be used, which could result in more uneven terrain being used to spawn stations. `Value` can be `true` or `false`.|

|Setting:|TerrainCheckIncrementDistance|
|:----|:----|
|XML:|`<TerrainCheckIncrementDistance>Value</TerrainCheckIncrementDistance>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.TerrainCheckIncrementDistance.Value`|
|Description:|This setting allows you to specify how far each directional step from the proposed spawn coords should be checked. `Value` can be any number higher than `0`|

|Setting:|SmallTerrainCheckDistance|
|:----|:----|
|XML:|`<SmallTerrainCheckDistance>Value</SmallTerrainCheckDistance>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.SmallTerrainCheckDistance.Value`|
|Description:|This setting allows you to specify how far from the proposed spawn coords should be checked for small stations. `Value` can be any number higher than `0`|

|Setting:|MediumSpawnChanceBaseValue|
|:----|:----|
|XML:|`<MediumSpawnChanceBaseValue>Value</MediumSpawnChanceBaseValue>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.MediumSpawnChanceBaseValue.Value`|
|Description:|This setting allows you to specify the base chance a medium station has of appearing instead of a small station. `Value` can be any integer number between `0` and `100`.|

|Setting:|MediumSpawnChanceIncrement|
|:----|:----|
|XML:|`<MediumSpawnChanceIncrement>Value</MediumSpawnChanceIncrement>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.MediumSpawnChanceIncrement.Value`|
|Description:|This setting allows you to specify how much the `MediumSpawnChanceBaseValue` increases for every small station that spawns. `Value` can be any integer number between `0` and `100`.|

|Setting:|MediumSpawnDistanceIncrement|
|:----|:----|
|XML:|`<MediumSpawnDistanceIncrement>Value</MediumSpawnDistanceIncrement>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.MediumSpawnDistanceIncrement.Value`|
|Description:|This setting allows you to specify the additional distance from players that is added to `MinimumSpawnDistanceFromPlayers` and `MaximumSpawnDistanceFromPlayers` when spawning a medium station. Value can be any number higher than `0`|

|Setting:|MediumTerrainCheckDistance|
|:----|:----|
|XML:|`<MediumTerrainCheckDistance>Value</MediumTerrainCheckDistance>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.MediumTerrainCheckDistance.Value`|
|Description:|This setting allows you to specify how far from the proposed spawn coords should be checked for medium stations. `Value` can be any number higher than `0`|

|Setting:|LargeSpawnChanceBaseValue|
|:----|:----|
|XML:|`<LargeSpawnChanceBaseValue>Value</LargeSpawnChanceBaseValue>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.LargeSpawnChanceBaseValue.Value`|
|Description:|This setting allows you to specify the base chance a large station has of appearing instead of a medium station. `Value` can be any integer number between `0` and `100`.|

|Setting:|LargeSpawnChanceIncrement|
|:----|:----|
|XML:|`<LargeSpawnChanceIncrement>Value</LargeSpawnChanceIncrement>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.LargeSpawnChanceIncrement.Value`|
|Description:|This setting allows you to specify how much the `LargeSpawnChanceBaseValue` increases for every medium station that spawns. `Value` can be any integer number between `0` and `100`.|

|Setting:|LargeSpawnDistanceIncrement|
|:----|:----|
|XML:|`<LargeSpawnDistanceIncrement>Value</LargeSpawnDistanceIncrement>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.LargeSpawnDistanceIncrement.Value`|
|Description:|This setting allows you to specify the additional distance from players that is added to `MinimumSpawnDistanceFromPlayers` and `MaximumSpawnDistanceFromPlayers` when spawning a large station. Value can be any number higher than `0`|

|Setting:|LargeTerrainCheckDistance|
|:----|:----|
|XML:|`<LargeTerrainCheckDistance>Value</LargeTerrainCheckDistance>`|
|Chat Command:|`/MES.Settings.PlanetaryInstallations.LargeTerrainCheckDistance.Value`|
|Description:|This setting allows you to specify how far from the proposed spawn coords should be checked for large stations. `Value` can be any number higher than `0`|  

|Setting:|UseMaxSpawnGroupFrequency|
|:----|:----|
|XML:|`<UseMaxSpawnGroupFrequency>Value</UseMaxSpawnGroupFrequency>`|
|Chat Command:|`/MES.Settings.RandomEncounters.UseMaxSpawnGroupFrequency.Value`|
|Description:|This setting allows you to specify if the spawn group frequency should be set no higher than a provided value. The `MaxSpawnGroupFrequency` value would be the frequency used if the spawn group frequency is above that value. `Value` can be set to `true` or `false`|

|Setting:|MaxSpawnGroupFrequency|
|:----|:----|
|XML:|`<MaxSpawnGroupFrequency>Value</MaxSpawnGroupFrequency>`|
|Chat Command:|`/MES.Settings.RandomEncounters.MaxSpawnGroupFrequency.Value`|
|Description:|This setting allows you to specify the spawning frequency of Planetary Installations if `UseMaxSpawnGroupFrequency` is set to `true`. `Value` can be replaced with any number (no lower than `0`).|

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
