using System;
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
    public class EventAction{
        public string ProfileSubtypeId;
        public bool ChangeBooleans;
        public List<string> SetSandboxBooleansTrue;
        public List<string> SetSandboxBooleansFalse;

        public bool ChangeCounters;
        public List<string> IncreaseSandboxCounters;
        public List<string> DecreaseSandboxCounters;
        public int IncreaseSandboxCountersAmount;
        public int DecreaseSandboxCountersAmount;

        public bool UseChatBroadcast;
        public List<ChatProfile> ChatData;
        public string DebugHudMessage;

        public EventAction()
        {
            ProfileSubtypeId = "";
            ChangeBooleans = false;
			SetSandboxBooleansTrue = new List<string>();
			SetSandboxBooleansFalse = new List<string>();

			ChangeCounters = false;
			IncreaseSandboxCounters = new List<string>();
			DecreaseSandboxCounters = new List<string>();
            IncreaseSandboxCountersAmount = 1;

            UseChatBroadcast = false;
            ChatData = new List<ChatProfile>();


            DebugHudMessage = "";
        }

        public void InitTags(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return;

            var descSplit = data.Split('\n');

            foreach (var tagRaw in descSplit)
            {

                var tag = tagRaw.Trim();

                //ChangeBooleans
                if (tag.StartsWith("[ChangeBooleans:") == true)
                {

                    TagParse.TagBoolCheck(tag, ref this.ChangeBooleans);
                }

                //SetSandboxBooleansTrue
                if (tag.StartsWith("[SetSandboxBooleansTrue:") == true)
                {

                    TagParse.TagStringListCheck(tag, ref this.SetSandboxBooleansTrue);
                }

                //SetSandboxBooleansFalse
                if (tag.StartsWith("[SetSandboxBooleansFalse:") == true)
                {

                    TagParse.TagStringListCheck(tag, ref this.SetSandboxBooleansFalse);
                }

                //ChangeCounters
                if (tag.StartsWith("[ChangeCounters:") == true)
                {

                    TagParse.TagBoolCheck(tag, ref this.ChangeCounters);
                }

                //IncreaseSandboxCounters
                if (tag.StartsWith("[IncreaseSandboxCounters:") == true)
                {

                    TagParse.TagStringListCheck(tag, ref this.IncreaseSandboxCounters);
                }

                //DecreaseSandboxCounters
                if (tag.StartsWith("[DecreaseSandboxCounters:") == true)
                {

                    TagParse.TagStringListCheck(tag, ref this.DecreaseSandboxCounters);
                }


                //IncreaseSandboxCountersAmount
                if (tag.StartsWith("[IncreaseSandboxCountersAmount:") == true)
                {

                    TagParse.TagIntCheck(tag, ref this.IncreaseSandboxCountersAmount);
                }

                //DecreaseSandboxCounters
                if (tag.StartsWith("[DecreaseSandboxCountersAmount:") == true)
                {

                    TagParse.TagIntCheck(tag, ref this.DecreaseSandboxCountersAmount);
                }



                //UseChatBroadcast
                if (tag.StartsWith("[UseChatBroadcast:") == true)
                {

                    TagParse.TagBoolCheck(tag, ref this.UseChatBroadcast);
                }


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




                //DebugHudMessage
                if (tag.StartsWith("[DebugHudMessage:") == true)
                {

                    TagParse.TagStringCheck(tag, ref this.DebugHudMessage);
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



            public static void ExecuteAction(EventAction ActionProfile)
            {
                var EventBroadcastSystem = new EventBroadcastSystem();
                //Booleans
                if (ActionProfile.ChangeBooleans == true)
                {
                    for (int i = 0; i < ActionProfile.SetSandboxBooleansTrue.Count; i++)
                    {
                        MyAPIGateway.Utilities.SetVariable<bool>(ActionProfile.SetSandboxBooleansTrue[i], true);

                    }
                    for (int i = 0; i < ActionProfile.SetSandboxBooleansFalse.Count; i++)
                    {
                        MyAPIGateway.Utilities.SetVariable<bool>(ActionProfile.SetSandboxBooleansFalse[i], false);
                    }

                }

                //Counter
                if (ActionProfile.ChangeCounters)
                {
                    for (int i = 0; i < ActionProfile.IncreaseSandboxCounters.Count; i++)
                    {
                        MyAPIGateway.Utilities.SetVariable<int>(ActionProfile.IncreaseSandboxCounters[i], ActionProfile.IncreaseSandboxCountersAmount);

                    }
                    for (int i = 0; i < ActionProfile.SetSandboxBooleansFalse.Count; i++)
                    {
                        MyAPIGateway.Utilities.SetVariable<int>(ActionProfile.DecreaseSandboxCounters[i], ActionProfile.DecreaseSandboxCountersAmount);
                    }

                }


                //ChatBroadcast
                if (ActionProfile.UseChatBroadcast == true)
                {
                    foreach (var chatData in ActionProfile.ChatData)
                    {
                        EventBroadcastSystem.BroadcastRequest(chatData);
                    }
                }

                MyVisualScriptLogicProvider.ShowNotificationToAll(ActionProfile.DebugHudMessage, 3000);
        }


    }
}



