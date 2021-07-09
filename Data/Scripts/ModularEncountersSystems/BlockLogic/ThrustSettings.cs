using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Profiles;
using ProtoBuf;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.BlockLogic {

	[ProtoContract]
	public class ThrustSettings {

		[ProtoMember(1)]
		public bool RestrictNpcIonThrust;

		[ProtoMember(2)]
		public float NpcIonThrustForceMultiply;

		[ProtoMember(3)]
		public float NpcIonThrustPowerMultiply;

		[ProtoMember(4)]
		public bool RestrictNpcAtmoThrust;

		[ProtoMember(5)]
		public float NpcAtmoThrustForceMultiply;

		[ProtoMember(6)]
		public float NpcAtmoThrustPowerMultiply;

		[ProtoMember(7)]
		public bool RestrictNpcHydroThrust;

		[ProtoMember(8)]
		public float NpcHydroThrustForceMultiply;

		[ProtoMember(9)]
		public float NpcHydroThrustPowerMultiply;

		public ThrustSettings() {

			RestrictNpcIonThrust = false;
			NpcIonThrustForceMultiply = 1;
			NpcIonThrustPowerMultiply = 1;

			RestrictNpcAtmoThrust = false;
			NpcAtmoThrustForceMultiply = 1;
			NpcAtmoThrustPowerMultiply = 1;

			RestrictNpcHydroThrust = false;
			NpcHydroThrustForceMultiply = 1;
			NpcHydroThrustPowerMultiply = 1;

		}

		public ThrustSettings(ManipulationProfile profile) {

			RestrictNpcIonThrust = profile.RestrictNpcIonThrust;
			NpcIonThrustForceMultiply = profile.NpcIonThrustForceMultiply;
			NpcIonThrustPowerMultiply = profile.NpcIonThrustPowerMultiply;

			RestrictNpcAtmoThrust = profile.RestrictNpcAtmoThrust;
			NpcAtmoThrustForceMultiply = profile.NpcAtmoThrustForceMultiply;
			NpcAtmoThrustPowerMultiply = profile.NpcAtmoThrustPowerMultiply;

			RestrictNpcHydroThrust = profile.RestrictNpcHydroThrust;
			NpcHydroThrustForceMultiply = profile.NpcHydroThrustForceMultiply;
			NpcHydroThrustPowerMultiply = profile.NpcHydroThrustPowerMultiply;

		}

		public static ThrustSettings ConvertFromString(string data) {

			var bytes = Convert.FromBase64String(data);

			if (bytes == null)
				return new ThrustSettings();

			var settings = MyAPIGateway.Utilities.SerializeFromBinary<ThrustSettings>(bytes);

			if (settings == null)
				return new ThrustSettings();

			return settings;

		}

		public string ConvertToString() {

			var bytes = MyAPIGateway.Utilities.SerializeToBinary(this);
			return Convert.ToBase64String(bytes);

		}

	}

}
