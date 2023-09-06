using System;
using System.Collections.Generic;
using NikosAssets.Helpers.Extensions;
using Random = UnityEngine.Random;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// A singleton helper class to access a <see cref="StringHasher"/> globally
    /// </summary>
    public static class StringHasherGlobal
    {
        private static StringHasher _hasher;
        public static StringHasher Hasher
        {
            get
            {
                if (_hasher == null)
                    _hasher = new StringHasher();

                return _hasher;
            }
        }
    }
    
    /// <summary>
    /// A helper class to map key (number hashes) to existing or yet to be added strings
    /// </summary>
    public class StringHasher
    {
        public Dictionary<int, string> stringHashDictInt32 = new Dictionary<int, string>();
        public Dictionary<ulong, string> stringHashDictUInt64 = new Dictionary<ulong, string>();

        public int reserved32 = -1;
        public ulong reservedU64 = 0;

        public StringHasher()
        {
        }
        
        public StringHasher(int reserved32)
        {
            this.reserved32 = reserved32;
        }
        
        public StringHasher(ulong reservedU64)
        {
            this.reservedU64 = reservedU64;
        }
        
        public StringHasher(int reserved32, ulong reservedU64)
        {
            this.reserved32 = reserved32;
            this.reservedU64 = reservedU64;
        }

        /// <summary>
        /// Returns the hash value for the given string "<paramref name="value"/>",
        /// either an existing one or a new one using the <see cref="stringHashDictInt32"/>
        /// </summary>
        /// <param name="desiredHashKey">
        /// The desired hash for the given string value
        /// </param>
        /// <param name="value">
        /// The string value to map to the generated or desired hash
        /// </param>
        /// <param name="checkValue">
        /// Check if the value already has a key that doesnt match the desiredKey?         /// </param>
        /// <returns>
        /// Returns the reserved number, if the string value is null or empty.
        /// Returns the desiredHash value if the string entry matched the hash or no entry was found.
        /// Returns a new random hash value if the string value did not match the desired key on the found entry (hash collision)
        /// or the desired hash matched the reserved number
        /// </returns>
        public virtual int GetAndSet32(int desiredHashKey, string value, bool checkValue = true)
        {
            if (string.IsNullOrEmpty(value))
                return this.reserved32;

            //no hash defined, create one
            if (desiredHashKey == this.reserved32)
                desiredHashKey = value.GetHashCode();

            //is the entry present
            if (this.stringHashDictInt32.TryGetValue(desiredHashKey, out string valueOfDesiredHash))
            {
                //is the value equal
                if (valueOfDesiredHash.Equals(value))
                    return desiredHashKey;
                
                foreach (KeyValuePair<int,string> valuePair in this.stringHashDictInt32)
                    if (valuePair.Value.Equals(value)) return valuePair.Key;

                //hash collision but with different values! create/ adapt a new hash value
                desiredHashKey = (int)(desiredHashKey * .3f) + Random.Range((int)(Int32.MinValue * .5f), (int)(Int32.MaxValue * .5f));
                do
                {
                    ++desiredHashKey;
                } while (this.stringHashDictInt32.ContainsKey(desiredHashKey));
            }
            //check if the value already has a key that doesnt match the desiredKey? 
            else if (checkValue)
            {
                foreach (KeyValuePair<int,string> valuePair in this.stringHashDictInt32)
                    if (valuePair.Value.Equals(value)) return valuePair.Key;
            }
            
            //add a new kvp and return the unique hash
            this.stringHashDictInt32.Add(desiredHashKey, value);
            return desiredHashKey;
        }

        /// <summary>
        /// Returns the hash value for the given string "<paramref name="value"/>",
        /// either an existing one or a new one using the <see cref="stringHashDictUInt64"/>
        /// </summary>
        /// <param name="desiredHashKey">
        /// The desired hash for the given string value
        /// </param>
        /// <param name="value">
        /// The string value to map to the generated or desired hash
        /// </param>
        /// <param name="checkValue">
        /// Check if the value already has a key that doesnt match the desiredKey?
        /// </param>
        /// <returns>
        /// Returns the reserved number, if the string value is null or empty.
        /// Returns the desiredHash value if the string entry matched the hash or no entry was found.
        /// Returns a new random hash value if the string value did not match the desired key on the found entry (hash collision)
        /// or the desired hash matched the reserved number
        /// </returns>
        public virtual ulong GetAndSetU64(ulong desiredHashKey, string value, bool checkValue = true)
        {
            if (string.IsNullOrEmpty(value))
                return this.reservedU64;

            //no hash defined, create one
            if (desiredHashKey == this.reservedU64)
                desiredHashKey = value.GetUInt64Hash();

            //is the entry present?
            if (this.stringHashDictUInt64.TryGetValue(desiredHashKey, out string valueOfDesiredHash))
            {
                //is the value equal?
                if (valueOfDesiredHash.Equals(value))
                    //kvp match -> hash is already correct!
                    return desiredHashKey;
                
                foreach (KeyValuePair<ulong,string> valuePair in this.stringHashDictUInt64)
                    if (valuePair.Value.Equals(value)) return valuePair.Key;

                //hash collision but with different values! create/ adapt a new hash value
                desiredHashKey = (ulong)(desiredHashKey * .3f) + (ulong)Random.Range(0, Int32.MaxValue);
                do
                {
                    ++desiredHashKey;
                } while (this.stringHashDictUInt64.ContainsKey(desiredHashKey));
            }
            //check if the value already has a key that doesnt match the desiredKey? 
            else if (checkValue)
            {
                foreach (KeyValuePair<ulong,string> valuePair in this.stringHashDictUInt64)
                    if (valuePair.Value.Equals(value)) return valuePair.Key;
            }
            
            //add a new kvp and return the unique hash
            this.stringHashDictUInt64.Add(desiredHashKey, value);
            return desiredHashKey;
        }
    }
}
