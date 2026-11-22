# Update 2.76.00

* Added: First level of support for vanilla NPC bots. See [this explanation](https://discord.com/channels/398249868805537803/557949898922786816/1571995313302933651) on the MES discord for details.
* Added: New SpawnDebug logging category "All" which turns all logging on / off: /MES.SpawnDebug.All.true
* Improved: Information output form /MES.GESAP and /MES.Info.GetActiveNpcs commands.
* Fixed [#466](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/466): Duplicate enter/leave messages for zones. Thanks to Jasper for the PR!
* Fixed: Issue with NPC encounter tracking that would cause subgrids (including wheels) of encounters to count as individual encounters. As such, they would quickly fill up the pool of maximum allowed NPC spawns in an area until removed. This will not fix existing NPCs but will fix any newly spawning ones. Use /MES.GESAP to see active NPCs for each spawning type and clean them up manually to fix spawning issues. Big thanks to Mike Dude and Digi for helping with this.
