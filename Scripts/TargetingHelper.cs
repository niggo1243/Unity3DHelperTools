using System.Collections.Generic;
using UnityEngine;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// A helper class for Component targeting and list sorting
    /// </summary>
    public static class TargetingHelper
    {
        /// <summary>
        /// Sorts the passed <paramref name="targets"/> by distance checking against the <paramref name="checkAgainst"/> component
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="checkAgainst"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// The sorted list
        /// </returns>
        public static List<T> GetCompsSortedByDist<T>(List<T> targets, T checkAgainst) where T : Component
        {
            if (targets == null || targets.Count < 2)
                return targets;

            //sort the list by distance to the given transform
            targets.Sort((a, b) => CompareTargetsByDistanceTo(a, b, checkAgainst));

            return targets;
        }
        
        public static T GetCompAtMedianDist<T>(List<T> targets, T checkAgainst) where T : Component
        {
            if (targets == null || targets.Count < 1)
                return null;

            //half the count, ceil it up and correct it with -1 (because we count from 0)
            return GetCompsSortedByDist(targets, checkAgainst)[Mathf.CeilToInt(targets.Count * .5f) - 1];
        }

        public static T CalcAverageComp<T>(List<T> targets, T checkAgainst) where T : Component
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

        public static T GetRandomComp<T>(List<T> targets) where T : Component
        {
            //unity says its inclusive for both!
            return (targets == null || targets.Count < 1) ? null : targets[UnityEngine.Random.Range(0, targets.Count - 1)];
        }

        public static float CalcDistSquaredSum<T>(List<T> targets, T checkAgainst) where T : Component
        {
            float distSum = 0;

            foreach (T comp in targets)
            {
                distSum += NumericHelper.DistanceSquared(comp.transform.position, checkAgainst.transform.position);
            }

            return distSum;
        }

        public static float CalcAverageDistSquared<T>(List<T> targets, T checkAgainst) where T : Component
        {
            if (targets == null || targets.Count < 1)
                return 0;

            return CalcDistSquaredSum(targets, checkAgainst) / targets.Count;
        }
        
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
    }
}
