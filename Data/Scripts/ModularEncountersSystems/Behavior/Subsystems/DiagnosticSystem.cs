using ModularEncountersSystems.Helpers;
using ModularEncountersSystems.Logging;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModularEncountersSystems.Behavior.Subsystems {

	public class DiagnosticOutput {

		public string Output;

		public DiagnosticOutput() {

			Output = "";

		}
	
	}

	public class DiagnosticSystem {

		private IMyRemoteControl _remoteControl;
		private IBehavior _behavior;
		private DiagnosticOutput _output;

		private string _diagnosticLogFileName;

		

		public DiagnosticSystem(IBehavior behavior, IMyRemoteControl remoteControl) {

			_remoteControl = remoteControl;
			_behavior = behavior;
			_output = new DiagnosticOutput();

			if (remoteControl == null || behavior == null)
				return;

			_diagnosticLogFileName = LoggerTools.GetDateAndTime() + " - " + _remoteControl.SlimBlock.CubeGrid.CustomName + "-" + _remoteControl.SlimBlock.CubeGrid.CustomName + ".log";

		}

		public void UpdateLogFile() {

			try {

				using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage(_diagnosticLogFileName, typeof(IBehavior))) {

					//_output.Output = ;
					writer.Write(_behavior);
					//SpawnLogger.Write("Test", SpawnerDebugEnum.Error, true);

				}

			} catch (Exception e) {

				SpawnLogger.Write(e.ToString(), SpawnerDebugEnum.Error, true);
			
			}
		
		}

	}

}
