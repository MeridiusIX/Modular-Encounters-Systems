using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Utils;

namespace ModularEncountersSystems.Logging {


	public enum DebugEnum {

		None,
		TempDebug,
		Terminal,

	}

	[Flags]
	public enum SpawnerDebugEnum {

		None = 0,
		API = 1,
		BlockLogic = 1 << 1,
		CleanUp = 1 << 2,
		Error = 1 << 3,
		GameLog = 1 << 4,
		Manipulation = 1 << 5,
		Pathing = 1 << 6,
		Settings = 1 << 7,
		SpawnGroup = 1 << 8,
		Spawning = 1 << 9,
		Startup = 1 << 10,
		Zone = 1 << 11,
		Entity = 1 << 12,
		PostSpawn = 1 << 13,
		Dev = 1 << 14,
		SpawnRecord = 1 << 15,

	}

	public static class SpawnLogger {

		public static bool LoggerDebugMode = false;
		public static bool DebugToGameLog = false;

		//Spawner Debugs
		public static StringBuilder API = new StringBuilder();
		public static StringBuilder BlockLogic = new StringBuilder();
		public static StringBuilder CleanUp = new StringBuilder();
		public static StringBuilder Dev = new StringBuilder();
		public static StringBuilder Entity = new StringBuilder();
		public static StringBuilder Error = new StringBuilder();
		public static StringBuilder Manipulation = new StringBuilder();
		public static StringBuilder Pathing = new StringBuilder();
		public static StringBuilder PostSpawn = new StringBuilder();
		public static StringBuilder Settings = new StringBuilder();
		public static StringBuilder SpawnGroup = new StringBuilder();
		public static StringBuilder Spawning = new StringBuilder();
		public static StringBuilder SpawnRecord = new StringBuilder();
		public static StringBuilder Startup = new StringBuilder();
		public static StringBuilder Zone = new StringBuilder();

		public static List<LoggerQueue> QueuedItems = new List<LoggerQueue>();
		public static Dictionary<string, int> Reason = null;

		//Behavior Debugs

		public static SpawnerDebugEnum ActiveDebug = SpawnerDebugEnum.None;

		public static void Setup() {

			int activeInt = 0;

			if (MyAPIGateway.Utilities.GetVariable<int>("MES-ActiveSpawnerDebug", out activeInt)) {

				ActiveDebug = (SpawnerDebugEnum)activeInt;
				Write("Active Debug Loaded: " + ActiveDebug.ToString(), SpawnerDebugEnum.Startup, true);

			} else {

				Write("Active Debug Not Found", SpawnerDebugEnum.Startup, true);

			}

			if (!ActiveDebug.HasFlag(SpawnerDebugEnum.SpawnRecord)) {

				ActiveDebug |= SpawnerDebugEnum.SpawnRecord;

			}
		
		}

		public static void Queue(string msg, SpawnerDebugEnum type, bool force = false, bool addToReason = false) {

			QueuedItems.Add(new LoggerQueue(msg, type, force));

			if (Reason == null || !addToReason)
				return;

			string trimmedReason = msg.Trim();
			int output = 0;

			if (!Reason.TryGetValue(trimmedReason, out output))
				Reason.Add(trimmedReason, 0);

			Reason[trimmedReason]++;

		}

		public static void ProcessQueue() {

			lock (QueuedItems) {

				for (int i = 0; i < QueuedItems.Count; i++) {

					var item = QueuedItems[i];
					Write(item.message, item.type, item.forceSpawn);

				}

				QueuedItems.Clear();
			
			}
		
		}

		public static bool SetActiveDebugFlag(SpawnerDebugEnum type, bool mode) {

			bool updated = false;

			if (mode && !ActiveDebug.HasFlag(type)) {

				ActiveDebug |= type;
				updated = true;

			} else if (!mode && ActiveDebug.HasFlag(type)) {

				ActiveDebug &= ~type;
				updated = true;

			}

			if (updated) {

				var intVal = (int)ActiveDebug;
				Write(type.ToString() + " / " + intVal, SpawnerDebugEnum.Settings, true);
				MyAPIGateway.Utilities.SetVariable<int>("MES-ActiveSpawnerDebug", intVal);

			}

			return updated;
		
		}

		public static void Write(string msg, SpawnerDebugEnum type, bool forceGameLog = false) {

			if (type == SpawnerDebugEnum.API)
				WriteToBuilder(msg, type, API, forceGameLog);

			if (type == SpawnerDebugEnum.BlockLogic)
				WriteToBuilder(msg, type, BlockLogic, forceGameLog);

			if (type == SpawnerDebugEnum.CleanUp)
				WriteToBuilder(msg, type, CleanUp, forceGameLog);

			if (type == SpawnerDebugEnum.Dev)
				WriteToBuilder(msg, type, Dev, forceGameLog);

			if (type == SpawnerDebugEnum.Entity)
				WriteToBuilder(msg, type, Entity, forceGameLog);

			if (type == SpawnerDebugEnum.Error)
				WriteToBuilder(msg, type, Error, forceGameLog);

			if (type == SpawnerDebugEnum.Manipulation)
				WriteToBuilder(msg, type, Manipulation, forceGameLog);

			if (type == SpawnerDebugEnum.Pathing)
				WriteToBuilder(msg, type, Pathing, forceGameLog);

			if (type == SpawnerDebugEnum.PostSpawn)
				WriteToBuilder(msg, type, PostSpawn, forceGameLog);

			if (type == SpawnerDebugEnum.Settings)
				WriteToBuilder(msg, type, Settings, forceGameLog);

			if (type == SpawnerDebugEnum.SpawnGroup)
				WriteToBuilder(msg, type, SpawnGroup, forceGameLog);

			if (type == SpawnerDebugEnum.Spawning)
				WriteToBuilder(msg, type, Spawning, forceGameLog);

			if (type == SpawnerDebugEnum.SpawnRecord)
				WriteToBuilder(msg, type, SpawnRecord, forceGameLog);

			if (type == SpawnerDebugEnum.Startup)
				WriteToBuilder(msg, type, Startup, forceGameLog);

			if (type == SpawnerDebugEnum.Zone)
				WriteToBuilder(msg, type, Zone, forceGameLog);

		}

		public static void WriteToBuilder(string msg, SpawnerDebugEnum type, StringBuilder sb, bool forceGameLog) {

			if (!ActiveDebug.HasFlag(type) && !forceGameLog)
				return;

			LoggerTools.AppendDateAndTime(sb);
			sb.Append("MES / ").Append(type.ToString()).Append(": ").Append(msg).AppendLine();
			WriteToGameLog(msg, type, forceGameLog);

		}

		public static void WriteToGameLog(string msg, SpawnerDebugEnum type, bool forceGameLog) {

			if (!ActiveDebug.HasFlag(SpawnerDebugEnum.GameLog) && !forceGameLog)
				return;

			MyLog.Default.WriteLineAndConsole("MES Spawner / " + type.ToString() + ": " + msg);
		
		}

		

	}

}
