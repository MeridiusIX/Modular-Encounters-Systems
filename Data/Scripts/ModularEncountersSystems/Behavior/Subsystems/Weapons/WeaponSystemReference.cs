using System;
using System.Collections.Generic;
using System.Text;
using ModularEncountersSystems.API;
using ModularEncountersSystems.Behavior.Subsystems.Profiles;
using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using ModularEncountersSystems.Spawning.Manipulation;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;
using static ModularEncountersSystems.API.WcApiDef;

namespace ModularEncountersSystems.Behavior.Subsystems.Weapons {
	public class WeaponSystemReference {

		public string ProfileSubtypeId;

		//Configurable - Enabled Weapons
		public bool UseStaticGuns;
		public bool UseTurrets;

		//Configurable - Static Weapons
		public double MaxStaticWeaponRange;
		public double WeaponMaxAngleFromTarget;
		public double WeaponMaxBaseDistanceTarget;

		//Configurable - Barrage Fire
		public bool UseBarrageFire;
		public int MaxFireRateForBarrageWeapons;

		//Configurable - Ammo Replenish
		public bool UseAmmoReplenish;
		public int AmmoReplenishClipAmount;
		public int MaxAmmoReplenishments;

		//Configurable - WeaponCore
		public bool UseAntiSmartWeapons;
		public bool AllowHomingWeaponMultiTargeting;
		public int MultiTargetCheckCooldown;

		public WeaponSystemReference() {

			ProfileSubtypeId = "";

			UseStaticGuns = false;
			UseTurrets = true;

			MaxStaticWeaponRange = 5000;
			WeaponMaxAngleFromTarget = 6;
			WeaponMaxBaseDistanceTarget = 20;

			UseBarrageFire = false;
			MaxFireRateForBarrageWeapons = 200;

			UseAmmoReplenish = true;
			AmmoReplenishClipAmount = 15;
			MaxAmmoReplenishments = 10;

			UseAntiSmartWeapons = false;
			AllowHomingWeaponMultiTargeting = false;
			MultiTargetCheckCooldown = 5;

		}

		public void InitTags(string data) {

			if (string.IsNullOrWhiteSpace(data) == false) {

				var descSplit = data.Split('\n');

				foreach (var tag in descSplit) {

					//UseStaticGuns
					if (tag.Contains("[UseStaticGuns:") == true) {

						TagParse.TagBoolCheck(tag, ref UseStaticGuns);

					}

					//UseTurrets
					if (tag.Contains("[UseTurrets:") == true) {

						TagParse.TagBoolCheck(tag, ref UseTurrets);

					}

					//MaxStaticWeaponRange
					if (tag.Contains("[MaxStaticWeaponRange:") == true) {

						TagParse.TagDoubleCheck(tag, ref MaxStaticWeaponRange);

					}

					//WeaponMaxAngleFromTarget
					if (tag.Contains("[WeaponMaxAngleFromTarget:") == true) {

						TagParse.TagDoubleCheck(tag, ref WeaponMaxAngleFromTarget);

					}

					//WeaponMaxBaseDistanceTarget
					if (tag.Contains("[WeaponMaxBaseDistanceTarget:") == true) {

						TagParse.TagDoubleCheck(tag, ref WeaponMaxBaseDistanceTarget);

					}

					//UseBarrageFire
					if (tag.Contains("[UseBarrageFire:") == true) {

						TagParse.TagBoolCheck(tag, ref UseBarrageFire);

					}

					//MaxFireRateForBarrageWeapons
					if (tag.Contains("[MaxFireRateForBarrageWeapons:") == true) {

						TagParse.TagIntCheck(tag, ref MaxFireRateForBarrageWeapons);

					}

					//UseAmmoReplenish
					if (tag.Contains("[UseAmmoReplenish:") == true) {

						TagParse.TagBoolCheck(tag, ref UseAmmoReplenish);

					}

					//AmmoReplenishClipAmount
					if (tag.Contains("[AmmoReplenishClipAmount:") == true) {

						TagParse.TagIntCheck(tag, ref AmmoReplenishClipAmount);

					}

					//MaxAmmoReplenishments
					if (tag.Contains("[MaxAmmoReplenishments:") == true) {

						TagParse.TagIntCheck(tag, ref MaxAmmoReplenishments);

					}

					//UseAntiSmartWeapons
					if (tag.Contains("[UseAntiSmartWeapons:") == true) {

						TagParse.TagBoolCheck(tag, ref UseAntiSmartWeapons);

					}

					//AllowHomingWeaponMultiTargeting
					if (tag.Contains("[AllowHomingWeaponMultiTargeting:") == true) {

						TagParse.TagBoolCheck(tag, ref AllowHomingWeaponMultiTargeting);

					}

					//MultiTargetCheckCooldown
					if (tag.Contains("[MultiTargetCheckCooldown:") == true) {

						TagParse.TagIntCheck(tag, ref MultiTargetCheckCooldown);

					}

				}

			}

		}

	}

}
