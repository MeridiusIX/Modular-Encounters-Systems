using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Terminal;
using ProtoBuf;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace ModularEncountersSystems.BlockLogic {
	public class PrefabConsoleTable : BaseBlockLogic, IBlockLogic {

		private BlockEntity _block;
		private IMyCubeGrid _grid;

		public IMyProjector Projector;
		public IMyStoreBlock Store;
		public StoreTableLink Link;

		public PrefabConsoleTable(BlockEntity block) {

			Setup(block);

		}

		internal override void Setup(BlockEntity block) {

			base.Setup(block);

			if (block?.Block == null) {

				_isValid = false;
				return;
			
			}

			_block = block;
			Projector = block.Block as IMyProjector;

			if (Projector == null) {

				_isValid = false;
				return;

			}

			/*
			if (Link == null)
				return;

			if (Link.StoreEntityId == 0) {

				var storeSlim = Projector.SlimBlock.CubeGrid.GetCubeBlock(Link.StoreMinPosition);

				if (storeSlim?.FatBlock != null) {

					Store = storeSlim.FatBlock as IMyStoreBlock;
					Link.StoreEntityId = storeSlim.FatBlock.EntityId;

				}

			} else {

				IMyEntity storeEntity = null;

				if (MyAPIGateway.Entities.TryGetEntityById(Link.StoreEntityId, out storeEntity))
					Store = storeEntity as IMyStoreBlock;

			}

			if (Store == null || (Store.CubeGrid != Projector.CubeGrid && !Store.IsInSameLogicalGroupAs(Projector))) {

				_isValid = false;
				return;

			}

			RemoveFromStore();
			*/

			//Pass
			_grid = Projector.SlimBlock.CubeGrid;
			_grid.OnBlockOwnershipChanged += OwnershipChange;
			_grid.OnGridSplit += GridSplit;

		}

		internal void OwnershipChange(IMyCubeGrid cubeGrid) {

			if (!_block.ActiveEntity() || _grid == null || _grid.MarkedForClose) {

				Unload();
				return;
			
			}

			var cubeBlock = _block.Block as MyCubeBlock;

			if (cubeBlock?.IDModule == null)
				return;

			if (cubeBlock.IDModule.ShareMode != VRage.Game.MyOwnershipShareModeEnum.All)
				cubeBlock.ChangeBlockOwnerRequest(Block.Block.OwnerId, VRage.Game.MyOwnershipShareModeEnum.All);

		}

		internal void GridSplit(IMyCubeGrid a, IMyCubeGrid b) {

			if (!_block.ActiveEntity() || _grid == null || _grid.MarkedForClose) {

				Unload();
				return;
			
			}

			_grid.OnGridSplit -= GridSplit;
			_grid.OnBlockOwnershipChanged -= OwnershipChange;

			_grid = Projector.SlimBlock.CubeGrid;

			_grid.OnGridSplit += GridSplit;
			_grid.OnBlockOwnershipChanged += OwnershipChange;


		}

		internal void Unload() {

			if (_grid != null) {

				_grid.OnBlockOwnershipChanged -= OwnershipChange;
				_grid.OnGridSplit -= GridSplit;

			}

			_isValid = false;
		
		}

	}

	[ProtoContract]
	public class StoreTableLink {

		[ProtoMember(1)] public SerializableVector3I StoreMinPosition;
		[ProtoMember(2)] public long StoreEntityId;
		[ProtoMember(3)] public int BlockLimit;
		[ProtoMember(4)] public string GridSize;
		[ProtoMember(5)] public long StoreItemId;

		public StoreTableLink() {

			StoreMinPosition = new SerializableVector3I();
			StoreEntityId = 0;
			BlockLimit = 2000;
			GridSize = "Both";
			StoreItemId = 0;

		}

	}

}
