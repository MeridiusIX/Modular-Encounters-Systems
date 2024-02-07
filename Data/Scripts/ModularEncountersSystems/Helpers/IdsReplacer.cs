using ModularEncountersSystems.Logging;
using ModularEncountersSystems.World;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;

namespace ModularEncountersSystems.Helpers {

	public class IdsReplacer {

		public static string ReplaceId(NpcData npcData, string tag)
		{
			if (npcData == null)
				return tag;

			var SpawnGroupName = npcData.SpawnGroupName;
			var Faction = npcData.InitialFaction;

			if (tag.Contains("{SpawnGroupName}") && SpawnGroupName != null)
			{
				tag = tag.Replace("{SpawnGroupName}", SpawnGroupName);
			}

			if (tag.Contains("{Faction}") && Faction != null)
			{
				tag = tag.Replace("{Faction}", Faction);
			}


			return tag;
		}

		public static List<string> ReplaceIds(NpcData npcData, List<string> tags)
		{
			if (npcData == null)
				return tags;

			if (tags.Count < 1)
				return tags;


			List<string> new_tags = new List<string>();

            foreach (var tag in tags)
            {
				var tagnew = ReplaceId(npcData, tag);
				new_tags.Add(tagnew);
			}


			return new_tags;
		}

	}

}
