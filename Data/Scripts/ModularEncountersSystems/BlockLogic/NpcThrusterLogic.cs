using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Manipulation;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using System;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;

namespace ModularEncountersSystems.BlockLogic {

	public class NpcThrusterLogic : BaseBlockLogic, IBlockLogic {

		public IMyThrust Thruster;
		public ThrustSettings Settings;
		public string SettingsString;

		public bool RestrictThrustUse = false;
		public float ThrustForceMultiplier = 1;
		public float ThrustPowerMultiplier = 1;

		public IMyCubeGrid OriginalGrid;

		public bool NpcOwned;

		public NpcThrusterLogic(BlockEntity block) {

			Setup(block);

		}

		internal override void Setup(BlockEntity block) {

			Thruster = block.Block as IMyThrust;
			OriginalGrid = Thruster.SlimBlock.CubeGrid;

			bool gotData = false;

			if (OriginalGrid.Storage != null) {

				if (OriginalGrid.Storage.TryGetValue(StorageTools.NpcThrusterDataKey, out SettingsString))
					gotData = GetSettings(SettingsString);

			}

			if (!gotData && Thruster.Storage != null && Thruster.Storage.TryGetValue(StorageTools.NpcThrusterDataKey, out SettingsString))
				gotData = GetSettings(SettingsString);

			if (!gotData) {

				_isValid = false;
				return;

			}

			var thrustDef = Thruster.SlimBlock.BlockDefinition as MyThrustDefinition;

			if (thrustDef?.ThrusterType == null) {

				_isValid = false;
				return;

			}

			var thrustType = thrustDef.ThrusterType.ToString();

			if (thrustType == "Atmospheric") {

				RestrictThrustUse = Settings.RestrictNpcAtmoThrust;
				ThrustForceMultiplier = Settings.NpcAtmoThrustForceMultiply;
				ThrustPowerMultiplier = Settings.NpcAtmoThrustPowerMultiply;

			}

			if (thrustType == "Hydrogen") {

				RestrictThrustUse = Settings.RestrictNpcHydroThrust;
				ThrustForceMultiplier = Settings.NpcHydroThrustForceMultiply;
				ThrustPowerMultiplier = Settings.NpcHydroThrustPowerMultiply;

			}

			if (thrustType == "Ion") {

				RestrictThrustUse = Settings.RestrictNpcIonThrust;
				ThrustForceMultiplier = Settings.NpcIonThrustForceMultiply;
				ThrustPowerMultiplier = Settings.NpcIonThrustPowerMultiply;

			}

			Thruster.SlimBlock.CubeGrid.OnBlockOwnershipChanged += GridOwnershipChange;
			Thruster.IsWorkingChanged += WorkingChanged;
			Thruster.SlimBlock.CubeGrid.OnGridSplit += GridSplit;
			GridOwnershipChange(Thruster.SlimBlock.CubeGrid);
			WorkingChanged(Thruster);

		}

		public void GridOwnershipChange(IMyCubeGrid grid) {

			NpcOwned = false;

			if (Thruster.MarkedForClose)
				return;

			if (grid.BigOwners != null && grid.BigOwners.Count > 0) {

				foreach (var owner in grid.BigOwners) {

					if (owner == 0)
						continue;

					if (!(MyAPIGateway.Players.TryGetSteamId(owner) > 0)) {

						NpcOwned = true;
						break;

					}

				}

			}

			if (NpcOwned) {

				Thruster.ThrustMultiplier = ThrustForceMultiplier;
				Thruster.PowerConsumptionMultiplier = ThrustPowerMultiplier;


			} else {

				Thruster.ThrustMultiplier = 1;
				Thruster.PowerConsumptionMultiplier = 1;

				if (RestrictThrustUse)
					Thruster.Enabled = false;

			}

		}

		public void WorkingChanged(IMyCubeBlock block) {

			if (Thruster.Enabled) {

				if (RestrictThrustUse && !NpcOwned)
					Thruster.Enabled = false;

			}

		}

		public void GridSplit(IMyCubeGrid a, IMyCubeGrid b) {

			if (Thruster.MarkedForClose || Thruster.SlimBlock.CubeGrid == OriginalGrid)
				return;

			a.OnBlockOwnershipChanged -= GridOwnershipChange;
			b.OnBlockOwnershipChanged -= GridOwnershipChange;

			a.OnGridSplit -= GridSplit;
			b.OnGridSplit -= GridSplit;

			if (Thruster.MarkedForClose)
				return;

			if (Thruster.Storage == null)
				Thruster.Storage = new MyModStorageComponent();

			if (Thruster.Storage.ContainsKey(StorageTools.NpcThrusterDataKey))
				Thruster.Storage[StorageTools.NpcThrusterDataKey] = SettingsString;
			else
				Thruster.Storage.Add(StorageTools.NpcThrusterDataKey, SettingsString);

			Thruster.SlimBlock.CubeGrid.OnBlockOwnershipChanged += GridOwnershipChange;
			GridOwnershipChange(Thruster.SlimBlock.CubeGrid);
			WorkingChanged(Thruster);

		}

		public bool GetSettings(string data) {

			try {

				var bytes = Convert.FromBase64String(data);

				if (bytes == null)
					return false;

				Settings = MyAPIGateway.Utilities.SerializeFromBinary<ThrustSettings>(bytes);

				if (Settings != null)
					return true;

				return false;

			} catch (Exception e) {

				return false;

			}

		}

		internal override void Unload(IMyEntity entity = null) {

			base.Unload(entity);

			if (Thruster != null) {

				Thruster.SlimBlock.CubeGrid.OnBlockOwnershipChanged -= GridOwnershipChange;
				Thruster.IsWorkingChanged -= WorkingChanged;
				Thruster.SlimBlock.CubeGrid.OnGridSplit -= GridSplit;

			}

		}

	}

}