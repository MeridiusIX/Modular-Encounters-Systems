using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Weapons;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Weapons;
using SpaceEngineers.Game.ModAPI;
using ProtoBuf;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;
using ModularEncountersSystems;
using ModularEncountersSystems.Behavior;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Behavior.Subsystems.Trigger;

namespace ModularEncountersSystems.Behavior.Subsystems {

	[Flags]
	public enum DamageReaction {

		None = 0,
		Evasion = 1,
		ChangeTarget = 2,
		BarrelRoll = 4,
		Alert = 8,
		CallReinforcements = 16

	}

	public class DamageSystem {

		//Configurable
		public bool UseDamageDetection;


		//Non-Configurable
		public IMyRemoteControl RemoteControl;
		public IMyCubeGrid CurrentCubeGrid;
		public List<IMyCubeGrid> CurrentGrids;
		public bool SetupHandler;

		private IBehavior _behavior;

		public Func<bool> IsRemoteWorking;
		private TriggerSystem _trigger;

		public DamageSystem(IMyRemoteControl remoteControl = null, IBehavior behavior = null) {

			UseDamageDetection = false;

			RemoteControl = null;
			CurrentGrids = new List<IMyCubeGrid>();
			SetupHandler = false;

			Setup(remoteControl);

		}

		private void Setup(IMyRemoteControl remoteControl) {

			if(remoteControl == null || MyAPIGateway.Entities.Exist(remoteControl?.SlimBlock?.CubeGrid) == false) {

				return;

			}

			this.RemoteControl = remoteControl;

		}

		public void SetupReferences(TriggerSystem trigger) {

			_trigger = trigger;

		}

		public void SetupDamageHandler() {

			if(this.UseDamageDetection == false || this.SetupHandler == true) {

				return;

			}

			this.SetupHandler = true;
			RegisterGridOnWatcher(this.RemoteControl?.SlimBlock?.CubeGrid);

		}

		public void DamageHandler(object target, MyDamageInformation info) {

			if(IsRemoteWorking != null && !IsRemoteWorking.Invoke())
				return;

			var block = target as IMySlimBlock;

			if(target == null || this.RemoteControl?.SlimBlock?.CubeGrid == null)
				return;

			if (!this.RemoteControl.SlimBlock.CubeGrid.IsSameConstructAs(block.CubeGrid))
				return;

			_trigger.ProcessDamageTriggerWatchers(target, info);

		}

		public void UnregisterDamageHandler() {

			DamageHelper.MonitoredGrids.Remove(this.CurrentCubeGrid);

			if(this.CurrentCubeGrid != null && MyAPIGateway.Entities.Exist(this.CurrentCubeGrid)) {

				this.CurrentCubeGrid.OnGridSplit -= GridSplit;

			}

		}

		public void GridSplit(IMyCubeGrid gridA, IMyCubeGrid gridB) {

			if (!this.CurrentGrids.Contains(gridA))
				this.CurrentGrids.Add(gridA);

			if (!this.CurrentGrids.Contains(gridB))
				this.CurrentGrids.Add(gridB);

			UnregisterGridOnWatcher();

			if(this.RemoteControl?.SlimBlock?.CubeGrid == null) {

				return;

			}

			this.CurrentCubeGrid = this.RemoteControl.SlimBlock.CubeGrid;
			RegisterGridOnWatcher(this.CurrentCubeGrid);

		}

		public void RegisterGridOnWatcher(IMyCubeGrid cubeGrid) {

			if(cubeGrid == null || MyAPIGateway.Entities.Exist(cubeGrid) == false) {

				return;

			}

			this.CurrentGrids.Clear();
			this.CurrentGrids = MyAPIGateway.GridGroups.GetGroup(cubeGrid, GridLinkTypeEnum.Mechanical);

			foreach(var grid in this.CurrentGrids) {

				grid.OnGridSplit += GridSplit;

				lock (DamageHelper.MonitoredGrids) {

					DamageHelper.MonitoredGrids.Add(grid);

				}

				if(!DamageHelper.RegisteredDamageHandlers.ContainsKey(grid)) {

					DamageHelper.DamageRelay += DamageHandler;

				}

			}

		}

		public void UnregisterGridOnWatcher() {

			foreach(var grid in this.CurrentGrids) {

				if(grid == null || MyAPIGateway.Entities.Exist(grid) == false) {

					return;

				}

				grid.OnGridSplit -= GridSplit;
				DamageHelper.MonitoredGrids.RemoveAll(g => g == grid);
				DamageHelper.DamageRelay -= DamageHandler;

			}

		}

		public void InitTags() {

			if(string.IsNullOrWhiteSpace(this.RemoteControl.CustomData) == false) {

				var descSplit = this.RemoteControl.CustomData.Split('\n');

				foreach(var tag in descSplit) {

					//UseStaticGuns
					if(tag.Contains("[UseDamageDetection:") == true) {

						TagParse.TagBoolCheck(tag, ref UseDamageDetection);

					}

				}

			}

		}

	}


}
