using ProtoBuf;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;

namespace ModularEncountersSystems.API {
	//Only Include this file in your project
	public class NebulaAPI {

		private static WeatherBuilder[] CustomWeathers = new WeatherBuilder[]
		{
			//Put any Custom Weathers you want here
			/*new WeatherBuilder()
			{
				 Name = "Example",
				 MinLightningFrequency = 5,
				 MaxLightningFrequency = 10,

				Lightning = new LightningBuilder()
				{
					MaxLife = 25,
					BoltParts = 50,
					BoltVariation = 100,
					BoltRadius = 5,
					Color = Vector4.One * 3,
				},

				RadiationCharacterDamage = 20,
				AmbientSound = "JGeigerAmbient",
				DisableDampenersCharacter  = true,
				DisableDampenersGrid = true,
				RenderIons = true,
				AmbientRadiationAmount = 5,
				DamageRadiationAmount = 200,
				BlocksToDisable = new string[]
				{
					"MyObjectBuilder_JumpDrive"
				},
				HudWarning = "If this is enabled go yell at jakaria :/",
				Weight = 1,
				CharacterWindForce = 0,
				GridWindForce = 0,
				DustAmount = 0,
				GridDragForce = 0,
				CharacterDragForce = 0,
			},*/
		};

		//Do not touch anything else

		public static string ModName = MyAPIGateway.Utilities.GamePaths.ModScopeName.Split('_')[1];
		public const ushort ModHandlerID = 13377;
		public const ushort ModHandlerIDWeather = 13378;
		public const int ModAPIVersion = 4;
		public static bool Registered { get; private set; } = false;

		private static Dictionary<string, Delegate> ModAPIMethods;

		private static Func<int, string, bool> _VerifyVersion;
		private static Func<Vector3D, bool> _InsideNebulaBounding;
		private static Func<Vector3D, bool> _InsideNebula;
		private static Func<Vector3D, float> _GetNebulaDensity;
		private static Func<Vector3D, float> _GetMaterial;
		private static Action<Vector3D> _CreateLightning;
		private static Func<Vector3D, string, bool, bool> _CreateWeather;
		private static Func<Vector3D, bool> _CreateRandomWeather;
		private static Func<Vector3D, bool> _RemoveWeather;
		private static Func<Vector3D, string> _GetWeather;
		private static Action<int?> _ForceRenderRadiation;
		private static Action<bool?> _ForceRenderIons;
		private static Action<string> _RunCommand;

		/// <summary>
		/// Returns true if the version is compatibile with the API Backend, this is automatically called
		/// </summary>
		public static bool VerifyVersion(int Version, string ModName) => _VerifyVersion?.Invoke(Version, ModName) ?? false;

		/// <summary>
		/// Returns true if the position is inside of a nebula's bounding box
		/// </summary>
		public static bool InsideNebulaBounding(Vector3D Position) => _InsideNebulaBounding?.Invoke(Position) ?? false;

		/// <summary>
		/// Returns true if the position is inside of a nebula cloud
		/// </summary>
		public static bool InsideNebula(Vector3D Position) => _InsideNebula?.Invoke(Position) ?? false;

		/// <summary>
		/// Returns the density (0-1) of the nebula at the position
		/// </summary>
		public static float GetNebulaDensity(Vector3D Position) => _GetNebulaDensity?.Invoke(Position) ?? 0;

		/// <summary>
		/// Returns the ratio of the primary and secondary color, values closer to 0 are primary and 1 are secondary
		/// </summary>
		public static float GetMaterial(Vector3D Position) => _GetMaterial?.Invoke(Position) ?? 0;

		/// <summary>
		/// Creates a lightning at the provided position
		/// </summary>
		public static void CreateLightning(Vector3D Position) => _CreateLightning?.Invoke(Position);

		/// <summary>
		/// Creates a weather at the provided position with the weather string, returns false if not possible
		/// </summary>
		public static bool CreateWeather(Vector3D Position, string Weather, bool Natural) => _CreateWeather?.Invoke(Position, Weather, Natural) ?? false;

		/// <summary>
		/// Creates a random weather, returns false if not possible
		/// </summary>
		public static bool CreateRandomWeather(Vector3D Position) => _CreateRandomWeather?.Invoke(Position) ?? false;

		/// <summary>
		/// Removes the weather at the position, returns false if no weather is found
		/// </summary>
		public static bool RemoveWeather(Vector3D Position) => _RemoveWeather?.Invoke(Position) ?? false;

		/// <summary>
		/// Gets the current weather at the provided position
		/// </summary>
		public static string GetWeather(Vector3D Position) => _GetWeather?.Invoke(Position) ?? null;

