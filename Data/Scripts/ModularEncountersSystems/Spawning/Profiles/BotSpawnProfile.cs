using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class BotSpawnProfile {

		public string BotType;
		public bool UseAiEnabled;
		public string BotBehavior;
		public string BotDisplayName;
		public Vector3D Color;

		public BotSpawnProfile() {

			BotType = "";
			UseAiEnabled = false;
			BotBehavior = "";
			BotDisplayName = "";
			Color = new Vector3D();

		}

	}

}
