using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Tasks {
    public abstract class TaskItem {

        internal bool _isValid = true;
        internal byte _tickCounter = 0;
        internal byte _tickTrigger = 1;

        public virtual void Run() {

            

        }

        public bool Timer() {

            _tickCounter++;

            if (_tickCounter < _tickTrigger)
                return false;

            _tickCounter = 0;
            return true;

        }

        public virtual bool Valid() {

            return _isValid;
        
        }

    }
}
