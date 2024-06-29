using UnityEngine;

using UnityEditor;

namespace NullrefLib.Unity.Editor {
	public static class LightingUtilities {
		public static bool CanBake => (!Lightmapping.isRunning) &&
			(Lightmapping.giWorkflowMode == Lightmapping.GIWorkflowMode.OnDemand);

		[MenuItem("Tools/Nullref Tools/Lighting/Bake lights", false, 0)]
		public static void BakeLights() {
			if (Lightmapping.isRunning)
				return;

			Lightmapping.BakeAsync();
		}

		[MenuItem("Tools/Nullref Tools/Lighting/Bake lights", true, 0)]
		public static bool CanBakeValidate() => CanBake;

		//------------------

		[MenuItem("Tools/Nullref Tools/Lighting/Stop baking", false, 1)]
		public static void StopBake() {
			if (!Lightmapping.isRunning)
				return;

			Lightmapping.Cancel();
			if (Lightmapping.isRunning)
				Lightmapping.ForceStop();
		}

		[MenuItem("Tools/Nullref Tools/Lighting/Stop baking", true, 1)]
		public static bool StopBakeValidate() => Lightmapping.isRunning;

		[MenuItem("Tools/Nullref Tools/Lighting/Toggle Auto-Generate", false, 2)]
		public static void ToggleAutoBake() {
			if (Lightmapping.giWorkflowMode == Lightmapping.GIWorkflowMode.OnDemand)
			{
				Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.Iterative;
				Debug.Log("Baking mode set to Auto-Generate.");
			} else
			{
				Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.OnDemand;
				Debug.Log("Baking mode set to Manual.");
			}
		}
	}
}