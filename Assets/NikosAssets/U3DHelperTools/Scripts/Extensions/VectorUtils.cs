using UnityEngine;

namespace NikosAssets.Helpers.Extensions
{
    public static class VectorUtils
    {
        #region Set and Get X
        
        public static Vector2 GetWithNewX(this Vector2 v, float x)
        {
            return new Vector2(x, v.y);
        }
        
        public static Vector3 GetWithNewX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }
        
        public static Vector4 GetWithNewX(this Vector4 v, float x)
        {
            return new Vector4(x, v.y, v.z, v.w);
        }
        
        #endregion

        #region Set and Get Y

        public static Vector2 GetWithNewY(this Vector2 v, float y)
        {
            return new Vector2(v.x, y);
        }
        
        public static Vector3 GetWithNewY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }
        
        public static Vector4 GetWithNewY(this Vector4 v, float y)
        {
            return new Vector4(v.x, y, v.z, v.w);
        }
        
        #endregion
        
        #region Set and Get Z

        public static Vector3 GetWithNewZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }
        
        public static Vector4 GetWithNewZ(this Vector4 v, float z)
        {
            return new Vector4(v.x, v.y, z, v.w);
        }
        
        #endregion
        
        #region Set and Get W

        public static Vector4 GetWithNewW(this Vector4 v, float w)
        {
            return new Vector4(v.x, v.y, v.z, w);
        }
        
        #endregion

        #region Divide
        
        /// <summary>
        /// Returns a <see cref="Vector2"/> division for each value of the given vectors
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns>
        /// The item by item divided <see cref="Vector2"/>
        /// </returns>
        public static Vector2 Divide(this Vector2 dividend, Vector2 divisor)
        {
            return new Vector2(dividend.x / divisor.x, dividend.y / divisor.y);
        }

        /// <summary>
        /// Returns a <see cref="Vector3"/> division for each value of the given vectors
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns>
        /// The item by item divided <see cref="Vector3"/>
        /// </returns>
        public static Vector3 Divide(this Vector3 dividend, Vector3 divisor)
        {
            return new Vector3(dividend.x / divisor.x, dividend.y / divisor.y, dividend.z / divisor.z);
        }
        
        /// <summary>
        /// Returns a <see cref="Vector4"/> division for each value of the given vectors
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns>
        /// The item by item divided <see cref="Vector4"/>
        /// </returns>
        public static Vector4 Divide(this Vector4 dividend, Vector4 divisor)
        {
            return new Vector4(dividend.x / divisor.x, dividend.y / divisor.y, dividend.z / divisor.z, dividend.w / divisor.w);
        }
        
        #endregion

        #region Multiply

        public static Vector2 Multiply(this Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }
        
        public static Vector3 Multiply(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        
        public static Vector4 Multiply(this Vector4 a, Vector4 b)
        {
            return new Vector4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
        }

        #endregion
    }
}
