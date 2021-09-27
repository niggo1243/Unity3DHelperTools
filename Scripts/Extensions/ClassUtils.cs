using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NikosAssets.Helpers.Extensions
{
    public static class ClassUtils
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

        /// <summary>
        /// @First() from System.Linq... I know it can be null, so you can stop throwing exceptions at me!!
        /// Fine Ill do it myself
        /// </summary>
        /// <param name="collection"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T MyFirst<T>(this IList<T> collection)
        {
            if (collection.Count < 1)
                return default;

            return collection[0];
        }
        
        public static ulong GetUInt64Hash(this string text)
        {
            return text.GetUInt64Hash(SHA256.Create());
        }
        
        /// <summary>
        ///
        /// author: https://stackoverflow.com/a/50364956
        /// </summary>
        /// <param name="text"></param>
        /// <param name="hasher"></param>
        /// <returns></returns>
        public static ulong GetUInt64Hash(this string text, HashAlgorithm hasher)
        {
            using (hasher)
            {
                var bytes = hasher.ComputeHash(Encoding.Default.GetBytes(text));
                Array.Resize(ref bytes, bytes.Length + bytes.Length % 8); //make multiple of 8 if hash is not, for exampel SHA1 creates 20 bytes. 
                return Enumerable.Range(0, bytes.Length / 8) // create a counter for de number of 8 bytes in the bytearray
                    .Select(i => BitConverter.ToUInt64(bytes, i * 8)) // combine 8 bytes at a time into a integer
                    .Aggregate((x, y) =>x ^ y); //xor the bytes together so you end up with a ulong (64-bit int)
            }
        }
    }
}