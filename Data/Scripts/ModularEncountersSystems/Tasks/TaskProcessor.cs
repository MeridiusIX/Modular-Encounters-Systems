using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Tasks {
    public static class TaskProcessor {

        public static SubscribeTask Tick1 = new SubscribeTask(1);
        public static SubscribeTask Tick10 = new SubscribeTask(10);
        public static SubscribeTask Tick30 = new SubscribeTask(30);
        public static SubscribeTask Tick60 = new SubscribeTask(60);
        public static SubscribeTask Tick100 = new SubscribeTask(100);

        public static List<ITaskItem> Tasks = new List<ITaskItem>();

        private static byte _tickIncrement;
        public static byte TickIncrement {
            get {

                if (_tickIncrement >= 255)
                    _tickIncrement = 0;
                else
                    _tickIncrement++;

                return _tickIncrement;

            }
        }

        public static void Setup() {

            Tasks.Add(Tick1);
            Tasks.Add(Tick10);
            Tasks.Add(Tick30);
            Tasks.Add(Tick60);
            Tasks.Add(Tick100);

        }

        public static void Process() {

            for (int i = Tasks.Count - 1; i >= 0; i--) {

                if (!Tasks[i].Valid()) {

                    Tasks.RemoveAt(i);
                    continue;

                }

                if(Tasks[i].Timer())
                Tasks[i].Run();
            
            }
        
        }

    }

}
