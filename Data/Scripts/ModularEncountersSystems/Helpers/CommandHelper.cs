using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Tasks;
using Sandbox.ModAPI;
using System;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Helpers {

    public enum CommandType {

        DroneAntenna,
        PlayerChat,

    }

    public enum CommandTransmissionType {

        None,
        Greeting,
        Warning,
        Taunt,
        Attack,
        Relent,
        Retreat,

    }

    public class Command {

        public string CommandCode;

        public CommandType Type;

        public IMyEntity RemoteControl;

        public IMyEntity Character;

        public IMyEntity SenderEntity { get { return (RemoteControl != null ? RemoteControl : Character); } }

        public double Radius;

        public bool IgnoreAntennaRequirement;

        public bool IgnoreReceiverAntennaRequirement;

        public long TargetEntityId;

        public IMyEntity TargetEntity;

        public float DamageAmount;

        public Vector3D Position;

        public long PlayerIdentity;

        public bool UseTriggerTargetDistance;

        public long DamagerEntityId;

        public bool SingleRecipient;

        public bool ReturnToSender;

        public long Recipient;

        public EncounterWaypoint Waypoint;

        public bool MatchSenderReceiverOwners;

        public bool CheckRelationSenderReceiver;
        public RelationTypeEnum Relation;


        public long CommandOwnerId;

        public float GridValueScore;

        public CommandTransmissionType TransmissionType;

        public IBehavior Behavior;

        public bool RequestEscortSlot;

        public int DelayTicks;

        public bool FromEvent;

        public Command() {

            Defaults();

        }

        public static Command PlayerRelatedCommand(long playerId) {

            var command = new Command();
            command.PlayerIdentity = playerId;
            return command;

        }


        public void Defaults() {

            CommandCode = "";
            Type = CommandType.DroneAntenna;
            RemoteControl = null;
            Character = null;
            Radius = 0;
            IgnoreAntennaRequirement = false;
            IgnoreReceiverAntennaRequirement = false;
            TargetEntityId = 0;
            TargetEntity = null;
            Position = Vector3D.Zero;
            PlayerIdentity = 0;
            UseTriggerTargetDistance = false;
            SingleRecipient = false;
            ReturnToSender = false;
            Recipient = 0;
            Waypoint = null;
            MatchSenderReceiverOwners = false;
            CommandOwnerId = 0;
            GridValueScore = 0;
            TransmissionType = CommandTransmissionType.None;
            Behavior = null;
            RequestEscortSlot = false;
            DelayTicks = 0;
            FromEvent = false;

            CheckRelationSenderReceiver = false;
            Relation = RelationTypeEnum.None;

        }
        public void PrepareEventCommand(CommandProfile profile, Vector3D position)
        {

            this.FromEvent = true;
            this.DelayTicks = profile.CommandDelayTicks;
            this.SingleRecipient = profile.SingleRecipient;



            this.IgnoreAntennaRequirement = true;
            this.MatchSenderReceiverOwners = false;
            this.IgnoreReceiverAntennaRequirement = profile.IgnoreReceiverAntennaRequirement;



            this.CommandCode = profile.CommandCode;

            this.Position = position;


            this.Radius = profile.Radius;

            this.CheckRelationSenderReceiver = false;



        }



        public void PrepareCommand(IBehavior behavior, CommandProfile profile, ActionReferenceProfile action, Command receivedCommand, long attackerId, long detectedId) {

            this.Behavior = behavior;
            this.DelayTicks = profile.CommandDelayTicks;
            this.SingleRecipient = profile.SingleRecipient;
            this.IgnoreAntennaRequirement = profile.IgnoreAntennaRequirement;
            this.IgnoreReceiverAntennaRequirement = profile.IgnoreReceiverAntennaRequirement;
            this.MatchSenderReceiverOwners = profile.MatchSenderReceiverOwners;
            this.CheckRelationSenderReceiver = profile.CheckRelationSenderReceiver;
            this.Relation = profile.Relation;




            RemoteControl = behavior.RemoteControl;
            CommandOwnerId = behavior.RemoteControl.OwnerId;
            this.RequestEscortSlot = profile.RequestEscortSlot;
            double sendRadius = 0;

            if (profile.CommandCode.Contains("{FactionTag}")){
                this.CommandCode = profile.CommandCode.Replace("{FactionTag}", behavior.RemoteControl.GetOwnerFactionTag());
            }
            else
            {
                this.CommandCode = profile.CommandCode;
            }
            





            if (profile.IgnoreAntennaRequirement) {

                sendRadius = profile.Radius;
                this.IgnoreAntennaRequirement = true;

            } else {

                var antenna = behavior.Grid.GetAntennaWithHighestRange();

                if (antenna != null)
                    sendRadius = antenna.Radius;

            }

            if (profile.MaxRadius > -1 && sendRadius > profile.MaxRadius)
                sendRadius = profile.MaxRadius;

            this.Radius = sendRadius;

            if (profile.SendTargetEntityId)

                if (behavior.AutoPilot.Targeting.HasTarget())
                    this.TargetEntityId = behavior.AutoPilot.Targeting.Target.GetEntityId();
                else
                    BehaviorLogger.Write("No Current Target To Send With Command", BehaviorDebugEnum.Command);

            if (profile.SendSelfAsTargetEntityId)
                this.TargetEntityId = behavior.RemoteControl.EntityId;

            if (profile.SendDamagerEntityId)

                if (behavior.BehaviorSettings.LastDamagerEntity == 0)
                    this.DamagerEntityId = behavior.BehaviorSettings.LastDamagerEntity;
                else
                    BehaviorLogger.Write("No Damager ID To Send With Command", BehaviorDebugEnum.Command);

            if (profile.SendGridValue) {

                GridValueScore = behavior.CurrentGrid?.TargetValue() ?? 0;

            }

            TransmissionType = profile.TransmissionType;

            if (receivedCommand != null) {

                if (profile.ReturnToSender) {

                    this.SingleRecipient = true;
                    this.Recipient = receivedCommand.RemoteControl.EntityId;

                }

            }

            if (profile.SendWaypoint) {

                WaypointProfile waypointProfile = null;

                if (ProfileManager.WaypointProfiles.TryGetValue(profile.Waypoint, out waypointProfile)) {

                    if ((int)waypointProfile.Waypoint > 2) {

                        BehaviorLogger.Write(action.ProfileSubtypeId + ": Creating an Entity/Relative Waypoint", BehaviorDebugEnum.Command);

                        if (waypointProfile.RelativeEntity == RelativeEntityType.Self)
                            this.Waypoint = waypointProfile.GenerateEncounterWaypoint(RemoteControl);

                        if (waypointProfile.RelativeEntity == RelativeEntityType.Target && behavior.AutoPilot.Targeting.HasTarget())
                            this.Waypoint = waypointProfile.GenerateEncounterWaypoint(behavior.AutoPilot.Targeting.Target.GetEntity());
                        else
                            BehaviorLogger.Write("No Current Target To Send As Target Relative Waypoint", BehaviorDebugEnum.Command);

                        if (waypointProfile.RelativeEntity == RelativeEntityType.Damager) {

                            IMyEntity entity = null;

                            if (MyAPIGateway.Entities.TryGetEntityById(behavior.BehaviorSettings.LastDamagerEntity, out entity)) {

                                var parentEnt = entity.GetTopMostParent();

                                if (parentEnt != null) {

                                    if (parentEnt as IMyCubeGrid != null) {

                                        //BehaviorLogger.Write("Damager Parent Entity Valid", BehaviorDebugEnum.General);
                                        var gridGroup = MyAPIGateway.GridGroups.GetGroup(behavior.RemoteControl.SlimBlock.CubeGrid, GridLinkTypeEnum.Physical);
                                        bool isSameGridConstrust = false;

                                        foreach (var grid in gridGroup) {

                                            if (grid.EntityId == parentEnt.EntityId) {

                                                //BehaviorLogger.Write("Damager Parent Entity Was Same Grid", BehaviorDebugEnum.General);
                                                isSameGridConstrust = true;
                                                break;

                                            }

                                        }

                                        if (!isSameGridConstrust) {

                                            this.Waypoint = waypointProfile.GenerateEncounterWaypoint(parentEnt);

                                        }

                                    } else {

                                        var potentialPlayer = PlayerManager.GetPlayerUsingTool(entity);

                                        if (potentialPlayer != null) {

                                            this.Waypoint = waypointProfile.GenerateEncounterWaypoint(parentEnt);

                                        }

                                    }

                                }

                            }

                        }

                    } else {

                        this.Waypoint = waypointProfile.GenerateEncounterWaypoint(RemoteControl);

                    }


                }

            }


        }

    }

    public static class CommandHelper {

        public static Action<Command> CommandTrigger;

        public static void SendCommand(Command command, bool sendNow = false) {

            if (command.DelayTicks == 0 || sendNow)
                CommandTrigger?.Invoke(command);
            else
                TaskProcessor.Tasks.Add(new DelayedCommand(command));
        
        }

    }

}
