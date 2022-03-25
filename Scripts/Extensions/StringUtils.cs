using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NikosAssets.Helpers.Extensions
{
    public static class StringUtils
    {
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

        public static string CropString(this string text, int maxLength = 40, string cropSymbols = "...")
        {
            if (maxLength >= text.Length)
                return text;
            
            return text.Substring(0, maxLength) + cropSymbols;
        }
    }
}
