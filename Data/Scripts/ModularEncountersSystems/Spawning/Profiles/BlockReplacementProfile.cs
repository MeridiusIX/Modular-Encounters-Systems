using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class BlockReplacementProfile {

		public List<MyDefinitionId> OldBlock;
		public List<MyDefinitionId> NewBlock;
		public List<int> Limit;

		public Dictionary<MyDefinitionId, MyDefinitionId> Replacement;
		public Dictionary<MyDefinitionId, int> CountReplacement;

		public BlockReplacementProfile(string data = null) {

			OldBlock = new List<MyDefinitionId>();
			NewBlock = new List<MyDefinitionId>();
			Limit = new List<int>();

			InitTags(data);

			Replacement = new Dictionary<MyDefinitionId, MyDefinitionId>();
			CountReplacement = new Dictionary<MyDefinitionId, int>();

			if (OldBlock.Count == 0 || NewBlock.Count == 0)
				return;

			int count = OldBlock.Count <= NewBlock.Count ? OldBlock.Count : NewBlock.Count;

			for (int i = 0; i < OldBlock.Count && i < NewBlock.Count; i++) {

				if (!Replacement.ContainsKey(OldBlock[i]))
					Replacement.Add(OldBlock[i], NewBlock[i]);

			}

			for (int i = 0; i < NewBlock.Count && i < Limit.Count; i++) {

				if (!CountReplacement.ContainsKey(NewBlock[i]))
					CountReplacement.Add(NewBlock[i], Limit[i]);

			}

			if (CountReplacement.Count == 0)
				CountReplacement = null;

		}

		public void InitTags(string data) {

			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			foreach (var tag in descSplit) {

				//OldBlock
				if (tag.Contains("[OldBlock:")) {

					TagParse.TagMyDefIdCheck(tag, ref OldBlock);
					continue;

				}

				//NewBlock
				if (tag.Contains("[NewBlock:")) {

					TagParse.TagMyDefIdCheck(tag, ref NewBlock);
					continue;

				}

				//Limit
				if (tag.Contains("[Limit:")) {

					TagParse.TagIntListCheck(tag, ref Limit);
					continue;

				}

			}

		}

	}

}
