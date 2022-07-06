using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Spawning;
using ModularEncountersSystems.Watchers;
using ModularEncountersSystems.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Tasks {
    public class WeatherSwitch : TaskItem, ITaskItem {

        internal float _stepsRemainingStart;
        internal bool _newWeather;

        public WeatherSwitch(Vector3D coords, string newWeather, float switchTime) {

            _isValid = true;
            _tickTrigger = 15;
            _stepsRemainingStart = (float)Math.Ceiling(switchTime / 2) * 4;
            var startWeather = MyAPIGateway.Session.WeatherEffects.GetWeather(coords);

            if (!string.IsNullOrWhiteSpace(startWeather) && startWeather != "Clear") {
            
            
            
            } else {
            
            
            
            }

        }

        public override void Run() {

            if (!_newWeather) {

                _stepsRemainingStart--;

            } else {
            
                
            
            }

        }

    }

}
