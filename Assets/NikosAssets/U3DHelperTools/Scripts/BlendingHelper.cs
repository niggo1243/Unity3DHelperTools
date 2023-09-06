using System;

using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Serialization;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// A helper class to blend <see cref="float"/>s, <see cref="Vector2"/>s, <see cref="Vector3"/>s and <see cref="Vector4"/>s
    /// </summary>
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
        [FormerlySerializedAs("blendTime")]
        [ShowIf(nameof(blendOverTime))]
        [Tooltip("The blend in and out time")]
        [AllowNesting]
        public float blendSpeed = 1;

        /// <summary>
        /// Blends the "<paramref name="currentValue"/>" to "<paramref name="targetValue"/>" if "<paramref name="blendOverTime"/>" is true.
        /// </summary>
        /// <param name="currentValue">
        /// The starting value
        /// </param>
        /// <param name="targetValue">
        /// The desired final value
        /// </param>
        /// <param name="lerp">
        /// If true, use the lerp logic, otherwise move towards
        /// </param>
        /// <param name="blendOverTime">
        /// If false, returns the "<paramref name="targetValue"/>" with no blending whatsoever
        /// </param>
        /// <param name="blendSpeed">
        /// The speed to turn "<paramref name="currentValue"/>" into "<paramref name="targetValue"/>"
        /// </param>
        /// <param name="deltaTime">
        /// Is multiplied with "<paramref name="blendSpeed"/>" during the blending
        /// </param>
        /// <returns>
        /// The blended value between "<paramref name="currentValue"/>" and "<paramref name="targetValue"/>"
        /// </returns>
        public static float Blend(float currentValue, float targetValue, 
            bool lerp, bool blendOverTime, float blendSpeed, float deltaTime = 1)
        {
            if (!blendOverTime) return targetValue;
            
            return lerp
                ?
                Mathf.Lerp(currentValue, targetValue, blendSpeed * deltaTime)
                :
                Mathf.MoveTowards(currentValue, targetValue, blendSpeed * deltaTime);
        }
        
        /// <summary>
        /// Blends the "<paramref name="currentValue"/>" to "<paramref name="targetValue"/>" if "<paramref name="blendOverTime"/>" is true.
        /// </summary>
        /// <param name="currentValue">
        /// The starting value
        /// </param>
        /// <param name="targetValue">
        /// The desired final value
        /// </param>
        /// <param name="lerp">
        /// If true, use the lerp logic, otherwise move towards
        /// </param>
        /// <param name="blendOverTime">
        /// If false, returns the "<paramref name="targetValue"/>" with no blending whatsoever
        /// </param>
        /// <param name="blendSpeed">
        /// The speed to turn "<paramref name="currentValue"/>" into "<paramref name="targetValue"/>"
        /// </param>
        /// <param name="deltaTime">
        /// Is multiplied with "<paramref name="blendSpeed"/>" during the blending
        /// </param>
        /// <returns>
        /// The blended value between "<paramref name="currentValue"/>" and "<paramref name="targetValue"/>"
        /// </returns>
        public static Vector2 Blend(Vector2 currentValue, Vector2 targetValue, 
            bool lerp, bool blendOverTime, float blendSpeed, float deltaTime = 1)
        {
            if (!blendOverTime) return targetValue;
            
            return lerp
                ?
                Vector2.Lerp(currentValue, targetValue, blendSpeed * deltaTime)
                :
                Vector2.MoveTowards(currentValue, targetValue, blendSpeed * deltaTime);
        }

        /// <summary>
        /// Blends the "<paramref name="currentValue"/>" to "<paramref name="targetValue"/>" if "<paramref name="blendOverTime"/>" is true.
        /// </summary>
        /// <param name="currentValue">
        /// The starting value
        /// </param>
        /// <param name="targetValue">
        /// The desired final value
        /// </param>
        /// <param name="lerp">
        /// If true, use the lerp logic, otherwise move towards
        /// </param>
        /// <param name="blendOverTime">
        /// If false, returns the "<paramref name="targetValue"/>" with no blending whatsoever
        /// </param>
        /// <param name="blendSpeed">
        /// The speed to turn "<paramref name="currentValue"/>" into "<paramref name="targetValue"/>"
        /// </param>
        /// <param name="deltaTime">
        /// Is multiplied with "<paramref name="blendSpeed"/>" during the blending
        /// </param>
        /// <returns>
        /// The blended value between "<paramref name="currentValue"/>" and "<paramref name="targetValue"/>"
        /// </returns>
        public static Vector3 Blend(Vector3 currentValue, Vector3 targetValue, 
            bool lerp, bool blendOverTime, float blendSpeed, float deltaTime = 1)
        {
            if (!blendOverTime) return targetValue;
            
            return lerp
                ?
                Vector3.Lerp(currentValue, targetValue, blendSpeed * deltaTime)
                :
                Vector3.MoveTowards(currentValue, targetValue, blendSpeed * deltaTime);
        }
        
        /// <summary>
        /// Blends the "<paramref name="currentValue"/>" to "<paramref name="targetValue"/>" if "<paramref name="blendOverTime"/>" is true.
        /// </summary>
        /// <param name="currentValue">
        /// The starting value
        /// </param>
        /// <param name="targetValue">
        /// The desired final value
        /// </param>
        /// <param name="lerp">
        /// If true, use the lerp logic, otherwise move towards
        /// </param>
        /// <param name="blendOverTime">
        /// If false, returns the "<paramref name="targetValue"/>" with no blending whatsoever
        /// </param>
        /// <param name="blendSpeed">
        /// The speed to turn "<paramref name="currentValue"/>" into "<paramref name="targetValue"/>"
        /// </param>
        /// <param name="deltaTime">
        /// Is multiplied with "<paramref name="blendSpeed"/>" during the blending
        /// </param>
        /// <returns>
        /// The blended value between <paramref name="currentValue"/> and <paramref name="targetValue"/>
        /// </returns>
        public static Vector4 Blend(Vector4 currentValue, Vector4 targetValue, 
            bool lerp, bool blendOverTime, float blendSpeed, float deltaTime = 1)
        {
            if (!blendOverTime) return targetValue;
            
            return lerp
                ?
                Vector4.Lerp(currentValue, targetValue, blendSpeed * deltaTime)
                :
                Vector4.MoveTowards(currentValue, targetValue, blendSpeed * deltaTime);
        }
        
        /// <summary>
        /// Blends the "<paramref name="currentValue"/>" to "<paramref name="targetValue"/>" if <see cref="blendOverTime"/> is true.
        /// </summary>
        /// <param name="currentValue">
        /// The starting value
        /// </param>
        /// <param name="targetValue">
        /// The desired final value
        /// </param>
        /// <param name="deltaTime">
        /// Is multiplied with <see cref="blendSpeed"/> during the blending
        /// </param>
        /// <returns>
        /// The blended value between "<paramref name="currentValue"/>" and "<paramref name="targetValue"/>"
        /// </returns>
        public virtual float Blend(float currentValue, float targetValue, float deltaTime = 1)
        {
            return Blend(currentValue, targetValue, lerp, blendOverTime, blendSpeed, deltaTime);
        }
        
        /// <summary>
        /// Blends the "<paramref name="currentValue"/>" to "<paramref name="targetValue"/>" if <see cref="blendOverTime"/> is true.
        /// </summary>
        /// <param name="currentValue">
        /// The starting value
        /// </param>
        /// <param name="targetValue">
        /// The desired final value
        /// </param>
        /// <param name="deltaTime">
        /// Is multiplied with <see cref="blendSpeed"/> during the blending
        /// </param>
        /// <returns>
        /// The blended value between "<paramref name="currentValue"/>" and "<paramref name="targetValue"/>"
        /// </returns>
        public virtual Vector2 Blend(Vector2 currentValue, Vector2 targetValue, float deltaTime = 1)
        {
            return Blend(currentValue, targetValue, lerp, blendOverTime, blendSpeed, deltaTime);
        }
        
        /// <summary>
        /// Blends the "<paramref name="currentValue"/>" to "<paramref name="targetValue"/>" if <see cref="blendOverTime"/> is true.
        /// </summary>
        /// <param name="currentValue">
        /// The starting value
        /// </param>
        /// <param name="targetValue">
        /// The desired final value
        /// </param>
        /// <param name="deltaTime">
        /// Is multiplied with <see cref="blendSpeed"/> during the blending
        /// </param>
        /// <returns>
        /// The blended value between "<paramref name="currentValue"/>" and "<paramref name="targetValue"/>"
        /// </returns>
        public virtual Vector3 Blend(Vector3 currentValue, Vector3 targetValue, float deltaTime = 1)
        {
            return Blend(currentValue, targetValue, lerp, blendOverTime, blendSpeed, deltaTime);
        }

        /// <summary>
        /// Blends the "<paramref name="currentValue"/>" to "<paramref name="targetValue"/>" if <see cref="blendOverTime"/> is true.
        /// </summary>
        /// <param name="currentValue">
        /// The starting value
        /// </param>
        /// <param name="targetValue">
        /// The desired final value
        /// </param>
        /// <param name="deltaTime">
        /// Is multiplied with <see cref="blendSpeed"/> during the blending
        /// </param>
        /// <returns>
        /// The blended value between "<paramref name="currentValue"/>" and "<paramref name="targetValue"/>"
        /// </returns>
        public virtual Vector4 Blend(Vector4 currentValue, Vector4 targetValue, float deltaTime = 1)
        {
            return Blend(currentValue, targetValue, lerp, blendOverTime, blendSpeed, deltaTime);
        }
    }
}
