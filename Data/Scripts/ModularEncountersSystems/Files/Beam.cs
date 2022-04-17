using System;
using System.Collections.Generic;
using System.Text;
using VRage;

namespace ModularEncountersSystems.Files {

	public struct BeamEffect {

		public bool LinearBeam;
		public float LinearBeamMinWidth;
		public float LinearBeamMaxWidth;

		public SerializableVector3[] LinearBeamColors;
		public bool FadeThroughLinearBeamColors;

		public string ImpactParticleId;
		public float ImpactParticleScale;
		public SerializableVector3 ImpactParticleColor;
		public int ImpactParticlePerTicks;

		public string FireSound;

		public BeamEffect(bool init = false) {

			LinearBeam = false;
			LinearBeamMinWidth = 1;
			LinearBeamMaxWidth = 2;

			LinearBeamColors = new SerializableVector3[] { };
			FadeThroughLinearBeamColors = false;

			ImpactParticleId = "";
			ImpactParticleScale = 1;
			ImpactParticleColor = new SerializableVector3();
			ImpactParticlePerTicks = 60;

			FireSound = "";

		}
	
	}

	public struct BeamBehavior {

		public double MaxRange;
		public double PadDistanceFromOrigin;
	
	}

	public struct BeamDamage {

		public bool RegularDamage;
		public float RegularDamageAmount;

		public bool PenetrativeDamage;

		public bool ExplosionDamage;
		public bool ExplosionDamageAmount;
		public float ExplosionDamageRadius;

		public bool TeslaDamage;
		public int TeslaDamageBlockCount;
		public int TeslaDamageEffectDuration;

		public bool ShieldDamage;
		public float ShieldDamageAmount;

	}

	public class Beam {

		public BeamBehavior Behavior;
		public BeamEffect Visual;
		public BeamDamage Damage;

	}

}
