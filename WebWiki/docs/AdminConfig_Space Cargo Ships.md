#AdminConfig_Space Cargo Ships.md

You can find the Space Cargo Ships Settings Configuration File in `MySaveWorldFolder\Storage\1521905890.sbm_ModularEncountersSpawner\Config-SpaceCargoShips.xml`. The settings you can modify are listed below:

|Setting:|EnableSpawns|
|:----|:----|
|XML:|`<EnableSpawns>Value</EnableSpawns>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.EnableSpawns.Value`|
|Description:|This setting allows you to Enable or Disable All Encounters from this Spawn Type.|

|Setting:|FirstSpawnTime|
|:----|:----|
|XML:|`<FirstSpawnTime>Value</FirstSpawnTime>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.FirstSpawnTime.Value`|
|Description:|This setting allows you to set how long (in seconds) after the game starts until the first Cargo Ship Spawn Event triggers. `Value` can be replaced with any integer number (no lower than `0`).|

|Setting:|MinSpawnTime|
|:----|:----|
|XML:|`<MinSpawnTime>Value</MinSpawnTime>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.MinSpawnTime.Value`|
|Description:|This setting allows you to set the minimum time (in seconds) until the next Cargo Ship Spawn Event triggers. The time trigger is a random number between `MinSpawnTime` and `MaxSpawnTime`. `Value` can be replaced with any integer number (no lower than `0` and should be lower than `MaxSpawnTime`).|

|Setting:|MaxSpawnTime|
|:----|:----|
|XML:|`<MaxSpawnTime>Value</MaxSpawnTime>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.MaxSpawnTime.Value`|
|Description:|This setting allows you to set the maximum time (in seconds) until the next Cargo Ship Spawn Event triggers. The time trigger is a random number between `MinSpawnTime` and `MaxSpawnTime`. `Value` can be replaced with any integer number (no lower than `0` and should be higher than `MinSpawnTime`).|

|Setting:|MaxShipsPerArea|
|:----|:----|
|XML:|`<MaxShipsPerArea>Value</MaxShipsPerArea>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.MaxShipsPerArea.Value`|
|Description:|This setting allows you to specify how many Space Cargo Ships can be present in a specified distance from the player. If there are more than the `Value` in this setting, then no other ships will spawn. The area size is determined in the `AreaSize` setting below. `Value` can be replaced with any integer number (no lower than `0`).|

|Setting:|AreaSize|
|:----|:----|
|XML:|`<AreaSize>Value</AreaSize>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.AreaSize.Value`|
|Description:|This setting allows you to specify the area radius when checking for existing Space Cargo Ships for `MaxShipsPerArea`. `Value` can be replaced with any number (no lower than `0`).|

|Setting:|MaxSpawnAttempts|
|:----|:----|
|XML:|`<MaxSpawnAttempts>Value</MaxSpawnAttempts>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.MaxSpawnAttempts.Value`|
|Description:|This setting allows you to specify how many times the spawner should attempt to find a valid random travel path for the Cargo Ship during the spawning phase. `Value` can be replaced with any integer number (no lower than `0`).|

|Setting:|MinPathDistanceFromPlayer|
|:----|:----|
|XML:|`<MinPathDistanceFromPlayer>Value</MinPathDistanceFromPlayer>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.MinPathDistanceFromPlayer.Value`|
|Description:|This setting allows you to specify the minimum distance the Cargo Ship travel path should pass the player position. The distance is a random number between `MinPathDistanceFromPlayer` and `MaxPathDistanceFromPlayer`. `Value` can be replaced with any integer number (no lower than `0` and should be lower than `MaxPathDistanceFromPlayer`).|

|Setting:|MaxPathDistanceFromPlayer|
|:----|:----|
|XML:|`<MaxPathDistanceFromPlayer>Value</MaxPathDistanceFromPlayer>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.MaxPathDistanceFromPlayer.Value`|
|Description:|This setting allows you to specify the maximum distance the Cargo Ship travel path should pass the player position. The distance is a random number between `MinPathDistanceFromPlayer` and `MaxPathDistanceFromPlayer`. `Value` can be replaced with any integer number (no lower than `0` and should be higher than `MinPathDistanceFromPlayer`).|

|Setting:|MinLunarSpawnHeight|
|:----|:----|
|XML:|`<MinLunarSpawnHeight>Value</MinLunarSpawnHeight>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.MinLunarSpawnHeight.Value`|
|Description:|This setting allows you to specify the minimum surface altitude the Cargo Ship travel path should be above the player. The distance is a random number between `MinLunarSpawnHeight` and `MaxLunarSpawnHeight`. `Value` can be replaced with any integer number (no lower than `0` and should be lower than `MaxLunarSpawnHeight`).|

|Setting:|MaxLunarSpawnHeight|
|:----|:----|
|XML:|`<MaxLunarSpawnHeight>Value</MaxLunarSpawnHeight>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.MaxLunarSpawnHeight.Value`|
|Description:|This setting allows you to specify the minimum surface altitude the Cargo Ship travel path should be above the player. The distance is a random number between `MinLunarSpawnHeight` and `MaxLunarSpawnHeight`. `Value` can be replaced with any integer number (no lower than `0` and should be higher than `MinLunarSpawnHeight`).|

