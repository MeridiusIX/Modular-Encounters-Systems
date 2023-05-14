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


namespace ModularEncountersSystems.Events.Action {

    [ProtoContract]
    public partial class EventActionProfile{

        [ProtoMember(1)] public string ProfileSubtypeId;
        [ProtoMember(2)] public List<ChatProfile> ChatData;
        [ProtoMember(3)] public List<SpawnProfile> Spawner;

        [ProtoIgnore]
        public EventActionReferenceProfile ActionReference {

            get {

                if (_actionReference == null) {

                    ProfileManager.EventActionReferenceProfiles.TryGetValue(ProfileSubtypeId, out _actionReference);

                }

                return _actionReference;

            }

        }

        [ProtoIgnore] private EventActionReferenceProfile _actionReference;

        public EventActionProfile() {

            ChatData = new List<ChatProfile>();
            Spawner = new List<SpawnProfile>();
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


                //Spawner
                if (tag.Contains("[SpawnData:") == true)
                {

                    string tempValue = "";
                    TagParse.TagStringCheck(tag, ref tempValue);
                    bool gotSpawn = false;

                    if (string.IsNullOrWhiteSpace(tempValue) == false)
                    {

                        byte[] byteData = { };

                        if (ProfileManager.SpawnerObjectTemplates.TryGetValue(tempValue, out byteData) == true)
                        {

                            try
                            {

                                var profile = MyAPIGateway.Utilities.SerializeFromBinary<SpawnProfile>(byteData);

                                if (profile != null)
                                {

                                    Spawner.Add(profile);
                                    gotSpawn = true;

                                }

                            }
                            catch (Exception)
                            {



                            }

                        }

                    }

                    if (!gotSpawn)
                        ProfileManager.ReportProfileError(tempValue, "Provided Spawn Profile Could Not Be Loaded In Profile: " + ProfileSubtypeId);

                }
            }

        }

    }

}



