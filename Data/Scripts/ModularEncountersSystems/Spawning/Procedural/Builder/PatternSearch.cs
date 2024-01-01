using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Procedural.Builder {

	public struct LineData {

		public bool Valid;
		public Vector3I Start;
		public Vector3I End;
		public Vector3I Direction;
		public Vector3I AdjacentDirection;
		public int Length;

		public LineData(Vector3I start, Vector3I end, Vector3I dir, Vector3I searchDir, int length) {

			Start = start;
			End = end;
			Direction = dir;
			Length = length;
			AdjacentDirection = searchDir;
			Valid = true;
		
		}

		public LineData(bool valid = false) {

			Start = Vector3I.Zero;
			End = Vector3I.Zero;
			Direction = Vector3I.Zero;
			AdjacentDirection = Vector3I.Zero;
			Valid = false;
			Length = 0;

		}

		public void OffsetLine(Vector3I offsetAmount) {

			Start += offsetAmount;
			End += offsetAmount;
		
		}

	}

	public static class PatternSearch {

		public static void FindAllLines(List<LineData> lineCollection, ShipConstruct construct, BlockCategory category, Vector3I blockType, Vector3I start, Vector3I dirToConstruct, Vector3I advanceDir, Vector3I searchDir, int advancePadding, int searchPadding) {

			var currentPosition = start;
			MyObjectBuilder_CubeBlock referenceBlock = construct.GetReferenceBlock(category, blockType);
			MyObjectBuilder_CubeBlock searchBlock = null;

			if (referenceBlock == null) {

				construct.Log.Append("PatternSearch.FindAllLines() could not get referenceBlock. Is Null.").AppendLine();
				return;

			}
				

			//Move Toward Grid Until Desired Block is Located
			for (int i = 0; i < 150; i++) {

				if (i == 149) {

					construct.Log.Append("PatternSearch.FindAllLines() could not locate construct initial start point.").AppendLine();
					return;

				}
					

				searchBlock = construct.GetBlock(currentPosition);

				if (searchBlock == null) {

					currentPosition += dirToConstruct;
					continue;
				
				}

				if (searchBlock.GetId() == referenceBlock.GetId())
					break;
				else
					construct.Log.Append("PatternSearch.FindAllLines() found block, but ID doesn't match referenceBlock.").AppendLine();

				currentPosition += dirToConstruct;

			}

			//Begin Scanning Lines
			for (int i = 0; i <= construct.Max.X; i++) {

				var lineStartCoords = currentPosition + (i * searchDir);
				var line = FindLine(construct, category, blockType, lineStartCoords, advanceDir, searchDir, advancePadding, searchPadding, construct.Max.Z + 5);

				if (!line.Valid)
					break;

				lineCollection.Add(line);

			}
		
		}

		public static LineData FindLine(ShipConstruct construct, BlockCategory category, Vector3I blockType, Vector3I start, Vector3I advanceDir, Vector3I searchDir, int advancePadding, int searchPadding, int maxAdvance) {

			Vector3I? firstPoint = null;
			Vector3I? endPoint = null;
			int lineLength = 0;

			for (int i = 0; i <= maxAdvance; i++) {

				var currentPosition = start + (i * advanceDir);
				var backCheck = CheckPadding(construct, category, blockType, currentPosition, -advanceDir, advancePadding);
				var sideCheck = CheckPadding(construct, category, blockType, currentPosition, searchDir, searchPadding);
				var forwardCheck = CheckPadding(construct, category, blockType, currentPosition, advanceDir, advancePadding);

				if (!firstPoint.HasValue && !backCheck) {

					continue;
				
				}

				if (!sideCheck || !forwardCheck) {

					if (!firstPoint.HasValue)
						continue;
					else
						break;
				
				}

				if (!firstPoint.HasValue)
					firstPoint = currentPosition;

				endPoint = currentPosition;
				lineLength++;

			}

			if (!firstPoint.HasValue)
				return new LineData();

			return new LineData(firstPoint.Value, endPoint.Value, advanceDir, searchDir, lineLength);
		
		}

		public static bool CheckPadding(ShipConstruct construct, BlockCategory category, Vector3I blockType, Vector3I start, Vector3I direction, int paddingSteps) {

			var currentPos = start;
			var block = construct.GetReferenceBlock(category, blockType);

			if (block == null)
				return false;

			var id = block.GetId();
			MyObjectBuilder_CubeBlock checkBlock = null;

			for (int i = 0; i < paddingSteps; i++) {

				currentPos += direction;
				checkBlock = construct.GetBlock(currentPos);

				if (checkBlock == null || checkBlock.GetId() != id)
					return false;

			}

			return true;
		
		}

		public static int CheckLine(ShipConstruct construct, BlockCategory category, Vector3I block, Vector3I increment, Vector3I position, bool matchAnyBlock = false) {

			int hits = 0;
			var currentPosition = position;
			var refBlock = construct.GetReferenceBlock(category, block);

			if (refBlock == null)
				return hits;

			for (int i = 0; i < 250; i++) {

				var cubeBlock = construct.GetBlock(currentPosition);

				if (cubeBlock == null)
					break;

				if (!matchAnyBlock)
					if (cubeBlock.GetId() != refBlock.GetId())
						break;

				currentPosition += increment;
				hits += 1;

			}

			return hits;

		}

	}
	
}
