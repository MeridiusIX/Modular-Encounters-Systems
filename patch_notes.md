# Update 2.74.00

* New MES companion mod: [Suppress Vanilla Planetary Installations](https://steamcommunity.com/sharedfiles/filedetails/?id=3796756949).
* Added big improvements & fixes to zone handling - thanks to Jasper for the PR! MES previously did not implement several of the options listed on its wiki and zones were generally very buggy. This PR means MES now supports:

  * UseAllowedFactions & AllowedFactions to restrict spawns in zones to include only SpawnGroups associated with the specified factions.
  * UseRestrictedFactions & RestrictedFactions to restrict spawns in zones from including SpawnGroups associated with the specified factions.
  * UseAllowedModIDs & AllowedModIDs to restrict spawns in zones to include all SpawnGroups in specific Steam workshop mods. (supersedes restrictions based on faction)
  * UseRestrictedModIDs & RestrictedModIDs to restrict spawns in zones to exclude all SpawnGroups specific Steam workshop mods. (supersedes restrictions based on faction)
  * UseAllowedSpawnGroups & AllowedSpawnGroups to set spawns in zones to include specified SpawnGroups. (supersedes restrictions based on mod id and faction)
  * UseRestrictedSpawnGroups & RestrictedSpawnGroups to set spawns in zones to exclude specified SpawnGroups. (supersedes restrictions based on mod id and faction)
  * NoSpawnZone:true to prevent any spawns in a zone. (supersedes all other restrictions)
  * Added ability to define more than a single Zone Coordinates and Radius via [CoordinateRadiusPairs:{X:0 Y:0 Z:0},double].
  * Added Action [ChangeZoneOnlyByName:bool] - allows for the changing of a zone's characteristics without the encounter physically being located within the zone.
  * Added Event Actions [ChangeZoneByName:bool], [ZoneRadiusChangeTypes:validvariable,validvariable,validvariable,etc.] and [ZoneRadiusChangeAmounts:double,double,double,etc.]
  * Fixed multiple issues with the ChangeZoneRadius Actions.
* Added SpawnCondition [UseRandomCustomFaction:bool] to complete the set.
* Added Actions [SetSandboxStrings:bool] & [SandboxStrings:variablename,value].
* Added Actions [SetCustomVector3Ds:bool] & [CustomVector3Ds:variablename,{X:0 Y:0 Z:0}].
* Added Action [ChangePlayerCreditsAmountCounter:countername].
* Added Action [ProcessStaticEncountersLocationVariable:customVector3Dvariablename].
* Added Action [TeleportPlayerCoordsVariabl:customVector3Dvariablename].
* Added Action [CustomVector3DsFromVariable:variablename,Vector3Dvariablename].
* Added PlayerConditions [MinPlayerCreditBalanceCounter:countername] & [MaxPlayerCreditBalanceCounter:countername].
* Added support for variables to PlayerCondition [CheckReputationwithFaction:string].
* Added support for variables to Trigger Type ButtonPress' [ButtonPanelName:string].
* Added support for variable {SpawnGroupName} to SpawnConditions [SandboxVariables:string] and [FalseSandboxVariables:string]
* Added EventAction [DebugChatMessage:string].
* Added EventCondition [CheckOnSession:bool] - runs only once on game load.
* Added EventActions [SetSandboxStrings:bool] & [SandboxStrings:variablename,value].
* Added EventActions [SetSandboxVector3Ds:bool] & [SandboxVector3Ds:variablename,{X:0 Y:0 Z:0}].
* Added support for counter-type variables to DebugMessage.
* Improved parsing of information defined in base game SpawnGroups. This explicitly will only affect vanilla SpawnGroups.
* Fix for space random encounter SpawnGroups that used the base game Factions-syntax to define the faction always spawning as SPRT.

enenra & CptArthur
