using ModularEncountersSystems.Configuration.Editor;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Configuration.Editor {
    public static class EditorTools {

        private static List<string> _stringList = new List<string>();

        public static string EditSettings(string receivedCommand) {

            if (receivedCommand.StartsWith("/MES.Settings.ResetAll")) {

                Settings.General = new ConfigGeneral();
                Settings.General.SaveSettings();

                Settings.Grids = new ConfigGrids();
                Settings.Grids.SaveSettings();

                Settings.CustomBlocks = new ConfigCustomBlocks();
                Settings.CustomBlocks.SaveSettings();

                Settings.SpaceCargoShips = new ConfigSpaceCargoShips();
                Settings.SpaceCargoShips.SaveSettings();

                Settings.RandomEncounters = new ConfigRandomEncounters();
                Settings.RandomEncounters.SaveSettings();

                Settings.PlanetaryCargoShips = new ConfigPlanetaryCargoShips();
                Settings.PlanetaryCargoShips.SaveSettings();

                Settings.PlanetaryInstallations = new ConfigPlanetaryInstallations();
                Settings.PlanetaryInstallations.SaveSettings();

                Settings.BossEncounters = new ConfigBossEncounters();
                Settings.BossEncounters.SaveSettings();

                Settings.Creatures = new ConfigCreatures();
                Settings.Creatures.SaveSettings();

                Settings.OtherNPCs = new ConfigOtherNPCs();
                Settings.OtherNPCs.SaveSettings();

                return "All Settings Have Been Reset";
            
            }

            if (receivedCommand.StartsWith("/MES.Settings.General."))
                return Settings.General.EditFields(receivedCommand);

            if (receivedCommand.StartsWith("/MES.Settings.Grids."))
                return Settings.Grids.EditFields(receivedCommand);

            if (receivedCommand.StartsWith("/MES.Settings.CustomBlocks."))
                return Settings.CustomBlocks.EditFields(receivedCommand);

            if (receivedCommand.StartsWith("/MES.Settings.SpaceCargoShips."))
                return Settings.SpaceCargoShips.EditFields(receivedCommand);

            if (receivedCommand.StartsWith("/MES.Settings.RandomEncounters."))
                return Settings.RandomEncounters.EditFields(receivedCommand);

            if (receivedCommand.StartsWith("/MES.Settings.PlanetaryCargoShips."))
                return Settings.PlanetaryCargoShips.EditFields(receivedCommand);

            if (receivedCommand.StartsWith("/MES.Settings.PlanetaryInstallations."))
                return Settings.PlanetaryInstallations.EditFields(receivedCommand);

            if (receivedCommand.StartsWith("/MES.Settings.BossEncounters."))
                return Settings.BossEncounters.EditFields(receivedCommand);

            if (receivedCommand.StartsWith("/MES.Settings.OtherNPCs."))
                return Settings.OtherNPCs.EditFields(receivedCommand);

            if (receivedCommand.StartsWith("/MES.Settings.Creatures."))
                return Settings.Creatures.EditFields(receivedCommand);

            return "Command Not Recognized";
        
        }

        public static bool SetCommandValueBool(string command, ref bool target) {

            var splitCommand = (command.Trim()).Split('.');
            bool result = false;

            if (splitCommand.Length < 5 || !bool.TryParse(splitCommand[4], out result))
                return false;

            target = result;
            return true;
        
        }

        public static bool SetCommandValueDouble(string command, ref double target) {

            var splitCommand = (command.Trim()).Split('.');
            double result = 0;

            if (splitCommand.Length < 5 || !double.TryParse(splitCommand[4], out result))
                return false;

            target = result;
            return true;

        }

        public static bool SetCommandValueFloat(string command, ref float target) {

            var splitCommand = (command.Trim()).Split('.');
            float result = 0;

            if (splitCommand.Length < 5 || !float.TryParse(splitCommand[4], out result))
                return false;

            target = result;
            return true;

        }

        public static bool SetCommandValueInt(string command, ref int target) {

            var splitCommand = (command.Trim()).Split('.');
            int result = 0;

            if (splitCommand.Length < 5 || !int.TryParse(splitCommand[4], out result))
                return false;

            target = result;
            return true;

        }

        public static bool SetCommandValueLong(string command, ref long target) {

            var splitCommand = (command.Trim()).Split('.');
            long result = 0;

            if (splitCommand.Length < 5 || !long.TryParse(splitCommand[4], out result))
                return false;

            target = result;
            return true;

        }

        public static bool SetCommandValueString(string command, ref string target) {

            var splitCommand = (command.Trim()).Split('.');

            if (splitCommand.Length < 5)
                return false;

            target = splitCommand[4];
            return true;

        }

        ///MES.Settings.Grids.GlobalReplenishmentProfiles.Add.MES-Replenishment-BaseRules
        public static bool SetCommandValueStringArray(string command, ref string[] target) {

            var splitCommand = (command.Trim()).Split('.');

            if (splitCommand.Length < 6)
                return false;

            _stringList.Clear();
            _stringList.AddArray<string>(target);

            if (splitCommand[4] == "Add") {

                if (!_stringList.Contains(splitCommand[5])) {

                    _stringList.Add(splitCommand[5]);
                    target = _stringList.ToArray();

                }
                
                return true;

            }

            if (splitCommand[4] == "Remove") {

                if (_stringList.Contains(splitCommand[5])) {

                    _stringList.Remove(splitCommand[5]);
                    target = _stringList.ToArray();

                }
                
                return true;

            }

            return false;

        }

    }

}
