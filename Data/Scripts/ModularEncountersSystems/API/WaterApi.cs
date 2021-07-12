using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;

namespace ModularEncountersSystems.API {
	//Only Include this file in your project

	public static class WaterAPI {

		public static string ModName = MyAPIGateway.Utilities.GamePaths.ModScopeName.Split('_')[1];
		public const ushort ModHandlerID = 50271;
		public const int ModAPIVersion = 15;
		public static bool Registered { get; private set; } = false;

		private static Dictionary<string, Delegate> ModAPIMethods;

		private static Func<int, string, bool> _VerifyVersion;

		private static Func<Vector3D, long?, bool> _IsUnderwater;
		private static Func<LineD, long?, int> _LineIntersectsWater;
		private static Action<List<LineD>, ICollection<int>, long?> _LineIntersectsWaterList;
		private static Func<Vector3D, long?> _GetClosestWater;
		private static Func<BoundingSphereD, long?, int> _SphereIntersectsWater;
		private static Action<List<BoundingSphereD>, ICollection<int>, long?> _SphereIntersectsWaterList;
		private static Func<Vector3D, long?, Vector3D> _GetClosestSurfacePoint;
		private static Action<List<Vector3D>, ICollection<Vector3D>, long?> _GetClosestSurfacePointList;
		private static Func<Vector3D, long?, float?> _GetDepth;
		private static Action _ForceSync;
		private static Action<string> _RunCommand;
		private static Func<Vector3D, long?, Vector3D> _GetUpDirection;
		private static Func<long, bool> _HasWater;
		private static Func<Vector3D, MyCubeSize, long?, float> _GetBuoyancyMultiplier;
		private static Func<long, int> _GetCrushDepth;

		private static Func<long, MyTuple<Vector3D, float, float, float>> _GetPhysicalData;
		private static Func<long, MyTuple<float, float, float, int>> _GetWaveData;
		private static Func<long, MyTuple<Vector3D, bool, bool>> _GetRenderData;
		private static Func<long, MyTuple<float, float>> _GetPhysicsData;
		private static Func<long, MyTuple<float, float>> _GetTideData;
		private static Func<long, Vector3D> _GetTideDirection;

		private static Action<Vector3D, float, bool> _CreateSplash;
		private static Action<Vector3D, float> _CreateBubble;

		/// <summary>
		/// Returns true if the version is compatibile with the API Backend, this is automatically called
		/// </summary>
		public static bool VerifyVersion(int Version, string ModName) => _VerifyVersion?.Invoke(Version, ModName) ?? false;

		/// <summary>
		/// Returns true if the provided planet entity ID has water
		/// </summary>
		public static bool HasWater(long ID) => _HasWater?.Invoke(ID) ?? false;

		/// <summary>
		/// Returns true if the position is underwater
		/// </summary>
		public static bool IsUnderwater(Vector3D Position, long? ID = null) => _IsUnderwater?.Invoke(Position, ID) ?? false;

		/// <summary>
		/// Overwater = 0, ExitsWater = 1, EntersWater = 2, Underwater = 3
		/// </summary>
		public static int LineIntersectsWater(LineD Line, long? ID = null) => _LineIntersectsWater?.Invoke(Line, ID) ?? 0;

		/// <summary>
		/// Overwater = 0, ExitsWater = 1, EntersWater = 2, Underwater = 3
		/// </summary>
		public static void LineIntersectsWater(List<LineD> Lines, ICollection<int> Intersections, long? ID = null) => _LineIntersectsWaterList?.Invoke(Lines, Intersections, ID);

		/// <summary>
		/// Gets the closest water to the provided water
		/// </summary>
		public static long? GetClosestWater(Vector3D Position) => _GetClosestWater?.Invoke(Position) ?? null;

		/// <summary>
		/// Overwater = 0, ExitsWater = 1, EntersWater = 2, Underwater = 3
		/// </summary>
		public static int SphereIntersectsWater(BoundingSphereD Sphere, long? ID = null) => _SphereIntersectsWater?.Invoke(Sphere, ID) ?? 0;

