using System;
using System.Collections;
using NaughtyAttributes;
using NikosAssets.Helpers.Extensions;
using UnityEngine;

namespace NikosAssets.Helpers.Framework
{
    public abstract class BaseLerpColorOfRendererMono : BaseNotesMono
    { 
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public Renderer rendererToChangeColor;
        
        [BoxGroup("PingPong " + HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public float pingPongColorChangeTime = .5f;

        protected Color _originalColor;
        public Color OriginalColor => _originalColor;
        
        protected Coroutine _lerpColorPingPongCor;
        
        public abstract bool IsLerping { get; }

        public virtual bool IsPingPongRunning => _lerpColorPingPongCor != null;

        public virtual bool IsSaveForCustomLerp => !IsLerping;

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
        
        protected virtual IEnumerator LerpColorsPingPong(Color targetColor, Color returnColor, float timeInSec, Func<bool> saveToLerpCondition = null)
        {
            while (enabled && gameObject.activeInHierarchy)
            {
                yield return LerpColorIfSaveCoroutine(targetColor, timeInSec, saveToLerpCondition);
                yield return LerpColorIfSaveCoroutine(returnColor, timeInSec, saveToLerpCondition);
                yield return new WaitForEndOfFrame();
            }
        }

        protected abstract IEnumerator LerpColorIfSaveCoroutine(Color targetColor, float timeInSec, Func<bool> saveToLerpCondition = null);

        public virtual bool LerpColorIfSave(Color targetColor, float timeInSec, Func<bool> saveToLerpCondition = null)
        {
            bool isSave = IsSaveForCustomLerp;

            if (isSave)
                StartCoroutine(LerpColorIfSaveCoroutine(targetColor, timeInSec, saveToLerpCondition));

            return isSave;
        }
        
        public virtual void StartLerpColorsPingPong(Color targetColor, Color returnColor, float timeInSec, Func<bool> saveToLerpCondition = null)
        {
            _lerpColorPingPongCor = this.StartAndReplaceCoroutine(ref _lerpColorPingPongCor,
                LerpColorsPingPong(targetColor, returnColor, timeInSec, saveToLerpCondition));
        }

        public virtual void StartLerpColorsPingPong(Color targetColor, Color returnColor, Func<bool> saveToLerpCondition = null)
        {
            StartLerpColorsPingPong(targetColor, returnColor, pingPongColorChangeTime, saveToLerpCondition);
        }
        
        public virtual void StopLerpColorsPingPong()
        {
            this.StopRunningCoroutine(_lerpColorPingPongCor);
        }
        
        public abstract void LerpColor(Color targetColor, float timeInSec);
    }
}
