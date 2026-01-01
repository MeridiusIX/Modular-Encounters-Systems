using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.BlockLogic;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Events.Action;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Missions;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Spawning.Manipulation;
using ModularEncountersSystems.Spawning.Profiles;
using ModularEncountersSystems.Sync;
using ModularEncountersSystems.Tasks;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using ModularEncountersSystems.Zones;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger
{
    public partial class TriggerSystem
    {
        public void ProcessAction(TriggerProfile trigger, ActionProfile actionsBase, long attackerEntityId = 0, long detectedEntity = 0, Command command = null)
        {

            BehaviorLogger.Write(trigger.ProfileSubtypeId + " Attempting To Execute Action Profile " + actionsBase.ProfileSubtypeId, BehaviorDebugEnum.Action);

            var actions = actionsBase.ActionReference;



            if (actions == null)
            {

                BehaviorLogger.Write(actionsBase.ProfileSubtypeId + " Has No Associated Action Reference Profile. Aborting." + actionsBase.ProfileSubtypeId, BehaviorDebugEnum.Action);
                return;

            }

            if (!string.IsNullOrWhiteSpace(actions.ParentGridNameRequirement) && !string.IsNullOrWhiteSpace(_behavior?.RemoteControl?.SlimBlock?.CubeGrid?.CustomName))
            {

                if (_behavior.RemoteControl.SlimBlock.CubeGrid.CustomName != actions.ParentGridNameRequirement)
                    return;

            }

            if (actions.Chance < 100)
            {

                var roll = MathTools.RandomBetween(0, 101);

                if (roll > actions.Chance)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Did Not Pass Chance Check", BehaviorDebugEnum.Action);
                    return;

                }


            }

            LocalApi.BehaviorTriggerWatcher?.Invoke(RemoteControl, trigger.ProfileSubtypeId, actions.ProfileSubtypeId, _behavior.AutoPilot.Targeting.Target?.GetEntity(), _behavior.AutoPilot.GetCurrentWaypoint());

            BehaviorLogger.Write($"{actions.ProfileSubtypeId}: Performing Eligible Actions: AEI[{attackerEntityId}], DE[{detectedEntity}]", BehaviorDebugEnum.Action);

            var lastAction = "";
            try
            {
                //Debug Message
                lastAction = "DebugMessage";
                if (!string.IsNullOrWhiteSpace(actions.DebugMessage))
                {

                    var message = IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, actions.DebugMessage);

                    foreach (var customVar in _behavior?.CurrentGrid?.Npc?.CustomCountersVariables)
                    {
                        if (message.Contains("{" + customVar.Key + "}"))
                        {
                            message = message.Replace("{" + customVar.Key + "}", customVar.Value.ToString());
                        }
                    }

                    MyVisualScriptLogicProvider.SendChatMessage(message, RemoteControl.SlimBlock.CubeGrid.CustomName);


                    //MyVisualScriptLogicProvider.ShowNotificationToAll(actions.DebugMessage, 4000);

                }

                //ChatBroadcast
                lastAction = "ChatBroadcast";
                if (actions.UseChatBroadcast == true)
                {

                    foreach (var chatData in actionsBase.ChatData)
                    {

                        BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Chat Broadcast", BehaviorDebugEnum.Action);
                        _broadcast.BroadcastRequest(chatData, command);

                    }

                }

                //Playsound cue
                lastAction = "PlayDialogueCue";
                if (actions.PlayDialogueCue)
                {
                    bool foundDialogueCueId = false;

                    ChatProfile chat = null;


                    List<long> tempPlayerIdList = new List<long>();

                    if (actions.PlayDialogueToSpecificPlayers && actions.PlayDialoguePlayerConditionIds.Count > 0 && _behavior.RemoteControl != null)
                    {
                        foreach (var player in PlayerManager.Players)
                        {
                            var PlayDialoguePlayerConditionIds = IdsReplacer.ReplaceIds(_behavior?.CurrentGrid?.Npc ?? null, actions.PlayDialoguePlayerConditionIds);

                            if (PlayerCondition.ArePlayerConditionsMet(PlayDialoguePlayerConditionIds, player.Player.IdentityId, actions.PlayDialogueOverridePositionInPlayerCondition, _behavior.RemoteControl.GetPosition()))
                            {
                                tempPlayerIdList.Add(player.Player.IdentityId);
                            }
                        }
                    }






                    if (_dialogueBank.GetChatProfile(actions.DialogueCueId, ref chat, actions.PlayDialogueToSpecificPlayers))
                    {
                        BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Chat Broadcast", BehaviorDebugEnum.Action);
                        _broadcast.BroadcastRequest(chat, command, tempPlayerIdList);

                    }

                }

                lastAction = "ProcessStaticEncountersAtLocation";
                if (actions.ProcessStaticEncountersAtLocation)
                {
                    bool updateStatics = false;

                    //Static Encounters
                    for (int i = NpcManager.StaticEncounters.Count - 1; i >= 0; i--)
                    {

                        var encounter = NpcManager.StaticEncounters[i];
                        encounter.ProcessEncounter(ref updateStatics, true, actions.ProcessStaticEncountersLocation);

                    }

                    if (updateStatics)
                        NpcManager.UpdateStaticEncounters();
                }


                //PlaySoundAtPosition
                lastAction = "PlaySoundAtPosition";
                if (actions.PlaySoundAtPosition && !string.IsNullOrWhiteSpace(actions.SoundAtPosition))
                {

                    MyVisualScriptLogicProvider.PlaySingleSoundAtPosition(actions.SoundAtPosition, RemoteControl.GetPosition());

                }

                //BarrellRoll
                lastAction = "BarrellRoll";
                if (actions.BarrelRoll == true)
                {

                    _behavior.AutoPilot.ActivateBarrelRoll();

                }

                //HeavyYaw
                lastAction = "HeavyYaw";
                if (actions.HeavyYaw == true)
                {

                    _behavior.AutoPilot.ActivateHeavyYaw();

                }

                //Ramming
                lastAction = "Ramming";
                if (actions.Ramming == true)
                {

                    _behavior.AutoPilot.ActivateRamming();

                }

                //Strafe - Implement Post Release
                lastAction = "Strafe";
                if (actions.Strafe == true)
                {

                    //_autopilot.ChangeAutoPilotMode(AutoPilotMode.Strafe);

                }

                //ChangeAutopilotSpeed
                lastAction = "ChangeAutopilotSpeed";
                if (actions.ChangeAutopilotSpeed == true)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Changing AutoPilot Speed To: " + actions.NewAutopilotSpeed.ToString(), BehaviorDebugEnum.Action);
                    _autopilot.State.MaxSpeedOverride = actions.NewAutopilotSpeed;
                    var blockList = BlockCollectionHelper.GetGridControllers(RemoteControl.SlimBlock.CubeGrid);

                    foreach (var block in blockList)
                    {

                        var tBlock = block as IMyRemoteControl;

                        if (tBlock != null)
                        {

                            tBlock.SpeedLimit = actions.NewAutopilotSpeed >= 0 ? actions.NewAutopilotSpeed : 100;

                        }

                    }

                }

                //ChangeAutopilotMinAltitude
                lastAction = "ChangeAutopilotMinAltitude";
                if (actions.ChangeAutopilotMinAltitude == true)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Changing AutoPilot MinAltitude To: " + actions.NewAutopilotMinAltitude.ToString(), BehaviorDebugEnum.Action);
                    _autopilot.State.MinAltitudeOverride = actions.NewAutopilotMinAltitude;
                    var blockList = BlockCollectionHelper.GetGridControllers(RemoteControl.SlimBlock.CubeGrid);

                    foreach (var block in blockList)
                    {

                        var tBlock = block as IMyRemoteControl;

                        if (tBlock != null)
                        {

                            tBlock.SpeedLimit = actions.NewAutopilotMinAltitude >= 0 ? actions.NewAutopilotMinAltitude : 100;

                        }

                    }

                }

                //ChangeFlyLevelWithGravity
                lastAction = "ChangeFlyLevelWithGravity";
                if (actions.ChangeFlyLevelWithGravity)
                {
                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Changing FlyLevelWithGravity To: " + actions.NewFlyLevelWithGravity.ToString(), BehaviorDebugEnum.Action);
                    _autopilot.State.UseFlyLevelWithGravity = actions.NewFlyLevelWithGravity;
                }

                //SpawnReinforcements
                lastAction = "SpawnReinforcements";
                if (actions.SpawnEncounter == true)
                {

                    foreach (var spawner in actionsBase.Spawner)
                    {

                        if (spawner.UseSpawn)
                        {

                            if (!string.IsNullOrWhiteSpace(spawner.ParentGridNameRequirement) && !string.IsNullOrWhiteSpace(_behavior?.RemoteControl?.SlimBlock?.CubeGrid?.CustomName))
                            {

                                if (_behavior.RemoteControl.SlimBlock.CubeGrid.CustomName != spawner.ParentGridNameRequirement)
                                    continue;

                            }

                            BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Spawn", BehaviorDebugEnum.Spawn);
                            if (spawner.IsReadyToSpawn())
                            {

                                //BehaviorLogger.AddMsg("Do Spawn", true);
                                spawner.AssignInitialMatrix(RemoteControl.WorldMatrix);
                                spawner.CurrentFactionTag = spawner.ForceSameFactionOwnership && !string.IsNullOrWhiteSpace(_owner.Faction?.Tag) ? _owner.Faction.Tag : "";

                                spawner.SpawnGroups = IdsReplacer.ReplaceIds(_behavior?.CurrentGrid?.Npc ?? null, spawner.SpawnGroups);

                                spawner.ParentId = _behavior.RemoteControl.OwnerId;
                                spawner.ParentBehavior = _behavior;

                                BehaviorSpawnHelper.BehaviorSpawnRequest(spawner);

                            }

                        }

                    }

                }

                //SelfDestruct
                lastAction = "SelfDestruct";
                if (actions.SelfDestruct == true)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting SelfDestruct", BehaviorDebugEnum.Action);
                    var blockList = BlockCollectionHelper.GetBlocksOfType<IMyWarhead>(RemoteControl.SlimBlock.CubeGrid);
                    int totalWarheads = 0;

                    foreach (var tblock in blockList)
                    {

                        var block = tblock as IMyWarhead;

                        if (block != null)
                        {

                            if (!actions.StaggerWarheadDetonation)
                            {

                                block.IsArmed = true;
                                block.DetonationTime = 0 + actions.SelfDestructTimerPadding;
                                block.Detonate();
                                totalWarheads++;

                            }
                            else
                            {

                                totalWarheads++;
                                block.IsArmed = true;
                                block.DetonationTime = (totalWarheads * actions.SelfDestructTimeBetweenBlasts) + actions.SelfDestructTimerPadding;
                                block.StartCountdown();

                            }

                        }

                    }

                    //BehaviorLogger.AddMsg("TotalBlocks:  " + blockList.Count.ToString(), true);
                    //BehaviorLogger.AddMsg("TotalWarheads: " + totalWarheads.ToString(), true);

                    //TODO: Shield EMP

                }

                //Retreat
                lastAction = "Retreat";
                if (actions.Retreat)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Retreat", BehaviorDebugEnum.Action);
                    _despawn.Retreat();

                }

                //RecalculateDespawnCoords
                lastAction = "RecalculateDespawnCoords";
                if (actions.RecalculateDespawnCoords && this.RemoteControl != null)
                {

                    _behavior.AutoPilot.State.CargoShipDespawn = new EncounterWaypoint(_behavior.AutoPilot.CalculateDespawnCoords(this.RemoteControl.GetPosition()));

                }

                //ForceDespawn
                lastAction = "ForceDespawn";
                if (actions.ForceDespawn)
                {


                    if (actions.TryToDespawnThisGridOnly)
                        _despawn.DespawnThisGrid();
                    else
                        _despawn.DespawnGrid();

                }

                //TerminateBehavior
                lastAction = "TerminateBehavior";
                if (actions.TerminateBehavior)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Termination Of Behavior", BehaviorDebugEnum.Action);
                    _autopilot.ActivateAutoPilot(Vector3D.Zero, NewAutoPilotMode.None);
                    _behavior.BehaviorTerminated = true;
                    if (_behavior.CurrentGrid?.Npc != null)
                        _behavior.CurrentGrid.Npc.BehaviorTerminationReason = "Behavior Terminated By Trigger/Action";

                }

                //BroadcastCommandProfiles
                lastAction = "BroadcastCommandProfiles";
                if (actions.BroadcastCommandProfiles)
                {

                    foreach (var commandId in actions.CommandProfileIds)
                    {

                        CommandProfile commandProfile = null;


                        var _commandId = IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, commandId);
                        if (!ProfileManager.CommandProfiles.TryGetValue(_commandId, out commandProfile))
                        {

                            BehaviorLogger.Write(commandId + ": Command Profile Not Found", BehaviorDebugEnum.Action);
                            continue;

                        }

                        var newCommand = new Command();
                        newCommand.PrepareCommand(_behavior, commandProfile, actions, command, attackerEntityId, detectedEntity);
                        BehaviorLogger.Write(actions.ProfileSubtypeId + ": Sending Command: " + newCommand.CommandCode, BehaviorDebugEnum.Action);
                        BehaviorLogger.Write(actions.ProfileSubtypeId + ": Sending Command: " + newCommand.CommandCode, BehaviorDebugEnum.Command);
                        CommandHelper.SendCommand(newCommand);

                    }

                }

                //BroadcastGenericCommand
                lastAction = "BroadcastGenericCommand";
                if (actions.BroadcastGenericCommand == true && RemoteControl != null)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Broadcast of Generic Command", BehaviorDebugEnum.Action);

                    double sendRadius = 0;

                    if (actions.SendCommandWithoutAntenna)
                    {

                        sendRadius = actions.SendCommandWithoutAntennaRadius;

                    }
                    else
                    {

                        var antenna = _behavior.Grid.GetAntennaWithHighestRange();

                        if (antenna != null)
                            sendRadius = antenna.Radius;

                    }

                    if (sendRadius != 0)
                    {

                        var newCommand = new Command();
                        newCommand.CommandCode = actions.BroadcastSendCode;
                        newCommand.RemoteControl = RemoteControl;
                        newCommand.Radius = sendRadius;
                        CommandHelper.SendCommand(newCommand);

                    }

                }

                //BroadcastDamagerTarget
                lastAction = "BroadcastDamagerTarget";
                if (actions.BroadcastDamagerTarget == true && detectedEntity != 0 && RemoteControl != null)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Broadcast of Damager", BehaviorDebugEnum.Action);

                    double sendRadius = 0;

                    if (actions.SendCommandWithoutAntenna)
                    {

                        sendRadius = actions.SendCommandWithoutAntennaRadius;

                    }
                    else
                    {

                        var antenna = _behavior.Grid.GetAntennaWithHighestRange();

                        if (antenna != null)
                            sendRadius = antenna.Radius;

                    }

                    if (sendRadius != 0)
                    {

                        var newCommand = new Command();
                        newCommand.CommandCode = actions.BroadcastSendCode;
                        newCommand.RemoteControl = RemoteControl;
                        newCommand.Radius = sendRadius;
                        newCommand.TargetEntityId = detectedEntity;
                        CommandHelper.SendCommand(newCommand);

                    }

                }

                //InheritLastAttackerFromCommand
                lastAction = "InheritLastAttackerFromCommand";
                if (actions.InheritLastAttackerFromCommand)
                {

                    _behavior.BehaviorSettings.LastDamagerEntity = command != null ? command.TargetEntityId : 0;

                }

                //SwitchToReceivedTarget
                lastAction = "SwitchToReceivedTarget";
                if (actions.SwitchToReceivedTarget == true && (command != null || detectedEntity != 0))
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Switch to Received Target Data", BehaviorDebugEnum.Action);
                    long switchToId = 0;

                    if (command != null && command.TargetEntityId != 0)
                    {

                        switchToId = command.TargetEntityId;


                    }
                    else if (detectedEntity != 0)
                    {

                        switchToId = detectedEntity;

                    }

                    IMyEntity tempEntity = null;

                    if (MyAPIGateway.Entities.TryGetEntityById(switchToId, out tempEntity))
                    {

                        var parentEnt = tempEntity.GetTopMostParent();

                        if (parentEnt != null)
                        {

                            if (parentEnt as IMyCubeGrid != null)
                            {

                                var gridGroup = MyAPIGateway.GridGroups.GetGroup(RemoteControl.SlimBlock.CubeGrid, GridLinkTypeEnum.Physical);
                                bool isSameGridConstrust = false;

                                foreach (var grid in gridGroup)
                                {

                                    if (grid.EntityId == tempEntity.GetTopMostParent().EntityId)
                                    {

                                        //BehaviorLogger.Write("Damager Parent Entity Was Same Grid", BehaviorDebugEnum.General);
                                        isSameGridConstrust = true;
                                        break;

                                    }

                                }

                                if (!isSameGridConstrust)
                                {

                                    //BehaviorLogger.Write("Damager Parent Entity Was External", BehaviorDebugEnum.General);
                                    _behavior.AutoPilot.Targeting.ForceTargetEntityId = parentEnt.EntityId;
                                    _behavior.AutoPilot.Targeting.ForceTargetEntity = parentEnt;
                                    _behavior.AutoPilot.Targeting.ForceRefresh = true;

                                }

                            }
                            else
                            {

                                var potentialPlayer = PlayerManager.GetPlayerUsingTool(tempEntity);

                                if (potentialPlayer != null)
                                {

                                    _behavior.AutoPilot.Targeting.ForceTargetEntityId = potentialPlayer.Player.Character.EntityId;
                                    _behavior.AutoPilot.Targeting.ForceTargetEntity = potentialPlayer.Player.Character;
                                    _behavior.AutoPilot.Targeting.ForceRefresh = true;

                                }

                            }


                        }

                    }

                }

                //SwitchTargetToDamager
                lastAction = "SwitchTargetToDamager";
                if (actions.SwitchTargetToDamager == true && _behavior.BehaviorSettings.LastDamagerEntity != 0)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Switch Target to Damager", BehaviorDebugEnum.Action);

                    IMyEntity tempEntity = null;

                    if (MyAPIGateway.Entities.TryGetEntityById(_behavior.BehaviorSettings.LastDamagerEntity, out tempEntity))
                    {

                        //BehaviorLogger.Write("Damager Entity Valid", BehaviorDebugEnum.General);

                        var parentEnt = tempEntity.GetTopMostParent();

                        if (parentEnt != null)
                        {

                            if (parentEnt as IMyCubeGrid != null)
                            {

                                var gridGroup = MyAPIGateway.GridGroups.GetGroup(RemoteControl.SlimBlock.CubeGrid, GridLinkTypeEnum.Physical);
                                bool isSameGridConstrust = false;

                                foreach (var grid in gridGroup)
                                {

                                    if (grid.EntityId == tempEntity.GetTopMostParent().EntityId)
                                    {

                                        //BehaviorLogger.Write("Damager Parent Entity Was Same Grid", BehaviorDebugEnum.General);
                                        isSameGridConstrust = true;
                                        break;

                                    }

                                }

                                if (!isSameGridConstrust)
                                {

                                    //BehaviorLogger.Write("Damager Parent Entity Was External", BehaviorDebugEnum.General);
                                    _behavior.AutoPilot.Targeting.ForceTargetEntityId = parentEnt.EntityId;
                                    _behavior.AutoPilot.Targeting.ForceTargetEntity = parentEnt;
                                    _behavior.AutoPilot.Targeting.ForceRefresh = true;

                                }

                            }
                            else
                            {

                                var potentialPlayer = PlayerManager.GetPlayerUsingTool(tempEntity);

                                if (potentialPlayer != null)
                                {

                                    _behavior.AutoPilot.Targeting.ForceTargetEntityId = potentialPlayer.Player.Character.EntityId;
                                    _behavior.AutoPilot.Targeting.ForceTargetEntity = potentialPlayer.Player.Character;
                                    _behavior.AutoPilot.Targeting.ForceRefresh = true;

                                }

                            }


                        }

                    }

                }

                //ClearWaypoints
                lastAction = "ClearWaypoints";
                if (actions.ClearAllWaypoints)
                {

                    foreach (var waypoint in _behavior.AutoPilot.State.CargoShipWaypoints)
                    {

                        waypoint.SetValid(false);

                    }

                }

                //AddWaypoints
                lastAction = "AddWaypoints";
                if (actions.AddWaypoints)
                {

                    foreach (var waypointName in actions.WaypointsToAdd)
                    {

                        var waypoint = EncounterWaypoint.CalculateWaypoint(_behavior, IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, waypointName));

                        if (waypoint != null && waypoint.Valid)
                        {

                            _behavior.AutoPilot.State.CargoShipWaypoints.Add(waypoint);

                        }

                    }

                }

                //AddWaypointFromCommand
                lastAction = "AddWaypointFromCommand";
                if (actions.AddWaypointFromCommand && command?.Waypoint != null)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Adding Received Waypoint From Command", BehaviorDebugEnum.Action);
                    //VRage.Utils.MyLog.Default.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>> " + command.Waypoint.GetGPS("target_waypoint"));
                    _behavior.AutoPilot.State.CargoShipWaypoints.Add(command.Waypoint);

                }

                //AddCustomCountersVariablesFromCommand
                lastAction = "AddCustomCountersVariablesFromCommand";
                if (actions.AddCustomCountersVariablesFromCommand && command?.CustomCountersVariables != null)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Adding Custom Counters Variables From Command", BehaviorDebugEnum.Action);
                    var npcdata = _behavior?.CurrentGrid?.Npc;
                    foreach (var customVars in command.CustomCountersVariables)
                    {
                        npcdata.CustomCountersVariables[customVars.Key] = customVars.Value;
                    }

                }

                //CancelWaitingAtWaypoint
                lastAction = "CancelWaitingAtWaypoint";
                if (actions.CancelWaitingAtWaypoint)
                {

                    _behavior.AutoPilot.State.WaypointWaitTime = DateTime.MinValue;

                }

                //SwitchToNextWaypoint
                lastAction = "SwitchToNextWaypoint";
                if (actions.SwitchToNextWaypoint && _behavior.AutoPilot.State.CargoShipWaypoints.Count > 0)
                {

                    for (int i = 0; i < _behavior.AutoPilot.State.CargoShipWaypoints.Count; i++)
                    {

                        _behavior.AutoPilot.State.CargoShipWaypoints[0].Valid = false;

                    }

                }

                //AssignEscortFromCommand
                lastAction = "AssignEscortFromCommand";
                if (actions.AssignEscortFromCommand && command != null && command.RequestEscortSlot)
                {

                    var result = _behavior.Escort.ProcessEscortRequest(command);
                    BehaviorLogger.Write(result, BehaviorDebugEnum.Action);

                }

                //SwitchToBehavior
                lastAction = "SwitchToBehavior";
                if (actions.SwitchToBehavior == true)
                {

                    _behavior.ChangeBehavior(actions.NewBehavior, actions.PreserveSettingsOnBehaviorSwitch, actions.PreserveTriggersOnBehaviorSwitch, actions.PreserveTargetDataOnBehaviorSwitch);

                }

                //ChangePlayerCredits
                lastAction = "ChangePlayerCredits";
                if (actions.ChangePlayerCredits && _behavior.RemoteControl != null)
                {


                    bool SavedPlayerIdentityAlreadyIncluded = false;
                    foreach (var player in PlayerManager.Players)
                    {
                        var ChancePlayerCreditsPlayerConditionIds = IdsReplacer.ReplaceIds(_behavior?.CurrentGrid?.Npc ?? null, actions.ChangePlayerCreditsPlayerConditionIds);

                        if (PlayerCondition.ArePlayerConditionsMet(ChancePlayerCreditsPlayerConditionIds, player.Player.IdentityId, actions.ChangePlayerCreditsOverridePositionInPlayerCondition, _behavior.RemoteControl.GetPosition()))
                        {
                            if ((command?.PlayerIdentity ?? 0) != 0 && (command?.PlayerIdentity ?? 0) == player.Player.IdentityId)
                                SavedPlayerIdentityAlreadyIncluded = true;

                            long credits = 0;
                            player.Player.TryGetBalanceInfo(out credits);

                            if (actions.ChangePlayerCreditsAmount > 0)
                            {
                                player.Player.RequestChangeBalance(actions.ChangePlayerCreditsAmount);
                                PaymentSuccessTriggered = true;

                            }
                            else
                            {

                                if (actions.ChangePlayerCreditsAmount > credits)
                                {

                                    PaymentFailureTriggered = true;

                                }
                                else
                                {

                                    player.Player.RequestChangeBalance(actions.ChangePlayerCreditsAmount);
                                    PaymentSuccessTriggered = true;

                                }

                            }

                        }

                    }

                    if (actions.ChangePlayerCreditsIncludeSavedPlayerIdentity && !SavedPlayerIdentityAlreadyIncluded && command != null)
                    {

                        if (command.PlayerIdentity != 0)
                        {

                            var player = PlayerManager.GetPlayerWithIdentityId(command.PlayerIdentity);

                            if (player != null)
                            {

                                long credits = 0;
                                player.Player.TryGetBalanceInfo(out credits);

                                if (actions.ChangePlayerCreditsAmount > 0)
                                {

                                    player.Player.RequestChangeBalance(actions.ChangePlayerCreditsAmount);
                                    PaymentSuccessTriggered = true;

                                }
                                else
                                {

                                    if (actions.ChangePlayerCreditsAmount > credits)
                                    {

                                        PaymentFailureTriggered = true;

                                    }
                                    else
                                    {

                                        player.Player.RequestChangeBalance(actions.ChangePlayerCreditsAmount);
                                        PaymentSuccessTriggered = true;

                                    }

                                }

                            }

                        }
                    }
                }


                //ChangeNpcFactionCredits
                lastAction = "ChangeNpcFactionCredits";
                if (actions.ChangeNpcFactionCredits)
                {

                    IMyFaction faction = null;

                    if (string.IsNullOrWhiteSpace(actions.ChangeNpcFactionCreditsTag))
                    {

                        faction = _behavior.Owner.Faction;

                    }
                    else
                    {

                        faction = MyAPIGateway.Session.Factions.TryGetFactionByTag(actions.ChangeNpcFactionCreditsTag);

                    }

                    if (faction != null)
                    {

                        long credits = 0;
                        faction.TryGetBalanceInfo(out credits);

                        if (actions.ChangePlayerCreditsAmount > 0)
                        {

                            faction.RequestChangeBalance(actions.ChangePlayerCreditsAmount);
                            PaymentSuccessTriggered = true;

                        }
                        else
                        {

                            if (actions.ChangePlayerCreditsAmount > credits)
                            {

                                PaymentFailureTriggered = true;

                            }
                            else
                            {

                                faction.RequestChangeBalance(actions.ChangePlayerCreditsAmount);
                                PaymentSuccessTriggered = true;

                            }

                        }

                    }

                }

                //RefreshTarget
                lastAction = "RefreshTarget";
                if (actions.RefreshTarget == true)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Target Refresh", BehaviorDebugEnum.Action);
                    _autopilot.Targeting.ForceRefresh = true;

                }

                //ChangeTargetProfile
                lastAction = "ChangeTargetProfile";
                if (actions.ChangeTargetProfile == true)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Target Profile Change", BehaviorDebugEnum.Action);
                    _autopilot.Targeting.UseNewTargetProfile = true;
                    _autopilot.Targeting.NewTargetProfileName = actions.NewTargetProfileId;

                }

                //ChangeReputationWithPlayers
                lastAction = "ChangeReputationWithPlayers";
                if (actions.ChangeReputationWithPlayers == true)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Reputation Change With Players In Radius", BehaviorDebugEnum.Action);
                    FactionHelper.ChangeReputationWithPlayersInRadius(RemoteControl, actions.ReputationChangeRadius, actions.ReputationChangeAmount, actions.ReputationChangeFactions, actions.ReputationChangesForAllRadiusPlayerFactionMembers, actions.ReputationMinCap, actions.ReputationMaxCap, actions.ReputationPlayerConditionIds);

                }

                //ChangeAttackerReputation
                lastAction = "ChangeAttackerReputation";
                if (actions.ChangeAttackerReputation == true && detectedEntity != 0)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Reputation Change for Attacker", BehaviorDebugEnum.Action);
                    FactionHelper.ChangeDamageOwnerReputation(RemoteControl, actions.ChangeAttackerReputationFaction, detectedEntity, actions.ChangeAttackerReputationAmount, actions.ReputationChangesForAllAttackPlayerFactionMembers, actions.ReputationMinCap, actions.ReputationMaxCap);

                }

                //ChangeAttackersFactionAccount
                lastAction = "ChangeAttackersFactionAccount";
                if (actions.ChangeAttackersFactionAccount == true)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting FactionAccount Change for Attacker", BehaviorDebugEnum.Action);
                    FactionHelper.ChangeFactionAccountByAmount(RemoteControl, actions.ChangeAttackersFactionAccountByAmount, _settings.LastDamagerEntity);

                }

                //TriggerTimerBlock
                lastAction = "TriggerTimerBlock";
                if (actions.TriggerTimerBlocks == true)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Trigger of Timer Blocks", BehaviorDebugEnum.Action);
                    var blockList = BlockCollectionHelper.GetBlocksWithNames(RemoteControl.SlimBlock.CubeGrid, actions.TimerBlockNames);

                    foreach (var block in blockList)
                    {

                        var tBlock = block as IMyTimerBlock;

                        if (tBlock != null)
                        {

                            tBlock.Trigger();

                        }

                    }

                }

                //ChangeBlockNames
                lastAction = "ChangeBlockNames";
                if (actions.ChangeBlockNames)
                {

                    _behavior.Grid.RenameBlocks(actions.ChangeBlockNamesFrom, actions.ChangeBlockNamesTo, actions.ProfileSubtypeId);

                }

                //ToggleBlocksOfType
                lastAction = "ToggleBlocksOfType";
                if (actions.ToggleBlocksOfType)
                {

                    _behavior.Grid.ToggleBlocksOfType(actions.BlockTypesToToggle, actions.BlockTypeToggles);

                }

                //ChangeAntennaRanges
                lastAction = "ChangeAntennaRanges";
                if (actions.ChangeAntennaRanges)
                {

                    _behavior.Grid.SetGridAntennaRanges(actions.AntennaNamesForRangeChange, actions.AntennaRangeChangeType, actions.AntennaRangeChangeAmount);

                }

                //ChangeAntennaOwnership
                lastAction = "ChangeAntennaOwnership";
                if (actions.ChangeAntennaOwnership == true)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Antenna Ownership Change Block Count: " + AntennaList.Count, BehaviorDebugEnum.Action);
                    OwnershipHelper.ChangeAntennaBlockOwnership(AntennaList, actions.AntennaFactionOwner);

                }

                // ChangeAntennaHudText
                lastAction = "ChangeAntennaHudText";
                if (actions.ChangeAntennaHudText == true)
                {
                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Antenna Namechange Change Block Count: " + AntennaList.Count, BehaviorDebugEnum.Action);

                    var antennaHudText = actions.AntennaHudText;

                    if (actions.UseRandomNameGenerator)
                    {
                        antennaHudText = RandomNameGenerator.CreateRandomNameFromPattern(antennaHudText);
                    }


                    foreach (var antenna in AntennaList)
                    {

                        if (antenna == null)
                            continue;

                        antenna.HudText = antennaHudText;

                    }

                }

                // AppendAntennaHudText
                lastAction = "AppendAntennaHudText";
                if (actions.AppendAntennaHudText)
                {
                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Antenna Namechange Change Block Count: " + AntennaList.Count, BehaviorDebugEnum.Action);

                    var antennaHudTextSuffix = actions.AntennaHudTextSuffix;

                    if (actions.UseRandomNameGenerator)
                    {
                        antennaHudTextSuffix = RandomNameGenerator.CreateRandomNameFromPattern(antennaHudTextSuffix);
                    }


                    foreach (var antenna in AntennaList)
                    {

                        if (antenna == null)
                            continue;

                        antenna.HudText = antenna.HudText + antennaHudTextSuffix;

                    }
                }

                // SetAntennaThoughtBubble
                lastAction = "SetAntennaThoughtBubble";
                if (actions.SetAntennaThoughtBubble)
                {
                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Antenna SetAntennaThoughtBubble Change Block Count: " + AntennaList.Count, BehaviorDebugEnum.Action);

                    var thought = actions.SetAntennaThoughtBubbleName;

                    var npcdata = _behavior?.CurrentGrid?.Npc;

                    if (npcdata != null)
                    {
                        npcdata.AntennaThoughtBubble = thought;
                        foreach (var antenna in AntennaList)
                        {

                            if (antenna == null)
                                continue;

                            antenna.HudText = Utilities.SetAntennaThoughtBubble(antenna.HudText, thought, npcdata.AntennaThoughtBubblePercentageActive, npcdata.AntennaThoughtBubblePercentage);

                        }
                    }

                }

                // ClearAntennaThoughtBubble
                lastAction = "ClearAntennaThoughtBubble";
                if (actions.ClearAntennaThoughtBubble)
                {
                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Antenna SetAntennaThoughtBubble Change Block Count: " + AntennaList.Count, BehaviorDebugEnum.Action);



                    var npcdata = _behavior?.CurrentGrid?.Npc;

                    if (npcdata != null)
                    {
                        npcdata.AntennaThoughtBubble = "";
                        foreach (var antenna in AntennaList)
                        {

                            if (antenna == null)
                                continue;

                            antenna.HudText = Utilities.ClearAntennaThoughtBubble(antenna.HudText);

                        }
                    }

                }

                // StartAntennaThoughtBubblePercentage
                lastAction = "StartAntennaThoughtBubblePercentage";
                if (actions.StartAntennaThoughtBubblePercentage)
                {
                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Antenna StartAntennaThoughtBubblePercentage Change Block Count: " + AntennaList.Count, BehaviorDebugEnum.Action);

                    var startingvalue = actions.StartAntennaThoughtBubblePercentageValue;

                    var npcdata = _behavior?.CurrentGrid?.Npc;

                    if (npcdata != null)
                    {
                        npcdata.AntennaThoughtBubblePercentage = startingvalue;
                        npcdata.AntennaThoughtBubblePercentageActive = true;

                        foreach (var antenna in AntennaList)
                        {

                            if (antenna == null)
                                continue;

                            antenna.HudText = Utilities.SetAntennaThoughtBubble(antenna.HudText, npcdata.AntennaThoughtBubble, true, startingvalue);

                        }
                    }

                }

                // IncreaseAntennaThoughtBubblePercentage
                lastAction = "IncreaseAntennaThoughtBubblePercentage";
                if (actions.IncreaseAntennaThoughtBubblePercentage)
                {
                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Antenna StartAntennaThoughtBubblePercentage Change Block Count: " + AntennaList.Count, BehaviorDebugEnum.Action);

                    var increasevalue = actions.IncreaseAntennaThoughtBubblePercentageAmount; ;

                    var npcdata = _behavior?.CurrentGrid?.Npc;

                    if (npcdata != null)
                    {

                        var currentvalue = npcdata.AntennaThoughtBubblePercentage;

                        //If  not active then activate and start at 1...
                        if (!npcdata.AntennaThoughtBubblePercentageActive)
                        {
                            npcdata.AntennaThoughtBubblePercentageActive = true;
                            npcdata.AntennaThoughtBubblePercentage = 1;
                            currentvalue = 1;

                        }




                        var newvalue = currentvalue + Math.Abs(increasevalue);

                        newvalue = Math.Min(100, newvalue);
                        newvalue = Math.Max(0, newvalue);

                        if (newvalue <= 0)
                        {
                            AntennaThoughtBubblePercentageReachedMinTriggered = true;
                            newvalue = 0;
                        }

                        if (newvalue >= 100)
                        {
                            AntennaThoughtBubblePercentageReachedMaxTriggered = true;
                            newvalue = 100;
                        }

                        npcdata.AntennaThoughtBubblePercentage = newvalue;



                        foreach (var antenna in AntennaList)
                        {

                            if (antenna == null)
                                continue;

                            antenna.HudText = Utilities.SetAntennaThoughtBubble(antenna.HudText, npcdata.AntennaThoughtBubble, true, newvalue);

                        }
                    }

                }

                // DecreaseAntennaThoughtBubblePercentage
                lastAction = "DecreaseAntennaThoughtBubblePercentage";
                if (actions.DecreaseAntennaThoughtBubblePercentage)
                {
                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Antenna StartAntennaThoughtBubblePercentage Change Block Count: " + AntennaList.Count, BehaviorDebugEnum.Action);

                    var decreasevalue = actions.DecreaseAntennaThoughtBubblePercentageAmount; ;

                    var npcdata = _behavior?.CurrentGrid?.Npc;

                    if (npcdata != null)
                    {
                        var currentvalue = npcdata.AntennaThoughtBubblePercentage;


                        //If  not active then activate and start at 100
                        if (!npcdata.AntennaThoughtBubblePercentageActive)
                        {
                            npcdata.AntennaThoughtBubblePercentageActive = true;
                            npcdata.AntennaThoughtBubblePercentage = 100;
                            currentvalue = 100;

                        }


                        var newvalue = currentvalue - Math.Abs(decreasevalue);

                        newvalue = Math.Min(100, newvalue);
                        newvalue = Math.Max(0, newvalue);

                        if (newvalue <= 0)
                        {
                            AntennaThoughtBubblePercentageReachedMinTriggered = true;
                        }

                        if (newvalue >= 100)
                        {
                            AntennaThoughtBubblePercentageReachedMaxTriggered = true;
                        }

                        foreach (var antenna in AntennaList)
                        {

                            if (antenna == null)
                                continue;

                            antenna.HudText = Utilities.SetAntennaThoughtBubble(antenna.HudText, npcdata.AntennaThoughtBubble, true, newvalue);

                        }
                    }

                }

                // EndAntennaThoughtBubblePercentage
                lastAction = "EndAntennaThoughtBubblePercentage";
                if (actions.EndAntennaThoughtBubblePercentage)
                {
                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Antenna StartAntennaThoughtBubblePercentage Change Block Count: " + AntennaList.Count, BehaviorDebugEnum.Action);

                    var decreasevalue = actions.DecreaseAntennaThoughtBubblePercentageAmount; ;

                    var npcdata = _behavior?.CurrentGrid?.Npc;

                    if (npcdata != null)
                    {
                        npcdata.AntennaThoughtBubblePercentageActive = false;
                        npcdata.AntennaThoughtBubblePercentage = 10;



                        foreach (var antenna in AntennaList)
                        {

                            if (antenna == null)
                                continue;

                            antenna.HudText = Utilities.SetAntennaThoughtBubble(antenna.HudText, npcdata.AntennaThoughtBubble, false, 10);

                        }
                    }

                }

                // AddInstanceEventGroup
                lastAction = "AddInstanceEventGroup";
                if (actions.AddInstanceEventGroup)
                {
                    var instanceEventGroupReplaceValues = IdsReplacer.ReplaceIds(_behavior?.CurrentGrid?.Npc ?? null, actions.InstanceEventGroupReplaceValues);
                    LocalApi.InsertInstanceEventGroup(actions.InstanceEventGroupId, actions.InstanceEventGroupReplaceKeys, instanceEventGroupReplaceValues);
                }


                // CreateKnownPlayerArea
                lastAction = "CreateKnownPlayerArea";
                if (actions.CreateKnownPlayerArea == true)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Creation of Known Player Area in MES", BehaviorDebugEnum.Action);
                    KnownPlayerLocationManager.AddKnownPlayerLocation(RemoteControl.GetPosition(), _owner.Faction?.Tag, actions.KnownPlayerAreaRadius, actions.KnownPlayerAreaTimer, actions.KnownPlayerAreaMaxSpawns, actions.KnownPlayerAreaMinThreatForAvoidingAbandonment);

                }

                // RemoveKnownPlayerLocation
                lastAction = "RemoveKnownPlayerLocation";
                if (actions.RemoveKnownPlayerArea == true)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Removal of Known Player Area in MES", BehaviorDebugEnum.Action);
                    KnownPlayerLocationManager.RemoveLocation(RemoteControl.GetPosition(), _owner.Faction?.Tag, actions.RemoveAllKnownPlayerAreas);

                }

                // DamageToolAttacker
                lastAction = "DamageToolAttacker";
                if (actions.DamageToolAttacker == true && detectedEntity != 0)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Damage to Tool User", BehaviorDebugEnum.Action);
                    DamageHelper.ApplyDamageToTarget(attackerEntityId, actions.DamageToolAttackerAmount, actions.DamageToolAttackerParticle, actions.DamageToolAttackerSound);

                }

                // PlayParticleEffectAtRemote
                lastAction = "PlayParticleEffectAtRemote";
                if (actions.PlayParticleEffectAtRemote == true)
                {

                    EffectManager.SendParticleEffectRequest(actions.ParticleEffectId, RemoteControl.WorldMatrix, actions.ParticleEffectOffset, actions.ParticleEffectScale, actions.ParticleEffectMaxTime, actions.ParticleEffectColor);

                }

                // EnableHighestRangeAntennas
                lastAction = "EnableHighestRangeAntennas";
                if (actions.EnableHighestRangeAntennas)
                {

                    _behavior.Grid.ChangeHighestRangeAntennas(true);

                }

                // DisableHighestRangeAntennas
                lastAction = "DisableHighestRangeAntennas";
                if (actions.DisableHighestRangeAntennas)
                {

                    _behavior.Grid.ChangeHighestRangeAntennas(false);

                }

                //ResetCooldownTimeOfTriggers
                lastAction = "ResetCooldownTimeOfTriggers";
                if (actions.ResetCooldownTimeOfTriggers)
                {

                    ToggleTriggers(actions.ResetTriggerCooldownNames, CheckEnum.Ignore, CheckEnum.Yes);
                    ToggleTagTriggers(actions.ResetTriggerCooldownTags, CheckEnum.Ignore, CheckEnum.Yes);
                }

                //EnableTriggers
                lastAction = "EnableTriggers";
                if (actions.EnableTriggers)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + " Attempting To Enable " + actions.EnableTriggerNames.Count + " Triggers.", BehaviorDebugEnum.Action);
                    ToggleTriggers(actions.EnableTriggerNames, CheckEnum.Yes, CheckEnum.Ignore);
                    ToggleTagTriggers(actions.EnableTriggerTags, CheckEnum.Yes, CheckEnum.Ignore);
                }

                //DisableTriggers
                lastAction = "DisableTriggers";
                if (actions.DisableTriggers)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + " Attempting To Disable Triggers.", BehaviorDebugEnum.Action);
                    ToggleTriggers(actions.DisableTriggerNames, CheckEnum.No, CheckEnum.Ignore);
                    ToggleTagTriggers(actions.DisableTriggerTags, CheckEnum.No, CheckEnum.Ignore);
                }

                //ManuallyActivateTrigger
                lastAction = "ManuallyActivateTrigger";
                if (actions.ManuallyActivateTrigger)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + " Attempting To Manually Activate Triggers.", BehaviorDebugEnum.Action);

                    List<string> manuallyActivatedTriggerNames = new List<string>();
                    foreach (var manuallyActivatedTriggerName in actions.ManuallyActivatedTriggerNames)
                    {
                        manuallyActivatedTriggerNames.Add(IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, manuallyActivatedTriggerName));
                    }

                    List<string> manuallyActivatedTriggerTags = new List<string>();
                    foreach (var manuallyActivatedTriggerTag in actions.ManuallyActivatedTriggerTags)
                    {
                        manuallyActivatedTriggerTags.Add(IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, manuallyActivatedTriggerTag));
                    }

                    foreach (var manualTrigger in Triggers)
                    {

                        if (manuallyActivatedTriggerNames.Contains(manualTrigger.ProfileSubtypeId))
                            ProcessManualTrigger(manualTrigger, actions.ForceManualTriggerActivation);

                        foreach (var tag in manualTrigger.Tags)
                        {
                            if (manuallyActivatedTriggerTags.Contains(tag))
                                ProcessManualTrigger(manualTrigger, actions.ForceManualTriggerActivation);

                        }

                    }


                    foreach (var manualTrigger in CompromisedTriggers)
                    {

                        if (manuallyActivatedTriggerNames.Contains(manualTrigger.ProfileSubtypeId))
                            ProcessManualTrigger(manualTrigger, actions.ForceManualTriggerActivation);

                        foreach (var tag in manualTrigger.Tags)
                        {
                            if (manuallyActivatedTriggerTags.Contains(tag))
                                ProcessManualTrigger(manualTrigger, actions.ForceManualTriggerActivation);

                        }

                    }



                }

                //ChangeInertiaDampeners
                lastAction = "ChangeInertiaDampeners";
                if (actions.ChangeInertiaDampeners)
                {

                    RemoteControl.DampenersOverride = actions.InertiaDampenersEnable;

                }

                //ChangeRotationDirection
                lastAction = "ChangeRotationDirection";
                if (actions.ChangeRotationDirection)
                {

                    _behavior.BehaviorSettings.SetRotation(actions.RotationDirection);
                    _behavior.AutoPilot.StopAllThrust();

                }

                //GenerateExplosion
                lastAction = "GenerateExplosion";
                if (actions.GenerateExplosion)
                {

                    var coords = Vector3D.Transform(actions.ExplosionOffsetFromRemote, RemoteControl.WorldMatrix);
                    DamageHelper.CreateExplosion(coords, actions.ExplosionRange, actions.ExplosionDamage, RemoteControl, actions.ExplosionIgnoresVoxels);

                }

                //GridEditable
                lastAction = "GridEditable";
                if (actions.GridEditable != CheckEnum.Ignore)
                {

                    _behavior.Grid.SetGridEditable(RemoteControl.SlimBlock.CubeGrid, actions.GridEditable == CheckEnum.Yes);

                    if (actions.SubGridsEditable != CheckEnum.Ignore)
                    {

                        foreach (var cubeGrid in MyAPIGateway.GridGroups.GetGroup(RemoteControl.SlimBlock.CubeGrid, GridLinkTypeEnum.Physical))
                        {

                            _behavior.Grid.SetGridEditable(cubeGrid, actions.SubGridsEditable == CheckEnum.Yes);

                        }

                    }

                }

                //GridDestructible
                lastAction = "GridDestructible";
                if (actions.GridDestructible != CheckEnum.Ignore)
                {

                    _behavior.Grid.SetGridDestructible(RemoteControl.SlimBlock.CubeGrid, actions.GridDestructible == CheckEnum.Yes);

                    if (actions.SubGridsDestructible != CheckEnum.Ignore)
                    {

                        foreach (var cubeGrid in MyAPIGateway.GridGroups.GetGroup(RemoteControl.SlimBlock.CubeGrid, GridLinkTypeEnum.Physical))
                        {

                            _behavior.Grid.SetGridDestructible(cubeGrid, actions.SubGridsDestructible == CheckEnum.Yes);

                        }

                    }

                }

                //RecolorGrid
                lastAction = "RecolorGrid";
                if (actions.RecolorGrid)
                {

                    _behavior.Grid.RecolorBlocks(RemoteControl.SlimBlock.CubeGrid, actions.OldBlockColors, actions.OldBlockSkins, actions.NewBlockColors, actions.NewBlockSkins);

                    if (actions.RecolorSubGrids)
                    {

                        foreach (var cubeGrid in MyAPIGateway.GridGroups.GetGroup(RemoteControl.SlimBlock.CubeGrid, GridLinkTypeEnum.Physical))
                        {

                            _behavior.Grid.RecolorBlocks(cubeGrid, actions.OldBlockColors, actions.OldBlockSkins, actions.NewBlockColors, actions.NewBlockSkins);

                        }

                    }

                }

                //EnableBlocks
                lastAction = "EnableBlocks";
                if (actions.EnableBlocks)
                {

                    _behavior.Grid.EnableBlocks(actions.EnableBlockNames, actions.EnableBlockStates);

                }

                //HighlightBlocks
                lastAction = "HighlightBlocks";
                if (actions.HighlightBlocks)
                {

                    _behavior.Grid.HighlightBlocks(actions.HighlightBlockNames, actions.HighlightBlockModes);

                }


                //BuildProjectedBlocks
                lastAction = "BuildProjectedBlocks";
                if (actions.BuildProjectedBlocks)
                {

                    _behavior.Grid.BuildProjectedBlocks(actions.MaxProjectedBlocksToBuild);

                }

                //RepairBlocks
                lastAction = "RepairBlocks";
                if (actions.RepairBlocks)
                {
                    if (!actions.RepairBlocksIncludeSubgrids)
                    {

                        _behavior.Grid.BuildProjectedBlocks(actions.MaxProjectedBlocksToBuild);

                    }
                    else
                    {

                        int remainingBlocks = actions.MaxProjectedBlocksToBuild;

                        if (_behavior?.CurrentGrid != null)
                        {

                            lock (_behavior.CurrentGrid.LinkedGrids)
                            {

                                for (int i = _behavior.CurrentGrid.LinkedGrids.Count - 1; i >= 0; i--)
                                {

                                    var grid = GridManager.GetSafeGridFromIndex(i, _behavior.CurrentGrid.LinkedGrids);

                                    if (grid == null || !grid.ActiveEntity())
                                        continue;

                                    remainingBlocks -= grid.AutoRepairBlocks(false, remainingBlocks);

                                    if (remainingBlocks <= 0)
                                        break;

                                }

                            }

                        }

                    }

                }

                //ChangeGridOwnership
                lastAction = "ChangeGridOwnership";
                if (actions.ChangeGridOwnership)
                {
                    var blockList = BlockCollectionHelper.GetBlocksOfType<IMyTerminalBlock>(RemoteControl.SlimBlock.CubeGrid);

                    IMyFaction faction = MyAPIGateway.Session.Factions.TryGetFactionByTag(actions.OwnershipBlockFactions[0]);
                    if (faction != null)
                    {
                        var newOwner = FactionHelper.GetFactionOwner(faction);

                        foreach (var block in blockList)
                        {
                            var cubeBlock = block as MyCubeBlock;
                            cubeBlock.ChangeOwner(newOwner, MyOwnershipShareModeEnum.Faction);
                            cubeBlock.ChangeBlockOwnerRequest(newOwner, MyOwnershipShareModeEnum.Faction);
                        }
                    }
                }

                //ChangeBlockOwnership
                lastAction = "ChangeBlockOwnership";
                if (actions.ChangeBlockOwnership)
                {

                    if (actions.OwnershipBlockNames.Count == actions.OwnershipBlockFactions.Count)
                        BlockCollectionHelper.ChangeBlockOwnership(RemoteControl.SlimBlock.CubeGrid, actions.OwnershipBlockNames, actions.OwnershipBlockFactions);
                    else
                        BehaviorLogger.Write(actions.ProfileSubtypeId + ": Change Block Ownership Failed. Block Name List and Faction List Count Mismatch.", BehaviorDebugEnum.Action);

                }

                //RazeBlocksWithNames
                lastAction = "RazeBlocksWithNames";
                if (actions.RazeBlocksWithNames)
                {

                    _behavior.Grid.RazeBlocksWithNames(actions.RazeBlocksNames);

                }

                //RazeBlocksOfType
                lastAction = "RazeBlocksOfType";
                if (actions.RazeBlocksOfType)
                {

                    _behavior.Grid.RazeBlocksWithTypes(actions.RazeBlocksTypes);

                }

                //OverwriteAutopilotProfile
                lastAction = "OverwriteAutopilotProfile";
                if (actions.OverwriteAutopilotProfile)
                {

                    _behavior.AutoPilot.AssignAutoPilotDataMode(actions.OverwriteAutopilotId, actions.OverwriteAutopilotMode);

                }

                //ChangeAutoPilotProfile
                lastAction = "ChangeAutopilotProfile";
                if (actions.ChangeAutopilotProfile)
                {

                    _behavior.AutoPilot.SetAutoPilotDataMode(actions.AutopilotProfile);

                }

                //CreateRandomLightning
                lastAction = "CreateRandomLightning";
                if (actions.CreateRandomLightning)
                {

                    if (_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.CurrentPlanet.HasAtmosphere)
                    {

                        var up = Vector3D.Normalize(RemoteControl.GetPosition() - _behavior.AutoPilot.CurrentPlanet.Center());
                        var randomPerpendicular = MyUtils.GetRandomPerpendicularVector(ref up);
                        var strikeCoords = _behavior.AutoPilot.CurrentPlanet.SurfaceCoordsAtPosition(randomPerpendicular * MathTools.RandomBetween(actions.LightningMinDistance, actions.LightningMaxDistance) + RemoteControl.GetPosition());
                        DamageHelper.CreateLightning(strikeCoords, actions.LightningDamage, actions.LightningExplosionRadius, actions.LightningColor);

                    }

                }

                //CreateLightningAtAttacker
                lastAction = "CreateLightningAtAttacker";
                if (actions.CreateLightningAtAttacker && detectedEntity != 0)
                {

                    if (_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.CurrentPlanet.HasAtmosphere)
                    {

                        IMyEntity entity = null;

                        if (MyAPIGateway.Entities.TryGetEntityById(detectedEntity, out entity))
                        {

                            DamageHelper.CreateLightning(entity.PositionComp.WorldAABB.Center, actions.LightningDamage, actions.LightningExplosionRadius, actions.LightningColor);

                        }

                    }

                }

                //CreateLightningAtTarget
                lastAction = "CreateLightningAtTarget";
                if (actions.CreateLightningAtTarget && _behavior.AutoPilot.Targeting.HasTarget())
                {

                    if (_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.CurrentPlanet.HasAtmosphere)
                    {

                        DamageHelper.CreateLightning(_behavior.AutoPilot.Targeting.TargetLastKnownCoords, actions.LightningDamage, actions.LightningExplosionRadius, actions.LightningColor);

                    }

                }

                //AddDatapadsToSeats
                lastAction = "AddDatapadsToSeats";
                if (actions.AddDatapadsToSeats)
                {

                    _behavior.Grid.InsertDatapadsIntoSeats(actions.DatapadNamesToAdd, actions.DatapadCountToAdd);

                }

                //StopAllRotation
                lastAction = "StopAllRotation";
                if (actions.StopAllRotation)
                {

                    _behavior.AutoPilot.StopAllRotation();
                    _behavior.AutoPilot.ApplyGyroRotation();

                }

                //StopAllThrust
                lastAction = "StopAllThrust";
                if (actions.StopAllThrust)
                {

                    _behavior.AutoPilot.StopAllThrust();
                    _behavior.AutoPilot.ApplyThrust();

                }

                //RandomGyroRotation
                lastAction = "RandomGyroRotation";
                if (actions.RandomGyroRotation)
                {

                    if (MathTools.RandomBool())
                        _behavior.AutoPilot.RotationToApply.X = MathTools.RandomBetween(-314, 315, 100);

                    if (MathTools.RandomBool())
                        _behavior.AutoPilot.RotationToApply.Y = MathTools.RandomBetween(-314, 315, 100);

                    if (MathTools.RandomBool())
                        _behavior.AutoPilot.RotationToApply.Z = MathTools.RandomBetween(-314, 315, 100);

                    _behavior.AutoPilot.PrepareGyroForRotation();
                    _behavior.AutoPilot.ApplyGyroRotation();

                }

                //RandomThrustDirection
                lastAction = "RandomThrustDirection";
                if (actions.RandomThrustDirection)
                {

                    _behavior.AutoPilot.SetRandomThrust();
                    _behavior.AutoPilot.ApplyThrust();

                }

                //DisableAutopilot
                lastAction = "DisableAutopilot";
                if (actions.DisableAutopilot && _autopilot.State != null)
                    _autopilot.State.DisableAutopilot = true;

                //EnableAutopilot
                lastAction = "EnableAutopilot";
                if (actions.EnableAutopilot && _autopilot.State != null)
                    _autopilot.State.DisableAutopilot = false;

                //ChangeZoneAtPosition (While Inside Zone)
                lastAction = "ChangeZoneAtPosition";
                if (actions.ChangeZoneAtPosition)
                {

                    //ZoneToggleActiveAtPosition
                    lastAction = "ZoneToggleActiveAtPosition";
                    if (actions.ZoneToggleActiveAtPosition)
                        ZoneManager.ToggleZonesAtPosition(RemoteControl.GetPosition(), IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, actions.ZoneName), actions.ZoneToggleActiveAtPositionMode);

                    //ZoneRadiusChangeType
                    lastAction = "ZoneRadiusChangeType";
                    if (actions.ZoneRadiusChangeType != ModifierEnum.None)
                        ZoneManager.ChangeZoneRadius(RemoteControl.GetPosition(), IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, actions.ZoneName), actions.ZoneRadiusChangeAmount, actions.ZoneRadiusChangeType);

                    //ZoneCustomBoolChange
                    lastAction = "ZoneCustomBoolChange";
                    if (actions.ZoneCustomBoolChange)
                        if (actions.ZoneCustomBoolChangeUseKPL)
                            ZoneManager.ChangeZoneBools(RemoteControl.GetPosition(), IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, actions.ZoneName), actions.ZoneCustomBoolChangeName, actions.ZoneCustomBoolChangeValue);
                        else
                            ZoneManager.ChangeKPLBools(RemoteControl.GetPosition(), _behavior.Owner.Faction?.Tag ?? "Nobody", actions.ZoneCustomBoolChangeName, actions.ZoneCustomBoolChangeValue);

                    //ZoneCustomCounterChange
                    lastAction = "ZoneCustomCounterChange";
                    if (actions.ZoneCustomCounterChange)
                        if (actions.ZoneCustomCounterChangeUseKPL)
                            ZoneManager.ChangeZoneCounters(RemoteControl.GetPosition(), IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, actions.ZoneName), actions.ZoneCustomCounterChangeName, actions.ZoneCustomCounterChangeAmount, actions.ZoneCustomCounterChangeType);
                        else
                            ZoneManager.ChangeKPLCounters(RemoteControl.GetPosition(), _behavior.Owner.Faction?.Tag ?? "Nobody", actions.ZoneCustomCounterChangeName, actions.ZoneCustomCounterChangeAmount, actions.ZoneCustomCounterChangeType);

                }

                //Toggle Zone
                lastAction = "ZoneToggleActive";
                if (actions.ZoneToggleActive)
                    ZoneManager.ToggleZones(IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, actions.ZoneName), actions.ZoneToggleActiveMode);

                //SetCustomStrings
                lastAction = "SetCustomStrings";
                if (actions.SetCustomStrings)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + " Attempting To Set Custom Strings.", BehaviorDebugEnum.Action);

                    var npcdata = _behavior?.CurrentGrid?.Npc;
                    foreach (var customvar in actions.CustomStrings)
                    {
                        npcdata.CustomStrings[customvar.Key] = customvar.Value;
                    }

                }

                //SetCustomCountersVariables
                lastAction = "SetCustomCountersVariables";
                if (actions.SetCustomCountersVariables)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + " Attempting To Set Custom Counter Target Variable.", BehaviorDebugEnum.Action);

                    var npcdata = _behavior?.CurrentGrid?.Npc;
                    foreach (var customvar in actions.CustomCountersVariables)
                    {
                        npcdata.CustomCountersVariables[customvar.Key] = customvar.Value;
                    }

                }

                //ChangeBehaviorSubclass
                lastAction = "ChangeBehaviorSubclass";
                if (actions.ChangeBehaviorSubclass)
                {

                    //_behavior.BehaviorSettings.ActiveBehaviorType = actions.NewBehaviorSubclass;
                    _behavior.AssignSubClassBehavior(actions.NewBehaviorSubclass);
                    //TODO: Add Custom BehaviorMode Override

                }

                // AddBotsToGrid
                lastAction = "AddBotsToGrid";
                if (actions.AddBotsToGrid && _behavior.CurrentGrid != null && APIs.AiEnabled.Valid && actions.BotSpawnProfileNames.Count > 0)
                {

                    //MyVisualScriptLogicProvider.ShowNotificationToAll("Attempting To Add Bots", 3000);

                    List<Vector3D> list = new List<Vector3D>();
                    APIs.AiEnabled.GetAvailableGridNodes(_behavior.CurrentGrid.CubeGrid as MyCubeGrid, actions.BotCount, list, RemoteControl.WorldMatrix.Up, actions.OnlySpawnBotsInPressurizedRooms);
                    //var list = APIs.AiEnabled.GetAvailableGridNodes(_behavior.CurrentGrid.CubeGrid as MyCubeGrid, actions.BotCount, RemoteControl.WorldMatrix.Up, actions.OnlySpawnBotsInPressurizedRooms);

                    //MyVisualScriptLogicProvider.ShowNotificationToAll("Node Count: " + list.Count, 3000);

                    for (int i = 0; i < actions.BotCount; i++)
                    {

                        if (list.Count == 0)
                            break;

                        var cell = list[MathTools.RandomBetween(0, list.Count)];
                        var botProfileName = actions.BotSpawnProfileNames[MathTools.RandomBetween(0, actions.BotSpawnProfileNames.Count)];
                        BotSpawnProfile botProfile = null;

                        if (ProfileManager.BotSpawnProfiles.TryGetValue(botProfileName, out botProfile))
                        {

                            var coords = cell;
                            var matrix = MatrixD.CreateWorld(coords, RemoteControl.WorldMatrix.Backward, RemoteControl.WorldMatrix.Up);
                            IMyCharacter character = null;

                            BotSpawner.SpawnBotRequest(botProfile.SerializedData, matrix, out character, _behavior.CurrentGrid.CubeGrid as MyCubeGrid, RemoteControl.OwnerId);

                        }
                        else
                        {

                            //MyVisualScriptLogicProvider.ShowNotificationToAll("Bot Spawn Profile Not Found: " + botProfileName, 3000);

                        }

                        list.Remove(cell);

                    }

                    //MyVisualScriptLogicProvider.ShowNotificationToAll("Add Bots Done", 3000);

                }

                // SetWeaponsToMinRange
                lastAction = "SetWeaponsToMinRange";
                if (actions.SetWeaponsToMinRange)
                {

                    if (_behavior.CurrentGrid != null)
                        _behavior.CurrentGrid.SetAutomatedWeaponRanges(false);

                }

                // SetWeaponsToMaxRange
                lastAction = "SetWeaponsToMaxRange";
                if (actions.SetWeaponsToMaxRange)
                {

                    if (_behavior.CurrentGrid != null)
                        _behavior.CurrentGrid.SetAutomatedWeaponRanges(true);

                }

                //UseJetpackInhibitorEffect
                lastAction = "UseJetpackInhibitorEffect";
                if (actions.UseJetpackInhibitorEffect != BoolEnum.None)
                {

                    if (actions.UseJetpackInhibitorEffect == BoolEnum.True)
                    {

                        var remoteEntity = _behavior.RemoteControlBlockEntity;

                        if (_behavior.JetpackInhibitorLogic == null && remoteEntity != null)
                        {

                            _behavior.JetpackInhibitorLogic = new JetpackInhibitor(remoteEntity);

                        }

                    }
                    else
                    {

                        if (_behavior.JetpackInhibitorLogic != null)
                        {

                            _behavior.JetpackInhibitorLogic.Invalidate();
                            _behavior.JetpackInhibitorLogic = null;

                        }

                    }

                }

                //UseDrillInhibitorEffect
                lastAction = "UseDrillInhibitorEffect";
                if (actions.UseDrillInhibitorEffect != BoolEnum.None)
                {

                    if (actions.UseDrillInhibitorEffect == BoolEnum.True)
                    {

                        var remoteEntity = _behavior.RemoteControlBlockEntity;

                        if (_behavior.DrillInhibitorLogic == null && remoteEntity != null)
                        {

                            _behavior.DrillInhibitorLogic = new DrillInhibitor(remoteEntity);

                        }

                    }
                    else
                    {

                        if (_behavior.DrillInhibitorLogic != null)
                        {

                            _behavior.DrillInhibitorLogic.Invalidate();
                            _behavior.DrillInhibitorLogic = null;

                        }

                    }

                }

                //UseNanobotInhibitorEffect
                lastAction = "UseNanobotInhibitorEffect";
                if (actions.UseNanobotInhibitorEffect != BoolEnum.None)
                {

                    if (actions.UseNanobotInhibitorEffect == BoolEnum.True)
                    {

                        var remoteEntity = _behavior.RemoteControlBlockEntity;

                        if (_behavior.NanobotInhibitorLogic == null && remoteEntity != null)
                        {

                            _behavior.NanobotInhibitorLogic = new NanobotInhibitor(remoteEntity);

                        }

                    }
                    else
                    {

                        if (_behavior.NanobotInhibitorLogic != null)
                        {

                            _behavior.NanobotInhibitorLogic.Invalidate();
                            _behavior.NanobotInhibitorLogic = null;

                        }

                    }

                }

                //UseJumpInhibitorEffect
                lastAction = "UseJumpInhibitorEffect";
                if (actions.UseJumpInhibitorEffect != BoolEnum.None)
                {

                    if (actions.UseJumpInhibitorEffect == BoolEnum.True)
                    {

                        var remoteEntity = _behavior.RemoteControlBlockEntity;

                        if (_behavior.JumpInhibitorLogic == null && remoteEntity != null)
                        {

                            _behavior.JumpInhibitorLogic = new JumpDriveInhibitor(remoteEntity);

                        }

                    }
                    else
                    {

                        if (_behavior.JumpInhibitorLogic != null)
                        {

                            _behavior.JumpInhibitorLogic.Invalidate();
                            _behavior.JumpInhibitorLogic = null;

                        }

                    }

                }

                //UsePlayerInhibitorEffect
                lastAction = "UsePlayerInhibitorEffect";
                if (actions.UsePlayerInhibitorEffect != BoolEnum.None)
                {

                    if (actions.UsePlayerInhibitorEffect == BoolEnum.True)
                    {

                        var remoteEntity = _behavior.RemoteControlBlockEntity;

                        if (_behavior.PlayerInhibitorLogic == null && remoteEntity != null)
                        {

                            _behavior.PlayerInhibitorLogic = new PlayerInhibitor(remoteEntity);

                        }

                    }
                    else
                    {

                        if (_behavior.PlayerInhibitorLogic != null)
                        {

                            _behavior.PlayerInhibitorLogic.Invalidate();
                            _behavior.PlayerInhibitorLogic = null;

                        }

                    }

                }

                //SetGridCleanupExempt
                lastAction = "SetGridCleanupExempt";
                if (actions.SetGridCleanupExempt && _behavior.CurrentGrid != null)
                {

                    Cleaning.ExemptGrids.Add(new GridCleanupExemption(_behavior.CurrentGrid, actions.GridCleanupExemptDuration));

                }

                //JumpToTarget
                lastAction = "JumpToTarget";
                if (actions.JumpToTarget && _behavior.CurrentGrid != null && _behavior.AutoPilot.Targeting.HasTarget())
                {

                    var jumpResult = _behavior.Grid.JumpToCoords(_behavior.AutoPilot.Targeting.TargetLastKnownCoords);

                    if (jumpResult)
                    {

                        EventWatcher.GridJumped(0, "", RemoteControl.SlimBlock.CubeGrid.EntityId);

                    }

                    BehaviorLogger.Write("Attempt Jump To Target Entity Result: " + jumpResult, BehaviorDebugEnum.Action);

                }

                //JumpToJumpedEntity
                lastAction = "JumpToJumpedEntity";
                if (actions.JumpToJumpedEntity && trigger.JumpedGrid != null)
                {

                    var jumpResult = _behavior.Grid.JumpToCoords(trigger.JumpedGrid.GetPosition());

                    if (jumpResult)
                    {

                        EventWatcher.GridJumped(0, "", RemoteControl.SlimBlock.CubeGrid.EntityId);

                    }

                    BehaviorLogger.Write("Attempt Jump To Jumped Entity Result: " + jumpResult, BehaviorDebugEnum.Action);

                }

                //JumpToWaypoint
                lastAction = "JumpToWaypoint";
                if (actions.JumpToWaypoint && _behavior.CurrentGrid != null)
                {

                    var waypoint = EncounterWaypoint.CalculateWaypoint(_behavior, IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, actions.JumpWaypoint));
                    var jumpResult = _behavior.Grid.JumpToCoords(waypoint.GetCoords());

                    if (jumpResult)
                    {

                        EventWatcher.GridJumped(0, "", RemoteControl.SlimBlock.CubeGrid.EntityId);

                    }

                    BehaviorLogger.Write("Attempt Jump To Target Entity Result: " + jumpResult, BehaviorDebugEnum.Action);

                }

                //SpawnPlanet
                lastAction = "SpawnPlanet";
                if (actions.SpawnPlanet)
                {

                    WaypointProfile waypoint = null;

                    if (ProfileManager.WaypointProfiles.TryGetValue(IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, actions.PlanetWaypointProfile), out waypoint))
                    {

                        var coords = waypoint.GenerateEncounterWaypoint(RemoteControl);
                        var pos = coords.GetCoords();
                        var pos2 = (actions.PlanetSize * 1.89186136208056666) * new Vector3D(-0.577350269189626, -0.577350269189626, -0.577350269189626) + pos;
                        var planet = MyAPIGateway.Session.VoxelMaps.SpawnPlanet(actions.PlanetName, actions.PlanetSize, MathTools.RandomBetween(1000000, 10000000), pos2);
                        BehaviorLogger.Write("Planet Created From Action", BehaviorDebugEnum.Action);

                        if (actions.TemporaryPlanet && planet != null)
                        {

                            var planetEntity = PlanetManager.GetPlanetWithId(planet.EntityId);

                            if (planetEntity != null)
                            {

                                var time = MyAPIGateway.Session.GameDateTime + TimeSpan.FromSeconds(actions.PlanetTimeLimit);
                                var data = MyAPIGateway.Utilities.SerializeToBinary<DateTime>(time);
                                var strData = Convert.ToBase64String(data);
                                var entity = planet as IMyEntity;

                                if (entity.Storage == null)
                                    entity.Storage = new MyModStorageComponent();

                                if (entity.Storage.ContainsKey(StorageTools.MesTemporaryPlanetKey))
                                    entity.Storage.Add(StorageTools.MesTemporaryPlanetKey, strData);
                                else
                                    entity.Storage[StorageTools.MesTemporaryPlanetKey] = strData;

                                TaskProcessor.Tasks.Add(new TimedAction(actions.PlanetTimeLimit, planetEntity.DeletePlanet));

                                BehaviorLogger.Write("Temporary Planet Timer and Action Created", BehaviorDebugEnum.Action);

                            }
                            else
                            {

                                BehaviorLogger.Write("Temporary Planet Timer Could Not Be Created. PlanetEntity Object May Not Be Init Yet.", BehaviorDebugEnum.Action);

                            }

                        }

                    }

                }

                //ChangeBlocksShareModeAll
                lastAction = "ChangeBlocksShareModeAll";
                if (actions.ChangeBlocksShareModeAll)
                {

                    if (_behavior?.CurrentGrid != null)
                    {

                        lock (_behavior.CurrentGrid.LinkedGrids)
                        {

                            for (int i = _behavior.CurrentGrid.LinkedGrids.Count - 1; i >= 0; i--)
                            {

                                var grid = GridManager.GetSafeGridFromIndex(i, _behavior.CurrentGrid.LinkedGrids);

                                if (grid == null || !grid.ActiveEntity())
                                    continue;

                                lock (grid.AllTerminalBlocks)
                                {

                                    for (int j = grid.AllTerminalBlocks.Count - 1; j >= 0; j--)
                                    {

                                        var block = grid.AllTerminalBlocks[i];

                                        if (block == null || !block.ActiveEntity())
                                            continue;

                                        foreach (var name in actions.BlockNamesShareModeAll)
                                        {

                                            if (name == block.Block.CustomName)
                                            {

                                                var cubeBlock = block.Block as MyCubeBlock;
                                                cubeBlock.ChangeBlockOwnerRequest(block.Block.OwnerId, VRage.Game.MyOwnershipShareModeEnum.All);
                                                break;

                                            }

                                        }

                                    }

                                }

                            }

                        }

                    }

                }

                lastAction = "ActivateEvent";
                if (actions.ActivateEvent)
                {
                    var _ActivateEventTags = IdsReplacer.ReplaceIds(_behavior?.CurrentGrid?.Npc ?? null, actions.ActivateEventTags);
                    var _ActivateEventIds = IdsReplacer.ReplaceIds(_behavior?.CurrentGrid?.Npc ?? null, actions.ActivateEventIds);


                    //Something doesn't feel right here - CPT
                    for (int i = 0; i < Events.EventManager.EventsList.Count; i++)
                    {
                        var thisEvent = Events.EventManager.EventsList[i];

                        if (!thisEvent.Valid)
                        {
                            continue;
                        }

                        for (int j = 0; j < _ActivateEventTags.Count; j++)
                        {
                            var thisEventTag = _ActivateEventTags[j];

                            if (thisEvent.Profile.Tags.Contains(thisEventTag))
                            {
                                thisEvent.ActivateEventActions();
                                thisEvent.RunCount++;
                            }
                        }


                        for (int j = 0; j < _ActivateEventIds.Count; j++)
                        {
                            var thisEventId = _ActivateEventIds[j];

                            var SpawnGroupName = _behavior?.CurrentGrid?.Npc.SpawnGroupName;
                            var Faction = _behavior?.CurrentGrid?.Npc.InitialFaction;

                            if (thisEvent.ProfileSubtypeId == thisEventId)
                            {
                                thisEvent.ActivateEventActions();
                                thisEvent.RunCount++;
                            }
                        }



                    }
                }

                lastAction = "AddCustomDataToBlocks";
                if (actions.AddCustomDataToBlocks)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Adding Custom Data To Blocks. [Block Names: " + actions.CustomDataBlockNames.Count + "] [CustomData TextTemplates: " + actions.CustomDataFiles.Count + "]", BehaviorDebugEnum.Action);
                    _behavior.Grid.AddCustomData(actions.CustomDataBlockNames, actions.CustomDataFiles);

                }

                lastAction = "ApplyLcdChanges";
                if (actions.ApplyLcdChanges)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Adding LCD Content To Blocks.", BehaviorDebugEnum.Action);
                    _behavior.Grid.ApplyLcdContents(actions.LcdTextTemplateFile, actions.LcdBlockNames, actions.LcdTemplateIndexes);

                }

                lastAction = "ApplyContainerTypeToInventoryBlock";
                if (actions.ApplyContainerTypeToInventoryBlock)
                {

                    _behavior.Grid.ApplyContainerTypes(actions.ContainerTypeBlockNames, actions.ContainerTypeSubtypeIds);

                }

                lastAction = "ApplyStoreProfiles";
                if (actions.ApplyStoreProfiles && (_behavior.CurrentGrid?.ActiveEntity() ?? false) && _behavior.RemoteControl != null)
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Applying Store Profiles.", BehaviorDebugEnum.Action);
                    BehaviorLogger.Write(string.Format("Store Blocks / Store Profiles ::: {0} / {1}", actions.StoreBlocks.Count, actions.StoreProfiles.Count), BehaviorDebugEnum.Action);

                    for (int i = 0; i < actions.StoreProfiles.Count && i < actions.StoreBlocks.Count; i++)
                    {

                        if (string.IsNullOrWhiteSpace(actions.StoreProfiles[i]) || string.IsNullOrWhiteSpace(actions.StoreBlocks[i]))
                            continue;

                        StoreProfile profile = null;

                        if (!ProfileManager.StoreProfiles.TryGetValue(IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, actions.StoreProfiles[i]), out profile))
                        {

                            BehaviorLogger.Write(actions.ProfileSubtypeId + ": Couldn't find Store Profile With Name: " + IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, actions.StoreProfiles[i]), BehaviorDebugEnum.Action);
                            continue;

                        }

                        foreach (var store in _behavior.CurrentGrid.Stores)
                        {

                            if (!store.ActiveEntity() || store.Block.CustomName != IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, actions.StoreBlocks[i]) || store.Block.OwnerId != _behavior.RemoteControl.OwnerId)
                                continue;

                            BehaviorLogger.Write(actions.ProfileSubtypeId + ": Applying Store Profile With Name: " + IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, actions.StoreProfiles[i]), BehaviorDebugEnum.Action);
                            profile.ApplyProfileToBlock(store.Block as IMyStoreBlock, actions.ClearStoreContentsFirst);

                        }

                    }

                }

                lastAction = "ApplyContractProfiles";
                if (actions.ApplyContractProfiles && (_behavior.CurrentGrid?.ActiveEntity() ?? false))
                {

                    BehaviorLogger.Write(actions.ProfileSubtypeId + ": Applying contract Profiles.", BehaviorDebugEnum.Action);
                    //BehaviorLogger.Write(string.Format("Store Blocks / Store Profiles ::: {0} / {1}", actions.ContractBlocks.Count, actions.ContractBlocks.Count), BehaviorDebugEnum.Action);
                    for (int i = 0; i < actions.ContractBlockProfiles.Count && i < actions.ContractBlocks.Count; i++)
                    {

                        if (string.IsNullOrWhiteSpace(actions.ContractBlockProfiles[i]) || string.IsNullOrWhiteSpace(actions.ContractBlocks[i]))
                            continue;

                        ContractBlockProfile profile = null;

                        if (!ProfileManager.ContractBlockProfiles.TryGetValue(actions.ContractBlockProfiles[i], out profile))
                        {

                            BehaviorLogger.Write(actions.ProfileSubtypeId + ": Couldn't find Mission Profile With Name: " + actions.ContractBlockProfiles[i], BehaviorDebugEnum.Action);
                            continue;

                        }

                        foreach (var contractblock in _behavior.CurrentGrid.Contracts)
                        {

                            if (!contractblock.ActiveEntity() || contractblock.Block.CustomName != actions.ContractBlocks[i]) //|| contractblock.Block.OwnerId != _behavior.RemoteControl.OwnerId
                                continue;

                            BehaviorLogger.Write(actions.ProfileSubtypeId + ": Applying Contract Profile With Name: " + actions.ContractBlocks[i], BehaviorDebugEnum.Action);
                            profile.ApplyProfileToBlock(contractblock, _behavior?.CurrentGrid?.Npc.SpawnGroupName ?? "", actions.ClearContractContentsFirst);



                        }

                    }

                }


                lastAction = "UseCurrentPositionAsPatrolReference";
                if (actions.UseCurrentPositionAsPatrolReference && _behavior.RemoteControl != null)
                {

                    _behavior.BehaviorSettings.PatrolOverrideLocation = _behavior.RemoteControl.GetPosition();

                }

                lastAction = "ClearCustomPatrolReference";
                if (actions.ClearCustomPatrolReference)
                {

                    _behavior.BehaviorSettings.PatrolOverrideLocation = Vector3D.Zero;

                }

                lastAction = "SetGridToStatic";
                if (actions.SetGridToStatic && _behavior.RemoteControl != null)
                {

                    _behavior.RemoteControl.SlimBlock.CubeGrid.IsStatic = true;

                }

                lastAction = "SetGridToDynamic";
                if (actions.SetGridToDynamic && _behavior.RemoteControl != null)
                {

                    _behavior.RemoteControl.SlimBlock.CubeGrid.IsStatic = false;

                }

                lastAction = "AddResearchPoints";
                if (actions.AddResearchPoints && command != null)
                {

                    var player = PlayerManager.GetPlayerWithIdentityId(command.PlayerIdentity);

                    if (player != null)
                    {

                        player.Progression.Points += (byte)actions.ResearchPointsAmount;

                    }

                }

                lastAction = "CreateSafeZone";
                if (actions.CreateSafeZone && _behavior.RemoteControl != null)
                {

                    if (actions.IgnoreOtherSafeZonesDuringCreation || !SafeZoneManager.IsPositionInSafeZone(_behavior.RemoteControl.GetPosition()))
                    {

                        SafeZoneProfile profile = null;

                        if (ProfileManager.SafeZoneProfiles.TryGetValue(actions.SafeZoneProfile, out profile))
                        {

                            var matrix = _behavior.RemoteControl.WorldMatrix;

                            if (actions.SafeZonePositionGridCenter)
                                matrix.Translation = _behavior.RemoteControl.CubeGrid.WorldAABB.Center;

                            if (actions.SafeZonePositionTerrainSurface && PlanetManager.InGravity(matrix.Translation))
                            {

                                var planet = PlanetManager.GetNearestPlanet(matrix.Translation);

                                if (planet != null)
                                    matrix.Translation = planet.SurfaceCoordsAtPosition(matrix.Translation);

                            }

                            SafeZoneManager.CreateSafeZone(matrix, profile, "MES-NPC SafeZone - " + _behavior.RemoteControl.EntityId, actions.LinkSafeZoneToRemoteControl ? _behavior.RemoteControl : null);

                        }

                    }

                }

                lastAction = "AddTagsToPlayers";
                if (actions.AddTagsToPlayers && _behavior.RemoteControl != null)
                {
                    bool SavedPlayerIdentityAlreadyIncluded = false;
                    foreach (var player in PlayerManager.Players)
                    {
                        var AddTagsPlayerConditionIds = IdsReplacer.ReplaceIds(_behavior?.CurrentGrid?.Npc ?? null, actions.AddTagsPlayerConditionIds);

                        if (PlayerCondition.ArePlayerConditionsMet(AddTagsPlayerConditionIds, player.Player.IdentityId, actions.AddTagsOverridePositionInPlayerCondition, _behavior.RemoteControl.GetPosition()))
                        {
                            if ((command?.PlayerIdentity ?? 0) != 0 && (command?.PlayerIdentity ?? 0) == player.Player.IdentityId)
                                SavedPlayerIdentityAlreadyIncluded = true;

                            foreach (var addtag in actions.AddTags)
                            {
                                var tag = IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, addtag);

                                if (player.ProgressionData.Tags.Contains(tag))
                                    continue;
                                player.ProgressionData.Tags.Add(tag);
                            }

                        }

                    }

                    lastAction = "AddTagsIncludeSavedPlayerIdentity";
                    if (actions.AddTagsIncludeSavedPlayerIdentity && !SavedPlayerIdentityAlreadyIncluded && command != null)
                    {
                        var playerid = command?.PlayerIdentity ?? 0;
                        var player = PlayerManager.GetPlayerWithIdentityId(playerid);

                        if (player != null)
                        {
                            foreach (var addtag in actions.AddTags)
                            {
                                var tag = IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, addtag);

                                if (player.ProgressionData.Tags.Contains(tag))
                                    continue;
                                player.ProgressionData.Tags.Add(tag);
                            }
                        }
                    }
                }

                lastAction = "RemoveTagsFromPlayers";
                if (actions.RemoveTagsFromPlayers && _behavior.RemoteControl != null)
                {
                    bool SavedPlayerIdentityAlreadyIncluded = false;
                    foreach (var player in PlayerManager.Players)
                    {
                        var RemoveTagsPlayerConditionIds = IdsReplacer.ReplaceIds(_behavior?.CurrentGrid?.Npc ?? null, actions.RemoveTagsPlayerConditionIds);

                        if (PlayerCondition.ArePlayerConditionsMet(RemoveTagsPlayerConditionIds, player.Player.IdentityId, actions.RemoveTagsOverridePositioninPlayerCondition, _behavior.RemoteControl.GetPosition()))
                        {
                            if ((command?.PlayerIdentity ?? 0) != 0 && (command?.PlayerIdentity ?? 0) == player.Player.IdentityId)
                                SavedPlayerIdentityAlreadyIncluded = true;

                            foreach (var removetag in actions.RemoveTags)
                            {
                                var tag = IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, removetag);


                                if (!player.ProgressionData.Tags.Contains(tag))
                                    continue;

                                player.ProgressionData.Tags.Remove(tag);
                            }

                        }

                    }

                    lastAction = "RemoveTagsIncludeSavedPlayerIdentity";
                    if (actions.RemoveTagsIncludeSavedPlayerIdentity && !SavedPlayerIdentityAlreadyIncluded && command != null)
                    {
                        var playerid = command.PlayerIdentity;
                        var player = PlayerManager.GetPlayerWithIdentityId(playerid);

                        if (player != null)
                        {
                            foreach (var removetag in actions.RemoveTags)
                            {
                                var tag = IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, removetag);

                                if (!player.ProgressionData.Tags.Contains(tag))
                                    continue;

                                player.ProgressionData.Tags.Remove(tag);
                            }
                        }
                    }
                }


                lastAction = "TeleportPlayers";
                if (actions.TeleportPlayers && _behavior.RemoteControl != null)
                {
                    bool SavedPlayerIdentityAlreadyIncluded = false;
                    foreach (var player in PlayerManager.Players)
                    {
                        var TeleportPlayersConditionIds = IdsReplacer.ReplaceIds(_behavior?.CurrentGrid?.Npc ?? null, actions.TeleportPlayerConditionIds);

                        if (PlayerCondition.ArePlayerConditionsMet(TeleportPlayersConditionIds, player.Player.IdentityId, actions.TeleportPlayerOverridePositionInPlayerCondition, _behavior.RemoteControl.GetPosition()))
                        {
                            if ((command?.PlayerIdentity ?? 0) != 0 && (command?.PlayerIdentity ?? 0) == player.Player.IdentityId)
                                SavedPlayerIdentityAlreadyIncluded = true;

                            player.Player.Character.Teleport(MatrixD.CreateWorld(actions.TeleportPlayerCoords));
                        }

                    }

                    if (actions.AddTagsIncludeSavedPlayerIdentity && !SavedPlayerIdentityAlreadyIncluded && command != null)
                    {
                        var playerid = command?.PlayerIdentity ?? 0;
                        var player = PlayerManager.GetPlayerWithIdentityId(playerid);

                        if (player != null)
                        {
                            player.Player.Character.Teleport(MatrixD.CreateWorld(actions.TeleportPlayerCoords));
                        }
                    }
                }


                lastAction = "AddGPSToPlayers";
                if (actions.AddGPSToPlayers && _behavior.RemoteControl != null)
                {

                    var defaultDescription = "No description available";
                    var defaultColor = new Color(255, 178, 96); //  color as default

                    foreach (var player in PlayerManager.Players)
                    {
                        if (PlayerCondition.ArePlayerConditionsMet(actions.AddGPSPlayerConditionIds, player.Player.IdentityId, actions.AddGPSPlayerOverridePositionInPlayerCondition, _behavior.RemoteControl.GetPosition()))
                        {
                            for (int i = 0; i < actions.GPSNames.Count; i++)
                            {

                                // If GPSDescriptions is empty or index is out of range, use the default description
                                var description = defaultDescription;


                                // If GPSColors is empty or index is out of range, use the default color
                                var color = defaultColor;


                                if (actions.UseGPSObjective)
                                {
                                    MyVisualScriptLogicProvider.AddGPSObjective(actions.GPSNames[i], description, actions.GPSVector3Ds[i], color, 0, player.Player.IdentityId);
                                }
                                else
                                {
                                    MyVisualScriptLogicProvider.AddGPS(actions.GPSNames[i], description, actions.GPSVector3Ds[i], color, 0, player.Player.IdentityId);

                                    // To do: fix issue where GPS is sometimes not being added.  We can try doing something manual, but why would you want to do that work when the whole gps system sucks. -CPT

                                    //var gps = MyAPIGateway.Session.GPS.Create(actions.GPSNames[i], description, actions.GPSVector3Ds[i], true);
                                    //gps.GPSColor = color;
                                    //MyAPIGateway.Session.GPS.AddGps(player.Player.IdentityId, gps);

                                }
                            }
                        }
                    }
                }

                lastAction = "ResetCooldownTimeOfEvents";
                if (actions.ResetCooldownTimeOfEvents)
                {

                    EventActionProfile.ResetCooldownTimeOfEvents(IdsReplacer.ReplaceIds(_behavior?.CurrentGrid?.Npc ?? null, actions.ResetEventCooldownIds), IdsReplacer.ReplaceIds(_behavior?.CurrentGrid?.Npc ?? null, actions.ResetEventCooldownTags));


                }

                lastAction = "ToggleEvents";
                if (actions.ToggleEvents)
                {
                    EventActionProfile.ToggleEvents(IdsReplacer.ReplaceIds(_behavior?.CurrentGrid?.Npc ?? null, actions.ToggleEventIds), actions.ToggleEventIdModes, IdsReplacer.ReplaceIds(_behavior?.CurrentGrid?.Npc ?? null, actions.ToggleEventTags), actions.ToggleEventTagModes);
                }


                lastAction = "ResetThisStaticEncounter";
                if (actions.ResetThisStaticEncounter)
                {

                    var spawngroupname = _behavior?.CurrentGrid?.Npc.SpawnGroupName;

                    if (spawngroupname != null)
                        NpcManager.ResetThisResetThisStaticEncounter(spawngroupname);

                }


                lastAction = "SaveLocationToSandboxVariable";
                if (actions.SaveLocationToSandboxVariable)
                {
                    MyAPIGateway.Utilities.SetVariable(IdsReplacer.ReplaceId(_behavior?.CurrentGrid?.Npc ?? null, actions.LocationSandboxVariableName), RemoteControl.GetPosition());
                }




                //SetBooleansTrue
                lastAction = "SetBooleansTrue";
                foreach (var variable in actions.SetBooleansTrue)
                    _settings.SetCustomBool(variable, true);

                //SetBooleansFalse
                lastAction = "SetBooleansFalse";
                foreach (var variable in actions.SetBooleansFalse)
                    _settings.SetCustomBool(variable, false);



                // IncreaseCounters
                lastAction = "IncreaseCounters";
                var customCountersVariables = _behavior?.CurrentGrid?.Npc?.CustomCountersVariables;
                int increaseAmount = 0;
                if (actions.IncreaseCountersUseAmountVariable && customCountersVariables != null)
                {
                    foreach (var counterVar in customCountersVariables)
                    {
                        if (actions.IncreaseCountersAmountVariable == "{" + counterVar.Key + "}")
                        {
                            increaseAmount = actions.IncreaseCountersUseCommandScore && command != null ? command.NPCScoreValue : Math.Abs(counterVar.Value);
                            break;
                        }
                    }
                }
                else
                {
                    increaseAmount = actions.IncreaseCountersUseCommandScore && command != null ? command.NPCScoreValue : Math.Abs(actions.IncreaseCountersAmount);
                }

                foreach (var variable in actions.IncreaseCounters)
                {
                    _settings.SetCustomCounter(variable, increaseAmount);
                }

                // DecreaseCounters
                lastAction = "DecreaseCounters";
                int decreaseAmount = 0;
                if (actions.DecreaseCountersUseAmountVariable && customCountersVariables != null)
                {
                    foreach (var counterVar in customCountersVariables)
                    {
                        if (actions.DecreaseCountersAmountVariable == "{" + counterVar.Key + "}")
                        {
                            decreaseAmount = actions.DecreaseCountersUseCommandScore && command != null ? -command.NPCScoreValue : -Math.Abs(counterVar.Value);
                            break;
                        }
                    }
                }
                else
                {
                    decreaseAmount = actions.DecreaseCountersUseCommandScore && command != null ? -command.NPCScoreValue : -Math.Abs(actions.DecreaseCountersAmount);
                }

                foreach (var variable in actions.DecreaseCounters)
                {
                    _settings.SetCustomCounter(variable, decreaseAmount);
                }



                //ResetCounters
                lastAction = "ResetCounters";
                foreach (var variable in actions.ResetCounters)
                    _settings.SetCustomCounter(variable, 0, true);

                //SetCounters
                lastAction = "SetCounters";
                if (actions.SetCounters.Count == actions.SetCountersValues.Count)
                {

                    for (int i = 0; i < actions.SetCounters.Count; i++)
                        _settings.SetCustomCounter(actions.SetCounters[i], actions.SetCountersValues[i], false, true);

                }
                else if (actions.SetCounters.Count > 0 && actions.SetCountersUseAmountVariable && customCountersVariables != null)
                {
                    foreach (var counterVar in customCountersVariables)
                    {
                        if (actions.SetCountersAmountVariable == "{" + counterVar.Key + "}")
                        {
                            for (int i = 0; i < actions.SetCounters.Count; i++)
                                _settings.SetCustomCounter(actions.SetCounters[i], counterVar.Value, false, true);
                            break;
                        }
                    }
                }

                //SetSandboxBooleansTrue
                lastAction = "SetSandboxBooleansTrue";
                foreach (var variable in actions.SetSandboxBooleansTrue)
                    SetSandboxBool(variable, true);

                //SetSandboxBooleansFalse
                lastAction = "SetSandboxBooleansFalse";
                foreach (var variable in actions.SetSandboxBooleansFalse)
                    SetSandboxBool(variable, false);

                //IncreaseSandboxCounters
                lastAction = "IncreaseSandboxCounters";
                foreach (var variable in actions.IncreaseSandboxCounters)
                    SetSandboxCounter(variable, Math.Abs(actions.IncreaseSandboxCountersAmount));

                //DecreaseSandboxCounters
                lastAction = "DecreaseSandboxCounters";
                foreach (var variable in actions.DecreaseSandboxCounters)
                    SetSandboxCounter(variable, -Math.Abs(actions.DecreaseSandboxCountersAmount));

                //ResetSandboxCounters
                lastAction = "ResetSandboxCounters";
                foreach (var variable in actions.ResetSandboxCounters)
                    SetSandboxCounter(variable, 0);

                //SetSandboxCounters
                lastAction = "SetSandboxCounters";
                if (actions.SetSandboxCounters.Count != 0 && actions.SetSandboxCounters.Count == actions.SetSandboxCountersValues.Count)
                {

                    for (int i = 0; i < actions.SetSandboxCounters.Count; i++)
                        SetSandboxCounter(actions.SetSandboxCounters[i], actions.SetSandboxCountersValues[i], true);

                }

                //SaveSavePlayerIdentity
                lastAction = "SavePlayerIdentity";
                if (actions.SavePlayerIdentity)
                    _behavior.BehaviorSettings.SavedPlayerIdentityId = command?.PlayerIdentity ?? 0;

                //RemovePlayerIdentity
                lastAction = "RemovePlayerIdentity";
                if (actions.RemovePlayerIdentity)
                    _behavior.BehaviorSettings.SavedPlayerIdentityId = 0;

                //BehaviorSpecificEventA
                lastAction = "BehaviorSpecificEventA";
                if (actions.BehaviorSpecificEventA)
                    _behavior.BehaviorActionA = true;

                //BehaviorSpecificEventB
                lastAction = "BehaviorSpecificEventB";
                if (actions.BehaviorSpecificEventB)
                    _behavior.BehaviorActionB = true;

                //BehaviorSpecificEventC
                lastAction = "BehaviorSpecificEventC";
                if (actions.BehaviorSpecificEventC)
                    _behavior.BehaviorActionC = true;

                //BehaviorSpecificEventD
                lastAction = "BehaviorSpecificEventD";
                if (actions.BehaviorSpecificEventD)
                    _behavior.BehaviorActionD = true;

                //BehaviorSpecificEventE
                lastAction = "BehaviorSpecificEventE";
                if (actions.BehaviorSpecificEventE)
                    _behavior.BehaviorActionE = true;

                //BehaviorSpecificEventF
                lastAction = "BehaviorSpecificEventF";
                if (actions.BehaviorSpecificEventF)
                    _behavior.BehaviorActionF = true;

                //BehaviorSpecificEventG
                lastAction = "BehaviorSpecificEventG";
                if (actions.BehaviorSpecificEventG)
                    _behavior.BehaviorActionG = true;

                //BehaviorSpecificEventH
                lastAction = "BehaviorSpecificEventH";
                if (actions.BehaviorSpecificEventH)
                    _behavior.BehaviorActionH = true;
            }
            catch (Exception e)
            {
                BehaviorLogger.Write($"{actions.ProfileSubtypeId}: Error while executing action: '{lastAction}'", BehaviorDebugEnum.Error, true);
                throw (e);
            }
        }

    }
}
