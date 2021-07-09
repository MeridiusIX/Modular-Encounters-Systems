using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems.AutoPilot {
	public class GyroscopeProfile {

		public IMyGyro Block;
		public IMyRemoteControl RemoteControl;
		public IBehavior Behavior;

		public bool Working;
		public bool Valid;
		public bool Active;

		public bool EnableOverride;
		public Vector3D RawValues;
		public MatrixD RefMatrix;

		public float Yaw;
		public float Pitch;
		public float Roll;

		public double PitchMagnitude;
		public double YawMagnitude;
		public double RollMagnitude;

		public double AdjPitchMagnitude;
		public double AdjYawMagnitude;
		public double AdjRollMagnitude;

		public Vector3 PendingRotation;

		public GyroscopeProfile(IMyTerminalBlock block, IMyRemoteControl remoteControl, IBehavior behavior) {

			Block = block as IMyGyro;
			RemoteControl = remoteControl;
			Behavior = behavior;

			EnableOverride = false;
			RawValues = Vector3D.Zero;
			RefMatrix = MatrixD.Identity;
			Yaw = 0;
			Pitch = 0;
			Roll = 0;



			Valid = true;

			Block.OnClosing += CloseEntity;
			Block.IsWorkingChanged += WorkingChange;
			WorkingChange(Block);

			if (Working) {

				/*
				Block.GyroOverride = false;
				Block.Yaw = 0;
				Block.Pitch = 0;
				Block.Roll = 0;
				*/

			}

		}

		public bool UpdateRotation(double pitch_speed, double yaw_speed, double roll_speed, double multiplier, MatrixD matrix) {

			if (!Valid || !Working) {

				return false;

			}

			RawValues = new Vector3D(pitch_speed, yaw_speed, roll_speed);
			RefMatrix = matrix;
			var rotationVec = new Vector3D(-pitch_speed, yaw_speed, roll_speed); //because keen does some weird stuff with signs 
			rotationVec *= multiplier;
			var relativeRotationVec = Vector3D.TransformNormal(rotationVec, matrix);

			var gyroMatrix = this.Block.WorldMatrix;
			var transformedRotationVec = Vector3D.TransformNormal(relativeRotationVec, Matrix.Transpose(gyroMatrix));

			this.PendingRotation = Vector3.Zero;
			this.PendingRotation.X = (float)transformedRotationVec.X;
			this.PendingRotation.Y = (float)transformedRotationVec.Y;
			this.PendingRotation.Z = (float)transformedRotationVec.Z;

			return true;

		}

		public bool ApplyRotation() {

			if (!Valid || !Working) {

				return false;

			}

			var pendingEnable = this.PendingRotation != Vector3.Zero ? true : false;

			if (pendingEnable != this.EnableOverride) {

				this.EnableOverride = pendingEnable;
				Block.GyroOverride = this.EnableOverride;

			}

			if (this.PendingRotation.X != this.Pitch) {

				this.Pitch = this.PendingRotation.X;
				Block.Pitch = this.Pitch;

			}	

			if (this.PendingRotation.Y != this.Yaw) {

				this.Yaw = this.PendingRotation.Y;
				Block.Yaw = this.Yaw;

			}	

			if (this.PendingRotation.Z != this.Roll) {

				this.Roll = this.PendingRotation.Z;
				Block.Roll = this.Roll;

			}

			/*
			if (this?.Block?.SlimBlock?.CubeGrid?.Physics != null) {

				var anglularVel = this.Block.SlimBlock.CubeGrid.Physics.AngularVelocity;

				PitchMagnitude = Vector3D.Dot(anglularVel, RefMatrix.Right);
				YawMagnitude = Vector3D.Dot(anglularVel, RefMatrix.Up);
				RollMagnitude = Vector3D.Dot(anglularVel, RefMatrix.Forward);

				AdjPitchMagnitude = MagnitudeReduction(PitchMagnitude, Behavior.AutoPilot.PitchAngleDifference, Behavior.AutoPilot.PitchTargetAngleResult);
				AdjYawMagnitude = MagnitudeReduction(YawMagnitude, Behavior.AutoPilot.YawAngleDifference, Behavior.AutoPilot.YawTargetAngleResult, Behavior.AutoPilot.CurrentMode.HasFlag(NewAutoPilotMode.HeavyYaw));
				AdjRollMagnitude = MagnitudeReduction(RollMagnitude, Behavior.AutoPilot.RollAngleDifference, Behavior.AutoPilot.RollTargetAngleResult, Behavior.AutoPilot.CurrentMode.HasFlag(NewAutoPilotMode.BarrelRoll));

				
				//AdjPitchMagnitude = PitchMagnitude;
				//AdjYawMagnitude = YawMagnitude;
				//AdjRollMagnitude = RollMagnitude;
				

				var newAngularVelocity = (RefMatrix.Right * AdjPitchMagnitude) + (RefMatrix.Up * AdjYawMagnitude) + (RefMatrix.Forward * AdjRollMagnitude);

				if (anglularVel != newAngularVelocity)
					this.Block.SlimBlock.CubeGrid.Physics.AngularVelocity = newAngularVelocity;

			}
			*/

			return true;

		}

		public double MagnitudeReduction(double magnitude, double AngleDifference, double TargetAngleDifference, bool ignore = false) {

			if (ignore)
				return magnitude;

			if (Math.Abs(magnitude) < 1)
				return magnitude;

			if (AngleDifference >= (Behavior.AutoPilot.Data.LimitRotationSpeed ? 135 : 90))
				return magnitude;

			if (TargetAngleDifference >= 0)
				return magnitude;

			return magnitude *= 0.75;

		}

		public void StopRotation() {

			this.PendingRotation = Vector3.Zero;
			ApplyRotation();

		}

		private void WorkingChange(IMyCubeBlock cubeBlock) {

			Working = Block.IsWorking && Block.IsFunctional;

			if (!Working && Valid && Active) {

				Block.GyroOverride = false;
				Block.Pitch = 0;
				Block.Yaw = 0;
				Block.Roll = 0;
				Active = false;
			
			}

		}

		private void CloseEntity(IMyEntity entity) {

			//Logger.Write("Removed Thrust - Block Closed", BehaviorDebugEnum.Thrust);
			Valid = false;
			Active = false;
			Unload();

		}

		private void Unload() {

			if (Block == null)
				return;

			Block.OnClosing -= CloseEntity;
			Block.IsWorkingChanged -= WorkingChange;

		}

	}
}
