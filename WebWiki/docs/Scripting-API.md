#Scripting-API.md

Modular Encounters Systems includes a Scripting API that can be used to trigger spawn events, manipulate known player locations, interact with behavior trigger/actions, and more!

To use the API, you will need to download the [MESApi.cs](https://github.com/MeridiusIX/Modular-Encounters-Systems/blob/master/Data/Scripts/ModularEncountersSystems/API/MESApi.cs) file, and add it to your own mod. Once you have copied it, change the `namespace` to match the namespace of your own mod.

To initialize the API, simply create the API object in the `LoadData()` method of your SessionComponent. This should only be done on the Server Session. Running the API on a non-server client will not work. Example:  

```
public override void LoadData(){

    SpawnerAPI = new MESApi();

}
```

The API should be ready to use after the BeforeStart() SessionComponent method has run.  

After you have initialized the API, you can check `MESApiReady` to ensure it has loaded correctly. The `MESApi.cs` file has comments and documentation accompanying all methods, explaining their use and utility.  