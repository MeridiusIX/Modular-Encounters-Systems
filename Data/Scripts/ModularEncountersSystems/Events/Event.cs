﻿using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Helpers;
using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;
using Sandbox.ModAPI;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ProtoBuf;
using ModularEncountersSystems.Events.Action;
using ModularEncountersSystems.Events.Condition;

namespace ModularEncountersSystems.Events
{

    [ProtoContract]
    public class Event
    {
        //SAVE
        [ProtoMember(1)] public string ProfileSubtypeId;
        [ProtoMember(2)] public bool Ready;
        [ProtoMember(3)] public int RunCount;
        [ProtoMember(4)] public int CurrentActionIndex;
        [ProtoMember(5)] public int CooldownTimeTrigger;
        [ProtoMember(6)] public DateTime LastTriggerTime;

        //Previously EventTime Class Fields
        [ProtoMember(8)] public bool ProcessingSequentialActions;
        [ProtoMember(9)] public DateTime ActionStartTime;
        [ProtoMember(10)] public int IncrementMs;
        [ProtoMember(11)] public CheckType CheckingType;
        [ProtoMember(12)] public int ActionIndex;

        [ProtoMember(13)] public List<EventActionProfile> Actions;


        [ProtoMember(14)] public bool EventEnabled;

        [ProtoMember(15)] public bool Insertable;
        [ProtoMember(16)] public List<string> ReplaceKeys;
        [ProtoMember(17)] public List<string> ReplaceValues;
        [ProtoMember(18)] public long InstanceId;
        [ProtoMember(19)] public string TemplateProfileSubtype;
        [ProtoMember(20)] public bool MarkedforRemoval;

        [ProtoIgnore] public bool ErrorOnSetup;
        [ProtoIgnore] public string ErrorSetupMsg;
        [ProtoIgnore] public List<EventCondition> Conditions;
        [ProtoIgnore] public List<EventCondition> PersistantConditions;

        [ProtoIgnore] public int RequiredConditionIndex;

        [ProtoIgnore]
        public EventProfile Profile
        {

            get
            {

                if (_profile == null)
                {

                    if (Insertable)
                    {
                        var customdata = ""; 
                        if (!ProfileManager.TemplateEventProfiles.TryGetValue(TemplateProfileSubtype, out customdata))
                        {
                            ErrorOnSetup = true;
                            ErrorSetupMsg = "Could Not Find EventProfile With Name [" + TemplateProfileSubtype ?? null + "]";
                            return null;
                        }

                        _profile = new EventProfile();
                        _profile.ProfileSubtypeId = ProfileSubtypeId;
                        _profile.InitTags(IdsReplacer.ReplaceText(customdata,ReplaceKeys,ReplaceValues));
                        _profile.Tags.Add(InstanceId.ToString());
                    }
                    else
                    {
                        if (!ProfileManager.EventProfiles.TryGetValue(ProfileSubtypeId, out _profile))
                        {

                            ErrorOnSetup = true;
                            ErrorSetupMsg = "Could Not Find EventProfile With Name [" + ProfileSubtypeId ?? null + "]";
                            return null;
                            
                        }
                    }

                }

                return _profile;

            }

        }

        [ProtoIgnore]
        public bool Valid
        {

            get
            {

                if (ErrorOnSetup || Profile == null)
                    return false;

                return true;

            }

        }

        [ProtoIgnore] private EventProfile _profile;

        public Event()
        {



        }

        //This runs the first time a specific event is created
        public Event(string profileSubtypeId)
        {

            ProfileSubtypeId = profileSubtypeId;
            Insertable = false;
            Init(true);

            if (!Valid)
            {

                return;

            }


             
        }

        public void InitAsInsertable(string profilesubtypeId, long uniqueValue, List<string> _replacekeys, List<string> _replacevalues)
        {
            Insertable = true;
            // ??
            ReplaceKeys = new List<string>(_replacekeys);
            ReplaceValues = new List<string>(_replacevalues);
            InstanceId = uniqueValue;
            TemplateProfileSubtype = profilesubtypeId;
            ProfileSubtypeId = profilesubtypeId + "@" + uniqueValue.ToString();


            Init(true);


        }



