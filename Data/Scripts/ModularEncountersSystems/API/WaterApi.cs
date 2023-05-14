using ModularEncountersSystems.Logging;
using Sandbox.Game.Entities;
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
    //See the steam guide for how to use this
    //https://steamcommunity.com/sharedfiles/filedetails/?id=2639207010
    /// <summary>
    /// https://github.com/jakarianstudios/SE-Water/blob/master/API/WaterModAPI.cs
    /// </summary>

    public class WaterAPI {
        public static string ModName = "";
        public const ushort ModHandlerID = 50271;
        public const int ModAPIVersion = 19;
        public static bool Registered { get; private set; } = false;

        private static Dictionary<string, Delegate> ModAPIMethods;

        private static Func<int, string, bool> _VerifyVersion;

        private static Func<Vector3D, MyPlanet, bool> _IsUnderwater;
        private static Func<LineD, MyPlanet, int> _LineIntersectsWater;
        private static Action<List<LineD>, ICollection<int>, MyPlanet> _LineIntersectsWaterList;
        private static Func<Vector3D, MyPlanet> _GetClosestWater;
        private static Func<BoundingSphereD, MyPlanet, int> _SphereIntersectsWater;
        private static Action<List<BoundingSphereD>, ICollection<int>, MyPlanet> _SphereIntersectsWaterList;
        private static Func<Vector3D, MyPlanet, Vector3D> _GetClosestSurfacePoint;
        private static Action<List<Vector3D>, ICollection<Vector3D>, MyPlanet> _GetClosestSurfacePointList;
        private static Func<Vector3D, MyPlanet, float?> _GetDepth;
        private static Action _ForceSync;
        private static Action<string> _RunCommand;
        private static Func<Vector3D, MyPlanet, Vector3D> _GetUpDirection;
        private static Func<MyPlanet, bool> _HasWater;
        private static Func<Vector3D, MyCubeSize, MyPlanet, float> _GetBuoyancyMultiplier;
        private static Func<MyPlanet, int> _GetCrushDepth;

        private static Func<MyPlanet, MyTuple<Vector3D, float, float, float>> _GetPhysicalData;
        private static Func<MyPlanet, MyTuple<float, float, float, int>> _GetWaveData;
        private static Func<MyPlanet, MyTuple<Vector3D, bool, bool>> _GetRenderData;
        private static Func<MyPlanet, MyTuple<float, float>> _GetPhysicsData;
        private static Func<MyPlanet, MyTuple<float, float>> _GetTideData;
        private static Func<MyPlanet, Vector3D> _GetTideDirection;

        private static Action<Vector3D, float, bool> _CreateSplash;
        private static Action<Vector3D, float> _CreateBubble;
        private static Action<Vector3D, Vector3D, float, int> _CreatePhysicsSplash;

        /// <summary>
        /// Returns true if the version is compatibile with the API Backend, this is automatically called
        /// </summary>
        public static bool VerifyVersion(int Version, string ModName) => _VerifyVersion?.Invoke(Version, ModName) ?? false;

        /// <summary>
        /// Returns true if the provided planet entity ID has water
        /// </summary>
        public static bool HasWater(MyPlanet planet) => _HasWater?.Invoke(planet) ?? false;

        /// <summary>
        /// Returns true if the position is underwater
        /// </summary>
        public static bool IsUnderwater(Vector3D Position, MyPlanet ID = null) => _IsUnderwater?.Invoke(Position, ID) ?? false;

        /// <summary>
        /// Overwater = 0, ExitsWater = 1, EntersWater = 2, Underwater = 3
        /// </summary>
        public static int LineIntersectsWater(LineD Line, MyPlanet ID = null) => _LineIntersectsWater?.Invoke(Line, ID) ?? 0;

        /// <summary>
        /// Overwater = 0, ExitsWater = 1, EntersWater = 2, Underwater = 3
        /// </summary>
        public static void LineIntersectsWater(List<LineD> Lines, ICollection<int> Intersections, MyPlanet ID = null) => _LineIntersectsWaterList?.Invoke(Lines, Intersections, ID);

        /// <summary>
        /// Gets the closest water to the provided water
        /// </summary>
        public static MyPlanet GetClosestWater(Vector3D Position) => _GetClosestWater?.Invoke(Position) ?? null;

        /// <summary>
        /// Overwater = 0, ExitsWater = 1, EntersWater = 2, Underwater = 3
        /// </summary>
        public static int SphereIntersectsWater(BoundingSphereD Sphere, MyPlanet ID = null) => _SphereIntersectsWater?.Invoke(Sphere, ID) ?? 0;

        /// <summary>
        /// Overwater = 0, ExitsWater = 1, EntersWater = 2, Underwater = 3
        /// </summary>
        public static void SphereIntersectsWater(List<BoundingSphereD> Spheres, ICollection<int> Intersections, MyPlanet ID = null) => _SphereIntersectsWaterList?.Invoke(Spheres, Intersections, ID);


        /// <summary>
        /// Returns the closest position on the water surface
        /// </summary>
        public static Vector3D GetClosestSurfacePoint(Vector3D Position, MyPlanet ID = null) => _GetClosestSurfacePoint?.Invoke(Position, ID) ?? Position;

        /// <summary>
        /// Returns the closest position on the water surface
        /// </summary>
        public static void GetClosestSurfacePoint(List<Vector3D> Positions, ICollection<Vector3D> Points, MyPlanet ID = null) => _GetClosestSurfacePointList?.Invoke(Positions, Points, ID);


        /// <summary>
        /// Returns the depth the position is underwater
        /// </summary>
        public static float? GetDepth(Vector3D Position, MyPlanet ID = null) => _GetDepth?.Invoke(Position, ID) ?? null;

        /// <summary>
        /// Creates a splash at the provided position
        /// </summary>
        public static void CreateSplash(Vector3D Position, float Radius, bool Audible) => _CreateSplash?.Invoke(Position, Radius, Audible);

        /// <summary>
        /// Creates a physical splash at the provided position (Particles outside of the water)
        /// </summary>
        public static void CreatePhysicsSplash(Vector3D Position, Vector3D Velocity, float Radius, int Count = 1) => _CreatePhysicsSplash?.Invoke(Position, Velocity, Radius, Count);

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
        public static Vector3D GetUpDirection(Vector3D Position, MyPlanet ID = null) => _GetUpDirection?.Invoke(Position, ID) ?? Vector3D.Up;

        /// <summary>
        /// Gets the buoyancy multiplier to help calculate buoyancy of a grid, used in the final calculation of grid buoyancy.
        /// </summary>
        public static float GetBuoyancyMultiplier(Vector3D Position, MyCubeSize GridSize, MyPlanet ID = null) => _GetBuoyancyMultiplier?.Invoke(Position, GridSize, ID) ?? 0;

        /// <summary>
        /// Gets crush damage
        /// </summary>
        [Obsolete]
        public static float GetCrushDepth(MyPlanet planet) => _GetCrushDepth?.Invoke(planet) ?? 500;

        /// <summary>
        /// Gets position, radius, minimum radius, and maximum radius- in that order.
        /// </summary>
        public static MyTuple<Vector3D, float, float, float> GetPhysical(MyPlanet planet) => (MyTuple<Vector3D, float, float, float>)(_GetPhysicalData?.Invoke(planet) ?? null);

        /// <summary>
        /// Gets wave height, wave speed, wave scale, and seed- in that order.
        /// </summary>
        public static MyTuple<float, float, float, int> GetWaveData(MyPlanet planet) => (MyTuple<float, float, float, int>)(_GetWaveData?.Invoke(planet) ?? null);

        /// <summary>
        /// Gets fog color, transparency toggle, and lighting toggle- in that order.
        /// </summary>
        public static MyTuple<Vector3D, bool, bool> GetRenderData(MyPlanet planet) => (MyTuple<Vector3D, bool, bool>)(_GetRenderData?.Invoke(planet) ?? null);

        /// <summary>
        /// Gets tide height and tide speed- in that order.
        /// </summary>
        public static MyTuple<float, float> GetTideData(MyPlanet planet) => (MyTuple<float, float>)(_GetTideData?.Invoke(planet) ?? null);

        /// <summary>
        /// Gets density and buoyancy multiplier- in that order.
        /// </summary>
        public static MyTuple<float, float> GetPhysicsData(MyPlanet planet) => (MyTuple<float, float>)(_GetPhysicsData?.Invoke(planet) ?? null);

        /// <summary>
        /// Gets the direction of high tide, from center of the water to the surface
        /// </summary>
        public static Vector3D GetTideDirection(MyPlanet planet) => (Vector3D)(_GetTideDirection?.Invoke(planet) ?? null);

        /// <summary>
        /// Do not use. This is for the session component to register automatically
        /// </summary>
        public static void LoadData() {
            Register();
        }

        /// <summary>
        /// Do not use. This is for the session component to register automatically
        /// </summary>
        protected static void UnloadData() {
            Unregister();
        }

        /// <summary>
        /// Registers the mod and sets the mod name if it is not already set
        /// </summary>
        public static void Register() {
            MyAPIGateway.Utilities.RegisterMessageHandler(ModHandlerID, ModHandler);

            if (ModName == "") {
                if (MyAPIGateway.Utilities.GamePaths.ModScopeName.Contains("_"))
                    ModName = MyAPIGateway.Utilities.GamePaths.ModScopeName.Split('_')[1];
                else
                    ModName = MyAPIGateway.Utilities.GamePaths.ModScopeName;
            }
        }

        /// <summary>
        /// Unregisters the mod
        /// </summary>
        public static void Unregister() {
            MyAPIGateway.Utilities.UnregisterMessageHandler(ModHandlerID, ModHandler);
            Registered = false;
        }

        private static void ModHandler(object obj) {
            if (obj == null) {
                return;
            }

            if (obj is Dictionary<string, Delegate>) {
                ModAPIMethods = (Dictionary<string, Delegate>)obj;
                _VerifyVersion = (Func<int, string, bool>)ModAPIMethods["VerifyVersion"];

                Registered = VerifyVersion(ModAPIVersion, ModName);

                MyLog.Default.WriteLine("Registering WaterAPI for Mod '" + ModName + "'");

                if (Registered) {
                    try {
                        _IsUnderwater = (Func<Vector3D, MyPlanet, bool>)ModAPIMethods["IsUnderwater"];
                        _GetClosestWater = (Func<Vector3D, MyPlanet>)ModAPIMethods["GetClosestWater"];
                        _SphereIntersectsWater = (Func<BoundingSphereD, MyPlanet, int>)ModAPIMethods["SphereIntersectsWater"];
                        _SphereIntersectsWaterList = (Action<List<BoundingSphereD>, ICollection<int>, MyPlanet>)ModAPIMethods["SphereIntersectsWaterList"];
                        _GetClosestSurfacePoint = (Func<Vector3D, MyPlanet, Vector3D>)ModAPIMethods["GetClosestSurfacePoint"];
                        _GetClosestSurfacePointList = (Action<List<Vector3D>, ICollection<Vector3D>, MyPlanet>)ModAPIMethods["GetClosestSurfacePointList"];
                        _LineIntersectsWater = (Func<LineD, MyPlanet, int>)ModAPIMethods["LineIntersectsWater"];
                        _LineIntersectsWaterList = (Action<List<LineD>, ICollection<int>, MyPlanet>)ModAPIMethods["LineIntersectsWaterList"];
                        _GetDepth = (Func<Vector3D, MyPlanet, float?>)ModAPIMethods["GetDepth"];
                        _CreateSplash = (Action<Vector3D, float, bool>)ModAPIMethods["CreateSplash"];
                        _CreatePhysicsSplash = (Action<Vector3D, Vector3D, float, int>)ModAPIMethods["CreatePhysicsSplash"];
                        _CreateBubble = (Action<Vector3D, float>)ModAPIMethods["CreateBubble"];
                        _ForceSync = (Action)ModAPIMethods["ForceSync"];
                        _RunCommand = (Action<string>)ModAPIMethods["RunCommand"];
                        _GetUpDirection = (Func<Vector3D, MyPlanet, Vector3D>)ModAPIMethods["GetUpDirection"];
                        _HasWater = (Func<MyPlanet, bool>)ModAPIMethods["HasWater"];
                        _GetBuoyancyMultiplier = (Func<Vector3D, MyCubeSize, MyPlanet, float>)ModAPIMethods["GetBuoyancyMultiplier"];
                        _GetCrushDepth = (Func<MyPlanet, int>)ModAPIMethods["GetCrushDepth"];
                        _GetPhysicalData = (Func<MyPlanet, MyTuple<Vector3D, float, float, float>>)ModAPIMethods["GetPhysicalData"];
                        _GetWaveData = (Func<MyPlanet, MyTuple<float, float, float, int>>)ModAPIMethods["GetWaveData"];
                        _GetRenderData = (Func<MyPlanet, MyTuple<Vector3D, bool, bool>>)ModAPIMethods["GetRenderData"];
                        _GetPhysicsData = (Func<MyPlanet, MyTuple<float, float>>)ModAPIMethods["GetPhysicsData"];
                        _GetTideData = (Func<MyPlanet, MyTuple<float, float>>)ModAPIMethods["GetTideData"];
                        _GetTideDirection = (Func<MyPlanet, Vector3D>)ModAPIMethods["GetTideDirection"];
                    } catch (Exception e) {
                        MyAPIGateway.Utilities.ShowMessage("WaterMod", "Mod '" + ModName + "' encountered an error when registering the Water Mod API, see log for more info.");
                        MyLog.Default.WriteLine("WaterMod: " + e);
                    }
                }
            }
        }
    }
}