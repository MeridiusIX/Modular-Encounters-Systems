using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ProtoBuf;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Behavior.Subsystems {

	[ProtoContract]
	public class EscortProfile {

		[ProtoMember(1)] public long ParentId;
		[ProtoMember(2)] public long ChildId;
		[ProtoMember(3)] public Vector3D Offset;
		[ProtoMember(4)] public bool Valid;

		[ProtoIgnore] public IBehavior ParentBehavior;
		[ProtoIgnore] public IBehavior ChildBehavior;

		public EscortProfile() {

			ParentId = 0;
			ChildId = 0;
			Offset = Vector3D.Zero;
			Valid = false;

		}

		public void GetBehaviors() {

			ParentBehavior = BehaviorManager.GetBehavior(ParentId);
			ChildBehavior = BehaviorManager.GetBehavior(ChildId);
			Valid = ParentBehavior != null && ChildBehavior != null;

		}

		public Vector3D GetTransformedOffset(Vector3D existingCoords) {

			var matrix = ParentBehavior?.RemoteControl?.WorldMatrix ?? MatrixD.Identity;

			if (matrix == MatrixD.Identity) {

				Valid = false;
				return existingCoords;

			}
			

			return Vector3D.Transform(Offset, matrix);
		
		}

		public bool ValidationCheck() {

			if (!Valid)
				return false;

			if (ParentBehavior == null) {

				ParentBehavior = BehaviorManager.GetBehavior(ParentId);

				if (ParentBehavior == null) {

					Valid = false;
					return false;

				}	

			}

			if (!ParentBehavior.IsAIReady()) {

				Valid = false;
				return false;

			}

			if (ChildBehavior == null) {

				ChildBehavior = BehaviorManager.GetBehavior(ChildId);

				if (ChildBehavior == null) {

					Valid = false;
					return false;

				}

			}

			if (!ChildBehavior.IsAIReady()) {

				Valid = false;
				return false;

			}

			return true;

		}
	
	}

	public class EscortSystem {

		public List<Vector3D> EscortOffsets;

		internal IBehavior _behavior;
		internal IMyRemoteControl _remoteControl;

		internal List<Vector3D> _availableOffsets;

		public EscortSystem(IBehavior behavior, IMyRemoteControl remoteControl) {

			EscortOffsets = new List<Vector3D>();

			_behavior = behavior;
			_remoteControl = remoteControl;

			_availableOffsets = new List<Vector3D>();

		}

		public void CleanUpEscorts() {

			for (int i = _behavior.BehaviorSettings.ActiveEscorts.Count - 1; i >= 0; i--) {

				var escort = _behavior.BehaviorSettings.ActiveEscorts[i];

				if (escort == null || !escort.ValidationCheck()) {

					_behavior.BehaviorSettings.ActiveEscorts.RemoveAt(i);
					continue;

				}

			}

		}

		public void InitializeEscorts() {

			CleanUpEscorts();

			foreach (var escort in _behavior.BehaviorSettings.ActiveEscorts) {

				escort.ChildBehavior.BehaviorSettings.ParentEscort = escort;

			}
		
		}

		public void InitTags() {

			if (string.IsNullOrWhiteSpace(_behavior.RemoteControl.CustomData) == false) {

				var descSplit = _behavior.RemoteControl.CustomData.Split('\n');

				foreach (var tag in descSplit) {

					//EscortOffsets
					if (tag.Contains("[EscortOffsets:") == true) {

						TagParse.TagVector3DListCheck(tag, ref EscortOffsets);

					}


				}

			}

		}

		public bool DoesEscortExistLocally(long parentId, long childId) {

			CleanUpEscorts();

			foreach (var escort in _behavior.BehaviorSettings.ActiveEscorts) {

				if (parentId == escort.ParentId && childId == escort.ChildId)
					return true;

			}

			return false;
		
		}

		public string ProcessEscortRequest(Command command) {

			CleanUpEscorts();

			SpawnLogger.Write("Escort Request From: " + command.RemoteControl.EntityId, SpawnerDebugEnum.Dev);
			long requestor = command.RemoteControl.EntityId;

			//Check To See If Any Free Offsets Exist
			_availableOffsets.Clear();

			foreach (var offset in EscortOffsets) {

				bool usedOffset = false;

				foreach (var escort in _behavior.BehaviorSettings.ActiveEscorts) {

					if (offset == escort.Offset) {

						usedOffset = true;
						break;

					}
					
				}

				if (usedOffset)
					continue;

				_availableOffsets.Add(offset);
			
			}

			if (_availableOffsets.Count == 0)
				return "No Available Offsets To Assign Escort To";

			//See If Child Already Has a Parent. Abandon if Other Parent Is Closer
			if(command.Behavior.BehaviorSettings.ParentEscort != null && command.Behavior.BehaviorSettings.ParentEscort.ValidationCheck()) {

				var myDistance = Vector3D.Distance(command.RemoteControl.GetPosition(), _behavior.RemoteControl.GetPosition());
				var theirDistance = Vector3D.Distance(command.RemoteControl.GetPosition(), command.Behavior.BehaviorSettings.ParentEscort.ParentBehavior.RemoteControl.GetPosition());

				if (theirDistance <= myDistance) {

					return "Requestor Already Has Closer Parent";
				
				}

			}

			//Check for Existing Offset Assignment with Same Child
			foreach (var escort in _behavior.BehaviorSettings.ActiveEscorts) {

				if (escort.ChildId == requestor) {

					return "Requestor Already A Child Escort";

				}

			}

			//Checks Passed, Determine Which Offset is Closest To Child
			Vector3D selectedOffset = Vector3D.Zero;
			double closestDistance = -1;

			foreach (var offset in _availableOffsets) {

				var dist = Vector3D.Distance(command.RemoteControl.GetPosition(), Vector3D.Transform(offset, _behavior.RemoteControl.WorldMatrix));

				if (closestDistance == -1 || dist < closestDistance) {

					closestDistance = dist;
					selectedOffset = offset;

				}
			
			}

			//Create Escort, Assign
			var newEscort = CreateEscort(_behavior.RemoteControl.EntityId, requestor, selectedOffset);

			if (!newEscort.ValidationCheck()) {

				return "Escort Validation Check Failed";

			}

			if (command.Behavior.BehaviorSettings.ParentEscort != null) {

				var oldEscort = command.Behavior.BehaviorSettings.ParentEscort;
				oldEscort.Valid = false;

				if (oldEscort.ParentBehavior?.BehaviorSettings?.ActiveEscorts != null) {

					for (int i = oldEscort.ParentBehavior.BehaviorSettings.ActiveEscorts.Count - 1; i >= 0; i--) {

						var escort = oldEscort.ParentBehavior.BehaviorSettings.ActiveEscorts[i];

						if (escort == null || escort.ChildId == requestor) {

							oldEscort.ParentBehavior.BehaviorSettings.ActiveEscorts.RemoveAt(i);

						}

					}

				}

			}

			newEscort.ChildBehavior.BehaviorSettings.ParentEscort = newEscort;
			_behavior.BehaviorSettings.ActiveEscorts.Add(newEscort);
			SpawnLogger.Write("[" + newEscort.ChildBehavior.GridName + " / " + newEscort.ChildBehavior.CurrentGrid.Npc.BehaviorName + " / " + newEscort.ChildBehavior.RemoteControl.EntityId + "] Has Been Assigned Escort.", SpawnerDebugEnum.Dev);

			return "Escort Assignment Successful";

		}

		public EscortProfile CreateEscort(long parentId, long childId, Vector3D offset) {

			var escort = new EscortProfile();
			escort.ParentId = parentId;
			escort.ChildId = childId;
			escort.GetBehaviors();

			if (!escort.Valid)
				return null;

			escort.Offset = offset;
			return escort;

		}

		public void ClearParentEscort() {

			if (_behavior.BehaviorSettings.ParentEscort == null)
				return;

			var parentEscort = _behavior.BehaviorSettings.ParentEscort;
			parentEscort.Valid = false;

			if (parentEscort.ParentBehavior?.BehaviorSettings?.ActiveEscorts != null) {

				for (int i = parentEscort.ParentBehavior.BehaviorSettings.ActiveEscorts.Count - 1; i >= 0; i--) {

					var escort = parentEscort.ParentBehavior.BehaviorSettings.ActiveEscorts[i];

					if (escort == null || escort.ChildId == _behavior.RemoteControl.EntityId) {

						parentEscort.ParentBehavior.BehaviorSettings.ActiveEscorts.RemoveAt(i);

					}

				}

			}

			_behavior.BehaviorSettings.ParentEscort = null;

			var controller = _behavior.RemoteControl as MyShipController;

			if (controller != null && controller.RelativeDampeningEntity != null)
				controller.RelativeDampeningEntity = null;

		}

	}

}
