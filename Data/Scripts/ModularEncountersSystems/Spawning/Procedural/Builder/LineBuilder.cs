using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Procedural.Builder {

	public static class LineBuilder {

		public static Dictionary<string, LinePattern> LinePatterns = new Dictionary<string, LinePattern>();

		public static void Setup() {

			LinePatterns.Add("TopSingleSlopeLine", new LinePattern("TopSingleSlopeLine"));
			LinePatterns.Add("TopSlopeCornerLine", new LinePattern("TopSlopeCornerLine"));

			LinePatterns.Add("TopRampLine", new LinePattern("TopRampLine")); 
			LinePatterns.Add("TopRampCornerLine", new LinePattern("TopRampCornerLine"));

		}

		public static void BuildStraightArmorLine(ShipConstruct construct, BlockCategory category, Vector3I block, int steps, Vector3I increment, Vector3I endIncrement, bool xsymm, bool ysymm, ref Vector3I currentPosition) {

			currentPosition = BuildLine(construct, category, block, increment, steps, currentPosition, xsymm, ysymm);
			currentPosition += endIncrement;

		}

		public static void BuildStraightStackedArmorLine(ShipConstruct construct, BlockCategory category, int steps, Vector3I increment, Vector3I stackIncrement, Vector3I endIncrement, bool xsymm, bool ysymm, ref Vector3I currentPosition, params Vector3I[] blocks) {

			currentPosition = BuildStackedBlocksLine(construct, category, increment, stackIncrement, steps, currentPosition, xsymm, ysymm, construct.BuildBlockStackList(blocks, blocks.Length, useAll: true));
			currentPosition += endIncrement;

		}

		public static void BuildDualBlockLine(ShipConstruct construct, BlockCategory category, int steps, int secondStepAdd, Vector3I increment, Vector3I secondLineOffset, Vector3I endIncrement, Vector3I singleStepEndIncrement, Vector3I firstBlock, Vector3I secondBlock, bool xsymm, bool ysymm, ref Vector3I currentPosition) {

			construct.Log.Append("BuildDualBlockLine: Steps: ").Append(steps.ToString()).AppendLine();
			var firstPos = BuildLine(construct, BlockCategory.Armor, firstBlock, increment, steps, currentPosition, xsymm, ysymm);

			if (steps > 1) {

				var secondPos = BuildLine(construct, BlockCategory.Armor, secondBlock, increment, steps += secondStepAdd, currentPosition += secondLineOffset, xsymm, ysymm);
				currentPosition = secondPos + endIncrement;

			} else {

				currentPosition = firstPos + singleStepEndIncrement;

			}

		}

		public static void BuildDualStackedBlockLine(ShipConstruct construct, BlockCategory category, int steps, int secondStepAdd, Vector3I increment, Vector3I stackIncrement, Vector3I secondLineOffset, Vector3I endIncrement, Vector3I singleStepEndIncrement, bool xsymm, bool ysymm, int stackLength, ref Vector3I currentPosition, params Vector3I[] blocks) {

			if (blocks == null || blocks.Length == 0 || blocks.Length % stackLength != 0) {

				//Todo: Raise error about odd amount of blocks provided
				return;

			}

			int halfArrayLength = blocks.Length / 2;

			var firstPos = BuildStackedBlocksLine(construct, BlockCategory.Armor, increment, stackIncrement, steps, currentPosition, xsymm, ysymm, construct.BuildBlockStackList(blocks, stackLength));

			if (steps > 1) {

				var secondPos = BuildStackedBlocksLine(construct, BlockCategory.Armor, increment, stackIncrement, steps += secondStepAdd, currentPosition += secondLineOffset, xsymm, ysymm, construct.BuildBlockStackList(blocks, stackLength, false));
				currentPosition = secondPos + endIncrement;

			} else {

				currentPosition = firstPos + singleStepEndIncrement;

			}


		}



		public static Vector3I BuildLine(ShipConstruct construct, BlockCategory category, Vector3I block, Vector3I increment, int steps, Vector3I position, bool xSymmetry = false, bool ySymmetry = false) {

			int stepsTaken = 0;
			var pos = position;

			while (true) {

				construct.PlaceBlock(category, pos, pos, block, xSymmetry, ySymmetry);

				stepsTaken++;

				if (stepsTaken >= steps)
					break;

				pos += increment;

			}

			return pos;

		}

		public static void BuildPatternedLines(ShipConstruct construct, List<LineData> lineCollection, Vector3I lineOffset, string firstLinePattern, string remainingLinePatterns = null, bool xSymmetry = false, bool ySymmetry = false) {

			if (lineCollection == null || lineCollection.Count == 0 || string.IsNullOrWhiteSpace(firstLinePattern))
				return;

			LinePattern startingLinePattern = null;

			if (!LinePatterns.TryGetValue(firstLinePattern, out startingLinePattern)) {

				return;
			
			}

			LinePattern secondaryLinePattern = null;

			if (string.IsNullOrWhiteSpace(remainingLinePatterns) || !LinePatterns.TryGetValue(remainingLinePatterns, out secondaryLinePattern)) {

				secondaryLinePattern = startingLinePattern;

			}

			for (int i = 0; i < lineCollection.Count; i++) {

				var line = lineCollection[i];
				line.OffsetLine(lineOffset);
				var pattern = i == 0 ? startingLinePattern : secondaryLinePattern;

				var currentPosition = line.Start;
				int currentLength = 0;
				int startIndex = 0;
				int endIndex = -1;
				bool startedLine = false;
				bool endingLine = false;

				while (true) {


					if (!startedLine) {

						if (pattern.StartLine(construct, line, currentLength, currentPosition, -line.AdjacentDirection)) {

							startedLine = true;

						} else {

							currentPosition += line.Direction;
							currentLength++;

							if (currentLength > line.Length)
								break;

							continue;

						}

					}

					if (!endingLine) {

						if (pattern.EndLine(construct, line, currentLength, currentPosition, line.Direction, -line.AdjacentDirection)) {

							endingLine = true;
							endIndex = 0;
						
						}
					
					}

					Vector3I blockToPlace = pattern.GetNextBlock(ref startIndex, ref endIndex);
					construct.PlaceBlock(pattern.Category, currentPosition, currentPosition, blockToPlace, xSymmetry, ySymmetry);
					
					currentPosition += line.Direction;
					currentLength++;

					if (currentLength >= line.Length || (endingLine && endIndex >= pattern.LinePatternEnd.Length))
						break;

				}
				
			}
		
		}

		public static void BuildStackedBlocks(ShipConstruct construct, BlockCategory category, Vector3I increment, Vector3I currentPosition, bool xSymmetry, bool ySymmetry, params Vector3I[] blocks) {

			if (blocks == null || blocks.Length == 0) {

				construct.Log.Append("BuildStackedBlocks: Array Null or Empty").AppendLine();
				return;

			}


			var pos = currentPosition;

			foreach (var block in blocks) {

				construct.PlaceBlock(category, pos, pos, block, xSymmetry, ySymmetry);
				pos += increment;

			}

		}

		public static Vector3I BuildStackedBlocksLine(ShipConstruct construct, BlockCategory category, Vector3I increment, Vector3I stackIncrement, int steps, Vector3I position, bool xSymmetry, bool ySymmetry, List<Vector3I> blocks) {

			if (blocks == null) {

				construct.Log.Append("BuildStackedBlocksLine: List Null").AppendLine();
				return position;

			}

			if (blocks.Count == 0) {

				construct.Log.Append("BuildStackedBlocksLine: List Empty").AppendLine();
				return position;

			}

			var pos = position;
			var stepsTaken = 0;

			while (true) {

				var cellPos = pos;

				for (int i = 0; i < blocks.Count; i++) {

					//Place
					construct.PlaceBlock(category, cellPos, cellPos, blocks[i], xSymmetry, ySymmetry);
					cellPos += stackIncrement;

				}

				stepsTaken++;

				if (stepsTaken >= steps)
					break;

				pos += increment;

			}

			return pos;

		}

		

		public static void FillSpaceWithLines(ShipConstruct construct, BlockCategory category, Vector3I startCoords, Vector3I advanceIncrement, Vector3I searchIncrement, int maxSearchCells, Vector3I block, bool xSymmetry, bool ySymmetry) {

			var currentPosition = startCoords;

			while (true) {

				if (!construct.CanPlaceBlockAtMin(currentPosition, currentPosition, true, true))
					return;

				construct.PlaceBlock(category, currentPosition, currentPosition, block, xSymmetry, ySymmetry);
				bool limitReached = false;
				var searchPosition = currentPosition + searchIncrement;

				//Scan
				for (int i = 0; i < maxSearchCells; i++) {

					if (i == maxSearchCells - 1) {

						limitReached = true;
						break;

					}

					if (!construct.CanPlaceBlockAtMin(searchPosition, searchPosition, true, true)) {

						searchPosition -= searchIncrement;
						break;

					}

					searchPosition += searchIncrement;

				}

				if (limitReached)
					break;

				//Place
				while (true) {

					if (!construct.CanPlaceBlockAtMin(searchPosition, searchPosition, true, true)) {

						break;

					}

					construct.PlaceBlock(category, searchPosition, searchPosition, block, xSymmetry, ySymmetry);
					searchPosition -= searchIncrement;

				}

				currentPosition += advanceIncrement;

			}
		
		}

		public static void GetTopLinePatternRandomPair(ref string start, ref string additional) {

			var roll = MathTools.RandomBetween(0, 4);

			if (roll == 0) {

				start = "TopSingleSlopeLine";
				return;
			
			}

			if (roll == 1) {

				start = "TopRampLine";
				return;

			}

			if (roll == 2) {

				start = "TopSingleSlopeLine";
				additional = "TopSlopeCornerLine";
				return;

			}

			if (roll == 3) {

				start = "TopRampLine";
				additional = "TopRampCornerLine";
				return;

			}

			return;

		}

	}

}