        public void Init(bool FirstTime = false)
        {
            if (Profile == null)
            {

                return;

            }


            if (FirstTime)
            {
                EventEnabled = Profile.UseEvent;
                RunCount = 0;
                CurrentActionIndex = 0;
                CooldownTimeTrigger = MathTools.RandomBetween(Profile.MinCooldownMs, Profile.MaxCooldownMs);
                LastTriggerTime = MyAPIGateway.Session.GameDateTime;
                MarkedforRemoval = false;

            }



            Ready = false;
            Conditions = new List<EventCondition>();
            PersistantConditions = new List<EventCondition>();
            Actions = new List<EventActionProfile>();



            foreach (var conditionName in Profile.PersistantConditionIds)
            {

                EventCondition conditionProfile = null;

                if (Insertable)
                {
                    var customdata = "";
                    if (ProfileManager.TemplateEventConditions.TryGetValue(conditionName, out customdata))
                    {
                        conditionProfile = new EventCondition();
                        conditionProfile.ProfileSubtypeId = conditionName + "@" + InstanceId.ToString();
                        conditionProfile.InitTags(IdsReplacer.ReplaceText(customdata, ReplaceKeys, ReplaceValues));
                        PersistantConditions.Add(conditionProfile);
                        continue;

                    }
                }

                if (ProfileManager.EventConditions.TryGetValue(conditionName, out conditionProfile))
                {
                    PersistantConditions.Add(conditionProfile);
                }

            }


            foreach (var conditionName in Profile.ConditionIds)
            {

                EventCondition conditionProfile = null;

                if (Insertable)
                {
                    var customdata = "";
                    if (ProfileManager.TemplateEventConditions.TryGetValue(conditionName, out customdata))
                    {
                        conditionProfile = new EventCondition();
                        conditionProfile.ProfileSubtypeId = conditionName + "@" + InstanceId.ToString();
                        conditionProfile.InitTags(IdsReplacer.ReplaceText(customdata, ReplaceKeys, ReplaceValues));
                        Conditions.Add(conditionProfile);
                        continue;
                    }
                }

                if (ProfileManager.EventConditions.TryGetValue(conditionName, out conditionProfile))
                {
                    Conditions.Add(conditionProfile);
                }

            }




            foreach (var actionName in Profile.ActionIds)
            {
                EventActionProfile actionProfile = null;

                if (Insertable)
                {
                    var customdata = "";
                    if (ProfileManager.TemplateEventActionProfiles.TryGetValue(actionName, out customdata))
                    {
                        var instancesubtypeId = actionName + "@" + InstanceId.ToString();

                        // Check if instancesubtypeId already exists in the dictionary
                        if (!ProfileManager.EventActionReferenceProfiles.ContainsKey(instancesubtypeId))
                        {
                            var actionReference = new EventActionReferenceProfile();
                            actionReference.InitTags(IdsReplacer.ReplaceText(customdata, ReplaceKeys, ReplaceValues));
                            actionReference.ProfileSubtypeId = instancesubtypeId;

                            var actionObject = new EventActionProfile();
                            actionObject.InitTags(IdsReplacer.ReplaceText(customdata, ReplaceKeys, ReplaceValues));
                            actionObject.ProfileSubtypeId = instancesubtypeId;

                            Actions.Add(actionObject);

                            ProfileManager.EventActionReferenceProfiles.Add(instancesubtypeId, actionReference);
                        }
                        else
                        {
                            // Handle the case where the key already exists, if necessary
                            var actionObject = new EventActionProfile();
                            actionObject.InitTags(IdsReplacer.ReplaceText(customdata, ReplaceKeys, ReplaceValues));
                            actionObject.ProfileSubtypeId = instancesubtypeId;

                            Actions.Add(actionObject);
                        }

                        continue;
                    }
                }

                byte[] bytes = null;

                if (ProfileManager.EventActionObjectTemplates.TryGetValue(actionName, out bytes))
                {
                    EventActionProfile action = MyAPIGateway.Utilities.SerializeFromBinary<EventActionProfile>(bytes);

                    if (action != null)
                    {
                        Actions.Add(action);
                    }
                }
            }

        }

