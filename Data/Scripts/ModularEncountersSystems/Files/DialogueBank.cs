using System.Collections.Generic;
using System.Xml.Serialization;
using ModularEncountersSystems.Events.Condition;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;
using ModularEncountersSystems.Helpers;
using System;
using Sandbox.ModAPI;
using ModularEncountersSystems.Logging;
using Sandbox.Game;
using System.Linq;

namespace ModularEncountersSystems.Files
{

    public class DialogueBank
    {
        [XmlElement("ChatProfileId")]
        public string ChatProfileId;

        [XmlElement("DialogueCue")]
        public List<DialogueCue> DialogueCues;

        [XmlIgnore]
        public ChatProfile _chatProfile;

        [XmlIgnore]
        private bool setup = false;

        public void init()
        {
            setup = true;
            MyVisualScriptLogicProvider.ShowNotificationToAll("Setup", 5000);
            _chatProfile = null;

            bool gotChat = false;

            if (string.IsNullOrWhiteSpace(ChatProfileId) == false)
            {

                byte[] byteData = { };

                if (ProfileManager.ChatObjectTemplates.TryGetValue(ChatProfileId, out byteData) == true)
                {

                    try
                    {

                        var profile = MyAPIGateway.Utilities.SerializeFromBinary<ChatProfile>(byteData);

                        if (profile != null)
                        {

                            _chatProfile = profile;
                            gotChat = true;

                        }
                        else
                        {

                            BehaviorLogger.Write("Deserialized Chat Profile was Null", BehaviorDebugEnum.Error, true);

                        }

                    }
                    catch (Exception e)
                    {

                        BehaviorLogger.Write("Caught Exception While Attaching to Action Profile:", BehaviorDebugEnum.Error, true);
                        BehaviorLogger.Write(e.ToString(), BehaviorDebugEnum.Error, true);

                    }

                }
                else
                {

                    ProfileManager.ReportProfileError(ChatProfileId, "Chat Profile Not Registered in Profile Manager");

                }


            }
        }

        public bool GetChatProfile(string cueID, ref ChatProfile chat)
        {
            if(!this.setup)
                this.init();

            if (_chatProfile == null)
                return false;

            chat = _chatProfile;

            bool preparedChat = false;

            foreach (var Cues in this.DialogueCues)
            {

                if (cueID == Cues.DialogueCueId)
                {
                    foreach (var entries in Cues.Entries)
                    {

                        EventCondition conditionProfile = null;


                        if (!string.IsNullOrEmpty(entries.EventConditionId))
                        {
                            if (ProfileManager.EventConditions.TryGetValue(entries.EventConditionId, out conditionProfile))
                            {
                                if (!EventCondition.IsConditionMet(conditionProfile))
                                    continue;
                            }
                            else
                            {
                                continue;
                            }
                        }




                        //
                        var entry = entries.GetRandomEntry();

                        chat.BroadcastRandomly = false;
                        chat.ChatMessages = new List<string> { entry.Message };
                        chat.ChatAudio = new List<string> { entry.Message };


                        preparedChat = true;
                        return true;
                    }

                }

            }

            return false;

        }

    }




    public class DialogueCue
    {
        public string DialogueCueId;

        [XmlElement("Entries")]
        public List<Entries> Entries;
    }


    public class Entries
    {
        public string EventConditionId;

        [XmlElement("Entry")]
        public List<Entry> EntryList;



        public Entry GetRandomEntry()
        {
            Random random = new Random();

            if (EntryList == null || EntryList.Count == 0)
                return default(Entry); // Return default Entry if the list is empty

            // Calculate total weights
            float totalWeights = EntryList.Sum(entry => entry.Weights);


            // Generate a random value between 0 and totalWeights
            float randomValue = (float)random.NextDouble() * totalWeights;

            // Iterate through the EntryList and find the corresponding Entry based on weights
            foreach (var entry in EntryList)
            {
                if (randomValue < entry.Weights)
                    return entry;

                randomValue -= entry.Weights;
            }

            // In case of some error, return the last entry
            return EntryList.Last();

        }

    }

    // Class representing the <Entry> element
    public class Entry
    {
        public string Message = "";
        public string AudioId = "";
        public float Weights = 1;




    }

}



