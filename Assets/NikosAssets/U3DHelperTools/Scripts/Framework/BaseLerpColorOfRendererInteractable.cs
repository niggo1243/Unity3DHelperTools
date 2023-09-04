using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Framework
{
    public abstract class BaseLerpColorOfRendererInteractable : BaseLerpColorOfRendererMono
    {
        [BoxGroup("Hover " + HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public Color hoverColor;
        
        [BoxGroup("Hover " + HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public float lerpColorBetweenHoverTimeInSec = .5f;

        [BoxGroup("Hover " + HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public bool useDifferentColorOnHoverExit;
        
        [BoxGroup("Hover " + HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [ShowIf(nameof(useDifferentColorOnHoverExit))]
        public Color exitHoverColor;
        
        [BoxGroup("Select " + HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public Color selectColor;

        [BoxGroup("Select " + HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
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
