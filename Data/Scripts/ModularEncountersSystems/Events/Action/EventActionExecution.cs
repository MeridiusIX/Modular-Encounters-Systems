using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Events.Condition;
using ModularEncountersSystems.Files;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Zones;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Events.Action {
	public partial class EventActionProfile {

		public void ExecuteAction() {

			var actions = ActionReference;
			var EventBroadcastSystem = new EventBroadcastSystem();



			if (actions.Chance < 100)
			{

				var roll = MathTools.RandomBetween(0, 101);

				if (roll > actions.Chance)
				{

					BehaviorLogger.Write(actions.ProfileSubtypeId + ": Did Not Pass Chance Check", BehaviorDebugEnum.Action);
					return;

				}


			}






			//DebugHudMessage
			if (!string.IsNullOrWhiteSpace(actions.DebugHudMessage))
				MyVisualScriptLogicProvider.ShowNotificationToAll(actions.DebugHudMessage, 3000);

			//Booleans
			if (actions.ChangeBooleans == true) {

				for (int i = 0; i < actions.SetBooleansTrue.Count; i++) {

					MyAPIGateway.Utilities.SetVariable<bool>(actions.SetBooleansTrue[i], true);

				}
				for (int i = 0; i < actions.SetBooleansFalse.Count; i++) {

					MyAPIGateway.Utilities.SetVariable<bool>(actions.SetBooleansFalse[i], false);

				}

			}

			//Change Counter
			if (actions.ChangeCounters) {

				bool fail = false;
				if (actions.IncreaseCounters.Count != actions.IncreaseCountersAmount.Count)
					fail = true;

				if (actions.DecreaseCounters.Count != actions.DecreaseCountersAmount.Count)
					fail = true;

				if (actions.SetCounters.Count != actions.SetCountersAmount.Count)
				{
					fail = true;
				}

                if (!fail)
                {
					for (int i = 0; i < actions.SetCounters.Count; i++)
					{
						SetCounter(actions.SetCounters[i], actions.SetCountersAmount[i], true);
					}


					for (int i = 0; i < actions.IncreaseCounters.Count; i++)
					{
						SetCounter(actions.IncreaseCounters[i], Math.Abs(actions.IncreaseCountersAmount[i]), false);
					}

					for (int i = 0; i < actions.DecreaseCounters.Count; i++)
					{

						SetCounter(actions.DecreaseCounters[i], -Math.Abs(actions.DecreaseCountersAmount[i]), false);
					}
				}


			}



			if (actions.ResetCooldownTimeOfEvents)
			{
				ResetCooldownTimeOfEvents(actions.ResetEventCooldownIds, actions.ResetEventCooldownTags);
			}

            if (actions.IncreaseRunCountOfEvents)
            {
				IncreaseRunCountEvents(actions.IncreaseRunCountEventIds, actions.IncreaseRunCountEventIdAmount, actions.IncreaseRunCountEventTags, actions.IncreaseRunCountEventTagAmount);
            }
			if (actions.ToggleEvents)
			{
				ToggleEvents(actions.ToggleEventIds, actions.ToggleEventIdModes, actions.ToggleEventTags, actions.ToggleEventTagModes);
			}

            if (actions.AddTagstoPlayers)
            {
				foreach(var player in PlayerManager.Players)
				{

                    if (PlayerCondition.ArePlayerConditionsMet(actions.AddTagsPlayerConditionIds, player.Player.IdentityId))
                    {
						foreach (var tag in actions.AddTags)
						{
							if (player.ProgressionData.Tags.Contains(tag))
								continue;
							player.ProgressionData.Tags.Add(tag);
						}
                    }
				}
            }

			if (actions.RemoveTagsFromPlayers)
			{
				foreach (var player in PlayerManager.Players)
				{

					if (PlayerCondition.ArePlayerConditionsMet(actions.RemoveTagsPlayerConditionIds, player.Player.IdentityId))
					{
						foreach (var tag in actions.RemoveTags)
						{
							if (!player.ProgressionData.Tags.Contains(tag))
								continue;
							player.ProgressionData.Tags.Remove(tag);
						}
					}
				}
			}

			//BroadcastCommandProfiles
			if (actions.BroadcastCommandProfiles)
			{

				foreach (var commandId in actions.CommandProfileIds)
				{

					CommandProfile commandProfile = null;

					if (!ProfileManager.CommandProfiles.TryGetValue(commandId, out commandProfile))
					{

						BehaviorLogger.Write(commandId + ": Command Profile Not Found", BehaviorDebugEnum.Action);
						continue;

					}

					var newCommand = new Command();
					newCommand.PrepareEventCommand(commandProfile, actions.CommandProfileOriginCoords);
					BehaviorLogger.Write(actions.ProfileSubtypeId + ": Sending Command: " + newCommand.CommandCode, BehaviorDebugEnum.Action);

					CommandHelper.SendCommand(newCommand);

				}

			}



			if (actions.TeleportPlayers)
			{
				foreach (var player in PlayerManager.Players)
				{
					if (PlayerCondition.ArePlayerConditionsMet(actions.TeleportPlayerConditionIds, player.Player.IdentityId))
					{
						player.Player.Character.Teleport(MatrixD.CreateWorld(actions.TeleportPlayerCoords));

					}
					
				}
			}

            if (actions.FadeInPlayers)
            {
				foreach (var player in PlayerManager.Players)
				{
					if (PlayerCondition.ArePlayerConditionsMet(actions.FadeInPlayerConditionIds, player.Player.IdentityId))
					{
						MyVisualScriptLogicProvider.ScreenColorFadingStart(2, true, player.Player.IdentityId);
						MyVisualScriptLogicProvider.ScreenColorFadingMinimalizeHUD(true, player.Player.IdentityId);
					}
				}
			}



            if (actions.FadeOutPlayers)
            {
				foreach (var player in PlayerManager.Players)
				{
					if (PlayerCondition.ArePlayerConditionsMet(actions.FadeOutPlayerConditionIds, player.Player.IdentityId))
					{
						MyVisualScriptLogicProvider.ScreenColorFadingStartSwitch(8, player.Player.IdentityId);
					}
				}
			}

			if (actions.AddItemToPlayersInventory)
			{
				foreach (var player in PlayerManager.Players)
				{
					if (PlayerCondition.ArePlayerConditionsMet(actions.AddItemPlayerConditionIds, player.Player.IdentityId))
					{
                        foreach (var raw_id in actions.ItemIds)
                        {
							string itemType = raw_id.Split('/')[0];
							StoreProfileItemTypes storeItemType = (StoreProfileItemTypes)Enum.Parse(typeof(StoreProfileItemTypes), itemType);

							Type type = null;

							if (storeItemType == StoreProfileItemTypes.None)
								type = typeof(MyObjectBuilder_DefinitionBase);

							if (storeItemType == StoreProfileItemTypes.Ammo)
								type = typeof(MyObjectBuilder_AmmoMagazine);

							if (storeItemType == StoreProfileItemTypes.Ore)
								type = typeof(MyObjectBuilder_Ore);

							if (storeItemType == StoreProfileItemTypes.Ingot)
								type = typeof(MyObjectBuilder_Ingot);

							if (storeItemType == StoreProfileItemTypes.Component)
								type = typeof(MyObjectBuilder_Component);

							if (storeItemType == StoreProfileItemTypes.Tool)
								type = typeof(MyObjectBuilder_PhysicalGunObject);


							if (storeItemType == StoreProfileItemTypes.Consumable)
								type = typeof(MyObjectBuilder_ConsumableItem);

							var item = MyDefinitionManager.Static.GetPhysicalItemDefinition(new MyDefinitionId(type, raw_id.Split('/')[1])).Id;

							MyVisualScriptLogicProvider.AddToPlayersInventory(player.Player.IdentityId, item);
						}
					}
				}
			}


			if (actions.ChangeReputationWithPlayers)
            {
				List<long> players = new List<long>();


				foreach (var player in PlayerManager.Players)
				{
					if (PlayerCondition.ArePlayerConditionsMet(actions.ReputationPlayerConditionIds, player.Player.IdentityId))
					{
						players.Add(player.Player.IdentityId);
					}
				}


				FactionHelper.ChangePlayerReputationWithFactions(null,actions.ReputationChangeAmount, players, actions.ReputationChangeFactions, actions.ReputationChangesForAllRadiusPlayerFactionMembers, actions.ReputationMinCap, actions.ReputationMaxCap);
			}

			if (actions.AddGPSToPlayers)
			{

				foreach (var player in PlayerManager.Players)
				{
					if (PlayerCondition.ArePlayerConditionsMet(actions.AddGPSPlayerConditionIds, player.Player.IdentityId))
					{
						for (int i = 0; i < actions.GPSNames.Count; i++)
						{
							var Color = new Color((Vector3)actions.GPSColors[i]);

							if (actions.UseGPSObjective)
							{
								MyVisualScriptLogicProvider.AddGPSObjective(actions.GPSNames[i], actions.GPSDescriptions[i], actions.GPSVector3Ds[i], Color, 0, player.Player.IdentityId);
							}
							else
							{
								MyVisualScriptLogicProvider.AddGPS(actions.GPSNames[i], actions.GPSDescriptions[i], actions.GPSVector3Ds[i], Color, 0, player.Player.IdentityId);
							}
						}
					}
				}
			}




			if (actions.RemoveGPSFromPlayers)
			{
				foreach (var player in PlayerManager.Players)
				{
					if (PlayerCondition.ArePlayerConditionsMet(actions.AddGPSPlayerConditionIds, player.Player.IdentityId))
					{
						for (int i = 0; i < actions.GPSNames.Count; i++)
						{
							MyVisualScriptLogicProvider.RemoveGPS(actions.GPSNames[i], player.Player.IdentityId);
						}
					}
				}

			}



			//ChatBroadcast
			if (actions.UseChatBroadcast == true) {

				foreach (var chatData in ChatData) {
					EventBroadcastSystem.BroadcastRequest(chatData);

				}

			}



			if (actions.SpawnEncounter)
			{
				if(Spawner.Count == actions.SpawnFactionTags.Count && Spawner.Count == actions.SpawnVector3Ds.Count)
				{
					for (int i = 0; i < Spawner.Count; i++)
					{
						var spawner = Spawner[i];

						if (spawner.UseSpawn)
						{
							BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Spawn", BehaviorDebugEnum.Spawn);
							if (spawner.IsReadyToSpawn())
							{
								var coords = actions.SpawnVector3Ds[i];
								var planet = PlanetManager.GetNearestPlanet(coords);

								MatrixD WorldMatrix = MatrixD.Zero;

								if (planet != null && PlanetManager.InGravity(coords))
								{
									var up = planet.UpAtPosition(coords);
									var forward = Vector3D.CalculatePerpendicularVector(up);
									WorldMatrix = MatrixD.CreateWorld(coords, forward, up);
								}
								else
									WorldMatrix = MatrixD.CreateWorld(coords);

								spawner.AssignInitialMatrix(WorldMatrix);
								spawner.CurrentFactionTag = actions.SpawnFactionTags[i];
								BehaviorSpawnHelper.BehaviorSpawnRequest(spawner);
							}

						}

					}
				}

			}




			if (actions.ChangeZoneAtPosition)
			{

				if (actions.ZoneNames.Count != actions.ZoneCoords.Count)
					return;


				if (actions.ZoneNames.Count != actions.ZoneToggleActiveModes.Count)
					return;


				for (int i = 0; i < actions.ZoneNames.Count; i++)
				{
					ZoneManager.ToggleZonesAtPosition(actions.ZoneCoords[i], actions.ZoneNames[i], actions.ZoneToggleActiveModes[i]);
				}


			}

			if (actions.ActivateCustomAction)
			{
				System.Action<object[]> action;
				List<object> args = new List<object>();


				//Yes I tried args.Add(), and args.AddRange() both didn't work

				foreach (var item in actions.CustomActionArgumentsString)
				{
					args.Add((object)item);
				}

				foreach (var item in actions.CustomActionArgumentsBool)
				{
					args.Add((object)item);
				}

				foreach (var item in actions.CustomActionArgumentsInt)
				{
					args.Add((object)item);
				}

				foreach (var item in actions.CustomActionArgumentsFloat)
				{
					args.Add((object)item);
				}

				foreach (var item in actions.CustomActionArgumentsLong)
				{
					args.Add((object)item);
				}

				foreach (var item in actions.CustomActionArgumentsDouble)
				{
					args.Add((object)item);
				}

				foreach (var item in actions.CustomActionArgumentsVector3D)
				{
					args.Add((object)item);
				}

				if (LocalApi.CustomActions.TryGetValue(actions.CustomActionName, out action))
				{
					action?.Invoke(args.ToArray());
				}


			}


				/*
				//SetEventControllers
				if (actions.SetEventControllers)
					EventControllerSettings(actions.EventControllerNames, actions.EventControllersActive, actions.EventControllersSetCurrentTime);
				*/

			}

		/*
		private void EventControllerSettings(List<string> names, List<bool> active, List<bool> setCurrentTime) {

			for (int i = 0; i < names.Count && i < active.Count && i < setCurrentTime.Count; i++) {

				bool found = false;

				foreach (var controller in EventManager.EventControllersList) {

					if (controller.ProfileSubtypeId != names[i])
						continue;

					found = true;

					controller.Active = active[i];

					if (setCurrentTime[i])
						controller.StartDate = Helpers.Time.GetRealIngameTime();

					break;

				}

				if (found)
					continue;

				var newcontroller = EventController.CreateController(names[i]);
				newcontroller.Active = active[i];
				newcontroller.StartDate = MyAPIGateway.Session.GameDateTime;
				EventManager.EventControllersList.Add(newcontroller);

			}
		
		}
		*/

	

		public void SetCounter(string counterName, int amount, bool hardSet = false)
		{

			if (hardSet)
			{

				MyAPIGateway.Utilities.SetVariable<int>(counterName, amount);
				return;

			}

			int existingCounter = 0;

			MyAPIGateway.Utilities.GetVariable(counterName, out existingCounter);

			//This is for ResetCounters
			if (amount == 0)
			{

				MyAPIGateway.Utilities.SetVariable<int>(counterName, 0);
				return;

			}

			else
			{
				existingCounter += amount;
				MyAPIGateway.Utilities.SetVariable<int>(counterName, existingCounter);
				return;

			}

		}


		public static void ResetCooldownTimeOfEvents(List<string> ResetEventCooldownNames, List<string> ResetEventCooldownTags)
		{
			foreach (var EventName in ResetEventCooldownNames)
			{
				var Name = EventName;
				foreach (var Event in EventManager.EventsList)
				{
					if (Name == Event.ProfileSubtypeId)
					{
						Event.LastTriggerTime = MyAPIGateway.Session.GameDateTime;
						Event.CooldownTimeTrigger = MathTools.RandomBetween(Event.Profile.MinCooldownMs, Event.Profile.MaxCooldownMs);
					}
				}
			}

			foreach (var Tag in ResetEventCooldownTags)
			{
				foreach (var Event in EventManager.EventsList)
				{
					if (Event.Profile.Tags.Contains(Tag))
					{
						Event.LastTriggerTime = MyAPIGateway.Session.GameDateTime;
						Event.CooldownTimeTrigger = MathTools.RandomBetween(Event.Profile.MinCooldownMs, Event.Profile.MaxCooldownMs);
					}
				}
			}

		}

		public static void IncreaseRunCountEvents(List<string> IncreaseRunCountEventIds, List<int> IncreaseRunCountEventIdAmount, List<string> IncreaseRunCountEventTags, List<int> IncreaseRunCountEventTagAmount)
		{
			for (int i = 0; i < IncreaseRunCountEventIds.Count; i++)
			{
				var Name = IncreaseRunCountEventIds[i];

				foreach (var Event in EventManager.EventsList)
				{
					if (Name == Event.ProfileSubtypeId)
					{
						Event.RunCount += IncreaseRunCountEventIdAmount[i];
					}
				}
			}

			for (int i = 0; i < IncreaseRunCountEventTags.Count; i++)
			{
				var Tag = IncreaseRunCountEventTags[i];
				foreach (var Event in EventManager.EventsList)
				{
					if (Event.Profile.Tags.Contains(Tag))
					{
						Event.RunCount += IncreaseRunCountEventTagAmount[i];
					}
				}
			}

		}

		public static void ToggleEvents(List<string> ToggleEventIds, List<bool> ToggleEventIdModes, List<string> ToggleEventTags, List<bool> ToggleEventTagModes)
		{
			for (int i = 0; i < ToggleEventIds.Count; i++)
			{
				var Name = ToggleEventIds[i];

				foreach (var Event in EventManager.EventsList)
				{
					if (Name == Event.ProfileSubtypeId)
					{
						Event.EventEnabled = ToggleEventIdModes[i];
					}
				}
			}

			for (int i = 0; i < ToggleEventTags.Count; i++)
			{
				var Tag = ToggleEventTags[i];
				foreach (var Event in EventManager.EventsList)
				{
					if (Event.Profile.Tags.Contains(Tag))
					{
						Event.EventEnabled = ToggleEventTagModes[i];
					}
				}
			}

		}











	}

}
