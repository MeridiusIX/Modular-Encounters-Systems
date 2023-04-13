using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace ModularEncountersSystems.Helpers {
	public static class CollectionHelper {

		public static MyDefinitionId? GetRandomIdFromList(List<MyDefinitionId> list, List<MyDefinitionId> exclusion) {

			var index = MathTools.RandomBetween(0, list.Count);

			if (exclusion == null || !exclusion.Contains(list[index]))
				return list[index];

			int lower = index;
			int upper = index;

			while (lower >= 0 || upper < list.Count) {

				lower--;
				upper++;

				if (MathTools.RandomBool()) {

					if (lower >= 0) {

						if (!exclusion.Contains(list[lower]))
							return list[lower];

					}

					if (upper < list.Count) {

						if (!exclusion.Contains(list[upper]))
							return list[upper];

					}

				} else {

					if (upper < list.Count) {

						if (!exclusion.Contains(list[upper]))
							return list[upper];

					}

					if (lower >= 0) {

						if (!exclusion.Contains(list[lower]))
							return list[lower];

					}

				}

			}

			return null;

		}

	}
}
