using ModularEncountersSystems.Helpers;
using System;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems.AutoPilot {
	public class ThrustAction {

		//X =   +Right / -Left
		//Y =      +Up / -Down
		//Z = +Forward / -Backward

		public bool ControlX;
		public bool InvertX;
		public float StrengthX;

		public bool ControlY;
		public bool InvertY;
		public float StrengthY;

		public bool ControlZ;
		public bool InvertZ;
		public float StrengthZ;

		public ThrustAction() {

			DisableAll();

		}

		public void DisableAll() {

			ControlX = false;
			ControlY = false;
			ControlZ = false;

			InvertX = false;
			InvertY = false;
			InvertZ = false;

			StrengthX = 0;
			StrengthY = 0;
			StrengthZ = 0;

		}

		public Base6Directions.Direction GetTransformedBaseDirection(Direction direction, MyBlockOrientation orientation) {


			var baseDir = new Base6Directions.Direction();

			if (direction == Direction.Right)
				baseDir = orientation.TransformDirection(Base6Directions.Direction.Right);

			if (direction == Direction.Left)
				baseDir = orientation.TransformDirection(Base6Directions.Direction.Left);

			if (direction == Direction.Up)
				baseDir = orientation.TransformDirection(Base6Directions.Direction.Up);

			if (direction == Direction.Down)
				baseDir = orientation.TransformDirection(Base6Directions.Direction.Down);

			if (direction == Direction.Forward)
				baseDir = orientation.TransformDirection(Base6Directions.Direction.Forward);

			if (direction == Direction.Backward)
				baseDir = orientation.TransformDirection(Base6Directions.Direction.Backward);

			return baseDir;

		}

		public Vector3D GetThrustAsVector() {

			var x = Math.Round(ControlX ? (InvertX ? StrengthX * -1 : StrengthX) : 0, 4);
			var y = Math.Round(ControlY ? (InvertY ? StrengthY * -1 : StrengthY) : 0, 4);
			var z = Math.Round(ControlZ ? (InvertZ ? StrengthZ * -1 : StrengthZ) : 0, 4);
			return new Vector3D(x, y, z);

		}

		public Vector3D GetThrustDataFromDirection(Direction direction, MyBlockOrientation orientation) {


			if (direction == Direction.None)
				return Vector3D.Zero;

			var baseDir = GetTransformedBaseDirection(direction, orientation);

			if (baseDir == Base6Directions.Direction.Right || baseDir == Base6Directions.Direction.Left) {

				return new Vector3D(ControlX ? 1 : 0, !InvertX ? 1 : -1, StrengthX);
			
			}

			if (baseDir == Base6Directions.Direction.Up || baseDir == Base6Directions.Direction.Down) {

				return new Vector3D(ControlY ? 1 : 0, !InvertY ? 1 : -1, StrengthY);

			}

			if (baseDir == Base6Directions.Direction.Forward || baseDir == Base6Directions.Direction.Backward) {

				return new Vector3D(ControlZ ? 1 : 0, !InvertZ ? 1 : -1, StrengthZ);

			}

			return Vector3D.Zero;

		}

		public bool IsThrustEnabledInDirection(Direction direction, MyBlockOrientation orientation) {

			if (direction == Direction.None)
				return false;

			var baseDir = GetTransformedBaseDirection(direction, orientation);

			if (baseDir == Base6Directions.Direction.Right || baseDir == Base6Directions.Direction.Left)
				return ControlX;

			if (baseDir == Base6Directions.Direction.Up || baseDir == Base6Directions.Direction.Down)
				return ControlY;

			if (baseDir == Base6Directions.Direction.Forward || baseDir == Base6Directions.Direction.Backward)
				return ControlZ;

			return false;
		
		}

		public void SetDir(bool enable, bool invert, float strength, Direction direction, MyBlockOrientation orientation) {

			if (direction == Direction.None)
				return;

			SetTransformedThrustData(GetTransformedBaseDirection(direction, orientation), enable, strength);

		}

		public void SetX(bool enable, bool invert, float strength, MyBlockOrientation orientation) {

			var axisDir = !invert ? orientation.TransformDirection(Base6Directions.Direction.Right) : orientation.TransformDirection(Base6Directions.Direction.Left);
			SetTransformedThrustData(axisDir, enable, strength);

		}

		public void SetY(bool enable, bool invert, float strength, MyBlockOrientation orientation) {

			var axisDir = !invert ? orientation.TransformDirection(Base6Directions.Direction.Up) : orientation.TransformDirection(Base6Directions.Direction.Down);
			SetTransformedThrustData(axisDir, enable, strength);

		}

		public void SetZ(bool enable, bool invert, float strength, MyBlockOrientation orientation) {

			var axisDir = !invert ? orientation.TransformDirection(Base6Directions.Direction.Forward) : orientation.TransformDirection(Base6Directions.Direction.Backward);
			SetTransformedThrustData(axisDir, enable, strength);

		}

		public void SetTransformedThrustData(Base6Directions.Direction direction, bool enable, float strength) {

			if (direction == Base6Directions.Direction.Right) {

				ControlX = enable;
				InvertX = false;
				StrengthX = strength;
				return;

			}

			if (direction == Base6Directions.Direction.Left) {

				ControlX = enable;
				InvertX = true;
				StrengthX = strength;
				return;

			}

			if (direction == Base6Directions.Direction.Up) {

				ControlY = enable;
				InvertY = false;
				StrengthY = strength;
				return;

			}

			if (direction == Base6Directions.Direction.Down) {

				ControlY = enable;
				InvertY = true;
				StrengthY = strength;
				return;

			}

			if (direction == Base6Directions.Direction.Forward) {

				ControlZ = enable;
				InvertZ = false;
				StrengthZ = strength;
				return;

			}

			if (direction == Base6Directions.Direction.Backward) {

				ControlZ = enable;
				InvertZ = true;
				StrengthZ = strength;
				return;

			}

		}

	}

}
