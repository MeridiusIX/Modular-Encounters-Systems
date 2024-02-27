using ModularEncountersSystems.Helpers;
using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems.AutoPilot {
	public partial class AutoPilotSystem {

		public List<GyroscopeProfile> GyroProfiles;
		public GyroscopeProfile ActiveGyro;

		public MatrixD RefBlockMatrixRotation;

		public Vector3 RotationToApply;

		public double YawAngleDifference;
		public double PitchAngleDifference;
		public double RollAngleDifference;

		public double YawTargetAngleResult;
		public double PitchTargetAngleResult;
		public double RollTargetAngleResult;

		public double ExistingPitchMagnitude;
		public double ExistingYawMagnitude;
		public double ExistingRollMagnitude;

		public float PreviousPitchSpeed;
		public float PreviousYawSpeed;
		public float PreviousRollSpeed;

		public double DebugYawAngleLeft;
		public double DebugYawAngleRight;

		public double MaxRotationForGridSize;

		public void ApplyGyroRotation() {

			if (ActiveGyro == null || !ActiveGyro.Active)
				return;

			ActiveGyro.ApplyRotation();

			if (Data.UseForcedRotationDampening) {

				double pitch = ExistingPitchMagnitude;
				double yaw = ExistingYawMagnitude;
				double roll = ExistingRollMagnitude;

				//Pitch
				if (Math.Sign(RotationToApply.X) != Math.Sign(ExistingPitchMagnitude) || (RotationToApply.X / ExistingPitchMagnitude) > Data.ForcedRotationDampeningAmount) {

					pitch = ExistingPitchMagnitude - (ExistingPitchMagnitude * Data.ForcedRotationDampeningAmount);
					//MyVisualScriptLogicProvider.ShowNotificationToAll("Damp Pitch", 250);

				}

				//Yaw
				if (Math.Sign(RotationToApply.Y) != Math.Sign(ExistingYawMagnitude) || (RotationToApply.Y / ExistingYawMagnitude) > Data.ForcedRotationDampeningAmount) {

					yaw = ExistingYawMagnitude - (ExistingYawMagnitude * Data.ForcedRotationDampeningAmount);
					//MyVisualScriptLogicProvider.ShowNotificationToAll("Damp Yaw", 250);

				}

				//Roll
				if (Math.Sign(RotationToApply.Z) != Math.Sign(ExistingRollMagnitude) || (RotationToApply.Z / ExistingRollMagnitude) > Data.ForcedRotationDampeningAmount) {

					roll = ExistingRollMagnitude - (ExistingRollMagnitude * Data.ForcedRotationDampeningAmount);
					//MyVisualScriptLogicProvider.ShowNotificationToAll("Damp Roll", 250);

				}

				var newAngularVelocity = (_remoteControl.WorldMatrix.Right * pitch) + (_remoteControl.WorldMatrix.Up * -yaw) + (_remoteControl.WorldMatrix.Forward * roll);
				_behavior.CurrentGrid.CubeGrid.Physics.SetSpeeds(_behavior.CurrentGrid.CubeGrid.Physics.LinearVelocity, newAngularVelocity);
				//MyVisualScriptLogicProvider.ShowNotificationToAll("Damp Rotation", 250);

			}

		}

		public double CalculateGyroAxisRadians(double angleA, double angleB, double totalAngle, bool isSmallGrid, ref double angleDifference) {

			angleDifference = 0;
			double angleDirection = 1;

			if (angleA > angleB) {

				//Positive
				angleDifference = angleA - angleB;

			} else {

				//Negative
				angleDirection = -1;
				angleDifference = angleB - angleA;

			}

			if (totalAngle > 0) {

				var angleAddition = 180 - angleDifference;
				angleDifference += angleAddition;
			
			}

			angleDifference /= 2;

			if (angleDifference <= this.Data.DesiredAngleToTarget) {

				return 0;

			}

			if (angleDifference >= Data.RotationSlowdownAngle) {

				return isSmallGrid ? Math.PI * 2 * angleDirection : Math.PI * angleDirection;

			}

			return (angleDifference * (Math.PI / 180)) * angleDirection;

		}

		public void CalculateGyroRotation() {

			this.RotationToApply = Vector3.Zero;

			var rotationTarget = _currentWaypoint;

			if (Targeting.HasTarget()) {

				if (CurrentMode.HasFlag(NewAutoPilotMode.RotateToTarget) || CurrentMode.HasFlag(NewAutoPilotMode.Ram))
					rotationTarget = Targeting.TargetLastKnownCoords;

			}

			MatrixD referenceMatrix = this.RefBlockMatrixRotation; //This should be either the control block or at least represent what direction the ship should face
			Vector3D directionToTarget = Vector3D.Normalize(rotationTarget - referenceMatrix.Translation);
			Vector3 gyroRotation = new Vector3(0, 0, 0); // Pitch,Yaw,Roll

			ExistingPitchMagnitude = 0;
			ExistingYawMagnitude = 0;
			ExistingRollMagnitude = 0;

			//Measure Existing Speed
			if (_remoteControl?.SlimBlock?.CubeGrid?.Physics != null) {

				var angularVelocity = _remoteControl.SlimBlock.CubeGrid.Physics.AngularVelocity;
				ExistingPitchMagnitude = Vector3D.Dot(angularVelocity, _remoteControl.WorldMatrix.Right);
				ExistingYawMagnitude = -Vector3D.Dot(angularVelocity, _remoteControl.WorldMatrix.Up);
				ExistingRollMagnitude = Vector3D.Dot(angularVelocity, _remoteControl.WorldMatrix.Forward);

			}

			//Get Actual Angle To Target
			double angleToTarget = VectorHelper.GetAngleBetweenDirections(referenceMatrix.Forward, directionToTarget);
			this.AngleToCurrentWaypoint = angleToTarget;
			this.AngleToUpDirection = VectorHelper.GetAngleBetweenDirections(referenceMatrix.Up, _upDirection);
			//MyVisualScriptLogicProvider.ShowNotificationToAll("Total Angle: " + angleToTarget.ToString(), 166);


			if (angleToTarget <= this.Data.DesiredAngleToTarget && _upDirection == Vector3D.Zero) {

				this.RotationToApply = Vector3.Zero;
				return;

			}

			var gridSize = (_remoteControl.CubeGrid.GridSizeEnum == MyCubeSize.Small);

			if (MaxRotationForGridSize == 0)
				MaxRotationForGridSize = gridSize ? 6.28 : 3.14;

			//Calculate Yaw
			double angleLeftToTarget = VectorHelper.GetAngleBetweenDirections(referenceMatrix.Left, directionToTarget);
			double angleRightToTarget = VectorHelper.GetAngleBetweenDirections(referenceMatrix.Right, directionToTarget);
			DebugYawAngleLeft = angleLeftToTarget;
			DebugYawAngleRight = angleRightToTarget;
			YawTargetAngleResult = VectorHelper.GetAngleBetweenDirections(referenceMatrix.Forward, directionToTarget) - VectorHelper.GetAngleBetweenDirections(referenceMatrix.Backward, directionToTarget);
			gyroRotation.Y = (float)CalculateGyroAxisRadians(angleLeftToTarget, angleRightToTarget, YawTargetAngleResult, gridSize, ref YawAngleDifference);

			//Calculate Pitch
			if (_upDirection != Vector3D.Zero && CurrentMode.HasFlag(NewAutoPilotMode.LevelWithGravity) && !CurrentMode.HasFlag(NewAutoPilotMode.Ram)) {

				double angleForwardToUp = VectorHelper.GetAngleBetweenDirections(referenceMatrix.Forward, _upDirection);
				double angleBackwardToUp = VectorHelper.GetAngleBetweenDirections(referenceMatrix.Backward, _upDirection);
				PitchTargetAngleResult = VectorHelper.GetAngleBetweenDirections(referenceMatrix.Up, _upDirection) - VectorHelper.GetAngleBetweenDirections(referenceMatrix.Down, _upDirection);
				gyroRotation.X = (float)CalculateGyroAxisRadians(angleForwardToUp, angleBackwardToUp, PitchTargetAngleResult, gridSize, ref PitchAngleDifference);

			} else {

				double angleUpToTarget = VectorHelper.GetAngleBetweenDirections(referenceMatrix.Up, directionToTarget);
				double angleDownToTarget = VectorHelper.GetAngleBetweenDirections(referenceMatrix.Down, directionToTarget);
				PitchTargetAngleResult = VectorHelper.GetAngleBetweenDirections(referenceMatrix.Forward, directionToTarget) - VectorHelper.GetAngleBetweenDirections(referenceMatrix.Backward, directionToTarget);
				gyroRotation.X = (float)CalculateGyroAxisRadians(angleDownToTarget, angleUpToTarget, PitchTargetAngleResult, gridSize, ref PitchAngleDifference);

			}

			//Calculate Roll - If Specified
			if (_upDirection != Vector3D.Zero) {

				double angleRollLeftToUp = VectorHelper.GetAngleBetweenDirections(referenceMatrix.Left, _upDirection);
				double angleRollRightToUp = VectorHelper.GetAngleBetweenDirections(referenceMatrix.Right, _upDirection);

				if (angleRollLeftToUp == angleRollRightToUp) {

					angleRollLeftToUp--;
					angleRollRightToUp++;

				}

				RollTargetAngleResult = VectorHelper.GetAngleBetweenDirections(referenceMatrix.Up, _upDirection) - VectorHelper.GetAngleBetweenDirections(referenceMatrix.Down, _upDirection);
				gyroRotation.Z = (float)CalculateGyroAxisRadians(angleRollLeftToUp, angleRollRightToUp, RollTargetAngleResult, gridSize, ref RollAngleDifference);

			} else {

				RollTargetAngleResult = 0;
				RollAngleDifference = 0;

			}

			//Limit Speed
			if (ExistingPitchMagnitude != 0)
				gyroRotation.X = LimitSpeed(gyroRotation.X, ExistingPitchMagnitude);

			if (ExistingYawMagnitude != 0)
				gyroRotation.Y = LimitSpeed(gyroRotation.Y, ExistingYawMagnitude);

			if (ExistingRollMagnitude != 0)
				gyroRotation.Z = LimitSpeed(gyroRotation.Z, ExistingRollMagnitude);

			this.RotationToApply = gyroRotation;

			return;

		}

		public void BoostUptoRotationSpeed(float currentMagnitude, double axisAngleDifference, ref float currentSpeed, ref float previousSpeed) {

			var absCurrent = Math.Abs(currentSpeed);

			//Sign Check
			if (Math.Sign(currentSpeed) != Math.Sign(currentMagnitude)) {

				currentSpeed += (float)MathHelper.Clamp((Math.Sign(currentSpeed) * MaxRotationForGridSize), -MaxRotationForGridSize, MaxRotationForGridSize);
				previousSpeed = absCurrent;
				return;

			}

			if (previousSpeed == 0) {

				previousSpeed = absCurrent;
				return;
			
			}

			if (absCurrent > previousSpeed) {

				currentSpeed += (float)MathHelper.Clamp((Math.Sign(currentSpeed) * MaxRotationForGridSize), -MaxRotationForGridSize, MaxRotationForGridSize);

			}

			previousSpeed = absCurrent;

		}

		public float LimitSpeed(float proposedSpeed, double actualSpeed) {

			if (float.IsNaN(proposedSpeed) || double.IsNaN(actualSpeed))
				return 0;

			if (proposedSpeed == 0 || Math.Sign(proposedSpeed) != Math.Sign(actualSpeed))
				return proposedSpeed;

			if (_behavior.AutoPilot.Data.LimitRotationSpeed && Math.Abs(actualSpeed) > Math.Abs(proposedSpeed))
				return 0;

			if(Math.Abs(actualSpeed) > _behavior.AutoPilot.Data.MaxRotationMagnitude)
				return 0;

			return proposedSpeed;
		
		}

		public void GetNextEligibleGyro() {

			for (int i = GyroProfiles.Count - 1; i >= 0; i--) {

				var profile = GyroProfiles[i];

				if (!profile.Valid) {

					GyroProfiles.RemoveAt(i);
					continue;

				}

				if (!profile.Working)
					continue;

				profile.Active = true;
				this.ActiveGyro = profile;
				break;

			}
		
		}

		public MatrixD GetReferenceMatrix(MatrixD originalMatrix) {

			if (_behavior.BehaviorSettings.RotationDirection == Direction.Forward)
				return originalMatrix;

			if (_behavior.BehaviorSettings.RotationDirection == Direction.Backward)
				return MatrixD.CreateWorld(originalMatrix.Translation, originalMatrix.Backward, originalMatrix.Up);

			if (_behavior.BehaviorSettings.RotationDirection == Direction.Left)
				return MatrixD.CreateWorld(originalMatrix.Translation, originalMatrix.Left, originalMatrix.Up);

			if (_behavior.BehaviorSettings.RotationDirection == Direction.Right)
				return MatrixD.CreateWorld(originalMatrix.Translation, originalMatrix.Right, originalMatrix.Up);

			if (_behavior.BehaviorSettings.RotationDirection == Direction.Down)
				return MatrixD.CreateWorld(originalMatrix.Translation, originalMatrix.Down, originalMatrix.Forward);

			if (_behavior.BehaviorSettings.RotationDirection == Direction.Up)
				return MatrixD.CreateWorld(originalMatrix.Translation, originalMatrix.Up, originalMatrix.Backward);

			return originalMatrix;

		}

		public void PrepareGyroForRotation() {

			if (ActiveGyro == null || !ActiveGyro.Active)
				GetNextEligibleGyro();

			if (ActiveGyro == null || !ActiveGyro.Active)
				return;

			ActiveGyro.UpdateRotation(this.RotationToApply.X, this.RotationToApply.Y, this.RotationToApply.Z, this.Data.RotationMultiplier, this.RefBlockMatrixRotation);

		}

		public void ProcessRotationParallel(bool hasWaypoint) {

			if (hasWaypoint && (CurrentMode.HasFlag(NewAutoPilotMode.RotateToWaypoint) || CurrentMode.HasFlag(NewAutoPilotMode.RotateToTarget) || CurrentMode.HasFlag(NewAutoPilotMode.Ram))) {

				CalculateGyroRotation();

			} else {

				this.RotationToApply = Vector3.Zero;
			
			}

			if (CurrentMode.HasFlag(NewAutoPilotMode.BarrelRoll)) {

				this.RotationToApply.Z = 10;

			}

			if (CurrentMode.HasFlag(NewAutoPilotMode.HeavyYaw)) {

				this.RotationToApply.Y = 10;

			}

			PrepareGyroForRotation();

		}

		public void StopAllRotation() {

			this.RotationToApply = Vector3.Zero;
			ActiveGyro?.StopRotation();

		}

	}

}