|Setting:|MinSpawnDistFromEntities|
|:----|:----|
|XML:|`<MinSpawnDistFromEntities>Value</MinSpawnDistFromEntities>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.MinSpawnDistFromEntities.Value`|
|Description:|This setting allows you to specify the minimum distance the encounter should spawn from other grids and players. `Value` can be replaced with any number (no lower than `0`).|

|Setting:|MinPathDistance|
|:----|:----|
|XML:|`<MinPathDistance>Value</MinPathDistance>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.MinPathDistance.Value`|
|Description:|This setting allows you to specify the minimum path distance the cargo ship will travel before despawning. The distance is a random number between `MinPathDistance` and `MaxPathDistance`. `Value` can be replaced with any integer number (no lower than `0` and should be lower than `MaxPathDistance`).|

|Setting:|MaxPathDistance|
|:----|:----|
|XML:|`<MaxPathDistance>Value</MaxPathDistance>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.MaxPathDistance.Value`|
|Description:|This setting allows you to specify the maximum path distance the cargo ship will travel before despawning. The distance is a random number between `MinPathDistance` and `MaxPathDistance`. `Value` can be replaced with any integer number (no lower than `0` and should be lower than `MinPathDistance`).|

|Setting:|PathCheckStep|
|:----|:----|
|XML:|`<PathCheckStep>Value</PathCheckStep>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.PathCheckStep.Value`|
|Description:|This setting allows you to specify the distance increment that is used when the spawner checks the Cargo Ship travel path for obstructions / gravity / etc. `Value` can be replaced with any number (no lower than `0`).|

|Setting:|DespawnDistanceFromEndPath|
|:----|:----|
|XML:|`<DespawnDistanceFromEndPath>Value</DespawnDistanceFromEndPath>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.DespawnDistanceFromEndPath.Value`|
|Description:|This setting allows you to specify how far from the end of the Cargo Ship travel path the ship will initiate its despawn. `Value` can be replaced with any number (no lower than `0`).|

|Setting:|DespawnDistanceFromPlayer|
|:----|:----|
|XML:|`<DespawnDistanceFromPlayer>Value</DespawnDistanceFromPlayer>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.DespawnDistanceFromPlayer.Value`|
|Description:|This setting allows you to specify the minimum distance players must be from the encounter before despawn is triggered. `Value` can be replaced with any number (no lower than `0`).|

|Setting:|UseMinimumSpeed|
|:----|:----|
|XML:|`<UseMinimumSpeed>Value</UseMinimumSpeed>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.UseMinimumSpeed.Value`|
|Description:|This setting allows you to specify if cargo ships should move at a minimum speed at spawn. If the speed limit specified in the spawn group is lower than the `MinimumSpeed` value, then that value will be applied. `Value` can be set to `true` or `false`|

|Setting:|MinimumSpeed|
|:----|:----|
|XML:|`<MinimumSpeed>Value</MinimumSpeed>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.MinimumSpeed.Value`|
|Description:|This setting allows you to specify the minimum speed a Cargo Ship should be moving at spawn if `UseMinimumSpeed` is set to `true`. `Value` can be replaced with any number (no lower than `0`).|

|Setting:|UseSpeedOverride|
|:----|:----|
|XML:|`<UseSpeedOverride>Value</UseSpeedOverride>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.UseSpeedOverride.Value`|
|Description:|This setting allows you to specify if cargo ships should all use a specific speed. The `SpeedOverride` value would be the speed used. `Value` can be set to `true` or `false`|

|Setting:|SpeedOverride|
|:----|:----|
|XML:|`<SpeedOverride>Value</SpeedOverride>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.SpeedOverride.Value`|
|Description:|This setting allows you to specify the speed a Cargo Ship should be moving at spawn if `UseSpeedOverride` is set to `true`. `Value` can be replaced with any number (no lower than `0`).|

|Setting:|UseMaxSpawnGroupFrequency|
|:----|:----|
|XML:|`<UseMaxSpawnGroupFrequency>Value</UseMaxSpawnGroupFrequency>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.UseMaxSpawnGroupFrequency.Value`|
|Description:|This setting allows you to specify if the spawn group frequency should be set no higher than a provided value. The `MaxSpawnGroupFrequency` value would be the frequency used if the spawn group frequency is above that value. `Value` can be set to `true` or `false`|

|Setting:|MaxSpawnGroupFrequency|
|:----|:----|
|XML:|`<MaxSpawnGroupFrequency>Value</MaxSpawnGroupFrequency>`|
|Chat Command:|`/MES.Settings.SpaceCargoShips.MaxSpawnGroupFrequency.Value`|
|Description:|This setting allows you to specify the spawning frequency of Space Cargo Ships if `UseMaxSpawnGroupFrequency` is set to `true`. `Value` can be replaced with any number (no lower than `0`).|
