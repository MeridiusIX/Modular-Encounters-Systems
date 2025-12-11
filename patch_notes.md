# Update 2.71.45

* Added TriggerType "InsideActiveZone" to check if an encounter is in a zone that is active - as opposed to "InsideZone" which matches zones independent of their active state.
* Added TriggerType "OutsideActiveZone" with the same deal.
* Added PlayerCondition tag "[CheckPlayerInActiveZone:bool]" as opposed to "CheckPlayerInZone" with the same idea as above.
* Fixed MES occasionally removing player grids. Thanks to irreality-net for the PR.

enenra & CptArthur
