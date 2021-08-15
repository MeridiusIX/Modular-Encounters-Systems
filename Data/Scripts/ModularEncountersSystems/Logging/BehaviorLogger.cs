using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Utils;

namespace ModularEncountersSystems.Logging {


	[Flags]
	public enum BehaviorDebugEnum {

		None = 0,
		Action = 1,
		AutoPilot = 1 << 1,
		BehaviorMode = 1 << 2,
		BehaviorSetup = 1 << 3,
		BehaviorSpecific = 1 << 4,
		Chat = 1 << 5,
		Command = 1 << 6,
		Condition = 1 << 7,
		Collision = 1 << 8,
		Despawn = 1 << 9,
		Error = 1 << 10,
		GameLog = 1 << 11,
		General = 1 << 12,
		Owner = 1 << 13,
		Settings = 1 << 14,
		Spawn = 1 << 15,
		Startup = 1 << 16,
		TargetAcquisition = 1 << 17,
		TargetEvaluation = 1 << 18,
		Thrust = 1 << 19,
		Trigger = 1 << 20,
		Weapon = 1 << 21,
		Target = 1 << 22,
		Dev = 1 << 23,
		BehaviorLog = 1 << 24,

	}

	public static class BehaviorLogger {

		public static bool LoggerDebugMode = false;
		public static bool DebugToGameLog = false;

		//Behavior Debugs
		public static StringBuilder Action = new StringBuilder();
		public static StringBuilder AutoPilot = new StringBuilder();
		public static StringBuilder BehaviorMode = new StringBuilder();
		public static StringBuilder BehaviorSetup = new StringBuilder();
		public static StringBuilder BehaviorSpecific = new StringBuilder();
		public static StringBuilder Chat = new StringBuilder();
		public static StringBuilder Command = new StringBuilder();
		public static StringBuilder Condition = new StringBuilder();
		public static StringBuilder Collision = new StringBuilder();
		public static StringBuilder Despawn = new StringBuilder();
		public static StringBuilder Dev = new StringBuilder();
		public static StringBuilder Error = new StringBuilder();
		public static StringBuilder General = new StringBuilder();
		public static StringBuilder Owner = new StringBuilder();
		public static StringBuilder Spawn = new StringBuilder();
		public static StringBuilder Startup = new StringBuilder();
		public static StringBuilder TargetAcquisition = new StringBuilder();
		public static StringBuilder TargetEvaluation = new StringBuilder();
		public static StringBuilder Thrust = new StringBuilder();
		public static StringBuilder Trigger = new StringBuilder();
		public static StringBuilder Weapon = new StringBuilder();

		public static BehaviorDebugEnum ActiveDebug = BehaviorDebugEnum.None;

		public static void Setup() {

			int activeInt = 0;

			if (MyAPIGateway.Utilities.GetVariable<int>("MES-ActiveBehaviorDebug", out activeInt)) {

				ActiveDebug = (BehaviorDebugEnum)activeInt;
				Write("Active Behavior Debug Loaded: " + ActiveDebug.ToString(), BehaviorDebugEnum.Startup, true);

			} else {

				Write("Active Debug Not Found", BehaviorDebugEnum.Startup, true);

			}
		
		}

		public static bool SetActiveDebugFlag(BehaviorDebugEnum type, bool mode) {

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
				Write(type.ToString() + " / " + intVal, BehaviorDebugEnum.Settings, true);
				MyAPIGateway.Utilities.SetVariable<int>("MES-ActiveBehaviorDebug", intVal);

			}

			return updated;
		
		}

		public static void Write(string msg, BehaviorDebugEnum type, bool forceGameLog = false) {

			if (type == BehaviorDebugEnum.Action)
				WriteToBuilder(msg, type, Action, forceGameLog);

			if (type == BehaviorDebugEnum.AutoPilot)
				WriteToBuilder(msg, type, AutoPilot, forceGameLog);

			if (type == BehaviorDebugEnum.BehaviorMode)
				WriteToBuilder(msg, type, BehaviorMode, forceGameLog);

			if (type == BehaviorDebugEnum.BehaviorSetup)
				WriteToBuilder(msg, type, BehaviorSetup, forceGameLog);

			if (type == BehaviorDebugEnum.BehaviorSpecific)
				WriteToBuilder(msg, type, BehaviorSpecific, forceGameLog);

			if (type == BehaviorDebugEnum.Chat)
				WriteToBuilder(msg, type, Chat, forceGameLog);

			if (type == BehaviorDebugEnum.Command)
				WriteToBuilder(msg, type, Command, forceGameLog);

			if (type == BehaviorDebugEnum.Condition)
				WriteToBuilder(msg, type, Condition, forceGameLog);

			if (type == BehaviorDebugEnum.Collision)
				WriteToBuilder(msg, type, Collision, forceGameLog);

			if (type == BehaviorDebugEnum.Despawn)
				WriteToBuilder(msg, type, Despawn, forceGameLog);

			if (type == BehaviorDebugEnum.Dev)
				WriteToBuilder(msg, type, Dev, forceGameLog);

			if (type == BehaviorDebugEnum.Error)
				WriteToBuilder(msg, type, Error, forceGameLog);

			if (type == BehaviorDebugEnum.General)
				WriteToBuilder(msg, type, General, forceGameLog);

			if (type == BehaviorDebugEnum.Owner)
				WriteToBuilder(msg, type, Owner, forceGameLog);

			if (type == BehaviorDebugEnum.Spawn)
				WriteToBuilder(msg, type, Spawn, forceGameLog);

			if (type == BehaviorDebugEnum.Startup)
				WriteToBuilder(msg, type, Startup, forceGameLog);

			if (type == BehaviorDebugEnum.TargetAcquisition)
				WriteToBuilder(msg, type, TargetAcquisition, forceGameLog);

			if (type == BehaviorDebugEnum.TargetEvaluation)
				WriteToBuilder(msg, type, TargetEvaluation, forceGameLog);

			if (type == BehaviorDebugEnum.Thrust)
				WriteToBuilder(msg, type, Thrust, forceGameLog);

			if (type == BehaviorDebugEnum.Trigger)
				WriteToBuilder(msg, type, Trigger, forceGameLog);

			if (type == BehaviorDebugEnum.Weapon)
				WriteToBuilder(msg, type, Weapon, forceGameLog);

		}

		public static void WriteToBuilder(string msg, BehaviorDebugEnum type, StringBuilder sb, bool forceGameLog) {

			if (!ActiveDebug.HasFlag(type) && !forceGameLog)
				return;

			LoggerTools.AppendDateAndTime(sb);
			sb.Append("MES Behavior / ").Append(type.ToString()).Append(": ").Append(msg).AppendLine();
			WriteToGameLog(msg, type, forceGameLog);

		}

		public static void WriteToGameLog(string msg, BehaviorDebugEnum type, bool forceGameLog) {

			if (!ActiveDebug.HasFlag(BehaviorDebugEnum.GameLog) && !forceGameLog)
				return;

			MyLog.Default.WriteLineAndConsole("MES / " + type.ToString() + ": " + msg);
		
		}


	}

}
