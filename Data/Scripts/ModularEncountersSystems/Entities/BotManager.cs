using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ModularEncountersSystems.Helpers;
using Sandbox.ModAPI;
using VRage.Collections;
using VRage.Game.ModAPI;
using VRage.Serialization;
using ModularEncountersSystems.World;

namespace ModularEncountersSystems.Entities
{
    public static class BotManager
    {
        private static List<IMyPlayer> ActiveBotsAndPlayers = new List<IMyPlayer>();
        public static MyConcurrentList<BotEntity> Bots = new MyConcurrentList<BotEntity>();
        public static Action UnloadEntities;

        public static void RefreshAllBots()
        {
            lock (ActiveBotsAndPlayers)
            {
                ActiveBotsAndPlayers.Clear();
                MyAPIGateway.Players.GetPlayers(ActiveBotsAndPlayers);

                foreach (var bot in ActiveBotsAndPlayers)
                {
                    if (string.IsNullOrWhiteSpace(bot.DisplayName))
                        continue;

                    bool foundExisting = false;

                    for (int i = Bots.Count - 1; i >= 0; i--)
                    {
                        var botEnt = Bots[i];
                        botEnt.RefreshBotEntity();

                        if (botEnt.Bot == null || botEnt.Closed)
                        {
                            Bots.RemoveAtFast(i);
                            continue;
                        }

                        if (bot.Character != null && botEnt.Bot.EntityId == bot.Character.EntityId && botEnt.Player != null && FactionHelper.IsIdentityNPC(botEnt.Player.IdentityId))
                        {
                            //VRage.Utils.MyLog.Default.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>> BotManager FoundExisting");
                            foundExisting = true;

                            if (botEnt.Player.IdentityId != bot.IdentityId)
                                botEnt.Player = bot;

                            break;
                        }
                    }

                    if (foundExisting)
                        continue;

                    if (FactionHelper.IsIdentityNPC(bot.IdentityId))
                    {
                        //VRage.Utils.MyLog.Default.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>> BotManager FoundNew");
                        new BotEntity(bot.Character);
                    }
                }
            }
        }

        public static string GetActiveBots()
        {
            var sb = new StringBuilder();

            sb.Append("::: All Active Bots :::").AppendLine();
            sb.Append("- Total Bot Count:    ").Append(Bots.Count).AppendLine();
            sb.Append("- MES Bot Count:      ").Append(Bots.Count(x => x.SpawnedByMES)).AppendLine();
            sb.Append("- Non-MES Bot Count:  ").Append(Bots.Count(x => !x.SpawnedByMES)).AppendLine();
            sb.AppendLine();

            var printed = new List<long>();
            foreach (var npc in NpcManager.ActiveNpcs)
            {
                if (npc.Npc.Bots.Count > 0)
                    sb.Append("- " + npc.GridName).AppendLine();

                foreach (var bot in npc.Npc.Bots)
                {
                    bot.GetBotInfo(sb);
                    printed.Add(bot.Entity.EntityId);
                }
            }

            if (printed.Count < Bots.Count)
                sb.Append("- " + "Tracked Non-MES Bots").AppendLine();

            foreach (var b in Bots)
            {
                if (!printed.Contains(b.Entity.EntityId))
                {
                    b.GetBotInfo(sb);
                    printed.Add(b.Entity.EntityId);
                }
            }

            return sb.ToString();
        }
    }
}
