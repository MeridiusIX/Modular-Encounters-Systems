using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Manipulation.Procedural {

	public enum MountBlock {

		Any,
		Armor,
		Conveyor,
		Doorway

	}

	public enum MountDirection {

		Front,
		Back,
		Top,
		Bottom,
		Left,
		Right

	}

	public enum GridType {

		None,
		AtmoShip,
		HydrogenShip,
		IonShip,
		PlanetStation,
		SpaceStation

	}

	public enum GridComponentType {

		None,
		ShipCore,
		MidSection,
		NacelleArm,
		Nacelle,
		Greeble,
		TurretMount,
		Turret,
		SideSection,
		StationCore,
		SupportBeam,
		FrontBridge,
		MidBridge,

	}

	public enum ComponentAttributes {

		None,
		ForwardThrust,
		ReverseThrust,
		DownwardThrust,
		UpwardThrust,
		LeftThrust,
		RightThrust,
		Gyroscopics,
		Controllers,
		Containers,
		Production,
		Power,
		Weapons,
		Signals,
		Gases,
		Entrance,

	}

}
