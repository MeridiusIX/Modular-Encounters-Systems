using ModularEncountersSystems.Behavior.Subsystems.AutoPilot;
using ModularEncountersSystems.Core;
using ModularEncountersSystems.Files;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Behavior {

	public class NauticalRoutes : IBehaviorSubClass{

        public List<Route> _routes;


        DateTime WaitTime;
		IBehavior _behavior;
		public BehaviorSubclass SubClass { get { return _subClass; } set { _subClass = value; } }
		private BehaviorSubclass _subClass;

		public string DefaultWeaponProfile { get { return _defaultWeaponProfile; } }
		private string _defaultWeaponProfile;

        private Vector3D _NauticalRoute
        {
            get
            {
                var state = _behavior.AutoPilot.State;

                // Step 1: If RouteDirection is unset, pick randomly
                if (state.RouteDirection == RouteDirection.Unset)
                {
                    Random rand = new Random();
                    state.RouteDirection = (rand.Next(2) == 0) ? RouteDirection.Forward : RouteDirection.Backward;
                }

                // Step 2: If NodePositions not defined, pick closest route
                if (state.NodePositions == null || state.NodePositions.Count == 0)
                {
                    Route closestRoute = null;
                    Vector3D closestNode = new Vector3D();
                    int closestNodeIndex = 0;
                    double minDistance = double.MaxValue;

                    // Iterate all routes
                    for (int i = 0; i < _routes.Count; i++)
                    {
                        var route = _routes[i];
                        for (int j = 0; j < route.NodePositions.Count; j++)
                        {
                            double distance = Vector3D.Distance(_behavior.RemoteControl.GetPosition(), route.NodePositions[j]);
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                closestRoute = route;
                                closestNode = route.NodePositions[j];
                                closestNodeIndex = j;
                            }
                        }
                    }

                    if (closestRoute != null)
                    {
                        state.NodePositions = new List<Vector3D>(closestRoute.NodePositions);

                        // Set HeadingTowardsNode to the closest node index
                        state.HeadingTowardsNode = closestNodeIndex;

                    }
                    else
                    {
                        // fallback if no routes available
                        MyAPIGateway.Utilities.ShowMessage("MES", $"Routes: {_routes?.Count ?? -1}");
                        if (_routes != null)
                        {
                            for (int i = 0; i < _routes.Count; i++)
                            {
                                var route = _routes[i];
                                MyAPIGateway.Utilities.ShowMessage("MES", $"Route {i} nodes: {route.Nodes?.Count ?? -1}");
                                MyAPIGateway.Utilities.ShowMessage("MES", $"Route {i} nodespositions: {route.NodePositions?.Count ?? -1}");
                            }
                        }

                        return _behavior.RemoteControl.GetPosition();
                    }
                }
               
                // Step 3: Return current node we are heading towards

                if(_behavior.AutoPilot.CurrentPlanet == null)
                    return state.NodePositions[state.HeadingTowardsNode];

                return _behavior.AutoPilot.CurrentPlanet.SurfaceCoordsAtPosition(state.NodePositions[state.HeadingTowardsNode]);



            }
        }





        public NauticalRoutes(IBehavior behavior) {

			_subClass = BehaviorSubclass.NauticalRoutes;
			_behavior = behavior;
			WaitTime = MyAPIGateway.Session.GameDateTime;
			_defaultWeaponProfile = "MES-Weapons-GenericStandard";

            _routes = new List<Route>();

        }

		public void ProcessBehavior() {

			if(MES_SessionCore.IsServer == false) {

				return;

			}

			//Logger.Write(Mode.ToString(), BehaviorDebugEnum.General);

			if (_behavior.Mode != BehaviorMode.Retreat && _behavior.BehaviorSettings.DoRetreat == true){

				_behavior.ChangeCoreBehaviorMode(BehaviorMode.Retreat);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.LevelWithGravity | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.WaterNavigation);

			}
			
			if(_behavior.Mode == BehaviorMode.Init) {

				ReturnToRoute();

			}
            
			if(_behavior.Mode == BehaviorMode.ApproachWaypoint)
			{
				if(Vector3D.Distance(_NauticalRoute,_behavior.RemoteControl.GetPosition()) < 150 + _behavior.AutoPilot.Data.WaypointTolerance)
				{
                    GoToNextNode();
                    ReturnToRoute();
                }

                if (_behavior.AutoPilot.Targeting.HasTarget())
                {
                    _behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
                    _behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.LevelWithGravity | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.WaterNavigation);
                }

            }



            /*
			if(!_behavior.AutoPilot.Targeting.HasTarget() && _behavior.Mode != BehaviorMode.Retreat && _behavior.Mode != BehaviorMode.WaitingForTarget) {


				_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitingForTarget);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.LevelWithGravity);
				_behavior.BehaviorTriggerD = true;

			}
            */

			//A - Stop All Movement
			if (_behavior.BehaviorActionA) {

				_behavior.BehaviorActionA = false;
				_behavior.ChangeCoreBehaviorMode(BehaviorMode.WaitAtWaypoint);
				_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.LevelWithGravity);
				WaitTime = MyAPIGateway.Session.GameDateTime;
				_behavior.BehaviorTriggerC = true;

			}

			//WaitAtWaypoint
			if (_behavior.Mode == BehaviorMode.WaitAtWaypoint) {

				var timespan = MyAPIGateway.Session.GameDateTime - WaitTime;

				if (timespan.TotalSeconds >= _behavior.AutoPilot.Data.WaypointWaitTimeTrigger) {

                    ReturnToRoute();

					_behavior.BehaviorTriggerD = true;

				}

			}

			//Approach
			if (_behavior.Mode == BehaviorMode.ApproachTarget) {

				bool inRange = false;

				if (!_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.DistanceToTargetWaypoint < _behavior.AutoPilot.Data.EngageDistanceSpace)
					inRange = true;

				if(_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.DistanceToTargetWaypoint < _behavior.AutoPilot.Data.EngageDistancePlanet)
					inRange = true;

				if (inRange) {

					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToTarget | NewAutoPilotMode.LevelWithGravity | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.WaterNavigation);
					_behavior.ChangeCoreBehaviorMode(BehaviorMode.EngageTarget);
					_behavior.BehaviorTriggerA = true;

				}

			}

			//Engage
			if (_behavior.Mode == BehaviorMode.EngageTarget) {

				bool outRange = false;

				if (!_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.DistanceToTargetWaypoint > _behavior.AutoPilot.Data.DisengageDistanceSpace)
					outRange = true;

				if (_behavior.AutoPilot.InGravity() && _behavior.AutoPilot.DistanceToTargetWaypoint > _behavior.AutoPilot.Data.DisengageDistancePlanet)
					outRange = true;

				if (outRange) {

					_behavior.AutoPilot.ActivateAutoPilot(_behavior.RemoteControl.GetPosition(), NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.LevelWithGravity | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.WaterNavigation);
					_behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachTarget);
					_behavior.BehaviorTriggerB = true;

				}

			}

			//Retreat
			if (_behavior.Mode == BehaviorMode.Retreat) {

				if (_behavior.Despawn.NearestPlayer?.Player?.Controller?.ControlledEntity?.Entity != null) {

					//Logger.AddMsg("DespawnCoordsCreated", true);
					_behavior.AutoPilot.SetInitialWaypoint(VectorHelper.GetDirectionAwayFromTarget(_behavior.RemoteControl.GetPosition(), _behavior.Despawn.NearestPlayer.GetPosition()) * 1000 + _behavior.RemoteControl.GetPosition());

				}

			}

		}

        private void ReturnToRoute()
        {

            _behavior.ChangeCoreBehaviorMode(BehaviorMode.ApproachWaypoint);
            _behavior.AutoPilot.ActivateAutoPilot(_NauticalRoute, NewAutoPilotMode.RotateToWaypoint | NewAutoPilotMode.LevelWithGravity | NewAutoPilotMode.ThrustForward | NewAutoPilotMode.WaypointFromTarget | NewAutoPilotMode.WaterNavigation);

        }


		private void GoToNextNode()
		{
            var state = _behavior.AutoPilot.State;

            if (state.NodePositions != null && state.NodePositions.Count > 0)
            {
                int count = state.NodePositions.Count;

                if (state.RouteDirection == RouteDirection.Forward)
                {
                    // Move forward and wrap to 0 if at the end
                    state.HeadingTowardsNode = (state.HeadingTowardsNode + 1) % count;
                }
                else if (state.RouteDirection == RouteDirection.Backward)
                {
                    // Move backward and wrap to last index if at 0
                    state.HeadingTowardsNode = (state.HeadingTowardsNode - 1 + count) % count;
                }
            }


        }

        public void SetDefaultTags() {

			//Behavior Specific Defaults
			_behavior.AutoPilot.Data = ProfileManager.GetAutopilotProfile("RAI-Generic-Autopilot-Nautical");
			_behavior.Despawn.UseNoTargetTimer = false;
			
			if (string.IsNullOrWhiteSpace(_behavior.BehaviorSettings.WeaponsSystemProfile)) {

				_behavior.BehaviorSettings.WeaponsSystemProfile = _defaultWeaponProfile;

			}

		}

		public override string ToString() {

            var sb = new StringBuilder();
            sb.Append("::: Nautical Routes Behavior :::").AppendLine();
            sb.Append(" - Nodes:             ").Append(_behavior.AutoPilot.State.HeadingTowardsNode).AppendLine();


            if (_routes.Count > 0)
            {
                sb.Append(" - Loaded Routes:      ");
                foreach (var bank in _routes)
                {

                    sb.Append(bank.name).Append(", ");

                }

                sb.AppendLine();
            }

            sb.AppendLine();
            return sb.ToString();

        }

		public void InitTags() {

			if(string.IsNullOrWhiteSpace(_behavior.RemoteControl?.CustomData) == false) {

				var descSplit = _behavior.RemoteControl.CustomData.Split('\n');

				foreach(var tag in descSplit) {

                    if (tag.Contains("[Routes:") == true)
                    {
                        bool gotTrigger = false;
                        string FileSource = "";
                        TagParse.TagStringCheck(tag, ref FileSource);
                        //MyAPIGateway.Utilities.ShowMessage("MES", $"{FileSource}");
                        if (string.IsNullOrWhiteSpace(FileSource) == false)
                        {

                            Route route = ProfileManager.GetRoute(FileSource);

                            if (route != null)
                            {
                                gotTrigger = true;
                                _routes.Add(route);
                            }
                        }

                        if (!gotTrigger)
                        {
                            ProfileManager.ReportProfileError(FileSource, "Could Not Add Route To Behavior");
                            MyAPIGateway.Utilities.ShowMessage("MES", FileSource);
                        }

                    }

                }
				
			}

		}

	}

}
	
