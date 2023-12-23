using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Framework
{
    /// <summary>
    /// A helper wrapper class that prepares the usage and handling of color lerping of an interactable renderer (can be hovered and selected),
    /// where the actual color lerping has to be implemented in a subclass (for example via tweens or similar).
    /// </summary>
    public abstract class BaseLerpColorOfRendererInteractable : BaseLerpColorOfRendererMono
    {
        [BoxGroup("Hover " + HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public Color hoverColor;
        
        /// <summary>
        /// The time it takes to lerp the color when starting or exiting the hover of the renderer
        /// </summary>
        [BoxGroup("Hover " + HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("The time it takes to lerp the color when starting or exiting the hover of the renderer")]
        public float lerpColorBetweenHoverTimeInSec = .5f;

        /// <summary>
        /// When the renderer is not hovered anymore, should it show another color than before?
        /// </summary>
        [BoxGroup("Hover " + HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("When the renderer is not hovered anymore, should it show another color than before?")]
        public bool useDifferentColorOnHoverExit;
        
        [BoxGroup("Hover " + HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [ShowIf(nameof(useDifferentColorOnHoverExit))]
        public Color exitHoverColor;
        
        [BoxGroup("Select " + HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public Color selectColor;

        /// <summary>
        /// The time it takes to lerp the color when selecting or deselecting the renderer
        /// </summary>
        [BoxGroup("Select " + HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("The time it takes to lerp the color when selecting or deselecting the renderer")]
        public float lerpColorBetweenClicksTimeInSec = .1f;
        
        public virtual Color GetReturnColor() => useDifferentColorOnHoverExit ? exitHoverColor : OriginalColor;

        public virtual void LerpColorToSelectedColor()
        {
            LerpColor(selectColor, lerpColorBetweenClicksTimeInSec);
        }
        
        public virtual void LerpColorToHoverColor()
        {
            LerpColor(hoverColor, lerpColorBetweenHoverTimeInSec);
        }
        
        public virtual void LerpColorToDefaultState()
        {
            LerpColor(GetReturnColor(), lerpColorBetweenHoverTimeInSec);
        }
    }
}
