using ModularEncountersSystems.Configuration;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;
using Sandbox.ModAPI;

namespace ModularEncountersSystems.Events
{

    public enum CheckType
    {
        ExecuteAction
    }

    public class EventTime
    {
        public Event Event;
        public DateTime StartDate;
        public int Timeinms;
        public CheckType Type;
     

        

        public EventTime()
        {
            Event = new Event();
            StartDate = new DateTime();
            Timeinms = 5000;
            Type = CheckType.ExecuteAction;
        }
    }


}
