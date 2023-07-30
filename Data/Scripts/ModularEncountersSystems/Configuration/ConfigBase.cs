using ModularEncountersSystems.Configuration.Editor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using VRage.Game;

namespace ModularEncountersSystems.Configuration {

	public class PlanetSpawnFilter {

		public string PlanetName;
		public long PlanetId;

		[XmlArrayItem("SpawnGroup")]
		public string[] PlanetSpawnGroupBlacklist;


		public PlanetSpawnFilter() {

			PlanetSpawnGroupBlacklist = new string[] { "AddSpawnGroupIdHere", "AddAnotherSpawnGroupIdHere" };

		}

		public PlanetSpawnFilter(string name, long id) {

			PlanetName = name;
			PlanetId = id;
			PlanetSpawnGroupBlacklist = new string[] { "AddSpawnGroupIdHere", "AddAnotherSpawnGroupIdHere" };
		
		}

	}

	public abstract class ConfigBase {

		public string[] SpawnTypeBlacklist;
		public string[] SpawnTypePlanetBlacklist;

		[XmlArrayItem("PlanetSpawnFilter")]
		public PlanetSpawnFilter[] PlanetSpawnFilters;

		public bool UseTypeDisownTimer;
		public int TypeDisownTimer;

		public bool UseTimeout;
		public double TimeoutRadius;
		public int TimeoutSpawnLimit;
		public int TimeoutDuration;

		public bool UseCleanupSettings;
		public bool OnlyCleanNpcsFromMes;
		public bool CleanupUseDistance;
		public bool CleanupUseTimer;
		public bool CleanupUseBlockLimit;
		public bool CleanupDistanceStartsTimer;
		public bool CleanupResetTimerWithinDistance;
		public double CleanupDistanceTrigger;
		public int CleanupTimerTrigger;
		public int CleanupBlockLimitTrigger;
		public bool CleanupIncludeUnowned;
		public bool CleanupUnpoweredOverride;
		public double CleanupUnpoweredDistanceTrigger;
		public int CleanupUnpoweredTimerTrigger;

		public bool UseBlockDisable;
		public string[] DisableBlocksByType;
		public string[] DisableBlocksByDefinitionId;

		[XmlIgnore]
		public bool ConfigLoaded;

		[XmlIgnore]
		public Dictionary<string, Func<string, object, bool>> EditorBaseReference;

		[XmlIgnore]
		public List<MyDefinitionId> DisableBlocksDefinitionList;

		[XmlIgnore]
		public List<PlanetSpawnFilter> PlanetSpawnFilterList;

