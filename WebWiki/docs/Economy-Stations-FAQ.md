#Economy-Stations-FAQ.md

**What are Economy Stations?**  
Economy Stations are stations that can appear in your world that allow you to buy and sell items, and also fulfill contracts for additional credits and faction reputation. They are often owned by neutral factions, although sometimes they can be owned by Pirates as well. These stations will always be located in a SafeZone, so they cannot be captured or damaged by players.

**Where do these stations appear?**  
There are 3 types of location that you can usually find these stations. 
 - Planetary (On the surface of a planet)
 - Orbital (Just outside the gravity range of a planet)
 - Deep Space (You likely will not find these by exploration alone)

**How do I find them?**  
Here are a few methods for locating these stations:
 - Check the Seat Inventory of your RespawnShip. It should contain a DataPad that lists the location of a station nearby.
 - Try flying around a small moon. Because each planet often has a number of stations, it's often easy to find some around small planet entities.
 - Some stations also sell DataPads that contain coordinates to other stations in your game world.

**The stations don't appear in the Admin Entity List...**  
When there are no players near a station, they seem to temporarily despawn. Once you get close again, they will respawn and should appear in the list.

**Can these stations be added to Existing Saves and/or Custom Planets?**  
They can, however it is important to understand how these stations are created by the game. When you load a world with the Enable Economy Option turned on, the game does a one-time evaluation of the world and calculates where all the stations for each faction will appear. Because of this, you need to be careful when enabling the Economy Option to an existing save. For example, if you enable it on an Empty World, all the stations are created in Deep Space - making them all incredibly difficult to ever find. Disabling and Re-enabling Economy after the stations have already been created does not appear to fix this either. Here are some steps you can follow to ensure things go smoothly:
 - Create your save, ensuring Enable Economy is initially **Disabled**  
 - Add planets manually (vanilla or custom, they both work), and make any other initial changes you need.
 - Once you are satisfied with your save, enable the Economy Option.
 - Stations should now appear in all areas!

**How do I add stations to an existing save when Economy is already on?**  
This one is a little trickier, and you may have to bump up your max faction count -- but to ask the game to generate new stations on an existing save with economy already turned on, the following steps can be used:  

 - Save and exit the game.  You cannot make this change to a running game (or running server).  
 - Open up the Sandbox.sbc file for your save.  Make sure the one you open is in line with the time when you last saved it. 
 - Search for `MyObjectBuilder_SessionComponentEconomy` in this file. It will be near the bottom.
 - Change the value for `GenerateFactionsOnStart` to be `true` (note the lower case). 
 ```<GenerateFactionsOnStart>true</GenerateFactionsOnStart>```
 - Save and close the file
 - Reload your world, switch to spectator mode (admin), fly up to the nearest moon, and teleport your character there (ctrl+spacebar).
    - Make sure to fly around with your jetpack, as stations don't seem to spawn for spectator camera. 
    - Moons generally have the highest density of stations, so it will be easiest to find one there.

**What role does the Modular Encounters Spawner mod have with these stations.**  
None. There are a lot of functionalities that the stations have that we cannot access via ModAPI. As a result, I decided that the Modular Encounters Spawner would not manage these stations in any capacity. The spawner mod does not spawn them, and it does not despawn them. Since there's no interaction between them, they should be fully safe to use together.