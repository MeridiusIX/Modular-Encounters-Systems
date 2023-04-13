using System;
using ProtoBuf;
using System.Collections.Generic;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Behavior.Subsystems;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;


namespace ModularEncountersSystems.Events
{
    [ProtoContract]
    public class EventActionProfile{

        [ProtoMember(1)]
        public ChatProfile ChatDataDefunct;

        [ProtoMember(2)]
        public SpawnProfile SpawnerDefunct;

        [ProtoMember(3)]
        public string ProfileSubtypeId;

        [ProtoMember(4)]
        public List<ChatProfile> ChatData;

        [ProtoMember(5)]
        public List<SpawnProfile> Spawner;

        //[ProtoIgnore]
        public EventActionReferenceProfile ActionReference
        {

            get
            {

                if (_actionReference == null)
                {

                    ProfileManager.EventActionReferenceProfiles.TryGetValue(ProfileSubtypeId, out _actionReference);

                }

                return _actionReference;

            }

        }

        //[ProtoIgnore]
        private EventActionReferenceProfile _actionReference;

        public EventActionProfile()
        {

            ChatData = new List<ChatProfile>();
            ChatDataDefunct = new ChatProfile();

            Spawner = new List<SpawnProfile>();
            SpawnerDefunct = new SpawnProfile();

            ProfileSubtypeId = "";

        }





        public void InitTags(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return;

            var descSplit = data.Split('\n');

            foreach (var tagRaw in descSplit)
            {

                var tag = tagRaw.Trim();


                //ChatData
                if (tag.StartsWith("[ChatData:") == true)
                {

                    string tempValue = "";
                    TagParse.TagStringCheck(tag, ref tempValue);
                    bool gotChat = false;

                    if (string.IsNullOrWhiteSpace(tempValue) == false)
                    {

                        byte[] byteData = { };

                        if (ProfileManager.ChatObjectTemplates.TryGetValue(tempValue, out byteData) == true)
                        {

                            try
                            {

                                var profile = MyAPIGateway.Utilities.SerializeFromBinary<ChatProfile>(byteData);

                                if (profile != null)
                                {

                                    ChatData.Add(profile);
                                    gotChat = true;

                                }
                                else
                                {

                                    //BehaviorLogger.Write("Deserialized Chat Profile was Null", BehaviorDebugEnum.Error, true);

                                }

                            }
                            catch (Exception e)
                            {

                                //BehaviorLogger.Write("Caught Exception While Attaching to Action Profile:", BehaviorDebugEnum.Error, true);
                               // BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Error, true);

                            }

                        }
                        else
                        {

                          //  ProfileManager.ReportProfileError(tempValue, "Chat Profile Not Registered in Profile Manager");

                        }

                    }

                    if (!gotChat)
                        ProfileManager.ReportProfileError(tempValue, "Provided Chat Profile Could Not Be Loaded in Trigger: " + ProfileSubtypeId);

                }





                
            }

        }

        public static void ExecuteActions(Event Event)
        {
            var Actions = Event.Actions;
            if (Event.ActionExecution == EventActionExecutionEnum.AtOnce)
            {
                for (int i = 0; i < Actions.Count; i++)
                {
                    ExecuteAction(Actions[i]);
                }
            }

            if (Event.ActionExecution == EventActionExecutionEnum.SingleRandom)
            {
                var tja = MathTools.RandomBetween(0, (Actions.Count-1));
                ExecuteAction(Actions[tja]);
            }

            if (Event.ActionExecution == EventActionExecutionEnum.Sequential)
            {

                for (int i = 0; i < (Actions.Count ); i++)
                {
                    var EventTime = new EventTime();
                    EventTime.Event = Event;
                    EventTime.StartDate = MyAPIGateway.Session.GameDateTime;
                    EventTime.Timeinms = (i * Event.TimeTillNextAction);
                    EventTime.Type = CheckType.ExecuteAction;
                    EventTime.ActionIndex = i;
                    EventManager.EventTimes.Add(EventTime);
                }
            }
        }



            public static void ExecuteAction(EventActionProfile actionsBase)
            {
                var actions = actionsBase.ActionReference;
                var EventBroadcastSystem = new EventBroadcastSystem();
                //Booleans
                if (actions.ChangeBooleans == true)
                {
                    for (int i = 0; i < actions.SetSandboxBooleansTrue.Count; i++)
                    {
                        MyAPIGateway.Utilities.SetVariable<bool>(actions.SetSandboxBooleansTrue[i], true);

                    }
                    for (int i = 0; i < actions.SetSandboxBooleansFalse.Count; i++)
                    {
                        MyAPIGateway.Utilities.SetVariable<bool>(actions.SetSandboxBooleansFalse[i], false);
                    }

                }

                //Counter
                if (actions.ChangeCounters)
                {
                    for (int i = 0; i < actions.IncreaseSandboxCounters.Count; i++)
                    {
                        MyAPIGateway.Utilities.SetVariable<int>(actions.IncreaseSandboxCounters[i], actions.IncreaseSandboxCountersAmount);

                    }
                    for (int i = 0; i < actions.SetSandboxBooleansFalse.Count; i++)
                    {
                        MyAPIGateway.Utilities.SetVariable<int>(actions.DecreaseSandboxCounters[i], actions.DecreaseSandboxCountersAmount);
                    }

                }


                //ChatBroadcast
                if (actions.UseChatBroadcast == true)
                {
                    foreach (var chatData in actionsBase.ChatData)
                    {
                        EventBroadcastSystem.BroadcastRequest(chatData);
                    }
                }

                MyVisualScriptLogicProvider.ShowNotificationToAll(actions.DebugHudMessage, 3000);
        }


    }
}



