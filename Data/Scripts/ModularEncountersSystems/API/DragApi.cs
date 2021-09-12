using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.API {
	public class RemoteDragSettings {

		private bool init = false;
		private bool m_Response = false;
		public const long MODID = 571920453;//mod ID of the Aerodynamic Physics mod this is the mod we want to talk to. 
		private const int GETSETTINGS = 1;

		private bool m_DigiPhysics = true;//keep digi physics enabled if mod is not installed. 
		private bool m_AdvLift = false;
		private enum SettingsEnum : int {
			digi = 0,
			advlift,
			show_warning,
			showburn,
			showsmoke
		}
		public bool Heartbeat {
			get {
				if (!init)
					Register();
				return m_Response;
			}
		}

		public bool DigiPhysics {
			get {
				if (!init)
					Register();
				if (m_Response)
					return (bool)GetSetting((int)SettingsEnum.digi);
				return m_DigiPhysics;
			}
		}
		public bool AdvLift {
			get {
				if (!init)
					Register();
				if (m_Response)
					return (bool)GetSetting((int)SettingsEnum.advlift);
				return m_AdvLift;
			}
		}
		private static Func<int, object> GetSetting;
		private static Func<MyPlanet, Vector3D, IMyEntity, Vector3D> WindGetter;
		private static Func<IMyEntity, BoundingBox?> SurfaceAreaGetter;
		private static Func<IMyEntity, MyTuple<double, double, double, double, double, double>?> HeatGetter;

		public Vector3D? GetWind(MyPlanet Planet, Vector3D pos, IMyEntity ent) {
			if (WindGetter != null)
				return WindGetter(Planet, pos, ent);
			return null;
		}



		/// <summary>
		/// Gets the visible surface area of an object
		/// </summary>
		/// <param name="ent">Entity</param>
		/// <returns>Local aligned bounding box</returns>
		public BoundingBox? GetSurfaceArea(IMyEntity ent) {
			if (SurfaceAreaGetter != null)
				return SurfaceAreaGetter(ent);
			return null;
		}
		/// <summary>
		/// Returns heat data, null if entity does not have a drag calculation, otherwise returns (heat.front, heat.back, heat.left, heat.right, heat.up, heat.down)
		/// </summary>
		/// <param name="ent">Entity</param>
		/// <returns>MyTuple<double, double, double, double, double, double>(heat.front, heat.back, heat.left, heat.right, heat.up, heat.down)</returns>
		public MyTuple<double, double, double, double, double, double>? GetHeat(IMyEntity ent) {
			if (HeatGetter != null) {
				return HeatGetter(ent);
			}
			return null;
		}

		public void Register() {
			if (init) {
				MyAPIGateway.Utilities.UnregisterMessageHandler(MODID, Handler);//clear if already registered. 
			}
			init = true;
			MyAPIGateway.Utilities.RegisterMessageHandler(MODID, Handler);
			MyAPIGateway.Utilities.SendModMessage(MODID, GETSETTINGS);

		}
		public void UnRegister() {
			if (init) {
				MyAPIGateway.Utilities.UnregisterMessageHandler(MODID, Handler);
			}
			init = false;
		}

		private void Handler(object obj) {
			try {

				if (obj is MyTuple<Func<int, object>, Func<MyPlanet, Vector3D, IMyEntity, Vector3D>>) {
					m_Response = true;
					var tupl = (MyTuple<Func<int, object>, Func<MyPlanet, Vector3D, IMyEntity, Vector3D>>)obj;
					GetSetting = tupl.Item1;
					WindGetter = tupl.Item2;

				}
				if (obj is MyTuple<Func<IMyEntity, BoundingBox?>, Func<IMyEntity, MyTuple<double, double, double, double, double, double>?>>) {
					var tupl = (MyTuple<Func<IMyEntity, BoundingBox?>, Func<IMyEntity, MyTuple<double, double, double, double, double, double>?>>)obj;
					SurfaceAreaGetter = tupl.Item1;
					HeatGetter = tupl.Item2;
				}
			} catch {

			}
		}
	}
}
