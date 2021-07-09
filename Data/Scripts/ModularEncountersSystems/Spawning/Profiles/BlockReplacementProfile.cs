using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class BlockReplacementProfile {

		public List<MyDefinitionId> OldBlock;
		public List<MyDefinitionId> NewBlock;

		public Dictionary<MyDefinitionId, MyDefinitionId> Replacement;

		public BlockReplacementProfile(string data = null) {

			OldBlock = new List<MyDefinitionId>();
			NewBlock = new List<MyDefinitionId>();

			InitTags(data);

			Replacement = new Dictionary<MyDefinitionId, MyDefinitionId>();

			if (OldBlock.Count == 0 || NewBlock.Count == 0)
				return;

			int count = OldBlock.Count <= NewBlock.Count ? OldBlock.Count : NewBlock.Count;

			for (int i = 0; i < count; i++) {

				if (!Replacement.ContainsKey(OldBlock[i]))
					Replacement.Add(OldBlock[i], NewBlock[i]);

			}

		}

		public void InitTags(string data) {

			if (string.IsNullOrWhiteSpace(data))
				return;

			var descSplit = data.Split('\n');

			foreach (var tag in descSplit) {

				if (tag.Contains("[OldBlock:")) {

					TagParse.TagMyDefIdCheck(tag, ref OldBlock);
					continue;

				}

				if (tag.Contains("[MatchOnlyTypeId:")) {

					TagParse.TagMyDefIdCheck(tag, ref NewBlock);
					continue;

				}

			}

		}

	}

}
