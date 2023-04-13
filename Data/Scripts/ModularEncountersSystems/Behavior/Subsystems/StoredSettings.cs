using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ProtoBuf;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems {

	[ProtoContract]
	public class StoredSettings{
		
		[ProtoMember(1)]
		public BehaviorMode Mode;

		[ProtoMember(2)]
		public long CurrentTargetEntityId;

		[ProtoMember(3)]
		public Dictionary<string, bool> StoredCustomBooleans;

		[ProtoMember(4)]
		public Dictionary<string, int> StoredCustomCounters;

		[ProtoMember(5)]
		public List<TriggerProfile> Triggers;

		[ProtoMember(6)]
		public List<TriggerProfile> DamageTriggers;

		[ProtoMember(7)]
		public List<TriggerProfile> CommandTriggers;

		[ProtoMember(8)]
		public float TotalDamageAccumulated;

		[ProtoMember(9)]
		public DateTime LastDamageTakenTime;

		[ProtoMember(10)]
		public string CurrentTargetProfile;

		[ProtoMember(11)]
		public List<TriggerProfile> CompromisedTriggers;

		[ProtoMember(12)]
		public Direction RotationDirection;

		[ProtoMember(13)]
		public SerializableBlockOrientation BlockOrientation;

		[ProtoMember(14)]
		public Vector3D DespawnCoords;

		[ProtoMember(15)]
		public Vector3D StoredCoords;

		[ProtoMember(16)]
		public Vector3D StartCoords;

		[ProtoMember(17)]
		public long LastDamagerEntity;

		[ProtoMember(18)]
		public NewAutoPilotMode AutoPilotFlags;

		[ProtoMember(19)]
		public AutoPilotDataMode APDataMode;

		[ProtoMember(20)]
		public bool IgnoreTriggers;

		[ProtoMember(21)]
		public string PrimaryAutopilotId;

		[ProtoMember(22)]
		public string SecondaryAutopilotId;

		[ProtoMember(23)]
		public string TertiaryAutopilotId;

		[ProtoMember(24)]
		public Vector3D InitialWaypointBackup;

		[ProtoMember(25)]
		public Vector3D PendingWaypointBackup;

		[ProtoMember(26)]
		public AutoPilotState State;

		[ProtoMember(27)]
		public bool DoRetreat;

		[ProtoMember(28)]
		public bool DoDespawn;

		[ProtoMember(29)]
		public bool SubclassBehaviorDefaultsSet;

		[ProtoMember(30)]
		public BehaviorSubclass ActiveBehaviorType;

		[ProtoMember(31)]
		public int HomingWeaponRangeOverride;

		[ProtoMember(32)]
		public Vector3D PatrolOverrideLocation;

		[ProtoMember(33)]
		public List<EscortProfile> ActiveEscorts;

		[ProtoMember(34)]
		internal EscortProfile _parentEscort;

		[ProtoMember(35)]
		public string WeaponsSystemProfile;

		[ProtoMember(36)]
		public string OverrideTargetProfile;

		[ProtoMember(37)]
		public short InitialWeaponCount;

		[ProtoMember(38)]
		public short InitialTurretCount;

		[ProtoMember(39)]
		public short InitialGunCount;

		[ProtoMember(40)]
		public double InitialGridIntegrity; //Block Health = BuildIntegrity - CurrentDamage

		[ProtoMember(41)]
		public long SavedPlayerIdentityId;

		[ProtoIgnore]
		public EscortProfile ParentEscort { get {

				return _parentEscort;

			}
			set {

				_parentEscort = value;
				SpawnLogger.Write(Behavior.RemoteControl.EntityId + " Parent Escort Set Null: " + (_parentEscort == null).ToString(), SpawnerDebugEnum.Dev);
			
			}
		
		}

		[ProtoIgnore]
		public IBehavior Behavior;

		public StoredSettings(){
			
			Mode = BehaviorMode.Init;
			CurrentTargetEntityId = 0;
			StoredCustomBooleans = new Dictionary<string, bool>();
			StoredCustomCounters = new Dictionary<string, int>();

			Triggers = new List<TriggerProfile>();
			DamageTriggers = new List<TriggerProfile>();
			CommandTriggers = new List<TriggerProfile>();
			CompromisedTriggers = new List<TriggerProfile>();

			TotalDamageAccumulated = 0;
			LastDamageTakenTime = MyAPIGateway.Session.GameDateTime;

			CurrentTargetProfile = "";
			OverrideTargetProfile = "";

			RotationDirection = Direction.Forward;
			BlockOrientation = new SerializableBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Up);

			DespawnCoords = Vector3D.Zero;
			StoredCoords = Vector3D.Zero;
			StartCoords = Vector3D.Zero;

			LastDamagerEntity = 0;

			AutoPilotFlags = NewAutoPilotMode.None;
			APDataMode = AutoPilotDataMode.Primary;

			PrimaryAutopilotId = "";
			SecondaryAutopilotId = "";
			TertiaryAutopilotId = "";

			IgnoreTriggers = false;

			State = new AutoPilotState();

			DoRetreat = false;
			DoDespawn = false;

			SubclassBehaviorDefaultsSet = false;
			ActiveBehaviorType = BehaviorSubclass.None;

			HomingWeaponRangeOverride = -1;

			PatrolOverrideLocation = Vector3D.Zero;

			ActiveEscorts = new List<EscortProfile>();
			_parentEscort = null;

			WeaponsSystemProfile = "";

			SavedPlayerIdentityId = 0;

		}

		public StoredSettings(StoredSettings oldSettings, bool preserveSettings, bool preserveTriggers, bool preserveTargetProfile) : base() {

			//Stuff From Old Settings
			if (!preserveSettings)
				return;

			//this.Mode = oldSettings.Mode;
			this.StoredCustomBooleans = oldSettings.StoredCustomBooleans;
			this.StoredCustomCounters = oldSettings.StoredCustomCounters;
			this.TotalDamageAccumulated = oldSettings.TotalDamageAccumulated;
			this.LastDamageTakenTime = oldSettings.LastDamageTakenTime;
			this.RotationDirection = oldSettings.RotationDirection;
			this.BlockOrientation = oldSettings.BlockOrientation;
			this.DespawnCoords = oldSettings.DespawnCoords;
			this.StoredCoords = oldSettings.StoredCoords;
			this.StartCoords = oldSettings.StartCoords;
			this.LastDamagerEntity = oldSettings.LastDamagerEntity;
			this.AutoPilotFlags = oldSettings.AutoPilotFlags;
			this.APDataMode = oldSettings.APDataMode;
			this.DoDespawn = oldSettings.DoDespawn;
			this.DoRetreat = oldSettings.DoRetreat;
			this.HomingWeaponRangeOverride = oldSettings.HomingWeaponRangeOverride;
			this.PatrolOverrideLocation = oldSettings.PatrolOverrideLocation;
			this.ActiveEscorts = oldSettings.ActiveEscorts;
			this.WeaponsSystemProfile = oldSettings.WeaponsSystemProfile;
			this.SavedPlayerIdentityId = oldSettings.SavedPlayerIdentityId;

			//Triggers
			if (preserveTriggers) {

				this.Triggers = oldSettings.Triggers;
				this.DamageTriggers = oldSettings.DamageTriggers;
				this.CommandTriggers = oldSettings.CommandTriggers;
				this.CompromisedTriggers = oldSettings.CompromisedTriggers;

			} else {

				IgnoreTriggers = true;

			}

			//TargetProfile
			if (preserveTargetProfile) {

				this.CurrentTargetProfile = oldSettings.CurrentTargetProfile;
				this.CurrentTargetEntityId = oldSettings.CurrentTargetEntityId;

			}

			this.SetRotation(RotationDirection);

		}

		public bool GetCustomBoolResult(string name){

			if (string.IsNullOrWhiteSpace(name))
				return false;

			bool result = false;
			this.StoredCustomBooleans.TryGetValue(name, out result);
			return result;
			
		}
		
		public bool GetCustomCounterResult(string varName, int target, CounterCompareEnum compareType){

			if (string.IsNullOrWhiteSpace(varName)) {

				//BehaviorLogger.Write("Counter String Name Null", BehaviorDebugEnum.Dev);
				return false;

			}
				

			int result = 0;
			this.StoredCustomCounters.TryGetValue(varName, out result);
			BehaviorLogger.Write(varName + ": " + result.ToString() + " / " + target.ToString(), BehaviorDebugEnum.Condition);

			if(compareType == CounterCompareEnum.GreaterOrEqual)
				return (result >= target);

			if (compareType == CounterCompareEnum.Greater)
				return (result > target);

			if (compareType == CounterCompareEnum.Equal)
				return (result == target);

			if (compareType == CounterCompareEnum.NotEqual)
				return (result != target);

			if (compareType == CounterCompareEnum.Less)
				return (result < target);

			if (compareType == CounterCompareEnum.LessOrEqual)
				return (result <= target);

			return false;

		}
		
		public void SetCustomBool(string name, bool value){

			if (string.IsNullOrWhiteSpace(name))
				return;

			if (this.StoredCustomBooleans.ContainsKey(name)){
				
				this.StoredCustomBooleans[name] = value;
				
			}else{
				
				this.StoredCustomBooleans.Add(name, value);
				
			}
			
		}
		
		public void SetCustomCounter(string name, int value, bool reset = false, bool hardSet = false){

			if (string.IsNullOrWhiteSpace(name))
				return;

			if(this.StoredCustomCounters.ContainsKey(name)){

				if (hardSet) {

					this.StoredCustomCounters[name] = value;
					return;

				}

				if (reset) {

					this.StoredCustomCounters[name] = 0;

				} else {

					//BehaviorLogger.AddMsg("Increased Counter", true);
					this.StoredCustomCounters[name] += value;

				}

			}else{

				//BehaviorLogger.AddMsg("Increased Counter", true);
				this.StoredCustomCounters.Add(name, value);
				
			}
			
		}

		public void SetRotation(Direction newDirection) {

			RotationDirection = newDirection;

			if (RotationDirection == Direction.Forward || RotationDirection == Direction.None)
				BlockOrientation = new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Up);

			if (RotationDirection == Direction.Left)
				BlockOrientation = new MyBlockOrientation(Base6Directions.Direction.Left, Base6Directions.Direction.Up);

			if (RotationDirection == Direction.Backward)
				BlockOrientation = new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Up);

			if (RotationDirection == Direction.Right)
				BlockOrientation = new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Up);

			if (RotationDirection == Direction.Up)
				BlockOrientation = new MyBlockOrientation(Base6Directions.Direction.Up, Base6Directions.Direction.Backward);

			if (RotationDirection == Direction.Down)
				BlockOrientation = new MyBlockOrientation(Base6Directions.Direction.Down, Base6Directions.Direction.Forward);

		}

	}
	
}
	
	