﻿using UnityEngine;

namespace NikosAssets.Helpers.Extensions
{
    /// <summary>
    /// A general <see cref="UnityEngine"/>.<see cref="UnityEngine.Object"/> extension helper class
    /// </summary>
    public static class ObjectUtils
    {
        /// <summary> Perform a deep Copy of the object. Binary Serialization is used to perform the copy </summary>
        /// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
        /// Very Expensive!!
        /// <typeparam name="T">The type of object being copied </typeparam>
        /// <param name="source">The object instance to copy </param>
        /// <returns>The copied object </returns>
        public static T CloneSerializable<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new System.ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (UnityEngine.Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            System.Runtime.Serialization.IFormatter formatter =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.Stream stream = new System.IO.MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                return (T) formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Creates and returns a clone of any given scriptable object.
        /// </summary>
        public static T CloneScriptableObject<T>(this T scriptableObject) where T : ScriptableObject
        {
            if (scriptableObject == null)
            {
                Debug.LogError($"ScriptableObject was null. Returning default {typeof(T)} object.");
                return (T)ScriptableObject.CreateInstance(typeof(T));
            }
 
            T instance = Object.Instantiate(scriptableObject);
            instance.name = scriptableObject.name; // remove (Clone) from name
            return instance;
        }
    }
}