#AdminConfig_Combat.md

**Content will release with update 2.68.0**  

You can find the Grids Settings Configuration File in `MySaveWorldFolder\Storage\1521905890.sbm_ModularEncountersSpawner\Config-Combat.xml`. The settings you can modify are listed below:

|Setting:|EnableCombatPhaseSystem|
|:----|:----|
|XML:|`<EnableCombatPhaseSystem>Value</EnableCombatPhaseSystem>`|
|Chat Command:|`/MES.Settings.Combat.EnableCombatPhaseSystem.Value`|
|Description:|This setting allows you to specify if the Combat Phase system should be used. This system sets 2 timers that alternate a Peace phase and a Combat phase. During the Combat phase, you may find more actively hostile encounters.|

|Setting:|MinCombatPhaseSeconds|
|:----|:----|
|XML:|`<MinCombatPhaseSeconds>Value</MinCombatPhaseSeconds>`|
|Chat Command:|`/MES.Settings.Combat.MinCombatPhaseSeconds.Value`|
|Description:|Specifies the minimum length of time a Combat phase can last.|

|Setting:|MaxCombatPhaseSeconds|
|:----|:----|
|XML:|`<MaxCombatPhaseSeconds>Value</MaxCombatPhaseSeconds>`|
|Chat Command:|`/MES.Settings.Combat.MaxCombatPhaseSeconds.Value`|
|Description:|Specifies the maximum length of time a Combat phase can last.|

|Setting:|MinPeacePhaseSeconds|
|:----|:----|
|XML:|`<MinPeacePhaseSeconds>Value</MinPeacePhaseSeconds>`|
|Chat Command:|`/MES.Settings.Combat.MinPeacePhaseSeconds.Value`|
|Description:|Specifies the minimum length of time a Peace phase can last.|

|Setting:|MaxPeacePhaseSeconds|
|:----|:----|
|XML:|`<MaxPeacePhaseSeconds>Value</MaxPeacePhaseSeconds>`|
|Chat Command:|`/MES.Settings.Combat.MaxPeacePhaseSeconds.Value`|
|Description:|Specifies the maximum length of time a Peace phase can last.|

|Setting:|CombatPhaseModIdOverride|
|:----|:----|
|XML:|`<CombatPhaseModIdOverride>`<br />   `<string>Value1</string>`<br />   `<string>Value2</string>`<br />`</CombatPhaseModIdOverride>`|
|Chat Command (Add):|`/MES.Settings.Combat.CombatPhaseModIdOverride.Add.Value`|
|Chat Command (Remove):|`/MES.Settings.Combat.CombatPhaseModIdOverride.Remove.Value`|
|Description:|Allows you to specify one or more Mod IDs that will have their encounters registered as Combat phase only encounters. To add more IDs to the list, simply create a new line between the `<CombatPhaseModIdOverride>` and `</CombatPhaseModIdOverride>` tags and enter the following `<string>Value</string>` - Replace `Value` with the Mod ID you want to register.

|Setting:|AllPhaseModIdOverride|
|:----|:----|
|XML:|`<AllPhaseModIdOverride>`<br />   `<string>Value1</string>`<br />   `<string>Value2</string>`<br />`</AllPhaseModIdOverride>`|
|Chat Command (Add):|`/MES.Settings.Combat.AllPhaseModIdOverride.Add.Value`|
|Chat Command (Remove):|`/MES.Settings.Combat.AllPhaseModIdOverride.Remove.Value`|
|Description:|Allows you to specify one or more Mod IDs that will have their encounters registered to be able to spawn during Combat and Peace phases. To add more IDs to the list, simply create a new line between the `<AllPhaseModIdOverride>` and `</AllPhaseModIdOverride>` tags and enter the following `<string>Value</string>` - Replace `Value` with the Mod ID you want to register.

|Setting:|CombatPhaseSpawnGroupOverride|
|:----|:----|
|XML:|`<CombatPhaseSpawnGroupOverride>`<br />   `<string>Value1</string>`<br />   `<string>Value2</string>`<br />`</CombatPhaseSpawnGroupOverride>`|
|Chat Command (Add):|`/MES.Settings.Combat.CombatPhaseSpawnGroupOverride.Add.Value`|
|Chat Command (Remove):|`/MES.Settings.Combat.CombatPhaseSpawnGroupOverride.Remove.Value`|
|Description:|Allows you to specify one or more SpawnGroup Names that will have their encounters registered as Combat phase only encounters. To add more Names to the list, simply create a new line between the `<CombatPhaseSpawnGroupOverride>` and `</CombatPhaseSpawnGroupOverride>` tags and enter the following `<string>Value</string>` - Replace `Value` with the SpawnGroup you want to register.

|Setting:|AllPhaseSpawnGroupOverride|
|:----|:----|
|XML:|`<AllPhaseSpawnGroupOverride>`<br />   `<string>Value1</string>`<br />   `<string>Value2</string>`<br />`</AllPhaseSpawnGroupOverride>`|
|Chat Command (Add):|`/MES.Settings.Combat.AllPhaseSpawnGroupOverride.Add.Value`|
|Chat Command (Remove):|`/MES.Settings.Combat.AllPhaseSpawnGroupOverride.Remove.Value`|
|Description:|Allows you to specify one or more SpawnGroup Names that will have their encounters registered to be able to spawn during Combat and Peace phases. To add more Names to the list, simply create a new line between the `<AllPhaseSpawnGroupOverride>` and `</AllPhaseSpawnGroupOverride>` tags and enter the following `<string>Value</string>` - Replace `Value` with the SpawnGroup you want to register.

|Setting:|UseCombatPhaseSpawnTimerMultiplier|
|:----|:----|
|XML:|`<UseCombatPhaseSpawnTimerMultiplier>Value</UseCombatPhaseSpawnTimerMultiplier>`|
|Chat Command:|`/MES.Settings.Combat.UseCombatPhaseSpawnTimerMultiplier.Value`|
|Description:|Allows you to specify if the Combat Phase spawn timers should be increased or decreased. Example: Setting to 2 would mean encounters spawn twice as quickly during Combat, while setting to 0.5 would result in them taking twice as long to spawn.|

|Setting:|CombatPhaseSpawnTimerMultiplier|
|:----|:----|
|XML:|`<CombatPhaseSpawnTimerMultiplier>Value</CombatPhaseSpawnTimerMultiplier>`|
|Chat Command:|`/MES.Settings.Combat.CombatPhaseSpawnTimerMultiplier.Value`|
|Description:|Specifies the multiplier that is applied to the Spawn Timer increments if using the `UseCombatPhaseSpawnTimerMultiplier` option.|

|Setting:|AnnouncePhaseChanges|
|:----|:----|
|XML:|`<AnnouncePhaseChanges>Value</AnnouncePhaseChanges>`|
|Chat Command:|`/MES.Settings.Combat.AnnouncePhaseChanges.Value`|
|Description:|Allows you to specify if an announcement should be broadcast to players when Combat and Peace phases are changed.|
