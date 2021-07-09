using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Logging;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;

namespace ModularEncountersSystems.Sync {

	public class ChatSoundData {

		public string SoundId;
		public string Avatar;
		public float VolumeMultiplier;

		public ChatSoundData(string soundId, string avatar, float volume) {

			SoundId = soundId;
			Avatar = avatar;
			VolumeMultiplier = volume;

		}
	
	} 

	public static class EffectManager {

		public static bool SoundsPending = false;
		public static bool SoundsPlaying = false;
		public static List<ChatSoundData> SoundsPendingList = new List<ChatSoundData>();

		public static IMyEntity CurrentPlayerEntity;
		public static MyEntity3DSoundEmitter SoundEmitter;
		public static bool GotFirstEmitter;
		public static string CurrentAvatar;

		public static void ClientReceiveEffect(Effects effectData) {

			if(effectData.Mode == EffectSyncMode.PlayerSound) {
				

				SoundsPendingList.Add(new ChatSoundData(effectData.SoundId, effectData.AvatarId, effectData.SoundVolume));
				SoundsPending = true;

			}

			if(effectData.Mode == EffectSyncMode.PositionSound) {

				//Logger.Write("Process Position Sound");
				ProcessPositionSoundEffect(effectData);

			}

			if (effectData.Mode == EffectSyncMode.Particle) {

				//Logger.Write("Process Particle");
				ProcessParticleEffect(effectData);

			}

		}

		public static void SendParticleEffectRequest(string id, MatrixD remoteMatrix, Vector3D offset, float scale, float maxTime, Vector3D color) {

			var effect = new Effects();
			effect.Mode = EffectSyncMode.Particle;
			effect.Coords = Vector3D.Transform(offset, remoteMatrix);
			effect.ParticleId = id;
			effect.ParticleScale = scale;
			effect.ParticleColor = color;
			effect.ParticleMaxTime = maxTime;
			effect.ParticleForwardDir = remoteMatrix.Forward;
			effect.ParticleUpDir = remoteMatrix.Up;
			var syncData = new SyncContainer(effect);

			foreach (var player in PlayerManager.Players) {

				if(player.ActiveEntity() && player.Distance(effect.Coords) <= 15000)
					SyncManager.SendSyncMesage(syncData, player.Player.SteamUserId);

			}

		}

		public static void ProcessParticleEffect(Effects effectData) {

			MyParticleEffect effect;
			var particleMatrix = MatrixD.CreateWorld(effectData.Coords, effectData.ParticleForwardDir, effectData.ParticleUpDir);
			var particleCoords = effectData.Coords;

			if (MyParticlesManager.TryCreateParticleEffect(effectData.ParticleId, ref particleMatrix, ref particleCoords, uint.MaxValue, out effect) == false) {

				return;

			}

			effect.UserScale = effectData.ParticleScale;

			if (effectData.ParticleMaxTime > 0) {

				//effect.DurationMin = effectData.ParticleMaxTime;
				//effect.DurationMax = effectData.ParticleMaxTime;

			}

			if (effectData.ParticleColor != Vector3D.Zero) {

				//var newColor = new Vector4((float)effectData.ParticleColor.X, (float)effectData.ParticleColor.Y, (float)effectData.ParticleColor.Z, 1);
				//effect.UserColorMultiplier = newColor;

			}

			effect.Velocity = effectData.Velocity;
			//effect.Loop = false;

		}

		public static void ProcessPlayerSoundEffect() {

			if(SoundsPending == false) {

				return;

			}

			if(CheckPlayerSoundEmitter() == false) {

				return;

			}

			if (SoundEmitter.IsPlaying == true) {

				ProcessAvatarDisplay();
				return;

			} else if(SoundsPendingList.Count > 0) {

				var soundPair = new MySoundPair(SoundsPendingList[0].SoundId);
				SoundEmitter.VolumeMultiplier = SoundsPendingList[0].VolumeMultiplier;
				SoundEmitter.PlaySound(soundPair, false, false, true, true, false);
				SoundsPlaying = true;
				SoundsPendingList.RemoveAt(0);

			}
			
			if(SoundsPendingList.Count == 0){

				SoundsPlaying = false;
				SoundsPending = false;
				ProcessAvatarDisplay();
				return;
			
			}

		}

		public static void ProcessAvatarDisplay() {

			if (SoundEmitter == null)
				return;

			if (!SoundEmitter.IsPlaying) {

				SoundsPlaying = false;
				return;

			}

			//Do Something With This Later

		}

		public static bool CheckPlayerSoundEmitter() {

			if(MyAPIGateway.Session.LocalHumanPlayer?.Controller?.ControlledEntity?.Entity == null) {

				CurrentPlayerEntity = null;
				SoundEmitter = null;
				SoundsPlaying = false;

				if (GotFirstEmitter) {

					SoundsPendingList.Clear();
					SoundsPending = false;
				
				}

				return false;

			}

			if(MyAPIGateway.Session.LocalHumanPlayer.Controller.ControlledEntity.Entity == CurrentPlayerEntity) {

				return true;

			}

			CurrentPlayerEntity = MyAPIGateway.Session.LocalHumanPlayer.Character;
			SoundEmitter = new MyEntity3DSoundEmitter(CurrentPlayerEntity as MyEntity);
			GotFirstEmitter = true;
			return true;

		}

		public static void ProcessPositionSoundEffect(Effects effectData) {

			MyVisualScriptLogicProvider.PlaySingleSoundAtPosition(effectData.SoundId, effectData.Coords);

		}

	}
}
