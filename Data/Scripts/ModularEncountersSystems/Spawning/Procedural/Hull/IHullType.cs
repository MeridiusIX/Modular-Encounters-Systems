using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Procedural.Hull {
	public interface IHullType {

		ShipConstruct Construct { get; }
		void InitialHullSetup();
		void ThrusterPlacement();
		void InteriorPlacement();
		void SystemsPlacement();
		void GreebleHull();
		void PaintingAndSkins();
		void SpawnCurrentConstruct(MatrixD matrix);

	}

}
