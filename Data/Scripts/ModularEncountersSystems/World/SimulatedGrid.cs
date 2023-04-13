using ModularEncountersSystems.Entities;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.ObjectBuilders;
using VRageMath;

namespace ModularEncountersSystems.World {
	public class SimulatedGrid {

		public bool Active;

		public string PrefabName;
		public string SpawnGroupName;
		public int SpawnConditionsIndex;
		public List<int> ManipulationsUsed;

		public string SerializedBehavior;

		public Vector3D WorldMatrixCoords;
		public Vector3D WorldMatrixForward;
		public Vector3D WorldMatrixUp;

		public bool UsingRemoteControlCode;
		public string RemoteControlCode;

		public Dictionary<Vector3I, List<StoreItemData>> StoreBlockData;

		public SimulatedGrid() {

			ManipulationsUsed = new List<int>();
			StoreBlockData = new Dictionary<Vector3I, List<StoreItemData>>();


		}

		public void InitSimulatedGrid(GridEntity grid, NpcData data) {

			//Null Checks
			if (grid?.CubeGrid == null) {

				Active = false;
				return;

			}
			

			//Grab Behavior

			//Grab Store Data
		
		}

	}

	[ProtoContract]
	public struct StoreItemData {

		[ProtoMember(1)] public SerializableDefinitionId Id;
		[ProtoMember(2)] public int Amount;
		[ProtoMember(3)] public int Price;
		[ProtoMember(4)] public bool IsOrder;

		public StoreItemData(SerializableDefinitionId id, int amount, int price, bool isOrder) {

			Id = id;
			Amount = amount;
			Price = price;
			IsOrder = isOrder;

		}

	}

}
