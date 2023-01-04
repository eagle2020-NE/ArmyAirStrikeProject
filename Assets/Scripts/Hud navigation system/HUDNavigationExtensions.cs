using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace hudNavigation
{
    public static class HUDNavigationExtensions
    {
        #region Extension Methods
        public static float GetDistance(this HUDNavigationElement element, Transform other)
        {
            return Vector2.Distance(new Vector2(element.transform.position.x, element.transform.position.z), new Vector2(other.position.x, other.position.z));
        }


        public static Vector3 GetPosition(this HUDNavigationElement element)
        {
            return element.transform.position;
        }


        public static Vector3 GetPositionOffset(this HUDNavigationElement element, Vector3 otherPosition)
        {
            return element.transform.position - otherPosition;
        }


        public static float GetRadius(this RectTransform rect)
        {
            Vector3[] arr = new Vector3[4];
            rect.GetLocalCorners(arr);
            float _radius = Mathf.Abs(arr[0].y);
            if (Mathf.Abs(arr[0].x) < Mathf.Abs(arr[0].y))
                _radius = Mathf.Abs(arr[0].x);

            return _radius;
        }


        public static Vector3 KeepInRectBounds(this RectTransform rect, Vector3 markerPos, out bool outOfBounds)
        {
            Vector3 oldPos = markerPos;
            markerPos = Vector3.Min(markerPos, rect.rect.max);
            markerPos = Vector3.Max(markerPos, rect.rect.min);

            outOfBounds = oldPos != markerPos;

            return markerPos;
        }


        public static float GetIconRadius(this HUDNavigationElement element, NavigationElementType elementType)
        {
            float radius = (elementType == NavigationElementType.Radar) ? element.Radar.PrefabRect.sizeDelta.x : 0;
            return radius / 2f;
        }


        public static bool IsVisibleOnScreen(this HUDNavigationElement element, Vector3 screenPos)
        {
            return screenPos.z > 0 && screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height;
        }

        public static bool IsOnScreenHotspot(this HUDNavigationElement element, Camera camera, RectTransform hotspot, float screenWidth, float screenHeight)
        {
            var screenPos = camera.WorldToScreenPoint(element.transform.position);

            screenPos.z = 0f;
            screenPos.x -= screenWidth / 2.0f;
            screenPos.y -= screenHeight / 2.0f;

            if (hotspot.rect.Contains(screenPos))
            {
                return true;
            }

            return false;
        }


        public static void SetMarkerPosition(this HUDNavigationElement element, NavigationElementType elementType, Vector3 markerPos, RectTransform parentRect = null)
        {
            // set marker position
            if (elementType == NavigationElementType.Radar)
                element.Radar.transform.localPosition = markerPos;
            
        }


        public static void SetMarkerActive(this HUDNavigationElement element, NavigationElementType elementType, bool value)
        {
            // get marker gameobject
            GameObject markerGO = null;
            switch (elementType)
            {
                case NavigationElementType.Radar:
                    markerGO = element.Radar.gameObject;
                    break;
                default:
                    break;
            }

            // set marker gameobject active/inactive
            if (markerGO != null)
            {
                // only update if value has changed
                if (value != markerGO.activeSelf)
                {
                    // invoke events
                    if (value) // appeared
                        element.OnAppear.Invoke(element, elementType);
                    else // disappeared
                        element.OnDisappear.Invoke(element, elementType);

                    // set active state
                    markerGO.gameObject.SetActive(value);
                }
            }
        }
        #endregion


        #region Radar Extension Methods
        public static void ShowRadarAboveArrow(this HUDNavigationElement element, bool value)
        {
            if (element.Radar.ArrowAbove == null)
                return;

            // only update if value has changed
            if (value != element.Radar.ArrowAbove.gameObject.activeSelf)
                element.Radar.ArrowAbove.gameObject.SetActive(value);
        }


        public static void ShowRadarBelowArrow(this HUDNavigationElement element, bool value)
        {
            if (element.Radar.ArrowBelow == null)
                return;

            // only update if value has changed
            if (value != element.Radar.ArrowBelow.gameObject.activeSelf)
                element.Radar.ArrowBelow.gameObject.SetActive(value);
        }
        #endregion
    }
}

