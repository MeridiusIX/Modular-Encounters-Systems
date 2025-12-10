using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ModularEncountersSystems.Helpers {
    public static class Utilities {


        public static string ChangeAntennaName(string oldAntennaString, string newAntennaName)
        {
            //ChangeAntennaName("Scout-Antenna [Patrolling 60%]", "Guard-Antenna")
            // Returns: "Guard-Antenna [Patrolling 60%]"
            //ChangeAntennaName("Scout-Antenna", "Guard-Antenna")
            // Returns: "Guard-Antenna"


            // Extract existing thought bubble (if present)
            Match match = Regex.Match(oldAntennaString, @"\s*(\[(.*?)\])$");

            string thoughtBubble = match.Success ? $" {match.Groups[1].Value}" : "";

            // Return the new antenna name with preserved thought bubble
            return $"{newAntennaName}{thoughtBubble}";
        }




        public static string SetAntennaThoughtBubble(
            string antennaString,
            string thought,
            bool AntennaThoughtBubblePercentageActive,
            int AntennaThoughtBubblePercentage)
        {
            // Remove any existing thought bubble at the end
            string cleaned = Regex.Replace(antennaString, @"\s*\[.*\]$", "");

            // Build the thought bubble content
            string thoughtBubble = thought;
            if (AntennaThoughtBubblePercentageActive)
            {
                thoughtBubble += $" {AntennaThoughtBubblePercentage}%";
            }

            if (string.IsNullOrWhiteSpace(thoughtBubble))
                return cleaned;


            // Return with the new thought bubble
            return $"{cleaned} [{thoughtBubble}]";
        }


        public static string ClearAntennaThoughtBubble(string antennaString)
        {
            //ClearAntennaThoughtBubble("Sniper-Antenna [Focused 100%]")
            // Returns: "Sniper-Antenna"

            //ClearAntennaThoughtBubble("Sniper-Antenna")
            // Returns: "Sniper-Antenna"


            // Remove the trailing thought bubble, if present
            return Regex.Replace(antennaString, @"\s*\[.*\]$", "");
        }



    }
}
