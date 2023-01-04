using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace hudNavigation
{
    public class HNSSceneConfiguration : ScriptableObject
    {
        // OVERRIDES
        public bool overrideRadarSettings = false;


		// RADAR
		public bool useRadar = true;
		public RadarModes radarMode = RadarModes.RotateRadar;
		public float radarZoom = 1f;
		public float radarRadius = 50f;
		public float radarMaxRadius = 75f;
		public bool useRadarHeightSystem = true;
		public float radarDistanceAbove = 10f;
		public float radarDistanceBelow = 10f;
	}
}

