using ModularEncountersSystems.Files;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Spawning.Profiles {
	public class StoreProfile {

		public string ProfileSubtypeId;
		public bool SetupComplete;

		public string FileSource;

		public int MinOfferItems;
		public int MaxOfferItems;

		public int MinOrderItems;
		public int MaxOrderItems;

		public List<string> Offers;
		public List<string> Orders;

		public List<StoreItem> OfferItems;
		public List<StoreItem> OrderItems;

		public StoreProfile() {

			ProfileSubtypeId = "";
			SetupComplete = false;

			FileSource = "";

			MinOfferItems = 10;
			MaxOfferItems = 10;

			MinOrderItems = 10;
			MaxOrderItems = 10;

			Offers = new List<string>();
			Orders = new List<string>();

			OfferItems = new List<StoreItem>();
			OrderItems = new List<StoreItem>();

		}

		public void Setup() {

			if (SetupComplete)
				return;
		
		}

	}
}
