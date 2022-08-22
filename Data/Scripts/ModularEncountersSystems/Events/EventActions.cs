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




                //DecreaseSandboxCounters
                if (tag.StartsWith("[DebugHudMessage:") == true)
                {

                    TagParse.TagStringCheck(tag, ref this.DebugHudMessage);
                }



                
            }

        }

        public static void ExecuteActions(Event Event, List<EventAction> Profiles)
        {
            if (Event.ActionExecution == EventActionExecutionEnum.AtOnce)
            {
                for (int i = 0; i < Profiles.Count; i++)
                {
                    ExecuteAction(Profiles[i]);
                }
            }

            if (Event.ActionExecution == EventActionExecutionEnum.SingleRandom)
            {
                var tja = MathTools.RandomBetween(0, (Profiles.Count-1));
                ExecuteAction(Profiles[tja]);
            }

            if (Event.ActionExecution == EventActionExecutionEnum.Sequential)
            {
                if (Profiles.Count <= 0)
                    return;

                if (Event.AtAction >= Profiles.Count)
                    Event.AtAction = 0;

                ExecuteAction(Profiles[Event.AtAction]);
                Event.AtAction++;

                var EventTime = new EventTime();
                EventTime.Event = Event;
                EventTime.StartDate = MyAPIGateway.Session.GameDateTime;
                EventTime.Timeinms = Event.TimeTillNextAction;
                EventTime.Type = CheckType.ExecuteAction;
                EventManager.EventTimes.Add(EventTime);





            }

        }



            public static void ExecuteAction(EventAction Profile)
            {
            //Booleans
            if (Profile.ChangeBooleans == true)
            {
                for (int i = 0; i < Profile.SetSandboxBooleansTrue.Count; i++)
                {
                    MyAPIGateway.Utilities.SetVariable<bool>(Profile.SetSandboxBooleansTrue[i], true);

                }
                for (int i = 0; i < Profile.SetSandboxBooleansFalse.Count; i++)
                {
                    MyAPIGateway.Utilities.SetVariable<bool>(Profile.SetSandboxBooleansFalse[i], false);
                }

            }

            //Counter
            if (Profile.ChangeCounters)
            {
                for (int i = 0; i < Profile.IncreaseSandboxCounters.Count; i++)
                {
                    MyAPIGateway.Utilities.SetVariable<int>(Profile.IncreaseSandboxCounters[i], Profile.IncreaseSandboxCountersAmount);

                }
                for (int i = 0; i < Profile.SetSandboxBooleansFalse.Count; i++)
                {
                    MyAPIGateway.Utilities.SetVariable<int>(Profile.DecreaseSandboxCounters[i], Profile.DecreaseSandboxCountersAmount);
                }
            }


            //ChatBroadcast
            if (Profile.UseChatBroadcast == true)
            {

                foreach (var chatData in Profile.ChatData)
                {

                    //BehaviorLogger.Write(actions.ProfileSubtypeId + ": Attempting Chat Broadcast", BehaviorDebugEnum.Action);

                    BroadcastSystem.


                    .BroadcastRequest(chatData);

                }

            }


            MyVisualScriptLogicProvider.ShowNotificationToAll(Profile.DebugHudMessage, 3000);
        }


    }
}



