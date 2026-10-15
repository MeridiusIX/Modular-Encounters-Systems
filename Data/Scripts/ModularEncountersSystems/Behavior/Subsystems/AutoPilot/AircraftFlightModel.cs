using System;
using VRage.Game;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems.AutoPilot {

	public partial class AutoPilotSystem {

		public void ApplyAircraftFlightModel(bool hasWaypoint) {

			var rotatingToWaypoint = hasWaypoint && (CurrentMode.HasFlag(NewAutoPilotMode.RotateToWaypoint) || CurrentMode.HasFlag(NewAutoPilotMode.RotateToTarget) || CurrentMode.HasFlag(NewAutoPilotMode.Ram));

			if (rotatingToWaypoint && this.Data.RollIntoTurnsMultiplier != 0 && !CurrentMode.HasFlag(NewAutoPilotMode.BarrelRoll))
				ApplyRollIntoTurns();

			if (this.Data.ReturnToLevelMultiplier > 0)
				ApplyReturnToLevel(rotatingToWaypoint);

		}

		public bool ApplyAircraftGyroRotation() {

			if ((this.Data.MaxAngleFromVelocity >= 180 && this.Data.MaxRotationStep <= 0) || this.RotationToApply == Vector3.Zero)
				return false;

			var worldRate = GetWorldRotationRate();
			LimitAngleFromVelocity(ref worldRate);
			LimitRotationStep(ref worldRate);
			ActiveGyro.UpdateRotationWorld(new Vector3D(this.RotationToApply.X, this.RotationToApply.Y, this.RotationToApply.Z), worldRate, this.RefBlockMatrixRotation);
			return true;

		}

		public Vector3D GetRotationDirection() {

			var rotationTarget = _currentWaypoint;

			if (Targeting.HasTarget() && (CurrentMode.HasFlag(NewAutoPilotMode.RotateToTarget) || CurrentMode.HasFlag(NewAutoPilotMode.Ram)))
				rotationTarget = Targeting.TargetLastKnownCoords;

			return Vector3D.Normalize(rotationTarget - this.RefBlockMatrixRotation.Translation);

		}

		public Vector3D GetWorldRotationRate() {

			var matrix = this.RefBlockMatrixRotation;
			double multiplier = this.Data.RotationMultiplier;
			double pitch = this.RotationToApply.X * this.Data.RotationMultiplierPitch * multiplier;
			double yaw = this.RotationToApply.Y * this.Data.RotationMultiplierYaw * multiplier;
			double roll = this.RotationToApply.Z * this.Data.RotationMultiplierRoll * multiplier;
			return (matrix.Right * pitch) + (matrix.Up * -yaw) + (matrix.Forward * roll);

		}

		public void LimitAngleFromVelocity(ref Vector3D worldRate) {

			var limit = this.Data.MaxAngleFromVelocity;

			if (limit >= 180 || MyVelocity.LengthSquared() <= 1)
				return;

			var forward = this.RefBlockMatrixRotation.Forward;
			var flight = Vector3D.Normalize(MyVelocity);
			var angleFromVelocity = MathHelper.ToDegrees(Math.Acos(MathHelper.Clamp(Vector3D.Dot(forward, flight), -1, 1)));
			var away = Vector3D.Cross(flight, forward);

			if (angleFromVelocity <= 0.5 || away.LengthSquared() <= 1e-8)
				return;

			away = Vector3D.Normalize(away);
			var softStart = limit * 0.6;
			var keep = MathHelper.Clamp((limit - angleFromVelocity) / Math.Max(limit - softStart, 1), 0, 1);
			var outward = Vector3D.Dot(worldRate, away);

			if (outward > 0)
				worldRate -= away * outward * (1 - keep);

			if (angleFromVelocity > limit)
				worldRate -= away * MathHelper.ToRadians(angleFromVelocity - limit) * 3;

		}

		public void LimitRotationStep(ref Vector3D worldRate) {

			var maxStep = this.Data.MaxRotationStep;

			if (maxStep <= 0 || _remoteControl?.SlimBlock?.CubeGrid?.Physics == null)
				return;

			Vector3D current = _remoteControl.SlimBlock.CubeGrid.Physics.AngularVelocity;
			var step = worldRate - current;
			var stepLength = step.Length();

			if (stepLength > maxStep)
				worldRate = current + step * (maxStep / stepLength);

		}

		public void ApplyRollIntoTurns() {

			if (_upDirection == Vector3D.Zero)
				return;

			var matrix = this.RefBlockMatrixRotation;
			var rotationDirection = GetRotationDirection();
			var forwardFlat = matrix.Forward - _upDirection * Vector3D.Dot(matrix.Forward, _upDirection);
			var targetFlat = rotationDirection - _upDirection * Vector3D.Dot(rotationDirection, _upDirection);

			if (!(forwardFlat.LengthSquared() >= 0.01) || !(targetFlat.LengthSquared() >= 1e-6))
				return;

			var rightFlat = Vector3D.Cross(forwardFlat, _upDirection);
			var headingError = MathHelper.ToDegrees(Math.Atan2(Vector3D.Dot(targetFlat, rightFlat) / rightFlat.Length(), Vector3D.Dot(targetFlat, forwardFlat) / forwardFlat.Length()));
			var maxBank = MathHelper.Clamp(this.Data.MaxBankAngle, 0, 89);
			var wantedBank = MathHelper.Clamp(this.Data.RollIntoTurnsMultiplier * headingError, -maxBank, maxBank);

			var right = -Vector3D.Dot(_upDirection, matrix.Right);
			var below = Vector3D.Dot(_upDirection, matrix.Up);
			var bank = MathHelper.ToDegrees(Math.Atan2(right, below));

			if (MaxRotationForGridSize == 0)
				MaxRotationForGridSize = _remoteControl.CubeGrid.GridSizeEnum == MyCubeSize.Small ? 6.28 : 3.14;

			var bankError = wantedBank - bank;

			if (bankError > 180)
				bankError -= 360;
			else if (bankError < -180)
				bankError += 360;

			var roll = (float)MathHelper.Clamp(MathHelper.ToRadians(bankError), -MaxRotationForGridSize, MaxRotationForGridSize);

			if (ExistingRollMagnitude != 0)
				roll = LimitSpeed(roll, ExistingRollMagnitude);

			this.RotationToApply.Z = roll;

		}

		public void ApplyReturnToLevel(bool rotatingToWaypoint) {

			if (rotatingToWaypoint || _upDirection == Vector3D.Zero || Math.Abs(this.RotationToApply.Y) > 0.0001f || Math.Abs(this.RotationToApply.Z) > 0.0001f)
				return;

			if (MyVelocity.LengthSquared() < this.Data.ReturnToLevelSpeedCutOff * this.Data.ReturnToLevelSpeedCutOff)
				return;

			var matrix = this.RefBlockMatrixRotation;
			var right = -Vector3D.Dot(_upDirection, matrix.Right);
			var below = Vector3D.Dot(_upDirection, matrix.Up);
			var horizontalWings = Math.Sqrt(right * right + below * below);
			var bank = MathHelper.ToDegrees(Math.Atan2(right, below));
			var absBank = Math.Abs(bank);

			if (absBank <= 3)
				return;

			var fade = absBank <= 90 ? 1 : MathHelper.Clamp((150 - absBank) / 60, 0, 1);

			if (fade <= 0)
				return;

			if (MaxRotationForGridSize == 0)
				MaxRotationForGridSize = _remoteControl.CubeGrid.GridSizeEnum == MyCubeSize.Small ? 6.28 : 3.14;

			this.RotationToApply.Z -= (float)(this.Data.ReturnToLevelMultiplier * MathHelper.Clamp(bank / 90, -1, 1) * horizontalWings * fade * MaxRotationForGridSize);

		}

	}

}
