using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace ModularEncountersSystems.Behavior.Subsystems {

	public class OwnerSystem{
		
		public IMyRemoteControl RemoteControl;
		public string RequiredFactionTag;
		public bool NpcOwned;
		public bool WasNpcOwned;
		public bool AllowHumansInFaction;

		public IMyFaction Faction;
		public long FactionId;
		
		public bool UseGridReclamation;
		public double SecondsBetweenAttempts;
		public int ReclamationTimer;
		public int ReclamationTimerTrigger;
		
		public Random Rnd;
		
		public OwnerSystem(IMyRemoteControl remoteControl = null) {
			
			RemoteControl = null;
			RequiredFactionTag = "";
			NpcOwned = false;
			WasNpcOwned = false;
			AllowHumansInFaction = false;

			UseGridReclamation = false;
			SecondsBetweenAttempts = 60;
			ReclamationTimer = 0;
			
			Rnd = new Random();

			Setup(remoteControl);


		}
		
		private void Setup(IMyRemoteControl remoteControl){
			
			if(remoteControl == null){
				
				BehaviorLogger.Write("OwnerSystem Could Not Init. RemoteControl null", BehaviorDebugEnum.Owner);
				return;
				
			}
			
			remoteControl.OwnershipChanged += CheckIfNpcOwned;
			this.RemoteControl = remoteControl;
			CheckIfNpcOwned(remoteControl);
			
		}

		public void InitTags() {



		}

		public void ChangeRequiredFaction(string newFaction){
			
			this.RequiredFactionTag = newFaction;
			CheckIfNpcOwned(this.RemoteControl);
			
		}
		
		public void CheckIfNpcOwned(IMyTerminalBlock block){

			var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(block.OwnerId);

			if (faction == null) {

				BehaviorLogger.Write("Faction Null While Using TryGetPlayerFaction. Attempting Advanced Faction Location", BehaviorDebugEnum.Owner);
				var factions = MyAPIGateway.Session.Factions.Factions;

				foreach (var fct in factions.Keys) {

					bool skip = false;

					foreach (var member in factions[fct].Members.Keys) {

						if (factions[fct].Members[member].PlayerId == block.OwnerId) {

							faction = factions[fct];
							BehaviorLogger.Write("Got Faction With Advanced Find: " + faction.Tag, BehaviorDebugEnum.Owner);
							skip = true;
							break;
						
						}
					
					}

					if (skip)
						break;
				
				}

			}

			if (faction != null) {

				if (faction.Tag != this.RequiredFactionTag && string.IsNullOrEmpty(this.RequiredFactionTag) == false) {

					this.NpcOwned = false;
					BehaviorLogger.Write("Owner Check: Incorrect Faction Tag", BehaviorDebugEnum.Owner);
					return;

				}

				if (this.AllowHumansInFaction == false) {

					if (faction.IsEveryoneNpc() == true) {

						this.NpcOwned = true;
						this.WasNpcOwned = true;
						this.Faction = faction;
						this.FactionId = faction.FactionId;
						BehaviorLogger.Write("Owner Check: Valid NPC Faction", BehaviorDebugEnum.Owner);
						var npcSteam = MyAPIGateway.Players.TryGetSteamId(block.OwnerId);

						if (npcSteam != 0) {

							BehaviorLogger.Write("Warning. NPC Identity: " + block.OwnerId.ToString() + " has a SteamId of: " + npcSteam.ToString() + " - Please Alert Mod Author", BehaviorDebugEnum.Error);

						}

						return;

					} else {

						bool hasHuman = false;
						var identities = new List<IMyIdentity>();
						MyAPIGateway.Players.GetAllIdentites(identities);

						for (int i = faction.Members.Keys.Count() - 1; i >= 0; i--) {

							long member = faction.Members.Keys.ElementAt(i);
							var isNpc = FactionHelper.IsIdentityNPC(member);

							if (isNpc)
								continue;

							for (int j = identities.Count - 1; j >= 0; j--) {

								var identity = identities[j];

								if (identity.IdentityId == member && !string.IsNullOrWhiteSpace(identity.DisplayName)) {

									hasHuman = true;
									break;

								}

							}

							if (hasHuman)
								break;

						}

						if (!hasHuman) {

							this.NpcOwned = true;
							this.WasNpcOwned = true;
							this.Faction = faction;
							this.FactionId = faction.FactionId;
							BehaviorLogger.Write("Owner Check: Valid NPC Faction", BehaviorDebugEnum.Owner);
							var npcSteam = MyAPIGateway.Players.TryGetSteamId(block.OwnerId);

							if (npcSteam != 0) {

								BehaviorLogger.Write("Warning. NPC Identity: " + block.OwnerId.ToString() + " has a SteamId of: " + npcSteam.ToString() + " - Please Alert Mod Author", BehaviorDebugEnum.Error);

							}

							return;

						} else {

							BehaviorLogger.Write("Owner Check: Faction Contains Humans. Cannot Be Used By MES", BehaviorDebugEnum.Owner);

						}

					}

				} else {

					var npcSteam = MyAPIGateway.Players.TryGetSteamId(block.OwnerId);

					if (npcSteam == 0) {

						this.NpcOwned = true;
						this.WasNpcOwned = true;
						this.Faction = faction;
						this.FactionId = faction.FactionId;
						BehaviorLogger.Write("Owner Check: Valid NPC Faction", BehaviorDebugEnum.Owner);
						return;

					}

				}

			} else {

				BehaviorLogger.Write("Faction for provided OwnerId is Null", BehaviorDebugEnum.Owner);

			}

			//TODO: Maybe Update This To Include Factionless NPCs?
			var gridName = block?.SlimBlock?.CubeGrid?.CustomName ?? "Unnamed Grid";
			BehaviorLogger.Write("Owner Check: " + gridName + " Not NPC Faction", BehaviorDebugEnum.Owner);
			this.Faction = null;
			this.FactionId = 0;
			this.NpcOwned = false;
			
		}
		
		public void GridReclamation(){
			
			if(this.UseGridReclamation == false){
				
				return;
				
			}
				
			this.ReclamationTimer++;
			
			if(this.ReclamationTimer < this.SecondsBetweenAttempts){
				
				return;
				
			}
			
			this.ReclamationTimer = 0;
			
			if(this.RemoteControl?.SlimBlock?.CubeGrid == null){
				
				return;
				
			}
			
			var blockList = new List<IMySlimBlock>();
			RemoteControl.SlimBlock.CubeGrid.GetBlocks(blockList);
			var unownedBlocks = new List<IMyCubeBlock>();
			
			foreach(var block in blockList){
				
				if(block.FatBlock == null){
					
					continue;
					
				}
				
				if((block.BlockDefinition as MyCubeBlockDefinition).OwnershipIntegrityRatio == 0){
					
					continue;
					
				}
				
				if(block.FatBlock.OwnerId != this.RemoteControl.OwnerId){
					
					unownedBlocks.Add(block.FatBlock);
					
				}
				
			}
			
			if(unownedBlocks.Count > 0){
				
				var randBlock = unownedBlocks[Rnd.Next(0, unownedBlocks.Count)];
				var blockEntity = randBlock as IMyEntity;
				var myCubeBlock = blockEntity as MyCubeBlock;
				myCubeBlock.ChangeBlockOwnerRequest(this.RemoteControl.OwnerId, MyOwnershipShareModeEnum.Faction);
				
			}

		}
		
	}
	
}