using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;
using ModularEncountersSystems.Helpers;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems.AutoPilot {

	[ProtoContract]
	public class AutoPilotState {

		//Autopilot Data
		[ProtoMember(1)]
		public string PrimaryAutopilotId;

		[ProtoMember(2)]
		public string SecondaryAutopilotId;

		[ProtoMember(3)]
		public string TertiaryAutopilotId;

		[ProtoIgnore]
		public AutoPilotProfile PrimaryAutoPilot;

		[ProtoIgnore]
		public AutoPilotProfile SecondaryAutoPilot;

		[ProtoIgnore]
		public AutoPilotProfile TertiaryAutoPilot;

		[ProtoMember(4)]
		public AutoPilotDataMode DataMode;

		//Autopilot Mode

		[ProtoMember(5)]
		public bool FirstRun;

		[ProtoMember(6)]
		public AutoPilotType CurrentAutoPilot;

		[ProtoMember(7)]
		public NewAutoPilotMode AutoPilotFlags;

		//Coordinate Data

		[ProtoMember(8)]
		public Vector3D InitialWaypoint;

		[ProtoMember(9)]
		public Vector3D PendingWaypoint;

		[ProtoMember(10)]
		public Vector3D CurrentWaypoint;

		//CargoShip Data

		[ProtoMember(11)]
		public List<EncounterWaypoint> CargoShipWaypoints;

		[ProtoMember(12)]
		public EncounterWaypoint CargoShipDespawn;

		[ProtoMember(13)]
		public double MaxSpeedOverride;

		[ProtoMember(14)]
		public DateTime WaypointWaitTime;

		//User Custom Mode

		[ProtoMember(15)]
		public bool UseFlyLevelWithGravity;

		[ProtoMember(16)]
		public bool UseFlyLevelWithGravityIdle;

		[ProtoMember(17)]
		public NewAutoPilotMode NormalAutopilotFlags;

		[ProtoMember(18)]
		public bool DisableAutopilot;

		public AutoPilotState() {

			PrimaryAutopilotId = "";
			SecondaryAutopilotId = "";
			TertiaryAutopilotId = "";

			PrimaryAutoPilot = new AutoPilotProfile();
			SecondaryAutoPilot = new AutoPilotProfile();
			TertiaryAutoPilot = new AutoPilotProfile();

			DataMode = AutoPilotDataMode.Primary;

			FirstRun = false;
			CurrentAutoPilot = AutoPilotType.None;
			AutoPilotFlags = NewAutoPilotMode.None;

			CargoShipWaypoints = new List<EncounterWaypoint>();
			CargoShipDespawn = new EncounterWaypoint();

			MaxSpeedOverride = -1;

			WaypointWaitTime = DateTime.MinValue;

			UseFlyLevelWithGravity = false;
			UseFlyLevelWithGravityIdle = false;
			NormalAutopilotFlags = NewAutoPilotMode.None;

			DisableAutopilot = false;

		}

	}

}
