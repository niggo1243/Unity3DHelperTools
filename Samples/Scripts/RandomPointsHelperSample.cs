using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Samples
{
    public class RandomPointsHelperSample : BaseNotesMono
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public Transform target;

        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public RandomPointsHelper randomPointsHelper = new RandomPointsHelper();

        [Button("Set target at a random point in an imaginary sphere")]
        public void SetAtRandomPointInSphere()
        {
            target.transform.localPosition = randomPointsHelper.GetRandomPointInSphere();
            LogClosestPointOnSurface();
        }

        [Button("Set target at a random point inside a sphere area (with min/max angles)")]
        public void SetAtRandomPointInSphereArea()
        {
            target.position = randomPointsHelper.GetRandomPointInSphereArea(this.transform);
            LogClosestPointOnSurface();
        }

        [Button("Set target at a random position on an imaginary straight line")]
        public void SetAtRandomPointOnStraightLine()
        {
            target.position = randomPointsHelper.GetRandomPointOnStraightLine(this.transform.position, 
                this.transform.forward);
            LogClosestPointOnSurface();
        }

        [Button("Log a random unit sphere point")]
        public void LogRandomPointInUnitSphereArea()
        {
            Debug.Log("Random unit sphere point: " 
                      + RandomPointsHelper.GetRandomUnitPointInSphereArea(randomPointsHelper.minAngleFromLookDir, randomPointsHelper.maxAngleFromLookDir));
        }

        [Button("Log the next closest point on a surface relative to the target")]
        public void LogClosestPointOnSurface()
        {
            Debug.Log("Closest surface point: " + RandomPointsHelper.GetClosestPointOnSurface(target.position, Vector3.down, 100));
        }
    }
}
