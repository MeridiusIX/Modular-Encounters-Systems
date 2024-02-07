using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Events;
using ModularEncountersSystems.Events.Condition;
using ModularEncountersSystems.Files;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Profiles;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace ModularEncountersSystems.Helpers {
	public static class TagParse {

		public static string[] ProcessTag(string tag) {

			var thisTag = tag.Trim();

			if (thisTag.Length > 0 && thisTag[0] == '[')
				thisTag = thisTag.Remove(0, 1);

			if (thisTag.Length > 0 && thisTag[thisTag.Length - 1] == ']')
				thisTag = thisTag.Remove(thisTag.Length - 1, 1);

			var tagSplit = thisTag.Split(':');
			string a = "";
			string b = "";

			if (tagSplit.Length > 2) {

				a = tagSplit[0];

				for (int i = 1; i < tagSplit.Length; i++) {

					b += tagSplit[i];

					if (i != tagSplit.Length - 1) {

						b += ":";

					}

				}

				tagSplit = new string[] { a, b };
				//Logger.Write("MultipColonSplit - " + b, true);

			}

			return tagSplit;

		}

		public static string FixVectorString(string source) {

			string newString = source;

			if (newString.Length == 0)
				return source;

			if (newString[0] == '{')
				newString = newString.Remove(0, 1);

			if (newString.Length == 0)
				return source;

			if (newString[newString.Length - 1] == '}')
				newString = newString.Remove(newString.Length - 1, 1);

			return newString;

		}

		public static void TagActionExecutionCheck(string tag, ref ActionExecutionEnum original) {

			ActionExecutionEnum result = ActionExecutionEnum.All;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (ActionExecutionEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagAutoPilotProfileModeCheck(string tag, ref AutoPilotDataMode original) {

			AutoPilotDataMode result = AutoPilotDataMode.Primary;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (AutoPilotDataMode.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagBehaviorModeEnumCheck(string tag, ref BehaviorMode original) {

			BehaviorMode result = BehaviorMode.Init;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (BehaviorMode.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagBehaviorModeEnumCheck(string tag, ref List<BehaviorMode> original) {

			BehaviorMode result = BehaviorMode.Init;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (BehaviorMode.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original.Add(result);

		}

		public static void TagBehaviorSubclassEnumCheck(string tag, ref BehaviorSubclass original) {

			BehaviorSubclass result = BehaviorSubclass.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (BehaviorSubclass.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagBehaviorSubclassEnumCheck(string tag, ref List<BehaviorSubclass> original) {

			BehaviorSubclass result = BehaviorSubclass.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (BehaviorSubclass.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original.Add(result);

		}

		public static void TagBlockSizeEnumCheck(string tag, ref BlockSizeEnum original) {

			BlockSizeEnum result = BlockSizeEnum.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (BlockSizeEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagBlockTargetTypesCheck(string tag, ref List<BlockTypeEnum> orginal) {

			BlockTypeEnum result = BlockTypeEnum.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (BlockTypeEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			orginal.Add(result);

		}

		public static void TagBoolCheck(string tag, ref bool original) {

			bool result = false;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (bool.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original = result;

		}

		public static void TagBoolEnumCheck(string tag, ref BoolEnum original) {

			BoolEnum result = BoolEnum.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				BoolEnum.TryParse(tagSplit[1], true, out result);

			}

			original = result;

		}

		public static void TagBotProfileListCheck(string tag, ref List<BotSpawnProfile> original) {

			var tagSplit = ProcessTag(tag);
			BotSpawnProfile profile = null;

			if (tagSplit.Length == 2) {

				if (!ProfileManager.BotSpawnProfiles.TryGetValue(tagSplit[1], out profile)) {

					return;

				}

			} else {

				return;

			}

			original.Add(profile);

		}

		public static void TagBroadcastTypeEnumCheck(string tag, ref List<BroadcastType> original) {

			BroadcastType result = BroadcastType.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (BroadcastType.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original.Add(result);

		}

		public static void TagCheckEnumCheck(string tag, ref CheckEnum original) {

			CheckEnum result = CheckEnum.Ignore;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (CheckEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagCommandTransmissionTypeEnumCheck(string tag, ref CommandTransmissionType original) {

			CommandTransmissionType result = CommandTransmissionType.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (CommandTransmissionType.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagCounterCompareEnumCheck(string tag, ref CounterCompareEnum original) {

			CounterCompareEnum result = CounterCompareEnum.GreaterOrEqual;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (CounterCompareEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagCounterCompareEnumCheck(string tag, ref List<CounterCompareEnum> original) {

			CounterCompareEnum result = CounterCompareEnum.GreaterOrEqual;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (CounterCompareEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original.Add(result);

		}
		/*
		public static void TagDayOfWeekEnumCheck(string tag, ref List<DayOfWeek> original) {

			DayOfWeek result = DayOfWeek.Monday;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (DayOfWeek.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original.Add(result);

		}
		*/

		public static void TagThreatScoreTypeEnumCheck(string tag, ref ThreatScoreTypeEnum original)
		{

			ThreatScoreTypeEnum result = ThreatScoreTypeEnum.Player;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2)
			{

				if (ThreatScoreTypeEnum.TryParse(tagSplit[1], out result) == false)
				{

					return;

				}

			}

			original = result;

		}


		public static void TagDirectionEnumCheck(string tag, ref Direction original) {

			Direction result = Direction.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (Direction.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagEventProfileCheck(string tag, ref List<EventProfile> original) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length < 2) {
				return;
			}

			var key = tagSplit[1];
			EventProfile result = null;

			if (ProfileManager.EventProfiles.TryGetValue(key, out result))
				original.Add(result);

		}

		public static void TagFloatCheck(string tag, ref float original) {

			float result = 0;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (float.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original = result;

		}

		public static void TagFloatCheck(string tag, ref List<float> original) {

			float result = 0;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (float.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original.Add(result);

		}

		public static void TagDoubleCheck(string tag, ref double original) {

			double result = 0;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (double.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original = result;

		}

		public static void TagDoubleCheck(string tag, ref List<double> original)
		{

			double result = 0;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2)
			{

				if (double.TryParse(tagSplit[1], out result) == false)
				{

					return;

				}

			}
			else
			{

				return;

			}

			original.Add(result);

		}



		public static void TagIntCheck(string tag, ref int original) {

			int result = 0;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (int.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original = result;

		}

		public static void TagIntOrDayCheck(string tag, ref int original) {

			int result = 0;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (tagSplit[1] == "Day") {

					original = (int)MyAPIGateway.Session.SessionSettings.SunRotationIntervalMinutes;
					return;

				}

				if (int.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original = result;

		}

		public static void TagIntListCheck(string tag, ref List<int> result) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				var array = tagSplit[1].Split(',');

				foreach (var item in array) {

					if (string.IsNullOrWhiteSpace(item))
						continue;

					int number = 0;

					if (int.TryParse(item, out number) == false) {

						continue;

					}
		
					result.Add(number);

				}

			}

			result.RemoveAll(item => item == 0);

		}

		public static void TagIntListCheck(string tag, bool preserveZero, ref List<int> result) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				var array = tagSplit[1].Split(',');

				foreach (var item in array) {

					if (string.IsNullOrWhiteSpace(item))
						continue;

					int number = 0;

					if (int.TryParse(item, out number) == false) {

						continue;

					}

					result.Add(number);

				}

			}

			//result.RemoveAll(item => item == 0);

		}

		public static void TagLongCheck(string tag, ref long original) {

			long result = 0;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (long.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original = result;

		}

		public static void TagLongCheck(string tag, ref List<long> original) {

			long result = 0;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (long.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original.Add(result);

		}

		public static void TagMyDefIdCheck(string tag, ref MyDefinitionId original) {

			MyDefinitionId result = new MyDefinitionId();
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (MyDefinitionId.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original = result;

		}

		public static void TagMyDefIdCheck(string tag, ref List<MyDefinitionId> original) {

			MyDefinitionId result = new MyDefinitionId();
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (MyDefinitionId.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original.Add(result);

		}

		public static void TagMyDefIdCheck(string tag, ref List<SerializableDefinitionId> original) {

			MyDefinitionId result = new MyDefinitionId();
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (MyDefinitionId.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original.Add(result);

		}

		public static void TagReplenishProfileCheck(string tag, ref List<ReplenishmentProfile> original) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length < 2) {

				return;

			}

			var key = tagSplit[1];
			ReplenishmentProfile result = null;

			if (ProfileManager.ReplenishmentProfiles.TryGetValue(key, out result))
				original.Add(result);

		}

		public static void TagLootProfileCheck(string tag, ref List<LootProfile> original) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length < 2) {

				return;

			}

			var key = tagSplit[1];
			LootProfile result = null;

			if (ProfileManager.LootProfiles.TryGetValue(key, out result))
				original.Add(result);

		}

		public static void TagManipulationProfileCheck(string tag, ref ManipulationProfile original) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length < 2) {

				return;

			}

			var key = tagSplit[1];
			ManipulationProfile result = null;

			if (ProfileManager.ManipulationProfiles.TryGetValue(key, out result))
				original = result;

		}

		public static void TagManipulationProfileCheck(string tag, ref List<ManipulationProfile> original) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length < 2) {

				return;

			}

			var key = tagSplit[1];
			ManipulationProfile result = null;

			if (ProfileManager.ManipulationProfiles.TryGetValue(key, out result))
				original.Add(result);

		}

		public static void TagPrefabSpawnModeEnumCheck(string tag, ref PrefabSpawnMode original) {

			PrefabSpawnMode result = PrefabSpawnMode.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (PrefabSpawnMode.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original = result;

		}

		public static void TagGuidCheck(string tag, ref Guid original) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				var temp = tagSplit[1];
				try {

					var guid = new Guid(temp);
					original = guid;

				} catch (Exception e) {

					return;

				}

			}

		}

		//TagModifierEnumCheck
		public static void TagModifierEnumCheck(string tag, ref ModifierEnum original) {

			ModifierEnum result = ModifierEnum.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (ModifierEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagModifierEnumCheck(string tag, ref List<ModifierEnum> original) {

			ModifierEnum result = ModifierEnum.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (ModifierEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original.Add(result);

		}

		public static void TagRelativeEntityEnumCheck(string tag, ref RelativeEntityType original) {

			RelativeEntityType result = RelativeEntityType.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (WaypointType.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagSafeZoneShapeEnumCheck(string tag, ref MySafeZoneShape original) {

			MySafeZoneShape result = MySafeZoneShape.Sphere;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (MySafeZoneShape.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagSafeZoneAccessTypeEnumCheck(string tag, ref SafeZoneAccessType original) {

			SafeZoneAccessType result = SafeZoneAccessType.Blacklist;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (SafeZoneAccessType.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagSafeZoneActionEnumCheck(string tag, ref SafeZoneAction original) {

			SafeZoneAction result = SafeZoneAction.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (SafeZoneAction.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original |= result;

		}

		public static void TagSpawnConditionsProfileCheck(string tag, ref SpawnConditionsProfile original) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length < 2) {

				return;

			}

			var key = tagSplit[1];
			SpawnConditionsProfile result = null;

			if (ProfileManager.SpawnConditionProfiles.TryGetValue(key, out result))
				original = result;

		}

		public static void TagSpawnConditionsProfileCheck(string tag, ref List<SpawnConditionsProfile> original) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length < 2) {

				return;

			}

			var key = tagSplit[1];
			SpawnConditionsProfile result = null;

			if (ProfileManager.SpawnConditionProfiles.TryGetValue(key, out result))
				original.Add(result);

		}

		public static void TagSpawnTypeEnumCheck(string tag, ref SpawnTypeEnum original) {

			SpawnTypeEnum result = SpawnTypeEnum.CustomSpawn;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (SpawnTypeEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagSpawningTypeEnumCheck(string tag, ref SpawningType original) {

			SpawningType result = SpawningType.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (SpawningType.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original = result;

		}

		public static void TagSpawningTypeEnumCheck(string tag, ref List<SpawningType> original) {

			SpawningType result = SpawningType.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (SpawningType.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original.Add(result);

		}

		public static void TagStringCheck(string tag, ref string original) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				original = tagSplit[1];

			}

		}

		public static void TagStringListCheck(string tag, ref List<string> result) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				var array = tagSplit[1].Split(',');

				foreach (var item in array) {

					if (string.IsNullOrWhiteSpace(item)) {

						continue;

					}

					result.Add(item);

				}

			}

		}

		public static void TagStringListCheck(string tag, bool useCommaSplit, ref List<string> result) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (useCommaSplit) {

					var array = tagSplit[1].Split(',');

					foreach (var item in array) {

						if (string.IsNullOrWhiteSpace(item)) {

							continue;

						}

						result.Add(item);

					}

				} else {

					if (string.IsNullOrWhiteSpace(tagSplit[1])) {

						return;

					}

					result.Add(tagSplit[1]);

				}

			}

		}

		public static void TagSwitchEnumCheck(string tag, ref SwitchEnum original) {

			SwitchEnum result = SwitchEnum.Off;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (SwitchEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagSwitchEnumCheck(string tag, ref List<SwitchEnum> original) {

			SwitchEnum result = SwitchEnum.Off;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (SwitchEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original.Add(result);

		}

		public static void TagGridConfigurationCheck(string tag, ref GridConfigurationEnum original)
		{

			GridConfigurationEnum result = GridConfigurationEnum.All;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2)
			{

				if (GridConfigurationEnum.TryParse(tagSplit[1], out result) == false)
				{

					return;

				}

			}

			original = result;


		}


		public static void TagTargetFilterEnumCheck(string tag, ref List<TargetFilterEnum> original) {

			TargetFilterEnum result = TargetFilterEnum.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (TargetFilterEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original.Add(result);

		}

		public static void TagTargetObstructionEnumCheck(string tag, ref TargetObstructionEnum original) {

			TargetObstructionEnum result = TargetObstructionEnum.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (TargetObstructionEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagTargetOwnerEnumCheck(string tag, ref OwnerTypeEnum original) {

			OwnerTypeEnum result = OwnerTypeEnum.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (OwnerTypeEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			if(!original.HasFlag(result))
				original |= result;

		}

		public static void TagTargetRelationEnumCheck(string tag, ref RelationTypeEnum original) {

			RelationTypeEnum result = RelationTypeEnum.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (RelationTypeEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			if (!original.HasFlag(result))
				original |= result;

		}

		public static void TagTargetSortEnumCheck(string tag, ref TargetSortEnum original) {

			TargetSortEnum result = TargetSortEnum.Random;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (TargetSortEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagTargetTypeEnumCheck(string tag, ref TargetTypeEnum original) {

			TargetTypeEnum result = TargetTypeEnum.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (TargetTypeEnum.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagTextTemplateCheck(string tag, ref TextTemplate original) {

			TextTemplate result = null;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				result = ProfileManager.GetTextTemplate(tagSplit[1]);

				if (result == null) {

					return;

				}

			}

			original = result;

		}

		public static void TagTextTemplateCheck(string tag, ref List<TextTemplate> original) {

			TextTemplate result = null;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				result = ProfileManager.GetTextTemplate(tagSplit[1]);

				if (result == null) {

					return;

				}

			}

			original.Add(result);

		}

		public static void TagMDIDictionaryCheck(string tag, ref Dictionary<MyDefinitionId, MyDefinitionId> result) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				var array = tagSplit[1].Split(',');

				foreach (var item in array) {

					if (string.IsNullOrWhiteSpace(item)) {

						continue;

					}

					var secondSplit = item.Split('|');

					var targetId = new MyDefinitionId();
					var newId = new MyDefinitionId();

					if (secondSplit.Length == 2) {

						MyDefinitionId.TryParse(secondSplit[0], out targetId);
						MyDefinitionId.TryParse(secondSplit[1], out newId);

					}

					if (targetId != new MyDefinitionId() && newId != new MyDefinitionId() && result.ContainsKey(targetId) == false) {

						result.Add(targetId, newId);

					}

				}

			}

		}

		public static void TagShortCheck(string tag, ref short original) {

			short result = 0;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (short.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original = result;

		}

		public static void TagStringDictionaryCheck(string tag, ref Dictionary<string, string> result) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				var array = tagSplit[1].Split(',');

				foreach (var item in array) {

					if (string.IsNullOrWhiteSpace(item)) {

						continue;

					}

					var secondSplit = item.Split('|');

					string key = secondSplit[0];
					string val = secondSplit[1];

					if (string.IsNullOrWhiteSpace(key) == false && string.IsNullOrWhiteSpace(val) == false && result.ContainsKey(val) == false) {

						result.Add(key, val);

					}

				}

			}

		}

		public static void TagUintCheck(string tag, ref uint original) {

			uint result = 0;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (uint.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			} else {

				return;

			}

			original = result;

		}

		public static void TagVector3DictionaryCheck(string tag, ref Dictionary<Vector3, Vector3> result) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				var array = tagSplit[1].Split(',');

				foreach (var item in array) {

					if (string.IsNullOrWhiteSpace(item)) {

						continue;

					}

					var secondSplit = item.Split('|');

					string key = secondSplit[0];
					string val = secondSplit[1];

					key = FixVectorString(key);
					val = FixVectorString(val);

					if (string.IsNullOrWhiteSpace(key) == true || string.IsNullOrWhiteSpace(val) == true) {

						continue;

					}

					Vector3D keyVector = Vector3D.Zero;
					Vector3D valVector = Vector3D.Zero;

					if (Vector3D.TryParse(key, out keyVector) == false || Vector3D.TryParse(val, out valVector) == false) {

						continue;

					}

					result.Add((Vector3)keyVector, (Vector3)valVector);

				}

			}

		}

		public static void TagVector3Check(string tag, ref Vector3 original) {

			Vector3D result = Vector3D.Zero;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (Vector3D.TryParse(FixVectorString(tagSplit[1]), out result) == false) {

					return;

				}

			} else {

				return;

			}

			original = ((Vector3)result);

		}

		public static void TagVector3Check(string tag, ref List<Vector3> original) {

			Vector3D result = Vector3D.Zero;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (Vector3D.TryParse(FixVectorString(tagSplit[1]), out result) == false) {

					return;

				}

			} else {

				return;

			}

			original.Add((Vector3)result);

		}

		public static void TagVector3DCheck(string tag, ref Vector3D original) {

			Vector3D result = Vector3D.Zero;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (Vector3D.TryParse(FixVectorString(tagSplit[1]), out result) == false) {

					return;

				}

			} else {

				return;

			}

			original = result;

		}

		public static void TagVector3ICheck(string tag, ref Vector3I original) {

			Vector3D result = Vector3D.Zero;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (Vector3D.TryParse(FixVectorString(tagSplit[1]), out result) == false) {

					return;

				}

			} else {

				return;

			}

			original = (Vector3I)result;

		}

		public static void TagBoolListCheck(string tag, ref List<bool> result) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				var array = tagSplit[1].Split(',');

				foreach (var item in array) {

					if (string.IsNullOrWhiteSpace(item)) {

						continue;

					}

					bool entry = false;

					if (bool.TryParse(item, out entry) == false) {

						continue;

					}

					result.Add(entry);

				}

			}

		}

		public static void TagVector3DListCheck(string tag, ref List<Vector3D> result) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				var array = tagSplit[1].Split(',');

				foreach (var item in array) {

					if (string.IsNullOrWhiteSpace(item)) {

						continue;

					}

					Vector3D entry = Vector3D.Zero;

					if (Vector3D.TryParse(FixVectorString(item), out entry) == false) {

						continue;

					}

					result.Add(entry);

				}

			}

		}

		public static void TagVector3StringDictionaryCheck(string tag, ref Dictionary<Vector3, string> result) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				var array = tagSplit[1].Split(',');

				foreach (var item in array) {

					if (string.IsNullOrWhiteSpace(item)) {

						continue;

					}

					var secondSplit = item.Split('|');

					string key = secondSplit[0];
					key = FixVectorString(key);
					string val = secondSplit[1];

					if (string.IsNullOrWhiteSpace(key) == true || string.IsNullOrWhiteSpace(val) == true) {

						continue;

					}

					Vector3D keyVector = Vector3D.Zero;

					if (Vector3D.TryParse(key, out keyVector) == false) {

						continue;

					}

					result.Add((Vector3)keyVector, val);

				}

			}

		}

		public static void TagUlongListCheck(string tag, ref List<ulong> result) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				var array = tagSplit[1].Split(',');

				foreach (var item in array) {

					if (string.IsNullOrWhiteSpace(item))
						continue;

					ulong modId = 0;

					if (ulong.TryParse(item, out modId) == false)
						continue;

					result.Add(modId);

				}

			}

			result.RemoveAll(item => item == 0);

		}

		public static void TagWaypointTypeEnumCheck(string tag, ref WaypointType original) {

			WaypointType result = WaypointType.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (WaypointType.TryParse(tagSplit[1], out result) == false) {

					return;

				}

			}

			original = result;

		}

		public static void TagZoneConditionsProfileCheck(string tag, ref List<ZoneConditionsProfile> original) {

			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				var key = tagSplit[1];
				ZoneConditionsProfile result = null;

				if (ProfileManager.ZoneConditionsProfiles.TryGetValue(key, out result))
					original.Add(result);

			}

		}


		/* CPT shenanigans
		public static void TagObjectListCheck(string tag, ref List<object> original)
		{

			string input ="";


			bool dummybool = false;
			string dummystring = "";
			int dummyint = 0;
			Vector3D dummyVector3D = new Vector3D(0,0,0);


			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2)
			{
				input = tagSplit[1];

			}

			string[] parts = input.Split('@');

			string type = parts[0].ToLower();   // "bool"
			string value = "[sillyme:" + parts[1] + "]";  // "false"

			if (type.Contains("bool"))
			{
				TagBoolCheck(value, ref dummybool);
				original.Add((bool)dummybool);

				return;
			}

			if (type == "string")
			{
				TagStringCheck(value, ref dummystring);
				original.Add((string)dummystring);
				return;
			}

			if (type == "int")
			{
				TagIntCheck(value, ref dummyint);
				original.Add((int)dummyint);
				return;
			}

			if (type == "vector3d")
			{
				TagVector3DCheck(value, ref dummyVector3D);
				original.Add((Vector3D)dummyVector3D);
				return;
			}


		}

		*/


	}

}
