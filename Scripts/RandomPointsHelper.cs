using System;

using UnityEngine;
using UnityEngine.AI;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// A helper class to determine random points either in air or on surfaces or <see cref="NavMesh"/>es
    /// </summary>
    [Serializable]
    public class RandomPointsHelper 
    {
        /// <summary>
        /// The random distance is generated based on the minimum (x) and maximum (y) float value (both inclusive)
        /// </summary>
        [Tooltip("the random distance is generated based on the minimum (x) and maximum (y) float value (both inclusive)")]
        public Vector2 minMaxDistance = new Vector2(1, 2);

        [Range(0, 180)]
        public float minAngleFromLookDir = 0;
        
        [Range(0, 180)]
        public float maxAngleFromLookDir = 180;

        [Range(-180, 180)]
        public float shiftAngleClockwise = 0;

        public virtual Vector3 GetRandomPointInSphere()
        {
            return UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(this.minMaxDistance.x, this.minMaxDistance.y);
        }

        public virtual Vector3 GetRandomPointOnStraightLine(Vector3 originPoint, Vector3 normalizedLookDirection)
        {
            return GetRandomPointOnStraightLine(originPoint, normalizedLookDirection, minMaxDistance.x, minMaxDistance.y);
        }

        public virtual Vector3 GetRandomPointInSphereArea(Transform target)
        {
            return GetRandomPointInSphereArea(target.position, target.up, target.forward, minAngleFromLookDir, maxAngleFromLookDir, 
                minMaxDistance.x, minMaxDistance.y, shiftAngleClockwise);
        }

        #region Public Static Methods

        public static Vector3 GetRandomPointOnStraightLine(Vector3 originPoint, Vector3 normalizedLookDirection, float minDist, float maxDist)
        {
            return originPoint + (normalizedLookDirection * UnityEngine.Random.Range(minDist, maxDist));
        }
        
        public static Vector3 GetRandomPointInSphereArea(Vector3 position, Vector3 localUp, Vector3 localFwd, float minAngle, float maxAngle, 
            float minDist, float maxDist, float shiftAngleClock)
        {
            //half the angle since the rotation from up and down is max 180 -> 360/2
            Vector3 randomSphereAreaUnitPoint 
                = RandomPointsHelper.GetRandomUnitPointInSphereArea(minAngle, maxAngle);

            //Debug.DrawRay(Vector3.zero, randomSphereAreaUnitPoint, Color.black, 3f);

            //add length to the unit point
            Vector3 randomSpherePoint = 
                GetRandomPointOnStraightLine(Vector3.zero, randomSphereAreaUnitPoint, minDist, maxDist);

            //Debug.DrawRay(Vector3.zero, randomSphereAreaUnitPoint, Color.black, 3f);
            
            //Debug.DrawRay(target.position, randomSphereTransformedPoint, Color.green, 3f);

            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, localFwd);
            Quaternion shiftRotation = Quaternion.AngleAxis(shiftAngleClock, localUp);
            randomSpherePoint = shiftRotation * rotation * randomSpherePoint;
            
            Vector3 randomSphereTransformedPoint = position + randomSpherePoint;

            //Debug.DrawRay(target.position, randomSphereTransformedPoint, Color.blue, 3f);

            return randomSphereTransformedPoint;
        }
        
        public static Vector3 GetRandomUnitPointInSphereArea(float minAngle = 0, float maxAngle = 180)
        {
            float randomAngle = UnityEngine.Random.Range(minAngle, maxAngle);
            float randomAngle2Rad = Mathf.Deg2Rad * randomAngle;

            Vector3 randomRotatedVec = Vector3.RotateTowards(Vector3.up, Vector3.down, randomAngle2Rad, 0);
            Quaternion rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.up);

            randomRotatedVec = rotation * randomRotatedVec;

            return randomRotatedVec;
        }

        public static Vector3 GetRandomPointOnSurface(Vector3 airPoint, Vector3 offsetAirPoint, Vector3 shootDir, 
            float maxDist = 50, params string[] layerMasks)
        {
            layerMasks = layerMasks == null || layerMasks.Length < 1 
                ? new string[] {"Default"} 
                : layerMasks;

            int layerMaskInt = LayerMask.GetMask(layerMasks);

            //Debug.DrawRay(offsetAirPoint, airPoint - offsetAirPoint, Color.blue, 3);

            Physics.Raycast(offsetAirPoint, shootDir, out RaycastHit raycastHit, maxDist, layerMaskInt);

            //Debug.DrawRay(offsetAirPoint, shootDir * maxDist, Color.black, 3);
            
            //Debug.Log("layernames: " + layerMasks.Length +  ", layerint: " + layerMaskInt);

            if (raycastHit.collider == null)
            {
                Debug.LogWarning("missed surface");
                Debug.DrawLine(offsetAirPoint, offsetAirPoint + shootDir * maxDist, Color.black, 3);

                return airPoint;
            }
            else
            {
                //Debug.Log("hit surface, layer: " + raycastHit.collider.gameObject.layer);

                return raycastHit.point;
            }
        }

        public static Vector3 GetClosestPointOnNavmesh(Vector3 pointSomewhere, float searchRadiusOfPoint = 5, int navMeshArea = -1)
        {
            if (NavMesh.SamplePosition(pointSomewhere, out NavMeshHit navMeshHit, searchRadiusOfPoint, navMeshArea))
            {
                return navMeshHit.position;
            }

            return pointSomewhere;
        }
        #endregion
    }
}
