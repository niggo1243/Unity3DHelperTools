using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace NikosAssets.Helpers.Serializing
{
    /// <summary>
    /// A helper class to (de)serialize and save <see cref="Vector2"/> structs via json
    /// </summary>
    [Serializable]
    public struct SerializableVector2: IEquatable<SerializableVector2>
    {
        public float x, y;
        
        public SerializableVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(SerializableVector2 other)
        {
            return Mathf.Approximately(other.x, x) && Mathf.Approximately(other.y, y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializableVector2(Vector2 v)
        {
            return new SerializableVector2(v.x, v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector2(SerializableVector2 sv)
        {
            return new Vector2(sv.x, sv.y);
        }
    }
}
