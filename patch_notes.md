# Update 2.73.02

* Added variable and RandomNameGenerator support for SetCustomStrings action.
* Add support for variables in ReplaceValues for missions.
* Add support for variables to LcdTemplateFiles, LcdBlockNames and LcdText tags.
* Added ability for the ButtonPress-trigger to listen to any button press as opposed to only presses of specific buttons. See [commit ](https://github.com/MeridiusIX/Modular-Encounters-Systems/commit/dec7bd31cd743ed51da3e51de576a3e3060aa4b9)for details.
* Added ability to change arbitrary faction reputations via actions and event actions. See [commit ](https://github.com/MeridiusIX/Modular-Encounters-Systems/commits/master/)for details.
* Changed action execution order and moved all counter logic to the beginning. This means that counters can be increased, and their increased state is then considered in the rest of the action.

enenra & CptArthur
