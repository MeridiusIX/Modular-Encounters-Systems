# Update 2.75.02

* Added: SpawnGroup tag [ReplenishTanks:bool] (default true) to control if NPCs have their tanks refilled when low.
* Improved [#405](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/405): Allow multiple triggers to trigger on the same target change as opposed to only one. Thanks to Mike Dude for the PR!
* Improved: If any block on a NPC's grid is owned by a player, tank refill is disabled.
* Fixed [#411](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/411): DespawnMES trigger type not triggering properly for the Any option. Thanks to Mike Dude for the PR!
* Fixed [#410](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/410): Fix Trigger tag [AllowTargetFarWithoutTarget:bool] not working correctly. Thanks to Mike Dude for the PR!
* Fixed [#408](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/408): Escort slots not properly being cleaned up. Thanks to Mike Dude for the PR!
* Fixed [#406](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/406): NPCs with thrust restrictions would have their thrusters turned off on savegame load. Thanks to Mike Dude for the PR!
* Fixed: Event leak and added additional safeties.
* Fixed: NPC grids would not get their owners evaluated correctly and as a result may incorrectly be marked as player-owned.
