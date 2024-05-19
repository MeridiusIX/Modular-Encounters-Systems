using ModularEncountersSystems.Helpers;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Profiles
{
	public class FactionIconProfile
	{
		public string ProfileSubtypeId;

		public string Faction;
		public Vector3D Color;
		public Vector3D Background;

		public Dictionary<string, Action<string, object>> EditorReference;


		public FactionIconProfile(string data = null) {

			ProfileSubtypeId = "";
			Faction = "";
			Color = Vector3D.Zero;
			Background = Vector3D.Zero;

			EditorReference = new Dictionary<string, Action<string, object>> {

				{"Faction", (s, o) => TagParse.TagStringCheck(s, ref Faction) },

				{"Color", (s, o) => TagParse.TagVector3DCheck(s, ref Color) },
				{"Background", (s, o) => TagParse.TagVector3DCheck(s, ref Background) },

			};

		}

		public static void ProcessFactionIcons() {

			foreach (var profile in ProfileManager.FactionIconProfiles.Keys)
				ProfileManager.FactionIconProfiles[profile].Process();
		
		}

		public void Process() {

			foreach (var faction in FactionHelper.NpcFactions) {

				if (faction == null || faction.Tag != Faction)
					continue;

				var factionDef = MyDefinitionManager.Static.TryGetFactionDefinition(faction.Tag);

				if (factionDef == null)
					break;

				MyAPIGateway.Session.Factions.EditFaction(faction.FactionId, faction.Tag, faction.Name, faction.Description, faction.PrivateInfo, factionDef.FactionIcon.ToString(), (Vector3)Background, (Vector3)Color);
				break;
			
			}
		
		}

		public void EditValue(string receivedValue)
		{

			var processedTag = TagParse.ProcessTag(receivedValue);

			if (processedTag.Length < 2)
				return;

			Action<string, object> referenceMethod = null;

			if (!EditorReference.TryGetValue(processedTag[0], out referenceMethod))
				//TODO: Notes About Value Not Found
				return;

			referenceMethod?.Invoke(receivedValue, null);

		}

		public void InitTags(string customData)
		{

			if (string.IsNullOrWhiteSpace(customData) == false)
			{

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit)
				{

					EditValue(tag);

				}

			}

		}

	}
}
