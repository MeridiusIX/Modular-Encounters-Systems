using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Spawning.Procedural.Hull {
	public interface IHullType {

		void InitialHullSetup(ShipConstruct construct);
		void ThrusterPlacement(ShipConstruct construct);
		void InteriorPlacement(ShipConstruct construct);
		void SystemsPlacement(ShipConstruct construct);
		void GreebleHull(ShipConstruct construct);
		void PaintingAndSkins(ShipConstruct construct);


	}

}
