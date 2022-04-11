using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// Helps with <see cref="ICollection"/>s and <see cref="List{T}"/>s
    /// </summary>
    public class CollectionHelper : MonoBehaviour
    {
        public enum ItemMatching
        {
            MatchNone = 0,
            MatchAtLeastOne = 1,
            MatchAll = 2,
            MatchAllIncludingAmount = 3
        }
        
        public static bool CollectionIsNullOrEmpty(ICollection collection)
        {
            return collection == null || collection.Count <= 0;
        }

        /// <summary>
        /// Checks if the given index is in bounds of the given <see cref="ICollection"/>
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="i"></param>
        /// <param name="logErrorOnInvalidIndex">
        /// Log index out of bounds error?
        /// </param>
        /// <returns>
        /// true, if <paramref name="i"/> is in <paramref name="collection"/>
        /// </returns>
        public static bool CollectionAndIndexChecker(ICollection collection, int i, bool logErrorOnInvalidIndex = false)
        {
            if (collection == null)
            {
                Debug.LogError("The given Collection is null");

                return false;
            }

            if (i < 0 || i >= collection.Count || collection.Count <= 0)
            {
                if (logErrorOnInvalidIndex)
                    Debug.LogError("Invalid index: " + i + " or the collection is empty: " + collection.Count);

                return false;
            }

            return true;
        }

        public static T GetListItemAtIndex<T>(List<T> list, int i)
        {
            if (CollectionAndIndexChecker(list, i, true))
            {
                return list[i];
            }

            return default(T);
        }

        public static T GetQueueItemAtIndex<T> (Queue<T> queue, int i)
        {
            if (CollectionAndIndexChecker(queue, i, true))
            {
                T[] arrayT = queue.ToArray();

                return arrayT[i];
            }

            return default(T);
        }

        public static List<T> ListOfOne<T>(T item)
        {
            return new List<T> { item };
        }

        /// <summary>
        /// Increment or decrement (and cycle around) correctly respecting the length of a collection
        /// </summary>
        /// <param name="increment">increment otherwise decrement</param>
        /// <param name="pointer">the current index to increase or decrease</param>
        /// <param name="lengthOfCollection">the length to loop around</param>
        /// <returns>
        /// The correct index, potentially looped around
        /// <example>
        /// <paramref name="pointer"/> = 2;
        /// <paramref name="lengthOfCollection"/> = 3;
        /// <paramref name="increment"/> = true;
        /// result = 0
        /// </example>
        /// </returns>
        public static int PointerHandler(bool increment, int pointer, int lengthOfCollection)
        {
            if (increment && ++pointer >= lengthOfCollection)
            {
                pointer = 0;
            }
            else if (!increment && --pointer < 0)
            {
                if (lengthOfCollection - 1 < 0)
                {
                    pointer = 0;
                }
                else
                {
                    pointer = lengthOfCollection - 1;
                }
            }

            return pointer;
        }

        /// <summary>
        /// Iterates through the <paramref name="collection"/> and calls ToString() on each item or logs "null" if item was null
        /// </summary>
        /// <param name="collection"></param>
        /// <typeparam name="T"></typeparam>
        public static void LogCollection<T>(Collection<T> collection) where T : UnityEngine.Object
        {
            foreach (T t in collection)
            {
                if (t == null)
                    Debug.Log("null");
                else
                    Debug.Log(t.ToString());
            }
        }
    }
}
