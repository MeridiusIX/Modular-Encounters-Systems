#Troubleshooting-Tips.md

This page aims to provide some common troubleshooting steps you can try if your spawns or behaviors are not functioning as expected.

# Common

**Ensure That Profiles (SpawnGroup, Behavior, Profile, etc) Loaded**  
Check to see if your various profiles were detected at load time. You can easily get this information with the chat command `/MES.Info.GetAllProfiles`.

***

**Check Your File Formats**  
If you save one of your Definition Files (ie, SpawnGroup, Profile, or Other XML Based Document) with a File Extension other than `.sbc`, then the game will not load it. A common error is saving them with the `.xml` or `.txt` formats.

***

**Check For Duplicate SubtypeIDs**  
If you use the same SubtypeID twice for a profile, spawngroup, etc - then the game will only use one of the definitions/profiles. This is a little trickier to look for, so I recommend using Notepad++ and using its search features to search for the ID across all your mod files.


# Spawning


# Behaviors

**Does the Grid Have a Remote Control?**  
Ensure the grid you are trying to add behavior to has a Remote Control block that can be replaced by the Spawner during spawning (or that it is already has a RivalAI compatible remote control).  

***

**Is the Spawn Properly Configured?**  
Check your SpawnGroup / Spawn Conditions Profile Definition to ensure that the following tags are included:  

`[UseRivalAi:true]`  
`[RivalAiReplaceRemoteControl:true] //If prefab has vanilla remote control`  

Also, ensure that each of your prefabs that will use a behavior has the `<Behaviour>YourBehaviorSubtypeId</Behaviour>` tag, and ensure that `YourBehaviorSubtypeId` is replaced with the actual SubtypeId of your behavior profile. 
