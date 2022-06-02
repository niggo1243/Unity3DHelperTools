using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Samples
{
    public class TargetingHelperSample : BaseNotesMono
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public List<Transform> targets = new List<Transform>();

        [Button("Log the closest child target to this transform")]
        public void LogClosestTarget()
        {
            Debug.Log("TargetingHelper closest dist target: "
                      + TargetingHelper.GetCompsSortedByDist(targets, this.transform).FirstOrDefault().name);
        }
        
        [Button("Log the median child target to this transform (by distance)")]
        public void LogMedianDistTarget()
        {
            Debug.Log("TargetingHelper median dist target: "
                      + TargetingHelper.GetCompAtMedianDist(targets, this.transform).name);
        }
        
        [Button("Log the average child target to this transform (by distance)")]
        public void LogApproxAverageDistTarget()
        {
            Debug.Log("TargetingHelper avg dist (approx) target: "
                      + TargetingHelper.GetCompAtAverageDist(targets, this.transform).name
                      + ", avg distance squared: "
                      + TargetingHelper.GetAverageDistSquared(targets, this.transform));
        }
        
        [Button("Log a random target")]
        public void LogRandomTarget()
        {
            Debug.Log("TargetingHelper random target: " + TargetingHelper.GetRandomComp(targets).name);
        }
        
    }
}
