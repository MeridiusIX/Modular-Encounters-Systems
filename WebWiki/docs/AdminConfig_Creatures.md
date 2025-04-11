#AdminConfig_Creatures.md

You can find the Creatures Settings Configuration File in `MySaveWorldFolder\Storage\1521905890.sbm_ModularEncountersSpawner\Config-Creatures.xml`. The settings you can modify are listed below:

|Setting:|EnableSpawns|
|:----|:----|
|XML:|`<EnableSpawns>Value</EnableSpawns>`|
|Chat Command:|`/MES.Settings.Creatures.EnableSpawns.Value`|
|Description:|This setting allows you to Enable or Disable All Encounters from this Spawn Type.|

|Setting:|OverrideVanillaCreatureSpawns|
|:----|:----|
|XML:|`<OverrideVanillaCreatureSpawns>Value</OverrideVanillaCreatureSpawns>`|
|Chat Command:|`/MES.Settings.Creatures.OverrideVanillaCreatureSpawns.Value`|
|Description:|This setting allows you to override the vanilla creature spawner so MES is exclusively responsible for spawning creatures / bots. This option has the same effect as loading the Planet Creature Spawner mod.|

|Setting:|MinCreatureSpawnTime|
|:----|:----|
|XML:|`<MinCreatureSpawnTime>Value</MinCreatureSpawnTime>`|
|Chat Command:|`/MES.Settings.Creatures.MinCreatureSpawnTime.Value`|
|Description:|This setting allows you to set the minimum amount of time it takes for a creature spawn to happen near a player. Time is measured in seconds.|

|Setting:|MaxCreatureSpawnTime|
|:----|:----|
|XML:|`<MaxCreatureSpawnTime>Value</MaxCreatureSpawnTime>`|
|Chat Command:|`/MES.Settings.Creatures.MaxCreatureSpawnTime.Value`|
|Description:|This setting allows you to set the maximum amount of time it takes for a creature spawn to happen near a player. Time is measured in seconds.|

|Setting:|MaxPlayerAltitudeForSpawn|
|:----|:----|
|XML:|`<MaxPlayerAltitudeForSpawn>Value</MaxPlayerAltitudeForSpawn>`|
|Chat Command:|`/MES.Settings.Creatures.MaxPlayerAltitudeForSpawn.Value`|
|Description:|This setting allows you to set the maximum altitude from the planet surface that a player must be within in order to be eligible to spawn a creature / bot encounter.|

|Setting:|CoordsAttemptsPerCreature|
|:----|:----|
|XML:|`<CoordsAttemptsPerCreature>Value</CoordsAttemptsPerCreature>`|
|Chat Command:|`/MES.Settings.Creatures.CoordsAttemptsPerCreature.Value`|
|Description:|This setting allows you to set the maximum amount of attempts that coordinate generation for creature spawning will be attempted for areas where they may have difficulty spawning.|

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
