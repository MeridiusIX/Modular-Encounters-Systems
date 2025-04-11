#Encounter-Attributes.md

# Behavior Difficulty

This section defines the various difficulty levels of encounters behaviors, ie how they interact with the player and other entities in the world.

|Rating|Title&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Description|
|:-----|:-----|:-----|
|★☆☆☆☆|No Challenge|Encounters are not hostile.|
|★★☆☆☆|Minimal|Encounters are passively hostile, will only use turrets if you get close.|
|★★★☆☆|Average|Encounters will likely not engage players unless provoked or approached.|
|★★★★☆|Hostile|Encounters will seek players and attack when conditions are met (visibility, signals, etc).|
|★★★★★|Nightmare|Encounters are easily aware of targets in the area and will relentlessly attack.|

***

# Grid Difficulty

This section defines how difficult the ships / stations you'll encounter can be.

|Rating|Title&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Description|
|:-----|:-----|:-----|
|★☆☆☆☆|No Challenge|Grids are not significantly armed and/or armored.|
|★★☆☆☆|Minimal|Smaller grids that are lightly armed and/or armored.|
|★★★☆☆|Average|Average sized grids that are moderately armed and/or armored.|
|★★★★☆|Difficult|Larger sized grids that are heavily armed and/or armored.|
|★★★★★|Nightmare|Massive sized grids that are heavily armed and/or armored.|

***

# Environments

This section defines the various environments that encounters can appear in.

|Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Description|
|:-----|:-----|
|Space|Encounters can appear in space (zero gravity).|
|Atmosphere|Encounters can appear on planets with an atmosphere.|
|Gravity|Encounters can appear on planets without an atmosphere.|
|Water|Encounters can appear on water surface (while using Water Mod).|
|Underwater|Encounters can appear under the water surface (while using Water Mod).|

***

# Encounter Types

This section defines the various types of encounters and how/when they generally spawn.

|Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Description|
|:-----|:-----|
|Space Cargo Ship|Encounters that appear on a timer in space, fly in a linear path, and despawn when they reach the end of their path.|
|Planetary Cargo Ship|Encounters that appear on a timer near a planet's surface, fly in a linear path, and despawn when they reach the end of their path.|
|Random Encounter|Encounters that appear at random in space as the player travels across large distances.|
|Planetary Installation|Encounters that appear at random on a planet's surface as the player travels across large distances.|
|Static Encounter|Encounters that appear once the player has gotten close to where it should appear. These encounters only appear once.|
|Boss Encounter|Difficult encounters the player can optionally engage by traveling to a designated GPS signal.|
|Drone Encounter|Encounters that appear randomly on a timer near the player proximity.|
|Creature|Animal or bot encounters that appear on a planet's surface near the player.|

***

# Spawning Conditions

This section defines various additional conditions that may also be used when determining if an encounter is eligible to spawn.

|Condition&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Description|
|:-----|:-----|
|Chance|Encounters will use an additional chance roll to determine if they're allowed to spawn in certain environments.|
|Combat Phase|Encounters will only appear during a Combat Phase in the world. If Combat Phases are disabled, then this check is omitted.|
|Known Player Location|Encounters can designate an area around a player as a Known Player Location, which can potentially allow encounters to spawn more often or detect the player easier.|
|Reputation|Encounters will only appear if a player's reputation with an NPC faction is at a certain value.|
|Threat Score|Encounters will only appear once players have progressed / built enough grid resources in an area.|
|Weather|Certain encounters will only appear during specific weather events or times of the day.|
|Zones|Certain encounters will only appear in certain areas.|

***

# Special Abilities

This section defines special blocks, attributes, and abilities that encounters may possess.

|Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Description|
|:-----|:-----|
|Defense Shields|Spawns with Defense Shields (if the Defense Shields mod is loaded).|
|Energy Inhibitor|Drains suit energy from unseated players while in the inhibitor block range (disable the inhibitor block to stop this effect).|
|Grinder Damage|Damages players that attack an encounter while using a grinder (disable the AI Control Module to stop this effect).|
|Hand Drill Inhibitor|Disables Hand Drills while in the inhibitor block range (disable the inhibitor block to stop this effect).|
|Jetpack Inhibitor|Disables Jetpack Dampeners while in the inhibitor block range, and disables Jetpack entirely at closer ranges (disable the inhibitor block to stop this effect).|
|Jump Drive Inhibitor|Disables Jump Drive blocks while in the inhibitor block range (disable the inhibitor block to stop this effect).|
|Merchant Equipment|Encounters may include store blocks, Shipyard System, and/or Suit Upgrade Stations.|
|Nanobot Inhibitor|Disables Nanobot blocks while in the inhibitor block range (disable the inhibitor block to stop this effect).|
|Personnel Inhibitor|Damages unseated players while in the inhibitor block range (disable the inhibitor block to stop this effect).|
|Reinforcements|Calls for additional support / spawns drones to assist.|
|Randomized Weapons|Spawns with randomized weapons, which also includes weapons from mods you may have loaded.|