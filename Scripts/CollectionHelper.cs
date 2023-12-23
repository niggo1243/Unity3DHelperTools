using System.Collections;
using System.Collections.Generic;
using NikosAssets.Helpers.Interfaces;
using UnityEngine;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// Helps with <see cref="ICollection"/>s and <see cref="List{T}"/>s
    /// </summary>
    public static class CollectionHelper
    {
        /// <summary>
        /// An enum for collection matching checks (<see cref="CollectionHelper.CollectionsMatcher"/>)
        /// </summary>
        public enum ItemMatching
        {
            MatchNone = 0,
            MatchAtLeastOne = 1,
            MatchAll = 2,
            MatchAllIncludingAmount = 3
        }
        
        /// <summary>
        /// Randomizes the item order of the given "<paramref name="collection"/>"
        /// </summary>
        /// <param name="collection">Shuffle this</param>
        /// <typeparam name="T">The item type of the "<paramref name="collection"/>"</typeparam>
        public static void ShuffleList<T>(List<T> collection)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                T temp = collection[i];
                int randomIndex = UnityEngine.Random.Range(i, collection.Count);
                collection[i] = collection[randomIndex];
                collection[randomIndex] = temp;
            }
        }

        /// <summary>
        /// Checks if the given <see cref="ICollection{T}"/>s match based on the desired "<paramref name="matching"/>" setup,
        /// with "<paramref name="colA"/>" being the main collection to check (outer loop).
        /// </summary>
        /// <param name="matching"></param>
        /// <param name="colA"></param>
        /// <param name="colB"></param>
        /// <returns>
        /// true if matched successfully, otherwise false
        /// </returns>
        public static bool CollectionsMatcher(ItemMatching matching, ICollection colA, ICollection colB)
        {
            bool success = colA.Count == 0 && colB.Count == 0;

            switch (matching)
            {
                case ItemMatching.MatchAtLeastOne:
                    foreach (object objA in colA)
                    {
                        foreach (object objB in colB)
                        {
                            if (objA.Equals(objB)) return true;
                        }
                    }
                    break;
                
                case ItemMatching.MatchAll:
                    success = CollectionsMatchAllItems(colA, colB);
                    break;
                
                case ItemMatching.MatchNone:
                    success = true;
                    foreach (object objA in colA)
                    {
                        foreach (object objB in colB)
                        {
                            if (objA.Equals(objB)) return false;
                        }
                    }
                    break;
                
                case ItemMatching.MatchAllIncludingAmount:
                    if (colA.Count != colB.Count) return false;
                    success = CollectionsMatchAllItems(colA, colB);
                    break;
            }

            return success;
        }

        /// <summary>
        /// Checks if the given <see cref="ICollection"/> elements all match against each other
        /// </summary>
        /// <param name="colA"></param>
        /// <param name="colB"></param>
        /// <returns>
        /// true if matched successfully, otherwise false
        /// </returns>
        public static bool CollectionsMatchAllItems(ICollection colA, ICollection colB)
        {
            bool success = colA.Count == 0 && colB.Count == 0;

            foreach (object objA in colA)
            {
                success = false;
                        
                foreach (object objB in colB)
                {
                    if (objA.Equals(objB))
                    {
                        success = true;
                        break;
                    }
                }

                //stop the check
                if (!success) return false;
            }
            
            return success;
        }
        
        /// <summary>
        /// Create a list with only 1 (the given) element in it
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>A list with count = 1</returns>
        public static List<T> ListOfOne<T>(T item)
        {
            return new List<T> { item };
        }

        /// <summary>
        /// Increment or decrement (and cycle around) correctly respecting the length of a collection
        /// </summary>
        /// <param name="increment">increment, otherwise decrement</param>
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
        /// Iterates through the "<paramref name="collection"/>" and calls ToString() on each item or logs "null" if item was null
        /// </summary>
        /// <param name="collection"></param>
        /// <typeparam name="T"></typeparam>
        public static void LogCollection<T>(ICollection<T> collection)
        {
            foreach (T t in collection)
            {
                if (t == null)
                    Debug.Log("null");
                else
                    Debug.Log(t.ToString());
            }
        }
        
        /// <summary>
        /// Checks if the given <see cref="ICollection"/> is null or empty
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>true if "<paramref name="collection"/>" is null or empty, otherwise false</returns>
        public static bool CollectionIsNullOrEmpty(ICollection collection)
        {
            return collection == null || collection.Count <= 0;
        }

        /// <summary>
        /// Checks if the given index (<paramref name="i"/>) is in bounds of the given <see cref="ICollection"/> (<paramref name="collection"/>)
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="i"></param>
        /// <param name="logError">
        /// Log index out of bounds error?
        /// </param>
        /// <returns>
        /// true, if "<paramref name="i"/>" is in "<paramref name="collection"/>"
        /// </returns>
        public static bool CollectionAndIndexChecker(ICollection collection, int i, bool logError = false)
        {
            if (collection == null)
            {
                if (logError)
                    Debug.LogError("The given Collection is null");

                return false;
            }

            if (i < 0 || i >= collection.Count || collection.Count <= 0)
            {
                if (logError)
                    Debug.LogError("Invalid index: " + i + " or the collection is empty: " + collection.Count);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Doesn't throw any Exceptions if the list is null or empty and if the index is out of bounds
        /// </summary>
        /// <param name="list"></param>
        /// <param name="i"></param>
        /// <param name="logError">
        /// Should we at least log the errors?
        /// </param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// default value if the list is null or empty
        /// </returns>
        public static T GetListItemAtIndex<T>(List<T> list, int i, bool logError = false)
        {
            if (CollectionAndIndexChecker(list, i, logError))
            {
                return list[i];
            }

            return default(T);
        }

        /// <summary>
        /// Doesn't throw any Exceptions if the queue is null or empty and if the index is out of bounds
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="i"></param>
        /// <param name="logError">
        /// Should we at least log the errors?
        /// </param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// default value if the queue is null or empty
        /// </returns>
        public static T GetQueueItemAtIndex<T> (Queue<T> queue, int i, bool logError = false)
        {
            if (CollectionAndIndexChecker(queue, i, logError))
            {
                T[] arrayT = queue.ToArray();

                return arrayT[i];
            }

            return default(T);
        }

        /// <summary>
        /// Get a random winner for every <typeparamref name="ChanceType"/> list item of the given "<paramref name="list"/>"" that has won its
        /// <see cref="Interfaces.IChance"/>.<see cref="Interfaces.IChance.Chance"/>
        /// </summary>
        /// <param name="list">Contains <typeparamref name="ChanceType"/> items that must implement the <see cref="Interfaces.IChance"/> interface</param>
        /// <typeparam name="ChanceType">Must implement the <see cref="Interfaces.IChance"/> interface</typeparam>
        /// <returns>A random <typeparamref name="ChanceType"/> otherwise default if no winner is found</returns>
        public static ChanceType GetRandomChanceWinnerFromList<ChanceType>(List<ChanceType> list) where ChanceType : IChance
        {
            List<ChanceType> tempList = list.FindAll(item => NumericHelper.RandomChanceSuccess01(item.Chance));
            if (tempList.Count < 1) return default(ChanceType);

            return tempList[Random.Range(0, tempList.Count)];
        }
        
        /// <summary>
        /// Get a random list item
        /// </summary>
        /// <param name="list">A list with any itemtype</param>
        /// <typeparam name="T">Some item</typeparam>
        /// <returns>A random list item, otherwise default(<typeparamref name="T"/>)</returns>
        public static T GetRandomWinnerFromList<T>(List<T> list)
        {
            if (list.Count < 1) return default(T);
            return list[Random.Range(0, list.Count)];
        }
    }
}
