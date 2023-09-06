using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace NikosAssets.Helpers.Serializing
{
    /// <summary>
    /// A helper class to (de)serialize and save <see cref="Vector4"/> structs via json
    /// </summary>
    [Serializable]
    public struct SerializableVector4 : IEquatable<SerializableVector4>
    {
        public float x, y, z, w;
        
        public SerializableVector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(SerializableVector4 other)
        {
            return Mathf.Approximately(other.x, x) 
                   && Mathf.Approximately(other.y, y)
                   && Mathf.Approximately(other.z, z)
                   && Mathf.Approximately(other.w, w);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializableVector4(Vector4 v)
        {
            return new SerializableVector4(v.x, v.y, v.z, v.w);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector4(SerializableVector4 sv)
        {
            return new Vector4(sv.x, sv.y, sv.z, sv.w);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializableVector4(Color c)
        {
            return new SerializableVector4(c.r, c.g, c.b, c.a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color(SerializableVector4 sv)
        {
            return new Color(sv.x, sv.y, sv.z, sv.w);
        }
    }
}