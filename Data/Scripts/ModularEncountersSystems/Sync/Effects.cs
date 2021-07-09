using ProtoBuf;
using VRageMath;

namespace ModularEncountersSystems.Sync {

	public enum EffectSyncMode {

		None,
		PlayerSound,
		PositionSound,
		Particle,

	}

	[ProtoContract]
	public class Effects {

		[ProtoMember(1)]
		public EffectSyncMode Mode;

		[ProtoMember(2)]
		public Vector3D Coords;

		[ProtoMember(3)]
		public string SoundId;

		[ProtoMember(4)]
		public string ParticleId;

		[ProtoMember(5)]
		public float ParticleScale;

		[ProtoMember(6)]
		public Vector3D ParticleColor;

		[ProtoMember(7)]
		public float ParticleMaxTime;

		[ProtoMember(8)]
		public Vector3D ParticleForwardDir;

		[ProtoMember(9)]
		public Vector3D ParticleUpDir;

		[ProtoMember(10)]
		public Vector3 Velocity;

		[ProtoMember(11)]
		public string AvatarId;

		[ProtoMember(12)]
		public float SoundVolume;

		public Effects() {

			Mode = EffectSyncMode.None;
			Coords = Vector3D.Zero;
			SoundId = "";
			ParticleId = "";
			ParticleScale = 1;
			ParticleColor = Vector3D.Zero;
			ParticleMaxTime = -1;
			ParticleForwardDir = Vector3D.Forward;
			ParticleUpDir = Vector3D.Up;
			Velocity = Vector3.Zero;
			AvatarId = "";
			SoundVolume = 0;

		}

	}

}
