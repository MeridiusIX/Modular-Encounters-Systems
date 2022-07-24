using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Entities;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.ObjectBuilders;


namespace ModularEncountersSystems.BlockLogic {

	public class ReactorPrimingLogic : BaseBlockLogic, IBlockLogic{
		
		IMyReactor Reactor;

		public ReactorPrimingLogic(BlockEntity block) {

			Setup(block);

		}

		internal override void Setup(BlockEntity block) {

			Reactor = block.Block as IMyReactor;

			if (Reactor == null) {

				_isValid = false;
				return;

			}

			if (Reactor.IsFunctional == true) {

				if (Reactor.GetInventory().Empty() == true) {

					var fuelId = new MyDefinitionId(typeof(MyObjectBuilder_Ingot), "UraniumB");
					var content = (MyObjectBuilder_PhysicalObject)MyObjectBuilderSerializer.CreateNewObject(fuelId);
					var fuelItem = new MyObjectBuilder_InventoryItem { Amount = 1, Content = content };

					if (Reactor.GetInventory().CanItemsBeAdded((MyFixedPoint)Settings.CustomBlocks.ProprietaryReactorFuelAmount, fuelId) == true && MyAPIGateway.Multiplayer.IsServer == true) {

						Reactor.GetInventory().AddItems((MyFixedPoint)Settings.CustomBlocks.ProprietaryReactorFuelAmount, fuelItem.Content);

					}

				}

			}

			_isValid = false;

		}

	}
	
}