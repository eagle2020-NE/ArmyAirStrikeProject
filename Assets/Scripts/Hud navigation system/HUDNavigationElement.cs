using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace hudNavigation
{
    public class HUDNavigationElement : MonoBehaviour
    {
        #region variables

        public HNSPrefabs Prefabs = new HNSPrefabs();

        // RADAR SETTINGS
        public bool hideInRadar = false;
        public bool ignoreRadarRadius = false;
        public bool rotateWithGameObject = true;
        public bool useRadarHeightSystem = true;

        // EVENTS
        public NavigationElementEvent OnElementReady = new NavigationElementEvent();
        public NavigationTypeEvent OnAppear = new NavigationTypeEvent();
        public NavigationTypeEvent OnDisappear = new NavigationTypeEvent();
        public NavigationTypeEvent OnEnterRadius = new NavigationTypeEvent();
        public NavigationTypeEvent OnLeaveRadius = new NavigationTypeEvent();

        [HideInInspector]
        public bool IsActive = true;


        [HideInInspector]
        public HNSRadarPrefab Radar;

        [HideInInspector]
        public bool IsInRadarRadius;

        protected bool _isInitialized = false;
        #endregion


        #region main method

        void Start()
        {
            // disable, if navigation system is missing
            if (HudNavigationSystem.Instance == null)
            {
                Debug.LogError("HUDNavigationSystem not found in scene!");
                this.enabled = false;
                return;
            }

            // initialize settings
            InitializeSettings();

            // initialize components
            Initialize();
        }

        void OnEnable()
        {
            if (!_isInitialized)
                return;

            Initialize();
        }
        void OnDisable()
        {
            // remove element from the navigation system
            if (HudNavigationSystem.Instance != null)
                HudNavigationSystem.Instance.RemoveNavigationElement(this);

            // destroy all marker references
            if (Radar != null)
                Destroy(Radar.gameObject);
        }


        #endregion

        void InitializeSettings()
        {
            //if (Settings == null)
            //    return;

            //// misc
            //this.Prefabs = Settings.Prefabs;

            //// radar settings
            //this.hideInRadar = Settings.hideInRadar;
            //this.ignoreRadarRadius = Settings.ignoreRadarRadius;
            //this.rotateWithGameObject = Settings.rotateWithGameObject;
            //this.useRadarHeightSystem = Settings.useRadarHeightSystem;
        }

        void Initialize()
        {
            // create marker references
            CreateMarkerReferences();

            // add element to the navigation system
            if (HudNavigationSystem.Instance != null)
                HudNavigationSystem.Instance.AddNavigationElement(this);

            // set as initialized
            _isInitialized = true;

            // invoke events
            OnElementReady.Invoke(this);
        }

        void CreateMarkerReferences()
        {
            CreateRadarMarker();
        }

        void CreateRadarMarker()
        {
            if (Prefabs.RadarPrefab == null)
                return;

            // create radar gameobject
            GameObject radarGO = Instantiate(Prefabs.RadarPrefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
            radarGO.transform.SetParent(HudNavigationCanvas.Instance.Radar.ElementContainer, false);
            radarGO.SetActive(false);

            // assign radar prefab
            Radar = radarGO.GetComponent<HNSRadarPrefab>();
        }
    }



    #region subClass

    [System.Serializable]
	public class HNSPrefabs
	{
		public HNSRadarPrefab RadarPrefab;
	}

    [System.Serializable]
    public enum NavigationElementType { Radar, CompassBar, Indicator, Minimap };


    [System.Serializable]
    public class NavigationElementEvent : UnityEvent<HUDNavigationElement> { }


    [System.Serializable]
    public class NavigationTypeEvent : UnityEvent<HUDNavigationElement, NavigationElementType> { }
    #endregion
}

