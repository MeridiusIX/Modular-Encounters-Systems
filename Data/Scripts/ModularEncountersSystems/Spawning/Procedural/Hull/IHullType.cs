using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Spawning.Procedural.Hull {
	public interface IHullType {

		void ThrusterPlacement(ShipConstruct construct);

		void InteriorPlacement(ShipConstruct construct);

		void InitialHullSetup(ShipConstruct construct);

		void FirstOutline(ShipConstruct construct);

		void MaterializeHull(ShipConstruct construct);

	}

}
