using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Serialization;

namespace ModularEncountersSystems.Spawning.Manipulation {
	public static class StorageTools {

		//Guid Keychain (har har har)
		public static Guid CustomDataKey = new Guid("74de02b3-27f9-4960-b1c4-27351f2b06d1");
		public static Guid RivalAiBehaviorDataKey = new Guid("96DFB932-7756-4A79-9A61-49A89C6D624A");
		public static Guid RivalAiStoredDataKey = new Guid("87EBD383-DF97-454F-9022-92423426E5C9");
		public static Guid NpcDataKey = new Guid("29FFD684-13D7-4045-BF76-CD48BF80E36A");
		public static Guid LegacyNpcDataKey = new Guid("AD4DBD09-359D-48F5-9F48-54D352B59171");
		public static Guid NpcThrusterDataKey = new Guid("BF529D66-9AA1-4419-B5D3-A65BA21306CE");

		public static void ApplyCustomBlockStorage(MyObjectBuilder_CubeBlock block, Guid storageKey, string storageValue) {

			if (block.ComponentContainer == null) {

				block.ComponentContainer = new MyObjectBuilder_ComponentContainer();

			}

			if (block.ComponentContainer.Components == null) {

				block.ComponentContainer.Components = new List<MyObjectBuilder_ComponentContainer.ComponentData>();

			}

			bool foundModStorage = false;

			foreach (var component in block.ComponentContainer.Components) {

				if (component.TypeId != "MyModStorageComponentBase") {

					continue;

				}

				var storage = component.Component as MyObjectBuilder_ModStorageComponent;

				if (storage == null) {

					continue;

				}

				foundModStorage = true;

				if (storage.Storage.Dictionary.ContainsKey(storageKey) == true) {

					storage.Storage.Dictionary[storageKey] = storageValue;

				} else {

					storage.Storage.Dictionary.Add(storageKey, storageValue);

				}

			}

			if (foundModStorage == false) {

				var modStorage = new MyObjectBuilder_ModStorageComponent();
				var dictA = new Dictionary<Guid, string>();
				dictA.Add(storageKey, storageValue);
				var dictB = new SerializableDictionary<Guid, string>(dictA);
				modStorage.Storage = dictB;
				var componentData = new MyObjectBuilder_ComponentContainer.ComponentData();
				componentData.TypeId = "MyModStorageComponentBase";
				componentData.Component = modStorage;
				block.ComponentContainer.Components.Add(componentData);

			}

		}

		public static void ApplyCustomGridStorage(MyObjectBuilder_CubeGrid grid, Guid storageKey, string storageValue) {

			if (grid.ComponentContainer == null) {

				grid.ComponentContainer = new MyObjectBuilder_ComponentContainer();

			}

			if (grid.ComponentContainer.Components == null) {

				grid.ComponentContainer.Components = new List<MyObjectBuilder_ComponentContainer.ComponentData>();

			}

			bool foundModStorage = false;

			foreach (var component in grid.ComponentContainer.Components) {

				if (component.TypeId != "MyModStorageComponentBase") {

					continue;

				}

				var storage = component.Component as MyObjectBuilder_ModStorageComponent;

				if (storage == null) {

					continue;

				}

				foundModStorage = true;

				if (storage.Storage.Dictionary.ContainsKey(storageKey) == true) {

					storage.Storage.Dictionary[storageKey] = storageValue;

				} else {

					storage.Storage.Dictionary.Add(storageKey, storageValue);

				}

			}

			if (foundModStorage == false) {

				var modStorage = new MyObjectBuilder_ModStorageComponent();
				var dictA = new Dictionary<Guid, string>();
				dictA.Add(storageKey, storageValue);
				var dictB = new SerializableDictionary<Guid, string>(dictA);
				modStorage.Storage = dictB;
				var componentData = new MyObjectBuilder_ComponentContainer.ComponentData();
				componentData.TypeId = "MyModStorageComponentBase";
				componentData.Component = modStorage;
				grid.ComponentContainer.Components.Add(componentData);

			}

		}

	}
}
