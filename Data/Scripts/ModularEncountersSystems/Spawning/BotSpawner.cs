using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.World;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.AI;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Spawning
{
    public static class BotSpawner
    {
        public static List<IMyCockpit> OccupiedCockpits = new List<IMyCockpit>();

        public static void Setup()
        {

            BotOverrideConfig();

            MES_SessionCore.UnloadActions += Unload;

        }

        public static bool SpawnBots(SpawnGroupCollection collection, PathDetails path, EnvironmentEvaluation environment)
        {
            int count = 0;

            if (collection.Conditions.MinCreatureCount == collection.Conditions.MaxCreatureCount)
                count = collection.Conditions.MinCreatureCount;

            else
                count = MathTools.RandomBetween(collection.Conditions.MinCreatureCount, collection.Conditions.MaxCreatureCount);

            if (count > path.CreatureCoords.Count)
                count = path.CreatureCoords.Count;

            bool spawnedBot = false;

            if (count == 0)
                return false;

            for (int i = 0; i < count; i++)
            {

                BotSpawnProfile profile = null;
                string botType = "";
                string role = "";
                string name = null;
                bool useAiEnabled = false;
                Color? color = null;
                collection.Faction = collection.SelectRandomFaction();
                long owner = FactionHelper.GetFactionMemberIdFromTag(collection.Faction);
                var coords = path.CreatureCoords[i];
                //Logger.Write("Bot Coords: " + coords);
                var up = environment.NearestPlanet.UpAtPosition(coords);
                var forward = VectorHelper.RandomPerpendicular(up);
                var matrix = MatrixD.CreateWorld(coords, forward, up);

                if (collection.Conditions.BotProfiles.Count > 0)
                {

                    if (collection.Conditions.BotProfiles.Count == 1)
                    {

                        profile = collection.Conditions.BotProfiles[0];

                    }
                    else
                    {

                        profile = collection.Conditions.BotProfiles[MathTools.RandomBetween(0, collection.Conditions.BotProfiles.Count)];

                    }


                    IMyCharacter character = null;
                    SpawnBotRequest(profile, matrix, out character, null, owner);
                    spawnedBot = true;

                }
                else
                {

                    if (collection.Conditions.CreatureIds.Count > 0)
                    {

                        if (collection.Conditions.CreatureIds.Count == 1)
                        {

                            botType = collection.Conditions.CreatureIds[0];

                        }
                        else
                        {

                            botType = collection.Conditions.CreatureIds[MathTools.RandomBetween(0, collection.Conditions.CreatureIds.Count)];

                        }

                        role = collection.Conditions.AiEnabledRole;
                        useAiEnabled = collection.Conditions.AiEnabledModBots;

                        if (string.IsNullOrWhiteSpace(botType))
                            continue;

                        IMyCharacter character = null;
                        //MyVisualScriptLogicProvider.ShowNotificationToAll("Bot Faction Owner: " + collection.Faction, 3000);
                        SpawnBotRequest(botType, matrix, out character, name, useAiEnabled, role, null, owner, color);
                        spawnedBot = true;

                    }

                }

            }

            return spawnedBot;

        }

        public static void SpawnBotRequest(string botId, MatrixD coords, out IMyCharacter character, string name = "", bool aiEnabled = false, string role = null, MyCubeGrid grid = null, long? owner = null, Color? color = null)
        {

            character = null;

            if (aiEnabled)
            {

                if (APIs.AiEnabledApiLoaded)
                {

                    //Logger.Write("API for AiEnabled Detected");
                    var position = new MyPositionAndOrientation(coords);
                    APIs.AiEnabled.SpawnBotQueued(botId, name, position, grid, role, owner, color);

                    if (character != null)
                    {

                        SpawnLogger.Write(string.Format("AiEnabled Character Spawning: {0}", botId), SpawnerDebugEnum.Spawning);

                        /*
						var botIdentity = character?.ControllerInfo?.ControllingIdentityId ?? 0;

						if (botIdentity != 0 && owner.HasValue) {


							var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction((long)owner.Value);

							if (faction != null) {

								var result = MyVisualScriptLogicProvider.SetPlayersFaction(botIdentity, faction?.Tag ?? "");
								//MyVisualScriptLogicProvider.ShowNotificationToAll("Bot Adding To Faction: " + result, 3000);

							} else {

								//MyVisualScriptLogicProvider.ShowNotificationToAll("Bot Faction Not Found", 3000);

							}

						} else {

							//MyVisualScriptLogicProvider.ShowNotificationToAll("Bot Identity or Owner is 0", 3000);

						}
						*/

                    }
                    else
                    {
                        SpawnLogger.Write(string.Format("AiEnabled Spawn Failed For: {0}", botId), SpawnerDebugEnum.Spawning);
                    }

                }
                else
                {
                    SpawnLogger.Write("AiEnabled Found, But API Not Loaded", SpawnerDebugEnum.Spawning);
                }
            }
            else
            {
                SpawnLogger.Write(string.Format("Regular Bot Spawning: {0}", botId), SpawnerDebugEnum.Spawning);
                MyVisualScriptLogicProvider.SpawnBot(botId, coords.Translation, coords.Forward, coords.Up, (string.IsNullOrWhiteSpace(name) ? null : name));
            }
        }

        public static void SpawnBotRequest(BotSpawnProfile profile, MatrixD coords, out IMyCharacter character, IBehavior behavior = null, long? owner = null, bool useBuiltIn = true, List<IMySlimBlock> spawnBlocks = null, List<Vector3D> spawnPoints = null, List<string> tags = null)
        {
            character = null;

            var grid = behavior?.CurrentGrid?.CubeGrid as MyCubeGrid ?? null;
            var npc = behavior?.CurrentGrid?.Npc ?? null;

            if (useBuiltIn)
            {
                IMyCockpit cockpit = null;

                if (spawnBlocks != null && spawnBlocks.Count > 0)
                {
                    List<IMySlimBlock> validBlocks = new List<IMySlimBlock>();
                    List<IMyCockpit> validCockpits = new List<IMyCockpit>();
                    foreach (var b in spawnBlocks)
                    {
                        if (profile.SpawnSeated && b.FatBlock != null && b.FatBlock as IMyCockpit != null && b.FatBlock.IsFunctional)
                        {
                            IMyCockpit cockpit_temp = b.FatBlock as IMyCockpit;
                            // TODO: Multiple actions spawning bots from the same trigger will clear the OccupiedCockpits list and try to spawn bots in the same seats because .IsOccupied is not updated quickly enough.
                            if (!cockpit_temp.IsOccupied && !OccupiedCockpits.Contains(cockpit_temp))
                            {
                                validCockpits.Add(cockpit_temp);
                            }
                        }

                        else if (!profile.SpawnSeated && !(b.FatBlock != null && b.FatBlock as IMyCockpit != null))
                            validBlocks.Add(b);
                    }

                    if (validBlocks.Count <= 0 && validCockpits.Count <= 0)
                    {
                        SpawnLogger.Write($"Error when attempting to spawn bot with profile '{profile.ProfileSubtypeId}': No valid SpawnBlocks on grid.", SpawnerDebugEnum.Spawning);
                        return;
                    }

                    if (profile.SpawnSeated)
                    {
                        cockpit = validCockpits[MathTools.RandomBetween(0, validCockpits.Count)];
                        OccupiedCockpits.Add(cockpit);
                        var loc = cockpit.PositionComp.WorldVolume.Center;

                        coords = MatrixD.CreateWorld(loc, behavior.Grid.RemoteControl.WorldMatrix.Backward, behavior.Grid.RemoteControl.WorldMatrix.Up);
                    }
                    else
                    {
                        var block = validBlocks[MathTools.RandomBetween(0, validBlocks.Count)];

                        Vector3D loc = Vector3D.Zero;
                        block.ComputeWorldCenter(out loc);

                        // TODO: Ideally gravity and block direction would be accounted for here.
                        coords = MatrixD.CreateWorld(loc, behavior.Grid.RemoteControl.WorldMatrix.Backward, behavior.Grid.RemoteControl.WorldMatrix.Up);
                        //MyAPIGateway.Physics.CastRay(loc, behavior.Grid.RemoteControl.Down);
                    }
                }
                else if (spawnPoints != null && spawnPoints.Count > 0)
                {
                    var offset = Vector3D.Transform(spawnPoints[MathTools.RandomBetween(0, spawnPoints.Count)], behavior.Grid.RemoteControl.WorldMatrix);
                    coords = MatrixD.CreateWorld(offset, behavior.Grid.RemoteControl.WorldMatrix.Backward, behavior.Grid.RemoteControl.WorldMatrix.Up);
                }
                else
                {
                    SpawnLogger.Write($"Error when attempting to spawn bot with profile '{profile.ProfileSubtypeId}': No SpawnBlocks nor SpawnPoints defined.", SpawnerDebugEnum.Spawning);
                    return;
                }

                IMyNPCSpawnerSessionComponent comp;
                if (!MyAPIGateway.Session.TryGetComponentByInterfaceType(out comp))
                {
                    SpawnLogger.Write($"Error when attempting to spawn bot with profile '{profile.ProfileSubtypeId}': Failed to get IMyNPCSpawnerSessionComponent.", SpawnerDebugEnum.Spawning);
                    return;
                }

                var factionTag = IdsReplacer.ReplaceId(npc, profile.BotFactionTag);
                var name = string.IsNullOrWhiteSpace(IdsReplacer.ReplaceId(npc, profile.BotDisplayName)) ? null : IdsReplacer.ReplaceId(npc, profile.BotDisplayName);
                var wContainer = string.IsNullOrWhiteSpace(IdsReplacer.ReplaceId(npc, profile.WeaponsContainerId)) ? null : IdsReplacer.ReplaceId(npc, profile.WeaponsContainerId);
                var lContainer = string.IsNullOrWhiteSpace(IdsReplacer.ReplaceId(npc, profile.LootContainerId)) ? null : IdsReplacer.ReplaceId(npc, profile.LootContainerId);
                var chatSet = string.IsNullOrWhiteSpace(IdsReplacer.ReplaceId(npc, profile.ChatMessageSetId)) ? null : IdsReplacer.ReplaceId(npc, profile.ChatMessageSetId);

                SpawnLogger.Write($"Regular Bot Spawning: {profile.BotType}", SpawnerDebugEnum.Spawning);
                var botId = comp.SpawnBot(
                    botDefinitionId: MyDefinitionId.Parse($"{profile.BotType}"),
                    position: coords.Translation,
                    direction: coords.Forward,
                    up: coords.Up,
                    name: name,
                    factionTag: factionTag,
                    weaponsContainerSubtype: wContainer,
                    lootContainerSubtype: lContainer,
                    chatMessageSetSubtype: chatSet,
                    squadGridEntityId: grid?.EntityId ?? 0
                    );

                IMyEntity bot;
                if (!MyAPIGateway.Entities.TryGetEntityById(botId, out bot))
                {
                    SpawnLogger.Write($"Error when attempting to spawn bot with profile '{profile.ProfileSubtypeId}'.", SpawnerDebugEnum.Spawning);
                    return;
                }

                character = (IMyCharacter)bot;

                if (profile.Color != new Vector3I(300, 300, 300))
                    bot.Render.ColorMaskHsv = ColorExtensions.ColorToHSV(new Color(profile.Color.X, profile.Color.Y, profile.Color.Z));

                if (profile.AssetOverrideSets.Count > 0)
                {
                    var assetModifierComp = bot.Components.Get<MyNpcAssetModifierComponent>();
                    if (assetModifierComp != null)
                    {
                        // This picks a random set from the profile and then applies all its components. In the future it would be good to allow proper
                        // overriding of single assets but we currently cannot reset the skin to default so anything done here would be in addition to
                        // the defaults defined in Factions.sbc
                        var skin = profile.AssetOverrideSets[MathTools.RandomBetween(0, profile.AssetOverrideSets.Count)];

                        if (skin != SkinSetEnum.FactionDefault)
                        {
                            assetModifierComp.TryAddAssetModifier($"{skin}_Suit");
                            assetModifierComp.TryAddAssetModifier($"{skin}_Helmet");
                            assetModifierComp.TryAddAssetModifier($"{skin}_Boots");
                            assetModifierComp.TryAddAssetModifier($"{skin}_Gloves");
                        }
                    }
                    else
                    {
                        character.Close();
                        throw new Exception("Failed to get AssetModifierComp");
                    }
                }

                if (profile.SpawnSeated && cockpit != null)
                {
                    // This must be done 1 tick after the bot is created.
                    MyAPIGateway.Utilities.InvokeOnGameThread(() => cockpit.AttachPilot((IMyCharacter)bot));
                    //VRage.Utils.MyLog.Default.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>> Attached bot '{bot}' to cockpit: " + cockpit);
                }

                // Add to tracking
                List<string> appliedTags = new List<string>();
                if (tags != null)
                    appliedTags = tags;

                foreach (var t in profile.Tags)
                    if (!appliedTags.Contains(t))
                        appliedTags.Add(t);

                BotEntity botEntity = new BotEntity(character, profile, character)
                {
                    Tags = appliedTags
                };

                if (npc != null && !npc.Bots.Contains(botEntity))
                    npc.Bots.Add(botEntity);
            }

            else if (!useBuiltIn && APIs.AiEnabledApiLoaded)
            {

                //Logger.Write("API for AiEnabled Detected");
                var position = new MyPositionAndOrientation(coords);
                APIs.AiEnabled.SpawnBotQueued(position, profile.SerializedData, grid, owner);

                if (character != null)
                {
                    //SpawnLogger.Write(string.Format("AiEnabled Character Spawning: {0}", botId), SpawnerDebugEnum.Spawning);
                    var botIdentity = character?.ControllerInfo?.ControllingIdentityId ?? 0;

                    if (botIdentity != 0 && owner.HasValue)
                    {

                        var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction((long)owner.Value);

                        if (faction != null)
                        {
                            var result = MyVisualScriptLogicProvider.SetPlayersFaction(botIdentity, faction?.Tag ?? "");
                            //MyVisualScriptLogicProvider.ShowNotificationToAll("Bot Adding To Faction: " + result, 3000);
                        }
                        else
                        {
                            //MyVisualScriptLogicProvider.ShowNotificationToAll("Bot Faction Not Found", 3000);
                        }

                    }
                    else
                    {
                        //MyVisualScriptLogicProvider.ShowNotificationToAll("Bot Identity or Owner is 0", 3000);
                    }

                }
                else
                {
                    //SpawnLogger.Write(string.Format("AiEnabled Spawn Failed For: {0}", botId), SpawnerDebugEnum.Spawning);
                }
            }

            else
            {
                SpawnLogger.Write("AiEnabled API Not Loaded", SpawnerDebugEnum.Spawning);
            }

        }

        public static void BotSpawnFromAction()
        {



        }

        public static void BotOverrideConfig()
        {

            if (!AddonManager.PlanetCreatureSpawner && !Settings.Creatures.OverrideVanillaCreatureSpawns)
                return;

            SpawnLogger.Write("Scanning For Existing Planet Creatures / Bots", SpawnerDebugEnum.Startup, true);
            //Scan Planets and Generate Temp SpawnGroups
            var allPlanets = MyDefinitionManager.Static.GetPlanetsGeneratorsDefinitions();

            var bots = MyDefinitionManager.Static.GetBotDefinitions();
            var botIds = new List<MyPlanetAnimal>();

            MySpawnGroupDefinition dummyGroup = null;
            var spawnGroups = MyDefinitionManager.Static.GetSpawnGroupDefinitions();

            foreach (var group in spawnGroups)
            {

                if (group.Id.SubtypeName != "MES-CreatureDummySpawnGroup")
                    continue;

                dummyGroup = group;
                break;

            }

            foreach (var bot in bots)
            {

                if (bot as MyAnimalBotDefinition == null)
                    continue;

                var animal = new MyPlanetAnimal();
                animal.AnimalType = bot.Id.SubtypeName;
                botIds.Add(animal);

            }

            var botArray = botIds.ToArray();

            foreach (var planet in allPlanets)
            {

                //If No Planet Animal Info, Create it, Add Spider, and Skip
                if (planet.AnimalSpawnInfo == null && planet.NightAnimalSpawnInfo == null)
                {

                    AddAllBotsToPlanetGenerator(planet, botArray);
                    continue;

                }

                if (planet.AnimalSpawnInfo?.Animals != null && planet.AnimalSpawnInfo.Animals.Length > 0)
                {

                    var name = "CreatureSpawn-" + planet.Id.SubtypeName + "-InternalDaySpawns";

                    if (!SpawnGroupManager.SpawnGroupNames.Contains(name))
                    {

                        var spawnGroup = new ImprovedSpawnGroup();
                        spawnGroup.SpawnGroupName = name;
                        spawnGroup.SpawnConditionsProfiles[0].CreatureSpawn = true;
                        spawnGroup.SpawnConditionsProfiles[0].MinCreatureCount = planet.AnimalSpawnInfo.WaveCountMin;
                        spawnGroup.SpawnConditionsProfiles[0].MaxCreatureCount = planet.AnimalSpawnInfo.WaveCountMax;
                        spawnGroup.SpawnConditionsProfiles[0].MinCreatureDistance = (int)planet.AnimalSpawnInfo.SpawnDistMin;
                        spawnGroup.SpawnConditionsProfiles[0].MaxCreatureDistance = (int)planet.AnimalSpawnInfo.SpawnDistMax;
                        spawnGroup.SpawnConditionsProfiles[0].FactionOwner = "SPID";
                        spawnGroup.SpawnGroup = dummyGroup;
                        spawnGroup.SpawnConditionsProfiles[0].PlanetWhitelist.Add(planet.Id.SubtypeName);
                        spawnGroup.Frequency = 30;

                        foreach (var animal in planet.AnimalSpawnInfo.Animals)
                        {

                            spawnGroup.SpawnConditionsProfiles[0].CreatureIds.Add(animal.AnimalType);

                        }

                        SpawnGroupManager.AddSpawnGroup(spawnGroup);

                    }

                }

                if (planet.NightAnimalSpawnInfo?.Animals != null && planet.NightAnimalSpawnInfo.Animals.Length > 0)
                {

                    var name = "CreatureSpawn-" + planet.Id.SubtypeName + "-InternalNightSpawns";

                    if (!SpawnGroupManager.SpawnGroupNames.Contains(name))
                    {

                        var spawnGroup = new ImprovedSpawnGroup();
                        spawnGroup.SpawnGroupName = name;
                        spawnGroup.SpawnConditionsProfiles[0].CreatureSpawn = true;
                        spawnGroup.SpawnConditionsProfiles[0].MinCreatureCount = planet.NightAnimalSpawnInfo.WaveCountMin;
                        spawnGroup.SpawnConditionsProfiles[0].MaxCreatureCount = planet.NightAnimalSpawnInfo.WaveCountMax;
                        spawnGroup.SpawnConditionsProfiles[0].MinCreatureDistance = (int)planet.NightAnimalSpawnInfo.SpawnDistMin;
                        spawnGroup.SpawnConditionsProfiles[0].MaxCreatureDistance = (int)planet.NightAnimalSpawnInfo.SpawnDistMax;
                        spawnGroup.SpawnConditionsProfiles[0].FactionOwner = "SPID";
                        spawnGroup.SpawnGroup = dummyGroup;
                        spawnGroup.SpawnConditionsProfiles[0].UseDayOrNightOnly = true;
                        spawnGroup.SpawnConditionsProfiles[0].SpawnOnlyAtNight = true;
                        spawnGroup.SpawnConditionsProfiles[0].PlanetWhitelist.Add(planet.Id.SubtypeName);
                        spawnGroup.Frequency = 30;

                        foreach (var animal in planet.NightAnimalSpawnInfo.Animals)
                        {

                            spawnGroup.SpawnConditionsProfiles[0].CreatureIds.Add(animal.AnimalType);

                        }

                        SpawnGroupManager.AddSpawnGroup(spawnGroup);

                    }

                }

                AddAllBotsToPlanetGenerator(planet, botArray);

            }

            //Disable Wolves and Spiders
            MyAPIGateway.Session.SessionSettings.EnableWolfs = false;
            MyAPIGateway.Session.SessionSettings.EnableSpiders = false;

        }

        public static void AddAllBotsToPlanetGenerator(MyPlanetGeneratorDefinition planet, MyPlanetAnimal[] bots)
        {

            planet.AnimalSpawnInfo = new MyPlanetAnimalSpawnInfo();
            planet.AnimalSpawnInfo.SpawnDelayMin = 600000;
            planet.AnimalSpawnInfo.SpawnDelayMax = 3600000;
            planet.AnimalSpawnInfo.SpawnDistMin = 100;
            planet.AnimalSpawnInfo.SpawnDistMax = 1000;
            planet.AnimalSpawnInfo.WaveCountMin = 1;
            planet.AnimalSpawnInfo.WaveCountMax = 4;
            planet.AnimalSpawnInfo.Animals = bots;

        }

        public static void Unload()
        {



        }

    }

}
