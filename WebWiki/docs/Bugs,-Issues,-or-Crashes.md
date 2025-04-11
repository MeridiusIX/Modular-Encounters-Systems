#Bugs,-Issues,-or-Crashes.md

If you are having issues with the mods I curate, please read through this document.

# Reporting Issues/Crashes while using Torch and/or Plugins

If you are experiencing issues or crashing with any of my mods while using Torch and any of its plugins (or any plugins at all), I will ask that you reproduce the issues in an environment where Torch/Plugins are not involved before I consider investigating.

The reason I am taking this stance is because I have dealt with several instances of 'False Positive' issue/crash reports when Torch/Plugins are involved. This wastes a lot of my time since I end up chasing issues that do not actually exist in my mods. I understand many server operators prefer to use Torch/Plugins, but because of the unpredictible nature of those types of add-ons, I have no way to ensure that my mods will function properly when server admins choose to use them.

If you need a quick way to test mods in a Standard Dedicated Server, I recommend downloading the Dedicated Server software from your Steam Tools and loading/creating a world to test on your local machine. It will likely run a little choppy, but does provide a quick and easy way to test for specific bugs.

# Crashes

If you are seeing a situation where one of my mods is causing the game/server to crash (either to the desktop or the main menu), **you MUST provide a complete log file before I will investigate**. Without the log file, I cannot see what method/process is responsible for the crash and cannot fix it.

A snippet of the stack trace is often not sufficient, please provide the entire log.

If you are playing in Single Player or Hosting Local Multiplayer, you can find the most recent logs by doing the following:

 - Press `Windows Key + R`  

 - Enter `%AppData%\SpaceEngineers` and Press `OK`  

 - Look for files called `SpaceEngineers_YYYYMMDD_HHMMSSmmm.log` (`YYYYMMDD_HHMMSSmmm` will appear instead as a bunch of numbers representing a time stamp for when you started the session)  

 - Identify the most recent log from when the crash occured.  

 - The easiest way to provide me with the log file is to upload it on my **[Discord Server](https://discord.gg/8rZpMqq)**. 

If you are playing on a Dedicated Server, then the logs are usually found in the `SpaceEngineersDedicated` folder and follow the naming format `SpaceEngineersDedicated_YYYYMMDD_HHMMSSmmm.log`. If you are using a 3rd party hosting company, they may have the logs stored in other directories. Consult with their support services if you are unsure how to locate them.

# Issues

If you are experiencing issues that could not be resolved in the other troubleshooting pages, try the following steps first:

 - Remove any unrelated mods, and test to see if the issue persists.  

   - If the issue doesn't persist, then add the removed mods back a few at a time until you can narrow down which is causing the conflict. If you find another mod that is conflicting, let me know which mod(s) are causing the issue.  

   - If the issue does persist, note any relevant steps to reproduce the issue and provide them to me on the **Workshop Page Comments** or the **[Discord Server](https://discord.gg/8rZpMqq)**.
  
 - Try clearing your mod cache (Keep in mind this will force steam to redownload all workshop items you are currently subscribed to).  

   - `Right Click` on `Space Engineers` in your Steam Client Library  
   
   - Navigate to `Manage > Browse Local Files`  

   - In the folder that opens, press `Alt + UP` twice on your keyboard. This should bring you to a folder called `steamapps`

   - Open the `workshop` folder.

   - Delete the `appworkshop_244850.acf` file.

   - Open the `content` folder. Then open the `244850` folder.

   - Delete everything inside of the `244850` folder.

   - Restart Steam and wait for it to redownload your mod content.

# Common Issues and Resolutions

Below is a list of some common issues you may run into, and some potential solutions:

 - **Issues With The Modular Encounters Systems:** If you are having issues with Modular Encounters Systems, [try following the steps in this guide](https://github.com/MeridiusIX/Modular-Encounters-Systems/wiki/Encounter-Guide-and-Troubleshooting), specifically in the *Troubleshooting Encounters and Other Issues* section. Many issues can be resolved with the advice found in that section.  

 - **Logs Showing a Critical Error:** Anytime a mod is reporting a Critical Error, it's almost always the result of a corrupted download from Steam. Try unsubscribing from the mod, restarting Steam, and then resubscribing to the mod. If that doesn't fix it, then follow the steps above regarding clearing your mod cache.  

 - **Logs Show Mod Errors With Several `Resource not found, setting to null` messages:** This typically means that your server cannot resolve the path directory for some icon files used by some modded definitions. This is typically not going to cause any real issues with the mod, despite how many times it can appear in the log.  

 - **Logs Show Message Stating `Reference issue detected (circular reference or wrong order) for mod` one or more times:** This message will often appear if you are using any mods that have other mods listed as dependencies. In reality there's no issue, it's just a poorly worded message by the devs to indicate that dependency mods are being loaded. You will often see this with mods using the Modular Encounters Systems, WeaponCore, and TextHudAPI - since they are some of the most common mods that other mods will use as dependencies.  

# Steam Discussions

Please refrain from posting issues in the <u>**Discussions**</u> sections of my mod pages. The main <u>**Comments**</u> section is fine, but on most of my mod pages I do not have notifications turned on for Discussions, so I will probably miss them if you post them there.