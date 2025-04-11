#Wave-Spawners.md

The spawn types of `SpaceCargoShips`, `PlanetaryCargoShips`, and `Creatures` all have configuration options for Wave Spawning. When it is enabled, it will group players into clusters based on their distance from each other and after a special timer has reached the trigger time, it will execute several spawns of a type in the area of each cluster.

You can customize the timings, distances, and spawn types with the commands/configs below. Replace `SpawnType` with one of the spawntypes above matching the configuration you want to change it for.


|Setting:|EnableWaveSpawner|
|:----|:----|
|XML:|`<EnableWaveSpawner>Value</EnableWaveSpawner>`|
|Chat Command:|`/MES.Settings.SpawnType.EnableWaveSpawner.Value`|
|Description:|This setting allows you to specify if the Wave Spawner for Space Cargo Ships should be enabled. Default value is `false`. `Value` can be set to `true` or `false`|

|Setting:|UseSpecificRandomGroups|
|:----|:----|
|XML:|`<UseSpecificRandomGroups>`<br />   `<string>Value1</string>`<br />   `<string>Value2</string>`<br />`</UseSpecificRandomGroups>`|
|Chat Command (Add):|`/MES.Settings.SpawnType.UseSpecificRandomGroups.Add.Value`|
|Chat Command (Remove):|`/MES.Settings.SpawnType.UseSpecificRandomGroups.Remove.Value`|
|Description:|This setting allows you to specify one or more specific SpawnGroups that will only be used with the Space Cargo Ship Wave Spawner. To add more SpawnGroups to the list, simply create a new line between the `<UseSpecificRandomGroups>` and `</UseSpecificRandomGroups>` tags and enter the following `<string>Value</string>` - Replace `Value` with the SubtypeName of the SpawnGroup.

|Setting:|MinWaveSpawnTime|
|:----|:----|
|XML:|`<MinWaveSpawnTime>Value</MinWaveSpawnTime>`|
|Chat Command:|`/MES.Settings.SpawnType.MinWaveSpawnTime.Value`|
|Description:|This setting allows you to specify the Minimum Time between Wave Spawn Events (In Seconds). `Value` can be replaced with any Integer number (eg: `100`, `200`, `300`)|

|Setting:|MaxWaveSpawnTime|
|:----|:----|
|XML:|`<MaxWaveSpawnTime>Value</MaxWaveSpawnTime>`|
|Chat Command:|`/MES.Settings.SpawnType.MaxWaveSpawnTime.Value`|
|Description:|This setting allows you to specify the Maximum Time between Wave Spawn Events (In Seconds). `Value` can be replaced with any Integer number (eg: `100`, `200`, `300`)|

|Setting:|TotalSpawnEventsPerCluster|
|:----|:----|
|XML:|`<TotalSpawnEventsPerCluster>Value</TotalSpawnEventsPerCluster>`|
|Chat Command:|`/MES.Settings.SpawnType.TotalSpawnEventsPerCluster.Value`|
|Description:|This setting allows you to specify the number of SpawnGroups spawned at a Player Cluster during the Wave Spawn Event. `Value` can be replaced with any Integer number (eg: `1`, `5`, `10`)|

|Setting:|TimeBetweenWaveSpawns|
|:----|:----|
|XML:|`<TimeBetweenWaveSpawns>Value</TimeBetweenWaveSpawns>`|
|Chat Command:|`/MES.Settings.SpawnType.TimeBetweenWaveSpawns.Value`|
|Description:|This setting allows you to specify the time (in seconds) between SpawnGroups being spawned during Wave Spawn Events. `Value` can be replaced with any Integer number (eg: `1`, `5`, `10`)|

|Setting:|PlayerClusterDistance|
|:----|:----|
|XML:|`<PlayerClusterDistance>Value</PlayerClusterDistance>`|
|Chat Command:|`/MES.Settings.SpawnType.PlayerClusterDistance.Value`|
|Description:|This setting allows you to specify the max distance (in meters) that players within are considered a cluster (Waves spawn at clusters, not necessarily at individual players). `Value` can be replaced with any Integer number (eg: `5000`, `10000`, `15000`)|
