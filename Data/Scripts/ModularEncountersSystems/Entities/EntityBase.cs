using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Entities {

	public enum EntityType {
	
		Generic,
		Player,
		Grid,
		Block,
	
	}

	public abstract class EntityBase {

		public bool IsValidEntity;
		public IMyEntity Entity;
		public IMyEntity ParentEntity;
		public bool Closed;
		public EntityType Type;

		public EntityBase(IMyEntity entity) {

			if (entity == null) {

				Closed = true;
				return;

			}

			Entity = entity;
			ParentEntity = entity.GetTopMostParent();
			Closed = entity.Closed;
			Entity.OnClose += CloseEntity;

		}

		public virtual void CloseEntity(IMyEntity entity) {

			Closed = true;

		}

		//---------------------------------------------------
		//-----------Start Interface Methods-----------------
		//---------------------------------------------------

		public double BoundingBoxSize() {

			if (Entity?.PositionComp == null)
				return 0;

			return Vector3D.Distance(Entity.PositionComp.WorldAABB.Min, Entity.PositionComp.WorldAABB.Max);

		}

		public Vector3D CurrentAcceleration() {

			return EntityEvaluator.EntityAcceleration(ParentEntity);

		}
		public double CurrentAltitude() {

			if (Entity?.PositionComp == null)
				return 0;

			return EntityEvaluator.AltitudeAtPosition(Entity.PositionComp.WorldAABB.Center);

		}

		public double CurrentGravity() {

			if (Entity?.PositionComp == null)
				return 0;

			return EntityEvaluator.GravityAtPosition(Entity.PositionComp.WorldAABB.Center);

		}

		public double CurrentSpeed() {

			return EntityEvaluator.EntitySpeed(ParentEntity);

		}

		public Vector3D CurrentVelocity() {

			return EntityEvaluator.EntityVelocity(ParentEntity);

		}

		public virtual double Distance(Vector3D coords) {

			if (Entity?.PositionComp == null)
				return -1;

			return Vector3D.Distance(coords, Entity.PositionComp.WorldAABB.Center);

		}

		public virtual IMyEntity GetEntity() {

			if (Entity == null || Entity.MarkedForClose || Entity.Closed)
				return null;

			return Entity;
		
		}

		public long GetEntityId() {

			if (Entity == null || !MyAPIGateway.Entities.Exist(Entity))
				return 0;

			return Entity.EntityId;

		}

		public virtual EntityType GetEntityType() {

			return Type;
		
		}

		public IMyEntity GetParentEntity() {

			if (ParentEntity == null || !MyAPIGateway.Entities.Exist(ParentEntity))
				return null;

			return ParentEntity;

		}

		public virtual Vector3D GetPosition() {

			if (Entity?.PositionComp == null)
				return Vector3D.Zero;

			return Entity.PositionComp.WorldAABB.Center;

		}

		public bool InSafeZone() {

			if (Entity?.PositionComp == null)
				return false;

			return SafeZoneManager.IsPositionInSafeZone(Entity.PositionComp.WorldAABB.Center);

		}

		public virtual bool IsClosed() {

			return Closed;

		}

		public double MaxSpeed() {

			return EntityEvaluator.EntityMaxSpeed(ParentEntity);

		}

		public virtual bool ProtectedByShields() {

			return EntityEvaluator.EntityShielded(Entity);

		}

		public bool ValidEntity() {

			return IsValidEntity;

		}

		//---------------------------------------------------
		//------------End Interface Methods------------------
		//---------------------------------------------------

		public virtual void Unload() {

			if (Entity == null)
				return;

			Entity.OnClose -= CloseEntity;

		}

		

	}

}
