using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ProtoBuf;
using System.Collections.Generic;

namespace ModularEncountersSystems.Watchers {

	[ProtoContract]
	public class GridCleanupData {

		[ProtoMember(1)] public long GridId;
		[ProtoMember(2)] public int Timer;

		[ProtoIgnore] public bool Valid;
		[ProtoIgnore] public GridEntity Grid;
		[ProtoIgnore] public static List<GridCleanupData> CleanupData = new List<GridCleanupData>();

		public GridCleanupData() {

			GridId = 0;
			Timer = 0;
			Valid = false;

		}

		public GridCleanupData(GridEntity grid) {

			Init(grid);

		}

		public void Init(GridEntity grid) {

			if (grid != null) {

				if (grid.ActiveEntity()) {

					Valid = true;
					GridId = grid.CubeGrid.EntityId;
					Grid = grid;

				}

			} else {

				Grid = GridManager.GetGridEntity(GridId);

				if (Grid != null) {

					Valid = true;
					GridId = Grid.CubeGrid.EntityId;

				}
			
			}
		
		}

		public static GridCleanupData GetData(GridEntity grid) {

			for (int i = CleanupData.Count - 1; i >= 0; i--) {

				var data = CleanupData[i];

				if (!data.Grid.ActiveEntity()) {

					CleanupData.RemoveAt(i);
					continue;

				}

				if (data.Grid == grid)
					return data;
			
			}

			var newData = new GridCleanupData(grid);

			if (newData.Valid) {

				CleanupData.Add(newData);
				return newData;


			}

			return null;
		
		}

		public static void RemoveData(GridEntity grid) {

			for (int i = CleanupData.Count - 1; i >= 0; i--) {

				var data = CleanupData[i];

				if (!data.Grid.ActiveEntity()) {

					CleanupData.RemoveAt(i);
					continue;

				}

				if (data.Grid == grid) {

					CleanupData.RemoveAt(i);
					continue;

				}

			}

		}

		public static void LoadData() {

			var storedData = SerializationHelper.GetDataFromSandbox<List<GridCleanupData>>("MES-GridCleanupData");

			if (storedData != null)
				CleanupData = storedData;

			bool updateData = false;

			for (int i = CleanupData.Count - 1; i >= 0; i--) {

				var data = CleanupData[i];
				data.Init(null);

				if (!data.Valid) {

					updateData = true;
					CleanupData.RemoveAt(i);
					continue;

				}

			}

			if (updateData)
				SaveData();

		}

		public static void SaveData() {

			SerializationHelper.SaveDataToSandbox<List<GridCleanupData>>("MES-GridCleanupData", CleanupData);

		}

	}

}
