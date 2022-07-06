using ModularEncountersSystems.BlockLogic;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Entities {
	public class BlockEntity : EntityBase, ITarget {

		public IMyTerminalBlock Block;
		public IMyFunctionalBlock FunctionalBlock;

		public string BlockType;

		public bool Enabled;		//Block Is Turned On (If It Can Be)
		public bool Functional;		//Built & Undamaged
		public bool Modded;			//Is From a Mod
		public bool Working;        //Powered

		public GridEntity ParentGrid;
		public List<GridEntity> LinkedGrids;

		public BlockEntity(IMyEntity entity, IMyEntity parentEntity) : base(entity){

			if (entity == null || parentEntity == null)
				return;

			Type = EntityType.Block;

			Block = entity as IMyTerminalBlock;
			IsValidEntity = true;
			RefreshSubGrids();

			BlockType = Block.SlimBlock.BlockDefinition.Id.TypeId.ToString();

			Block.IsWorkingChanged += WorkingChanged;
			WorkingChanged(Block);

			FunctionalBlock = Block as IMyFunctionalBlock;

			if (FunctionalBlock != null) {

				FunctionalBlock.EnabledChanged += EnabledChanged;
				EnabledChanged(FunctionalBlock);

			}

		}

		public void EnabledChanged(IMyTerminalBlock block) {

			Enabled = FunctionalBlock.Enabled;

		}

		public void WorkingChanged(IMyCubeBlock block) {

			Functional = block.IsFunctional;
			Working = block.IsWorking;

		}

		//---------------------------------------------------
		//-----------Start Interface Methods-----------------
		//---------------------------------------------------

		public bool ActiveEntity() {

			if (Closed || !Functional)
				return false;

			return true;

		}
		public double BroadcastRange(bool onlyAntenna = false) {

			return EntityEvaluator.GridBroadcastRange(LinkedGrids, onlyAntenna);

		}

		public string FactionOwner() {

			var result = "";

			if (Block?.SlimBlock?.CubeGrid?.BigOwners != null) {

				if (Block.SlimBlock.CubeGrid.BigOwners.Count > 0) {

					var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(Block.SlimBlock.CubeGrid.BigOwners[0]);

					if (faction != null) {

						return faction.Tag;

					}

				}

			}

			return result;

		}

		public double GetCurrentHealth() {

			if (ActiveEntity() && Block?.SlimBlock != null)
				return Math.Round(Block.SlimBlock.BuildIntegrity - Block.SlimBlock.CurrentDamage, 3);
			return 0;
		
		}

		public override EntityType GetEntityType() {

			return EntityType.Grid;

		}



		public List<long> GetOwners(bool onlyGetCurrentEntity = false, bool includeMinorityOwners = false) {

			var result = new List<long>();

			if (!ActiveEntity())
				return result;

			if (!onlyGetCurrentEntity) {

				if (Block.OwnerId != 0) {

					result.Add(Block.OwnerId);
					return result;

				}

			}

			foreach (var grid in LinkedGrids) {

				if (grid == null || !grid.ActiveEntity())
					continue;

				if (onlyGetCurrentEntity && grid.CubeGrid != Block.SlimBlock.CubeGrid)
					continue;

				if (grid.CubeGrid?.BigOwners != null) {

					foreach (var owner in grid.CubeGrid.BigOwners) {

						if (!result.Contains(owner))
							result.Add(owner);

					}

				}

				if (!includeMinorityOwners)
					continue;

				if (grid.CubeGrid?.SmallOwners != null) {

					foreach (var owner in grid.CubeGrid.SmallOwners) {

						if (!result.Contains(owner))
							result.Add(owner);

					}

				}

			}

			return result;
		
		}

		public GridOwnershipEnum GetOwnerType() {

			if (!ActiveEntity())
				return GridOwnershipEnum.None;

			if (ParentGrid == null || ParentGrid.CubeGrid != Block.SlimBlock.CubeGrid) {

				ParentGrid = GridManager.GetGridEntity(Block.SlimBlock.CubeGrid);

				if(ParentGrid == null)
					return GridOwnershipEnum.None;

			}

			return ParentGrid.Ownership;

		}

		public bool IsPowered() {

			if (IsClosed() || !Functional)
				return false;

			if (FunctionalBlock != null) {

				if (Enabled) {

					return Working && Functional;

				} else {

					return EntityEvaluator.GridPowered(LinkedGrids);
				
				}
			
			}

			return Working && Functional;

		}

		public bool IsSameGrid(IMyEntity entity) {

			if (!ActiveEntity())
				return false;

			foreach (var grid in LinkedGrids) {

				if (!grid.ActiveEntity())
					continue;

				if (grid.CubeGrid.EntityId == entity.EntityId)
					return true;

			}

			return false;

		}

		public bool IsStatic() {

			if (!ActiveEntity())
				return false;

			return Block.SlimBlock.CubeGrid.IsStatic;

		}

		public int MovementScore() {

			if (!ActiveEntity())
				return 0;

			return EntityEvaluator.GridMovementScore(LinkedGrids);

		}

		public string Name() {

			if (!ActiveEntity())
				return "N/A";

			string blockName = !string.IsNullOrWhiteSpace(Block.CustomName) ? Block.CustomName : "(Unnamed Block)";
			string gridName = !string.IsNullOrWhiteSpace(Block.CubeGrid.CustomName) ? Block.CubeGrid.CustomName : "(Unnamed Grid)";
			return blockName + "//" + gridName;

		}

		public OwnerTypeEnum OwnerTypes(bool onlyGetCurrentEntity = false, bool includeMinorityOwners = false) {

			var owners = GetOwners(onlyGetCurrentEntity, includeMinorityOwners);
			return EntityEvaluator.GetOwnersFromList(owners);

		}

		public bool PlayerControlled() {

			foreach (var grid in LinkedGrids) {

				if (!grid.ActiveEntity())
					continue;

				if (EntityEvaluator.IsPlayerControlled(grid))
					return true;

			}

			return false;

		}

		public Vector2 PowerOutput() {

			return EntityEvaluator.GridPowerOutput(LinkedGrids);

		}

		public RelationTypeEnum RelationTypes(long ownerId, bool onlyGetCurrentEntity = false, bool includeMinorityOwners = false) {

			var owners = GetOwners(onlyGetCurrentEntity, includeMinorityOwners);
			return EntityEvaluator.GetRelationsFromList(ownerId, owners);

		}

		public void RefreshSubGrids() {

			if (ParentGrid == null)
				ParentGrid = GridManager.GetGridEntity(Block?.SlimBlock?.CubeGrid);

			if (ParentGrid == null)
				return;

			EntityEvaluator.GetAttachedGrids(ParentGrid);
			LinkedGrids = ParentGrid.LinkedGrids;

		}

		public float TargetValue() {

			return EntityEvaluator.GridTargetValue(LinkedGrids);

		}

		public int WeaponCount() {

			return EntityEvaluator.GridWeaponCount(LinkedGrids);

		}

		//---------------------------------------------------
		//------------End Interface Methods------------------
		//---------------------------------------------------

		public override void CloseEntity(IMyEntity entity) {

			base.CloseEntity(entity);
			BlockLogicManager.LogicBlocks.Remove(Block.EntityId);

		}

		public override void Unload() {

			base.Unload();
			Block.IsWorkingChanged -= WorkingChanged;
			

			if (FunctionalBlock != null)
				FunctionalBlock.EnabledChanged -= EnabledChanged;

		}

	}

}
