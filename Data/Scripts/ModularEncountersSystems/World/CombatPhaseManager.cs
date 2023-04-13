using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Tasks;
using ProtoBuf;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.World {

	public static class CombatPhaseManager {

		internal static CombatPhaseData _data;

		public static bool Active { 

			get {

				if (_data == null)
					return false;

				return _data.Active;

			}
			set {

				_data.ForcePhaseChange(value);	
				
			}
		}

		public static void Setup() {

			string stringData = null;
			MyAPIGateway.Utilities.GetVariable<string>("MES-CombatPhaseData", out stringData);
			bool restoredData = false;

			if (stringData != null) {

				var byteData = Convert.FromBase64String(stringData);

				if (byteData != null) {

					_data = MyAPIGateway.Utilities.SerializeFromBinary<CombatPhaseData>(byteData);

					if (_data != null)
						restoredData = true;
				
				}
			
			}

			if (!restoredData) {

				_data = new CombatPhaseData();
				Save();
			
			}

			TaskProcessor.Tick60.Tasks += ProcessCombatPhase;
			MES_SessionCore.UnloadActions += Unload;
			MES_SessionCore.SaveActions += Save;

		}

		public static void ProcessCombatPhase() {

			if (!Settings.Combat.EnableCombatPhaseSystem) {

				if (_data != null && _data.Active) {

					_data.Active = false;
					Save();
				
				}

				return;
			
			}

			if (_data.HasPhaseChanged() && Settings.Combat.AnnouncePhaseChanges) {

				AnnouncePhaseChange();

			}
		
		}

		public static void AnnouncePhaseChange() {

			string announce = _data.Active ? "Combat Phase is now Active" : "Combat Phase has Ended";
			string color = _data.Active ? "Red" : "Green";
			MyVisualScriptLogicProvider.ShowNotificationToAll(announce, 5000, color);
		
		}

		public static void ForcePhase(bool active) {

			if (_data != null) {

				_data.ForcePhaseChange(active);

				if (Settings.Combat.AnnouncePhaseChanges) {

					AnnouncePhaseChange();

				}

			}
				
		}

		internal static void Save() {

			var byteData = MyAPIGateway.Utilities.SerializeToBinary<CombatPhaseData>(_data);
			var stringData = Convert.ToBase64String(byteData);
			MyAPIGateway.Utilities.SetVariable<string>("MES-CombatPhaseData", stringData);
		
		}

		public static void Unload() {

			TaskProcessor.Tick60.Tasks -= ProcessCombatPhase;
			MES_SessionCore.UnloadActions -= Unload;
			MES_SessionCore.SaveActions -= Save;

		}
	
	}

	[ProtoContract]
	public class CombatPhaseData {

		[ProtoMember(1)] public bool Active;

		[ProtoMember(2)] internal int CombatTimeElapsed;
		[ProtoMember(3)] internal int CombatTimeTrigger;

		[ProtoMember(4)] internal int PeaceTimeElapsed;
		[ProtoMember(5)] internal int PeaceTimeTrigger;

		public CombatPhaseData() {

			Active = false;
			ResetTimers();
		
		}

		public void ForcePhaseChange(bool phase) {

			Active = phase;
			ResetTimers();

		}

		public bool HasPhaseChanged() {

			if (!Active) {

				PeaceTimeElapsed++;

				if (PeaceTimeElapsed >= PeaceTimeTrigger) {

					Active = true;
					ResetTimers();
					return true;
				
				}


			} else {

				CombatTimeElapsed++;

				if (CombatTimeElapsed >= CombatTimeTrigger) {

					Active = false;
					ResetTimers();
					return true;

				}

			}

			return false;
		
		}

		internal void ResetTimers() {

			PeaceTimeTrigger = MathTools.RandomBetween(Settings.Combat.MinPeacePhaseSeconds, Settings.Combat.MaxPeacePhaseSeconds);
			CombatTimeTrigger = MathTools.RandomBetween(Settings.Combat.MinCombatPhaseSeconds, Settings.Combat.MaxCombatPhaseSeconds);
			PeaceTimeElapsed = 0;
			CombatTimeElapsed = 0;

		}
	
	}

}