		/// <summary>
		/// Overwater = 0, ExitsWater = 1, EntersWater = 2, Underwater = 3
		/// </summary>
		public static void SphereIntersectsWater(List<BoundingSphereD> Spheres, ICollection<int> Intersections, long? ID = null) => _SphereIntersectsWaterList?.Invoke(Spheres, Intersections, ID);


		/// <summary>
		/// Returns the closest position on the water surface
		/// </summary>
		public static Vector3D GetClosestSurfacePoint(Vector3D Position, long? ID = null) => _GetClosestSurfacePoint?.Invoke(Position, ID) ?? Position;

		/// <summary>
		/// Returns the closest position on the water surface
		/// </summary>
		public static void GetClosestSurfacePoint(List<Vector3D> Positions, ICollection<Vector3D> Points, long? ID = null) => _GetClosestSurfacePointList?.Invoke(Positions, Points, ID);


		/// <summary>
		/// Returns the depth the position is underwater
		/// </summary>
		public static float? GetDepth(Vector3D Position, long? ID = null) => _GetDepth?.Invoke(Position, ID) ?? null;

		/// <summary>
		/// Creates a splash at the provided position
		/// </summary>
		public static void CreateSplash(Vector3D Position, float Radius, bool Audible) => _CreateSplash?.Invoke(Position, Radius, Audible);

		/// <summary>
		/// Creates a bubble at the provided position
		/// </summary>
		public static void CreateBubble(Vector3D Position, float Radius) => _CreateBubble?.Invoke(Position, Radius);

		/// <summary>
		/// Forces the server to sync with the client
		/// </summary>
		public static void ForceSync() => _ForceSync?.Invoke();

		/// <summary>
		/// Simulates a command being run by the client, EX: /wcreate, client must have permissions to run the command
		/// </summary>
		public static void RunCommand(string MessageText) => _RunCommand?.Invoke(MessageText);

		/// <summary>
		/// Gets the up direction at the position
		/// </summary>
		public static Vector3D GetUpDirection(Vector3D Position, long? ID = null) => _GetUpDirection?.Invoke(Position, ID) ?? Vector3D.Up;

		/// <summary>
		/// Gets the buoyancy multiplier to help calculate buoyancy of a grid, used in the final calculation of grid buoyancy.
		/// </summary>
		public static float GetBuoyancyMultiplier(Vector3D Position, MyCubeSize GridSize, long? ID = null) => _GetBuoyancyMultiplier?.Invoke(Position, GridSize, ID) ?? 0;

		/// <summary>
		/// Gets crush depth
		/// </summary>
		public static int GetCrushDepth(long ID) => _GetCrushDepth?.Invoke(ID) ?? 500;

		/// <summary>
		/// Gets position, radius, minimum radius, and maximum radius- in that order.
		/// </summary>
		public static MyTuple<Vector3D, float, float, float> GetPhysical(long ID) => (MyTuple<Vector3D, float, float, float>)(_GetPhysicalData?.Invoke(ID) ?? null);

		/// <summary>
		/// Gets wave height, wave speed, wave scale, and seed- in that order.
		/// </summary>
		public static MyTuple<float, float, float, int> GetWaveData(long ID) => (MyTuple<float, float, float, int>)(_GetWaveData?.Invoke(ID) ?? null);

		/// <summary>
		/// Gets fog color, transparency toggle, and lighting toggle- in that order.
		/// </summary>
		public static MyTuple<Vector3D, bool, bool> GetRenderData(long ID) => (MyTuple<Vector3D, bool, bool>)(_GetRenderData?.Invoke(ID) ?? null);

		/// <summary>
		/// Gets tide height and tide speed- in that order.
		/// </summary>
		public static MyTuple<float, float> GetTideData(long ID) => (MyTuple<float, float>)(_GetTideData?.Invoke(ID) ?? null);

		/// <summary>
		/// Gets tide height and tide speed- in that order.
		/// </summary>
		public static MyTuple<float, float> GetPhysicsData(long ID) => (MyTuple<float, float>)(_GetPhysicsData?.Invoke(ID) ?? null);

