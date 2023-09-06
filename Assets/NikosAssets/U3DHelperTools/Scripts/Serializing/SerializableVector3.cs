using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace NikosAssets.Helpers.Serializing
{
    /// <summary>
    /// A helper class to (de)serialize and save <see cref="Vector3"/> structs via json
    /// </summary>
    [Serializable]
    public struct SerializableVector3 : IEquatable<SerializableVector3>
    {
        public float x, y, z;
        
        public SerializableVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(SerializableVector3 other)
        {
            return Mathf.Approximately(other.x, x) 
                   && Mathf.Approximately(other.y, y)
                   && Mathf.Approximately(other.z, z);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializableVector3(Vector3 v)
        {
            return new SerializableVector3(v.x, v.y, v.z);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector3(SerializableVector3 sv)
        {
            return new Vector3(sv.x, sv.y, sv.z);
        }
    }
}