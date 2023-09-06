using System;

using UnityEngine;
using UnityEngine.AI;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// A helper class to determine random points in the 3D space
    /// </summary>
    [Serializable]
    public class RandomPointsHelper 
    {
        /// <summary>
        /// The random distance is generated based on the minimum (x) and maximum (y) float value (both inclusive)
        /// </summary>
        [Tooltip("the random distance is generated based on the minimum (x) and maximum (y) float value (both inclusive)")]
        public Vector2 minMaxDistance = new Vector2(1, 2);

        /// <summary>
        /// Used in the <see cref="GetRandomPointInSphereArea(UnityEngine.Transform)"/> method to determine the minimum spawn angle
        /// from the <see cref="Transform"/>'s look dir
        /// </summary>
        [Tooltip("Used in the GetRandomPointInSphereArea() method to determine the minimum spawn angle from the transform's look dir")]
        [Range(0, 180)]
        public float minAngleFromLookDir = 0;
        
        /// <summary>
        /// Used in the <see cref="GetRandomPointInSphereArea(UnityEngine.Transform)"/> method to determine the maximum spawn angle
        /// from the <see cref="Transform"/>'s look dir
        /// </summary>
        [Tooltip("Used in the GetRandomPointInSphereArea() method to determine the maximum spawn angle from the transform's look dir")]
        [Range(0, 180)]
        public float maxAngleFromLookDir = 180;

        /// <summary>
        /// Used in the <see cref="GetRandomPointInSphereArea(UnityEngine.Transform)"/> method to shift the spawn point relative to the
        /// <see cref="Transform"/> clockwise
        /// </summary>
        [Tooltip("Used in the GetRandomPointInSphereArea() method to shift the spawn point relative to the transform clockwise")]
        [Range(-180, 180)]
        public float shiftAngleClockwise = 0;

        /// <summary>
        /// Get a random point inside a unit sphere multiplied by and within bounds of <see cref="minMaxDistance"/>
        /// </summary>
        /// <returns>
        /// A random <see cref="Vector3"/> point
        /// </returns>
        public virtual Vector3 GetRandomPointInSphere()
        {
            return UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(this.minMaxDistance.x, this.minMaxDistance.y);
        }
        
        /// <summary>
        /// Get a random point inside a limited sphere area based on <see cref="minAngleFromLookDir"/>, <see cref="maxAngleFromLookDir"/>,
        /// <see cref="minMaxDistance"/> and <see cref="shiftAngleClockwise"/> for the given "<paramref name="target"/>s" position and orientation
        /// </summary>
        /// <param name="target"></param>
        /// <returns>A random <see cref="Vector3"/> point</returns>
        public virtual Vector3 GetRandomPointInSphereArea(Transform target)
        {
            return GetRandomPointInSphereArea(target.position, target.up, target.forward, minAngleFromLookDir, maxAngleFromLookDir, 
                minMaxDistance.x, minMaxDistance.y, shiftAngleClockwise);
        }

        /// <summary>
        /// Get a random <see cref="Vector3"/> point on a straight line based on the "<paramref name="originPoint"/>" and
        /// the "<paramref name="normalizedLookDirection"/>" multiplied by and in bounds of <see cref="minMaxDistance"/> 
        /// </summary>
        /// <param name="originPoint"></param>
        /// <param name="normalizedLookDirection"></param>
        /// <returns>A random <see cref="Vector3"/> point</returns>
        public virtual Vector3 GetRandomPointOnStraightLine(Vector3 originPoint, Vector3 normalizedLookDirection)
        {
            return GetRandomPointOnStraightLine(originPoint, normalizedLookDirection, minMaxDistance.x, minMaxDistance.y);
        }

        #region Public Static Methods

        /// <summary>
        /// Get a random <see cref="Vector3"/> point on a straight line based on the "<paramref name="originPoint"/>" and
        /// the "<paramref name="normalizedLookDirection"/>" multiplied by and in bounds of "<paramref name="minDist"/>" and "<paramref name="maxDist"/>"
        /// </summary>
        /// <param name="originPoint"></param>
        /// <param name="normalizedLookDirection"></param>
        /// <param name="minDist"></param>
        /// <param name="maxDist"></param>
        /// <returns>A random <see cref="Vector3"/> point</returns>
        public static Vector3 GetRandomPointOnStraightLine(Vector3 originPoint, Vector3 normalizedLookDirection, float minDist, float maxDist)
        {
            return originPoint + (normalizedLookDirection * UnityEngine.Random.Range(minDist, maxDist));
        }
        
        /// <summary>
        /// Get a random point inside a limited sphere area based on the given params for the given position and local orientations.
        /// Look into the non-static <see cref="GetRandomPointInSphereArea(UnityEngine.Transform)"/> method to understand the required params of this method.
        /// </summary>
        /// <param name="position">
        /// The starting position
        /// </param>
        /// <param name="localUp">
        /// The targets normalized local up vector
        /// </param>
        /// <param name="localFwd">
        /// The targets normalized local fwd vector
        /// </param>
        /// <param name="minAngle">
        /// The minimum spawn-point angle of the imaginary sphere
        /// </param>
        /// <param name="maxAngle">
        /// The maximum spawn-point angle of the imaginary sphere
        /// </param>
        /// <param name="minDist">
        /// The minimum spawn distance radius
        /// </param>
        /// <param name="maxDist">
        /// The maximum spawn distance radius
        /// </param>
        /// <param name="shiftAngleClock">
        /// Shift the spawn point clockwise
        /// </param>
        /// <returns>A random <see cref="Vector3"/> point</returns>
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
        
        /// <summary>
        /// Spawn a random point inside a unit sphere without any position or orientation context
        /// </summary>
        /// <param name="minAngle">
        /// </param>
        /// <param name="maxAngle"></param>
        /// <returns>A random <see cref="Vector3"/> point without context</returns>
        public static Vector3 GetRandomUnitPointInSphereArea(float minAngle = 0, float maxAngle = 180)
        {
            float randomAngle = UnityEngine.Random.Range(minAngle, maxAngle);
            float randomAngle2Rad = Mathf.Deg2Rad * randomAngle;

            Vector3 randomRotatedVec = Vector3.RotateTowards(Vector3.up, Vector3.down, randomAngle2Rad, 0);
            Quaternion rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.up);

            randomRotatedVec = rotation * randomRotatedVec;

            return randomRotatedVec;
        }

        /// <summary>
        /// Try to get the closest point on a surface with the accepted "<paramref name="layerMasks"/>" based
        /// on the starting "<paramref name="originalAndShootRayPoint"/>", the "<paramref name="shootDir"/>" and the "<paramref name="maxDist"/>"
        /// </summary>
        /// <param name="originalAndShootRayPoint">
        /// The original point and the point of the raycast that will search for a surface
        /// </param>
        /// <param name="shootDir">
        /// Try to hit a surface based on this dir
        /// </param>
        /// <param name="maxDist">
        /// How far should we shoot?
        /// </param>
        /// <param name="layerMasks">
        /// What colliders are accepted?
        /// </param>
        /// <returns>
        /// If no surface found, the "<paramref name="originalAndShootRayPoint"/>", otherwise a surface hit <see cref="Vector3"/> point
        /// </returns>
        public static Vector3 GetClosestPointOnSurface(Vector3 originalAndShootRayPoint, Vector3 shootDir,
            float maxDist = 50, params string[] layerMasks)
        {
            layerMasks = layerMasks == null || layerMasks.Length < 1
                ? new string[] {"Default"}
                : layerMasks;

            int layerMaskInt = LayerMask.GetMask(layerMasks);
            Physics.Raycast(originalAndShootRayPoint, shootDir, out RaycastHit raycastHit, maxDist, layerMaskInt);

            if (raycastHit.collider == null)
            {
                Debug.LogWarning("missed surface");
                Debug.DrawLine(originalAndShootRayPoint, originalAndShootRayPoint + shootDir * maxDist, Color.black, 3);

                return originalAndShootRayPoint;
            }

            return raycastHit.point;
        }

        /// <summary>
        /// Try to get the closest point on a surface with the accepted "<paramref name="layerMasks"/>" based
        /// on the starting "<paramref name="shootRayPoint"/>", the "<paramref name="shootDir"/>" and the "<paramref name="maxDist"/>"
        /// </summary>
        /// <param name="originalPoint">
        /// The original point
        /// </param>
        /// <param name="shootRayPoint">
        /// The starting point of the raycast that will search for a surface
        /// </param>
        /// <param name="shootDir">
        /// Try to hit a surface based on this dir
        /// </param>
        /// <param name="maxDist">
        /// How far should we shoot?
        /// </param>
        /// <param name="layerMasks">
        /// What collider layers are accepted?
        /// </param>
        /// <returns>
        /// If no surface found, the "<paramref name="originalPoint"/>", otherwise a surface hit <see cref="Vector3"/> point
        /// </returns>
        public static Vector3 GetRandomPointOnSurface(Vector3 originalPoint, Vector3 shootRayPoint, Vector3 shootDir, 
            float maxDist = 50, params string[] layerMasks)
        {
            layerMasks = layerMasks == null || layerMasks.Length < 1 
                ? new string[] {"Default"} 
                : layerMasks;

            int layerMaskInt = LayerMask.GetMask(layerMasks);

            //Debug.DrawRay(offsetAirPoint, airPoint - offsetAirPoint, Color.blue, 3);

            Physics.Raycast(shootRayPoint, shootDir, out RaycastHit raycastHit, maxDist, layerMaskInt);

            //Debug.DrawRay(offsetAirPoint, shootDir * maxDist, Color.black, 3);
            
            //Debug.Log("layernames: " + layerMasks.Length +  ", layerint: " + layerMaskInt);

            if (raycastHit.collider == null)
            {
                Debug.LogWarning("missed surface");
                Debug.DrawLine(shootRayPoint, shootRayPoint + shootDir * maxDist, Color.black, 3);

                return originalPoint;
            }
            else
            {
                //Debug.Log("hit surface, layer: " + raycastHit.collider.gameObject.layer);

                return raycastHit.point;
            }
        }
        
        #endregion

        #region Obsolete Methods
        #endregion
    }
}
