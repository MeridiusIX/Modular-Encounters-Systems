using System;
using System.Collections.Generic;
using System.Text;

namespace Modular_Encounters_Systems.Data.Scripts.ModularEncountersSystems.Configuration {
    public interface IConfig {

        bool Load();

        bool Save();

    }
}
