using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Zones;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger {

	public partial class TriggerSystem {

		//PlayerNear
		public bool CheckPlayerNear(TriggerProfile trigger) {

			return IsPlayerNearby(trigger);
		}


		public bool CheckPlayerNearPlayerCondition(TriggerProfile trigger)
		{
			return IsPlayerNearbyWithPlayerConditon(trigger);
		}





		//PlayerFar
		public bool CheckPlayerFar(TriggerProfile trigger) {
			return IsPlayerNearby(trigger, true);
		}

		public bool CheckPlayerFarPlayerCondition(TriggerProfile trigger)
		{
			return IsPlayerNearbyWithPlayerConditon(trigger, true);

		}




		//TargetNear
		public bool CheckTargetNear(TriggerProfile trigger) {

			bool hasTarget = _behavior.AutoPilot.Targeting.HasTarget();
			return (hasTarget && Vector3D.Distance(RemoteControl.GetPosition(), _behavior.AutoPilot.Targeting.TargetLastKnownCoords) < trigger.TargetDistance) || (!hasTarget && trigger.AllowTargetFarWithoutTarget);

		}

		//TargetFar
		public bool CheckTargetFar(TriggerProfile trigger) {

			return _behavior.AutoPilot.Targeting.HasTarget() && Vector3D.Distance(RemoteControl.GetPosition(), _behavior.AutoPilot.Targeting.TargetLastKnownCoords) > trigger.TargetDistance;

		}

		//DespawnNear
		public bool CheckDespawnNear(TriggerProfile trigger) {

			return _behavior.BehaviorSettings.DespawnCoords != Vector3D.Zero && Vector3D.Distance(RemoteControl.GetPosition(), _behavior.BehaviorSettings.DespawnCoords) < trigger.TargetDistance;

		}

		//DespawnFar
		public bool CheckDespawnFar(TriggerProfile trigger) {

			return _behavior.BehaviorSettings.DespawnCoords != Vector3D.Zero && Vector3D.Distance(RemoteControl.GetPosition(), _behavior.BehaviorSettings.DespawnCoords) > trigger.TargetDistance;

		}

		//TurretTarget
		public bool CheckTurretTarget(TriggerProfile trigger) {

			var turretTarget = _autopilot.Weapons.GetTurretTarget();

			if (turretTarget != 0) {

				trigger.DetectedEntityId = turretTarget;
				return true;

			}

			return false;

		}

		//NoWeapon
		public bool CheckNoWeapon(TriggerProfile trigger) {

			return !_autopilot.Weapons.HasWorkingWeapons();

		}

		//NoTarget
		public bool CheckNoTarget(TriggerProfile trigger) {

			return !_autopilot.Targeting.HasTarget();

		}

		//HasTarget
		public bool CheckHasTarget(TriggerProfile trigger) {

			return _autopilot.Targeting.HasTarget();

		}

		//AcquiredTarget
		public bool CheckAcquiredTarget(TriggerProfile trigger) {

			var result = _autopilot.Targeting.TargetAcquired;
			_autopilot.Targeting.TargetAcquired = false;
			return result;

		}

		//LostTarget
		public bool CheckLostTarget(TriggerProfile trigger) {

			var result = _autopilot.Targeting.TargetLost;
			_autopilot.Targeting.TargetLost = false;
			return result;

		}

		//SwitchedTarget
		public bool CheckSwitchedTarget(TriggerProfile trigger) {

			var result = _autopilot.Targeting.TargetSwitched;
			_autopilot.Targeting.TargetSwitched = false;
			return result;

		}

		//ChangedTarget
		public bool CheckChangedTarget(TriggerProfile trigger) {

			var result = _autopilot.Targeting.TargetChanged;
			_autopilot.Targeting.TargetChanged = false;
			return result;

		}

		//TargetInSafezone
		public bool CheckTargetInSafezone(TriggerProfile trigger) {

			return _autopilot.Targeting.HasTarget() && _autopilot.Targeting.Target.InSafeZone();

		}

		//BehaviorTriggerA
		public bool CheckBehaviorTriggerA(TriggerProfile trigger) {

			return _behavior.BehaviorTriggerA;

		}

		//BehaviorTriggerB
		public bool CheckBehaviorTriggerB(TriggerProfile trigger) {

			return _behavior.BehaviorTriggerB;

		}

		//BehaviorTriggerC
		public bool CheckBehaviorTriggerC(TriggerProfile trigger) {

			return _behavior.BehaviorTriggerC;

		}

		//BehaviorTriggerD
		public bool CheckBehaviorTriggerD(TriggerProfile trigger) {

			return _behavior.BehaviorTriggerD;

		}

		//BehaviorTriggerE
		public bool CheckBehaviorTriggerE(TriggerProfile trigger) {

			return _behavior.BehaviorTriggerE;

		}

		//BehaviorTriggerF
		public bool CheckBehaviorTriggerF(TriggerProfile trigger) {

			return _behavior.BehaviorTriggerF;

		}

		//BehaviorTriggerG
		public bool CheckBehaviorTriggerG(TriggerProfile trigger) {

			return _behavior.BehaviorTriggerG;

		}

		//PaymentSuccess
		public bool CheckPaymentSuccess(TriggerProfile trigger) {

			return PaymentSuccessTriggered;

		}

		//PaymentFailure
		public bool CheckPaymentFailure(TriggerProfile trigger) {

			return PaymentFailureTriggered;

		}

		//PlayerKnownLocation
		public bool CheckPlayerKnownLocation(TriggerProfile trigger) {

			return KnownPlayerLocationManager.IsPositionInKnownPlayerLocation(RemoteControl.GetPosition(), true, _behavior.Owner.Faction?.Tag);

		}

		//SensorActive
		public bool CheckSensorActive(TriggerProfile trigger) {

			return _behavior.Grid.SensorCheck(trigger.SensorName);

		}

		//SensorIdle
		public bool CheckSensorIdle(TriggerProfile trigger) {

			return _behavior.Grid.SensorCheck(trigger.SensorName, false);

		}

		//Weather
		public bool CheckWeather(TriggerProfile trigger) {

			var weather = MyAPIGateway.Session.WeatherEffects.GetWeather(RemoteControl.GetPosition());
			return !string.IsNullOrWhiteSpace(weather) && trigger.WeatherTypes.Contains(weather);

		}

		//JumpRequested
		public bool JumpRequested(TriggerProfile trigger) {

			var dist = Vector3D.Distance(trigger.JumpStart, _behavior.RemoteControl.GetPosition());
			return dist < trigger.TargetDistance;

		}

		//JumpCompleted
		public bool JumpCompleted(TriggerProfile trigger) {

			var dist = Vector3D.Distance(trigger.JumpStart, _behavior.RemoteControl.GetPosition());
			return dist < trigger.TargetDistance;

		}

		//InsideZone
		public bool InsideZone(TriggerProfile trigger) {

			return ZoneManager.InsideZoneWithName(RemoteControl.GetPosition(), trigger.ZoneName);

		}

		//OutsideZone
		public bool OutsideZone(TriggerProfile trigger) {

			return !ZoneManager.InsideZoneWithName(RemoteControl.GetPosition(), trigger.ZoneName);

		}

		//CheckSession
		public bool CheckSession(TriggerProfile trigger) {

			return !trigger.SessionTriggerActivated;

		}

		//CheckActiveWeaponsPercentage
		public bool CheckActiveWeaponsPercentage(TriggerProfile trigger) {

			float count = _behavior.AutoPilot.Weapons.GetActiveWeaponCount();

			if (count == 0)
				return false;

			return ((count / _behavior.BehaviorSettings.InitialWeaponCount) * 100) >= trigger.PercentageOfWeaponsRemaining;

		}

		//CheckActiveTurretsPercentage
		public bool CheckActiveTurretsPercentage(TriggerProfile trigger) {

			float count = _behavior.AutoPilot.Weapons.GetActiveTurretCount();

			if (count == 0)
				return false;

			return ((count / _behavior.BehaviorSettings.InitialTurretCount) * 100) >= trigger.PercentageOfWeaponsRemaining;

		}

		//CheckActiveGunsPercentage
		public bool CheckActiveGunsPercentage(TriggerProfile trigger) {

			float count = _behavior.AutoPilot.Weapons.GetActiveGunCount();

			if (count == 0)
				return false;

			return ((count / _behavior.BehaviorSettings.InitialGunCount) * 100) >= trigger.PercentageOfWeaponsRemaining;

		}

		//CheckHealthPercentage
		public bool CheckHealthPercentage(TriggerProfile trigger) {

			if (_behavior.CurrentGrid == null || !_behavior.CurrentGrid.ActiveEntity())
				return false;

			var health = _behavior.CurrentGrid.GetCurrentHealth();

			if (health == 0)
				return false;

			return ((health / _behavior.BehaviorSettings.InitialGridIntegrity) * 100) >= trigger.PercentageOfHealthRemaining;

		}

	}

}
