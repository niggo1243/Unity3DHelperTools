using System.Collections.Generic;
using System;

using UnityEngine;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// </summary>
    /// <typeparam name="ComponentTypeTarget"></typeparam>
    /// <typeparam name="ComponentTypeCheckAgainst"></typeparam>
    [Serializable]
    public class TargetingHelper
    {
        public enum TargetingType
        {
            First = 0,
            Last = 1,
            Closest = 2,
            Furthest = 3,
            
            Random = 4,

            Median = 5,
            Average = 6,

            Custom = 7
        }

        [SerializeField]
        protected TargetingType _targetingType;

        public TargetingType CurrentTargetingType
        {
            get => _targetingType;
            set
            {
                _targetingType = value;

                //update the func
                this.Init();
            }
        }

        public Func<List<Transform>, Transform, Vector3> CalcTargetPosition;
        public Func<List<Transform>, Transform, Transform> CalcTarget;

        public virtual void Init()
        {
            switch (_targetingType)
            {
                case TargetingType.First:

                    this.CalcTarget = delegate (List<Transform> targets, Transform checkAgainst)
                    {
                        if (targets.Count < 1)
                            return null;

                        return targets[0];
                    };

                    this.CalcTargetPosition = delegate (List<Transform> targets, Transform checkAgainst)
                    {
                        if (targets.Count < 1)
                            return Vector3.zero;

                        return targets[0].position;
                    };

                    break;

                case TargetingType.Last:

                    this.CalcTarget = delegate (List<Transform> targets, Transform checkAgainst)
                    {
                        if (targets.Count < 1)
                            return null;

                        return targets[targets.Count - 1];
                    };

                    this.CalcTargetPosition = delegate (List<Transform> targets, Transform checkAgainst)
                    {
                        if (targets.Count < 1)
                            return Vector3.zero;

                        return targets[targets.Count - 1].position;
                    };

                    break;

                case TargetingType.Random:

                    this.CalcTarget = GetRandomTransform;
                    this.CalcTargetPosition = GetRandomPosition;

                    break;

                case TargetingType.Median:

                    this.CalcTarget = GetTransformAtMedianDist;
                    this.CalcTargetPosition = CalcMedianPosition;

                    break;

                case TargetingType.Average:

                    this.CalcTarget = CalcAverageTransform;
                    this.CalcTargetPosition = CalcAveragePosition;

                    break;

                case TargetingType.Closest:

                    this.CalcTarget = delegate (List<Transform> targets, Transform checkAgainst)
                    {
                        if (targets.Count < 1)
                            return null;
                        
                        return GetTransformsSortedByDist(targets, checkAgainst)[0];
                    };

                    this.CalcTargetPosition = delegate (List<Transform> targets, Transform checkAgainst)
                    {
                        if (targets.Count < 1)
                            return Vector3.zero;

                        return GetTransformsSortedByDist(targets, checkAgainst)[0].position;
                    };

                    break;

                case TargetingType.Furthest:

                    this.CalcTarget = delegate (List<Transform> targets, Transform checkAgainst)
                    {
                        if (targets.Count < 1)
                            return null;

                        return GetTransformsSortedByDist(targets, checkAgainst)[targets.Count - 1];
                    };

                    this.CalcTargetPosition = delegate (List<Transform> targets, Transform checkAgainst)
                    {
                        if (targets.Count < 1)
                            return Vector3.zero;

                        return GetTransformsSortedByDist(targets, checkAgainst)[targets.Count - 1].position;
                    };

                    break;
            }
        }

        public static Vector3 CalcMedianPosition(List<Transform> targets, Transform checkAgainst)
        {
            return GetTransformAtMedianDist(targets, checkAgainst).position;
        }

        public static Transform GetTransformAtMedianDist (List<Transform> targets, Transform checkAgainst)
        {
            if (targets.Count < 1)
                return null;

            //half the count, ceil it up and correct it with -1 (because we count from 0)
            return GetTransformsSortedByDist(targets, checkAgainst)[Mathf.CeilToInt(targets.Count * .5f) - 1];
        }

        public static float CalcDistSquaredSum(List<Transform> targets, Transform checkAgainst)
        {
            float distSum = 0;

            foreach (Transform transform in targets)
            {
                distSum += NumericHelper.DistanceSquared(transform.position, checkAgainst.position);
            }

            return distSum;
        }

        public static float CalcAverageDistSquared(List<Transform> targets, Transform checkAgainst)
        {
            if (targets.Count < 1)
                return 0;

            return CalcDistSquaredSum(targets, checkAgainst) / targets.Count;
        }

        public static Transform CalcAverageTransform(List<Transform> targets, Transform checkAgainst)
        {
            if (targets.Count < 1)
                return null;

            float sumSquared = 0;
            float avgSumSquared = 0;
            float leastAvgOffset = 0;
            
            Transform avgTarget = null;
            //temp storage for distance to targets to save calculations in the second iteration
            Dictionary<float, Transform> distDict = new Dictionary<float, Transform>();

            //collect the distance of every transform in relation to the entity to check against
            foreach (Transform transform in targets)
            {
                float distanceSquared = NumericHelper.DistanceSquared(transform.position, checkAgainst.position);
                //add the sum
                sumSquared += distanceSquared;
                
                if (!distDict.ContainsKey(distanceSquared))
                    distDict.Add(distanceSquared, transform);
            }
            //calc the avg distance
            avgSumSquared = sumSquared / targets.Count;
            
            //find the closest target to the average distance
            foreach (KeyValuePair<float, Transform> pair in distDict)
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
        
        public static Vector3 CalcAveragePosition(List<Transform> targets, Transform checkAgainst)
        {
            return CalcAverageTransform(targets, checkAgainst).position;
        }

        public static Transform GetRandomTransform(List<Transform> targets, Transform checkAgainst)
        {
            //unity says its inclusive for both!
            return targets.Count < 1 ? null : targets[UnityEngine.Random.Range(0, targets.Count - 1)];
        }

        public static Vector3 GetRandomPosition(List<Transform> targets, Transform checkAgainst)
        {
            if (targets.Count < 1)
                return Vector3.zero;

            return GetRandomTransform(targets, checkAgainst).position;
        }

        public static List<Transform> GetTransformsSortedByDist(List<Transform> targets, Transform checkAgainst)
        {
            if (targets.Count < 2)
                return targets;

            //dont sort the original list
            List<Transform> transforms = new List<Transform>(targets);
            
            //sort the list by distance to the given transform
            transforms.Sort((transformA, transformB) =>
            {
                Vector3 position = checkAgainst.position;
                
                float distSqCompA = NumericHelper.DistanceSquared(transformA.position, position);
                float distSqCompB = NumericHelper.DistanceSquared(transformB.position, position);

                if (distSqCompA > distSqCompB)
                    return 1;

                if (distSqCompA < distSqCompB)
                    return -1;

                return 0;
            });

            return transforms;
        }
    }
}
