#Encounter-Guide.md

This guide provides some insight into how encounters spawns using the Modular Encounters Systems framework mod. This can be used to understand when you will see NPC grids in your game world.


# What Mods and Settings do you need?

Because the framework is a Mod, you will need to ensure that Experimental Mode is enabled. This is done from the options menu accessed from the title screen.  

The first mod you will need is the **Modular Encounters Systems**, which is responsible for spawning encounters in your game world. However, most encounter mods that use the features of the framework will often list it as a mod dependency in Steam - which allows Space Engineers to Automatically load it when selecting said encounter mods.  

Loading the framework mod by itself will not add any new encounters to your game. It is a framework mod that other encounter mods use to spawn their NPCs. I maintain a Steam Collection of mods that are friendly with MES, even if they do not directly use its features. That list can be found [at this link](https://steamcommunity.com/sharedfiles/filedetails/?id=1991339991). Please note this isn't an exhaustive list, so there are other encounter mods on the workshop that should also be compatible.

For world settings, you may want to enable **In-Game Scripts** and **Enable Drones**. In-game Scripts is what many mods still use for NPC Grid AI, and Enable Drones is the system a lot of mods use to call reinforcements (this is done via 'Pirate Antennas').  

The world options for **Cargo Ships** and **Random Encounters** will be automatically turned off when using Modular Encounters Systems. The reason these world options are disabled is because the framework implements its own scripting for these features (effectively replacing them), so having them enabled would likely cause conflicts. If you are using older encounter mods that say they need those world options enabled, don't worry - the framework is built to be backwards compatible with them!   
  
# What Types of Encounters Exist?  

There are a handful of ways that encounters can spawn using Modular Encounters Systems. Here are some of the different types and how they are triggered:   

* **Space Cargo Ship:** These encounters will spawn in space, or while the player is in a shallow gravity field (such as a moon). They typically appear about 5-10 KM from a player and will travel in a linear path for about 10-15 KM before despawning. These spawning events typically occur on a random timer, which is usually about 20-30 minutes.  

* **Random Encounters:** These encounters also appear while you are in space. They will not appear if you are in a gravity well of any kind. They will appear in a random position near a player, about 7-15 KM away. Random Encounter spawns are triggered when the player is exploring space, which means you need to travel a certain distance before they will appear (about 15 KM). There is also a cooldown per player after a spawn is triggered - this is to prevent a player from using their jump drive rapidly to generate new spawns, which can affect game performance if too many grids exist in the world at once.  

* **Planetary Cargo Ships:** These are very similar to their space counterparts, but will spawn typically in planets with a rich atmosphere.  

* **Planetary Installations:** These are similar to Random Encounters in Space, but will spawn as static grids on planets that have a lot of flat surfaces. Players will need to travel about 6 KM before a spawn event is triggered.  

* **Boss Encounters:** These encounters can appear in both Space and on Planets. They initially appear as a purple colored GPS marker, usually accompanied by a message that is broadcast to near-by player using the in-game chat system. The encounter will not physically spawn into the world until a player approaches the GPS signal (getting within about 300m of the signal). These signals will appear on a timer (approx 20min, and will remain visible for about another 20min). You should not approach one of these encounters unless you are well armed and ready for a difficult encounter.  

* **Drone Encounters:** These encounters will spawn near players using a timer and distance system that is set in their individual SpawnGroups. The behavior of these encounters is entirely dependent on how the mod author has configured them. This encounter type could be considered a miscellaneous one.  

* **Static Encounters:** These encounters are designed to always appear at a specific location in the game world once a player gets within a certain range of it. Typically, they will only spawn once, since they are locked to specific locations.  

* **Creature:** These encounters are not grid-based NPCs, but rather character-based. Common examples of these encounters would include the vanilla Wolves and Spiders. These encounters can sometimes use the **AiEnabled** mod to govern their behaviors as well.
