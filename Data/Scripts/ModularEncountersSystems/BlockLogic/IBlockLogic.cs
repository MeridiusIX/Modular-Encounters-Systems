using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.BlockLogic {
	public interface IBlockLogic {

		bool Active { get; }
		string LogicType { get; }

	}

}
