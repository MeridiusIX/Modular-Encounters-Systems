using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.ModAPI;
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

		public MyObjectBuilder_Datapad BuildDatapad() {

			MyObjectBuilder_Datapad datapad = new MyObjectBuilder_Datapad();
			datapad.Name = GetTitle();
			datapad.Data = GetBody();
			datapad.SubtypeName = "Datapad";
			return datapad;

		}

		public string GetTitle() {

			return TextTemplate.CleanString(DataPadTitle);

		}

		public string GetBody() {

			return TextTemplate.CleanString(DataPadBody);

		}

		

	}

	public struct LcdEntry {

		public int TextSurfaceIndex;

		public bool ApplyLcdText;
		public string LcdText;

		public bool ApplyLcdImage;
		public string[] LcdImages;
		public float LcdImageChangeDelay;

		public LcdEntry(bool dummy = false) {

			TextSurfaceIndex = -1;

			ApplyLcdText = false;
			LcdText = "";

			ApplyLcdImage = false;
			LcdImages = new string[] { };
			LcdImageChangeDelay = 1;

		}

		public void ApplyLcdContents(IMyTextSurfaceProvider provider) {

			if (provider == null)
				return;

			if (TextSurfaceIndex < 0) {

				for (int i = 0; i < provider.SurfaceCount; i++) {

					var panel = provider.GetSurface(i) as IMyTextSurface;

					if (panel == null)
						return;

					ApplyLcdContents(panel, i);

				}
			
			} else {

				var panel = provider.GetSurface(TextSurfaceIndex) as IMyTextSurface;

				if (panel == null)
					return;

				ApplyLcdContents(panel, TextSurfaceIndex);

			}
			
		}

		public void ApplyLcdContents(IMyTextSurface panel, int surfaceIndex) {

			if (ApplyLcdText) {

				panel.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
				panel.WriteText(TextTemplate.CleanString(LcdText));

			} else {

				panel.WriteText("");

			}

			if (ApplyLcdImage) {

				panel.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
				panel.ClearImagesFromSelection();
				panel.ChangeInterval = LcdImageChangeDelay;

				foreach (var image in LcdImages)
					panel.AddImageToSelection(image);

			}

		}
	
	}

	public class TextTemplate {

		public string Name;
		public string Title;
		public string Description;
		public string BlockName;
		public string CustomData;

		[XmlArrayItem("LcdEntry")]
		public LcdEntry[] LcdEntries;

		[XmlArrayItem("DataPadEntry")]
		public DataPadEntry[] DataPadEntries;

		public TextTemplate() {

			Name = "";
			Title = "";
			Description = "";
			BlockName = "";
			CustomData = "";
			LcdEntries = new LcdEntry[] { };
			DataPadEntries = new DataPadEntry[] { };

		}

		public static string CleanString(string str) {

			var sb = new StringBuilder();
			char[] delims = new[] { '\r', '\n' };
			string[] strings = str.Split(delims, StringSplitOptions.RemoveEmptyEntries);

			foreach (var item in strings) {

				sb.Append(item?.Trim() ?? "").AppendLine();

			}

			return sb.ToString();

		}

	}

}
