using ModularEncountersSystems.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Behavior {
	public interface IBehaviorSubClass {

		BehaviorSubclass SubClass { get; set; }

		void InitTags();

		void ProcessBehavior();

		void SetDefaultTags();

	}
}
