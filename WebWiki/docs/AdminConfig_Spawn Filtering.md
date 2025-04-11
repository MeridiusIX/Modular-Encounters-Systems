#AdminConfig_Spawn Filtering.md

The values in this section can be found in multiple config files, specifically:

 - **SpaceCargoShips**  
 - **RandomEncounters**  
 - **PlanetaryCargoShips**  
 - **PlanetaryInstallations**  
 - **BossEncounters**  
 - **DroneEncounters**  

These allow you to prevent certain SpawnGroups from appearing when they are spawned as a particular spawn type, or if the encounter is on a particular planet.

|Setting:|SpawnTypeBlacklist|
|:----|:----|
|XML:|`<SpawnTypeBlacklist>Value</SpawnTypeBlacklist>`|
|Chat Command (Add):   |`/MES.Settings.SpawnType.SpawnTypeBlacklist.Add.Value`|
|Chat Command (Remove):|`/MES.Settings.SpawnType.SpawnTypeBlacklist.Remove.Value`|
|Description:|This setting allows you to blacklist certain spawngroups or mod ids from appearing when this encounter type is used for spawning. Replace `SpawnType` with the type of spawn you want to control against (eg: `SpaceCargoShips`). Replace `Value` with the SpawnGroup subtypeId or mod Id.|

|Setting:|SpawnTypePlanetBlacklist|
|:----|:----|
|XML:|`<SpawnTypePlanetBlacklist>Value</SpawnTypePlanetBlacklist>`|
|Chat Command (Add):   |`/MES.Settings.SpawnType.SpawnTypePlanetBlacklist.Add.Value`|
|Chat Command (Remove):|`/MES.Settings.SpawnType.SpawnTypePlanetBlacklist.Remove.Value`|
|Description:|This setting allows you to blacklist certain planets from being able to spawn this type of encounter. Replace `SpawnType` with the type of spawn you want to control against (eg: `SpaceCargoShips`). Replace `Value` with the Planet subtypeId.|

|Setting:|PlanetSpawnFilters|
|:----|:----|
|Chat Command (Add):   |`/MES.Settings.SpawnType.PlanetSpawnFilters.Add.PlanetId.Value`|
|Chat Command (Remove):|`/MES.Settings.SpawnType.PlanetSpawnFilters.Remove.PlanetId.Value`|
|Description:|This setting allows you to blacklist certain encounters or entire mod IDs from spawning on certain planets. Replace `SpawnType` with the type of spawn you want to control against (eg: `SpaceCargoShips`). Replace `PlanetId` with the EntityId of the planet you want to control spawning on (you can get this with the `/MES.GESAP` chat command while standing on the planet). Replace `Value` with the SpawnGroup subtypeId or mod Id.|