        public void ActivateEventActions()
        {
            this.LastTriggerTime = MyAPIGateway.Session.GameDateTime;
            this.CooldownTimeTrigger = MathTools.RandomBetween(Profile.MinCooldownMs, Profile.MaxCooldownMs);

            //MyVisualScriptLogicProvider.ShowNotificationToAll("Action Count: " + Actions.Count, 5000);



            if (Actions.Count <= 0)
                return;

            if(Profile.ActionExecution == ActionExecutionEnum.Condition)
            {
                for (int i = 0; i < Actions.Count; i++)
                {
                    if(RequiredConditionIndex == i)
                        Actions[i].ExecuteAction(this.InstanceId);
                }
            }

            if (Profile.ActionExecution == ActionExecutionEnum.All)
            {
                for (int i = 0; i < Actions.Count; i++)
                {
                    Actions[i].ExecuteAction(InstanceId);
                }
            }

            if (Profile.ActionExecution == ActionExecutionEnum.Random)
            {
                var index = MathTools.RandomBetween(0, (Actions.Count));
                Actions[index].ExecuteAction(InstanceId);

            }

            if (Profile.ActionExecution == ActionExecutionEnum.Sequential)
            {

                ProcessingSequentialActions = true;
                ActionIndex = 0;
                ActionStartTime = MyAPIGateway.Session.GameDateTime;
                CheckingType = CheckType.ExecuteAction;
                IncrementMs = Profile.TimeUntilNextActionMs;

                /*
                for (int i = Actions.Count - 1; i >= 0; i--) {

                    var EventTime = new EventTime();
                    EventTime.Event = this;
                    EventTime.StartDate = MyAPIGateway.Session.GameDateTime;
                    EventTime.Timeinms = (i * Profile.TimeUntilNextActionMs);
                    EventTime.Type = CheckType.ExecuteAction;
                    EventTime.ActionIndex = i;
                    EventManager.EventTimes.Add(EventTime);

                }
                */
            }

        }

        public bool ValidateCooldown()
        {
            if ((this.Profile.StartsReady && this.RunCount == 0))
                return true;

            if (this.CooldownTimeTrigger > 0)
            {

                var duration = MyAPIGateway.Session.GameDateTime - this.LastTriggerTime;

                if (duration.TotalMilliseconds > this.CooldownTimeTrigger)
                    return true;
                else
                    return false;

            }



            return true;
        }







        public string GetInfo()
        {

            var conditions = new StringBuilder();
            for (int i = 0; i < Conditions.Count; i++)
            {
                conditions.Append(Conditions[i].ProfileSubtypeId).Append($":{EventCondition.IsConditionMet(Conditions[i])} ");
            }


            var persistantConditions = new StringBuilder();
            for (int i = 0; i < PersistantConditions.Count; i++)
            {
                persistantConditions.Append(PersistantConditions[i].ProfileSubtypeId).Append($":{EventCondition.IsConditionMet(PersistantConditions[i])} ");
            }
            var actions = new StringBuilder();
            for (int i = 0; i < Actions.Count; i++)
            {
                actions.Append(Actions[i].ProfileSubtypeId).Append(" ");
            }

            var sb = new StringBuilder();
            sb.Append(" - Profile SubtypeId:           ").Append(ProfileSubtypeId).AppendLine();
            sb.Append(" - EventEnabled:                ").Append(EventEnabled).AppendLine();
            sb.Append(" - UniqueEvent:                 ").Append(Profile.UniqueEvent).AppendLine();
            sb.Append(" - RunCount:                    ").Append(RunCount).AppendLine();

            sb.Append(" - CooldownReady:               ").Append(this.ValidateCooldown()).AppendLine();


            sb.Append(" - PersistantConditions Count:  ").Append(PersistantConditions.Count).AppendLine();
            sb.Append("     [] PersistantConditions:   ").Append(persistantConditions.ToString()).AppendLine();

            sb.Append(" - Conditions Count:            ").Append(Conditions.Count).AppendLine();
            sb.Append("     [] Conditions:             ").Append(conditions.ToString()).AppendLine();

            sb.Append(" - Actions Count:               ").Append(Actions.Count).AppendLine();
            sb.Append("     [] Actions:                ").Append(actions.ToString()).AppendLine();
            sb.Append(" - ErrorOnSetup:                ").Append(ErrorOnSetup).AppendLine();
            return sb.ToString();
        }


        public string GetShortInfo()
        {
            var sb = new StringBuilder();
            sb.Append(" - Profile SubtypeId:           ").Append(ProfileSubtypeId).AppendLine();
            sb.Append(" - EventEnabled:             ").Append(EventEnabled).AppendLine();
            sb.Append(" - UniqueEvent:                 ").Append(Profile.UniqueEvent).AppendLine();
            sb.Append(" - RunCount:                    ").Append(RunCount).AppendLine();
            return sb.ToString();
        }







    }
}
