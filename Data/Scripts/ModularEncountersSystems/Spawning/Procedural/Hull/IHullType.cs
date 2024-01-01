using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Procedural.Hull {
	public interface IHullType {

		ShipConstruct Construct { get; }
		ShipRules Rules { get; }
		void SpawnCurrentConstruct(MatrixD matrix, string prefabId);

	}

}
