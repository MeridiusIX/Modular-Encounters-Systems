using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Weapons;
using SpaceEngineers.Game.ModAPI;
using ProtoBuf;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;
using ModularEncountersSystems.Helpers;

namespace ModularEncountersSystems.Behavior.Subsystems.Profiles {
	public class MissileProfile {

		public IMyEntity Entity;
		public long EntityId;
		public long Owner;
		public long LauncherId;
		public double ExplodeRadius;
		public bool Removed;
		public Vector3D RemovalCoords;
		public DateTime RemovalTime;
		public List<object> HitObjects;

		public MissileProfile() {

			Entity = null;
			EntityId = 0;
			Owner = 0;
			LauncherId = 0;
			ExplodeRadius = 4;
			Removed = false;
			RemovalCoords = Vector3D.Zero;
			RemovalTime = MyAPIGateway.Session.GameDateTime;
			HitObjects = new List<object>();

		}

		public MissileProfile(IMyEntity entity, MyObjectBuilder_Missile ob) {

			Entity = entity;
			EntityId = entity.EntityId;
			Owner = ob.Owner;
			LauncherId = ob.LauncherId;
			ExplodeRadius = 4;
			Removed = false;
			RemovalCoords = Vector3D.Zero;
			RemovalTime = MyAPIGateway.Session.GameDateTime;
			HitObjects = new List<object>();
			//Logger.AddMsg("Owner: " + ob.Owner.ToString(), true);
			//Logger.AddMsg("Origin: " + ob.OriginEntity.ToString(), true);
			//Logger.AddMsg("Launcher: " + ob.LauncherId.ToString(), true);

		}

	}
}