		public ConfigBase() {

			SpawnTypeBlacklist = new string[]{ "AddSpawnGroupIdHere", "AddAnotherSpawnGroupIdHere" };
			SpawnTypePlanetBlacklist = new string[] { "AddPlanetIdHere", "AddAnotherPlanetIdHere" };
			PlanetSpawnFilters = new PlanetSpawnFilter[] { };

			UseTimeout = true;
			TimeoutDuration = 900;
			TimeoutRadius = 10000;
			TimeoutSpawnLimit = 4;

			UseCleanupSettings = true;
			OnlyCleanNpcsFromMes = true;
			CleanupUseDistance = true;
			CleanupUseTimer = false;
			CleanupUseBlockLimit = false;

			CleanupDistanceStartsTimer = false;
			CleanupResetTimerWithinDistance = false;
			CleanupDistanceTrigger = 50000;
			CleanupTimerTrigger = 1800;
			CleanupBlockLimitTrigger = 0;
			CleanupIncludeUnowned = true;
			CleanupUnpoweredOverride = true;
			CleanupUnpoweredDistanceTrigger = 25000;
			CleanupUnpoweredTimerTrigger = 900;

			UseBlockDisable = false;
			DisableBlocksByType = new string[] { "ObjectBuilderTypeHere", "AnotherObjectBuilderTypeHere" };
			DisableBlocksByDefinitionId = new string[] { "DefinitionIdHere", "AnotherDefinitionIdHere" };

			ConfigLoaded = false;

			EditorBaseReference = new Dictionary<string, Func<string, object, bool>> {

				{"SpawnTypeBlacklist", (s, o) => EditorTools.SetCommandValueStringArray(s, ref SpawnTypeBlacklist) },
				{"SpawnTypePlanetBlacklist", (s, o) => EditorTools.SetCommandValueStringArray(s, ref SpawnTypePlanetBlacklist) },
				{"PlanetSpawnFilters", (s, o) => EditorTools.SetCommandValuePlanetFilter(s, ref PlanetSpawnFilters) },

				{"UseTimeout", (s, o) => EditorTools.SetCommandValueBool(s, ref UseTimeout) },
				{"TimeoutRadius", (s, o) => EditorTools.SetCommandValueDouble(s, ref TimeoutRadius) },
				{"TimeoutSpawnLimit", (s, o) => EditorTools.SetCommandValueInt(s, ref TimeoutSpawnLimit) },
				{"TimeoutDuration", (s, o) => EditorTools.SetCommandValueInt(s, ref TimeoutDuration) },

				{"UseCleanupSettings", (s, o) => EditorTools.SetCommandValueBool(s, ref UseCleanupSettings) },
				{"OnlyCleanNpcsFromMes", (s, o) => EditorTools.SetCommandValueBool(s, ref OnlyCleanNpcsFromMes) },
				{"CleanupUseDistance", (s, o) => EditorTools.SetCommandValueBool(s, ref CleanupUseDistance) },
				{"CleanupUseTimer", (s, o) => EditorTools.SetCommandValueBool(s, ref CleanupUseTimer) },
				{"CleanupUseBlockLimit", (s, o) => EditorTools.SetCommandValueBool(s, ref CleanupUseBlockLimit) },

				{"CleanupDistanceStartsTimer", (s, o) => EditorTools.SetCommandValueBool(s, ref CleanupDistanceStartsTimer) },
				{"CleanupResetTimerWithinDistance", (s, o) => EditorTools.SetCommandValueBool(s, ref CleanupResetTimerWithinDistance) },
				{"CleanupDistanceTrigger", (s, o) => EditorTools.SetCommandValueDouble(s, ref CleanupDistanceTrigger) },
				{"CleanupTimerTrigger", (s, o) => EditorTools.SetCommandValueInt(s, ref CleanupTimerTrigger) },
				{"CleanupBlockLimitTrigger", (s, o) => EditorTools.SetCommandValueInt(s, ref CleanupBlockLimitTrigger) },
				{"CleanupIncludeUnowned", (s, o) => EditorTools.SetCommandValueBool(s, ref CleanupIncludeUnowned) },
				{"CleanupUnpoweredOverride", (s, o) => EditorTools.SetCommandValueBool(s, ref CleanupUnpoweredOverride) },
				{"CleanupUnpoweredDistanceTrigger", (s, o) => EditorTools.SetCommandValueDouble(s, ref CleanupUnpoweredDistanceTrigger) },
				{"CleanupUnpoweredTimerTrigger", (s, o) => EditorTools.SetCommandValueInt(s, ref CleanupUnpoweredTimerTrigger) },

				{"UseBlockDisable", (s, o) => EditorTools.SetCommandValueBool(s, ref UseBlockDisable) },
				{"DisableBlocksByType", (s, o) => EditorTools.SetCommandValueStringArray(s, ref DisableBlocksByType) },
				{"DisableBlocksByDefinitionId", (s, o) => EditorTools.SetCommandValueStringArray(s, ref DisableBlocksByDefinitionId) }

			};

			PlanetSpawnFilterList = new List<PlanetSpawnFilter>();
			DisableBlocksDefinitionList = new List<MyDefinitionId>();

		}

		public PlanetSpawnFilter GetPlanetSpawnFilter(long entityId) {

			foreach (var filter in PlanetSpawnFilters)
				if (filter.PlanetId == entityId)
					return filter;

			return null;
		
		}

		public void AddPlanetSpawnFilter(PlanetSpawnFilter filter) {

			PlanetSpawnFilterList.Clear();
			PlanetSpawnFilterList.AddArray<PlanetSpawnFilter>(PlanetSpawnFilters);

			for (int i = PlanetSpawnFilterList.Count - 1; i >= 0; i--) {

				var existingFilter = PlanetSpawnFilterList[i];

				if (existingFilter.PlanetId == filter.PlanetId)
					PlanetSpawnFilterList.RemoveAt(i);

			}

			PlanetSpawnFilterList.Add(filter);
			PlanetSpawnFilters = PlanetSpawnFilterList.ToArray();

		}

		public void InitDefinitionDisableList() {

			DisableBlocksDefinitionList.Clear();

			foreach (var def in DisableBlocksByDefinitionId) {

				var defId = new MyDefinitionId();

				if (MyDefinitionId.TryParse(def, out defId))
					DisableBlocksDefinitionList.Add(defId);

			}

		}

		public virtual string SaveSettings() {

			return "";
		
		}

	}

}
