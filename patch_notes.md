# Update 2.75.00

* Added [#390](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/390): A set of new Autopilot Profile tags to adjust encounter flight behavior to look more like a plane's: [UseAircraftFlightModel:bool], [MaxAngleFromVelocity:double], [RollIntoTurnsMultiplier:float], [MaxBankAngle:double], [ReturnToLevelMultiplier:float], [ReturnToLevelSpeedCutOff:double], [MaxRotationStep:double]. All are documented on the wiki. Thanks to Villanor for the PR!
* Added [#389](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/389): New commands /MES.Debug.ResetStaticEncounters to reset all static encounters in the world, and /MES.Debug.ResetStaticEncounters.Debug to give an overview of which static encounters, and why, would be affected by the reset command command.
* Added [#386](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/386): Several new tags for Command Profiles: [MatchSenderReceiverParent:bool], [MatchReceiverChild:bool], [MatchReceiverEscort:bool], [MatchReceiverLeader:bool]. Thanks to Mike Dude for the PR!
* Added: Action & Event Action [GPSColorVariables:VariableName] to set the color of the created GPS marker.
* Added: Mission Profile tag [ContractSubtypeId:string] to make MES use a supplied contract definition instead of its default. This enables the use of custom contract header images.
* Improved: Centralized many NPC / player checks to a single location to improve consistency.
* Fixed [#404](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/404): Escorts that were set to relative dampening with their leader would not clear that setting when the leader was destroyed or the encounter was no longer an escort. Thanks to Mike Dude for the PR!
* Fixed [#402](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/402): When trying to reach a waypoint, in some cases encounters would accelerate in the incorrect direction. Thanks to Mike Dude for the PR!
* Fixed [#401](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/401): Seated players could not be targeted properly by encounters in some cases. Thanks to Jasper for the PR!
* Fixed [#400](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/400): SpawnCondition [MaxWaterCoverage:float] would incorrectly reduce the spawn frequency of an encounter. Thanks to Villanor for the PR!
* Fixed [#396](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/396): Static Encounters placed underwater would respawn on the water's surface after a reload. Thanks to Villanor for the PR!
* Fixed [#389](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/389): In some cases the MES command /MES.Debug.ResetThisStaticEncounter.{SpawnGroupName} would not properly respawn the static encounter. Thanks to Villanor for the PR!
* Fixed [#386](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/386): Several issues around identifying if a command was sent from a parent to a child / from a child to a parent encounter. Thanks to Mike Dude for the PR!
* Fixed [#383](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/383): Autopilot incorrectly calculating braking thrust, leading to encounters boosting into obstacles instead of away from them. Thanks to Jasper for the PR!
* Fixed: Actions & Event Actions [GPSDescriptions:string] and [GPSColors:Color] not being used.
* Fixed: Offline players being considered NPCs in some cases.
* Fixed: Rare crash when list of players was edited by two parts of MES simultaneously.
* Fixed: MES not replenishing tanks of encounters, leading to them running out of H2 and falling out of the sky in some cases. Tank replenishment now requires [ReplenishSystems:true] in the SpawnGroup.

enenra & CptArthur
