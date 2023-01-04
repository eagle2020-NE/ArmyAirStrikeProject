using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace hudNavigation
{
    public class HudNavigationSystem : MonoBehaviour
    {

        #region singleton Setting
        private static HudNavigationSystem _Instance;
        public static HudNavigationSystem Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType<HudNavigationSystem>();
                }
                return _Instance;
            }
        }
        void Awake()
        {
            // destroy duplicate instances
            if (_Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            _Instance = this;
        }
        #endregion


        #region Variables

        // REFERENCES
        public Transform player;
        public Camera Camera;
        public _RotationReference RotationReference = _RotationReference.Camera;
        public _UpdateMode UpdateMode = _UpdateMode.LateUpdate;

        // RADAR
        [Tooltip("Enable, if you want to use the radar feature.")]
        public bool useRadar = true;
        [Tooltip("Select the radar mode you want to use.")]
        public RadarModes radarMode = RadarModes.RotateRadar;
        [Tooltip("Define the radar zoom. Change value to zoom the radar. Set to 1 for default radar zoom.")]
        public float radarZoom = 1f;
        [Tooltip("Define the radar radius. Elements outside this radius will be displayed on the border of the radar.")]
        public float radarRadius = 50f;
        [Tooltip("Define the maximum radar radius. Elements outside this radius will be hidden.")]
        public float radarMaxRadius = 75f;
        [Tooltip("Enable, if you want to show arrows pointing upwards/downwards if the element is physically above or below a certain distance.")]
        public bool useRadarHeightSystem = true;
        [Tooltip("Minimum distance upwards to activate the element's ABOVE arrow.")]
        public float radarDistanceAbove = 10f;
        [Tooltip("Minimum distance downwards to activate the element's BELOW arrow.")]
        public float radarDistanceBelow = 10f;
        [Tooltip("(DEBUG) Enable to show the radar's height gizmos.")]
        public bool showRadarHeightGizmos = false;
        [SerializeField] protected Vector2 radarHeightGizmoSize = new Vector2(100f, 100f);
        [SerializeField] protected Color radarHeightGizmoColor = new Color(0f, 0f, 1f, .4f);



        [HideInInspector]
        public List<HUDNavigationElement> NavigationElements;

        private HudNavigationCanvas _HUDNavigationCanvas;
        private Transform _rotationReference;
        private RectTransform _targetLock;
        private HUDNavigationElement cachedElement;
        #endregion

        #region main method
        public void SetPlayer(Transform player)
        {
            this.player = player;
        }

        private void Start()
        {
            _targetLock = Resolver.Instance.TargetLock;
            Camera = Resolver.Instance.Camera;

            // assign references
            if (_HUDNavigationCanvas == null)
            {
                _HUDNavigationCanvas = HudNavigationCanvas.Instance;

                // check if HUDNavigationCanvas exists
                if (_HUDNavigationCanvas == null)
                {
                    Debug.LogError("HUDNavigationCanvas not found in scene!");
                    this.enabled = false;
                    return;
                }
            }


            InitAllComponents();
        }

        void Update()
        {
            if (player == null)
            {
                return;
            }

            if (UpdateMode == _UpdateMode.Update)
            {
                UpdateAllComponents();
            }
        }

        void LateUpdate()
        {
            if (player == null)
            {
                return;
            }

            if (UpdateMode == _UpdateMode.LateUpdate)
            {
                UpdateAllComponents();
            }
        }

        private void FixedUpdate()
        {
            if (player == null)
            {
                return;
            }

            if (UpdateMode == _UpdateMode.FixedUpdate)
            {
                UpdateAllComponents();
            }
        }

        #endregion

        public void AddNavigationElement(HUDNavigationElement element)
        {
            if (element == null)
                return;

            // add element, if it doesn't exist yet
            if (!NavigationElements.Contains(element))
                NavigationElements.Add(element);
        }


        public void RemoveNavigationElement(HUDNavigationElement element)
        {
            if (element == null)
                return;

            // remove element from list
            NavigationElements.Remove(element);
        }


        public void EnableRadar(bool value)
        {
            if (useRadar != value)
            {
                useRadar = value;
                _HUDNavigationCanvas.ShowRadar(value);
            }
        }

        void InitAllComponents()
        {
            if (_HUDNavigationCanvas == null)
                return;

            // init radar
            if (useRadar)
            {
                _HUDNavigationCanvas.InitRadar();

                // make sure max radius is greater than radius
                if (radarMaxRadius < radarRadius)
                    radarMaxRadius = radarRadius;
            }
            else
            {
                _HUDNavigationCanvas.ShowRadar(false);
            }
        }

        void UpdateAllComponents()
        {
            // update navigation elements
            UpdateNavigationElements();

            // get rotation reference
            _rotationReference = GetRotationReference();

            // update radar
            if (useRadar)
                _HUDNavigationCanvas.UpdateRadar(_rotationReference, radarMode);
        }

        void UpdateNavigationElements()
        {
            if (_HUDNavigationCanvas == null || NavigationElements.Count <= 0)
                return;

            cachedElement = null;

            // update navigation elements
            for (var i = 0; i < NavigationElements.Count; i++)
            {
                cachedElement = NavigationElements[i];

                if (cachedElement == null)
                    continue;

                // check if element is active
                if (!cachedElement.IsActive)
                {
                    // disable all marker instances
                    cachedElement.SetMarkerActive(NavigationElementType.Radar, false);

                    // skip the element
                    continue;
                }

                // cache element values
                Vector3 _worldPos = cachedElement.GetPosition();
                Vector3 _screenPos = Camera.WorldToScreenPoint(_worldPos);
                float _distance = cachedElement.GetDistance(player);

                // update radar
                if (useRadar && cachedElement.Radar != null)
                    UpdateRadarElement(cachedElement, _screenPos, _distance);
            }
        }
        public Transform GetRotationReference()
        {
            return (RotationReference == _RotationReference.Camera) ? Camera.transform : player;
        }


        #region Radar Methods
        void UpdateRadarElement(HUDNavigationElement element, Vector3 screenPos, float distance)
        {
            float _scaledRadius = radarRadius * radarZoom;
            float _scaledMaxRadius = radarMaxRadius * radarZoom;

            // check if element is hidden within the radar
            if (element.hideInRadar)
            {
                element.SetMarkerActive(NavigationElementType.Radar, false);
                return;
            }

            // check distance
            if (distance > _scaledRadius)
            {
                // invoke events
                if (element.IsInRadarRadius)
                {
                    element.IsInRadarRadius = false;
                    element.OnLeaveRadius.Invoke(element, NavigationElementType.Radar);
                }

                // check max distance
                if (distance > _scaledMaxRadius && !element.ignoreRadarRadius)
                {
                    element.SetMarkerActive(NavigationElementType.Radar, false);
                    return;
                }

                // set scaled distance when out of range
                distance = _scaledRadius;
            }
            else
            {
                // invoke events
                if (!element.IsInRadarRadius)
                {
                    element.IsInRadarRadius = true;
                    element.OnEnterRadius.Invoke(element, NavigationElementType.Radar);
                }
            }

            // rotate marker within radar with gameobject?
            Transform rotationReference = GetRotationReference();
            if (radarMode == RadarModes.RotateRadar)
            {
                element.Radar.PrefabRect.rotation = Quaternion.identity;
                if (element.rotateWithGameObject)
                    element.Radar.Icon.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -element.transform.eulerAngles.y + rotationReference.eulerAngles.y));
            }
            else
            {
                if (element.rotateWithGameObject)
                    element.Radar.Icon.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -element.transform.eulerAngles.y));
            }

            // keep marker icon identity rotation?
            if (!element.rotateWithGameObject)
                element.Radar.Icon.transform.rotation = Quaternion.identity;

            // set marker active
            element.SetMarkerActive(NavigationElementType.Radar, true);

            // calculate marker position
            Vector3 posOffset = element.GetPositionOffset(player.position);
            Vector3 markerPos = new Vector3(posOffset.x, posOffset.z, 0f);
            markerPos.Normalize();
            markerPos *= (distance / _scaledRadius) * (_HUDNavigationCanvas.Radar.ElementContainer.GetRadius() - element.GetIconRadius(NavigationElementType.Radar));

            // set marker position
            element.SetMarkerPosition(NavigationElementType.Radar, markerPos);

            // handle marker's above/below arrows
            element.ShowRadarAboveArrow(useRadarHeightSystem && element.useRadarHeightSystem && element.IsInRadarRadius && -posOffset.y < -radarDistanceAbove);
            element.ShowRadarBelowArrow(useRadarHeightSystem && element.useRadarHeightSystem && element.IsInRadarRadius && -posOffset.y > radarDistanceBelow);
        }
        #endregion

        #region Configuration Methods
        /// <summary>
        /// Applies a new scene configuration.
        /// </summary>
        /// <param name="config">Scene Configuration.</param>
        public void ApplySceneConfiguration(HNSSceneConfiguration config)
        {
            if (config == null)
                return;

            // override radar settings
            if (config.overrideRadarSettings)
            {
                // radar usage changed?
                if (_HUDNavigationCanvas != null && this.useRadar != config.useRadar)
                    _HUDNavigationCanvas.ShowRadar(config.useRadar);

                this.useRadar = config.useRadar;
                this.radarMode = config.radarMode;
                this.radarZoom = config.radarZoom;
                this.radarRadius = config.radarRadius;
                this.radarMaxRadius = config.radarMaxRadius;
                this.useRadarHeightSystem = config.useRadarHeightSystem;
                this.radarDistanceAbove = config.radarDistanceAbove;
                this.radarDistanceBelow = config.radarDistanceBelow;
            }
        }
        #endregion


    }



    #region Enums

    [System.Serializable]
    public enum _RotationReference
    {
        Camera, Controller
    }


    [System.Serializable]
    public enum _UpdateMode
    {
        Update, LateUpdate,
        FixedUpdate
    }

    public enum RadarModes { RotateRadar, RotatePlayer };
    #endregion
}

