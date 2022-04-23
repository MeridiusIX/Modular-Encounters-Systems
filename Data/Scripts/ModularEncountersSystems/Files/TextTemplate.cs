using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ModularEncountersSystems.Files {

	public struct DataPadEntry {

		public string DataPadTitle;
		public string DataPadBody;

		public DataPadEntry(string title, string body) {

			DataPadTitle = title;
			DataPadBody = body;

		}
	
	}

	public class TextTemplate {

		public string Name;
		public string Title;
		public string Description;
		public string BlockName;
		public string CustomData;

		[XmlArrayItem("LcdIndex")]
		public int[] LcdIndexes;

		[XmlArrayItem("LcdText")]
		public string[] LcdTexts;

		[XmlArrayItem("LcdTexture")]
		public string[] LcdTextures;

		[XmlArrayItem("DataPadEntry")]
		public string[] DataPadEntries;

		[XmlArrayItem("DataPadTitle")]
		public string[] DataPadTitles;

		[XmlArrayItem("DataPadBody")]
		public string[] DataPadBodies;

		public TextTemplate() {

			Name = "";
			Title = "";
			Description = "";
			BlockName = "";
			CustomData = "";
			LcdIndexes = new int[] { };
			LcdTexts = new string[] { };
			LcdTextures = new string[] { };
			DataPadTitles = new string[] { };
			DataPadBodies = new string[] { };

		}

	}

}
