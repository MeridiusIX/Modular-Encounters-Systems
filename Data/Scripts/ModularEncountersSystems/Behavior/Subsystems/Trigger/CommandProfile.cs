using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger {
	public class CommandProfile {

		public string ProfileSubtypeId;
		public string CommandCode;

		public bool SingleRecipient;
		public bool IgnoreAntennaRequirement;
		public double Radius;
		public double MaxRadius;

		public bool SendTargetEntityId;
		public bool SendDamagerEntityId;
		public bool SendWaypoint;

		public bool MatchSenderReceiverOwners;

		public string Waypoint;

		public CommandProfile() {

			ProfileSubtypeId = "";
			CommandCode = "";

			SingleRecipient = false;
			IgnoreAntennaRequirement = false;
			Radius = 10000;
			MaxRadius = -1;

			SendTargetEntityId = false;
			SendDamagerEntityId = false;
			SendWaypoint = false;

			MatchSenderReceiverOwners = true;

			Waypoint = "";

		}

		public void InitTags(string tagData) {

			if (!string.IsNullOrWhiteSpace(tagData)) {

				var descSplit = tagData.Split('\n');

				foreach (var tag in descSplit) {

					//CommandCode
					if (tag.Contains("[CommandCode:") == true) {

						TagParse.TagStringCheck(tag, ref CommandCode);

					}

					//SingleRecipient
					if (tag.Contains("[SingleRecipient:") == true) {

						TagParse.TagBoolCheck(tag, ref SingleRecipient);

					}

					//IgnoreAntennaRequirement
					if (tag.Contains("[IgnoreAntennaRequirement:") == true) {

						TagParse.TagBoolCheck(tag, ref IgnoreAntennaRequirement);

					}

					//Radius
					if (tag.Contains("[Radius:") == true) {

						TagParse.TagDoubleCheck(tag, ref this.Radius);

					}

					//MaxRadius
					if (tag.Contains("[MaxRadius:") == true) {

						TagParse.TagDoubleCheck(tag, ref this.MaxRadius);

					}

					//SendTargetEntityId
					if (tag.Contains("[SendTargetEntityId:") == true) {

						TagParse.TagBoolCheck(tag, ref SendTargetEntityId);

					}

					//SendDamagerEntityId
					if (tag.Contains("[SendDamagerEntityId:") == true) {

						TagParse.TagBoolCheck(tag, ref SendDamagerEntityId);

					}

					//SendWaypoint
					if (tag.Contains("[SendWaypoint:") == true) {

						TagParse.TagBoolCheck(tag, ref SendWaypoint);

					}

					//MatchSenderReceiverOwners
					if (tag.Contains("[MatchSenderReceiverOwners:") == true) {

						TagParse.TagBoolCheck(tag, ref MatchSenderReceiverOwners);

					}

					//Waypoint
					if (tag.Contains("[Waypoint:") == true) {

						TagParse.TagStringCheck(tag, ref Waypoint);

					}

				}

			}

		}

	}

}
