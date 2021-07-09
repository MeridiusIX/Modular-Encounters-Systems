using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Tasks {
    public interface ITaskItem {

        void Run();
        bool Valid();

        bool Timer();

    }
}
