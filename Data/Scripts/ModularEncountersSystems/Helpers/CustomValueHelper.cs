using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Helpers {
	public static class CustomValueHelper {

		public static void ChangeCustomCounters(Dictionary<string, int> counters, List<string> counterNames, List<int> counterValues, List<ModifierEnum> counterModifiers) {

			var count = MathTools.LowestCount(counterNames.Count, counterValues.Count, counterModifiers.Count);

			for (int i = 0; i < count; i++) {

				int existing = 0;
				bool gotValue = counters.TryGetValue(counterNames[i], out existing);
				MathTools.ApplyModifier(counterValues[i], counterModifiers[i], ref existing);

				if (gotValue)
					counters[counterNames[i]] = existing;
				else
					counters.Add(counterNames[i], existing);

			}
		
		}

		public static void ChangeCustomCounters(Dictionary<string, long> counters, List<string> counterNames, List<long> counterValues, List<ModifierEnum> counterModifiers) {

			var count = MathTools.LowestCount(counterNames.Count, counterValues.Count, counterModifiers.Count);

			for (int i = 0; i < count; i++) {

				long existing = 0;
				bool gotValue = counters.TryGetValue(counterNames[i], out existing);
				MathTools.ApplyModifier(counterValues[i], counterModifiers[i], ref existing);

				if (gotValue)
					counters[counterNames[i]] = existing;
				else
					counters.Add(counterNames[i], existing);

			}

		}

		public static void ChangeCustomBools(Dictionary<string, bool> counters, List<string> counterNames, List<bool> counterValues) {

			var count = MathTools.LowestCount(counterNames.Count, counterValues.Count);

			for (int i = 0; i < count; i++) {

				bool existing = false;
				bool gotValue = counters.TryGetValue(counterNames[i], out existing);
				existing = counterValues[i];

				if (gotValue)
					counters[counterNames[i]] = existing;
				else
					counters.Add(counterNames[i], existing);

			}

		}

	}
}
