#AdminConfig_AdminDebugOptions.md

These commands can be used to force spawn random or specific spawn groups, enable debug mode, get data on loaded spawngroups / active players / active NPCs / etc, and much more. These are chat commands only, so there are no accompanying XML configs.

Here is a list of command categories:

[**BehaviorDebug**](#BehaviorDebug)  
[**Debug**](#Debug)  
[**Info**](#Info)  
[**Spawn**](#Spawn)  
[**SpawnDebug**](#SpawnDebug)  

# BehaviorDebug

|Setting:|Enable or Disable Behavior Logging|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.BehaviorDebug.Value1.Value2`|
|Description:|This chat command allows you to enable various forms of logging for Behavior Related events. This can help you troubleshoot issues.|
|Allowed Value1:|`Action`<br />`AutoPilot`<br />`BehaviorMode`<br />`BehaviorSetup`<br />`BehaviorSpecific`<br />`Chat`<br />`Command`<br />`Condition`<br />`Collision`<br />`Dev`<br />`Error`<br />`GameLog` (enable this to write enabled events to game log)<br />`General`<br />`Owner`<br />`Settings`<br />`Spawn`<br />`Startup`<br />`TargetAcquisition`<br />`TargetEvaluation`<br />`Thrust`<br />`Trigger`<br />`Weapon`<br />`Target`|
|Allowed Value2:|`true`<br />`false`|

# Debug

|Setting:|Change Bool|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Debug.ChangeBool.Value1.Value2`|
|Description:|This chat command allows you to manually set or adjust the value of a Sandbox Boolean.|
|Allowed `Value1`|Name of Sandbox Boolean|
|Allowed `Value2`|Any Bool Value (`true` / `false`) you want to Set the Sandbox Boolean to.|

|Setting:|Change Counter|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Debug.ChangeCounter.Value1.Value2`|
|Description:|This chat command allows you to manually set or adjust the value of a Sandbox Counter.|
|Allowed `Value1`|Name of Sandbox Counter|
|Allowed `Value2`|Any Integer Value you want to Set the Sandbox Counter to.|

|Setting:|Clear All Timeouts|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Debug.ClearAllTimeouts`|
|Description:|This chat command will remove all Timeout Spawning Restrictions in the game world.|

|Setting:|Clear Ship Inventory|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Debug.ClearShipInventory`|
|Description:|This chat command will clear all inventory blocks of the grid the player is currently sitting on.|

|Setting:|Clear Static Encounters|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Debug.ClearStaticEncounters`|
|Description:|This chat command will clear all Static Encounters from the Spawner. This is useful for debugging in case you need to regenerate the static encounters at start up. It is usually recommended to Save/Reload after using this command.|

|Setting:|Clear Timeouts At Position|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Debug.ClearTimeoutsAtPosition`|
|Description:|This chat command will remove all Timeout Spawning Restrictions at the player's current position.|

|Setting:|Clear Unique Encounters|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Debug.ClearUniqueEncounters`|
|Description:|This chat command will clear any Unique Encounters that have spawned, allowing them to spawn again.|

|Setting:|Create Known Player Location|
|:----|:----|
|XML:|`N/A`|
|Chat Command 1:|`/MES.Debug.CreateKPL.FactionValue`|
|Chat Command 2:|`/MES.Debug.CreateKPL.FactionValue.RadiusValue`|
|Chat Command 3:|`/MES.Debug.CreateKPL.FactionValue.RadiusValue.DurationValue`|
|Chat Command 4:|`/MES.Debug.CreateKPL.FactionValue.RadiusValue.DurationValue.MaxEncounterValue`|
|Description:|This chat command allows you to create a Known Player Location at your current position.|
|Allowed Value for `FactionValue`:|Any Faction Tag|
|Allowed Value for `RadiusValue`:|Any Number Greater Than 0|
|Allowed Value for `DurationValue`:|Any Integer Greater Than 0|
|Allowed Value for `MaxEncounterValue`:|Any Integer Greater Than 0, or `-1` for Unused|

|Setting:|Create Planet|
|:----|:----|
|XML:|`N/A`|
|Chat Command 1:|`/MES.Debug.CreatePlanet.PlanetName.PlanetSize`|
|Description:|This chat command allows you to spawn a custom sized planet, at sizes smaller than 19000m or greater than 120000m.|
|Allowed Value for `PlanetName`:|Any Planet Name|
|Allowed Value for `PlanetSize`:|Any Number Greater Than 100|

|Setting:|Draw Paths|
|:----|:----|
|XML:|`N/A`|
|Chat Command 1:|`/MES.Debug.DrawPaths`|
|Description:|This chat command allows you to enable/disable (this command toggles) debug draw on behavior autopilot. This only works while you are offline or local host, it will not work on a dedicated server. While autopilot is active, several lines may be drawn from ships/drones using MES Behavior Autopilot.<br /><br /> - Green: Current Waypoint<br /> - Red: Collision<br /> - Orange: Evasion<br /> - Cyan: Offset Waypoint<br /> - Magenta: Planetary Pathing<br /> - Yellow: Weapon Prediction Waypoint<br /><br />If using a behavior that has water pathing, a Green path may be drawn showing the various water nodes it is currently following.|

|Setting:|Force Combat Phase|
|:----|:----|
|XML:|`N/A`|
|Chat Command 1:|`/MES.Debug.ForceCombatPhase`|
|Description:|This chat command allows you to immediately enable Combat Phase in the current world.|

|Setting:|Force Peace Phase|
|:----|:----|
|XML:|`N/A`|
|Chat Command 1:|`/MES.Debug.ForcePeacePhase`|
|Description:|This chat command allows you to immediately enable Peace Phase in the current world (stops the current Combat Phase).|

|Setting:|Process Prefabs|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Debug.ProcessPrefabs.Value`|
|Description:|This chat command will spawn all grids from a provided mod, and will collect data while running some tests on its performance. The results of this test are then printed to the user clipboard in a CSV format (can paste into spreadsheet apps such as excel, google sheets, etc). It is important that you only use this command on an empty world you only intend to use for this testing, because all grids are deleted between spawns of prefabs.|
|Allowed Value:|Partial or Full string of a Mod's Name (eg: `Corruption`)|  

|Setting:|Remove All Npcs|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Debug.RemoveAllNpcs`|
|Description:|This chat command will remove all grids that have NPC Ownership. Any grids with Player Ownership or No Ownership will be ignored.|

|Setting:|Reset Reputation|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Debug.ResetReputation.Value`|
|Description:|This chat command allows you to reset all players reputation with a specified faction.|
|Allowed Value:|Any NPC Faction Tag|

|Setting:|Reset Zones|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Debug.ResetZones`|
|Description:|This chat command allows you to reset all zones currently loaded in the game world and reload them using the profiles currently present in the mod loadout.|

|Setting:|Unlock Admin Blocks|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Debug.UnlockAdminBlocks`|
|Description:|This chat command allows you to unlock all MES Special Blocks (eg: AI Control Module, Inhibitors, etc). This only allows the player that entered the command to use the blocks, and only for the duration of the session.|

# Info

|Setting:|Get Active NPCs|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Info.GetActiveNpcs`|
|Description:|This chat command will gather a list of all Active NPC grids identified by the mod and save it to your clipboard.|

|Setting:|Get All Profiles|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Info.GetAllProfiles`|
|Description:|This chat command will gather a list of all MES Profiles and save it to your clipboard.|

|Setting:|Get Block Definitions|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Info.GetBlockDefinitions`|
|Description:|This chat command will gather a list of all currently loaded block definition data and save it to your clipboard.|

|Setting:|Get Block Mass Data|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Info.GetBlockMassData`|
|Description:|This chat command will gather a list of all Blocks in the game and how much they weigh.|

|Setting:|Get Colors From Grid|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Info.GetColorsFromGrid`|
|Description:|This chat command will gather a list of all colors from blocks on a particular grid and save it to your clipboard. You must be sitting in a seat of the grid you want to collect data from.|

|Setting:|Get Diagnostics|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Info.GetDiagnostics`|
|Description:|This chat command will gather a collection of information about the current session that may be related to the operation of the mod and save it to the Clipboard.|

|Setting:|Get Eligible Spawns At Position|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Info.GetEligibleSpawnsAtPosition`<br />`/MES.GESAP`|
|Description:|This chat command will gather a list of all Spawn Groups that are eligible to spawn at your position and saves it to your clipboard.|

|Setting:|Get Grid Behavior|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Info.GetGridBehavior`|
|Description:|This chat command will collect behavior information from a grid your player/spectator camera is pointing at and will return it to your Clipboard.|

|Setting:|Get Grid Data|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Info.GetGridData`|
|Description:|This chat command will collect data about a grid that was spawned as an NPC from MES. The info in this command is already present in the `/MES.Info.GetGridBehavior` command, so this is intended to be used if the grid has no behavior.|

|Setting:|Get Grid Matrix|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Info.GetGridMatrix`|
|Description:|This chat command will collect position information from a grid your player/spectator camera is pointing at and will return it to your Clipboard. The command will also pre-format some of this information into tags that can be used to configure Static Encounters.|

|Setting:|Get Item Mass Data|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Info.GetItemMassData`|
|Description:|This chat command will gather a list of all Items in the game and how much they weigh.|

|Setting:|Get Logging|
|:----|:----|
|XML:|`N/A`|
|Chat Command (Regular):|`/MES.Info.GetLogging.Value1.Value2`|
|Chat Command (SpawnDebug):|`/MES.IGLSD.Value2`|
|Chat Command (BehaviorDebug):|`/MES.IGLBD.Value2`|
|Description:|This chat command will copy logged data from a particular logging type and save it to your clipboard. `Value1` is replaced with `BehaviorDebug` or `SpawnDebug`. `Value2` is replaced with any of the logging types from the respective logging types in `Value1`. |

|Setting:|Get Players|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Info.GetPlayers`|
|Description:|This chat command will collect data on all players currently in the session and save it to your clipboard. |

|Setting:|Get Threat Score|
|:----|:----|
|XML:|`N/A`|
|Chat Command 1:|`/MES.Info.GetThreatScore`<br />`/MES.GTS`|
|Chat Command 2:|`/MES.Info.GetThreatScore.Value`<br />`/MES.GTS.Value`|
|Description:|This chat command will get the current Threat Score near your position and save it to your clipboard. By default, a range of `5000` meters is checked. You can provide a custom distance by replacing the `Value` text in Chat Command 2|

# Spawn

|Setting:|Spawn Space Cargo Ship|
|:----|:----|
|XML:|`N/A`|
|Chat Command 1:|`/MES.Spawn.SpaceCargoShip`<br />`/MES.SSCS`|
|Chat Command 2:|`/MES.Spawn.SpaceCargoShip.Value`<br />`/MES.SSCS.Value`|
|Description:|This chat command allows you to spawn a Space Cargo Ship near your player character. This spawn will obey the rules you have set for encounters to appear in that area. Chat Command 1 will spawn a random group from whatever groups are available. Chat Command 2 allows you to specify the Spawn Group you want to spawn, just replace `Value` with the SubtypeId of the Spawn Group you want to spawn.|

|Setting:|Spawn Random Encounter|
|:----|:----|
|XML:|`N/A`|
|Chat Command 1:|`/MES.Spawn.RandomEncounter`<br />`/MES.SRE`|
|Chat Command 2:|`/MES.Spawn.RandomEncounter.Value`<br />`/MES.SRE.Value`|
|Description:|This chat command allows you to spawn a Random Encounter near your player character. This spawn will obey the rules you have set for encounters to appear in that area. Chat Command 1 will spawn a random group from whatever groups are available. Chat Command 2 allows you to specify the Spawn Group you want to spawn, just replace `Value` with the SubtypeId of the Spawn Group you want to spawn.|

|Setting:|Spawn Boss Encounter|
|:----|:----|
|XML:|`N/A`|
|Chat Command 1:|`/MES.Spawn.BossEncounter`<br />`/MES.SBE`|
|Chat Command 2:|`/MES.Spawn.BossEncounter.Value`<br />`/MES.SBE.Value`|
|Description:|This chat command allows you to spawn a Boss Encounter near your player character. This spawn will obey the rules you have set for encounters to appear in that area. Chat Command 1 will spawn a random group from whatever groups are available. Chat Command 2 allows you to specify the Spawn Group you want to spawn, just replace `Value` with the SubtypeId of the Spawn Group you want to spawn.|

|Setting:|Spawn Planetary Cargo Ship|
|:----|:----|
|XML:|`N/A`|
|Chat Command 1:|`/MES.Spawn.PlanetaryCargoShip`<br />`/MES.SPCS`|
|Chat Command 2:|`/MES.Spawn.PlanetaryCargoShip.Value`<br />`/MES.SPCS.Value`|
|Description:|This chat command allows you to spawn a Planetary Cargo Ship near your player character. This spawn will obey the rules you have set for encounters to appear in that area. Chat Command 1 will spawn a random group from whatever groups are available. Chat Command 2 allows you to specify the Spawn Group you want to spawn, just replace `Value` with the SubtypeId of the Spawn Group you want to spawn.|

|Setting:|Spawn Planetary Installation|
|:----|:----|
|XML:|`N/A`|
|Chat Command 1:|`/MES.Spawn.PlanetaryInstallation`<br />`/MES.SPI`|
|Chat Command 2:|`/MES.Spawn.PlanetaryInstallation.Value`<br />`/MES.SPI.Value`|
|Description:|This chat command allows you to spawn a Planetary Installation near your player character. This spawn will obey the rules you have set for encounters to appear in that area. Chat Command 1 will spawn a random group from whatever groups are available. Chat Command 2 allows you to specify the Spawn Group you want to spawn, just replace `Value` with the SubtypeId of the Spawn Group you want to spawn.|

|Setting:|Spawn Drone Encounter|
|:----|:----|
|XML:|`N/A`|
|Chat Command 1:|`/MES.Spawn.DroneEncounter`<br />`/MES.SDE`|
|Chat Command 2:|`/MES.Spawn.DroneEncounter.Value`<br />`/MES.SDE.Value`|
|Description:|This chat command allows you to spawn a Drone Encounter near your player character. This spawn will obey the rules you have set for encounters to appear in that area. Chat Command 1 will spawn a random group from whatever groups are available. Chat Command 2 allows you to specify the Spawn Group you want to spawn, just replace `Value` with the SubtypeId of the Spawn Group you want to spawn.|

|Setting:|Spawn Prefab|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Spawn.Prefab.Value1`|
|Description:|This chat command allows you to spawn a Prefab near your player character. This spawn does not use SpawnGroup rules, it only spawns a grid. The spawned grid will also spawn with Player Ownership.|
|Allowed Value `Value1`|Any Prefab SubtypeId|

|Setting:|Spawn Prefab Station|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.Spawn.PrefabStation.Value1.Value2`|
|Description:|This chat command allows you to spawn a Prefab Station near the terrain of your player character. This spawn does not use SpawnGroup rules, it only spawns a grid. The spawned grid will also spawn with Player Ownership. Using `Value1`, you can specify a Height Offset from the surface, which allows you to test/debug which height is needed for setting up a Planetary Installation height offset.|
|Allowed Value `Value1`|Any Positive/Negative Integer|
|Allowed Value `Value2`|Any Prefab SubtypeId|

|Setting:|Spawn Creature|
|:----|:----|
|XML:|`N/A`|
|Chat Command 1:|`/MES.Spawn.Creature`<br />`/MES.SC`|
|Chat Command 2:|`/MES.Spawn.Creature.Value`<br />`/MES.SC.Value`|
|Description:|This chat command allows you to spawn a Creature/Bot near your player character. This spawn will obey the rules you have set for encounters to appear in that area. Chat Command 1 will spawn a random group from whatever groups are available. Chat Command 2 allows you to specify the Spawn Group you want to spawn, just replace `Value` with the SubtypeId of the Spawn Group you want to spawn.|

|Setting:|Activate Wave Spawner|
|:----|:----|
|XML:|`N/A`|
|Chat Command 1:|`/MES.Spawn.WaveSpawner.Space`|
|Chat Command 2:|`/MES.Spawn.WaveSpawner.Planet`|
|Chat Command 3:|`/MES.Spawn.WaveSpawner.Creature`|
|Description:|This chat command allows you to immediately start a Wave Spawning Event for all players in the current session, regardless of whether or not the specific Wave Spawner is enabled in the world. Depending on the chat command you use, you can start Wave Spawning events for Space Cargo Ships, Planetary Cargo Ships, or Creatures/Bots. These events will honor whatever Wave Spawner settings are currently in place in your config files.|

|Setting:|Force Spawn Timer|
|:----|:----|
|XML:|`N/A`|
|Chat Command 1:|`/MES.Spawn.ForceSpawnTimer.Value`|
|Description:|This chat command allows you to immediately finish the internal timer for your player and a particular spawning type. This allows you to quickly test / simulate a spawn as it would occur naturally, without having to wait as long. `Value` can be replaced with any of the following: <br /><br />`SpaceCargoShip`<br />`RandomEncounter`<br />`PlanetaryCargoShip`<br />`PlanetaryInstallation`<br />`BossEncounter`<br /> `DroneEncounter`<br />`Creature`|


# SpawnDebug

|Setting:|Enable or Disable Spawn Logging|
|:----|:----|
|XML:|`N/A`|
|Chat Command:|`/MES.SpawnDebug.Value1.Value2`|
|Description:|This chat command allows you to enable various forms of logging for Spawner Related events. This can help you troubleshoot issues.|
|Allowed Value1:|`API`<br />`BlockLogic`<br />`CleanUp`<br />`Dev`<br />`Entity`<br />`Error`<br />`GameLog`<br />`Manipulation`<br />`Pathing`<br />`PostSpawn`<br />`Settings`<br />`SpawnGroup`<br />`Spawning`<br />`SpawnRecord`<br />`Startup`<br />`Zone`|
|Allowed Value2:|`true`<br />`false`|
