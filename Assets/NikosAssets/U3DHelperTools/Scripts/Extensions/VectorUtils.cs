using UnityEngine;

namespace NikosAssets.Helpers.Extensions
{
    /// <summary>
    /// A Vector extension helper class for some handy operations
    /// </summary>
    public static class VectorUtils
    {
        #region Set and Get X
        
        /// <summary>
        /// Return the same <see cref="Vector2"/> but with a different "x" value
        /// </summary>
        /// <param name="v">The <see cref="Vector2"/> to keep and change</param>
        /// <param name="x">The single value to change</param>
        /// <returns>Return the same <see cref="Vector2"/> but with a different "x" value</returns>
        public static Vector2 GetWithNewX(this Vector2 v, float x)
        {
            return new Vector2(x, v.y);
        }
        
        /// <summary>
        /// Return the same <see cref="Vector3"/> but with a different "x" value
        /// </summary>
        /// <param name="v">The <see cref="Vector3"/> to keep and change</param>
        /// <param name="x">The single value to change</param>
        /// <returns>Return the same <see cref="Vector3"/> but with a different "x" value</returns>
        public static Vector3 GetWithNewX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }
        
        /// <summary>
        /// Return the same <see cref="Vector4"/> but with a different "x" value
        /// </summary>
        /// <param name="v">The <see cref="Vector4"/> to keep and change</param>
        /// <param name="x">The single value to change</param>
        /// <returns>Return the same <see cref="Vector4"/> but with a different "x" value</returns>
        public static Vector4 GetWithNewX(this Vector4 v, float x)
        {
            return new Vector4(x, v.y, v.z, v.w);
        }
        
        #endregion

        #region Set and Get Y

        /// <summary>
        /// Return the same <see cref="Vector2"/> but with a different "y" value
        /// </summary>
        /// <param name="v">The <see cref="Vector2"/> to keep and change</param>
        /// <param name="y">The single value to change</param>
        /// <returns>Return the same <see cref="Vector2"/> but with a different "y" value</returns>
        public static Vector2 GetWithNewY(this Vector2 v, float y)
        {
            return new Vector2(v.x, y);
        }
        
        /// <summary>
        /// Return the same <see cref="Vector3"/> but with a different "y" value
        /// </summary>
        /// <param name="v">The <see cref="Vector3"/> to keep and change</param>
        /// <param name="y">The single value to change</param>
        /// <returns>Return the same <see cref="Vector3"/> but with a different "y" value</returns>
        public static Vector3 GetWithNewY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }
        
        /// <summary>
        /// Return the same <see cref="Vector4"/> but with a different "y" value
        /// </summary>
        /// <param name="v">The <see cref="Vector4"/> to keep and change</param>
        /// <param name="y">The single value to change</param>
        /// <returns>Return the same <see cref="Vector4"/> but with a different "y" value</returns>
        public static Vector4 GetWithNewY(this Vector4 v, float y)
        {
            return new Vector4(v.x, y, v.z, v.w);
        }
        
        #endregion
        
        #region Set and Get Z

        /// <summary>
        /// Return the same <see cref="Vector3"/> but with a different "z" value
        /// </summary>
        /// <param name="v">The <see cref="Vector3"/> to keep and change</param>
        /// <param name="z">The single value to change</param>
        /// <returns>Return the same <see cref="Vector3"/> but with a different "z" value</returns>
        public static Vector3 GetWithNewZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }
        
        /// <summary>
        /// Return the same <see cref="Vector4"/> but with a different "z" value
        /// </summary>
        /// <param name="v">The <see cref="Vector4"/> to keep and change</param>
        /// <param name="z">The single value to change</param>
        /// <returns>Return the same <see cref="Vector4"/> but with a different "z" value</returns>
        public static Vector4 GetWithNewZ(this Vector4 v, float z)
        {
            return new Vector4(v.x, v.y, z, v.w);
        }
        
        #endregion
        
        #region Set and Get W

        /// <summary>
        /// Return the same <see cref="Vector4"/> but with a different "w" value
        /// </summary>
        /// <param name="v">The <see cref="Vector4"/> to keep and change</param>
        /// <param name="w">The single value to change</param>
        /// <returns>Return the same <see cref="Vector4"/> but with a different "w" value</returns>
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

        /// <summary>
        /// Multiply each value of the "<paramref name="b"/>" <see cref="Vector2"/> with this
        /// "<paramref name="a"/>" <see cref="Vector2"/> and return the result
        /// </summary>
        /// <param name="a"><see cref="Vector2"/></param>
        /// <param name="b"><see cref="Vector2"/></param>
        /// <returns>A <see cref="Vector2"/></returns>
        public static Vector2 Multiply(this Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }
        
        /// <summary>
        /// Multiply each value of the "<paramref name="b"/>" <see cref="Vector3"/> with this
        /// "<paramref name="a"/>" <see cref="Vector3"/> and return the result
        /// </summary>
        /// <param name="a"><see cref="Vector3"/></param>
        /// <param name="b"><see cref="Vector3"/></param>
        /// <returns>A <see cref="Vector3"/></returns>
        public static Vector3 Multiply(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        
        /// <summary>
        /// Multiply each value of the "<paramref name="b"/>" <see cref="Vector4"/> with this
        /// "<paramref name="a"/>" <see cref="Vector4"/> and return the result
        /// </summary>
        /// <param name="a"><see cref="Vector4"/></param>
        /// <param name="b"><see cref="Vector4"/></param>
        /// <returns>A <see cref="Vector4"/></returns>
        public static Vector4 Multiply(this Vector4 a, Vector4 b)
        {
            return new Vector4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
        }

        #endregion
    }
}
