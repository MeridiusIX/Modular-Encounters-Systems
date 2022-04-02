using ModularEncountersSystems.BlockLogic;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Sync;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Tasks {
	public class DebugRotationData : TaskItem, ITaskItem {

		private IMyCubeGrid _grid;
		private MyShipController _controller;
		private IMyRemoteControl _remoteControl;
		private DateTime _createTime;
		private ChatMessage _msg;

		private bool _endRotation;

		private double _force;
		private double _weight;
		private double _rotationIndicator;

		private Vector3D _startingForward;
		private Vector3D _lastDirection;
		private double _lastAngleFromLast;
		private Vector3D _endingForward;

		private double _lastMagnitude;

		private StringBuilder sb;
		private StringBuilder csv;

		public DebugRotationData(GridEntity grid, float rotationIndicator, ChatMessage msg) {

			_grid = grid.CubeGrid;
			_rotationIndicator = rotationIndicator;
			_msg = msg;
			sb = new StringBuilder();
			csv = new StringBuilder();

			if (grid.Controllers.Count == 0) {

				_isValid = false;
				return;
			
			}

			foreach (var control in grid.Controllers) {

				if (control.Block as IMyRemoteControl == null)
					continue;

				_remoteControl = control.Block as IMyRemoteControl;
				_startingForward = _remoteControl.WorldMatrix.Forward;
				_lastDirection = _startingForward;
				_weight = _remoteControl.CalculateShipMass().PhysicalMass;
				_controller = control.Block as MyShipController;
				_controller.MoveAndRotate(Vector3.Zero, new Vector2(0, 1f * (float)_rotationIndicator), 0);
				_controller.MoveAndRotate();
				break;

			}

			if (_controller == null) {

				_isValid = false;
				return;

			}

			foreach (var gyro in grid.Gyros) {

				if (gyro?.Block != null && gyro.Block.IsFunctional && gyro.Block.IsWorking) {

					var gyroBlock = gyro.Block as IMyGyro;
					var def = (MyGyroDefinition)gyro.Block.SlimBlock.BlockDefinition;
					_force += def.ForceMagnitude * gyroBlock.GyroPower * gyroBlock.GyroStrengthMultiplier;

				}

			}

			sb.Append("Grid Name:           ").Append(_remoteControl.SlimBlock.CubeGrid.CustomName).AppendLine();
			sb.Append("Grid Size:           ").Append(_remoteControl.SlimBlock.CubeGrid.GridSizeEnum).AppendLine();
			sb.Append("Grid Weight:         ").Append(_weight).AppendLine();
			sb.Append("Gyro Force:          ").Append(_force).AppendLine();
			sb.Append("Weight / Force:      ").Append(_weight / _force).AppendLine();
			sb.Append("Force / Weight:      ").Append(_force / _weight).AppendLine();
			sb.Append("Rotation Multiplier: ").Append(_rotationIndicator).AppendLine();
			sb.AppendLine().AppendLine();

			csv.Append("Mode,Time,Magnitude,Magnitude Change Increment,Magnitude Change Multiplier,Angle From Start / End,Angle From Previous Position,Previous Angle Change Increment,Previous Angle Change Multiplier").AppendLine();
			_createTime = MyAPIGateway.Session.GameDateTime;
			_tickTrigger = 6;

		}

		public override void Run() {

			var timespan = MyAPIGateway.Session.GameDateTime - _createTime;
			double time = timespan.TotalMilliseconds;
			var angularVelocity = _remoteControl.SlimBlock.CubeGrid.Physics.AngularVelocity;
			var yawMag = -Vector3D.Dot(angularVelocity, _remoteControl.WorldMatrix.Up);

			if ((!_endRotation && time > 10000) || (!_endRotation && _lastMagnitude >= yawMag)) {

				_endingForward = _remoteControl.WorldMatrix.Forward;
				_controller.MoveAndRotate(Vector3.Zero, new Vector2(0, -1f * (float)_rotationIndicator), 0);
				_controller.MoveAndRotate();
				_controller.MoveAndRotate(Vector3.Zero, new Vector2(0, -1f * (float)_rotationIndicator), 0);
				_controller.MoveAndRotate();
				_endRotation = true;

			}

			csv.Append(_endRotation ? "End Rotation," : "Rotation Active,");
			csv.Append(time).Append(",");
			csv.Append(yawMag).Append(",");
			csv.Append(Math.Round(yawMag, 10) - Math.Round(_lastMagnitude, 10)).Append(",");
			csv.Append(Math.Round(yawMag, 10) / Math.Round(_lastMagnitude, 10)).Append(",");

			_lastMagnitude = yawMag;
			double angleFromPoint = 0;

			if (!_endRotation) {
				
				angleFromPoint = VectorHelper.GetAngleBetweenDirections(_remoteControl.WorldMatrix.Forward, _startingForward);

			} else {

				angleFromPoint = VectorHelper.GetAngleBetweenDirections(_remoteControl.WorldMatrix.Forward, _endingForward);

			}

			csv.Append(angleFromPoint).Append(",");

			var angleFromLast = VectorHelper.GetAngleBetweenDirections(_remoteControl.WorldMatrix.Forward, _lastDirection);
			csv.Append(angleFromLast).Append(",");
			csv.Append(Math.Round(angleFromLast, 10) - Math.Round(_lastAngleFromLast, 10)).Append(",");
			csv.Append(Math.Round(angleFromLast, 10) / Math.Round(_lastAngleFromLast, 10));
			csv.AppendLine();
			_lastDirection = _remoteControl.WorldMatrix.Forward;
			_lastAngleFromLast = angleFromLast;


			if (_endRotation && yawMag <= 0) {

				_isValid = false;
				sb.Append(csv.ToString());
				_msg.ClipboardPayload = sb.ToString();
				_msg.Mode = ChatMsgMode.ReturnMessage;
				_msg.ReturnMessage = "Rotation Calculations Completed";
				_msg.Message = "/noprocessing";
				ChatManager.ProcessServerChat(_msg);

			}

		}

	}

}
