using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Wiki {
	public struct WikiItem {

		public string Name; //Name of the Property or Tag
		public string Category; //Class or Profile it belongs to.
		public string SubCategory; //Subcategory on wiki page (eg, General, World, etc)
		public string CompatibleWith; //BehaviorsMainly for Autopilot
		public string ValueType; //Class or Struct Type. Eg: Bool, Integer. Also could be considered "Allowed Values"
		public string Description; //Description of the Property
		public string DefaultValue; //Default Value, if Applicable
		public string MinPair; //Minimum item name associated to paired Properties
		public string MaxPair; //Maximum item name associated to paired Properties
		public double? MinValue; //Min value for numerical values
		public double? MaxValue; //Max value for numerical values

		public WikiItem(string name = null, string category = null, string subCategory = null, string compatibleWith = null, string valueType = null, string description = null, string defaultValue = null, string minPair = null, string maxPair = null, double? minValue = null, double? maxValue = null) {

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

			if (string.IsNullOrWhiteSpace(Name))
				return "";

			var sb = new StringBuilder();



			return sb.ToString();

		
		}

	}

}
