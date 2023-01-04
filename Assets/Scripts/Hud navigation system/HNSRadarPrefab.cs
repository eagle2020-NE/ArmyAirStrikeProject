using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hudNavigation
{
    public class HNSRadarPrefab : HNSPrefab
    {
        #region Variables
        [Header("Icon")]
        [Tooltip("Assign an image component.")]
        public Image Icon;

        [Header("Height Arrows")]
        [Tooltip("Assign the above arrow image component.")]
        public Image ArrowAbove;
        [Tooltip("Assign the above arrow image component.")]
        public Image ArrowBelow;
        #endregion

        #region Override Methods
        /// <summary>
        /// Change the color of the radar icon.
        /// </summary>
        /// <param name="color">Color.</param>
        public override void ChangeIconColor(Color color)
        {
            base.ChangeIconColor(color);
            if (Icon != null)
                Icon.color = color;
        }
        #endregion


    }
}

