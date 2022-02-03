using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Tasks {
    public abstract class TaskItem {

        internal bool _isValid = true;
        internal short _tickCounter = 0;
        internal short _tickTrigger = 1;

        public virtual void Run() {

            

        }

        public virtual void Invalidate() {

            _isValid = false;
        
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
