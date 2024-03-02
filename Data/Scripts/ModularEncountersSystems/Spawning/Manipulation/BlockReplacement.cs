using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Profiles;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.ObjectBuilders;

namespace ModularEncountersSystems.Spawning.Manipulation {
	public static class BlockReplacement {

		public static Dictionary<string, BlockReplacementProfile> BlockReplacementProfiles = new Dictionary<string, BlockReplacementProfile>();

		public static void ApplyBlockReplacements(MyObjectBuilder_CubeGrid cubeGrid, string profileName = "", Dictionary<MyDefinitionId, MyDefinitionId> referenceDict = null, Dictionary<MyDefinitionId, int> referenceCountDict = null, bool alwaysRemove = false, bool relaxedSize = false) {

			List<MyDefinitionId> unusedDefinitions = new List<MyDefinitionId>();

			var reference = referenceDict;
			Dictionary<MyDefinitionId, int> countLimitsReference = null;
			BlockReplacementProfile profile = null;

			if (reference == null) {

				if (!string.IsNullOrWhiteSpace(profileName)) {

					if (!ProfileManager.BlockReplacementProfiles.TryGetValue(profileName, out profile)) {

						SpawnLogger.Write(" - Could not get Block Replacement Profile with name: " + profileName, SpawnerDebugEnum.Manipulation);
						return;

					} else {

						SpawnLogger.Write(" - Got Block Replacement Profile with name: " + profileName, SpawnerDebugEnum.Manipulation);
						reference = profile.Replacement;
						countLimitsReference = profile.CountReplacement;

					}
						

				} else {

					SpawnLogger.Write(" - Provided Block Replacement Profile Null or Blank", SpawnerDebugEnum.Manipulation);
					return;
				
				}
			
			}

			if (countLimitsReference != null)
				RandomizeBlockIndexes(cubeGrid);

			for (int i = cubeGrid.CubeBlocks.Count - 1; i >= 0; i--) {

				var block = cubeGrid.CubeBlocks[i];
				var id = block.GetId();

				if (unusedDefinitions.Contains(id))
					continue;

				var replaceId = new MyDefinitionId();

				if (!reference.TryGetValue(id, out replaceId)) {

					unusedDefinitions.Add(id);
					continue;

				}

				var targetBlockDef = MyDefinitionManager.Static.GetCubeBlockDefinition(id);
				var newBlockDef = MyDefinitionManager.Static.GetCubeBlockDefinition(replaceId);

				if (targetBlockDef == null)
					continue;

				if (newBlockDef == null) {

					if(alwaysRemove)
						cubeGrid.CubeBlocks.Remove(block);

					continue;

				}

				if (!relaxedSize && targetBlockDef.Size != newBlockDef.Size)
					continue;

				var newBlockBuilder = MyObjectBuilderSerializer.CreateNewObject(newBlockDef.Id) as MyObjectBuilder_CubeBlock;

				if (newBlockBuilder == null)
					continue;

				if (!CountReferenceCheck(replaceId, countLimitsReference, referenceCountDict))
					continue;

				if (block.GetId().TypeId == newBlockBuilder.GetId().TypeId) {

					block.SubtypeName = newBlockBuilder.SubtypeName;
					SpecialBlockSettings(newBlockDef, block);
					CountReferenceApply(replaceId, countLimitsReference, referenceCountDict);
					continue;

				}

				if (id.TypeId == typeof(MyObjectBuilder_Beacon) && replaceId.TypeId == typeof(MyObjectBuilder_RadioAntenna)) {

					(newBlockBuilder as MyObjectBuilder_TerminalBlock).CustomName = (block as MyObjectBuilder_TerminalBlock).CustomName;
					(newBlockBuilder as MyObjectBuilder_RadioAntenna).BroadcastRadius = (block as MyObjectBuilder_Beacon).BroadcastRadius;

				}

				if (id.TypeId == typeof(MyObjectBuilder_RadioAntenna) && replaceId.TypeId == typeof(MyObjectBuilder_Beacon)) {

					(newBlockBuilder as MyObjectBuilder_TerminalBlock).CustomName = (block as MyObjectBuilder_TerminalBlock).CustomName;
					(newBlockBuilder as MyObjectBuilder_Beacon).BroadcastRadius = (block as MyObjectBuilder_RadioAntenna).BroadcastRadius;

				}

				newBlockBuilder.BlockOrientation = block.BlockOrientation;
				newBlockBuilder.Min = block.Min;
				newBlockBuilder.ColorMaskHSV = block.ColorMaskHSV;
				newBlockBuilder.Owner = block.Owner;
				newBlockBuilder.EntityId = block.EntityId;

				SpecialBlockSettings(newBlockDef, newBlockBuilder);

				cubeGrid.CubeBlocks.RemoveAt(i);
				cubeGrid.CubeBlocks.Add(newBlockBuilder);
				CountReferenceApply(replaceId, countLimitsReference, referenceCountDict);

			}

		}

		public static bool CountReferenceCheck(MyDefinitionId newId, Dictionary<MyDefinitionId, int> profileLimits, Dictionary<MyDefinitionId, int> existingCounts) {

			if (profileLimits == null || existingCounts == null)
				return true;

			int exist = 0;
			int limit = 0;

			if(!profileLimits.TryGetValue(newId, out limit) || limit == -1)
				return true;

			existingCounts.TryGetValue(newId, out exist);
				



			return exist < limit;

		}

		public static void CountReferenceApply(MyDefinitionId newId, Dictionary<MyDefinitionId, int> profileLimits, Dictionary<MyDefinitionId, int> existingCounts) {

			if (profileLimits == null || existingCounts == null)
				return;

			int exist = 0;
			int limit = 0;

			if (!profileLimits.TryGetValue(newId, out limit) || limit == -1)
				return;

			if (existingCounts.TryGetValue(newId, out exist))
				existingCounts[newId] += 1;
			else
				existingCounts.Add(newId, 1);

		}

		public static void SpecialBlockSettings(MyCubeBlockDefinition def, MyObjectBuilder_CubeBlock block) {

			//Set NPC Interior Turret To Maximum Range
			if (def.Id.SubtypeName == "NpcLargeInteriorTurret") {

				var turret = block as MyObjectBuilder_TurretBase;

				if (turret != null) {

					turret.Range = 800;

				}
			
			}
		
		}

		public static void RandomizeBlockIndexes(MyObjectBuilder_CubeGrid grid) {

			var increment = 0;
			var count = grid.CubeBlocks.Count;

			while (increment < grid.CubeBlocks.Count) {

				var randIndex = MathTools.RandomBetween(increment, count);
				var block = grid.CubeBlocks[randIndex];
				grid.CubeBlocks.RemoveAt(randIndex);
				grid.CubeBlocks.Insert(0, block);
				increment++;

			}
		
		}

	}

}
