using System;
using System.Collections;
using NaughtyAttributes;
using NikosAssets.Helpers.Extensions;
using UnityEngine;

namespace NikosAssets.Helpers.Framework
{
    /// <summary>
    /// A helper wrapper class that prepares the usage and handling of color lerping of a renderer,
    /// where the actual color lerping has to be implemented in a subclass (for example via tweens or similar)
    /// </summary>
    public abstract class BaseLerpColorOfRendererMono : BaseNotesMono
    {
        /// <summary>
        /// The material(s) of this renderer to change/ lerp the color
        /// </summary>
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("The material(s) of this renderer to change/ lerp the color")]
        public Renderer rendererToChangeColor;
     
        /// <summary>
        /// The time it takes to lerp the color (back and forth -> ping pong)
        /// </summary>
        [BoxGroup("PingPong " + HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("The time it takes to lerp the color (back and forth -> ping pong)")]
        public float pingPongColorChangeTime = .5f;

        protected Color _originalColor;
        /// <summary>
        /// The initial unchanged color of the <see cref="rendererToChangeColor"/>
        /// </summary>
        public Color OriginalColor => _originalColor;
        
        protected Coroutine _lerpColorPingPongCor;
        
        /// <summary>
        /// Is the color lerp currently ongoing?
        /// </summary>
        public abstract bool IsLerping { get; }

        /// <summary>
        /// Is the ping pong (back and forth) color lerping currently ongoing?
        /// </summary>
        public virtual bool IsPingPongRunning => _lerpColorPingPongCor != null;
        
        protected virtual void Start()
        {
            _originalColor = rendererToChangeColor.material.color;
        }
        
        protected virtual void OnDisable()
        {
            StopLerpColorsPingPong();
        }

        protected virtual void Reset()
        {
            if (rendererToChangeColor == null)
                rendererToChangeColor = GetComponent<Renderer>();
        }
        
        /// <summary>
        /// Lerp the color of the <see cref="rendererToChangeColor"/> from its current color to the "<paramref name="targetColor"/>"
        /// and afterwards back to "<paramref name="returnColor"/>" in an infinite loop, given the "<paramref name="saveToLerpCondition"/>" is valid
        /// </summary>
        /// <param name="targetColor">
        /// The initial <see cref="Color"/> to lerp towards
        /// </param>
        /// <param name="returnColor">
        /// The 2nd <see cref="Color"/> to lerp back to
        /// </param>
        /// <param name="timeInSec">
        /// The time it takes for each lerp cycle
        /// </param>
        /// <param name="saveToLerpCondition">
        /// Is the lerping allowed?
        /// </param>
        /// <returns><see cref="IEnumerator"/></returns>
        public virtual IEnumerator LerpColorsPingPong(Color targetColor, Color returnColor, float timeInSec, Func<bool> saveToLerpCondition = null)
        {
            while (enabled && gameObject.activeInHierarchy)
            {
                yield return LerpColorIfSaveCoroutine(targetColor, timeInSec, saveToLerpCondition);
                yield return LerpColorIfSaveCoroutine(returnColor, timeInSec, saveToLerpCondition);
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// Lerp the color of the <see cref="rendererToChangeColor"/> from its current color to the "<paramref name="targetColor"/>"
        /// ,given the "<paramref name="saveToLerpCondition"/>" is valid
        /// </summary>
        /// <param name="targetColor">
        /// The <see cref="Color"/> to lerp towards
        /// </param>
        /// <param name="timeInSec">
        /// The time it takes to lerp
        /// </param>
        /// <param name="saveToLerpCondition">
        /// Is the lerping allowed?
        /// </param>
        /// <returns><see cref="IEnumerator"/></returns>
        public abstract IEnumerator LerpColorIfSaveCoroutine(Color targetColor, float timeInSec, Func<bool> saveToLerpCondition = null);

        /// <summary>
        /// Start the Coroutine <see cref="_lerpColorPingPongCor"/> by calling <see cref="LerpColorsPingPong"/>,
        /// which lerps the color of the <see cref="rendererToChangeColor"/> from its current color to the "<paramref name="targetColor"/>"
        /// and afterwards back to "<paramref name="returnColor"/>" in an infinite loop, given the "<paramref name="saveToLerpCondition"/>" is valid
        /// </summary>
        /// <param name="targetColor">
        /// The initial <see cref="Color"/> to lerp towards
        /// </param>
        /// <param name="returnColor">
        /// The 2nd <see cref="Color"/> to lerp back to
        /// </param>
        /// <param name="timeInSec">
        /// The time it takes for each lerp cycle
        /// </param>
        /// <param name="saveToLerpCondition">
        /// Is the lerping allowed?
        /// </param>
        public virtual void StartLerpColorsPingPong(Color targetColor, Color returnColor, float timeInSec, Func<bool> saveToLerpCondition = null)
        {
            _lerpColorPingPongCor = this.StartAndReplaceCoroutine(ref _lerpColorPingPongCor,
                LerpColorsPingPong(targetColor, returnColor, timeInSec, saveToLerpCondition));
        }

        /// <summary>
        /// Start the Coroutine <see cref="_lerpColorPingPongCor"/> by calling <see cref="LerpColorsPingPong"/> (with <see cref="pingPongColorChangeTime"/>),
        /// which lerps the color of the <see cref="rendererToChangeColor"/> from its current color to the "<paramref name="targetColor"/>"
        /// and afterwards back to "<paramref name="returnColor"/>" in an infinite loop, given the "<paramref name="saveToLerpCondition"/>" is valid
        /// </summary>
        /// <param name="targetColor">
        /// The initial <see cref="Color"/> to lerp towards
        /// </param>
        /// <param name="returnColor">
        /// The 2nd <see cref="Color"/> to lerp back to
        /// </param>
        /// <param name="saveToLerpCondition">
        /// Is the lerping allowed?
        /// </param>
        public virtual void StartLerpColorsPingPong(Color targetColor, Color returnColor, Func<bool> saveToLerpCondition = null)
        {
            StartLerpColorsPingPong(targetColor, returnColor, pingPongColorChangeTime, saveToLerpCondition);
        }
        
        /// <summary>
        /// Stops the <see cref="_lerpColorPingPongCor"/> <see cref="Coroutine"/> if it was running
        /// </summary>
        public virtual void StopLerpColorsPingPong()
        {
            this.StopRunningCoroutine(_lerpColorPingPongCor);
        }
        
        /// <summary>
        /// Lerps the color of <see cref="rendererToChangeColor"/> towards "<paramref name="targetColor"/>" with the given "<paramref name="timeInSec"/>"
        /// </summary>
        /// <param name="targetColor"><see cref="Color"/></param>
        /// <param name="timeInSec">The time it takes in sec to lerp towards "<paramref name="targetColor"/>""</param>
        public abstract void LerpColor(Color targetColor, float timeInSec);
    }
}