		/// <summary>
		/// Forces Radiation to render with the provided amount, set null to reset to default
		/// </summary>
		public static void ForceRenderRadiation(int? Amount) => _ForceRenderRadiation?.Invoke(Amount);

		/// <summary>
		/// Forces Ions to render, set null to reset
		/// </summary>
		public static void ForceRenderIons(bool? Enabled) => _ForceRenderIons?.Invoke(Enabled);

		/// <summary>
		/// Simulates the player running a command
		/// </summary>
		public static void RunCommand(string Command) => _RunCommand?.Invoke(Command);

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
			}

			Registered = VerifyVersion(ModAPIVersion, ModName);

			if (Registered) {
				try {
					_InsideNebulaBounding = (Func<Vector3D, bool>)ModAPIMethods["InsideNebulaBounding"];
					_InsideNebula = (Func<Vector3D, bool>)ModAPIMethods["InsideNebula"];
					_GetNebulaDensity = (Func<Vector3D, float>)ModAPIMethods["GetNebulaDensity"];
					_GetMaterial = (Func<Vector3D, float>)ModAPIMethods["GetMaterial"];
					_CreateLightning = (Action<Vector3D>)ModAPIMethods["CreateLightning"];
					_CreateWeather = (Func<Vector3D, string, bool, bool>)ModAPIMethods["CreateWeather"];
					_CreateRandomWeather = (Func<Vector3D, bool>)ModAPIMethods["CreateRandomWeather"];
					_RemoveWeather = (Func<Vector3D, bool>)ModAPIMethods["RemoveWeather"];
					_GetWeather = (Func<Vector3D, string>)ModAPIMethods["GetWeather"];
					_ForceRenderRadiation = (Action<int?>)ModAPIMethods["ForceRenderRadiation"];
					_ForceRenderIons = (Action<bool?>)ModAPIMethods["ForceRenderIons"];
					_RunCommand = (Action<string>)ModAPIMethods["RunCommand"];
				} catch (Exception e) {
					MyAPIGateway.Utilities.ShowMessage("NebulaMod", "Mod '" + ModName + "' encountered an error when registering the Nebula Mod API, see log for more info.");
					MyLog.Default.WriteLine(e);
				}

				if (CustomWeathers.Length > 0) {
					MyAPIGateway.Utilities.SendModMessage(ModHandlerIDWeather, MyAPIGateway.Utilities.SerializeToBinary(CustomWeathers));
				}
			}
		}

		[ProtoContract]
		public class WeatherBuilder {
			[ProtoMember(1)]
			public string Name;

			//public readonly MyFogProperties FogProperties;
			//public MyParticleEffect ParticleEffect;

			[ProtoMember(5)]
			public int MinLightningFrequency;

			[ProtoMember(6)]
			public int MaxLightningFrequency;

			[ProtoMember(7)]
			public LightningBuilder Lightning;

			[ProtoMember(10)]
			public float RadiationCharacterDamage;
			[ProtoMember(11)]
			public string AmbientSound;

			[ProtoIgnore, XmlIgnore]
			public MySoundPair AmbientSoundPair;

			[ProtoMember(15), Obsolete]
			public bool ForceDisableDampeners;

			[ProtoMember(16)]
			public bool DisableDampenersGrid;

			[ProtoMember(17)]
			public bool DisableDampenersCharacter;

			[ProtoMember(20)]
			public bool RenderIons;

			[ProtoMember(21)]
			public bool RenderComets;

			[ProtoMember(25)]
			public int AmbientRadiationAmount;
			[ProtoMember(26)]
			public int DamageRadiationAmount;

			[ProtoMember(30)]
			public string[] BlocksToDisable;

			[ProtoMember(35)]
			public string HudWarning;

			[ProtoMember(36)]
			public int Weight;

			[ProtoMember(40)]
			public float CharacterWindForce;

			[ProtoMember(41)]
			public float GridWindForce;

			[ProtoMember(45)]
			public int DustAmount;

			[ProtoMember(46)]
			public float GridDragForce;

			[ProtoMember(47)]
			public float CharacterDragForce;
		}

		[ProtoContract]
		public class LightningBuilder {
			[ProtoMember(1)]
			public int MaxLife = 25;

			[ProtoMember(5)]
			public int BoltParts = 50;

			[ProtoMember(10)]
			public int BoltVariation = 100;

			[ProtoMember(15)]
			public int BoltRadius = 5;

			[ProtoMember(20)]
			public Vector4 Color = Vector4.One * 3;
		}
	}
}