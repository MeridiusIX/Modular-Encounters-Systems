using ModularEncountersSystems.Entities;
using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Behavior.Subsystems.Trigger {
	public class CommandProfile {

		public string ProfileSubtypeId;
		public string CommandCode;

		public int CommandDelayTicks;

		public bool SingleRecipient;
		public bool ReturnToSender;
		public bool IgnoreAntennaRequirement;
		public bool IgnoreReceiverAntennaRequirement;
		public double Radius;
		public double MaxRadius;

		public bool SendTargetEntityId;
		public bool SendDamagerEntityId;
		public bool SendSelfAsTargetEntityId;
		public bool SendWaypoint;
		public bool SendGridValue;

		public bool RequestEscortSlot;

		public CommandTransmissionType TransmissionType;

		public bool MatchSenderReceiverOwners;

		public bool CheckRelationSenderReceiver;
		public RelationTypeEnum Relation;


		public string Waypoint;

		public CommandProfile() {

			ProfileSubtypeId = "";
			CommandCode = "";

			CommandDelayTicks = 0;

			SingleRecipient = false;
			ReturnToSender = false;
			IgnoreAntennaRequirement = false;
			IgnoreReceiverAntennaRequirement = false;
			Radius = 10000;
			MaxRadius = -1;

			SendTargetEntityId = false;
			SendDamagerEntityId = false;
			SendSelfAsTargetEntityId = false;
			SendWaypoint = false;
			SendGridValue = false;

			RequestEscortSlot = false;

			TransmissionType = CommandTransmissionType.None;

			MatchSenderReceiverOwners = true;

			CheckRelationSenderReceiver = false;
			Relation = RelationTypeEnum.None;


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

					//CommandDelayTicks
					if (tag.Contains("[CommandDelayTicks:") == true) {

						TagParse.TagIntCheck(tag, ref CommandDelayTicks);

					}

					//SingleRecipient
					if (tag.Contains("[SingleRecipient:") == true) {

						TagParse.TagBoolCheck(tag, ref SingleRecipient);

					}

					//ReturnToSender
					if (tag.Contains("[ReturnToSender:") == true) {

						TagParse.TagBoolCheck(tag, ref ReturnToSender);

					}

					//IgnoreAntennaRequirement
					if (tag.Contains("[IgnoreAntennaRequirement:") == true) {

						TagParse.TagBoolCheck(tag, ref IgnoreAntennaRequirement);

					}

					//IgnoreReceiverAntennaRequirement
					if (tag.Contains("[IgnoreReceiverAntennaRequirement:") == true) {

						TagParse.TagBoolCheck(tag, ref IgnoreReceiverAntennaRequirement);

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

					//SendSelfAsTargetEntityId
					if (tag.Contains("[SendSelfAsTargetEntityId:") == true) {

						TagParse.TagBoolCheck(tag, ref SendSelfAsTargetEntityId);

					}

					//SendDamagerEntityId
					if (tag.Contains("[SendDamagerEntityId:") == true) {

						TagParse.TagBoolCheck(tag, ref SendDamagerEntityId);

					}

					//SendWaypoint
					if (tag.Contains("[SendWaypoint:") == true) {

						TagParse.TagBoolCheck(tag, ref SendWaypoint);

					}

					//SendGridValue
					if (tag.Contains("[SendGridValue:") == true) {

						TagParse.TagBoolCheck(tag, ref SendGridValue);

					}

					//RequestEscortSlot
					if (tag.Contains("[RequestEscortSlot:") == true) {

						TagParse.TagBoolCheck(tag, ref RequestEscortSlot);

					}

					//TransmissionType
					if (tag.Contains("[TransmissionType:") == true) {

						TagParse.TagCommandTransmissionTypeEnumCheck(tag, ref TransmissionType);

					}

					//MatchSenderReceiverOwners
					if (tag.Contains("[MatchSenderReceiverOwners:") == true) {

						TagParse.TagBoolCheck(tag, ref MatchSenderReceiverOwners);

					}



					//CheckReputationSenderReceiver
					if (tag.Contains("[CheckRelationSenderReceiver:") == true)
					{

						TagParse.TagBoolCheck(tag, ref CheckRelationSenderReceiver);

					}

					//MinReputation
					if (tag.Contains("[Relation:") == true)
					{

						TagParse.TagTargetRelationEnumCheck(tag, ref Relation);

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
