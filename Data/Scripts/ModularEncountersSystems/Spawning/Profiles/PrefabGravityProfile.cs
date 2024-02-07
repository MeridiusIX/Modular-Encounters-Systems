using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class PrefabGravityProfile {

		private List<string> _prefabIds;
		private List<float> _maxGravityAtmo;
		private List<float> _maxGravityVacuum;
		private Dictionary<string, Action<string, object>> EditorReference;

		public static List<string> PrefabIds;
		public static List<float> MaxGravityAtmo;
		public static List<float> MaxGravityVacuum;

		public PrefabGravityProfile() {


			if(PrefabIds == null)
				PrefabIds = new List<string>();

			if(MaxGravityAtmo == null)
				MaxGravityAtmo = new List<float>();

			if(MaxGravityVacuum == null)
				MaxGravityVacuum = new List<float>();

			_prefabIds = new List<string>();
			_maxGravityAtmo = new List<float>();
			_maxGravityVacuum = new List<float>();

			EditorReference = new Dictionary<string, Action<string, object>> {

				{"PrefabIds", (s, o) => TagParse.TagStringListCheck(s, ref _prefabIds) },
				{"MaxGravityAtmo", (s, o) => TagParse.TagFloatCheck(s, ref _maxGravityAtmo) },
				{"MaxGravityVacuum", (s, o) => TagParse.TagFloatCheck(s, ref _maxGravityVacuum) },

			};

		}

		public static bool CheckPrefabGravity(string prefabId, float currentGravity, bool checkVacuum) {

			if (PrefabIds.Count == 0)
				return true;

			for (int i = 0; i < PrefabIds.Count; i++) {

				if (PrefabIds[i] != prefabId)
					continue;

				if (checkVacuum)
					if (currentGravity >= MaxGravityAtmo[i])
						return false;
					else
						if (currentGravity >= MaxGravityVacuum[i])
						return false;

				return true;
			
			}

			return true;
		
		}

		public void InitTags(string customData) {

			if (string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach (var tag in descSplit) {

					EditValue(tag);

				}

			}

			for (int i = 0; i < PrefabIds.Count && i < MaxGravityAtmo.Count && i < MaxGravityVacuum.Count; i++) {

				if (PrefabIds.Contains(_prefabIds[i])) {

					continue;
				
				}

				PrefabIds.Add(_prefabIds[i]);
				MaxGravityAtmo.Add(_maxGravityAtmo[i]);
				MaxGravityVacuum.Add(_maxGravityVacuum[i]);
			
			}

		}

		private void EditValue(string receivedValue) {

			var processedTag = TagParse.ProcessTag(receivedValue);

			if (processedTag.Length < 2)
				return;

			Action<string, object> referenceMethod = null;

			if (!EditorReference.TryGetValue(processedTag[0], out referenceMethod))
				//TODO: Notes About Value Not Found
				return;

			referenceMethod?.Invoke(receivedValue, null);

		}

	}

}
