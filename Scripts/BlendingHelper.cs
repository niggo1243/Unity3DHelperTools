using System;

using UnityEngine;
using NaughtyAttributes;

namespace NikosAssets.Helpers
{
    [Serializable]
    public class BlendingHelper
    {
        /// <summary>
        /// Should the Settings blend a value?
        /// </summary>
        [Tooltip("Should the Settings blend a value?")]
        public bool blendOverTime;

        /// <summary>
        /// Should the blended value lerp or move towards?
        /// </summary>
        [ShowIf(nameof(blendOverTime))]
        [Tooltip("Should the blended value lerp or move towards?")]
        [AllowNesting]
        public bool lerp;

        /// <summary>
        /// The blend in and out time
        /// </summary>
        [ShowIf(nameof(blendOverTime))]
        [Tooltip("The blend in and out time")]
        public float blendTime = 1;

        public virtual float Blend(float currentValue, float targetValue, float deltaTime = 1)
        {
            if (!blendOverTime) return targetValue;
            
            return this.lerp
                ?
                Mathf.Lerp(currentValue, targetValue, this.blendTime * deltaTime)
                :
                Mathf.MoveTowards(currentValue, targetValue, this.blendTime * deltaTime);
        }
        
        public virtual Vector2 Blend(Vector2 currentValue, Vector2 targetValue, float deltaTime = 1)
        {
            if (!blendOverTime) return targetValue;
            
            return this.lerp
                ?
                Vector2.Lerp(currentValue, targetValue, this.blendTime * deltaTime)
                :
                Vector2.MoveTowards(currentValue, targetValue, this.blendTime * deltaTime);
        }
        
        public virtual Vector3 Blend(Vector3 currentValue, Vector3 targetValue, float deltaTime = 1)
        {
            if (!blendOverTime) return targetValue;
            
            return this.lerp
                ?
                Vector3.Lerp(currentValue, targetValue, this.blendTime * deltaTime)
                :
                Vector3.MoveTowards(currentValue, targetValue, this.blendTime * deltaTime);
        }
        
        public virtual Vector4 Blend(Vector4 currentValue, Vector4 targetValue, float deltaTime = 1)
        {
            if (!blendOverTime) return targetValue;
            
            return this.lerp
                ?
                Vector4.Lerp(currentValue, targetValue, this.blendTime * deltaTime)
                :
                Vector4.MoveTowards(currentValue, targetValue, this.blendTime * deltaTime);
        }
    }
}
