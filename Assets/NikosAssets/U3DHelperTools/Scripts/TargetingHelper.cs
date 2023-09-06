using System;
using System.Collections.Generic;
using UnityEngine;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// A helper class for <see cref="Component"/> targeting and list sorting by distance measures
    /// </summary>
    public static class TargetingHelper
    {
        /// <summary>
        /// Sorts the passed <paramref name="targets"/> by distance checking against the <paramref name="checkAgainst"/> <see cref="Component"/>.
        /// If you dont want to sort the original list, make a copy of it.
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="checkAgainst"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// The sorted (same) list by distance to <paramref name="checkAgainst"/>.
        /// The same list, if only 1 element is found in the list or the list is null.
        /// </returns>
        public static List<T> GetCompsSortedByDist<T>(List<T> targets, T checkAgainst) where T : Component
        {
            if (targets == null || targets.Count < 2)
                return targets;

            //sort the list by distance to the given transform
            targets.Sort((a, b) => CompareTargetsByDistanceTo(a, b, checkAgainst));

            return targets;
        }

        /// <summary>
        /// Get the <see cref="Component"/> that is closest to the median distance to <paramref name="checkAgainst"/>.
        /// If you dont want to sort the original list, make a copy of it.
        /// </summary>
        /// <param name="targets">
        /// The <see cref="Component"/> to pick from
        /// </param>
        /// <param name="checkAgainst">
        /// The <see cref="Component"/> to check the <paramref name="targets"/> against
        /// </param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// Null if the list is empty or null, otherwise
        /// the <see cref="Component"/> at median distance to <paramref name="checkAgainst"/>
        /// </returns>
        public static T GetCompAtMedianDist<T>(List<T> targets, T checkAgainst) where T : Component
        {
            if (targets == null || targets.Count < 1)
                return null;

            //half the count, ceil it up and correct it with -1 (because we count from 0)
            return GetCompsSortedByDist(targets, checkAgainst)[Mathf.CeilToInt(targets.Count * .5f) - 1];
        }

        /// <summary>
        /// Get the <see cref="Component"/> that is approx at the average distance to <paramref name="checkAgainst"/> relative to the other
        /// <paramref name="targets"/>.
        /// The given list will not be sorted and remains unchanged.
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="checkAgainst"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// Null if the list is null or empty, otherwise a <see cref="Component"/>
        /// </returns>
        public static T GetCompAtAverageDist<T>(List<T> targets, T checkAgainst) where T : Component
        {
            if (targets == null || targets.Count < 1)
                return null;

            float sumSquared = 0;
            float avgSumSquared = 0;
            float leastAvgOffset = 0;
            
            T avgTarget = null;
            //temp storage for distance to targets to save calculations in the second iteration
            Dictionary<float, T> distDict = new Dictionary<float, T>();

            //collect the distance of every transform in relation to the entity to check against
            foreach (T comp in targets)
            {
                float distanceSquared = NumericHelper.DistanceSquared(comp.transform.position, checkAgainst.transform.position);
                //add the sum
                sumSquared += distanceSquared;
                
                if (!distDict.ContainsKey(distanceSquared))
                    distDict.Add(distanceSquared, comp);
            }
            //calc the avg distance
            avgSumSquared = sumSquared / targets.Count;
            
            //find the closest target to the average distance
            foreach (KeyValuePair<float, T> pair in distDict)
            {
                float offset = Mathf.Abs(pair.Key - avgSumSquared);
                //the least offset = the closest
                if (avgTarget == null || offset < leastAvgOffset)
                {
                    avgTarget = pair.Value;
                    leastAvgOffset = offset;
                }
            }

            return avgTarget;
        }

        /// <summary>
        /// Get a random <see cref="Component"/> from the given list
        /// </summary>
        /// <param name="targets"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// Null if list is empty or null, otherwise a random <see cref="Component"/>
        /// </returns>
        public static T GetRandomComp<T>(List<T> targets) where T : Component
        {
            //unity says its inclusive for both!
            return (targets == null || targets.Count < 1) ? null : targets[UnityEngine.Random.Range(0, targets.Count - 1)];
        }

        /// <summary>
        /// Get the summed squared distance of the <paramref name="targets"/> to the <paramref name="checkAgainst"/> <see cref="Component"/>
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="checkAgainst"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The summed squared distance float value</returns>
        public static float GetDistSquaredSum<T>(List<T> targets, T checkAgainst) where T : Component
        {
            float distSum = 0;

            foreach (T comp in targets)
            {
                distSum += NumericHelper.DistanceSquared(comp.transform.position, checkAgainst.transform.position);
            }

            return distSum;
        }

        /// <summary>
        /// Get the average squared distance of the <paramref name="targets"/> to the <paramref name="checkAgainst"/> <see cref="Component"/> 
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="checkAgainst"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>0 if list is empty or null, otherwise the average squared distance</returns>
        public static float GetAverageDistSquared<T>(List<T> targets, T checkAgainst) where T : Component
        {
            if (targets == null || targets.Count < 1)
                return 0;

            return GetDistSquaredSum(targets, checkAgainst) / targets.Count;
        }
        
        /// <summary>
        /// A helper method to compare 2 <see cref="Component"/>s against another one based on their distance to it
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="checkAgainst"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int CompareTargetsByDistanceTo<T>(T a, T b, T checkAgainst) where T : Component
        {
            Vector3 position = checkAgainst.transform.position;
                
            float distSqCompA = NumericHelper.DistanceSquared(a.transform.position, position);
            float distSqCompB = NumericHelper.DistanceSquared(b.transform.position, position);

            if (distSqCompA > distSqCompB)
                return 1;

            if (distSqCompA < distSqCompB)
                return -1;

            return 0;
        }

        #region Obsolete Methods
        #endregion
    }
}
