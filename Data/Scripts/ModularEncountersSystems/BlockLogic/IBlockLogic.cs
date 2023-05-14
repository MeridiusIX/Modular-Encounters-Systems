using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;

namespace ModularEncountersSystems.BlockLogic {
	public interface IBlockLogic {

		bool Active { get; }
		string LogicType { get; }
		IMyCubeBlock CubeBlock { get; }

	}

}
