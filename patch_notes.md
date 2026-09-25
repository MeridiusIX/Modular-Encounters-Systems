# Update 2.74.03

* Added SpawnCondition [MaxWaterDepth:double].
* Added SpawnCondition [EnableItemTriggeredContracts:bool] to allow for the spawning of items for economy item-retrieval contracts (Economy must be on in world settings).
* Added Chat Command /MES.Info.GetLocationMatrix to copy location information to the clipboard for zone placement. Big thanks to [Digi](https://steamcommunity.com/id/hunterdigi/myworkshopfiles/?appid=244850) for guiding us through setting this up and providing a bunch of code!
* Added Action [ChangeNpcFactionCreditsAmountCounter:string] to define the amount in a counter variable.
* Improved [#362](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/362): Grid spawning-induced lag. Thanks to [@maxpowa](https://github.com/maxpowa) for the PR!
* Fixed [#360](https://github.com/MeridiusIX/Modular-Encounters-Systems/pull/360): ButtonPress trigger type could be triggered by any button panel with the same name - also on player grids. Thanks to [@Blaylock1988](https://github.com/Blaylock1988) for the PR!
* Fixed [#365](https://github.com/MeridiusIX/Modular-Encounters-Systems/issues/365): ChangeNpcFactionCredits did not use ChangeNpcFactionCreditsAmount to define the amount.
* Fixed some instances in which credits were not correctly deduced from player / faction accounts.

enenra & CptArthur
