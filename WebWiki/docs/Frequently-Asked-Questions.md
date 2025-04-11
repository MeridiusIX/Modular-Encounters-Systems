#Frequently-Asked-Questions.md

Here are some common questions I receive for Modular Encounters Systems (MES) and its add-on mods:

**Q: What order do I need to add MES and other Encounter Mods in the mod list?**  
A: MES and Encounter Mods utilizing it can be loaded in any order or position in your mod list. I do not build mods that require a specific load order, nor do I ever plan to - it adds needless complication otherwise.

**Q: How do I decrease / increase the amount of spawns?**  
A: You can use the configuration files listed in the **Admin & Configuration** section to control many aspects of spawn timing and frequency. Individual spawngroups can also be blacklisted by providing the spawngroup subtypeID (ie: its name). The configurations cannot change the spawn rates/frequencies of encounters from specific mods (eg: making Reavers less common, etc).

**Q: I see too many of the same things spawning in the area I'm in. How do I get more unique spawns?**  
A: If you're in an area that has unfavorable environmental conditions (thin atmosphere, rough terrain, etc), you may see less variety of encounters. You can solve this by moving to more open areas, and using planets have do not have shallow atmosphere (Pertam is an example of a shallow atmosphere world). Planet size is also something to consider, since smaller planets will result in a more shallow atmosphere. Alternatively, you can also add more encounter mods to your world - especially ones that have encounters that can operate in multiple environments.

**Q: How do I turn off Jetpack / Drill / etc Inhibitor blocks?**  
A: Shoot them until they break. There are no config options to disable these blocks, nor are there any plans to introduce any. Since turrets seem to become more useless against character entities with each game update, I have no incentive to remove these systems. Only a small percentage of NPC mods use the inhibitor blocks, so you have other options available if you don't enjoy that play style.

**Q: Is MES causing vanilla economy station stores to be empty?**  
A: No. What causes that is older mods that re-implement vanilla ore. If they're re-implemented without minimal price data that the economy system requires to generate pricing, then store blocks will be empty because all other item pricing cannot be calculated (ore -> ingot -> component -> block).

**Q: Does MES work the the Exploration Enhancement Mod (EEM)?**  
A: Yes. While EEM was not designed to specifically use MES, the spawner is designed in a way that allows virtually all NPC mods to work with it.