		/// <summary>
		/// Gets the direction of high tide, from center of the water to the surface
		/// </summary>
		public static Vector3D GetTideDirection(long ID) => (Vector3D)(_GetTideDirection?.Invoke(ID) ?? null);

		public static void LoadData() {
			MyAPIGateway.Utilities.RegisterMessageHandler(ModHandlerID, ModHandler);
		}

		public static void UnloadData() {
			MyAPIGateway.Utilities.UnregisterMessageHandler(ModHandlerID, ModHandler);
		}

		private static void ModHandler(object obj) {
			if (obj == null) {
				return;
			}

			if (obj is Dictionary<string, Delegate>) {
				ModAPIMethods = (Dictionary<string, Delegate>)obj;
				_VerifyVersion = (Func<int, string, bool>)ModAPIMethods["VerifyVersion"];

				Registered = VerifyVersion(ModAPIVersion, ModName);

				if (Registered) {
					try {
						_IsUnderwater = (Func<Vector3D, long?, bool>)ModAPIMethods["IsUnderwater"];
						_GetClosestWater = (Func<Vector3D, long?>)ModAPIMethods["GetClosestWater"];
						_SphereIntersectsWater = (Func<BoundingSphereD, long?, int>)ModAPIMethods["SphereIntersectsWater"];
						_SphereIntersectsWaterList = (Action<List<BoundingSphereD>, ICollection<int>, long?>)ModAPIMethods["SphereIntersectsWaterList"];
						_GetClosestSurfacePoint = (Func<Vector3D, long?, Vector3D>)ModAPIMethods["GetClosestSurfacePoint"];
						_GetClosestSurfacePointList = (Action<List<Vector3D>, ICollection<Vector3D>, long?>)ModAPIMethods["GetClosestSurfacePointList"];
						_LineIntersectsWater = (Func<LineD, long?, int>)ModAPIMethods["LineIntersectsWater"];
						_LineIntersectsWaterList = (Action<List<LineD>, ICollection<int>, long?>)ModAPIMethods["LineIntersectsWaterList"];
						_GetDepth = (Func<Vector3D, long?, float?>)ModAPIMethods["GetDepth"];
						_CreateSplash = (Action<Vector3D, float, bool>)ModAPIMethods["CreateSplash"];
						_CreateBubble = (Action<Vector3D, float>)ModAPIMethods["CreateBubble"];
						_ForceSync = (Action)ModAPIMethods["ForceSync"];
						_RunCommand = (Action<string>)ModAPIMethods["RunCommand"];
						_GetUpDirection = (Func<Vector3D, long?, Vector3D>)ModAPIMethods["GetUpDirection"];
						_HasWater = (Func<long, bool>)ModAPIMethods["HasWater"];
						_GetBuoyancyMultiplier = (Func<Vector3D, MyCubeSize, long?, float>)ModAPIMethods["GetBuoyancyMultiplier"];
						_GetCrushDepth = (Func<long, int>)ModAPIMethods["GetCrushDepth"];
						_GetPhysicalData = (Func<long, MyTuple<Vector3D, float, float, float>>)ModAPIMethods["GetPhysicalData"];
						_GetWaveData = (Func<long, MyTuple<float, float, float, int>>)ModAPIMethods["GetWaveData"];
						_GetRenderData = (Func<long, MyTuple<Vector3D, bool, bool>>)ModAPIMethods["GetRenderData"];
						_GetPhysicsData = (Func<long, MyTuple<float, float>>)ModAPIMethods["GetPhysicsData"];
						_GetTideData = (Func<long, MyTuple<float, float>>)ModAPIMethods["GetTideData"];
						_GetTideDirection = (Func<long, Vector3D>)ModAPIMethods["GetTideDirection"];
						SpawnLogger.Write("WaterMod API Loaded", SpawnerDebugEnum.Startup);
					} catch (Exception e) {
						MyAPIGateway.Utilities.ShowMessage("WaterMod", "Mod '" + ModName + "' encountered an error when registering the Water Mod API, see log for more info.");
						MyLog.Default.WriteLine(e);
					}
				}
			}
		}
	}
}