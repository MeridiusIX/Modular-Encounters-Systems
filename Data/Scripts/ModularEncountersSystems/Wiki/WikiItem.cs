using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Wiki {
	public class WikiItem {

		public string Name;
		public string Category;
		public string SubCategory;
		public string CompatibleWith;
		public string ValueType;
		public string Description;
		public string DefaultValue;
		public string MinPair;
		public string MaxPair;
		public double MinValue;
		public double MaxValue;

		public WikiItem() {

			Name = "";
			Category = "";
			SubCategory = "";
			CompatibleWith = "";
			ValueType = "";
			Description = "";
			DefaultValue = "";
			MinPair = "";
			MaxPair = "";
			MinValue = -1;
			MaxValue = -1;
		
		}

		public WikiItem(string name = "", string category = "", string subCategory = "", string compatibleWith = "", string valueType = "", string description = "", string defaultValue = "", string minPair = "", string maxPair = "", double minValue = -1, double maxValue = -1) {

			Name = name;
			Category = category;
			SubCategory = subCategory;
			CompatibleWith = compatibleWith;
			ValueType = valueType;
			Description = description;
			DefaultValue = defaultValue;
			MinPair = minPair;
			MaxPair = maxPair;
			MinValue = minValue;
			MaxValue = maxValue;

		}

		public string GenerateWikiItem() {

			var sb = new StringBuilder();

			return sb.ToString();
		
		}

	}

}
