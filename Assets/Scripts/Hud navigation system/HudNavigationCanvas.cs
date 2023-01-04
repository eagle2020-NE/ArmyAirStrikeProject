using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace hudNavigation
{
    public class HudNavigationCanvas : MonoBehaviour
    {
        private static HudNavigationCanvas _Instance;
        public static HudNavigationCanvas Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType<HudNavigationCanvas>();
                }
                return _Instance;
            }
        }


        void Awake()
        {
            if (_Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            _Instance = this;
        }


        [System.Serializable]
        public class _RadarReferences
        {
            public RectTransform Panel;
            public RectTransform Radar;
            public RectTransform PlayerIndicator;
            public RectTransform ElementContainer;
        }

        public _RadarReferences Radar;

        #region Radar Methods
        public void InitRadar()
        {
            // check references
            if (Radar.Panel == null || Radar.Radar == null || Radar.ElementContainer == null)
            {
                ReferencesMissing("Radar");
                return;
            }

            // show radar
            ShowRadar(true);
        }


        public void ShowRadar(bool value)
        {
            if (Radar.Panel != null)
                Radar.Panel.gameObject.SetActive(value);
        }


        public void UpdateRadar(Transform rotationReference, RadarModes radarType)
        {
            // assign map / player indicator rotation
            if (radarType == RadarModes.RotateRadar)
            {
                // set radar rotation
                Radar.Radar.transform.rotation = Quaternion.Euler(Radar.Panel.transform.eulerAngles.x, Radar.Panel.transform.eulerAngles.y, rotationReference.eulerAngles.y);
                if (Radar.PlayerIndicator != null)
                    Radar.PlayerIndicator.transform.rotation = Radar.Panel.transform.rotation;
            }
            else
            {
                // set player indicator rotation
                Radar.Radar.transform.rotation = Radar.Panel.transform.rotation;
                if (Radar.PlayerIndicator != null)
                    Radar.PlayerIndicator.transform.rotation = Quaternion.Euler(Radar.Panel.transform.eulerAngles.x, Radar.Panel.transform.eulerAngles.y, -rotationReference.eulerAngles.y);
            }
        }
        #endregion
        #region Utility Methods
        void ReferencesMissing(string feature)
        {
            Debug.LogErrorFormat("{0} references are missing! Please assign them on the HUDNavigationCanvas component.", feature);
            this.enabled = false;
        }
        #endregion


    }
}

