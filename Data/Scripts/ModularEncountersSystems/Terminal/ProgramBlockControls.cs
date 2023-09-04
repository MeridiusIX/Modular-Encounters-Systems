using ModularEncountersSystems.Sync;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace ModularEncountersSystems.Terminal{
	public class ProgramBlockControls {

		public static void SpawnProgramBlockForControls() {

			var randomDir = MyUtils.GetRandomVector3D();
			var randomSpawn = randomDir * 1100000;
			var cubeGridOb = new MyObjectBuilder_CubeGrid();
			cubeGridOb.PersistentFlags = MyPersistentEntityFlags2.InScene;
			cubeGridOb.IsStatic = false;
			cubeGridOb.GridSizeEnum = MyCubeSize.Small;
			cubeGridOb.LinearVelocity = new Vector3(0, 0, 0);
			cubeGridOb.AngularVelocity = new Vector3(0, 0, 0);
			cubeGridOb.PositionAndOrientation = new MyPositionAndOrientation(randomSpawn, Vector3.Forward, Vector3.Up);
			var cubeBlockOb = new MyObjectBuilder_MyProgrammableBlock();
			cubeBlockOb.Min = new Vector3I(0, 0, 0);
			cubeBlockOb.SubtypeName = "SmallProgrammableBlock";
			cubeBlockOb.EntityId = 0;
			cubeBlockOb.Owner = 0;
			cubeBlockOb.BlockOrientation = new SerializableBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Up);
			cubeGridOb.CubeBlocks.Add(cubeBlockOb);
			MyAPIGateway.Entities.RemapObjectBuilder(cubeGridOb);
			var entitySmall = MyAPIGateway.Entities.CreateFromObjectBuilderAndAdd(cubeGridOb);

			var customControlBool = MyAPIGateway.TerminalControls.CreateProperty<bool, IMyProgrammableBlock>("MES-SendChatCommand");
			customControlBool.Enabled = Block => true;
			customControlBool.Getter = Block => {

				var steamId = MyAPIGateway.Players.TryGetSteamId(Block.OwnerId);

				if (steamId > 0) {

					if (MyAPIGateway.Session.IsUserAdmin(steamId)) {

						var cubeBlock = Block as MyCubeBlock;

						if (cubeBlock?.IDModule != null && cubeBlock.IDModule.ShareMode == MyOwnershipShareModeEnum.None) {

							var data = new ChatMessage();
							data.Message = Block.CustomData.Trim();
							data.PlayerId = Block.OwnerId;
							data.PlayerPosition = Block.GetPosition();
							data.SteamId = steamId;
							data.IsAdmin = true;
							ChatManager.ProcessServerChat(data);
							return true;

						} else
							Block.CustomData = "Block Sharing Must Be 'No Share'.";

					} else
						Block.CustomData = "Block Owner Not Admin.";

				} else
					Block.CustomData = "Could Not Get ID From Block Owner.";

				return false;

			};
			MyAPIGateway.TerminalControls.AddControl<IMyProgrammableBlock>(customControlBool);

			entitySmall.Close();

		}

	}
}
